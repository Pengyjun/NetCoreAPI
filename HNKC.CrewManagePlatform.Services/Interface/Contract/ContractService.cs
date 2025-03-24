using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Dtos;
using HNKC.CrewManagePlatform.Models.Dtos.Contract;
using HNKC.CrewManagePlatform.Models.Dtos.CrewArchives;
using HNKC.CrewManagePlatform.Models.Enums;
using HNKC.CrewManagePlatform.Services.Interface.Certificate;
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
        private readonly ICertificateService _certificateService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="baseService"></param>
        /// <param name="certificateService"></param>
        public ContractService(ISqlSugarClient dbContext, IBaseService baseService, ICertificateService certificateService)
        {
            this._dbContext = dbContext;
            this._baseService = baseService;
            this._certificateService = certificateService;
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
            var roleType = await _baseService.CurRoleType();
            if (roleType == -1) { return new PageResult<ContractSearch>(); }
            #region 船员关联
            var uentityFist = _dbContext.Queryable<UserEntryInfo>()
                .GroupBy(u => u.UserEntryId)
                .Select(x => new { x.UserEntryId, EndTime = SqlFunc.AggregateMax(x.EndTime) });
            var uentity = _dbContext.Queryable<UserEntryInfo>()
                .InnerJoin(uentityFist, (x, y) => x.UserEntryId == y.UserEntryId && x.EndTime == y.EndTime);
            var crewWorkShip = _dbContext.Queryable<WorkShip>().Where(t => t.IsDelete == 1)
                                .GroupBy(u => u.WorkShipId)
                                .Select(t => new { t.WorkShipId, WorkShipStartTime = SqlFunc.AggregateMax(t.WorkShipStartTime) });
            var wShip = _dbContext.Queryable<WorkShip>().Where(t => t.IsDelete == 1)
              .InnerJoin(crewWorkShip, (x, y) => x.WorkShipId == y.WorkShipId && x.WorkShipStartTime == y.WorkShipStartTime);
            #endregion

            //获取提醒天数
            var remindSet = await _dbContext.Queryable<RemindSetting>().FirstAsync(t => t.RemindType == 11 && t.Enable == 1 && t.IsDelete == 1);
            int days = remindSet == null ? 0 : remindSet.Days;
            if (days == 0) return new PageResult<ContractSearch>();
            var rr = await _dbContext.Queryable<User>()
                .Where(t1 => t1.IsLoginUser == 1 && t1.IsDelete == 1)
                .WhereIF(!string.IsNullOrEmpty(requestBody.KeyWords), t1 => t1.Name.Contains(requestBody.KeyWords) || t1.Phone.Contains(requestBody.KeyWords) || t1.WorkNumber.Contains(requestBody.KeyWords) || t1.CardId.Contains(requestBody.KeyWords))
                .LeftJoin(uentity, (t1, t2) => t1.BusinessId == t2.UserEntryId)
                .Where((t1, t2) => SqlFunc.DateDiff(DateType.Day, DateTime.Now, Convert.ToDateTime(t2.EndTime)) + 1 <= days)
                .InnerJoin(wShip, (t1, t2, t5) => t1.BusinessId == t5.WorkShipId)
                .InnerJoin<OwnerShip>((t1, t2, t5, t3) => t5.OnShip == t3.BusinessId.ToString())
                .WhereIF(roleType == 3, (t1, t2, t5, t3) => t5.OnShip == t3.BusinessId.ToString() && GlobalCurrentUser.ShipId.ToString() == t5.OnShip)//船长
                .WhereIF(!string.IsNullOrEmpty(requestBody.EmploymentType), (t1, t2, t5, t3) => requestBody.EmploymentType == t2.EmploymentId)
                .Select((t1, t2, t5, t3) => new ContractSearch
                {
                    BId = t1.BusinessId.ToString(),
                    Id = t2.BusinessId.ToString(),
                    Country = t3.Country,
                    OnBoard = t5.OnShip,
                    ShipType = t3.ShipType,
                    UserName = t1.Name,
                    WorkNumber = t1.WorkNumber,
                    EndTime = t2.EndTime,
                    StartTime = t2.StartTime,
                    ContractMain = t2.ContractMain,
                    ContractType = t2.ContractType,
                    EmploymentType = t2.EmploymentId,
                    LaborCompany = t2.LaborCompany,
                    CardId = t1.CardId,
                    DueDays = SqlFunc.DateDiff(DateType.Day, DateTime.Now, Convert.ToDateTime(t2.EndTime)) + 1,
                    OnBoardPosition = t5.Postition,
                    WorkShipStartTime = t5.WorkShipStartTime,
                    WorkShipEndTime = t5.WorkShipEndTime,
                    DeleteResonEnum = t1.DeleteReson
                })
                .Distinct()
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
            var countryTable = await _dbContext.Queryable<CountryRegion>().Where(t => rr.Select(x => x.Country).Contains(t.BusinessId)).ToListAsync();
            //第一适任 第二适任
            var uIds = rr.Select(x => x.BId).ToList();
            var cerOfComps = await _dbContext.Queryable<CertificateOfCompetency>().Where(x => uIds.Contains(x.CertificateId.ToString()) && (x.Type == CertificatesEnum.FCertificate || x.Type == CertificatesEnum.SCertificate)).ToListAsync();
            foreach (var u in rr)
            {
                u.OnStatus = EnumUtil.GetDescription(_baseService.ShipUserStatus(u.WorkShipStartTime, u.WorkShipEndTime, u.DeleteResonEnum));
                u.ContractTypeName = EnumUtil.GetDescription(u.ContractType);
                u.EmploymentTypeName = empTable.FirstOrDefault(x => x.BusinessId.ToString() == u.EmploymentType)?.Name;
                u.OnBoardName = ownShipTable.FirstOrDefault(x => x.BusinessId.ToString() == u.OnBoard)?.ShipName;
                u.CountryName = countryTable.FirstOrDefault(x => x.BusinessId == u.Country)?.Name;
                u.ShipTypeName = EnumUtil.GetDescription(u.ShipType);
                u.Age = _baseService.CalculateAgeFromIdCard(u.CardId);
                //u.DueDays = TimeHelper.GetTimeSpan(Convert.ToDateTime(u.EndTime), DateTime.Now).Days + 1;
                if (cerOfComps != null)
                {
                    var cerofcF = cerOfComps.FirstOrDefault(x => x.Type == CertificatesEnum.FCertificate && x.CertificateId.ToString() == u.BId);
                    var cerofcS = cerOfComps.FirstOrDefault(x => x.Type == CertificatesEnum.SCertificate && x.CertificateId.ToString() == u.BId);
                    u.FPositionName = position.FirstOrDefault(x => x.BusinessId.ToString() == cerofcF?.FPosition)?.Name;
                    u.FPosition = cerofcF?.FPosition;
                    u.SPositionName = position.FirstOrDefault(x => x.BusinessId.ToString() == cerofcS?.SPosition)?.Name;
                    u.SPosition = cerofcS?.SPosition;
                }
                if (u.FPosition != null) u.FPositionName = position.FirstOrDefault(x => x.BusinessId.ToString() == u.FPosition)?.Name;
                if (u.SPosition != null) u.SPositionName = position.FirstOrDefault(x => x.BusinessId.ToString() == u.SPosition)?.Name;
                if (u.OnBoardPosition != null) u.OnBoardPositionName = position.FirstOrDefault(x => x.BusinessId.ToString() == u.OnBoardPosition)?.Name;
            }

            rt.List = rr.OrderBy(x => x.DueDays);
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
            var roleType = await _baseService.CurRoleType();
            if (roleType == -1) { return new PageResult<PromotionSearch>(); }
            #region 船员关联
            //var uentityFist = _dbContext.Queryable<UserEntryInfo>()
            //    .GroupBy(u => u.UserEntryId)
            //    .Select(x => new { x.UserEntryId, StartTime = SqlFunc.AggregateMax(x.StartTime) });
            //var uentity = _dbContext.Queryable<UserEntryInfo>()
            //    .InnerJoin(uentityFist, (x, y) => x.UserEntryId == y.UserEntryId && x.StartTime == y.StartTime);

            // 获取每个用户的最新入职时间
            var subQuery1 = _dbContext.Queryable<UserEntryInfo>()
                .GroupBy(u => new { u.UserEntryId })
                .Select(x => new { x.UserEntryId, StartTime = SqlFunc.AggregateMax(x.StartTime) });
            // 在这些最新入职时间的记录中，取 id 最大的一条
            var subQuery2 = _dbContext.Queryable<UserEntryInfo>()
                .Where(al => SqlFunc.Subqueryable<UserEntryInfo>()
                    .Where(sub => sub.UserEntryId == al.UserEntryId && sub.StartTime == al.StartTime)
                    .Any())
                .GroupBy(al => al.UserEntryId)
                .Select(al => new
                {
                    UserId = al.UserEntryId,
                    LatestId = SqlFunc.AggregateMax(al.Id)
                });
            // 根据 id 获取完整记录
            var result = _dbContext.Queryable<UserEntryInfo>()
                .InnerJoin(subQuery2, (a, latest) => a.Id == latest.LatestId)
                .Select(a => a);

            var crewWorkShip = _dbContext.Queryable<WorkShip>().Where(t => t.IsDelete == 1)
                         .GroupBy(u => u.WorkShipId)
                         .Select(t => new { t.WorkShipId, WorkShipStartTime = SqlFunc.AggregateMax(t.WorkShipStartTime) });
            var wShip = _dbContext.Queryable<WorkShip>().Where(t => t.IsDelete == 1)
              .InnerJoin(crewWorkShip, (x, y) => x.WorkShipId == y.WorkShipId && x.WorkShipStartTime == y.WorkShipStartTime);
            #endregion

            var rr = await _dbContext.Queryable<User>()
                .Where(t1 => t1.IsLoginUser == 1 && t1.IsDelete == 1)
                .WhereIF(!string.IsNullOrEmpty(requestBody.KeyWords), t1 => t1.Name.Contains(requestBody.KeyWords) || t1.Phone.Contains(requestBody.KeyWords) || t1.WorkNumber.Contains(requestBody.KeyWords) || t1.CardId.Contains(requestBody.KeyWords))
                .LeftJoin(result, (t1, t2) => t1.BusinessId == t2.UserEntryId)
                .LeftJoin(wShip, (t1, t2, t5) => t1.BusinessId == t5.WorkShipId)
                .InnerJoin<OwnerShip>((t1, t2, t5, t3) => t5.OnShip == t3.BusinessId.ToString())
                .WhereIF(roleType == 3, (t1, t2, t5, t3) => t5.OnShip == t3.BusinessId.ToString() && GlobalCurrentUser.ShipId.ToString() == t5.OnShip)//船长
                .WhereIF(!string.IsNullOrEmpty(requestBody.Position), (t1, t2, t5, t3) => requestBody.Position == t5.Postition)
                .Select((t1, t2, t5, t3) => new PromotionSearch
                {
                    BId = t1.BusinessId.ToString(),
                    Id = t2.BusinessId.ToString(),
                    Country = t3.Country,
                    OnBoard = t5.OnShip,
                    ShipType = t3.ShipType,
                    UserName = t1.Name,
                    WorkNumber = t1.WorkNumber,
                    LaborCompany = t2.LaborCompany,
                    CardId = t1.CardId,
                    OnBoardPosition = t5.Postition,
                    DeleteResonEnum = t1.DeleteReson,
                    WorkShipStartTime = t5.WorkShipStartTime,
                    WorkShipEndTime = t5.WorkShipEndTime
                })
                .Distinct()
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
            var countryTable = await _dbContext.Queryable<CountryRegion>().Where(t => rr.Select(x => x.Country).Contains(t.BusinessId)).ToListAsync();
            //第一适任 第二适任
            var uIds = rr.Select(x => x.BId).ToList();
            var cerOfComps = await _dbContext.Queryable<CertificateOfCompetency>().Where(x => uIds.Contains(x.CertificateId.ToString()) && (x.Type == CertificatesEnum.FCertificate || x.Type == CertificatesEnum.SCertificate)).ToListAsync();
            foreach (var u in rr)
            {
                u.OnStatus = EnumUtil.GetDescription(_baseService.ShipUserStatus(u.WorkShipStartTime, u.WorkShipEndTime, u.DeleteResonEnum));
                u.ContractTypeName = EnumUtil.GetDescription(u.ContractType);
                u.EmploymentTypeName = empTable.FirstOrDefault(x => x.BusinessId.ToString() == u.EmploymentType)?.Name;
                u.OnBoardName = ownShipTable.FirstOrDefault(x => x.BusinessId.ToString() == u.OnBoard)?.ShipName;
                u.CountryName = countryTable.FirstOrDefault(x => x.BusinessId == u.Country)?.Name;
                u.ShipTypeName = EnumUtil.GetDescription(u.ShipType);
                u.Age = _baseService.CalculateAgeFromIdCard(u.CardId);
                u.DueDays = TimeHelper.GetTimeSpan(Convert.ToDateTime(u.EndTime), DateTime.Now).Days + 1;
                if (cerOfComps != null)
                {
                    var cerofcF = cerOfComps.FirstOrDefault(x => x.Type == CertificatesEnum.FCertificate && x.CertificateId.ToString() == u.BId);
                    var cerofcS = cerOfComps.FirstOrDefault(x => x.Type == CertificatesEnum.SCertificate && x.CertificateId.ToString() == u.BId);
                    u.FPositionName = position.FirstOrDefault(x => x.BusinessId.ToString() == cerofcF?.FPosition)?.Name;
                    u.FPosition = cerofcF?.FPosition;
                    u.SPositionName = position.FirstOrDefault(x => x.BusinessId.ToString() == cerofcS?.SPosition)?.Name;
                    u.SPosition = cerofcS?.SPosition;
                }
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
                    await _baseService.InsertFileAsync(requestBody.PromotionScan, requestBody.BId);
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
                    requestBody.PromotionScan.ForEach(x => x.FileId = fileId);
                    await _baseService.InsertFileAsync(requestBody.PromotionScan, requestBody.BId);
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
            var roleType = await _baseService.CurRoleType();
            if (roleType == -1) { return new PageResult<TrainingRecordSearch>(); }
            var rr = await _dbContext.Queryable<User>()
                          .LeftJoin<TrainingRecord>((x, y) => y.FillRepUserId == x.BusinessId)
                          .Where((x, y) => y.IsDelete == 1 && string.IsNullOrWhiteSpace(y.PId.ToString()))
                          .WhereIF(!string.IsNullOrWhiteSpace(requestBody.FillRepUserName), (x, y) => y.FillRepUserName.Contains(requestBody.FillRepUserName))
                          .WhereIF(!string.IsNullOrWhiteSpace(requestBody.StartTime.ToString()) && !string.IsNullOrWhiteSpace(requestBody.EndTime.ToString()), (x, y) => y.Created >= Convert.ToDateTime(requestBody.StartTime) && y.Created < Convert.ToDateTime(requestBody.EndTime.Value.AddDays(1)))
                          .LeftJoin<TrainingType>((x, y, z) => y.TrainingType == z.BusinessId.ToString())
                          .WhereIF(!string.IsNullOrWhiteSpace(requestBody.TraningType), (x, y, z) => requestBody.TraningType == y.TrainingType)
                          .WhereIF(roleType == 3, (x, y, z) => y.FillRepUserId == GlobalCurrentUser.UserBusinessId)
                          .Select((x, y, z) => new TrainingRecordSearch
                          {
                              Id = y.BusinessId.ToString(),
                              FillRepUserName = x.Name,
                              TrainingTypeName = z.Name,
                              FillReportTime = y.Created.Value.ToString("yyyy/MM/dd"),
                              TrainingAddress = y.TrainingAddress,
                              TrainingTime = y.TrainingTime.Value.ToString("yyyy/MM/dd"),
                              TrainingType = y.TrainingType,
                              TrainingTitle = y.TrainingTitle,
                              Scans = y.TrainingScan,
                              TrainingCount = SqlFunc.Subqueryable<TrainingRecord>().
                                         Where(z => z.PId == y.BusinessId).Count()
                          })
                          .ToPageListAsync(requestBody.PageIndex, requestBody.PageSize, total);
            var bids = rr.Select(x => x.Id).ToList();
            var uu = await _dbContext.Queryable<TrainingRecord>().Where(t => bids.Contains(t.PId.ToString())).ToListAsync();
            var uids = uu.Select(x => x.TrainingId).ToList();
            var users = await _dbContext.Queryable<User>().Where(t => t.IsDelete == 1 && t.IsLoginUser == 1 && uids.Contains(t.BusinessId)).ToListAsync();
            //扫描件
            var trainType = await _dbContext.Queryable<TrainingType>().Where(t => t.IsDelete == 1).Select(x => new { x.BusinessId, x.Name }).ToListAsync();
            //获取文件
            var files = await _dbContext.Queryable<Files>().Where(t => rr.Select(x => x.Scans).ToList().Contains(t.FileId)).ToListAsync();
            List<TrainingRecordsForDetails> td = new();
            //获取文件
            var url = AppsettingsHelper.GetValue("UpdateItem:Url");

            foreach (var item in rr)
            {
                var trFile = files.Where(x => item.Scans == x.FileId)
                   .Select(x => new FileInfosForDetails
                   {
                       Id = x.BusinessId.ToString(),
                       FileSize = x.FileSize,
                       FileType = x.FileType,
                       Name = x.Name,
                       OriginName = x.OriginName,
                       SuffixName = x.SuffixName,
                       //Url = url + x.Name.Substring(0, x.Name.LastIndexOf(".")) + x.OriginName
                       Url = url + x.Name
                   })
                   .ToList();
                item.TrainingRecordScans = trFile;
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
                await _baseService.InsertFileAsync(requestBody.Scans, GlobalCurrentUser.UserBusinessId);
            }
            var bid = GuidUtil.Next();
            var add = new TrainingRecord
            {
                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                BusinessId = bid,
                FillRepUserId = GlobalCurrentUser.UserBusinessId,
                FillRepUserName = GlobalCurrentUser.Name,
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
                    FillRepUserName = GlobalCurrentUser.Name,
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
            var rt = await _dbContext.Queryable<TrainingRecord>().Where(t => t.BusinessId.ToString() == requestBody.Id).FirstAsync();
            if (rt != null)
            {
                var rr = await _dbContext.Queryable<TrainingRecord>().Where(t => rt.BusinessId == t.BusinessId || t.PId == rt.BusinessId).ToListAsync();
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
        /// 年度考核列表
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        public async Task<PageResult<YearCheckSearch>> SearchYearCheckAsync(YearCheckRequest requestBody)
        {
            RefAsync<int> total = 0;
            var roleType = await _baseService.CurRoleType();
            if (roleType == -1) { return new PageResult<YearCheckSearch>(); }
            #region 船员关联
            //var uentityFist = _dbContext.Queryable<UserEntryInfo>()
            //    .GroupBy(u => u.UserEntryId)
            //    .Select(x => new { x.UserEntryId, EndTime = SqlFunc.AggregateMax(x.EndTime) });
            //var uentity = _dbContext.Queryable<UserEntryInfo>()
            //    .InnerJoin(uentityFist, (x, y) => x.UserEntryId == y.UserEntryId && x.EndTime == y.EndTime);
            var crewWorkShip = _dbContext.Queryable<WorkShip>().Where(t => t.IsDelete == 1)
                        .GroupBy(u => u.WorkShipId)
                        .Select(t => new { t.WorkShipId, WorkShipStartTime = SqlFunc.AggregateMax(t.WorkShipStartTime) });
            var wShip = _dbContext.Queryable<WorkShip>().Where(t => t.IsDelete == 1)
              .InnerJoin(crewWorkShip, (x, y) => x.WorkShipId == y.WorkShipId && x.WorkShipStartTime == y.WorkShipStartTime);
            #endregion

            var rr = await _dbContext.Queryable<User>()
                .Where(t1 => t1.IsLoginUser == 1 && t1.IsDelete == 1)
                .WhereIF(!string.IsNullOrEmpty(requestBody.KeyWords), t1 => t1.Name.Contains(requestBody.KeyWords) || t1.Phone.Contains(requestBody.KeyWords) || t1.WorkNumber.Contains(requestBody.KeyWords) || t1.CardId.Contains(requestBody.KeyWords))
                .LeftJoin(wShip, (t1, t5) => t1.BusinessId == t5.WorkShipId)
                .InnerJoin<OwnerShip>((t1, t5, t3) => t5.OnShip == t3.BusinessId.ToString())
                .WhereIF(roleType == 3, (t1, t5, t3) => t5.OnShip == t3.BusinessId.ToString() && GlobalCurrentUser.ShipId.ToString() == t5.OnShip)
                .LeftJoin<YearCheck>((t1, t5, t3, t6) => t1.BusinessId == t6.TrainingId)
                .WhereIF(requestBody.CheckStatus == 1, (t1, t5, t3, t6) => t6.CheckType != CheckEnum.Normal)
                .WhereIF(requestBody.CheckStatus == 2, (t1, t5, t3, t6) => t6.BusinessId == null)
                .WhereIF(!string.IsNullOrEmpty(requestBody.Year), (t1, t5, t3, t6) => t6.TrainingTime.Value.Year.ToString() == requestBody.Year)
                .Select((t1, t5, t3, t6) => new YearCheckSearch
                {
                    BId = t1.BusinessId.ToString(),
                    Id = t6.BusinessId.ToString(),
                    Country = t3.Country,
                    OnBoard = t5.OnShip,
                    ShipType = t3.ShipType,
                    UserName = t1.Name,
                    WorkNumber = t1.WorkNumber,
                    CardId = t1.CardId,
                    Scans = t6.TrainingScan,
                    CheckYear = t6.TrainingTime == null ? DateTime.Now.Year.ToString() : t6.TrainingTime.Value.Year.ToString(),
                    CheckType = t6.CheckType,
                    CheckFillRepTime = t6.Created.Value.ToString("yyyy/MM/dd HH:mm:ss"),
                    CheckTypeStatus = t6.CheckType != CheckEnum.Normal ? "已考核" : "未考核",
                    DeleteResonEnum = t1.DeleteReson,
                    //ContractType = t2.ContractType,
                    OnBoardPosition = t5.Postition,
                    WorkShipStartTime = t5.WorkShipStartTime,
                    WorkShipEndTime = t5.WorkShipEndTime
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
            var countryTable = await _dbContext.Queryable<CountryRegion>().Where(t => rr.Select(x => x.Country).Contains(t.BusinessId)).ToListAsync();
            //第一适任 第二适任
            var uIds = rr.Select(x => x.BId).ToList();
            var cerOfComps = await _dbContext.Queryable<CertificateOfCompetency>().Where(x => uIds.Contains(x.CertificateId.ToString()) && (x.Type == CertificatesEnum.FCertificate || x.Type == CertificatesEnum.SCertificate)).ToListAsync();
            //获取文件
            var fileIds = rr.Select(x => x.Scans).ToList();
            var files = await _dbContext.Queryable<Files>().Where(t => fileIds.Contains(t.FileId)).ToListAsync();
            //获取文件
            var url = AppsettingsHelper.GetValue("UpdateItem:Url");
            foreach (var u in rr)
            {
                u.TrainingScans = files.Where(x => u.Scans == x.FileId)
                    .Select(x => new FileInfosForDetails
                    {
                        Id = x.BusinessId.ToString(),
                        FileSize = x.FileSize,
                        FileType = x.FileType,
                        Name = x.Name,
                        OriginName = x.OriginName,
                        SuffixName = x.SuffixName,
                        //Url = url + x.Name?.Substring(0, x.Name.LastIndexOf(".")) + x.OriginName
                        Url = url + x.Name
                    }).ToList();
                u.CheckTypeName = EnumUtil.GetDescription(u.CheckType);
                u.OnStatus = EnumUtil.GetDescription(_baseService.ShipUserStatus(u.WorkShipStartTime, u.WorkShipEndTime, u.DeleteResonEnum));
                //u.ContractTypeName = EnumUtil.GetDescription(u.ContractType);
                u.OnBoardName = ownShipTable.FirstOrDefault(x => x.BusinessId.ToString() == u.OnBoard)?.ShipName;
                u.CountryName = countryTable.FirstOrDefault(x => x.BusinessId == u.Country)?.Name;
                u.ShipTypeName = EnumUtil.GetDescription(u.ShipType);
                u.Age = _baseService.CalculateAgeFromIdCard(u.CardId);
                if (cerOfComps != null)
                {
                    var cerofcF = cerOfComps.FirstOrDefault(x => x.Type == CertificatesEnum.FCertificate && x.CertificateId.ToString() == u.BId);
                    var cerofcS = cerOfComps.FirstOrDefault(x => x.Type == CertificatesEnum.SCertificate && x.CertificateId.ToString() == u.BId);
                    u.FPositionName = position.FirstOrDefault(x => x.BusinessId.ToString() == cerofcF?.FPosition)?.Name;
                    u.FPosition = cerofcF?.FPosition;
                    u.SPositionName = position.FirstOrDefault(x => x.BusinessId.ToString() == cerofcS?.SPosition)?.Name;
                    u.SPosition = cerofcS?.SPosition;
                }
                if (u.FPosition != null) u.FPositionName = position.FirstOrDefault(x => x.BusinessId.ToString() == u.FPosition)?.Name;
                if (u.SPosition != null) u.SPositionName = position.FirstOrDefault(x => x.BusinessId.ToString() == u.SPosition)?.Name;
                if (u.OnBoardPosition != null) u.OnBoardPositionName = position.FirstOrDefault(x => x.BusinessId.ToString() == u.OnBoardPosition)?.Name;
            }

            rt.List = rr.OrderByDescending(t => t.UserName).ThenByDescending(t => t.CheckYear);
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
            if (requestBody.BId == Guid.Empty || string.IsNullOrWhiteSpace(requestBody.BId.ToString()))
            {
                return Result.Fail("BId不能为空");
            }

            var rt = await _dbContext.Queryable<YearCheck>().Where(t => t.IsDelete == 1 && t.TrainingId == requestBody.BId && requestBody.Year == t.TrainingTime.Value.Year).FirstAsync();
            if (rt != null)
            {
                if (requestBody.Scans != null && requestBody.Scans.Any())
                {
                    rt.CheckType = requestBody.CheckType;
                    var files = await _dbContext.Queryable<Files>().Where(t => t.FileId == rt.TrainingScan).ToListAsync();
                    await _dbContext.Deleteable(files).ExecuteCommandAsync();
                    rt.TrainingScan = GuidUtil.Next();
                    requestBody.Scans.ForEach(x => x.FileId = rt.TrainingScan);
                    await _baseService.InsertFileAsync(requestBody.Scans, rt.TrainingId);
                    await _dbContext.Updateable(rt).UpdateColumns(x => new { x.CheckType, x.TrainingScan, x.ModifiedBy, x.Modified }).ExecuteCommandAsync();
                    return Result.Success("成功");
                }
                else
                {
                    return Result.Success("失败");
                }
            }
            else
            {
                //若无考核 则新增一条考核
                YearCheck yearCheck = new YearCheck();
                yearCheck.Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId();
                yearCheck.BusinessId = GuidUtil.Next();
                yearCheck.TrainingTime = new DateTime(requestBody.Year, 1, 1);
                yearCheck.CheckType = requestBody.CheckType;
                yearCheck.TrainingScan = GuidUtil.Next();
                yearCheck.TrainingId = requestBody.BId;
                yearCheck.BusinessId = GuidUtil.Next();
                if (requestBody.Scans != null && requestBody.Scans.Any())
                {
                    requestBody.Scans.ForEach(x => x.FileId = yearCheck.TrainingScan);
                    await _baseService.InsertFileAsync(requestBody.Scans, requestBody.UserId);
                }
                await _dbContext.Insertable(yearCheck).ExecuteCommandAsync();
                return Result.Success("成功");
            }
        }

        #endregion

        /// <summary>
        /// 提醒证书/合同统计数
        /// </summary>
        /// <returns></returns>
        public async Task<Result> RemindCountAsync()
        {
            RemindCountDto rt = new();
            var contractList = await SearchContractAsync(new ContractRequest() { PageIndex = 1, PageSize = 1000000 });
            var certificateList = await _certificateService.SearchCertificateAsync(new CertificateRequest() { PageIndex = 1, PageSize = 1000000 });

            rt.ContractCount = contractList.List == null ? 0 : contractList.List.Count();
            rt.CertificateCount = certificateList.List == null ? 0 : certificateList.List.Count();

            return Result.Success(rt);
        }
    }
}
