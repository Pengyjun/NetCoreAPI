using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Dtos.Contract;
using HNKC.CrewManagePlatform.SqlSugars.Models;
using HNKC.CrewManagePlatform.Utils;
using SqlSugar;
using System.Data.SqlTypes;
using UtilsSharp;

namespace HNKC.CrewManagePlatform.Services.Interface.Contract
{
    /// <summary>
    /// 合同实现层
    /// </summary>
    public class ContractService : IContractService
    {
        private readonly ISqlSugarClient _dbContext;
        private readonly IBaseService _baseService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="baseService"></param>
        public ContractService(ISqlSugarClient dbContext, IBaseService baseService)
        {
            this._dbContext = dbContext;
            _baseService = baseService;
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
                .Where(t1 => t1.IsLoginUser == 1 && t1.IsDelete == 1)
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

            return await GetContractResultAsync(rr, total);
        }
        /// <summary>
        /// 获取查询结果集
        /// </summary>
        /// <param name="rr"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        private async Task<PageResult<ContractSearch>> GetContractResultAsync(List<ContractSearch> rr, int total)
        {
            PageResult<ContractSearch> rt = new();

            var position = await _dbContext.Queryable<Position>().Where(t => t.IsDelete == 1).ToListAsync();
            var empTable = await _dbContext.Queryable<EmploymentType>().Where(t => rr.Select(x => x.EmploymentType).Contains(t.BusinessId.ToString())).ToListAsync();
            var ownShipTable = await _dbContext.Queryable<OwnerShip>().Where(t => rr.Select(x => x.OnBoard).Contains(t.BusinessId.ToString())).ToListAsync();
            var countryTable = await _dbContext.Queryable<CountryRegion>().Where(t => rr.Select(x => x.Country).Contains(t.BusinessId.ToString())).ToListAsync();

            foreach (var u in rr)
            {
                u.OnStatus = EnumUtil.GetDescription(_baseService.ShipUserStatus(u.WorkShipStartTime, u.DeleteResonEnum));
                u.ContractTypeName = EnumUtil.GetDescription(u.ContractType);
                u.EmploymentTypeName = empTable.FirstOrDefault(x => x.BusinessId.ToString() == u.EmploymentType)?.Name;
                u.OnBoardName = ownShipTable.FirstOrDefault(x => x.BusinessId.ToString() == u.OnBoard)?.ShipName;
                u.CountryName = countryTable.FirstOrDefault(x => x.BusinessId.ToString() == u.Country)?.Name;
                u.ShipTypeName = EnumUtil.GetDescription(u.ShipType);
                u.Age = _baseService.CalculateAgeFromIdCard(u.CardId);
                u.DueDays = TimeHelper.GetTimeSpan(Convert.ToDateTime(u.EndTime), DateTime.Now).Days + 1;
                if (u.FPosition != null) u.FPositionName = position.FirstOrDefault(x => x.BusinessId.ToString() == u.FPosition)?.Name;
                if (u.SPosition != null) u.SPositionName = position.FirstOrDefault(x => x.BusinessId.ToString() == u.SPosition)?.Name;
                if (u.OnBoardPosition != null) u.OnBoardPositionName = position.FirstOrDefault(x => x.BusinessId.ToString() == u.OnBoardPosition)?.Name;
            }

            rt.List = rr;
            rt.TotalCount = total;
            return rt;
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
            if (requestBody.StartTime < newContract.EndTime)
            {
                return Result.Fail("续签开始时间不能小于当前合同结束时间");
            }
            if (newContract != null)
            {
                newContract.EmploymentId = requestBody.EmploymentType;
                newContract.LaborCompany = requestBody.LaborCompany;
                newContract.ContractType = requestBody.ContractType;
                newContract.EntryTime = requestBody.StartTime;
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
                    EntryTime = requestBody.StartTime,
                    EndTime = requestBody.EndTime,
                    LaborCompany = requestBody.LaborCompany,
                    UserEntryId = requestBody.BId
                };
                await _dbContext.Insertable(addContract).ExecuteCommandAsync();
                return Result.Success("续签成功");
            }
        }

        #endregion

