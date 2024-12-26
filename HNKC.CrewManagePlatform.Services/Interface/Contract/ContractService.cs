using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Dtos.Contract;
using HNKC.CrewManagePlatform.Models.Enums;
using HNKC.CrewManagePlatform.SqlSugars.Models;
using HNKC.CrewManagePlatform.Utils;
using SqlSugar;
using System.Globalization;
using UtilsSharp;

namespace HNKC.CrewManagePlatform.Services.Interface.Contract
{
    /// <summary>
    /// 合同实现层
    /// </summary>
    public class ContractService : IContractService
    {
        private readonly ISqlSugarClient _dbContext;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        public ContractService(ISqlSugarClient dbContext)
        {
            this._dbContext = dbContext;
        }
        #region 合同列表
        /// <summary>
        /// 合同列表
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        public async Task<PageResult<ContractSearch>> SearchContractAsync(ContractRequest requestBody)
        {
            RefAsync<int> total = 0;
            #region 船员关联
            var uentityFist = _dbContext.Queryable<UserEntryInfo>()
                .GroupBy(u => u.UserEntryId)
                .Select(x => new { x.UserEntryId, EndTime = SqlFunc.AggregateMax(x.EndTime) });
            var uentity = _dbContext.Queryable<UserEntryInfo>()
                .InnerJoin(uentityFist, (x, y) => x.UserEntryId == y.UserEntryId && x.EndTime == y.EndTime);
            var crewWorkShip = _dbContext.Queryable<WorkShip>()
            .GroupBy(u => u.WorkShipId)
            .Select(t => new { t.WorkShipId, WorkShipEndTime = SqlFunc.AggregateMax(t.WorkShipEndTime) });
            var wShip = _dbContext.Queryable<WorkShip>()
                .InnerJoin(crewWorkShip, (x, y) => x.WorkShipId == y.WorkShipId && x.WorkShipEndTime == y.WorkShipEndTime);
            #endregion

            var rr = await _dbContext.Queryable<User>()
                .Where(t1 => t1.IsLoginUser == 1)
                .WhereIF(!string.IsNullOrEmpty(requestBody.KeyWords), t1 => requestBody.KeyWords.Contains(t1.Name) || requestBody.KeyWords.Contains(t1.Phone) || requestBody.KeyWords.Contains(t1.WorkNumber) || requestBody.KeyWords.Contains(t1.CardId))
                .LeftJoin(uentity, (t1, t2) => t1.BusinessId == t2.UserEntryId)
                .InnerJoin<OwnerShip>((t1, t2, t3) => t1.OnBoard == t3.BusinessId.ToString())
                .InnerJoin<CertificateOfCompetency>((t1, t2, t3, t4) => t1.BusinessId == t4.CertificateId)
                .InnerJoin(wShip, (t1, t2, t3, t4, t5) => t1.BusinessId == t5.WorkShipId)
                .WhereIF(!string.IsNullOrEmpty(requestBody.EmploymentType), (t1, t2, t3, t4, t5) => requestBody.EmploymentType == t2.EmploymentId)
                .Select((t1, t2, t3, t4, t5) => new ContractSearch
                {
                    BId = t1.BusinessId.ToString(),
                    Id = t2.BusinessId.ToString(),
                    Country = t3.Country,
                    OnBoard = t1.OnBoard,
                    ShipType = t3.ShipType,
                    UserName = t1.Name,
                    WorkNumber = t1.WorkNumber,
                    EndTime = t2.EndTime.ToString("yyyy/MM/dd"),
                    StartTime = t2.StartTime.ToString("yyyy/MM/dd"),
                    ContractMain = t2.ContractMain,
                    ContractType = t2.ContractType,
                    EmploymentType = t2.EmploymentId,
                    LaborCompany = t2.LaborCompany,
                    CardId = t1.CardId,
                    FPosition = t4.FPosition,
                    SPosition = t4.SPosition,
                    OnBoardPosition = t5.Postition,
                    WorkShipStartTime = t5.WorkShipStartTime,
                    DeleteResonEnum = t1.DeleteReson
                })
                .ToPageListAsync(requestBody.PageIndex, requestBody.PageSize, total);

            return await GetResultAsync(rr, total);
        }
        /// <summary>
        /// 获取查询结果集
        /// </summary>
        /// <param name="rr"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        private async Task<PageResult<ContractSearch>> GetResultAsync(List<ContractSearch> rr, int total)
        {
            PageResult<ContractSearch> rt = new();

            var position = await _dbContext.Queryable<Position>().Where(t => t.IsDelete == 1).ToListAsync();
            var empTable = await _dbContext.Queryable<EmploymentType>().Where(t => rr.Select(x => x.EmploymentType).Contains(t.BusinessId.ToString())).ToListAsync();
            var ownShipTable = await _dbContext.Queryable<OwnerShip>().Where(t => rr.Select(x => x.OnBoard).Contains(t.BusinessId.ToString())).ToListAsync();
            var countryTable = await _dbContext.Queryable<CountryRegion>().Where(t => rr.Select(x => x.Country).Contains(t.BusinessId.ToString())).ToListAsync();

            foreach (var u in rr)
            {
                u.OnStatus = EnumUtil.GetDescription(ShipUserStatus(u.WorkShipStartTime, u.DeleteResonEnum));
                u.ContractTypeName = EnumUtil.GetDescription(u.ContractType);
                u.EmploymentTypeName = empTable.FirstOrDefault(x => x.BusinessId.ToString() == u.EmploymentType)?.Name;
                u.OnBoardName = ownShipTable.FirstOrDefault(x => x.BusinessId.ToString() == u.OnBoard)?.ShipName;
                u.CountryName = countryTable.FirstOrDefault(x => x.BusinessId.ToString() == u.Country)?.Name;
                u.ShipTypeName = EnumUtil.GetDescription(u.ShipType);
                u.Age = CalculateAgeFromIdCard(u.CardId);
                u.DueDays = TimeHelper.GetTimeSpan(Convert.ToDateTime(u.EndTime), DateTime.Now).Days + 1;
                if (u.FPosition != null) u.FPositionName = position.FirstOrDefault(x => x.BusinessId.ToString() == u.FPosition)?.Name;
                if (u.SPosition != null) u.SPositionName = position.FirstOrDefault(x => x.BusinessId.ToString() == u.SPosition)?.Name;
                if (u.OnBoardPosition != null) u.OnBoardPositionName = position.FirstOrDefault(x => x.BusinessId.ToString() == u.OnBoardPosition)?.Name;
            }

            rt.List = rr;
            rt.TotalCount = total;
            return rt;
        }
        /// <summary>
        /// 通过身份证与当前日期计算年龄
        /// </summary>
        /// <param name="idCard"></param>
        /// <returns></returns>
        private static int CalculateAgeFromIdCard(string idCard)
        {
            if (idCard.Length != 18)
            {
                throw new ArgumentException("身份证号码应为18位");
            }

            // 提取出生日期（身份证的前 6 位是出生年月日，格式为yyyyMMdd）
            string birthDateString = idCard.Substring(6, 8);

            DateTime birthDate = DateTime.ParseExact(birthDateString, "yyyyMMdd", CultureInfo.InvariantCulture);

            DateTime currentDate = DateTime.Now;

            // 计算年龄
            int age = currentDate.Year - birthDate.Year;

            // 如果当前日期的月份和日子还没到出生日期的月份和日子，就减去 1 年
            if (currentDate.Month < birthDate.Month || (currentDate.Month == birthDate.Month && currentDate.Day < birthDate.Day))
            {
                age--;
            }

            return age;
        }
        /// <summary>
        /// 船员状态
        /// </summary>
        /// <param name="departureTime">下船时间</param>
        /// <param name="deleteResonEnum">是否删除</param>
        /// <returns></returns>
        private static CrewStatusEnum ShipUserStatus(DateTime departureTime, CrewStatusEnum deleteResonEnum)
        {
            var status = new CrewStatusEnum();
            if (deleteResonEnum != CrewStatusEnum.Normal)
            {
                //删除：管理人员手动操作，包括离职、调离和退休，优先于其他任何状态
                status = deleteResonEnum;
            }
            //else if (holidayTime.HasValue)
            //{
            //    //休假：提交离船申请且经审批同意后，按所申请离船、归船日期设置为休假状态，优先于在岗、待岗状态
            //    status = CrewStatusEnum.XiuJia;
            //}
            else if (departureTime <= DateTime.Now)
            {
                //在岗、待岗:船员登记时必填任职船舶数据，看其中最新的任职船舶上船时间和下船时间，在此时间内为在岗状态，否则为待岗状态
                status = CrewStatusEnum.Normal;
            }
            return status;
        }
        #endregion

