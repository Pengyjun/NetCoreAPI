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
    public class CertificateService : ICertificateService
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
                .LeftJoin<CertificateOfCompetency>((t1, t2) => t1.BusinessId == t2.CertificateId)
                .WhereIF(requestBody.Certificates == CertificatesEnum.FCertificate, (t1, t2) => !string.IsNullOrEmpty(t2.FCertificate))
                .WhereIF(requestBody.Certificates == CertificatesEnum.SCertificate, (t1, t2) => !string.IsNullOrEmpty(t2.SCertificate))
                .WhereIF(requestBody.Certificates == CertificatesEnum.PXHGZ, (t1, t2) => !string.IsNullOrEmpty(t2.TrainingCertificate))
                .WhereIF(requestBody.Certificates == CertificatesEnum.JKZ, (t1, t2) => !string.IsNullOrEmpty(t2.HealthCertificate))
                .WhereIF(requestBody.Certificates == CertificatesEnum.HYZ, (t1, t2) => !string.IsNullOrEmpty(t2.SeamanCertificate))
                .WhereIF(requestBody.Certificates == CertificatesEnum.HZ, (t1, t2) => !string.IsNullOrEmpty(t2.PassportCertificate))
                .Where(t1 => t1.IsLoginUser == 1 && t1.IsDelete == 1)
                .WhereIF(!string.IsNullOrEmpty(requestBody.KeyWords), t1 => requestBody.KeyWords.Contains(t1.Name) || requestBody.KeyWords.Contains(t1.Phone) || requestBody.KeyWords.Contains(t1.WorkNumber) || requestBody.KeyWords.Contains(t1.CardId))
                .LeftJoin(uentity, (t1, t2, t3) => t1.BusinessId == t3.UserEntryId)
                .InnerJoin<OwnerShip>((t1, t2, t3, t4) => t1.OnBoard == t4.BusinessId.ToString())
                .InnerJoin(wShip, (t1, t2, t3, t4, t5) => t1.BusinessId == t5.WorkShipId)
                .Select((t1, t2, t3, t4, t5) => new CertificateSearch
                {
                    BId = t1.BusinessId.ToString(),
                    Id = t2.BusinessId.ToString(),
                    Country = t4.Country,
                    OnBoard = t1.OnBoard,
                    ShipType = t4.ShipType,
                    UserName = t1.Name,
                    WorkNumber = t1.WorkNumber,
                    FPosition = t2.FPosition,
                    SPosition = t2.SPosition,
                    OnBoardPosition = t5.Postition,
                    DeleteResonEnum = t1.DeleteReson,
                    WorkShipStartTime = t5.WorkShipStartTime,
                    CardId = t1.CardId
                })
                .ToPageListAsync(requestBody.PageIndex, requestBody.PageSize, total);
            return await GetResultAsync(rr, total, requestBody.Certificates);
        }
        /// <summary>
        /// 获取查询结果集
        /// </summary>
        /// <param name="rr"></param>
        /// <param name="total"></param>
        /// <param name="certificates"></param>
        /// <returns></returns>
        private async Task<PageResult<CertificateSearch>> GetResultAsync(List<CertificateSearch> rr, int total, CertificatesEnum certificates)
        {
            PageResult<CertificateSearch> rt = new();

            var position = await _dbContext.Queryable<Position>().Where(t => t.IsDelete == 1).ToListAsync();
            var ownShipTable = await _dbContext.Queryable<OwnerShip>().Where(t => rr.Select(x => x.OnBoard).Contains(t.BusinessId.ToString())).ToListAsync();
            var countryTable = await _dbContext.Queryable<CountryRegion>().Where(t => rr.Select(x => x.Country).Contains(t.BusinessId.ToString())).ToListAsync();
            var certificateOfCompetency = await _dbContext.Queryable<CertificateOfCompetency>().Where(t => rr.Select(x => x.BId).ToList().Contains(t.CertificateId.ToString())).ToListAsync();
            foreach (var u in rr)
            {
                var rs = certificateOfCompetency.FirstOrDefault(x => x.CertificateId.ToString() == u.BId);
                switch (certificates)
                {
                    case CertificatesEnum.FCertificate:
                        u.EffectiveTime = rs?.FEffectiveTime?.ToString("yyyy/MM/dd");
                        u.DueDays = TimeHelper.GetTimeSpan(Convert.ToDateTime(rs?.FEffectiveTime), DateTime.Now).Days + 1;
                        break;
                    case CertificatesEnum.SCertificate:
                        u.EffectiveTime = rs?.SEffectiveTime?.ToString("yyyy/MM/dd");
                        u.DueDays = TimeHelper.GetTimeSpan(Convert.ToDateTime(rs?.SEffectiveTime), DateTime.Now).Days + 1;
                        break;
                    case CertificatesEnum.PXHGZ:
                        u.EffectiveTime = rs?.TrainingSignTime?.ToString("yyyy/MM/dd");
                        u.DueDays = TimeHelper.GetTimeSpan(Convert.ToDateTime(rs?.TrainingSignTime), DateTime.Now).Days + 1;
                        break;
                    case CertificatesEnum.JKZ:
                        u.EffectiveTime = rs?.HealthEffectiveTime?.ToString("yyyy/MM/dd");
                        u.DueDays = TimeHelper.GetTimeSpan(Convert.ToDateTime(rs?.HealthEffectiveTime), DateTime.Now).Days + 1;
                        break;
                    case CertificatesEnum.HYZ:
                        u.EffectiveTime = rs?.SeamanEffectiveTime?.ToString("yyyy/MM/dd");
                        u.DueDays = TimeHelper.GetTimeSpan(Convert.ToDateTime(rs?.SeamanEffectiveTime), DateTime.Now).Days + 1;
                        break;
                    case CertificatesEnum.HZ:
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
            }

            rt.List = rr;
            rt.TotalCount = total;
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

                        break;
                    case CertificatesEnum.SCertificate:

                        break;
                    case CertificatesEnum.PXHGZ:

                        break;
                    case CertificatesEnum.JKZ:

                        break;
                    case CertificatesEnum.HYZ:

                        break;
                    case CertificatesEnum.HZ:

                        break;
                }

            }
            return Result.Success();
        }

        #endregion
    }
}