        #region 职务晋升列表
        /// <summary>
        /// 职务晋升列表
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        public async Task<PageResult<PromotionSearch>> SearchPromotionAsync(PromotionRequest requestBody)
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
                .Where(t1 => t1.IsLoginUser == 1 && t1.IsDelete == 1)
                .WhereIF(!string.IsNullOrEmpty(requestBody.KeyWords), t1 => requestBody.KeyWords.Contains(t1.Name) || requestBody.KeyWords.Contains(t1.Phone) || requestBody.KeyWords.Contains(t1.WorkNumber) || requestBody.KeyWords.Contains(t1.CardId))
                .LeftJoin(uentity, (t1, t2) => t1.BusinessId == t2.UserEntryId)
                .InnerJoin<OwnerShip>((t1, t2, t3) => t1.OnBoard == t3.BusinessId.ToString())
                .InnerJoin<CertificateOfCompetency>((t1, t2, t3, t4) => t1.BusinessId == t4.CertificateId)
                .InnerJoin(wShip, (t1, t2, t3, t4, t5) => t1.BusinessId == t5.WorkShipId)
                .WhereIF(!string.IsNullOrEmpty(requestBody.Position), (t1, t2, t3, t4, t5) => requestBody.KeyWords == t5.Postition)
                .Select((t1, t2, t3, t4, t5) => new PromotionSearch
                {
                    BId = t1.BusinessId.ToString(),
                    Id = t2.BusinessId.ToString(),
                    Country = t3.Country,
                    OnBoard = t1.OnBoard,
                    ShipType = t3.ShipType,
                    UserName = t1.Name,
                    WorkNumber = t1.WorkNumber,
                    LaborCompany = t2.LaborCompany,
                    CardId = t1.CardId,
                    OnBoardPosition = t5.Postition,
                    DeleteResonEnum = t1.DeleteReson
                })
                .ToPageListAsync(requestBody.PageIndex, requestBody.PageSize, total);