        #region 合同续签
        /// <summary>
        /// 合同续签
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        public async Task<Result> SaveContractAsync(ConntractRenewal requestBody)
        {
            var newContract = await _dbContext.Queryable<UserEntryInfo>()
                .Where(t => requestBody.Id == t.BusinessId.ToString())
                .OrderByDescending(x => x.EndTime)
                .FirstAsync();
            if (requestBody.EntryTime < newContract.EndTime)
            {
                return Result.Fail("续签开始时间不能小于当前合同结束时间");
            }
            if (newContract != null)
            {
                newContract.EmploymentId = requestBody.EmploymentType;
                newContract.LaborCompany = requestBody.LaborCompany;
                newContract.ContractType = requestBody.ContractType;
                newContract.EntryTime = requestBody.EntryTime;
                newContract.EndTime = requestBody.EndTime;
                newContract.ContractMain = requestBody.ContractMain;
                await _dbContext.Updateable(newContract).ExecuteCommandAsync();
                return Result.Success("续签更改成功");
            }
            else
            {
                var addContract = new UserEntryInfo
                {
                    Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                    BusinessId = GuidUtil.Next(),
                    ContractMain = requestBody.ContractMain,
                    ContractType = requestBody.ContractType,
                    EmploymentId = requestBody.EmploymentType,
                    EntryTime = requestBody.EntryTime,
                    EndTime = requestBody.EndTime,
                    LaborCompany = requestBody.LaborCompany,
                    UserEntryId = requestBody.BId
                };
                await _dbContext.Insertable(addContract).ExecuteCommandAsync();
                return Result.Success("续签成功");
            }
        }
        #endregion
    }
}
