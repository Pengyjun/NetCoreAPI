using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Dtos;
using HNKC.CrewManagePlatform.Models.Dtos.Contract;
using HNKC.CrewManagePlatform.Models.Enums;
using HNKC.CrewManagePlatform.SqlSugars.Models;
using HNKC.CrewManagePlatform.Utils;
using SqlSugar;
using UtilsSharp;

namespace HNKC.CrewManagePlatform.Services.Interface.Certificate
{
    /// <summary>
    /// 证书
    /// </summary>
    public class CertificateService : HNKC.CrewManagePlatform.Services.Interface.CurrentUser.CurrentUserService, ICertificateService
    {
        private readonly ISqlSugarClient _dbContext;
        private readonly IBaseService _baseService;
        /// <summary>
        /// 注入
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="baseService"></param>
        public CertificateService(ISqlSugarClient dbContext, IBaseService baseService)
        {
            this._dbContext = dbContext;
            _baseService = baseService;
        }

        #region 证书
        /// <summary>
        /// 证书列表
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        public async Task<PageResult<CertificateSearch>> SearchCertificateAsync(CertificateRequest requestBody)
        {
            RefAsync<int> total = 0;
            var roleType = await _baseService.CurRoleType();
            if (roleType == -1) { return new PageResult<CertificateSearch>(); }
            var onShips = await _dbContext.Queryable<WorkShip>().Where(t => t.IsDelete == 1 && GlobalCurrentUser.UserBusinessId == t.WorkShipId).Select(x => x.OnShip).ToListAsync();
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
            var remindSet = await _dbContext.Queryable<RemindSetting>().Where(t => t.RemindType == 22 && t.IsDelete == 1).ToListAsync();
            if (remindSet.Count == 0) return new PageResult<CertificateSearch>();
            var rr = await _dbContext.Queryable<User>()
                .LeftJoin<CertificateOfCompetency>((t1, t2) => t1.BusinessId == t2.CertificateId)
                .WhereIF(requestBody.Certificates == CertificatesEnum.FCertificate, (t1, t2) => t2.Type == CertificatesEnum.FCertificate)
                .WhereIF(requestBody.Certificates == CertificatesEnum.SCertificate, (t1, t2) => t2.Type == CertificatesEnum.SCertificate)
                .WhereIF(requestBody.Certificates == CertificatesEnum.PXHGZ, (t1, t2) => t2.Type == CertificatesEnum.PXHGZ)
                .WhereIF(requestBody.Certificates == CertificatesEnum.JKZ, (t1, t2) => t2.Type == CertificatesEnum.JKZ)
                .WhereIF(requestBody.Certificates == CertificatesEnum.HYZ, (t1, t2) => t2.Type == CertificatesEnum.HYZ)
                .WhereIF(requestBody.Certificates == CertificatesEnum.HZ, (t1, t2) => t2.Type == CertificatesEnum.HZ)
                .Where(t1 => t1.IsLoginUser == 1 && t1.IsDelete == 1)
                .WhereIF(!string.IsNullOrEmpty(requestBody.KeyWords), t1 => requestBody.KeyWords.Contains(t1.Name) || requestBody.KeyWords.Contains(t1.Phone) || requestBody.KeyWords.Contains(t1.WorkNumber) || requestBody.KeyWords.Contains(t1.CardId))
                .LeftJoin(uentity, (t1, t2, t3) => t1.BusinessId == t3.UserEntryId)
                .InnerJoin(wShip, (t1, t2, t3, t5) => t1.BusinessId == t5.WorkShipId)
                .InnerJoin<OwnerShip>((t1, t2, t3, t5, t4) => t5.OnShip == t4.BusinessId.ToString())
                .WhereIF(roleType == 3, (t1, t2, t3, t5, t4) => t5.OnShip == t3.BusinessId.ToString() && onShips.Contains(t5.OnShip))//船长
                .WhereIF(roleType == 2, (t1, t2, t3, t5, t4) => GlobalCurrentUser.UserBusinessId == t5.WorkShipId)//船员
                .InnerJoin<RemindSetting>((t1, t2, t3, t5, t4, t6) => t6.Types == t2.Type && t6.RemindType == 22)
                .Select((t1, t2, t3, t5, t4, t6) => new CertificateSearch
                {
                    Id = t2.BusinessId.ToString(),
                    BId = t1.BusinessId.ToString(),
                    Country = t4.Country,
                    OnBoard = t5.OnShip,
                    ShipType = t4.ShipType,
                    UserName = t1.Name,
                    WorkNumber = t1.WorkNumber,
                    OnBoardPosition = t5.Postition,
                    DeleteResonEnum = t1.DeleteReson,
                    WorkShipStartTime = t5.WorkShipStartTime,
                    CardId = t1.CardId
                })
                .Distinct()
                .ToListAsync();
            return await GetResultAsync(rr, requestBody.PageIndex, requestBody.PageSize);
        }
        /// <summary>
        /// 获取查询结果集
        /// </summary>
        /// <param name="rr"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        private async Task<PageResult<CertificateSearch>> GetResultAsync(List<CertificateSearch> rr, int pageIndex, int pageSize)
        {
            PageResult<CertificateSearch> rt = new();
            List<CertificateSearch> rList = new();

            var position = await _dbContext.Queryable<Position>().Where(t => t.IsDelete == 1).ToListAsync();
            var ownShipTable = await _dbContext.Queryable<OwnerShip>().Where(t => rr.Select(x => x.OnBoard).Contains(t.BusinessId.ToString())).ToListAsync();
            var countryTable = await _dbContext.Queryable<CountryRegion>().Where(t => rr.Select(x => x.Country).Contains(t.BusinessId.ToString())).ToListAsync();
            var certificateOfCompetency = await _dbContext.Queryable<CertificateOfCompetency>().Where(t => rr.Select(x => x.BId).ToList().Contains(t.CertificateId.ToString())).ToListAsync();
            foreach (var u in rr)
            {
                var rs = certificateOfCompetency.FirstOrDefault(x => x.CertificateId.ToString() == u.BId && u.Id == x.BusinessId.ToString());
                if (rs != null)
                {
                    switch (rs.Type)
                    {
                        case CertificatesEnum.FCertificate:
                            u.CertificateType = CertificatesEnum.FCertificate;
                            u.CertificateTypeName = EnumUtil.GetDescription(CertificatesEnum.FCertificate);
                            if (rs?.FEffectiveTime.HasValue == false) continue;
                            u.EffectiveTime = rs?.FEffectiveTime?.ToString("yyyy/MM/dd");
                            u.DueDays = TimeHelper.GetTimeSpan(Convert.ToDateTime(rs?.FEffectiveTime), DateTime.Now).Days + 1;
                            break;
                        case CertificatesEnum.SCertificate:
                            u.CertificateType = CertificatesEnum.SCertificate;
                            u.CertificateTypeName = EnumUtil.GetDescription(CertificatesEnum.SCertificate);
                            if (rs?.SEffectiveTime.HasValue == false) continue;
                            u.EffectiveTime = rs?.SEffectiveTime?.ToString("yyyy/MM/dd");
                            u.DueDays = TimeHelper.GetTimeSpan(Convert.ToDateTime(rs?.SEffectiveTime), DateTime.Now).Days + 1;
                            break;
                        case CertificatesEnum.PXHGZ:
                            u.CertificateType = CertificatesEnum.PXHGZ;
                            u.CertificateTypeName = EnumUtil.GetDescription(CertificatesEnum.PXHGZ);
                            if (rs?.TrainingSignTime.HasValue == false) continue;
                            u.EffectiveTime = rs?.TrainingSignTime?.ToString("yyyy/MM/dd");
                            u.DueDays = TimeHelper.GetTimeSpan(Convert.ToDateTime(rs?.TrainingSignTime), DateTime.Now).Days + 1;
                            break;
                        case CertificatesEnum.JKZ:
                            u.CertificateType = CertificatesEnum.JKZ;
                            u.CertificateTypeName = EnumUtil.GetDescription(CertificatesEnum.JKZ);
                            if (rs?.HealthEffectiveTime.HasValue == false) continue;
                            u.EffectiveTime = rs?.HealthEffectiveTime?.ToString("yyyy/MM/dd");
                            u.DueDays = TimeHelper.GetTimeSpan(Convert.ToDateTime(rs?.HealthEffectiveTime), DateTime.Now).Days + 1;
                            break;
                        case CertificatesEnum.HYZ:
                            u.CertificateType = CertificatesEnum.HYZ;
                            u.CertificateTypeName = EnumUtil.GetDescription(CertificatesEnum.HYZ);
                            if (rs?.SeamanEffectiveTime.HasValue == false) continue;
                            u.EffectiveTime = rs?.SeamanEffectiveTime?.ToString("yyyy/MM/dd");
                            u.DueDays = TimeHelper.GetTimeSpan(Convert.ToDateTime(rs?.SeamanEffectiveTime), DateTime.Now).Days + 1;
                            break;
                        case CertificatesEnum.HZ:
                            u.CertificateType = CertificatesEnum.HZ;
                            u.CertificateTypeName = EnumUtil.GetDescription(CertificatesEnum.HZ);
                            if (rs?.PassportEffectiveTime.HasValue == false) continue;
                            u.EffectiveTime = rs?.PassportEffectiveTime?.ToString("yyyy/MM/dd");
                            u.DueDays = TimeHelper.GetTimeSpan(Convert.ToDateTime(rs?.PassportEffectiveTime), DateTime.Now).Days + 1;
                            break;
                    }
                    u.OnStatus = EnumUtil.GetDescription(_baseService.ShipUserStatus(u.WorkShipStartTime, u.DeleteResonEnum));
                    u.OnBoardName = ownShipTable.FirstOrDefault(x => x.BusinessId.ToString() == u.OnBoard)?.ShipName;
                    u.CountryName = countryTable.FirstOrDefault(x => x.BusinessId.ToString() == u.Country)?.Name;
                    u.ShipTypeName = EnumUtil.GetDescription(u.ShipType);
                    u.Age = _baseService.CalculateAgeFromIdCard(u.CardId);
                    if (u.FPosition != null) u.FPositionName = position.FirstOrDefault(x => x.BusinessId.ToString() == u.FPosition)?.Name;
                    if (u.SPosition != null) u.SPositionName = position.FirstOrDefault(x => x.BusinessId.ToString() == u.SPosition)?.Name;
                    if (u.OnBoardPosition != null) u.OnBoardPositionName = position.FirstOrDefault(x => x.BusinessId.ToString() == u.OnBoardPosition)?.Name;
                    rList.Add(u);
                }
            }

            rt.List = rList.Skip((pageIndex - 1) * pageSize).Take(pageSize).OrderBy(x => x.DueDays);
            rt.TotalCount = rList.Count();
            return rt;
        }
        #endregion

