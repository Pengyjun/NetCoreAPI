using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Dtos.Contract;
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
            #endregion

            var rr = await _dbContext.Queryable<User>()
                .Where(t1 => t1.IsLoginUser == 1)
                .WhereIF(!string.IsNullOrEmpty(requestBody.KeyWords), t1 => requestBody.KeyWords.Contains(t1.Name) || requestBody.KeyWords.Contains(t1.Phone) || requestBody.KeyWords.Contains(t1.WorkNumber) || requestBody.KeyWords.Contains(t1.CardId))
                .LeftJoin(uentity, (t1, t2) => t1.BusinessId == t2.UserEntryId)
                .InnerJoin<OwnerShip>((t1, t2, t3) => t1.OnBoard == t3.BusinessId.ToString())
                .WhereIF(!string.IsNullOrEmpty(requestBody.EmploymentType), (t1, t2, t3) => requestBody.EmploymentType == t2.EmploymentId)
                .Select((t1, t2, t3) => new ContractSearch
                {
                    Id = t1.BusinessId.ToString(),
                    Country = t3.Country,
                    OnBoard = t1.OnBoard,
                    ShipType = t3.ShipType,
                    UserName = t1.Name,
                    WorkNumber = t1.WorkNumber,
                    EndTime = t2.EndTime.ToString("yyyy/MM/dd"),
                    EntryTime = t2.EntryTime.ToString("yyyy/MM/dd"),
                    ContractMain = t2.ContractMain,
                    ContractType = t2.ContractType,
                    EmploymentType = t2.EmploymentId,
                    LaborCompany = t2.LaborCompany,
                    CardId = t1.CardId
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

            var empTable = await _dbContext.Queryable<EmploymentType>().Where(t => rr.Select(x => x.EmploymentType).Contains(t.BusinessId.ToString())).ToListAsync();
            var ownShipTable = await _dbContext.Queryable<OwnerShip>().Where(t => rr.Select(x => x.OnBoard).Contains(t.BusinessId.ToString())).ToListAsync();
            var countryTable = await _dbContext.Queryable<CountryRegion>().Where(t => rr.Select(x => x.Country).Contains(t.BusinessId.ToString())).ToListAsync();

            foreach (var u in rr)
            {
                u.ContractTypeName = EnumUtil.GetDescription(u.ContractType);
                u.EmploymentTypeName = empTable.FirstOrDefault(x => x.BusinessId.ToString() == u.EmploymentType)?.Name;
                u.OnBoardName = ownShipTable.FirstOrDefault(x => x.BusinessId.ToString() == u.OnBoard)?.ShipName;
                u.CountryName = countryTable.FirstOrDefault(x => x.BusinessId.ToString() == u.Country)?.Name;
                u.ShipTypeName = EnumUtil.GetDescription(u.ShipType);
                u.Age = CalculateAgeFromIdCard(u.CardId);
                u.DueDays = TimeHelper.GetTimeSpan(Convert.ToDateTime(u.EndTime), DateTime.Now).Days + 1;
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
    }
}