            return await GetPromotionResultAsync(rr, total);
        }
        /// <summary>
        /// 获取查询结果集
        /// </summary>
        /// <param name="rr"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        private async Task<PageResult<PromotionSearch>> GetPromotionResultAsync(List<PromotionSearch> rr, int total)
        {
            PageResult<PromotionSearch> rt = new();

            var position = await _dbContext.Queryable<Position>().Where(t => t.IsDelete == 1).ToListAsync();
            var empTable = await _dbContext.Queryable<EmploymentType>().Where(t => rr.Select(x => x.EmploymentType).Contains(t.BusinessId.ToString())).ToListAsync();
            var ownShipTable = await _dbContext.Queryable<OwnerShip>().Where(t => rr.Select(x => x.OnBoard).Contains(t.BusinessId.ToString())).ToListAsync();
            var countryTable = await _dbContext.Queryable<CountryRegion>().Where(t => rr.Select(x => x.Country).Contains(t.BusinessId.ToString())).ToListAsync();

            foreach (var u in rr)
            {
                u.OnStatus = EnumUtil.GetDescription(_baseService.ShipUserStatus(u.WorkShipStartTime, u.DeleteResonEnum));
                u.ContractTypeName = EnumUtil.GetDescription(u.ContractType);
                u.EmploymentTypeName = empTable.FirstOrDefault(x => x.BusinessId.ToString() == u.EmploymentType)?.Name;
                u.OnBoardName = ownShipTable.FirstOrDefault(x => x.BusinessId.ToString() == u.OnBoard)?.ShipName;
                u.CountryName = countryTable.FirstOrDefault(x => x.BusinessId.ToString() == u.Country)?.Name;
                u.ShipTypeName = EnumUtil.GetDescription(u.ShipType);
                u.Age = _baseService.CalculateAgeFromIdCard(u.CardId);
                u.DueDays = TimeHelper.GetTimeSpan(Convert.ToDateTime(u.EndTime), DateTime.Now).Days + 1;
                if (u.FPosition != null) u.FPositionName = position.FirstOrDefault(x => x.BusinessId.ToString() == u.FPosition)?.Name;
                if (u.SPosition != null) u.SPositionName = position.FirstOrDefault(x => x.BusinessId.ToString() == u.SPosition)?.Name;
                if (u.OnBoardPosition != null) u.OnBoardPositionName = position.FirstOrDefault(x => x.BusinessId.ToString() == u.OnBoardPosition)?.Name;
            }

            rt.List = rr;
            rt.TotalCount = total;
            return rt;
        }
        #endregion

        #region 职务晋升
        /// <summary>
        /// 职务晋升
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        public async Task<Result> SavePromotionAsync(PositionPromotion requestBody)
        {
            var promotion = await _dbContext.Queryable<Promotion>()
                .Where(t => requestBody.Id == t.BusinessId.ToString())
                .OrderByDescending(x => x.PromotionTime)
                .FirstAsync();

            if (promotion != null)
            {
                promotion.OnShip = requestBody.OnShip;
                promotion.Postition = requestBody.Postition;
                if (requestBody.PromotionScan != null && requestBody.PromotionScan.Any())
                {
                    var files = await _dbContext.Queryable<Files>().Where(t => t.FileId == promotion.PromotionScan).ToListAsync();
                    await _dbContext.Deleteable(files).ExecuteCommandAsync();
                    promotion.PromotionScan = GuidUtil.Next();
                    requestBody.PromotionScan.ForEach(x => x.FileId = promotion.PromotionScan);
                    await _baseService.UpdateFileAsync(requestBody.PromotionScan, requestBody.BId);
                }
                await _dbContext.Updateable(promotion).ExecuteCommandAsync();
                return Result.Success("续签更改成功");
            }
            else
            {
                var fileId = GuidUtil.Next();
                var addPromotion = new Promotion
                {
                    Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                    BusinessId = GuidUtil.Next(),
                    PromotionScan = fileId,
                    OnShip = requestBody.OnShip,
                    Postition = requestBody.Postition,
                    PromotionId = requestBody.BId,
                    PromotionTime = requestBody.PromotionTime
                };
                if (requestBody.PromotionScan != null && requestBody.PromotionScan.Any())
                {
                    var files = await _dbContext.Queryable<Files>().Where(t => t.FileId == fileId).ToListAsync();
                    await _dbContext.Deleteable(files).ExecuteCommandAsync();
                    requestBody.PromotionScan.ForEach(x => x.FileId = fileId);
                    await _baseService.UpdateFileAsync(requestBody.PromotionScan, requestBody.BId);
                }
                await _dbContext.Insertable(addPromotion).ExecuteCommandAsync();
                return Result.Success("续签成功");
            }
        }

        #endregion
        #region 培训记录
        /// <summary>
        /// 培训记录列表
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        public async Task<PageResult<TrainingRecordSearch>> SearchTrainingRecordAsync(TrainingRecordRequest requestBody)
        {
            PageResult<TrainingRecordSearch> rt = new();
            RefAsync<int> total = 0;

            #region 用户培训相关
            //任职船舶 
            var trecord = _dbContext.Queryable<TrainingRecord>()
              .GroupBy(t => new { t.FillRepUserId, t.TrainingTitle, t.TrainingType })
              .Select(t => new { t.FillRepUserId, t.TrainingTitle, t.TrainingType });
            var ssss = trecord.ToList();
            var trecordRes = _dbContext.Queryable<TrainingRecord>()
                .InnerJoin(trecord, (x, y) => x.FillRepUserId == y.FillRepUserId && x.TrainingTitle == y.TrainingTitle && x.TrainingType == y.TrainingType);
            var ss = trecordRes.ToList();
            #endregion

            var rr = await _dbContext.Queryable<TrainingRecord>()
                .InnerJoin(trecord, (x, y) => x.FillRepUserId == y.FillRepUserId && x.TrainingTitle == y.TrainingTitle && x.TrainingType == y.TrainingType)
                .Where((x, y) => x.IsDelete == 1)
                .WhereIF(!string.IsNullOrEmpty(requestBody.TraningType), (x, y) => requestBody.TraningType == x.TrainingType)
                .WhereIF(!string.IsNullOrEmpty(requestBody.StartTime) && !string.IsNullOrEmpty(requestBody.EndTime), (x, y) => x.TrainingTime >= Convert.ToDateTime(requestBody.StartTime) && x.TrainingTime <= Convert.ToDateTime(requestBody.EndTime))
                .Select(x => new TrainingRecordSearch
                {
                    FillReportTime = x.Created.Value.ToString("yyyy/MM/dd"),
                    TrainingAddress = x.TrainingAddress,
                    TrainingTime = x.TrainingTime,
                    TrainingType = x.TrainingType,
                    TrainingTitle = x.TrainingTitle,
                    FillRepUserId = x.FillRepUserId
                })
                .ToPageListAsync(requestBody.PageIndex, requestBody.PageSize, total);
            rt.List = rr;
            rt.TotalCount = total;

            return rt;
        }

        #endregion
    }
}