        #region 续签
        /// <summary>
        /// 证书续签
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        public async Task<Result> SaveCertificateAsync(CertificateRenewal requestBody)
        {
            var cerFirst = await _dbContext.Queryable<CertificateOfCompetency>().Where(t => t.CertificateId.ToString() == requestBody.BId).FirstAsync();

            if (cerFirst != null)
            {
                switch (requestBody.Certificates)
                {
                    case CertificatesEnum.FCertificate:
                        cerFirst.FCertificate = requestBody.FCertificate;
                        cerFirst.FNavigationArea = requestBody.FNavigationArea;
                        cerFirst.FPosition = requestBody.FPosition;
                        cerFirst.FSignTime = requestBody.FSignTime;
                        cerFirst.FEffectiveTime = requestBody.FEffectiveTime;
                        if (requestBody.FScans != null && requestBody.FScans.Any())
                        {
                            var files = await _dbContext.Queryable<Files>().Where(t => t.FileId == cerFirst.FScans).ToListAsync();
                            await _dbContext.Deleteable(files).ExecuteCommandAsync();
                            cerFirst.FScans = GuidUtil.Next();
                            requestBody.FScans.ForEach(x => x.FileId = cerFirst.FScans);
                            await _baseService.UpdateFileAsync(requestBody.FScans, Guid.Parse(requestBody.BId));
                        }
                        await _dbContext.Updateable(cerFirst).UpdateColumns(x => new
                        {
                            x.FCertificate,
                            x.FNavigationArea,
                            x.FPosition,
                            x.FSignTime,
                            x.FScans,
                            x.FEffectiveTime
                        }).ExecuteCommandAsync();
                        break;
                    case CertificatesEnum.SCertificate:
                        cerFirst.SCertificate = requestBody.SCertificate;
                        cerFirst.SNavigationArea = requestBody.SNavigationArea;
                        cerFirst.SPosition = requestBody.SPosition;
                        cerFirst.SSignTime = requestBody.SSignTime;
                        cerFirst.SEffectiveTime = requestBody.SEffectiveTime;
                        if (requestBody.SScans != null && requestBody.SScans.Any())
                        {
                            var files = await _dbContext.Queryable<Files>().Where(t => t.FileId == cerFirst.SScans).ToListAsync();
                            await _dbContext.Deleteable(files).ExecuteCommandAsync();
                            cerFirst.SScans = GuidUtil.Next();
                            requestBody.SScans.ForEach(x => x.FileId = cerFirst.SScans);
                            await _baseService.UpdateFileAsync(requestBody.SScans, Guid.Parse(requestBody.BId));
                        }
                        await _dbContext.Updateable(cerFirst).UpdateColumns(x => new
                        {
                            x.SCertificate,
                            x.SNavigationArea,
                            x.SPosition,
                            x.SSignTime,
                            x.SScans,
                            x.SEffectiveTime
                        }).ExecuteCommandAsync();
                        break;
                    case CertificatesEnum.PXHGZ:
                        cerFirst.TrainingCertificate = requestBody.TrainingCertificate;
                        cerFirst.TrainingSignTime = requestBody.TrainingSignTime;
                        cerFirst.Z01EffectiveTime = requestBody.Z01EffectiveTime;
                        cerFirst.Z07EffectiveTime = requestBody.Z07EffectiveTime;
                        cerFirst.Z08EffectiveTime = requestBody.Z08EffectiveTime;
                        cerFirst.Z04EffectiveTime = requestBody.Z04EffectiveTime;
                        cerFirst.Z05EffectiveTime = requestBody.Z05EffectiveTime;
                        cerFirst.Z02EffectiveTime = requestBody.Z02EffectiveTime;
                        cerFirst.Z06EffectiveTime = requestBody.Z06EffectiveTime;
                        cerFirst.Z09EffectiveTime = requestBody.Z09EffectiveTime;
                        if (requestBody.TrainingScans != null && requestBody.TrainingScans.Any())
                        {
                            var files = await _dbContext.Queryable<Files>().Where(t => t.FileId == cerFirst.SScans).ToListAsync();
                            await _dbContext.Deleteable(files).ExecuteCommandAsync();
                            cerFirst.TrainingScans = GuidUtil.Next();
                            requestBody.TrainingScans.ForEach(x => x.FileId = cerFirst.TrainingScans);
                            await _baseService.UpdateFileAsync(requestBody.TrainingScans, Guid.Parse(requestBody.BId));
                        }
                        await _dbContext.Updateable(cerFirst).UpdateColumns(x => new
                        {
                            x.TrainingCertificate,
                            x.TrainingSignTime,
                            x.Z01EffectiveTime,
                            x.Z07EffectiveTime,
                            x.Z04EffectiveTime,
                            x.Z05EffectiveTime,
                            x.Z02EffectiveTime,
                            x.Z06EffectiveTime,
                            x.Z09EffectiveTime,
                            x.Z08EffectiveTime,
                            x.TrainingScans
                        }).ExecuteCommandAsync();
                        break;
                    case CertificatesEnum.JKZ:
                        cerFirst.HealthCertificate = requestBody.HealthCertificate;
                        cerFirst.HealthSignTime = requestBody.HealthSignTime;
                        cerFirst.HealthEffectiveTime = requestBody.HealthEffectiveTime;
                        if (requestBody.HealthScans != null && requestBody.HealthScans.Any())
                        {
                            var files = await _dbContext.Queryable<Files>().Where(t => t.FileId == cerFirst.HealthScans).ToListAsync();
                            await _dbContext.Deleteable(files).ExecuteCommandAsync();
                            cerFirst.HealthScans = GuidUtil.Next();
                            requestBody.HealthScans.ForEach(x => x.FileId = cerFirst.HealthScans);
                            await _baseService.UpdateFileAsync(requestBody.HealthScans, Guid.Parse(requestBody.BId));
                        }
                        await _dbContext.Updateable(cerFirst).UpdateColumns(x => new
                        {
                            x.HealthCertificate,
                            x.HealthSignTime,
                            x.HealthEffectiveTime,
                            x.HealthScans
                        }).ExecuteCommandAsync();
                        break;
                    case CertificatesEnum.HYZ:
                        cerFirst.SeamanCertificate = requestBody.HealthCertificate;
                        cerFirst.SeamanSignTime = requestBody.HealthSignTime;
                        cerFirst.SeamanEffectiveTime = requestBody.HealthEffectiveTime;
                        if (requestBody.SeamanScans != null && requestBody.SeamanScans.Any())
                        {
                            var files = await _dbContext.Queryable<Files>().Where(t => t.FileId == cerFirst.SeamanScans).ToListAsync();
                            await _dbContext.Deleteable(files).ExecuteCommandAsync();
                            cerFirst.SeamanScans = GuidUtil.Next();
                            requestBody.SeamanScans.ForEach(x => x.FileId = cerFirst.SeamanScans);
                            await _baseService.UpdateFileAsync(requestBody.SeamanScans, Guid.Parse(requestBody.BId));
                        }
                        await _dbContext.Updateable(cerFirst).UpdateColumns(x => new
                        {
                            x.SeamanCertificate,
                            x.SeamanSignTime,
                            x.SeamanEffectiveTime,
                            x.SeamanScans
                        }).ExecuteCommandAsync();
                        break;
                    case CertificatesEnum.HZ:
                        cerFirst.PassportCertificate = requestBody.PassportCertificate;
                        cerFirst.PassportSignTime = requestBody.PassportSignTime;
                        cerFirst.PassportEffectiveTime = requestBody.PassportEffectiveTime;
                        if (requestBody.PassportScans != null && requestBody.PassportScans.Any())
                        {
                            var files = await _dbContext.Queryable<Files>().Where(t => t.FileId == cerFirst.PassportScans).ToListAsync();
                            await _dbContext.Deleteable(files).ExecuteCommandAsync();
                            cerFirst.PassportScans = GuidUtil.Next();
                            requestBody.PassportScans.ForEach(x => x.FileId = cerFirst.PassportScans);
                            await _baseService.UpdateFileAsync(requestBody.PassportScans, Guid.Parse(requestBody.BId));
                        }
                        await _dbContext.Updateable(cerFirst).UpdateColumns(x => new
                        {
                            x.PassportCertificate,
                            x.PassportSignTime,
                            x.PassportEffectiveTime,
                            x.PassportScans
                        }).ExecuteCommandAsync();
                        break;
                }
            }
            return Result.Success("续签成功");
        }

        #endregion
    }
}
