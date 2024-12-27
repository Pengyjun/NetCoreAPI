using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Dtos;
using HNKC.CrewManagePlatform.Models.Dtos.Contract;
using HNKC.CrewManagePlatform.Models.Enums;
using HNKC.CrewManagePlatform.SqlSugars.Models;
using HNKC.CrewManagePlatform.Utils;
using SqlSugar;
using UtilsSharp;

namespace HNKC.CrewManagePlatform.Services.Interface.Contract
{
    /// <summary>
    /// 合同实现层
    /// </summary>
    public class ContractService : HNKC.CrewManagePlatform.Services.Interface.CurrentUser.CurrentUserService, IContractService
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

            //获取提醒天数
            var remindSet = await _dbContext.Queryable<RemindSetting>().FirstAsync(t => t.RemindType == 11 && t.IsDelete == 1);
            int days = remindSet == null ? 0 : remindSet.Days;
            if (days == 0) return new PageResult<ContractSearch>();
            var rr = await _dbContext.Queryable<User>()
                .Where(t1 => t1.IsLoginUser == 1 && t1.IsDelete == 1)
                .WhereIF(!string.IsNullOrEmpty(requestBody.KeyWords), t1 => t1.Name.Contains(requestBody.KeyWords) || t1.Phone.Contains(requestBody.KeyWords) || t1.WorkNumber.Contains(requestBody.KeyWords) || t1.CardId.Contains(requestBody.KeyWords))
                .LeftJoin(uentity, (t1, t2) => t1.BusinessId == t2.UserEntryId)
                .Where((t1, t2) => SqlFunc.DateDiff(DateType.Day, DateTime.Now, Convert.ToDateTime(t2.EndTime)) + 1 <= days)
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
                    DueDays = SqlFunc.DateDiff(DateType.Day, DateTime.Now, Convert.ToDateTime(t2.EndTime)) + 1,
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
                //u.DueDays = TimeHelper.GetTimeSpan(Convert.ToDateTime(u.EndTime), DateTime.Now).Days + 1;
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
                .WhereIF(!string.IsNullOrEmpty(requestBody.KeyWords), t1 => t1.Name.Contains(requestBody.KeyWords) || t1.Phone.Contains(requestBody.KeyWords) || t1.WorkNumber.Contains(requestBody.KeyWords) || t1.CardId.Contains(requestBody.KeyWords))
                .LeftJoin(uentity, (t1, t2) => t1.BusinessId == t2.UserEntryId)
                .InnerJoin<OwnerShip>((t1, t2, t3) => t1.OnBoard == t3.BusinessId.ToString())
                .InnerJoin<CertificateOfCompetency>((t1, t2, t3, t4) => t1.BusinessId == t4.CertificateId)
                .InnerJoin(wShip, (t1, t2, t3, t4, t5) => t1.BusinessId == t5.WorkShipId)
                .WhereIF(!string.IsNullOrEmpty(requestBody.Position), (t1, t2, t3, t4, t5) => requestBody.Position == t5.Postition)
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
                promotion.PromotionTime = DateTime.Now;
                if (requestBody.PromotionScan != null && requestBody.PromotionScan.Any())
                {
                    var files = await _dbContext.Queryable<Files>().Where(t => t.FileId == promotion.PromotionScan).ToListAsync();
                    await _dbContext.Deleteable(files).ExecuteCommandAsync();
                    promotion.PromotionScan = GuidUtil.Next();
                    requestBody.PromotionScan.ForEach(x => x.FileId = promotion.PromotionScan);
                    await _baseService.UpdateFileAsync(requestBody.PromotionScan, requestBody.BId);
                }
                await _dbContext.Updateable(promotion).ExecuteCommandAsync();
                return Result.Success("成功");
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
                    PromotionTime = DateTime.Now
                };
                if (requestBody.PromotionScan != null && requestBody.PromotionScan.Any())
                {
                    var files = await _dbContext.Queryable<Files>().Where(t => t.FileId == fileId).ToListAsync();
                    await _dbContext.Deleteable(files).ExecuteCommandAsync();
                    requestBody.PromotionScan.ForEach(x => x.FileId = fileId);
                    await _baseService.UpdateFileAsync(requestBody.PromotionScan, requestBody.BId);
                }
                await _dbContext.Insertable(addPromotion).ExecuteCommandAsync();
                return Result.Success("成功");
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
            var rr = await _dbContext.Queryable<TrainingRecord>()
                          .Where(y => y.IsDelete == 1 && string.IsNullOrEmpty(y.PId.ToString()))
                          .LeftJoin<User>((y, x) => y.FillRepUserId == x.BusinessId)
                          .LeftJoin<TrainingType>((y, x, z) => y.TrainingType == z.BusinessId.ToString())
                          .WhereIF(!string.IsNullOrEmpty(requestBody.TraningType), (y, x, z) => requestBody.TraningType == y.TrainingType)
                          .WhereIF(!string.IsNullOrEmpty(requestBody.StartTime) && !string.IsNullOrEmpty(requestBody.EndTime), (y, x, z) => y.TrainingTime >= Convert.ToDateTime(requestBody.StartTime) && y.TrainingTime <= Convert.ToDateTime(requestBody.EndTime))
                          .Select((y, x, z) => new TrainingRecordSearch
                          {
                              Id = y.BusinessId.ToString(),
                              FillRepUserName = x.Name,
                              TrainingTypeName = z.Name,
                              FillReportTime = y.Created.Value.ToString("yyyy/MM/dd"),
                              TrainingAddress = y.TrainingAddress,
                              TrainingTime = y.TrainingTime,
                              TrainingType = y.TrainingType,
                              TrainingTitle = y.TrainingTitle,
                              TrainingCount = SqlFunc.Subqueryable<TrainingRecord>().
                                         Where(z => z.PId == y.BusinessId).Count()
                          })
                          .ToPageListAsync(requestBody.PageIndex, requestBody.PageSize, total);
            var bids = rr.Select(x => x.Id).ToList();
            var uu = await _dbContext.Queryable<TrainingRecord>().Where(t => bids.Contains(t.PId.ToString())).ToListAsync();
            var uids = uu.Select(x => x.TrainingId).ToList();
            var users = await _dbContext.Queryable<User>().Where(t => t.IsDelete == 1 && t.IsLoginUser == 1 && uids.Contains(t.BusinessId)).ToListAsync();

            foreach (var item in rr)
            {
                var uuIds = uu.Where(x => x.PId.ToString() == item.Id).Select(x => x.TrainingId).ToList();
                item.UserDetails = string.Join(",", users.Where(x => uuIds.Contains(x.BusinessId)).Select(x => x.Name));
                item.UserIds = string.Join(",", users.Where(x => uuIds.Contains(x.BusinessId)).Select(x => x.BusinessId));
            }

            rt.List = rr;
            rt.TotalCount = total;
            return rt;
        }
        /// <summary>
        /// 保存培训记录
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        public async Task<Result> SaveTrainingRecordAsync(SaveTrainingRecord requestBody)
        {
            if (requestBody.Type == 1) return await InsertTrainingRecordAsync(requestBody);
            else return await UpdateTrainingRecordAsync(requestBody);
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        public async Task<Result> InsertTrainingRecordAsync(SaveTrainingRecord requestBody)
        {
            List<TrainingRecord> addList = new();
            var fileId = GuidUtil.Next();
            if (requestBody.Scans != null && requestBody.Scans.Any())
            {
                requestBody.Scans.ForEach(x => x.FileId = fileId);
                await _baseService.UpdateFileAsync(requestBody.Scans, GlobalCurrentUser.UserBusinessId);
            }
            var bid = GuidUtil.Next();
            var add = new TrainingRecord
            {
                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                BusinessId = bid,
                FillRepUserId = GlobalCurrentUser.UserBusinessId,
                TrainingAddress = requestBody.TrainingAddress,
                TrainingTime = requestBody.TrainingTime,
                TrainingTitle = requestBody.TrainingTitle,
                TrainingType = requestBody.TrainingType,
                TrainingScan = fileId,
                TrainingId = GuidUtil.Next()
            };
            addList.Add(add);
            foreach (var item in requestBody.UIds)
            {
                addList.Add(new TrainingRecord
                {
                    Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                    BusinessId = GuidUtil.Next(),
                    FillRepUserId = GlobalCurrentUser.UserBusinessId,
                    TrainingAddress = requestBody.TrainingAddress,
                    TrainingTime = requestBody.TrainingTime,
                    TrainingTitle = requestBody.TrainingTitle,
                    TrainingType = requestBody.TrainingType,
                    TrainingScan = fileId,
                    PId = bid,
                    TrainingId = Guid.Parse(item)
                });
            }
            await _dbContext.Insertable(addList).ExecuteCommandAsync();
            return Result.Success("新增成功");
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        private async Task<Result> UpdateTrainingRecordAsync(SaveTrainingRecord requestBody)
        {
            var rt = await _dbContext.Queryable<TrainingRecord>().Where(t => t.BusinessId.ToString() == requestBody.BId).FirstAsync();
            if (rt != null)
            {
                var rr = await _dbContext.Queryable<TrainingRecord>().Where(t => rt.PId == t.BusinessId).ToListAsync();
                if (rr.Any())
                {
                    await _dbContext.Deleteable(rr).ExecuteCommandAsync();
                    await InsertTrainingRecordAsync(requestBody);
                    return Result.Success("修改成功");
                }
                else
                {
                    return Result.Fail("失败");
                }
            }
            else
            {
                return Result.Fail("无数据");
            }
        }

        #endregion

        #region 年度考核
        /// <summary>
        /// 年度审核列表
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        public async Task<PageResult<YearCheckSearch>> SearchYearCheckAsync(YearCheckRequest requestBody)
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
                .WhereIF(!string.IsNullOrEmpty(requestBody.KeyWords), t1 => t1.Name.Contains(requestBody.KeyWords) || t1.Phone.Contains(requestBody.KeyWords) || t1.WorkNumber.Contains(requestBody.KeyWords) || t1.CardId.Contains(requestBody.KeyWords))
                .LeftJoin(uentity, (t1, t2) => t1.BusinessId == t2.UserEntryId)
                .InnerJoin<OwnerShip>((t1, t2, t3) => t1.OnBoard == t3.BusinessId.ToString())
                .InnerJoin<CertificateOfCompetency>((t1, t2, t3, t4) => t1.BusinessId == t4.CertificateId)
                .InnerJoin(wShip, (t1, t2, t3, t4, t5) => t1.BusinessId == t5.WorkShipId)
                .LeftJoin<YearCheck>((t1, t2, t3, t4, t5, t6) => t1.BusinessId == t6.TrainingId)
                .WhereIF(requestBody.CheckStatus == 1, (t1, t2, t3, t4, t5, t6) => t6.CheckType != CheckEnum.Normal)
                .WhereIF(requestBody.CheckStatus == 2, (t1, t2, t3, t4, t5, t6) => t6.CheckType == CheckEnum.Normal)
                .WhereIF(!string.IsNullOrEmpty(requestBody.Year), (t1, t2, t3, t4, t5, t6) => t6.TrainingTime.Value.Year.ToString() == requestBody.Year)
                .Select((t1, t2, t3, t4, t5, t6) => new YearCheckSearch
                {
                    BId = t1.BusinessId.ToString(),
                    Id = t6.BusinessId.ToString(),
                    Country = t3.Country,
                    OnBoard = t1.OnBoard,
                    ShipType = t3.ShipType,
                    UserName = t1.Name,
                    WorkNumber = t1.WorkNumber,
                    CardId = t1.CardId,
                    CheckType = t6.CheckType,
                    CheckFillRepTime = t6.Created.Value.ToString("yyyy/MM/dd HH:mm:ss"),
                    CheckTypeStatus = t6.CheckType != CheckEnum.Normal ? "已考核" : "未考核",
                    DeleteResonEnum = t1.DeleteReson,
                    ContractType = t2.ContractType,
                    FPosition = t4.FPosition,
                    SPosition = t4.SPosition,
                    OnBoardPosition = t5.Postition,
                    WorkShipStartTime = t5.WorkShipStartTime,
                })
                .ToPageListAsync(requestBody.PageIndex, requestBody.PageSize, total);
            return await GetYearCheckResultAsync(rr, total);
        }
        /// <summary>
        /// 获取查询结果集
        /// </summary>
        /// <param name="rr"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        private async Task<PageResult<YearCheckSearch>> GetYearCheckResultAsync(List<YearCheckSearch> rr, int total)
        {
            PageResult<YearCheckSearch> rt = new();

            var position = await _dbContext.Queryable<Position>().Where(t => t.IsDelete == 1).ToListAsync();
            var ownShipTable = await _dbContext.Queryable<OwnerShip>().Where(t => rr.Select(x => x.OnBoard).Contains(t.BusinessId.ToString())).ToListAsync();
            var countryTable = await _dbContext.Queryable<CountryRegion>().Where(t => rr.Select(x => x.Country).Contains(t.BusinessId.ToString())).ToListAsync();

            foreach (var u in rr)
            {
                u.CheckTypeName = EnumUtil.GetDescription(u.CheckType);
                u.OnStatus = EnumUtil.GetDescription(_baseService.ShipUserStatus(u.WorkShipStartTime, u.DeleteResonEnum));
                u.ContractTypeName = EnumUtil.GetDescription(u.ContractType);
                u.OnBoardName = ownShipTable.FirstOrDefault(x => x.BusinessId.ToString() == u.OnBoard)?.ShipName;
                u.CountryName = countryTable.FirstOrDefault(x => x.BusinessId.ToString() == u.Country)?.Name;
                u.ShipTypeName = EnumUtil.GetDescription(u.ShipType);
                u.Age = _baseService.CalculateAgeFromIdCard(u.CardId);
                if (u.FPosition != null) u.FPositionName = position.FirstOrDefault(x => x.BusinessId.ToString() == u.FPosition)?.Name;
                if (u.SPosition != null) u.SPositionName = position.FirstOrDefault(x => x.BusinessId.ToString() == u.SPosition)?.Name;
                if (u.OnBoardPosition != null) u.OnBoardPositionName = position.FirstOrDefault(x => x.BusinessId.ToString() == u.OnBoardPosition)?.Name;
            }

            rt.List = rr;
            rt.TotalCount = total;
            return rt;
        }

        #endregion

        #region 考核
        /// <summary>
        /// 年度考核
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        public async Task<Result> SaveYearCheckAsync(SaveYearCheck requestBody)
        {
            var rt = await _dbContext.Queryable<YearCheck>().Where(t => t.IsDelete == 1 && t.TrainingId.ToString() == requestBody.BId && DateTime.Now.Year == t.TrainingTime.Value.Year).FirstAsync();
            if (rt != null)
            {
                if (requestBody.Scans != null && requestBody.Scans.Any())
                {
                    rt.CheckType = requestBody.CheckType;
                    var files = await _dbContext.Queryable<Files>().Where(t => t.FileId == rt.TrainingScan).ToListAsync();
                    await _dbContext.Deleteable(files).ExecuteCommandAsync();
                    rt.TrainingScan = GuidUtil.Next();
                    requestBody.Scans.ForEach(x => x.FileId = rt.TrainingScan);
                    await _baseService.UpdateFileAsync(requestBody.Scans, Guid.Parse(requestBody.BId));
                    await _dbContext.Updateable(rt).UpdateColumns(x => new { x.CheckType, x.TrainingScan }).ExecuteCommandAsync();
                    return Result.Success("成功");
                }
                else
                {
                    return Result.Success("失败");
                }
            }
            else
            {
                var fileId = GuidUtil.Next();
                var addPromotion = new YearCheck
                {
                    Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                    BusinessId = GuidUtil.Next(),
                    TrainingScan = fileId,
                    CheckType = requestBody.CheckType,
                    TrainingId = Guid.Parse(requestBody.BId),
                    TrainingTime = DateTime.Now
                };
                if (requestBody.Scans != null && requestBody.Scans.Any())
                {
                    requestBody.Scans.ForEach(x => x.FileId = fileId);
                    await _baseService.UpdateFileAsync(requestBody.Scans, Guid.Parse(requestBody.BId));
                }
                await _dbContext.Insertable(addPromotion).ExecuteCommandAsync();
                return Result.Success("成功");
            }
        }
        #endregion
    }
}
