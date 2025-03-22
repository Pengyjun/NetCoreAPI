using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Dtos;
using HNKC.CrewManagePlatform.Models.Dtos.CrewArchives;
using HNKC.CrewManagePlatform.Models.Enums;
using HNKC.CrewManagePlatform.SqlSugars.Models;
using HNKC.CrewManagePlatform.Utils;
using SqlSugar;
using System.ComponentModel;
using UtilsSharp;

namespace HNKC.CrewManagePlatform.Services.Interface.CrewArchives
{
    /// <summary>
    /// 船员档案
    /// </summary>
    public class CrewArchivesService : HNKC.CrewManagePlatform.Services.Interface.CurrentUser.CurrentUserService, ICrewArchivesService
    {
        private ISqlSugarClient _dbContext { get; set; }
        private IBaseService _baseService { get; set; }
        /// <summary>
        /// 注入
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="baseService"></param>
        public CrewArchivesService(ISqlSugarClient dbContext, IBaseService baseService)
        {
            this._dbContext = dbContext;
            _baseService = baseService;
        }
        #region 船员档案列表
        /// <summary>
        /// 船员档案列表
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        public async Task<PageResult<SearchCrewArchivesResponse>> SearchCrewArchivesAsync(SearchCrewArchivesRequest requestBody)
        {
            RefAsync<int> total = 0;
            var roleType = await _baseService.CurRoleType();
            if (roleType == -1) { return new PageResult<SearchCrewArchivesResponse>(); }

            #region 船员关联
            //任职船舶
            var crewWorkShip = _dbContext.Queryable<WorkShip>()
                      .GroupBy(u => new { u.WorkShipId, u.OnShip })
                      .Select(t => new { t.WorkShipId, t.OnShip, WorkShipStartTime = SqlFunc.AggregateMax(t.WorkShipStartTime) });
            var wShip = _dbContext.Queryable<WorkShip>()
              .InnerJoin(crewWorkShip, (x, y) => x.WorkShipId == y.WorkShipId && x.OnShip == y.OnShip && x.WorkShipStartTime == y.WorkShipStartTime);
            //入职时间
            var uentityFist = _dbContext.Queryable<UserEntryInfo>()
                    .GroupBy(u => u.UserEntryId)
                    .Select(x => new { x.UserEntryId, StartTime = SqlFunc.AggregateMax(x.StartTime) });
            var uentity = _dbContext.Queryable<UserEntryInfo>()
                .InnerJoin(uentityFist, (x, y) => x.UserEntryId == y.UserEntryId && x.StartTime == y.StartTime);
            #endregion

            // 转义 % 和 _
            string escapedKeyword = requestBody.KeyWords == null ? string.Empty : requestBody.KeyWords.Replace("%", "\\%").Replace("_", "\\_");

            //名称相关不赋值
            var rt = await _dbContext.Queryable<User>()
                .Where(t => t.IsLoginUser == 1)
                .WhereIF(!string.IsNullOrWhiteSpace(requestBody.KeyWords), t => t.Name.Contains(escapedKeyword) || t.CardId.Contains(escapedKeyword)
                || t.Phone.Contains(escapedKeyword) || t.WorkNumber.Contains(escapedKeyword))
                .WhereIF(!string.IsNullOrWhiteSpace(requestBody.Name), t => t.Name.Contains(requestBody.Name))
                .WhereIF(!string.IsNullOrWhiteSpace(requestBody.CardId), t => t.CardId.Contains(requestBody.CardId))
                .WhereIF(!string.IsNullOrWhiteSpace(requestBody.WorkNumber), t => t.WorkNumber.Contains(requestBody.WorkNumber))
                .WhereIF(!string.IsNullOrWhiteSpace(requestBody.Phone), t => t.Phone.Contains(requestBody.Phone))
                .LeftJoin(wShip, (t, ws) => t.BusinessId == ws.WorkShipId)
                .WhereIF(!string.IsNullOrWhiteSpace(requestBody.OnBoard), (t, ws) => ws.OnShip == requestBody.OnBoard)//所在船舶
                .WhereIF(!string.IsNullOrWhiteSpace(requestBody.HistoryOnBoard), (t, ws) => ws.OnShip == requestBody.HistoryOnBoard)//履历船舶
                .WhereIF(!string.IsNullOrWhiteSpace(requestBody.HistoryProject), (t, ws) => ws.ProjectName.Contains(requestBody.HistoryProject))
                .WhereIF(!string.IsNullOrWhiteSpace(requestBody.HistoryCountry.ToString()), (t, ws) => ws.Country == requestBody.HistoryCountry)
                .LeftJoin<PositionOnBoard>((t, ws, pob) => ws.Postition == pob.BusinessId.ToString())
                .LeftJoin<OwnerShip>((t, ws, pob, ow) => ws.OnShip == ow.BusinessId.ToString())
                .LeftJoin(uentity, (t, ws, pob, ow, ue) => t.BusinessId == ue.UserEntryId)
                .WhereIF(roleType == 3, (t, ws, pob, ow, ue) => ws.OnShip == GlobalCurrentUser.ShipId)//船长
                .LeftJoin<SkillCertificates>((t, ws, pob, ow, ue, sc) => sc.SkillcertificateId == t.BusinessId)
                .LeftJoin<EducationalBackground>((t, ws, pob, ow, ue, sc, eb) => eb.QualificationId == t.BusinessId)
                .WhereIF(requestBody.ServiceBooks != null && requestBody.ServiceBooks.Any(),
                (t, ws, pob, ow, ue, sc, eb) => requestBody.ServiceBooks.Contains(((int)t.ServiceBookType).ToString()))//服务簿类型
                .WhereIF(requestBody.CertificateTypes != null && requestBody.CertificateTypes.Any(), (t, ws, pob, ow, ue, sc, eb) => requestBody.CertificateTypes.Contains(sc.SkillCertificateType.ToString()))
                .WhereIF(requestBody.QualificationTypes != null && requestBody.QualificationTypes.Any(), (t, ws, pob, ow, ue, sc, eb) => requestBody.QualificationTypes.Contains(eb.QualificationType.ToString()))
                .WhereIF(requestBody.Qualifications != null && requestBody.Qualifications.Any(), (t, ws, pob, ow, ue, sc, eb) => requestBody.Qualifications.Contains(eb.Qualification.ToString()))
                .WhereIF(requestBody.Staus != null && requestBody.Staus.Any(), (t, ws, pob, ow, ue, sc, eb) =>
                 (requestBody.Staus.Contains("0") && DateTime.Now < ws.WorkShipEndTime && t.DeleteReson == 0) || // 在岗
                 (requestBody.Staus.Contains("1") && (int)t.DeleteReson == 1) || // 离职
                 (requestBody.Staus.Contains("2") && (int)t.DeleteReson == 2) || // 调离
                 (requestBody.Staus.Contains("3") && (int)t.DeleteReson == 3) || // 待岗
                 (requestBody.Staus.Contains("5") && DateTime.Now >= ws.WorkShipEndTime)) // 待岗
                .WhereIF(requestBody.CrewType != null && requestBody.CrewType.Any(), (t, ws, pob, ow, ue, sc, eb) =>
                 requestBody.CrewType.Contains(pob.Type.ToString()))
                .WhereIF(requestBody.FPosition != null && requestBody.FPosition.Any(), (t, ws, pob, ow, ue, sc, eb) => SqlFunc.Subqueryable<CertificateOfCompetency>()//第一适任证
                               .Where(skcall => requestBody.FPosition.Contains(skcall.FPosition) && t.BusinessId == skcall.CertificateId).Any())
                .WhereIF(requestBody.SPosition != null && requestBody.SPosition.Any(), (t, ws, pob, ow, ue, sc, eb) => SqlFunc.Subqueryable<CertificateOfCompetency>()//第二适任证
                               .Where(skcall => requestBody.SPosition.Contains(skcall.SPosition.ToString()) && t.BusinessId == skcall.CertificateId).Any())
                .WhereIF(requestBody.PositionOnBoard != null, (t, ws, pob, ow, ue, sc, eb) => requestBody.PositionOnBoard == ws.Postition)
                .WhereIF(requestBody.TrainingCertificate, (t, ws, pob, ow, ue, sc, eb) => SqlFunc.Subqueryable<CertificateOfCompetency>()
                               .Where(skcall => skcall.Type == CertificatesEnum.PXHGZ && t.BusinessId == skcall.CertificateId).Any())
                .WhereIF(requestBody.Z01Effective, (t, ws, pob, ow, ue, sc, eb) => SqlFunc.Subqueryable<CertificateOfCompetency>()
                               .Where(skcall => !string.IsNullOrEmpty(skcall.Z01EffectiveTime.ToString()) && t.BusinessId == skcall.CertificateId).Any())
                .WhereIF(requestBody.Z08Effective, (t, ws, pob, ow, ue, sc, eb) => SqlFunc.Subqueryable<CertificateOfCompetency>()
                               .Where(skcall => !string.IsNullOrEmpty(skcall.Z08EffectiveTime.ToString()) && t.BusinessId == skcall.CertificateId).Any())
                .WhereIF(requestBody.Z04Effective, (t, ws, pob, ow, ue, sc, eb) => SqlFunc.Subqueryable<CertificateOfCompetency>()
                               .Where(skcall => !string.IsNullOrEmpty(skcall.Z04EffectiveTime.ToString()) && t.BusinessId == skcall.CertificateId).Any())
                .WhereIF(requestBody.Z05Effective, (t, ws, pob, ow, ue, sc, eb) => SqlFunc.Subqueryable<CertificateOfCompetency>()
                               .Where(skcall => !string.IsNullOrEmpty(skcall.Z05EffectiveTime.ToString()) && t.BusinessId == skcall.CertificateId).Any())
                .WhereIF(requestBody.Z02Effective, (t, ws, pob, ow, ue, sc, eb) => SqlFunc.Subqueryable<CertificateOfCompetency>()
                               .Where(skcall => !string.IsNullOrEmpty(skcall.Z02EffectiveTime.ToString()) && t.BusinessId == skcall.CertificateId).Any())
                .WhereIF(requestBody.Z06Effective, (t, ws, pob, ow, ue, sc, eb) => SqlFunc.Subqueryable<CertificateOfCompetency>()
                               .Where(skcall => !string.IsNullOrEmpty(skcall.Z06EffectiveTime.ToString()) && t.BusinessId == skcall.CertificateId).Any())
                .WhereIF(requestBody.Z09Effective, (t, ws, pob, ow, ue, sc, eb) => SqlFunc.Subqueryable<CertificateOfCompetency>()
                               .Where(skcall => !string.IsNullOrEmpty(skcall.Z09EffectiveTime.ToString()) && t.BusinessId == skcall.CertificateId).Any())
                .WhereIF(requestBody.SeamanCertificate, (t, ws, pob, ow, ue, sc, eb) => SqlFunc.Subqueryable<CertificateOfCompetency>()
                               .Where(skcall => skcall.Type == CertificatesEnum.HYZ && t.BusinessId == skcall.CertificateId).Any())
                .WhereIF(requestBody.PassportCertificate, (t, ws, pob, ow, ue, sc, eb) => SqlFunc.Subqueryable<CertificateOfCompetency>()
                               .Where(skcall => skcall.Type == CertificatesEnum.HZ && t.BusinessId == skcall.CertificateId).Any())
                .WhereIF(requestBody.HealthCertificate, (t, ws, pob, ow, ue, sc, eb) => SqlFunc.Subqueryable<CertificateOfCompetency>()
                               .Where(skcall => skcall.Type == CertificatesEnum.JKZ && t.BusinessId == skcall.CertificateId).Any())
                .WhereIF(requestBody.ShipTypes != null && requestBody.ShipTypes.Any(), (t, ws, pob, ow, ue, sc, eb) => SqlFunc.Subqueryable<OwnerShip>()
                               .Where(skcall => requestBody.ShipTypes.Contains(((int)ow.ShipType).ToString())).Any())//船舶类型
                .Select((t, ws, pob, ow, ue, sc, eb) => new SearchCrewArchivesResponse
                {
                    BId = t.BusinessId,
                    BtnType = t.IsDelete == 0 ? 1 : 0,
                    CrewPhoto = t.CrewPhoto,
                    UserName = t.Name,
                    WorkShipStartTime = ws.WorkShipStartTime,
                    WorkShipEndTime = ws.WorkShipEndTime,
                    OnBoard = ws.BusinessId.ToString(),
                    ShipId = ws.OnShip,
                    OnCountry = ow.Country,
                    CardId = t.CardId,
                    ShipType = ow.ShipType,
                    WorkNumber = t.WorkNumber,
                    ServiceBookType = t.ServiceBookType,
                    CrewType = pob.BusinessId.ToString(),
                    EmploymentType = ue.EmploymentId,
                    IsDelete = t.IsDelete,
                    DeleteReson = t.DeleteReson,
                    Created = t.Created,
                    StatusOrder = (DateTime.Now < ws.WorkShipStartTime ? "休假" : ws.WorkShipEndTime != null && DateTime.Now > ws.WorkShipEndTime ? "休假" : string.Empty) == "休假" ? 1 : t.DeleteReson != 0 ? (int)t.DeleteReson + 2 : (int)t.DeleteReson
                })
                .Distinct()
                .MergeTable()
                .OrderBy((t) => new
                {
                    t.StatusOrder,
                    Created = SqlFunc.Desc(t.Created)
                })
                .ToPageListAsync(requestBody.PageIndex, requestBody.PageSize, total);

            return await GetResult(requestBody, rt, total);
        }
        /// <summary>
        /// 获取列表结果集
        /// </summary>
        /// <param name="requestBody"></param>
        /// <param name="rt"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        private async Task<PageResult<SearchCrewArchivesResponse>> GetResult(SearchCrewArchivesRequest requestBody, List<SearchCrewArchivesResponse> rt, int total)
        {
            PageResult<SearchCrewArchivesResponse> page = new();
            if (rt.Any())
            {
                var uIds = rt.Select(x => x.BId).ToList();
                //在职状态
                var onBoardtab = await _dbContext.Queryable<WorkShip>().Where(t => t.IsDelete == 1 && uIds.Contains(t.WorkShipId)).ToListAsync();
                var ownerShipstab = await _dbContext.Queryable<OwnerShip>().Where(t => t.IsDelete == 1).ToListAsync();
                //国家地区
                var countrytab = await _dbContext.Queryable<CountryRegion>().Where(t => t.IsDelete == 1).ToListAsync();
                //船员类型
                var crewTypetab = await _dbContext.Queryable<PositionOnBoard>().Where(t => t.IsDelete == 1).ToListAsync();
                //用工类型
                var emptCodes = rt.Select(x => x.EmploymentType).ToList();
                var emptab = await _dbContext.Queryable<EmploymentType>().Where(t => t.IsDelete == 1 && emptCodes.Contains(t.BusinessId.ToString())).ToListAsync();
                //第一适任 第二适任
                var position = await _dbContext.Queryable<Position>().Where(t => t.IsDelete == 1).ToListAsync();
                var cerOfComps = await _dbContext.Queryable<CertificateOfCompetency>().Where(x => uIds.Contains(x.CertificateId) && (x.Type == CertificatesEnum.FCertificate || x.Type == CertificatesEnum.SCertificate)).ToListAsync();
                //技能证书
                var sctab = await _dbContext.Queryable<SkillCertificates>().Where(t => t.IsDelete == 1 && uIds.Contains(t.SkillcertificateId)).ToListAsync();
                //特设证书
                var spctab = await _dbContext.Queryable<SpecialEquips>().Where(t => t.IsDelete == 1 && uIds.Contains(t.SpecialEquipId)).ToListAsync();
                //航区
                var nArea = await _dbContext.Queryable<NavigationArea>().Where(t => t.IsDelete == 1).ToListAsync();
                //文件
                var url = AppsettingsHelper.GetValue("UpdateItem:Url");
                var files = rt.Select(t => t.CrewPhoto).ToArray();
                var fileInfo = await _dbContext.Queryable<Files>().Where(t => t.IsDelete == 1 && files.Contains(t.FileId)).ToListAsync();
                //名称赋值
                foreach (var t in rt)
                {
                    var sctabNames = "";//技能证书
                    var sctabs = new List<string>();//技能证书
                    var spctabNames = "";//特设证书
                    var spctabs = new List<string>();//特设证书
                    foreach (var s in sctab.Where(x => x.SkillcertificateId == t.BId))
                    {
                        var val = (int)s.SkillCertificateType;
                        sctabs.Add(val.ToString());
                        sctabNames += EnumUtil.GetDescription(s.SkillCertificateType) + "、";
                    }
                    if (!string.IsNullOrEmpty(sctabNames))
                    {
                        sctabNames = sctabNames.Substring(0, sctabNames.Length - 1);
                    }
                    foreach (var s in spctab.Where(x => x.SpecialEquipId == t.BId))
                    {
                        var val = (int)s.SpecialEquipsCertificateType;
                        spctabs.Add(val.ToString());
                        spctabNames += EnumUtil.GetDescription(s.SpecialEquipsCertificateType) + "、";
                    }
                    if (!string.IsNullOrEmpty(spctabNames))
                    {
                        spctabNames = spctabNames.Substring(0, spctabNames.Length - 1);
                    }
                    var ob = onBoardtab.Where(x => x.WorkShipId == t.BId && x.OnShip == t.ShipId).OrderByDescending(x => x.WorkShipStartTime).FirstOrDefault();
                    if (ob != null)
                    {
                        //所在船舶
                        var ownerShips = ownerShipstab.FirstOrDefault(x => ob.OnShip == x.BusinessId.ToString());
                        t.OnBoardName = ownerShips?.ShipName;
                        t.OnBoard = ob.OnShip;
                        if (ownerShips != null)
                        {
                            //所在国家
                            var country = countrytab.FirstOrDefault(x => ownerShips.Country == x.BusinessId);
                            t.OnCountry = ownerShips.Country;
                            t.OnCountryName = country?.Name;
                        }
                    }
                    t.ShipTypeName = EnumUtil.GetDescription(t.ShipType);
                    t.EmploymentTypeName = emptab.FirstOrDefault(x => x.BusinessId.ToString() == t.EmploymentType)?.Name;
                    t.CrewTypeName = crewTypetab.FirstOrDefault(x => t.CrewType == x.BusinessId.ToString())?.Name;
                    if (cerOfComps != null)
                    {
                        var cerofcF = cerOfComps.FirstOrDefault(x => x.Type == CertificatesEnum.FCertificate && x.CertificateId == t.BId);
                        var cerofcS = cerOfComps.FirstOrDefault(x => x.Type == CertificatesEnum.SCertificate && x.CertificateId == t.BId);
                        t.FPositionName = position.FirstOrDefault(x => x.BusinessId.ToString() == cerofcF?.FPosition)?.Name;
                        t.FPosition = cerofcF?.FPosition;
                        t.FNarea = cerofcF?.FNavigationArea;
                        t.FNareaName = nArea.FirstOrDefault(x => x.BusinessId.ToString() == cerofcF?.FNavigationArea)?.Name;
                        t.SPositionName = position.FirstOrDefault(x => x.BusinessId.ToString() == cerofcS?.SPosition)?.Name;
                        t.SPosition = cerofcS?.SPosition;
                        t.SNarea = cerofcS?.SNavigationArea;
                        t.SNareaName = nArea.FirstOrDefault(x => x.BusinessId.ToString() == cerofcS?.SNavigationArea)?.Name;
                    }
                    t.ServiceBookName = EnumUtil.GetDescription(t.ServiceBookType);
                    t.SkillsCertificateName = sctabNames;
                    t.SkillsCertificate = sctabs;
                    t.SpecialCertificate = spctabs;
                    t.SpecialCertificateName = spctabNames;
                    t.Age = _baseService.CalculateAgeFromIdCard(t.CardId);
                    t.OnStatus = ob == null ? CrewStatusEnum.DaiGang : ShipUserStatus(t.WorkShipStartTime, t.WorkShipEndTime, t.DeleteReson);
                    t.OnStatusName = ob == null ? EnumUtil.GetDescription(CrewStatusEnum.DaiGang) : EnumUtil.GetDescription(ShipUserStatus(t.WorkShipStartTime, t.WorkShipEndTime, t.DeleteReson));
                    var fileName = fileInfo.FirstOrDefault(x => x.FileId == t.CrewPhoto)?.Name;
                    t.Icon = string.IsNullOrWhiteSpace(fileName) ? string.Empty : url + fileName;
                }
            }
            page.List = rt;
            page.TotalCount = total;
            return page;
        }
        /// <summary>
        /// 船员状态
        /// </summary>
        /// <param name="StartTime">上船时间</param>
        /// <param name="EndTime">下船时间</param>
        /// <param name="deleteResonEnum">是否删除</param>
        /// <returns></returns>
        private static CrewStatusEnum ShipUserStatus(DateTime? StartTime, DateTime? EndTime, CrewStatusEnum deleteResonEnum)
        {
            var status = new CrewStatusEnum();
            if (deleteResonEnum != CrewStatusEnum.Normal)
            {
                //删除：管理人员手动操作，包括离职、调离和退休，优先于其他任何状态
                status = deleteResonEnum;
            }
            //若当前时间小于上船时间 则表示还在休假
            else if (DateTime.Now < StartTime)
            {
                status = CrewStatusEnum.XiuJia;
            }
            else if (DateTime.Now > StartTime && EndTime != null)
            {
                //若当前时间大于下船日期 则表示去休假了
                if (DateTime.Now > EndTime)
                {
                    status = CrewStatusEnum.XiuJia;
                }
            }
            return status;
        }

        #endregion
        /// <summary>
        /// 首页占比及统计数
        /// </summary>
        /// <returns></returns>
        public async Task<Result> CrewArchivesCountAsync()
        {
            //var roleType = await _baseService.CurRoleType();
            //if (roleType == -1) { return Result.Success(); }

            var dt = await SearchCrewArchivesAsync(new SearchCrewArchivesRequest() { PageIndex = 1, PageSize = 1000000 });

            //var udtab = await _dbContext.Queryable<User>().Where(t => t.IsLoginUser == 1).ToListAsync();
            //var udtabIds = udtab.Select(x => x.BusinessId.ToString()).ToList();
            //var onBoard = await _dbContext.Queryable<WorkShip>().Where(t => t.IsDelete == 1 && udtabIds.Contains(t.WorkShipId.ToString())).ToListAsync();
            if (dt.List == null) return Result.Success(new CrewArchivesResponse());
            var totalCount = dt.List.Count();//总数

            var onDutyCount = dt.List.Where(x => x.OnStatusName == "在岗").Count();
            //var onDutyCount = onBoard.Where(x => x.WorkShipEndTime <= DateTime.Now).Select(x => x.WorkShipId).Distinct().Count();//在船数
            var onDutyProp = totalCount == 0 ? 0 : Convert.ToInt32(onDutyCount / totalCount);

            var otherCount = dt.List.Where(x => x.OnStatusName == "离职" || x.OnStatusName == "调离" || x.OnStatusName == "退休").Count();
            //var otherCount = udtab.Where(x => x.DeleteReson != CrewStatusEnum.Normal && x.DeleteReson != CrewStatusEnum.XiuJia).Count();//离调退
            var otherProp = totalCount == 0 ? 0 : Convert.ToInt32(otherCount / totalCount);

            var waitCount = dt.List.Where(x => x.OnStatusName == "待岗").Count();
            //var waitCount = onBoard.Where(x => x.WorkShipEndTime > DateTime.Now).Select(x => x.WorkShipId).Distinct().Count() - otherCount;//待岗数 排掉休假 删除
            var waitProp = totalCount == 0 ? 0 : Convert.ToInt32(waitCount / totalCount);

            var holidayCount = dt.List.Where(x => x.OnStatusName == "休假").Count();
            //var holidayCount = onBoard.Where(x => x.HolidayTime > DateTime.Now).Select(x => x.WorkShipId).Distinct().Count();//休假数
            var holidayProp = totalCount == 0 ? 0 : Convert.ToInt32(holidayCount / totalCount);

            var rr = new CrewArchivesResponse
            {
                HolidayCount = holidayCount,
                OtherCount = otherCount,
                OtherProp = otherProp,
                HolidayProp = holidayProp,
                OnDutyCount = onDutyCount,
                OnDutyProp = onDutyProp,
                TatalCount = totalCount,
                WaitCount = waitCount,
                WaitProp = waitProp
            };
            return Result.Success(rr);
        }

        #region 数据增改
        /// <summary>
        /// 船员保存
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        public async Task<Result> SaveUserAsync(CrewArchivesRequest requestBody)
        {
            if (requestBody.BId == Guid.Empty || string.IsNullOrEmpty(requestBody.BId.ToString())) { return await InsertUserAsync(requestBody); }
            else { return await UpdateUserAsync(requestBody); }
        }
        /// <summary>
        /// 船员新增
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        private async Task<Result> InsertUserAsync(CrewArchivesRequest requestBody)
        {
            List<UploadResponse> upLoadFiles = new();

            #region 基本信息
            User userInfo = new();
            List<FamilyUser> hus = new();
            List<EmergencyContacts> ecs = new();
            List<UserEntryInfo> userEntrys = new();
            var uId = GuidUtil.Next();
            if (requestBody.BaseInfoDto != null)
            {
                //校验用户已经存在
                var existUi = await _dbContext.Queryable<User>().FirstAsync(t => t.IsLoginUser == 1 && requestBody.BaseInfoDto.WorkNumber == t.WorkNumber || requestBody.BaseInfoDto.Phone == t.Phone || requestBody.BaseInfoDto.CardId == t.CardId);
                if (existUi != null)
                {
                    if (existUi.Phone == requestBody.BaseInfoDto.Phone)
                    {
                        return Result.Fail("手机号重复");
                    }
                    if (existUi.WorkNumber == requestBody.BaseInfoDto.WorkNumber)
                    {
                        return Result.Fail("工号重复");
                    }
                    if (existUi.WorkNumber == requestBody.BaseInfoDto.WorkNumber)
                    {
                        return Result.Fail("身份证重复");
                    }
                }
                if (!IdCardUtils.ValidateIdCard(requestBody.BaseInfoDto.CardId))
                {
                    return Result.Fail("身份证错误：" + requestBody.BaseInfoDto.CardId);
                }
                if (!ValidateUtils.ValidatePhone(requestBody.BaseInfoDto.Phone))
                {
                    return Result.Fail("手机号错误：" + requestBody.BaseInfoDto.Phone);
                }
                userInfo = new()
                {
                    Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                    BuildAddress = requestBody.BaseInfoDto.BuildAddress,
                    BusinessId = uId,
                    CardId = requestBody.BaseInfoDto.CardId,
                    WorkNumber = requestBody.BaseInfoDto.WorkNumber,
                    Phone = requestBody.BaseInfoDto.Phone,
                    CrewPhoto = GuidUtil.Next(),
                    IdCardScans = GuidUtil.Next(),
                    PoliticalStatus = requestBody.BaseInfoDto.PoliticalStatus,
                    NativePlace = requestBody.BaseInfoDto.NativePlace,
                    Nation = requestBody.BaseInfoDto.Nation,
                    HomeAddress = requestBody.BaseInfoDto.HomeAddress,
                    //ShipType = requestBody.BaseInfoDto.ShipType,
                    //CrewType = requestBody.BaseInfoDto.CrewType,
                    ServiceBookType = requestBody.CertificateOfCompetencyDto.ServiceBookType,
                    //OnBoard = requestBody.BaseInfoDto.OnBoard,
                    //PositionOnBoard = requestBody.BaseInfoDto.PositionOnBoard,
                    Name = requestBody.BaseInfoDto.Name,
                    IsLoginUser = 1,
                    Oid = "101162350"
                };
                //文件
                if (requestBody.BaseInfoDto.UploadPhotoScans != null)
                {
                    var ff = requestBody.BaseInfoDto.UploadPhotoScans;
                    ff.FileId = userInfo.CrewPhoto;
                    upLoadFiles.Add(ff);
                }
                if (requestBody.BaseInfoDto.IdCardScansUpload != null && requestBody.BaseInfoDto.IdCardScansUpload.Any())
                {
                    var ff = requestBody.BaseInfoDto.IdCardScansUpload;
                    ff.ForEach(x => x.FileId = userInfo.IdCardScans);
                    upLoadFiles.AddRange(ff);
                }

                //劳务合同
                if (requestBody.BaseInfoDto.UserEntryInfo != null && requestBody.BaseInfoDto.UserEntryInfo.Any())
                {
                    var fileId = GuidUtil.Next();
                    foreach (var item in requestBody.BaseInfoDto.UserEntryInfo)
                    {
                        userEntrys.Add(new UserEntryInfo
                        {
                            BusinessId = GuidUtil.Next(),
                            Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                            ContractMain = item.ContarctMain,
                            EndTime = item.EndTime,
                            EntryScans = fileId,
                            StartTime = item.StartTime,
                            EntryTime = item.EntryTime,
                            LaborCompany = item.LaborCompany,
                            EmploymentId = item.EmploymentId,
                            UserEntryId = uId,
                            ContractType = item.ContractType
                        });

                        if (item.EntryScansUpload != null && item.EntryScansUpload.Any())
                        {
                            var ff = item.EntryScansUpload;
                            ff.ForEach(x => x.FileId = fileId);
                            upLoadFiles.AddRange(ff);
                        }
                    }

                }
                //家庭成员
                if (requestBody.BaseInfoDto.HomeUser != null && requestBody.BaseInfoDto.HomeUser.Any())
                {
                    //检验手机号是否存在  手机号唯一 只会绑定一位 除非注销或者删除
                    var existFamily = await _dbContext.Queryable<FamilyUser>().Where(t => requestBody.BaseInfoDto.HomeUser.Select(x => x.Phone).Contains(t.Phone)).ToListAsync();
                    foreach (var item in requestBody.BaseInfoDto.HomeUser)
                    {
                        var ef = existFamily.FirstOrDefault(x => x.Phone == item.Phone);
                        if (ef != null)
                        {
                            return Result.Fail("家庭成员已经绑定过，请先删除该/注销成员");
                        }
                        if (ValidateUtils.ValidatePhone(item.Phone) == false)
                        {
                            return Result.Fail("家庭成员手机号为");
                        }
                        if (!string.IsNullOrEmpty(item.Phone) && !ValidateUtils.ValidatePhone(item.Phone))
                        {
                            return Result.Fail("家庭成员手机号错误：" + item.Phone);
                        }
                        hus.Add(new FamilyUser
                        {
                            Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                            BusinessId = GuidUtil.Next(),
                            Phone = item.Phone,
                            RelationShip = item.RelationShip,
                            UserName = item.UserName,
                            WorkUnit = item.WorkUnit,
                            FamilyId = uId
                        });
                    }
                }
                //应急联系人
                if (requestBody.BaseInfoDto.EmergencyContacts != null && requestBody.BaseInfoDto.EmergencyContacts.Any())
                {
                    //检验手机号是否存在  手机号唯一 只会绑定一位 除非注销或者删除
                    var existFamily = await _dbContext.Queryable<FamilyUser>().Where(t => requestBody.BaseInfoDto.EmergencyContacts.Select(x => x.Phone).Contains(t.Phone)).ToListAsync();
                    foreach (var item in requestBody.BaseInfoDto.EmergencyContacts)
                    {
                        var ef = existFamily.FirstOrDefault(x => x.Phone == item.Phone);
                        if (ef != null)
                        {
                            return Result.Fail("应急联系人已经绑定过，请先删除/注销该成员");
                        }
                        if (ValidateUtils.ValidatePhone(item.Phone) == false)
                        {
                            return Result.Fail("应急联系人手机号为空");
                        }
                        if (!ValidateUtils.ValidatePhone(item.Phone))
                        {
                            return Result.Fail("应急联系人手机号错误：" + item.Phone);
                        }
                        ecs.Add(new EmergencyContacts
                        {
                            Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                            BusinessId = GuidUtil.Next(),
                            Phone = item.Phone,
                            RelationShip = item.RelationShip,
                            UserName = item.UserName,
                            EmergencyContactId = uId,
                            WorkUnit = item.WorkUnit
                        });
                    }
                }
            }
            if (userInfo != null) await _dbContext.Insertable(userInfo).ExecuteCommandAsync();
            if (userEntrys != null) await _dbContext.Insertable(userEntrys).ExecuteCommandAsync();
            if (hus.Any()) await _dbContext.Insertable(hus).ExecuteCommandAsync();
            if (ecs.Any()) await _dbContext.Insertable(ecs).ExecuteCommandAsync();
            #endregion

            #region 适任及证书
            List<CertificateOfCompetency> cocs = new();
            List<VisaRecords> vrs = new();
            List<SkillCertificates> sfs = new();
            List<SpecialEquips> ses = new();
            if (requestBody.CertificateOfCompetencyDto != null)
            {
                if (requestBody.CertificateOfCompetencyDto.FirstCertificateDto != null && requestBody.CertificateOfCompetencyDto.FirstCertificateDto.Any())
                {
                    foreach (var item in requestBody.CertificateOfCompetencyDto.FirstCertificateDto)
                    {
                        var fileId = GuidUtil.Next();
                        var f = new CertificateOfCompetency
                        {
                            CertificateId = uId,
                            BusinessId = GuidUtil.Next(),
                            Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                            FCertificate = item.FCertificate,
                            FNavigationArea = item.FNavigationArea,
                            FPosition = item.FPosition,
                            FSignTime = item.FSignTime,
                            FEffectiveTime = item.FEffectiveTime,
                            FScans = fileId,
                            Type = CertificatesEnum.FCertificate
                        };
                        cocs.Add(f);

                        //文件
                        if (item.FScansUpload != null && item.FScansUpload.Any())
                        {
                            var ff = item.FScansUpload;
                            ff.ForEach(x => x.FileId = f.FScans);
                            upLoadFiles.AddRange(ff);
                        }
                    }
                }
                if (requestBody.CertificateOfCompetencyDto.SeconedCertificateDto != null && requestBody.CertificateOfCompetencyDto.SeconedCertificateDto.Any())
                {
                    foreach (var item in requestBody.CertificateOfCompetencyDto.SeconedCertificateDto)
                    {
                        var fileId = GuidUtil.Next();
                        var s = new CertificateOfCompetency
                        {
                            CertificateId = uId,
                            BusinessId = GuidUtil.Next(),
                            Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                            SCertificate = item.SCertificate,
                            SNavigationArea = item.SNavigationArea,
                            SPosition = item.SPosition,
                            SSignTime = item.SSignTime,
                            SEffectiveTime = item.SEffectiveTime,
                            SScans = fileId,
                            Type = CertificatesEnum.SCertificate
                        };
                        cocs.Add(s);
                        if (item.SScansUpload != null && item.SScansUpload.Any())
                        {
                            var ff = item.SScansUpload;
                            ff.ForEach(x => x.FileId = s.SScans);
                            upLoadFiles.AddRange(ff);
                        }
                    }
                }
                if (requestBody.CertificateOfCompetencyDto.TrainingCertificateDto != null && requestBody.CertificateOfCompetencyDto.TrainingCertificateDto.Any())
                {
                    foreach (var item in requestBody.CertificateOfCompetencyDto.TrainingCertificateDto)
                    {
                        var fileId = GuidUtil.Next();
                        var t = new CertificateOfCompetency
                        {
                            CertificateId = uId,
                            BusinessId = GuidUtil.Next(),
                            Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                            TrainingCertificate = item.TrainingCertificate,
                            TrainingSignTime = item.TrainingSignTime,
                            TrainingScans = fileId,
                            Z01EffectiveTime = item.Z01EffectiveTime,
                            Z07EffectiveTime = item.Z07EffectiveTime,
                            Z08EffectiveTime = item.Z08EffectiveTime,
                            Z04EffectiveTime = item.Z04EffectiveTime,
                            Z05EffectiveTime = item.Z05EffectiveTime,
                            Z02EffectiveTime = item.Z02EffectiveTime,
                            Z06EffectiveTime = item.Z06EffectiveTime,
                            Z09EffectiveTime = item.Z09EffectiveTime,
                            Type = CertificatesEnum.PXHGZ
                        };
                        cocs.Add(t);
                        if (item.TrainingScansUpload != null && item.TrainingScansUpload.Any())
                        {
                            var ff = item.TrainingScansUpload;
                            ff.ForEach(x => x.FileId = t.TrainingScans);
                            upLoadFiles.AddRange(ff);
                        }
                    }
                }
                if (requestBody.CertificateOfCompetencyDto.HealthCertificateDto != null && requestBody.CertificateOfCompetencyDto.HealthCertificateDto.Any())
                {
                    foreach (var item in requestBody.CertificateOfCompetencyDto.HealthCertificateDto)
                    {
                        var fileId = GuidUtil.Next();
                        var h = new CertificateOfCompetency
                        {
                            CertificateId = uId,
                            BusinessId = GuidUtil.Next(),
                            Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                            HealthCertificate = item.HealthCertificate,
                            HealthSignTime = item.HealthSignTime,
                            HealthEffectiveTime = item.HealthEffectiveTime,
                            HealthScans = fileId,
                            Type = CertificatesEnum.JKZ
                        };
                        cocs.Add(h);
                        if (item.HealthScansUpload != null && item.HealthScansUpload.Any())
                        {
                            var ff = item.HealthScansUpload;
                            ff.ForEach(x => x.FileId = h.HealthScans);
                            upLoadFiles.AddRange(ff);
                        }
                    }
                }
                if (requestBody.CertificateOfCompetencyDto.SeamanCertificateDto != null && requestBody.CertificateOfCompetencyDto.SeamanCertificateDto.Any())
                {
                    foreach (var item in requestBody.CertificateOfCompetencyDto.SeamanCertificateDto)
                    {
                        var fileId = GuidUtil.Next();
                        var se = new CertificateOfCompetency
                        {
                            CertificateId = uId,
                            BusinessId = GuidUtil.Next(),
                            Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                            SeamanCertificate = item.SeamanCertificate,
                            SeamanSignTime = item.SeamanSignTime,
                            SeamanEffectiveTime = item.SeamanEffectiveTime,
                            SeamanScans = fileId,
                            Type = CertificatesEnum.HYZ
                        };
                        cocs.Add(se);
                        if (item.SeamanScansUpload != null && item.SeamanScansUpload.Any())
                        {
                            var ff = item.SeamanScansUpload;
                            ff.ForEach(x => x.FileId = se.SeamanScans);
                            upLoadFiles.AddRange(ff);
                        }
                    }
                }
                if (requestBody.CertificateOfCompetencyDto.PassportCertificateDto != null && requestBody.CertificateOfCompetencyDto.PassportCertificateDto.Any())
                {
                    foreach (var item in requestBody.CertificateOfCompetencyDto.PassportCertificateDto)
                    {
                        var fileId = GuidUtil.Next();
                        var id = GuidUtil.Next();
                        var p = new CertificateOfCompetency
                        {
                            CertificateId = uId,
                            BusinessId = id,
                            Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                            PassportCertificate = item.PassportCertificate,
                            PassportSignTime = item.PassportSignTime,
                            PassportEffectiveTime = item.PassportEffectiveTime,
                            PassportScans = fileId,
                            Type = CertificatesEnum.HZ
                        };

                        //签证记录
                        if (item.VisaRecords != null && item.VisaRecords.Any())
                        {
                            foreach (var item2 in item.VisaRecords)
                            {
                                vrs.Add(new VisaRecords
                                {
                                    Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                                    VisareCordId = uId,
                                    BusinessId = GuidUtil.Next(),
                                    Country = item2.Country,
                                    DueTime = item2.DueTime,
                                    VisaType = item2.VisaType,
                                    PassportCertificateId = id
                                });
                            }
                        }
                        cocs.Add(p);
                        if (item.PassportScansUpload != null && item.PassportScansUpload.Any())
                        {
                            var ff = item.PassportScansUpload;
                            ff.ForEach(x => x.FileId = p.PassportScans);
                            upLoadFiles.AddRange(ff);
                        }
                    }
                }

                //技能证书
                if (requestBody.CertificateOfCompetencyDto.SkillCertificates != null && requestBody.CertificateOfCompetencyDto.SkillCertificates.Any())
                {
                    foreach (var item in requestBody.CertificateOfCompetencyDto.SkillCertificates)
                    {
                        var fileId = GuidUtil.Next();
                        sfs.Add(new SkillCertificates
                        {
                            Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                            BusinessId = GuidUtil.Next(),
                            SkillCertificateType = item.SkillCertificateType,
                            SkillScans = fileId,
                            SkillcertificateId = uId
                        });
                        if (item.SkillScansUpload != null && item.SkillScansUpload.Any())
                        {
                            var ff = item.SkillScansUpload;
                            ff.ForEach(x => x.FileId = fileId);
                            upLoadFiles.AddRange(ff);
                        }
                    }
                }
                //特种设备证书
                if (requestBody.CertificateOfCompetencyDto.SpecialEquips != null && requestBody.CertificateOfCompetencyDto.SpecialEquips.Any())
                {
                    foreach (var item in requestBody.CertificateOfCompetencyDto.SpecialEquips)
                    {
                        var fileId = GuidUtil.Next();
                        ses.Add(new SpecialEquips
                        {
                            Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                            BusinessId = GuidUtil.Next(),
                            SpecialEquipsCertificateType = item.SpecialEquipsCertificateType,
                            SpecialEquipsEffectiveTime = item.SpecialEquipsEffectiveTime,
                            SpecialEquipsScans = fileId,
                            AnnualReviewTime = item.AnnualReviewTime,
                            SpecialEquipId = uId
                        });
                        if (item.SpecialEquipsScansUpload != null && item.SpecialEquipsScansUpload.Any())
                        {
                            var ff = item.SpecialEquipsScansUpload;
                            ff.ForEach(x => x.FileId = fileId);
                            upLoadFiles.AddRange(ff);
                        }
                    }
                }
            }
            if (cocs != null && cocs.Any()) await _dbContext.Insertable(cocs).ExecuteCommandAsync();
            if (vrs.Any()) await _dbContext.Insertable(vrs).ExecuteCommandAsync();
            if (sfs.Any()) await _dbContext.Insertable(sfs).ExecuteCommandAsync();
            if (ses.Any()) await _dbContext.Insertable(ses).ExecuteCommandAsync();
            #endregion

            #region 学历信息
            List<EducationalBackground> ebs = new();
            if (requestBody.EducationalBackgroundDto != null)
            {
                if (requestBody.EducationalBackgroundDto.QualificationInfos != null && requestBody.EducationalBackgroundDto.QualificationInfos.Any())
                {
                    foreach (var item in requestBody.EducationalBackgroundDto.QualificationInfos)
                    {
                        var qualificationScansId = GuidUtil.Next();
                        ebs.Add(new EducationalBackground
                        {
                            Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                            BusinessId = GuidUtil.Next(),
                            School = item.School,
                            Major = item.Major,
                            Qualification = item.Qualification,
                            EndTime = item.EndTime,
                            QualificationScans = qualificationScansId,
                            QualificationType = item.QualificationType,
                            StartTime = item.StartTime,
                            QualificationId = uId
                        });
                        if (item.QualificationScansUpload != null && item.QualificationScansUpload.Any())
                        {
                            var ff = item.QualificationScansUpload;
                            ff.ForEach(x => x.FileId = qualificationScansId);
                            upLoadFiles.AddRange(ff);
                        }
                    }
                }
            }
            if (ebs.Any()) await _dbContext.Insertable(ebs).ExecuteCommandAsync();
            #endregion

            #region 职务晋升
            List<Promotion> pts = new();
            if (requestBody.PromotionDto != null)
            {
                if (requestBody.PromotionDto.Promotions != null && requestBody.PromotionDto.Promotions.Any())
                {
                    foreach (var item in requestBody.PromotionDto.Promotions)
                    {
                        var promotionScanId = GuidUtil.Next();
                        pts.Add(new Promotion()
                        {
                            Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                            BusinessId = GuidUtil.Next(),
                            OnShip = item.OnShip,
                            Postition = item.Postition,
                            PromotionTime = item.PromotionTime,
                            PromotionScan = promotionScanId,
                            PromotionId = uId
                        });
                        if (item.PromotionScanUpload != null && item.PromotionScanUpload.Any())
                        {
                            var ff = item.PromotionScanUpload;
                            ff.ForEach(x => x.FileId = promotionScanId);
                            upLoadFiles.AddRange(ff);
                        }
                    }
                }
            }
            if (pts.Any()) await _dbContext.Insertable(pts).ExecuteCommandAsync();
            #endregion

            #region 任职船舶
            List<WorkShip> wss = new();
            if (requestBody.WorkShipDto != null)
            {
                if (requestBody.WorkShipDto.WorkShips != null && requestBody.WorkShipDto.WorkShips.Any())
                {
                    foreach (var item in requestBody.WorkShipDto.WorkShips)
                    {
                        if (item.OnShip == null) continue;
                        wss.Add(new WorkShip
                        {
                            Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                            BusinessId = GuidUtil.Next(),
                            OnShip = item.OnShip,
                            WorkShipStartTime = item.WorkShipStartTime,
                            WorkShipEndTime = item.WorkShipEndTime,
                            Postition = item.Postition,
                            WorkShipId = uId,
                            Country = item.Country,
                            ProjectName = item.ProjectName
                        });
                    }
                }
            }
            if (wss.Any()) await _dbContext.Insertable(wss).ExecuteCommandAsync();
            #endregion

            #region 培训记录
            List<TrainingRecord> trs = new();
            if (requestBody.TrainingRecordDto != null)
            {
                if (requestBody.TrainingRecordDto.TrainingRecords != null && requestBody.TrainingRecordDto.TrainingRecords.Any())
                {
                    foreach (var item in requestBody.TrainingRecordDto.TrainingRecords)
                    {
                        var trainingScanId = GuidUtil.Next();
                        trs.Add(new TrainingRecord
                        {
                            Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                            BusinessId = GuidUtil.Next(),
                            TrainingScan = trainingScanId,
                            TrainingTime = item.TrainingTime,
                            TrainingType = item.TrainingType,
                            TrainingId = uId
                        });
                        if (item.TrainingScanUpload != null && item.TrainingScanUpload.Any())
                        {
                            var ff = item.TrainingScanUpload;
                            ff.ForEach(x => x.FileId = trainingScanId);
                            upLoadFiles.AddRange(ff);
                        }
                    }
                }
            }
            if (trs.Any()) await _dbContext.Insertable(trs).ExecuteCommandAsync();
            #endregion

            #region 年度考核
            List<YearCheck> ycs = new();
            if (requestBody.YearCheckDto != null)
            {
                if (requestBody.YearCheckDto.YearChecks != null && requestBody.YearCheckDto.YearChecks.Any())
                {
                    foreach (var item in requestBody.YearCheckDto.YearChecks)
                    {
                        var yearCheckId = GuidUtil.Next();
                        ycs.Add(new YearCheck
                        {
                            Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                            BusinessId = GuidUtil.Next(),
                            CheckType = item.CheckType,
                            TrainingScan = yearCheckId,
                            TrainingTime = item.TrainingTime,
                            TrainingId = uId
                        });
                        if (item.TrainingScanUpload != null && item.TrainingScanUpload.Any())
                        {
                            var ff = item.TrainingScanUpload;
                            ff.ForEach(x => x.FileId = yearCheckId);
                            upLoadFiles.AddRange(ff);
                        }
                    }
                }
            }
            if (ycs.Any()) await _dbContext.Insertable(ycs).ExecuteCommandAsync();
            #endregion

            #region 保存文件
            upLoadFiles.ForEach(x => x.BId = GuidUtil.Next());
            upLoadFiles.ForEach(x => x.UserId = uId);
            await _baseService.InsertFileAsync(upLoadFiles, uId);
            #endregion

            return Result.Success();
        }
        /// <summary>
        /// 船员修改
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        private async Task<Result> UpdateUserAsync(CrewArchivesRequest requestBody)
        {

            List<UploadResponse> upLoadFiles = new();

            if (requestBody.BId == Guid.Empty || string.IsNullOrWhiteSpace(requestBody.BId.ToString()))
            {
                return Result.Fail("业务主键不能为空");
            }

            var userInfo = await _dbContext.Queryable<User>().Where(t => t.IsLoginUser == 1).FirstAsync(t => t.BusinessId == requestBody.BId);
            if (userInfo != null)
            {
                List<FamilyUser> husAdd = new();
                List<FamilyUser> husDel = new();
                List<EmergencyContacts> ecsAdd = new();
                List<EmergencyContacts> ecsDel = new();
                List<UserEntryInfo> userEntrys = new();
                List<UserEntryInfo> userEntryAdds = new();

                #region 基本信息
                if (requestBody.BaseInfoDto != null)
                {
                    if (!IdCardUtils.ValidateIdCard(requestBody.BaseInfoDto.CardId))
                    {
                        return Result.Fail("身份证错误");
                    }
                    if (!ValidateUtils.ValidatePhone(requestBody.BaseInfoDto.Phone))
                    {
                        return Result.Fail("手机号错误");
                    }
                    #region
                    userInfo.BuildAddress = requestBody.BaseInfoDto.BuildAddress;
                    userInfo.CardId = requestBody.BaseInfoDto.CardId;
                    userInfo.WorkNumber = requestBody.BaseInfoDto.WorkNumber;
                    userInfo.Phone = requestBody.BaseInfoDto.Phone;
                    userInfo.CrewPhoto = GuidUtil.Next();
                    userInfo.IdCardScans = GuidUtil.Next();
                    userInfo.PoliticalStatus = requestBody.BaseInfoDto.PoliticalStatus;
                    userInfo.NativePlace = requestBody.BaseInfoDto.NativePlace;
                    userInfo.Nation = requestBody.BaseInfoDto.Nation;
                    userInfo.HomeAddress = requestBody.BaseInfoDto.HomeAddress;
                    userInfo.ServiceBookType = requestBody.CertificateOfCompetencyDto.ServiceBookType;
                    userInfo.Name = requestBody.BaseInfoDto.Name;
                    #endregion
                    //文件
                    if (requestBody.BaseInfoDto.UploadPhotoScans != null)
                    {
                        var ff = requestBody.BaseInfoDto.UploadPhotoScans;
                        ff.FileId = userInfo.CrewPhoto;
                        upLoadFiles.Add(ff);
                    }
                    if (requestBody.BaseInfoDto.IdCardScansUpload != null && requestBody.BaseInfoDto.IdCardScansUpload.Any())
                    {
                        var ff = requestBody.BaseInfoDto.IdCardScansUpload;
                        ff.ForEach(x => x.FileId = userInfo.IdCardScans);
                        upLoadFiles.AddRange(ff);
                    }
                    //劳务合同
                    if (requestBody.BaseInfoDto.UserEntryInfo != null && requestBody.BaseInfoDto.UserEntryInfo.Any())
                    {
                        userEntrys = await _dbContext.Queryable<UserEntryInfo>().Where(t => t.IsDelete == 1 && t.UserEntryId == userInfo.BusinessId).ToListAsync();
                        var fileId = GuidUtil.Next();
                        foreach (var item in requestBody.BaseInfoDto.UserEntryInfo)
                        {
                            userEntryAdds.Add(new UserEntryInfo
                            {
                                BusinessId = GuidUtil.Next(),
                                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                                ContractMain = item.ContarctMain,
                                EndTime = item.EndTime,
                                EntryScans = fileId,
                                EntryTime = item.EntryTime,
                                LaborCompany = item.LaborCompany,
                                EmploymentId = item.EmploymentId,
                                UserEntryId = userInfo.BusinessId,
                                ContractType = item.ContractType,
                                StartTime = item.StartTime
                            });
                            if (item.EntryScansUpload != null && item.EntryScansUpload.Any())
                            {
                                var ff = item.EntryScansUpload;
                                ff.ForEach(x => x.FileId = fileId);
                                upLoadFiles.AddRange(ff);
                            }
                        }
                    }
                    //家庭成员
                    if (requestBody.BaseInfoDto.HomeUser != null && requestBody.BaseInfoDto.HomeUser.Any())
                    {
                        husDel = await _dbContext.Queryable<FamilyUser>().Where(t => t.FamilyId == userInfo.BusinessId).ToListAsync();
                        foreach (var item in requestBody.BaseInfoDto.HomeUser)
                        {
                            if (!ValidateUtils.ValidatePhone(item.Phone))
                            {
                                return Result.Fail("手机号错误");
                            }
                            husAdd.Add(new FamilyUser
                            {
                                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                                BusinessId = GuidUtil.Next(),
                                Phone = item.Phone,
                                RelationShip = item.RelationShip,
                                UserName = item.UserName,
                                WorkUnit = item.WorkUnit,
                                FamilyId = userInfo.BusinessId
                            });
                        }
                    }
                    //应急联系人
                    if (requestBody.BaseInfoDto.EmergencyContacts != null && requestBody.BaseInfoDto.EmergencyContacts.Any())
                    {
                        ecsDel = await _dbContext.Queryable<EmergencyContacts>().Where(t => t.EmergencyContactId == userInfo.BusinessId).ToListAsync();
                        foreach (var item in requestBody.BaseInfoDto.EmergencyContacts)
                        {
                            if (!ValidateUtils.ValidatePhone(item.Phone))
                            {
                                return Result.Fail("手机号错误");
                            }
                            ecsAdd.Add(new EmergencyContacts
                            {
                                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                                BusinessId = GuidUtil.Next(),
                                Phone = item.Phone,
                                RelationShip = item.RelationShip,
                                UserName = item.UserName,
                                EmergencyContactId = userInfo.BusinessId,
                                WorkUnit = item.WorkUnit
                            });
                        }
                    }
                }
                if (userInfo != null) await _dbContext.Updateable(userInfo).ExecuteCommandAsync();
                if (userEntrys != null) await _dbContext.Deleteable(userEntrys).ExecuteCommandAsync();
                if (husAdd != null && husAdd.Any()) await _dbContext.Insertable(husAdd).ExecuteCommandAsync();
                if (ecsAdd != null && ecsAdd.Any()) await _dbContext.Insertable(ecsAdd).ExecuteCommandAsync();
                if (husDel != null && husDel.Any()) await _dbContext.Deleteable(husDel).ExecuteCommandAsync();
                if (ecsDel != null && ecsDel.Any()) await _dbContext.Deleteable(ecsDel).ExecuteCommandAsync();
                if (userEntryAdds != null) await _dbContext.Insertable(userEntryAdds).ExecuteCommandAsync();
                #endregion

                #region 适任及证书
                List<CertificateOfCompetency> cocs = new();
                List<CertificateOfCompetency> cocsAdd = new();
                List<VisaRecords> vrs = new();
                List<SkillCertificates> sfs = new();
                List<SpecialEquips> ses = new();
                List<SpecialEquips> sesAdd = new();
                List<SkillCertificates> sfsAdd = new();
                List<VisaRecords> vrsAdd = new();
                if (requestBody.CertificateOfCompetencyDto != null)
                {
                    cocs = await _dbContext.Queryable<CertificateOfCompetency>().Where(t => t.IsDelete == 1 && t.CertificateId == userInfo.BusinessId).ToListAsync();
                    #region

                    if (requestBody.CertificateOfCompetencyDto.FirstCertificateDto != null && requestBody.CertificateOfCompetencyDto.FirstCertificateDto.Any())
                    {
                        foreach (var item in requestBody.CertificateOfCompetencyDto.FirstCertificateDto)
                        {
                            var fileId = GuidUtil.Next();
                            var f = new CertificateOfCompetency
                            {
                                CertificateId = userInfo.BusinessId,
                                BusinessId = GuidUtil.Next(),
                                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                                FCertificate = item.FCertificate,
                                FNavigationArea = item.FNavigationArea,
                                FPosition = item.FPosition,
                                FSignTime = item.FSignTime,
                                FEffectiveTime = item.FEffectiveTime,
                                FScans = fileId,
                                Type = CertificatesEnum.FCertificate
                            };
                            cocsAdd.Add(f);

                            //文件
                            if (item.FScansUpload != null && item.FScansUpload.Any())
                            {
                                var ff = item.FScansUpload;
                                ff.ForEach(x => x.FileId = f.FScans);
                                upLoadFiles.AddRange(ff);
                            }
                        }
                    }
                    if (requestBody.CertificateOfCompetencyDto.SeconedCertificateDto != null && requestBody.CertificateOfCompetencyDto.SeconedCertificateDto.Any())
                    {
                        foreach (var item in requestBody.CertificateOfCompetencyDto.SeconedCertificateDto)
                        {
                            var fileId = GuidUtil.Next();
                            var s = new CertificateOfCompetency
                            {
                                CertificateId = userInfo.BusinessId,
                                BusinessId = GuidUtil.Next(),
                                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                                SCertificate = item.SCertificate,
                                SNavigationArea = item.SNavigationArea,
                                SPosition = item.SPosition,
                                SSignTime = item.SSignTime,
                                SEffectiveTime = item.SEffectiveTime,
                                SScans = fileId,
                                Type = CertificatesEnum.SCertificate
                            };
                            cocsAdd.Add(s);
                            if (item.SScansUpload != null && item.SScansUpload.Any())
                            {
                                var ff = item.SScansUpload;
                                ff.ForEach(x => x.FileId = s.SScans);
                                upLoadFiles.AddRange(ff);
                            }
                        }
                    }
                    if (requestBody.CertificateOfCompetencyDto.TrainingCertificateDto != null && requestBody.CertificateOfCompetencyDto.TrainingCertificateDto.Any())
                    {
                        foreach (var item in requestBody.CertificateOfCompetencyDto.TrainingCertificateDto)
                        {
                            var fileId = GuidUtil.Next();
                            var t = new CertificateOfCompetency
                            {
                                CertificateId = userInfo.BusinessId,
                                BusinessId = GuidUtil.Next(),
                                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                                TrainingCertificate = item.TrainingCertificate,
                                TrainingSignTime = item.TrainingSignTime,
                                TrainingScans = fileId,
                                Z01EffectiveTime = item.Z01EffectiveTime,
                                Z07EffectiveTime = item.Z07EffectiveTime,
                                Z08EffectiveTime = item.Z08EffectiveTime,
                                Z04EffectiveTime = item.Z04EffectiveTime,
                                Z05EffectiveTime = item.Z05EffectiveTime,
                                Z02EffectiveTime = item.Z02EffectiveTime,
                                Z06EffectiveTime = item.Z06EffectiveTime,
                                Z09EffectiveTime = item.Z09EffectiveTime,
                                Type = CertificatesEnum.PXHGZ
                            };
                            cocsAdd.Add(t);
                            if (item.TrainingScansUpload != null && item.TrainingScansUpload.Any())
                            {
                                var ff = item.TrainingScansUpload;
                                ff.ForEach(x => x.FileId = t.TrainingScans);
                                upLoadFiles.AddRange(ff);
                            }
                        }
                    }
                    if (requestBody.CertificateOfCompetencyDto.HealthCertificateDto != null && requestBody.CertificateOfCompetencyDto.HealthCertificateDto.Any())
                    {
                        foreach (var item in requestBody.CertificateOfCompetencyDto.HealthCertificateDto)
                        {
                            var fileId = GuidUtil.Next();
                            var h = new CertificateOfCompetency
                            {
                                CertificateId = userInfo.BusinessId,
                                BusinessId = GuidUtil.Next(),
                                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                                HealthCertificate = item.HealthCertificate,
                                HealthSignTime = item.HealthSignTime,
                                HealthEffectiveTime = item.HealthEffectiveTime,
                                HealthScans = fileId,
                                Type = CertificatesEnum.JKZ
                            };
                            cocsAdd.Add(h);
                            if (item.HealthScansUpload != null && item.HealthScansUpload.Any())
                            {
                                var ff = item.HealthScansUpload;
                                ff.ForEach(x => x.FileId = h.HealthScans);
                                upLoadFiles.AddRange(ff);
                            }
                        }
                    }
                    if (requestBody.CertificateOfCompetencyDto.SeamanCertificateDto != null && requestBody.CertificateOfCompetencyDto.SeamanCertificateDto.Any())
                    {
                        foreach (var item in requestBody.CertificateOfCompetencyDto.SeamanCertificateDto)
                        {
                            var fileId = GuidUtil.Next();
                            var se = new CertificateOfCompetency
                            {
                                CertificateId = userInfo.BusinessId,
                                BusinessId = GuidUtil.Next(),
                                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                                SeamanCertificate = item.SeamanCertificate,
                                SeamanSignTime = item.SeamanSignTime,
                                SeamanEffectiveTime = item.SeamanEffectiveTime,
                                SeamanScans = fileId,
                                Type = CertificatesEnum.HYZ
                            };
                            cocsAdd.Add(se);
                            if (item.SeamanScansUpload != null && item.SeamanScansUpload.Any())
                            {
                                var ff = item.SeamanScansUpload;
                                ff.ForEach(x => x.FileId = se.SeamanScans);
                                upLoadFiles.AddRange(ff);
                            }
                        }
                    }
                    if (requestBody.CertificateOfCompetencyDto.PassportCertificateDto != null && requestBody.CertificateOfCompetencyDto.PassportCertificateDto.Any())
                    {
                        vrs = await _dbContext.Queryable<VisaRecords>().Where(t => t.VisareCordId == userInfo.BusinessId).ToListAsync();
                        foreach (var item in requestBody.CertificateOfCompetencyDto.PassportCertificateDto)
                        {
                            var fileId = GuidUtil.Next();
                            var id = GuidUtil.Next();
                            var p = new CertificateOfCompetency
                            {
                                CertificateId = userInfo.BusinessId,
                                BusinessId = id,
                                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                                PassportCertificate = item.PassportCertificate,
                                PassportSignTime = item.PassportSignTime,
                                PassportEffectiveTime = item.PassportEffectiveTime,
                                PassportScans = fileId,
                                Type = CertificatesEnum.HZ
                            };
                            cocsAdd.Add(p);

                            //签证记录
                            if (item.VisaRecords != null && item.VisaRecords.Any())
                            {
                                foreach (var item2 in item.VisaRecords)
                                {
                                    vrsAdd.Add(new VisaRecords
                                    {
                                        Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                                        VisareCordId = userInfo.BusinessId,
                                        BusinessId = GuidUtil.Next(),
                                        Country = item2.Country,
                                        DueTime = item2.DueTime,
                                        VisaType = item2.VisaType,
                                        PassportCertificateId = id
                                    });
                                }
                            }
                            if (item.PassportScansUpload != null && item.PassportScansUpload.Any())
                            {
                                var ff = item.PassportScansUpload;
                                ff.ForEach(x => x.FileId = p.PassportScans);
                                upLoadFiles.AddRange(ff);
                            }
                        }
                    }
                    //技能证书
                    if (requestBody.CertificateOfCompetencyDto.SkillCertificates != null && requestBody.CertificateOfCompetencyDto.SkillCertificates.Any())
                    {
                        sfs = await _dbContext.Queryable<SkillCertificates>().Where(t => t.IsDelete == 1 && t.SkillcertificateId == userInfo.BusinessId).ToListAsync();
                        foreach (var item in requestBody.CertificateOfCompetencyDto.SkillCertificates)
                        {
                            var filedId = GuidUtil.Next();
                            sfsAdd.Add(new SkillCertificates
                            {
                                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                                BusinessId = GuidUtil.Next(),
                                SkillCertificateType = item.SkillCertificateType,
                                SkillScans = filedId,
                                SkillcertificateId = userInfo.BusinessId
                            });
                            if (item.SkillScansUpload != null && item.SkillScansUpload.Any())
                            {
                                var ff = item.SkillScansUpload;
                                ff.ForEach(x => x.FileId = filedId);
                                upLoadFiles.AddRange(ff);
                            }
                        }
                    }
                    //特种设备证书
                    if (requestBody.CertificateOfCompetencyDto.SpecialEquips != null && requestBody.CertificateOfCompetencyDto.SpecialEquips.Any())
                    {
                        ses = await _dbContext.Queryable<SpecialEquips>().Where(t => t.IsDelete == 1 && t.SpecialEquipId == userInfo.BusinessId).ToListAsync();
                        foreach (var item in requestBody.CertificateOfCompetencyDto.SpecialEquips)
                        {
                            var fileId = GuidUtil.Next();
                            sesAdd.Add(new SpecialEquips
                            {
                                BusinessId = GuidUtil.Next(),
                                AnnualReviewTime = item.SpecialEquipsEffectiveTime,
                                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                                SpecialEquipId = userInfo.BusinessId,
                                SpecialEquipsCertificateType = item.SpecialEquipsCertificateType,
                                SpecialEquipsEffectiveTime = item.SpecialEquipsEffectiveTime,
                                SpecialEquipsScans = fileId
                            });
                            if (item.SpecialEquipsScansUpload != null && item.SpecialEquipsScansUpload.Any())
                            {
                                var ff = item.SpecialEquipsScansUpload;
                                ff.ForEach(x => x.FileId = fileId);
                                upLoadFiles.AddRange(ff);
                            }
                        }
                    }
                    #endregion
                }
                if (vrs.Any()) await _dbContext.Deleteable(vrs).ExecuteCommandAsync();
                if (sfs.Any()) await _dbContext.Deleteable(sfs).ExecuteCommandAsync();
                if (ses.Any()) await _dbContext.Deleteable(ses).ExecuteCommandAsync();
                if (cocs != null && cocs.Any()) await _dbContext.Deleteable(cocs).ExecuteCommandAsync();
                if (sesAdd != null && sesAdd.Any()) await _dbContext.Insertable(sesAdd).ExecuteCommandAsync();
                if (sfsAdd != null && sfsAdd.Any()) await _dbContext.Insertable(sfsAdd).ExecuteCommandAsync();
                if (vrsAdd != null && vrsAdd.Any()) await _dbContext.Insertable(vrsAdd).ExecuteCommandAsync();
                if (cocsAdd != null && cocsAdd.Any()) await _dbContext.Insertable(cocsAdd).ExecuteCommandAsync();
                #endregion

                #region 学历信息
                List<EducationalBackground> ebsAdd = new();
                List<EducationalBackground> ebsDel = new();
                if (requestBody.EducationalBackgroundDto != null)
                {
                    if (requestBody.EducationalBackgroundDto.QualificationInfos != null && requestBody.EducationalBackgroundDto.QualificationInfos.Any())
                    {
                        ebsDel = await _dbContext.Queryable<EducationalBackground>().Where(t => t.IsDelete == 1 && userInfo.BusinessId == t.QualificationId).ToListAsync();
                        foreach (var item in requestBody.EducationalBackgroundDto.QualificationInfos)
                        {
                            var fileId = GuidUtil.Next();
                            ebsAdd.Add(new EducationalBackground
                            {
                                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                                BusinessId = GuidUtil.Next(),
                                School = item.School,
                                Major = item.Major,
                                Qualification = item.Qualification,
                                EndTime = item.EndTime,
                                QualificationScans = fileId,
                                QualificationType = item.QualificationType,
                                StartTime = item.StartTime,
                                QualificationId = userInfo.BusinessId
                            });
                            if (item.QualificationScansUpload != null && item.QualificationScansUpload.Any())
                            {
                                var ff = item.QualificationScansUpload;
                                ff.ForEach(x => x.FileId = fileId);
                                upLoadFiles.AddRange(ff);
                            }
                        }
                    }
                }
                if (ebsDel != null && ebsDel.Any()) await _dbContext.Deleteable(ebsDel).ExecuteCommandAsync();
                if (ebsAdd != null && ebsAdd.Any()) await _dbContext.Insertable(ebsAdd).ExecuteCommandAsync();
                #endregion

                #region 职务晋升
                List<Promotion> pts = new();
                List<Promotion> ptsAdd = new();
                if (requestBody.PromotionDto != null)
                {
                    if (requestBody.PromotionDto.Promotions != null && requestBody.PromotionDto.Promotions.Any())
                    {
                        pts = await _dbContext.Queryable<Promotion>().Where(t => t.PromotionId == userInfo.BusinessId).ToListAsync();
                        foreach (var item in requestBody.PromotionDto.Promotions)
                        {
                            var fileId = GuidUtil.Next();
                            ptsAdd.Add(new Promotion()
                            {
                                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                                BusinessId = GuidUtil.Next(),
                                OnShip = item.OnShip,
                                Postition = item.Postition,
                                PromotionTime = item.PromotionTime,
                                PromotionScan = fileId,
                                PromotionId = userInfo.BusinessId
                            });
                            if (item.PromotionScanUpload != null && item.PromotionScanUpload.Any())
                            {
                                var ff = item.PromotionScanUpload;
                                ff.ForEach(x => x.FileId = fileId);
                                upLoadFiles.AddRange(ff);
                            }
                        }
                    }
                }
                if (ptsAdd != null && ptsAdd.Any()) await _dbContext.Insertable(ptsAdd).ExecuteCommandAsync();
                if (pts.Any()) await _dbContext.Deleteable(pts).ExecuteCommandAsync();
                #endregion

                #region 任职船舶
                List<WorkShip> wss = new();
                List<WorkShip> wssAdd = new();
                if (requestBody.WorkShipDto != null)
                {
                    if (requestBody.WorkShipDto.WorkShips != null && requestBody.WorkShipDto.WorkShips.Any())
                    {
                        wss = await _dbContext.Queryable<WorkShip>().Where(t => t.IsDelete == 1 && userInfo.BusinessId == t.WorkShipId).ToListAsync();
                        foreach (var item in requestBody.WorkShipDto.WorkShips)
                        {
                            wssAdd.Add(new WorkShip
                            {
                                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                                BusinessId = GuidUtil.Next(),
                                OnShip = item.OnShip,
                                WorkShipStartTime = item.WorkShipStartTime,
                                WorkShipEndTime = item.WorkShipEndTime,
                                Postition = item.Postition,
                                WorkShipId = userInfo.BusinessId,
                                Country = item.Country,
                                ProjectName = item.ProjectName
                            });
                        }
                    }
                }
                if (wss.Any()) await _dbContext.Deleteable(wss).ExecuteCommandAsync();
                if (wssAdd != null && wssAdd.Any()) await _dbContext.Insertable(wssAdd).ExecuteCommandAsync();
                #endregion

                #region 培训记录
                List<TrainingRecord> trs = new();
                List<TrainingRecord> trsAdd = new();
                if (requestBody.TrainingRecordDto != null)
                {
                    if (requestBody.TrainingRecordDto.TrainingRecords != null && requestBody.TrainingRecordDto.TrainingRecords.Any())
                    {
                        trs = await _dbContext.Queryable<TrainingRecord>().Where(t => t.TrainingId == userInfo.BusinessId).ToListAsync();
                        foreach (var item in requestBody.TrainingRecordDto.TrainingRecords)
                        {
                            var fileId = GuidUtil.Next();
                            trsAdd.Add(new TrainingRecord
                            {
                                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                                BusinessId = GuidUtil.Next(),
                                TrainingScan = fileId,
                                TrainingTime = item.TrainingTime,
                                TrainingType = item.TrainingType,
                                TrainingId = userInfo.BusinessId
                            });
                            if (item.TrainingScanUpload != null && item.TrainingScanUpload.Any())
                            {
                                var ff = item.TrainingScanUpload;
                                ff.ForEach(x => x.FileId = fileId);
                                upLoadFiles.AddRange(ff);
                            }
                        }
                    }
                }
                if (trs.Any()) await _dbContext.Deleteable(trs).ExecuteCommandAsync();
                if (trsAdd != null && trsAdd.Any()) await _dbContext.Insertable(trsAdd).ExecuteCommandAsync();
                #endregion

                #region 年度考核
                List<YearCheck> ycs = new();
                List<YearCheck> ycsAdd = new();
                if (requestBody.YearCheckDto != null)
                {
                    if (requestBody.YearCheckDto.YearChecks != null && requestBody.YearCheckDto.YearChecks.Any())
                    {
                        ycs = await _dbContext.Queryable<YearCheck>().Where(t => t.IsDelete == 1 && t.TrainingId == userInfo.BusinessId).ToListAsync();
                        foreach (var item in requestBody.YearCheckDto.YearChecks)
                        {
                            var fileId = GuidUtil.Next();
                            ycsAdd.Add(new YearCheck
                            {
                                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                                BusinessId = GuidUtil.Next(),
                                CheckType = item.CheckType,
                                TrainingScan = fileId,
                                TrainingTime = item.TrainingTime,
                                TrainingId = userInfo.BusinessId
                            });
                            if (item.TrainingScanUpload != null && item.TrainingScanUpload.Any())
                            {
                                var ff = item.TrainingScanUpload;
                                ff.ForEach(x => x.FileId = fileId);
                                upLoadFiles.AddRange(ff);
                            }
                        }
                    }
                }
                if (ycs.Any()) await _dbContext.Deleteable(ycs).ExecuteCommandAsync();
                if (ycsAdd != null && ycsAdd.Any()) await _dbContext.Insertable(ycsAdd).ExecuteCommandAsync();
                #endregion

                #region 文件保存
                upLoadFiles.ForEach(x => x.BId = GuidUtil.Next());
                upLoadFiles.ForEach(x => x.UserId = userInfo.BusinessId);
                await _baseService.UpdateFileAsync(upLoadFiles, userInfo.BusinessId);
                #endregion

                return Result.Success();
            }
            return Result.Fail("无船员/该船员已被删除");
        }
        #endregion
        /// <summary>
        /// 保存备注
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        public async Task<Result> SaveNotesAsync(NotesRequest requestBody)
        {
            if (requestBody.Type == 1) return await InsertNotesAsync(requestBody);
            else return await UpdateNotesAsync(requestBody);
        }
        /// <summary>
        /// 新增备注
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        private async Task<Result> InsertNotesAsync(NotesRequest requestBody)
        {
            if (requestBody.SaveNotes != null)
            {
                var userNotes = new UserNotes
                {
                    BusinessId = GuidUtil.Next(),
                    UserNoteId = requestBody.BId.Value,
                    Content = requestBody.SaveNotes.Content,
                    WritedUserId = GlobalCurrentUser.UserBusinessId.ToString(),
                    Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId()
                };
                await _dbContext.Insertable(userNotes).ExecuteCommandAsync();
                return Result.Success("保存成功");
            }
            else
            {
                return Result.Fail("保存失败");
            }
        }
        /// <summary>
        /// 修改备注
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        private async Task<Result> UpdateNotesAsync(NotesRequest requestBody)
        {
            if (requestBody.SaveNotes != null)
            {
                //获取数据
                var note = await _dbContext.Queryable<UserNotes>().Where(t => t.IsDelete == 1 && requestBody.BId == t.BusinessId).FirstAsync();
                if (note != null)
                {
                    note.Content = requestBody.SaveNotes.Content;
                    note.WritedUserId = GlobalCurrentUser.UserBusinessId.ToString();
                }

                await _dbContext.Insertable(note).ExecuteCommandAsync();
                return Result.Success("修改成功");
            }
            else
            {
                return Result.Fail("修改失败");
            }
        }
        /// <summary>
        /// 切换船员状态（删除/恢复）
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        public async Task<Result> ToggleUserStatusAsync(ToggleUserStatus requestBody)
        {
            if (requestBody.Type == 1) return await DeactivateUserASync(requestBody);
            else return await UndoDeleteUserAsync(requestBody);
        }
        /// <summary>
        /// 删除船员（非永久删除）
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        private async Task<Result> DeactivateUserASync(ToggleUserStatus requestBody)
        {
            var rt = await _dbContext.Queryable<User>().FirstAsync(t => t.IsDelete == 1 && t.BusinessId == requestBody.BId);
            if (rt != null)
            {
                rt.IsDelete = 0;
                rt.DeleteReson = requestBody.DeactivateStatus.Value;
                await _dbContext.Updateable(rt).UpdateColumns(x => new { x.IsDelete, x.DeleteReson }).ExecuteCommandAsync();
                return Result.Success("已删除");
            }
            else
            {
                return Result.Fail("数据不存在/已删除");
            }
        }
        /// <summary>
        /// 撤销删除（恢复）
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        private async Task<Result> UndoDeleteUserAsync(ToggleUserStatus requestBody)
        {
            var rt = await _dbContext.Queryable<User>().FirstAsync(t => t.IsDelete == 0 && t.BusinessId == requestBody.BId);
            if (rt != null)
            {
                rt.IsDelete = 1;
                rt.DeleteReson = CrewStatusEnum.Normal;
                await _dbContext.Updateable(rt).UpdateColumns(x => new { x.IsDelete, x.DeleteReson }).ExecuteCommandAsync();
                return Result.Success("已恢复");
            }
            else
            {
                return Result.Fail("数据不存在/已删除");
            }
        }
        /// <summary>
        /// 船员调任
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        public async Task<Result> CrewTransferAsync(CrewTransferRequest requestBody)
        {
            //修改原船舶的下船日期
            var shipWork = await _dbContext.Queryable<WorkShip>()
                   .Where(t => t.IsDelete == 1 && t.WorkShipId == requestBody.BId)
                   .OrderByDescending(x => x.WorkShipStartTime).FirstAsync();
            if (shipWork != null)
            {
                shipWork.WorkShipEndTime = requestBody.WorkShipStartTime;
                await _dbContext.Updateable(shipWork).UpdateColumns(x => x.WorkShipEndTime).ExecuteCommandAsync();
            }
            //新增新船舶的任职数据
            var addWorkShip = new WorkShip
            {
                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                BusinessId = GuidUtil.Next(),
                WorkShipStartTime = requestBody.WorkShipStartTime,
                OnShip = requestBody.OnShip,
                Postition = requestBody.Postition,
                WorkShipId = requestBody.BId
            };
            await _dbContext.Insertable(addWorkShip).ExecuteCommandAsync();

            return Result.Success("调任成功");
        }
        #region 下拉列表信息
        /// <summary>
        /// 基本下拉列表
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<Result> GetDropDownListAsync(int type)
        {
            Result rt = new();
            switch (type)
            {
                case 1://籍贯
                    rt = await GetNativePlaceListAsync();
                    break;
                case 2://民族
                    rt = await GetNationListAsync();
                    break;
                case 3://船员类型
                    rt = await GetCrewTypeListAsync();
                    break;
                case 4://船舶
                    rt = await GetOwnerShipListAsync();
                    break;
                case 5://培训类型
                    rt = await GetTrainingListAsync();
                    break;
                case 6://国家
                    rt = await GetCountryListAsync();
                    break;
                case 7://家庭关系
                    rt = GetFamilyRelationList();
                    break;
                case 8://船舶类型
                    rt = GetShipTypeList();
                    break;
                case 9://服务簿类型
                    rt = GetServiceBookList();
                    break;
                case 10://签证类型
                    rt = GetVisaTypeList();
                    break;
                case 11://技能证书
                    rt = GetCertificateTypeList();
                    break;
                case 12://学历类型
                    rt = GetQualificationTypeList();
                    break;
                case 13://学历
                    rt = GetQualificationList();
                    break;
                case 14://合同类型
                    rt = GetContractList();
                    break;
                case 15://考核类型
                    rt = GetCheckList();
                    break;
                case 16://船舶状态
                    rt = GetShipStateList();
                    break;
                case 17://适任职务
                    rt = await GetPositionListAsync();
                    break;
                case 18://船员状态
                    rt = GetCrewStatusList();
                    break;
                case 19://政治面貌
                    rt = await GetPoliticalListAsync();
                    break;
                case 20://用工形式
                    rt = await GetEmploymentTypeListAsync();
                    break;
                case 21://航区
                    rt = await GetNavigationareListAsync();
                    break;
                case 22://证书类型
                    rt = GetCertificatesList();
                    break;
                case 23://在船职务
                    rt = await GetOnBoardPositionListAsync();
                    break;
            }
            return rt;
        }
        /// <summary>
        /// 籍贯
        /// </summary>
        /// <returns></returns>
        private async Task<Result> GetNativePlaceListAsync()
        {
            var rr = await _dbContext.Queryable<AdministrativeDivision>().Where(t => t.IsDelete == 1 && t.SupRegionalismCode == "0").Select(t => new DropDownResponse
            {
                Key = t.BusinessId.ToString(),
                Value = t.Name,
            }).ToListAsync();
            return Result.Success(rr);
        }
        /// <summary>
        /// 民族
        /// </summary>
        /// <returns></returns>
        private async Task<Result> GetNationListAsync()
        {
            var rr = await _dbContext.Queryable<Nation>().Where(t => t.IsDelete == 1).Select(t => new DropDownResponse
            {
                Key = t.BusinessId.ToString(),
                Value = t.Name,
            }).ToListAsync();
            return Result.Success(rr);
        }
        /// <summary>
        /// 船员类型
        /// </summary>
        /// <returns></returns>
        private async Task<Result> GetCrewTypeListAsync()
        {
            var rr = await _dbContext.Queryable<PositionOnBoard>().Where(t => t.IsDelete == 1).Select(t => new DropDownResponse
            {
                Key = t.Type.ToString(),
                Value = t.Remark,
            }).Distinct().ToListAsync();
            return Result.Success(rr);
        }
        /// <summary>
        /// 在船职务
        /// </summary>
        /// <returns></returns>
        private async Task<Result> GetOnBoardPositionListAsync()
        {
            var rr = await _dbContext.Queryable<PositionOnBoard>().Where(t => t.IsDelete == 1).Select(t => new DropDownResponse
            {
                Key = t.BusinessId.ToString(),
                Value = t.Name,
            }).ToListAsync();
            return Result.Success(rr);
        }
        /// <summary>
        /// 船舶
        /// </summary>
        /// <returns></returns>
        private async Task<Result> GetOwnerShipListAsync()
        {
            var rr = await _dbContext.Queryable<OwnerShip>().Where(t => t.IsDelete == 1).Select(t => new DropDownResponse
            {
                Key = t.BusinessId.ToString(),
                Value = t.ShipName,
                Type = (int)t.ShipType,
                Country = t.Country,
                ProjectName = t.ProjectName
            }).ToListAsync();
            return Result.Success(rr);
        }
        /// <summary>
        /// 培训类型
        /// </summary>
        /// <returns></returns>
        private async Task<Result> GetTrainingListAsync()
        {
            var rr = await _dbContext.Queryable<TrainingType>().Where(t => t.IsDelete == 1).Select(t => new DropDownResponse
            {
                Key = t.BusinessId.ToString(),
                Value = t.Name,
            }).ToListAsync();
            return Result.Success(rr);
        }
        /// <summary>
        /// 政治面貌
        /// </summary>
        /// <returns></returns>
        private async Task<Result> GetPoliticalListAsync()
        {
            var rr = await _dbContext.Queryable<Political>().Where(t => t.IsDelete == 1).Select(t => new DropDownResponse
            {
                Key = t.BusinessId.ToString(),
                Value = t.Name,
            }).ToListAsync();
            return Result.Success(rr);
        }
        /// <summary>
        /// 用工形式
        /// </summary>
        /// <returns></returns>
        private async Task<Result> GetEmploymentTypeListAsync()
        {
            var rr = await _dbContext.Queryable<EmploymentType>().Where(t => t.IsDelete == 1 && (t.Code == "1" || t.Code == "2" || t.Code == "33")).Select(t => new DropDownResponse
            {
                Key = t.BusinessId.ToString(),
                Value = t.Name,
            }).ToListAsync();
            return Result.Success(rr);
        }
        /// <summary>
        /// 国家
        /// </summary>
        /// <returns></returns>
        private async Task<Result> GetCountryListAsync()
        {
            var rr = await _dbContext.Queryable<CountryRegion>().Where(t => t.IsDelete == 1).Select(t => new DropDownResponse
            {
                Key = t.BusinessId.ToString(),
                Value = t.Name,
            }).ToListAsync();
            return Result.Success(rr);
        }
        /// <summary>
        /// 航区
        /// </summary>
        /// <returns></returns>
        private async Task<Result> GetNavigationareListAsync()
        {
            var rr = await _dbContext.Queryable<NavigationArea>().Where(t => t.IsDelete == 1).Select(t => new DropDownResponse
            {
                Key = t.BusinessId.ToString(),
                Value = t.Name,
            }).ToListAsync();
            return Result.Success(rr);
        }
        /// <summary>
        /// 适任职务
        /// </summary>
        /// <returns></returns>
        private async Task<Result> GetPositionListAsync()
        {
            var rr = await _dbContext.Queryable<Position>().Where(t => t.IsDelete == 1).Select(t => new DropDownResponse
            {
                Key = t.BusinessId.ToString(),
                Value = t.Name,
            }).ToListAsync();
            return Result.Success(rr);
        }
        /// <summary>
        /// 家庭关系
        /// </summary>
        /// <returns></returns>
        private static Result GetFamilyRelationList()
        {
            var enumConvertList = Enum.GetValues(typeof(FamilyRelationEnum))
                                                   .Cast<FamilyRelationEnum>()
                                                   .Select(x => new DropDownResponse
                                                   {
                                                       Key = ((int)x).ToString(),
                                                       Value = GetEnumDescription(x),
                                                       Type = (int)x
                                                   })
                                                   .ToList();
            return Result.Success(enumConvertList);
        }
        /// <summary>
        /// 证书类型
        /// </summary>
        /// <returns></returns>
        private static Result GetCertificatesList()
        {
            var enumConvertList = Enum.GetValues(typeof(CertificatesEnum))
                                                   .Cast<CertificatesEnum>()
                                                   .Select(x => new DropDownResponse
                                                   {
                                                       Key = ((int)x).ToString(),
                                                       Value = GetEnumDescription(x),
                                                       Type = (int)x
                                                   })
                                                   .ToList();
            return Result.Success(enumConvertList);
        }
        /// <summary>
        /// 船舶类型
        /// </summary>
        /// <returns></returns>
        private static Result GetShipTypeList()
        {
            var enumConvertList = Enum.GetValues(typeof(ShipTypeEnum))
                                                   .Cast<ShipTypeEnum>()
                                                   .Select(x => new DropDownResponse
                                                   {
                                                       Key = ((int)x).ToString(),
                                                       Value = GetEnumDescription(x),
                                                       Type = (int)x
                                                   })
                                                   .ToList();
            return Result.Success(enumConvertList);
        }
        /// <summary>
        /// 服务簿类型
        /// </summary>
        /// <returns></returns>
        private static Result GetServiceBookList()
        {
            var enumConvertList = Enum.GetValues(typeof(ServiceBookEnum))
                                                   .Cast<ServiceBookEnum>()
                                                   .Select(x => new DropDownResponse
                                                   {
                                                       Key = ((int)x).ToString(),
                                                       Value = GetEnumDescription(x),
                                                       Type = (int)x
                                                   })
                                                   .ToList();
            return Result.Success(enumConvertList);
        }
        /// <summary>
        /// 签证类型
        /// </summary>
        /// <returns></returns>
        private static Result GetVisaTypeList()
        {
            var enumConvertList = Enum.GetValues(typeof(VisaTypeEnum))
                                                   .Cast<VisaTypeEnum>()
                                                   .Select(x => new DropDownResponse
                                                   {
                                                       Key = ((int)x).ToString(),
                                                       Value = GetEnumDescription(x),
                                                       Type = (int)x
                                                   })
                                                   .ToList();
            return Result.Success(enumConvertList);
        }
        /// <summary>
        /// 技能证书
        /// </summary>
        /// <returns></returns>
        private static Result GetCertificateTypeList()
        {
            var enumConvertList = Enum.GetValues(typeof(CertificateTypeEnum))
                                                   .Cast<CertificateTypeEnum>()
                                                   .Select(x => new DropDownResponse
                                                   {
                                                       Key = ((int)x).ToString(),
                                                       Value = GetEnumDescription(x),
                                                       Type = (int)x
                                                   })
                                                   .ToList();
            return Result.Success(enumConvertList);
        }
        /// <summary>
        /// 学历类型
        /// </summary>
        /// <returns></returns>
        private static Result GetQualificationTypeList()
        {
            var enumConvertList = Enum.GetValues(typeof(QualificationTypeEnum))
                                                   .Cast<QualificationTypeEnum>()
                                                   .Select(x => new DropDownResponse
                                                   {
                                                       Key = ((int)x).ToString(),
                                                       Value = GetEnumDescription(x),
                                                       Type = (int)x
                                                   })
                                                   .ToList();
            return Result.Success(enumConvertList);
        }
        /// <summary>
        /// 学历
        /// </summary>
        /// <returns></returns>
        private static Result GetQualificationList()
        {
            var enumConvertList = Enum.GetValues(typeof(QualificationEnum))
                                                   .Cast<QualificationEnum>()
                                                   .Select(x => new DropDownResponse
                                                   {
                                                       Key = ((int)x).ToString(),
                                                       Value = GetEnumDescription(x),
                                                       Type = (int)x
                                                   })
                                                   .ToList();
            return Result.Success(enumConvertList);
        }
        /// <summary>
        /// 合同
        /// </summary>
        /// <returns></returns>
        private static Result GetContractList()
        {
            var enumConvertList = Enum.GetValues(typeof(ContractEnum))
                                                   .Cast<ContractEnum>()
                                                   .Select(x => new DropDownResponse
                                                   {
                                                       Key = ((int)x).ToString(),
                                                       Value = GetEnumDescription(x),
                                                       Type = (int)x
                                                   })
                                                   .ToList();
            return Result.Success(enumConvertList);
        }
        /// <summary>
        /// 考核
        /// </summary>
        /// <returns></returns>
        private static Result GetCheckList()
        {
            var enumConvertList = Enum.GetValues(typeof(CheckEnum))
                                                   .Cast<CheckEnum>()
                                                   .Select(x => new DropDownResponse
                                                   {
                                                       Key = ((int)x).ToString(),
                                                       Value = GetEnumDescription(x),
                                                       Type = (int)x
                                                   })
                                                   .ToList();
            return Result.Success(enumConvertList);
        }
        /// <summary>
        /// 船舶状态
        /// </summary>
        /// <returns></returns>
        private static Result GetShipStateList()
        {
            var enumConvertList = Enum.GetValues(typeof(ShipStateEnum))
                                                   .Cast<ShipStateEnum>()
                                                   .Select(x => new DropDownResponse
                                                   {
                                                       Key = ((int)x).ToString(),
                                                       Value = GetEnumDescription(x),
                                                       Type = (int)x
                                                   })
                                                   .ToList();
            return Result.Success(enumConvertList);
        }
        /// <summary>
        /// 船员状态
        /// </summary>
        /// <returns></returns>
        private static Result GetCrewStatusList()
        {
            var enumConvertList = Enum.GetValues(typeof(CrewStatusEnum))
                                                   .Cast<CrewStatusEnum>()
                                                   .Select(x => new DropDownResponse
                                                   {
                                                       Key = ((int)x).ToString(),
                                                       Value = GetEnumDescription(x),
                                                       Type = (int)x
                                                   })
                                                   .ToList();
            return Result.Success(enumConvertList);
        }
        /// <summary>
        /// 获取枚举项的描述信息
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string GetEnumDescription(Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            return attribute != null ? attribute.Description : value.ToString();
        }
        #endregion

        #region 详情
        /// <summary>
        /// 学历信息
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        public async Task<Result> GetEducationalBackgroundDetailsAsync(string bId)
        {
            EducationalBackgroundDetails ur = new();
            var userInfo = await GetUserInfoAsync(bId);
            var edutab = await _dbContext.Queryable<EducationalBackground>().Where(t => t.IsDelete == 1 && t.QualificationId == userInfo.BusinessId).ToListAsync();
            //获取文件
            var fileIds = edutab.Select(x => x.QualificationScans.ToString()).ToList();
            var files = await _dbContext.Queryable<Files>().Where(t => fileIds.Contains(t.FileId.ToString())).ToListAsync();
            List<QualificationForDetails> qd = new();
            //获取文件
            var url = AppsettingsHelper.GetValue("UpdateItem:Url");
            foreach (var item in edutab)
            {
                var edFile = files.Where(x => item.QualificationScans == x.FileId)
                    .Select(x => new FileInfosForDetails
                    {
                        Id = x.BusinessId.ToString(),
                        FileSize = x.FileSize,
                        FileType = x.FileType,
                        Name = x.Name,
                        OriginName = x.OriginName,
                        SuffixName = x.SuffixName,
                        Url = url + x.Name
                    })
                    .ToList();
                qd.Add(new QualificationForDetails
                {
                    Id = item.BusinessId.ToString(),
                    StartTime = item.StartTime?.ToString("yyyy-MM-dd"),
                    EndTime = item.EndTime?.ToString("yyyy-MM-dd"),
                    Major = item.Major,
                    QualificationType = item.QualificationType,
                    QualificationTypeName = EnumUtil.GetDescription(item.QualificationType),
                    School = item.School,
                    QualificationName = EnumUtil.GetDescription(item.Qualification),
                    Qualification = item.Qualification,
                    QualificationScans = edFile
                });
            }
            ur.QualificationInfos = qd;

            return Result.Success(ur);
        }
        /// <summary>
        /// 备注
        /// </summary>
        /// <param name="bId"></param>
        /// <param name="keyWords"></param>
        /// <returns></returns>
        public async Task<Result> GetNotesDetailsAsync(string bId, string? keyWords)
        {
            NotesDetails nd = new();

            var users = await _dbContext.Queryable<User>()
                .Select(t => new { t.BusinessId, t.Name }).ToListAsync();
            var uNotes = await _dbContext.Queryable<UserNotes>().Where(t => t.IsDelete == 1 && t.UserNoteId.ToString() == bId)
                .OrderByDescending(x => x.Created)
                .ToListAsync();

            List<NotesForDetails> rt = new();
            foreach (var item in uNotes)
            {
                rt.Add(new NotesForDetails
                {
                    Id = item.UserNoteId.ToString(),
                    Content = item.Content,
                    NoteTime = string.IsNullOrWhiteSpace(item.Modified.ToString()) ? item.Created?.ToString("yyyy-MM-dd HH:mm:ss") : item.Modified?.ToString("yyyy-MM-dd HH:mm:ss"),
                    UserName = users.FirstOrDefault(x => x.BusinessId.ToString() == item.WritedUserId)?.Name
                });
            }

            var notesForDetailList =
                rt.WhereIF(!string.IsNullOrWhiteSpace(keyWords), un => un.Content.Contains(keyWords) || un.UserName.Contains(keyWords)).ToList();
            nd.NotesForDetails = notesForDetailList;

            return Result.Success(nd);
        }
        /// <summary>
        /// 职务晋升
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        public async Task<Result> GetPromotionDetailsAsync(string bId)
        {
            PromotionDetails ur = new();

            var userInfo = await GetUserInfoAsync(bId);
            var promotion = await _dbContext.Queryable<Promotion>().Where(t => t.IsDelete == 1 && t.PromotionId == userInfo.BusinessId).OrderByDescending(x => x.PromotionTime).ToListAsync();
            //获取文件
            var url = AppsettingsHelper.GetValue("UpdateItem:Url");
            var fileIds = promotion.Select(x => x.PromotionScan.ToString()).ToList();
            //获取文件
            var files = await _dbContext.Queryable<Files>().Where(t => fileIds.Contains(t.FileId.ToString())).ToListAsync();
            var ownships = await _dbContext.Queryable<OwnerShip>().Where(t => t.IsDelete == 1).Select(x => new { x.BusinessId, x.ShipName, x.Country }).ToListAsync();
            var positions = await _dbContext.Queryable<Position>().Where(t => t.IsDelete == 1).Select(x => new { x.BusinessId, x.Name }).ToListAsync();
            List<PromotionsForDetails> pd = new();
            foreach (var item in promotion)
            {
                var pdFiles = files.Where(x => item.PromotionScan == x.FileId)
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
                     })
                    .ToList();
                pd.Add(new PromotionsForDetails
                {
                    OnShip = item.OnShip,
                    OnShipName = ownships.FirstOrDefault(x => x.BusinessId.ToString() == item.OnShip)?.ShipName,
                    PostitionName = positions.FirstOrDefault(x => x.BusinessId.ToString() == item.Postition)?.Name,
                    Postition = item.Postition,
                    PromotionTime = item.PromotionTime?.ToString("yyyy-MM-dd"),
                    PromotionScans = pdFiles
                });
            }
            ur.Promotions = pd;

            return Result.Success(ur);
        }
        /// <summary>
        /// 培训记录
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        public async Task<Result> GetTrainingRecordDetailsAsync(string bId)
        {
            TrainingRecordDetails ur = new();

            var userInfo = await GetUserInfoAsync(bId);

            var traningRecord = await _dbContext.Queryable<TrainingRecord>().Where(t => t.IsDelete == 1 && t.TrainingId == userInfo.BusinessId).OrderByDescending(x => x.TrainingTime).ToListAsync();
            var trainType = await _dbContext.Queryable<TrainingType>().Where(t => t.IsDelete == 1).Select(x => new { x.BusinessId, x.Name }).ToListAsync();
            //获取文件
            var fileIds = traningRecord.Select(x => x.TrainingScan.ToString()).ToList();
            var files = await _dbContext.Queryable<Files>().Where(t => fileIds.Contains(t.FileId.ToString())).ToListAsync();
            List<TrainingRecordsForDetails> td = new();
            //获取文件
            var url = AppsettingsHelper.GetValue("UpdateItem:Url");
            foreach (var item in traningRecord)
            {
                var trFile = files.Where(x => item.TrainingScan == x.FileId)
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
                td.Add(new TrainingRecordsForDetails
                {
                    TrainingTitle = item.TrainingTitle,
                    TrainingTime = item.TrainingTime?.ToString("yyyy-MM-dd"),
                    TrainingType = item.TrainingType,
                    TrainingTypeName = trainType.FirstOrDefault(x => x.BusinessId.ToString() == item.TrainingType)?.Name,
                    TrainingScans = trFile
                });
            }
            ur.TrainingRecords = td;

            return Result.Success(ur);
        }
        /// <summary>
        /// 任职船舶
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        public async Task<Result> GetWorkShipDetailsAsync(string bId)
        {
            WorkShipDetails ur = new();

            var userInfo = await GetUserInfoAsync(bId);
            var workShips = await _dbContext.Queryable<WorkShip>().Where(t => t.IsDelete == 1 && t.WorkShipId == userInfo.BusinessId).OrderByDescending(t => t.WorkShipStartTime).ToListAsync();
            var ownships = await _dbContext.Queryable<OwnerShip>().Where(t => t.IsDelete == 1).Select(x => new { x.BusinessId, x.ShipName, x.Country, x.ShipType }).ToListAsync();
            var positions = await _dbContext.Queryable<PositionOnBoard>().Where(t => t.IsDelete == 1).Select(x => new { x.BusinessId, x.Name }).ToListAsync();
            var country = await _dbContext.Queryable<CountryRegion>().Where(t => t.IsDelete == 1).ToListAsync();

            List<WorkShipsForDetails> wd = new();
            foreach (var item in workShips)
            {
                wd.Add(new WorkShipsForDetails
                {
                    WorkShipEndTime = item.WorkShipEndTime,
                    WorkShipEndTimeStr = item.WorkShipEndTime == null ? "至今" : string.Empty,
                    OnShipName = ownships.FirstOrDefault(x => x.BusinessId.ToString() == item.OnShip)?.ShipName,
                    OnShip = item.OnShip,
                    Postition = item.Postition,
                    PostitionName = positions.FirstOrDefault(x => x.BusinessId.ToString() == item.Postition)?.Name,
                    WorkShipStartTime = item.WorkShipStartTime,
                    OnBoardDay = string.IsNullOrWhiteSpace(item.WorkShipEndTime.ToString()) ? TimeHelper.GetTimeSpan(item.WorkShipStartTime, DateTime.Now).Days + 1 : TimeHelper.GetTimeSpan(item.WorkShipStartTime, item.WorkShipEndTime.Value).Days + 1,
                    ShipType = ownships.FirstOrDefault(x => x.BusinessId.ToString() == item.OnShip).ShipType,
                    ShipTypeName = EnumUtil.GetDescription(ownships.FirstOrDefault(x => x.BusinessId.ToString() == item.OnShip).ShipType),
                    Holiday = 0,
                    CountryName = country.FirstOrDefault(x => x.BusinessId == item.Country)?.Name,
                    ProjectName = item.ProjectName,
                    Country = item.Country
                });
            }
            ur.WorkShips = wd;

            return Result.Success(ur);
        }
        /// <summary>
        /// 年度考核
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        public async Task<Result> GetYearCheckDetailAsync(string bId)
        {
            YearCheckDetails ur = new();
            var userInfo = await GetUserInfoAsync(bId);
            var yearChecks = await _dbContext.Queryable<YearCheck>().Where(t => t.IsDelete == 1 && t.TrainingId == userInfo.BusinessId).OrderByDescending(x => x.TrainingTime).ToListAsync();
            //获取文件
            var fileIds = yearChecks.Select(x => x.TrainingScan.ToString()).ToList();
            var files = await _dbContext.Queryable<Files>().Where(t => fileIds.Contains(t.FileId.ToString())).ToListAsync();
            List<YearChecksForDetails> yc = new();
            //获取文件
            var url = AppsettingsHelper.GetValue("UpdateItem:Url");
            foreach (var item in yearChecks)
            {
                var ycFile = files.Where(x => item.TrainingScan == x.FileId)
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
                    })
                    .ToList();
                yc.Add(new YearChecksForDetails
                {
                    TrainingTime = item.TrainingTime?.ToString("yyyy-MM-dd"),
                    CheckType = item.CheckType,
                    CheckTypeName = EnumUtil.GetDescription(item.CheckType),
                    TrainingScans = ycFile
                });
            }
            ur.YearChecks = yc;

            return Result.Success(ur);
        }
        /// <summary>
        /// 适任证书
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        public async Task<Result> GetCertificateOfCompetencyDetailsAsync(string bId, CertificatesEnum type)
        {
            CertificateOfCompetencyDetails ur = new();
            List<FirstCertificateOfCompetencyDetailsDto> ff = new();
            List<SecondCertificateOfCompetencyDetailsDto> ss = new();
            List<TrainingCertificateDetailsDto> tt = new();
            List<HealthCertificateDetailsDto> hh = new();
            List<SeamanCertificateDetailsDto> seaea = new();
            List<PassportCertificateDetailsDto> hzhz = new();
            var userInfo = await GetUserInfoAsync(bId);
            //获取文件
            var url = AppsettingsHelper.GetValue("UpdateItem:Url");
            var cerOfComps = await _dbContext.Queryable<CertificateOfCompetency>().Where(t => t.IsDelete == 1 && t.CertificateId == userInfo.BusinessId).ToListAsync();
            if (cerOfComps.Any())
            {
                //文件
                var fileAll = await _dbContext.Queryable<Files>().Where(t => t.IsDelete == 1 && userInfo.BusinessId == t.UserId).ToListAsync();
                //航区
                var navigationarea = await _dbContext.Queryable<NavigationArea>().Where(t => t.IsDelete == 1).ToListAsync();
                //适任职务
                var position = await _dbContext.Queryable<Position>().Where(t => t.IsDelete == 1).ToListAsync();
                //技能证书
                var fResult = cerOfComps.Where(x => x.Type == CertificatesEnum.FCertificate);
                var f = type == CertificatesEnum.FCertificate ? fResult.OrderByDescending(x => x.FEffectiveTime).ToList() : fResult.OrderByDescending(x => x.FEffectiveTime).Take(1).ToList();
                if (f.Any())
                {
                    foreach (var item in f)
                    {
                        ff.Add(new FirstCertificateOfCompetencyDetailsDto
                        {
                            FCertificate = item.FCertificate,
                            FSignTime = item.FSignTime,
                            FEffectiveTime = item.FEffectiveTime,
                            FEffectiveCountdown = item?.FEffectiveTime == null ? 0 : DateTimeUtils.CalculateValidityDays(Convert.ToDateTime(item?.FEffectiveTime)),
                            FNavigationArea = item?.FNavigationArea,
                            FNavigationAreaName = navigationarea.FirstOrDefault(x => x.BusinessId.ToString() == item?.FNavigationArea)?.Name,
                            FPosition = item?.FPosition,
                            FPositionName = position.FirstOrDefault(x => x.BusinessId.ToString() == item?.FPosition)?.Name,
                            FScans = fileAll.Where(x => item?.FScans == x.FileId)
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
                            })
                            .ToList()
                        });
                    }
                }
                var sResult = cerOfComps.Where(x => x.Type == CertificatesEnum.SCertificate);
                var s = type == CertificatesEnum.SCertificate ? sResult.OrderByDescending(x => x.SEffectiveTime).ToList() : sResult.OrderByDescending(x => x.SEffectiveTime).Take(1).ToList();
                if (s.Any())
                {
                    foreach (var item in s)
                    {
                        ss.Add(new SecondCertificateOfCompetencyDetailsDto
                        {
                            SCertificate = item.SCertificate,
                            SEffectiveCountdown = item?.SEffectiveTime == null ? 0 : DateTimeUtils.CalculateValidityDays(Convert.ToDateTime(item?.SEffectiveTime)),
                            SEffectiveTime = item?.SEffectiveTime,
                            SNavigationArea = item?.SNavigationArea,
                            SNavigationAreaName = navigationarea.FirstOrDefault(x => x.BusinessId.ToString() == item?.SNavigationArea)?.Name,
                            SPosition = item?.SPosition,
                            SPositionName = position.FirstOrDefault(x => x.BusinessId.ToString() == item?.SPosition)?.Name,
                            SSignTime = item?.SSignTime,
                            SScans = fileAll.Where(x => item?.SScans == x.FileId)
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
                            })
                            .ToList()
                        });
                    }
                }
                var tResult = cerOfComps.Where(x => x.Type == CertificatesEnum.PXHGZ);
                var t = type == CertificatesEnum.PXHGZ ? tResult.OrderByDescending(x => x.TrainingSignTime).ToList() : tResult.OrderByDescending(x => x.TrainingSignTime).Take(1).ToList();
                if (t.Any())
                {
                    foreach (var item in t)
                    {
                        tt.Add(new TrainingCertificateDetailsDto
                        {
                            TrainingCertificate = item.TrainingCertificate,
                            TrainingSignTime = item.TrainingSignTime,
                            Z01EffectiveTime = item.Z01EffectiveTime,
                            Z07EffectiveTime = item.Z07EffectiveTime,
                            Z08EffectiveTime = item.Z08EffectiveTime,
                            Z04EffectiveTime = item.Z04EffectiveTime,
                            Z05EffectiveTime = item.Z05EffectiveTime,
                            Z02EffectiveTime = item.Z02EffectiveTime,
                            Z06EffectiveTime = item.Z06EffectiveTime,
                            Z09EffectiveTime = item.Z09EffectiveTime,
                            TrainingScans = fileAll.Where(x => item?.TrainingScans == x.FileId)
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
                           })
                           .ToList()
                        });
                    }
                }
                var hResult = cerOfComps.Where(x => x.Type == CertificatesEnum.JKZ);
                var h = type == CertificatesEnum.JKZ ? hResult.OrderByDescending(x => x.HealthEffectiveTime).ToList() : hResult.OrderByDescending(x => x.HealthEffectiveTime).Take(1).ToList();
                if (h.Any())
                {
                    foreach (var item in h)
                    {
                        hh.Add(new HealthCertificateDetailsDto
                        {
                            HealthCertificate = item.HealthCertificate,
                            HealthSignTime = item.HealthSignTime,
                            HealthEffectiveTime = item.HealthEffectiveTime,
                            HealthScans = fileAll.Where(x => item?.HealthScans == x.FileId)
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
                           })
                           .ToList()
                        });
                    }
                }
                var seaResult = cerOfComps.Where(x => x.Type == CertificatesEnum.HYZ);
                var sea = type == CertificatesEnum.HYZ ? seaResult.OrderByDescending(x => x.SeamanEffectiveTime).ToList() : seaResult.OrderByDescending(x => x.SeamanEffectiveTime).Take(1).ToList();
                if (sea.Any())
                {
                    foreach (var item in sea)
                    {
                        seaea.Add(new SeamanCertificateDetailsDto
                        {
                            SeamanCertificate = item.SeamanCertificate,
                            SeamanSignTime = item.SeamanSignTime,
                            SeamanEffectiveTime = item.SeamanEffectiveTime,
                            SeamanScans = fileAll.Where(x => item?.SeamanScans == x.FileId)
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
                           })
                           .ToList()
                        });
                    }
                }
                var hzResult = cerOfComps.Where(x => x.Type == CertificatesEnum.HZ);
                var hz = type == CertificatesEnum.HZ ? hzResult.OrderByDescending(x => x.PassportEffectiveTime).ToList() : hzResult.OrderByDescending(x => x.PassportEffectiveTime).Take(1).ToList();
                if (hz.Any())
                {
                    var visarecords = await _dbContext.Queryable<VisaRecords>().Where(t => t.IsDelete == 1 && t.VisareCordId == userInfo.BusinessId).OrderByDescending(x => x.DueTime).ToListAsync();
                    var countrys = await _dbContext.Queryable<CountryRegion>().Where(t => t.IsDelete == 1).Select(x => new { x.BusinessId, x.Name }).ToListAsync();
                    foreach (var item in hz)
                    {
                        //签证记录
                        List<VisaRecordsForDetails> vfd = new();
                        var vc = visarecords.Where(x => x.PassportCertificateId == item.BusinessId).ToList();
                        foreach (var vv in vc)
                        {
                            vfd.Add(new VisaRecordsForDetails
                            {
                                Id = vv.BusinessId.ToString(),
                                Country = vv.Country,
                                CountryName = countrys.FirstOrDefault(x => x.BusinessId.ToString() == vv.Country)?.Name,
                                DueTime = vv.DueTime,
                                VisaType = vv.VisaType,
                                VisaTypeName = EnumUtil.GetDescription(vv.VisaType),
                                IsDue = vv.DueTime >= DateTime.Now ? false : true
                            });
                        }
                        hzhz.Add(new PassportCertificateDetailsDto
                        {
                            PassportCertificate = item.PassportCertificate,
                            PassportSignTime = item.PassportSignTime,
                            PassportEffectiveTime = item.PassportEffectiveTime,
                            VisaRecords = vfd,
                            PassportScans = fileAll.Where(x => item?.PassportScans == x.FileId)
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
                           })
                           .ToList()
                        });
                    }
                }

                var skillcf = await _dbContext.Queryable<SkillCertificates>().Where(t => t.IsDelete == 1 && t.SkillcertificateId == userInfo.BusinessId).ToListAsync();
                List<Guid?> skillFileIds = new();
                foreach (var item in skillcf)
                {
                    var ids = item.SkillScans;
                    if (item.SkillScans.ToString() != null)
                    {
                        skillFileIds.Add(ids);
                    }
                }
                //特种设备证书
                var specFillResult = await _dbContext.Queryable<SpecialEquips>().Where(t => t.IsDelete == 1 && t.SpecialEquipId == userInfo.BusinessId).ToListAsync();
                var specFill = type == CertificatesEnum.TZ ? specFillResult.OrderByDescending(x => x.SpecialEquipsEffectiveTime).ToList() : specFillResult.OrderByDescending(x => x.SpecialEquipsEffectiveTime).Take(1).ToList();
                List<Guid?> specfilesIds = new();
                foreach (var item in specFill)
                {
                    var ids = item.SpecialEquipsScans;
                    if (ids != null)
                    {
                        specfilesIds.Add(ids);
                    }
                }
                List<Guid?> curFilesIds = new();
                curFilesIds.AddRange(skillFileIds);
                curFilesIds.AddRange(specfilesIds);

                #region 扫描件
                //技能证书
                List<SkillCertificatesForDetails> sk = new();
                foreach (var item in skillcf)
                {
                    var skFiles = fileAll.Where(x => x.FileId == item.SkillScans)
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
                        })
                        .FirstOrDefault();

                    sk.Add(new SkillCertificatesForDetails
                    {
                        Id = item.BusinessId.ToString(),
                        SkillCertificateType = item.SkillCertificateType,
                        SkillCertificateTypeName = EnumUtil.GetDescription(item.SkillCertificateType),
                        SkillScans = skFiles
                    });
                }
                //特种设备证书
                List<SpecialEquipsForDetails> sp = new();
                foreach (var item in specFill)
                {
                    var spFiles = fileAll.Where(x => x.FileId == item.SpecialEquipsScans)
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
                        })
                        .FirstOrDefault();

                    sp.Add(new SpecialEquipsForDetails
                    {
                        Id = item.BusinessId.ToString(),
                        AnnualReviewTime = item.AnnualReviewTime,
                        SpecialEquipsEffectiveTime = item.SpecialEquipsEffectiveTime,
                        SpecialEquipsCertificateType = item.SpecialEquipsCertificateType,
                        SpecialEquipsCertificateTypeName = EnumUtil.FetchDescription(item.SpecialEquipsCertificateType),
                        SpecialEquipsScans = spFiles
                    });
                }
                #endregion
                ur.SkillCertificates = sk;
                ur.SpecialEquips = sp;
                ur.ServiceBookType = userInfo.ServiceBookType;
                ur.FirstCertificateOfCompetencyDetailsDto = ff;
                ur.SecondCertificateOfCompetencyDetailsDto = ss;
                ur.TrainingCertificateDetailsDto = tt;
                ur.HealthCertificateDetailsDto = hh;
                ur.SeamanCertificateDetailsDto = seaea;
                ur.PassportCertificateDetailsDto = hzhz;
            }
            return Result.Success(ur);
        }
        /// <summary>
        /// 劳务详情
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        public async Task<Result> GetLaborServicesDetailsAsync(string bId)
        {
            LaborServicesInfoDetails ur = new();

            var userInfo = await GetUserInfoAsync(bId);
            //入职材料
            var userEntryInfo = await _dbContext.Queryable<UserEntryInfo>().Where(t => t.IsDelete == 1 && t.UserEntryId == userInfo.BusinessId).OrderByDescending(x => new { x.ContractType, x.EntryTime }).ToListAsync();
            var fIds = userEntryInfo.Select(x => x.EntryScans.ToString()).ToList();
            //用工类型
            var empType = await _dbContext.Queryable<EmploymentType>().Where(t => t.IsDelete == 1).ToListAsync();
            //读取用户入职的所有文件资料
            var files = await _dbContext.Queryable<Files>().Where(t => t.IsDelete == 1 && fIds.Contains(t.FileId.ToString())).ToListAsync();
            List<UserEntryInfosForDetails> uEntry = new();
            //获取文件
            var url = AppsettingsHelper.GetValue("UpdateItem:Url");
            foreach (var item in userEntryInfo)
            {
                uEntry.Add(new UserEntryInfosForDetails
                {
                    Id = item.BusinessId.ToString(),
                    ContarctMain = item.ContractMain,
                    EntryDate = item.ContractType == ContractEnum.NoFixedTerm ? "无固定期限合同" : item.StartTime?.ToString("yyyy-MM-dd") + "~" + item.EndTime?.ToString("yyyy-MM-dd"),
                    LaborCompany = item.LaborCompany,
                    ContractType = item.ContractType,
                    ContractTypeName = EnumUtil.GetDescription(item.ContractType),
                    EmploymentName = empType.FirstOrDefault(x => x.BusinessId.ToString() == item.EmploymentId)?.Name,
                    EmploymentId = item.EmploymentId,
                    Staus = item.ContractType == ContractEnum.NoFixedTerm ? "进行中" : item.EndTime >= DateTime.Now ? "进行中" : "已结束",
                    EntryTime = item.EntryTime,
                    EndTime = item.EndTime,
                    StartTime = item.StartTime
                });
            }

            //最新入职文件
            var newEntryFilesIds = userEntryInfo.FirstOrDefault()?.EntryScans;
            var newFiles = files
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
                })
                .ToList();
            ur = new LaborServicesInfoDetails
            {
                EntryTime = userEntryInfo.FirstOrDefault()?.EntryTime.ToString("yyyy-MM-dd"),
                EntryMaterial = newFiles,
                UserEntryInfosForDetails = uEntry
            };

            return Result.Success(ur);
        }
        /// <summary>
        /// 基本信息
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        public async Task<Result> GetBasesicDetailsAsync(string bId)
        {
            PageResult<BaseInfoDetails> rt = new();
            BaseInfoDetails ur = new();

            var userInfo = await GetUserInfoAsync(bId);
            //获取文件
            var url = AppsettingsHelper.GetValue("UpdateItem:Url");
            #region 匹配简易信息
            ur.Name = userInfo.Name;
            ur.WorkNumber = userInfo.WorkNumber;
            ur.Phone = userInfo.Phone;
            ur.CardId = userInfo.CardId;
            ur.HomeAddress = userInfo.HomeAddress;
            ur.BuildAddress = userInfo.BuildAddress;
            //服务簿类型
            ur.ServiceBookType = userInfo.ServiceBookType;
            ur.ServiceBookTypeName = EnumUtil.GetDescription(userInfo.ServiceBookType);
            #endregion

            #region 匹配链表字段

            var userWorkShip = await _dbContext.Queryable<WorkShip>().Where(t => t.IsDelete == 1 && t.WorkShipId == userInfo.BusinessId).OrderByDescending(t => t.WorkShipStartTime).FirstAsync();
            if (userWorkShip != null)
            {
                //用户状态
                ur.StatusName = EnumUtil.GetDescription(ShipUserStatus(userWorkShip.WorkShipStartTime, userWorkShip.WorkShipEndTime, userInfo.DeleteReson));//删除原因获取用户状态

                //当前船舶任职时间
                ur.CurrentShipEntryTime = string.IsNullOrWhiteSpace(userWorkShip.WorkShipStartTime.ToString()) || userWorkShip.WorkShipStartTime == DateTime.MinValue
                                        ? ""
                                        : $"{userWorkShip.WorkShipStartTime:yyyy-MM-dd}~{(userWorkShip.WorkShipEndTime?.ToString("yyyy-MM-dd") ?? "至今")}";

                //所在船舶  获取调任船舶最新数据
                var onBoard = await _dbContext.Queryable<OwnerShip>().Where(t => t.BusinessId.ToString() == userWorkShip.OnShip).FirstAsync();
                ur.OnBoardName = onBoard?.ShipName;
                ur.OnBoard = userWorkShip.OnShip;
                ur.ShipType = onBoard.ShipType;
                ur.ShipTypeName = EnumUtil.GetDescription(onBoard.ShipType);
                //  获取调任船舶最新数据
                //var position = await _dbContext.Queryable<PositionOnBoard>().FirstAsync(t => userWorkShip.Postition == t.BusinessId.ToString());

                //船员类型  取最新的在船职务
                var onBoardPosition = await _dbContext.Queryable<PositionOnBoard>().FirstAsync(t => t.IsDelete == 1 && t.BusinessId.ToString() == userWorkShip.Postition);
                ur.CrewTypeName = onBoardPosition?.Remark;
                ur.CrewType = userWorkShip.Postition;
                //在船职务
                ur.PositionOnBoard = onBoardPosition?.BusinessId.ToString();
                ur.PositionOnBoardName = onBoardPosition?.Name;
            }
            var userScans = await _dbContext.Queryable<Files>().Where(t => t.IsDelete == 1 && userInfo.BusinessId == t.UserId).ToListAsync();
            //照片
            var userPhoto = userScans.FirstOrDefault(x => x.FileId == userInfo.CrewPhoto);
            ur.PhotoScans = userPhoto == null ? null : new FileInfosForDetails
            {
                Id = userPhoto.BusinessId.ToString(),
                FileSize = userPhoto.FileSize,
                FileType = userPhoto.FileType,
                Name = userPhoto.Name,
                OriginName = userPhoto.OriginName,
                SuffixName = userPhoto.SuffixName,
                //Url = url + userPhoto.Name?.Substring(0, userPhoto.Name.LastIndexOf(".")) + userPhoto.OriginName
                Url = url + userPhoto.Name
            };
            //扫描件
            List<FileInfosForDetails> ui = new();
            foreach (var item in userScans.Where(t => t.IsDelete == 1 && userInfo.IdCardScans == t.FileId))
            {
                ui.Add(new FileInfosForDetails
                {
                    Id = item.BusinessId.ToString(),
                    FileSize = item.FileSize,
                    FileType = item.FileType,
                    Name = item.Name,
                    OriginName = item.OriginName,
                    SuffixName = item.SuffixName,
                    //Url = url + item.Name?.Substring(0, item.Name.LastIndexOf(".")) + item.OriginName
                    Url = url + item.Name
                });
            }
            ur.IdCardScans = ui;
            //政治面貌
            var politicalStatus = await _dbContext.Queryable<Political>().FirstAsync(t => t.IsDelete == 1 && userInfo.PoliticalStatus == t.BusinessId.ToString());
            ur.PoliticalStatusName = politicalStatus?.Name;
            ur.PoliticalStatus = userInfo.PoliticalStatus;
            //籍贯
            var nanivePlace = await _dbContext.Queryable<AdministrativeDivision>().FirstAsync(t => t.IsDelete == 1 && t.SupRegionalismCode == "0" && t.BusinessId.ToString() == userInfo.NativePlace);
            ur.NativePlace = userInfo.NativePlace;
            ur.NativePlaceName = nanivePlace?.Name;
            //民族
            var nation = await _dbContext.Queryable<Nation>().FirstAsync(t => t.IsDelete == 1 && t.BusinessId.ToString() == userInfo.Nation);
            ur.NationName = nation?.Name;
            ur.Nation = userInfo.Nation;

            //家庭成员
            var familyUser = await _dbContext.Queryable<FamilyUser>().Where(t => t.IsDelete == 1 && t.FamilyId == userInfo.BusinessId).ToListAsync();
            List<UserInfosForDetails> fu = new();
            foreach (var item in familyUser)
            {
                fu.Add(new UserInfosForDetails
                {
                    Id = item.BusinessId.ToString(),
                    Phone = item.Phone,
                    RelationShipName = EnumUtil.GetDescription(item.RelationShip),
                    RelationShip = item.RelationShip,
                    UserName = item.UserName,
                    WorkUnit = item.WorkUnit
                });
            }
            ur.HomeUser = fu;
            //紧急联系人
            var ecUser = await _dbContext.Queryable<EmergencyContacts>().Where(t => t.IsDelete == 1 && t.EmergencyContactId == userInfo.BusinessId).ToListAsync();
            List<UserInfosForDetails> eu = new();
            foreach (var item in ecUser)
            {
                eu.Add(new UserInfosForDetails
                {
                    Id = item.BusinessId.ToString(),
                    Phone = item.Phone,
                    RelationShip = item.RelationShip,
                    RelationShipName = EnumUtil.GetDescription(item.RelationShip),
                    UserName = item.UserName,
                    WorkUnit = item.WorkUnit
                });
            };
            ur.EmergencyContacts = eu;
            #endregion

            return Result.Success(ur); ;
        }
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        private async Task<User> GetUserInfoAsync(string bId)
        {
            return await _dbContext.Queryable<User>().FirstAsync(t => t.BusinessId.ToString() == bId);
        }
        #endregion

        #region 船员动态
        /// <summary>
        /// 船员动态列表
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        public async Task<PageResult<SearchCrewDynamics>> SearchCrewDynamicsAsync(CrewDynamicsRequest requestBody)
        {
            RefAsync<int> total = 0;
            var roleType = await _baseService.CurRoleType();
            if (roleType == -1) { return new PageResult<SearchCrewDynamics>(); }
            #region 船员关联
            var crewWorkShip = _dbContext.Queryable<WorkShip>()
                     .GroupBy(u => new { u.WorkShipId, u.OnShip })
                     .Select(t => new { t.WorkShipId, t.OnShip, WorkShipStartTime = SqlFunc.AggregateMax(t.WorkShipStartTime) });
            var wShip = _dbContext.Queryable<WorkShip>()
              .InnerJoin(crewWorkShip, (x, y) => x.WorkShipId == y.WorkShipId && x.OnShip == y.OnShip && x.WorkShipStartTime == y.WorkShipStartTime);
            #endregion

            var departureApplyUser = _dbContext.Queryable<DepartureApplyUser>()
                .InnerJoin<DepartureApply>((dau, da) => dau.ApplyCode == da.ApplyCode)
                .Where((dau, da) => da.ApproveStatus == ApproveStatus.Pass)
                .GroupBy((dau, da) => new { dau.UserId })
                .Select((dau, da) => new
                {
                    dau.UserId,
                    HolidayDays = SqlFunc.AggregateSum(SqlFunc.DateDiff(DateType.Day, Convert.ToDateTime(dau.RealDisembarkDate ?? dau.DisembarkDate), Convert.ToDateTime(dau.RealReturnShipDate ?? dau.ReturnShipDate)) + 1)
                });

            var rr = await _dbContext.Queryable<User>()
                .Where(t1 => t1.IsLoginUser == 1 && t1.IsDelete == 1)
                .WhereIF(!string.IsNullOrWhiteSpace(requestBody.KeyWords), t1 => t1.Name.Contains(requestBody.KeyWords) || t1.Phone.Contains(requestBody.KeyWords) || t1.WorkNumber.Contains(requestBody.KeyWords) || t1.CardId.Contains(requestBody.KeyWords))
                .LeftJoin(departureApplyUser, (t1, dau) => t1.BusinessId == dau.UserId)
                .InnerJoin(wShip, (t1, dau, t5) => t1.BusinessId == t5.WorkShipId)
                .InnerJoin<OwnerShip>((t1, dau, t5, t3) => t5.OnShip == t3.BusinessId.ToString())
                .WhereIF(roleType == 3 || roleType == 4, (t1, dau, t5, t3) => GlobalCurrentUser.ShipId.ToString() == t5.OnShip)
                .WhereIF(!string.IsNullOrWhiteSpace(requestBody.StartTime.ToString()), (t1, dau, t5, t3) => t5.WorkShipStartTime >= requestBody.StartTime && t5.WorkShipStartTime <= requestBody.EndTime)
                .WhereIF(string.IsNullOrWhiteSpace(requestBody.StartTime.ToString()), (t1, dau, t5, t3) => t5.WorkShipStartTime.Year == DateTime.Now.Year)
                .WhereIF(requestBody.BoardingDays != 0, (t1, dau, t5, t3) => SqlFunc.DateDiff(DateType.Day, Convert.ToDateTime(t5.WorkShipStartTime), DateTime.Now) >= requestBody.BoardingDays)//在船天数
                .WhereIF(requestBody.HolidayDays != 0, (t1, dau, t5, t3) => dau.HolidayDays >= requestBody.HolidayDays)
                .Select((t1, dau, t5, t3) => new SearchCrewDynamics
                {
                    BId = t1.BusinessId.ToString(),
                    Country = t3.Country,
                    OnBoard = t5.OnShip,
                    ShipType = t3.ShipType,
                    UserName = t1.Name,
                    WorkNumber = t1.WorkNumber,
                    DeleteResonEnum = t1.DeleteReson,
                    BoardingTime = t5.WorkShipStartTime.ToString("yyyy-MM-dd"),
                    DisembarkTime = t5.WorkShipEndTime == null ? "" : t5.WorkShipEndTime.Value.ToString("yyyy-MM-dd"),
                    LBoardingTime = t5.WorkShipStartTime,
                    LDisembarkTime = t5.WorkShipEndTime,
                    CardId = t1.CardId,
                    HolidayDays = dau.HolidayDays,
                    OnBoardDays = SqlFunc.DateDiff(DateType.Day, Convert.ToDateTime(t5.WorkShipStartTime), DateTime.Now),
                    Created = t1.Created,
                    StatusOrder = (DateTime.Now < t5.WorkShipStartTime ? "休假" : t5.WorkShipEndTime != null && DateTime.Now > t5.WorkShipEndTime ? "休假" : string.Empty) == "休假" ? 1 :t1.DeleteReson != 0 ? (int)t1.DeleteReson + 2 : (int)t1.DeleteReson
                })
                .MergeTable()
                .OrderBy(t1 => new { t1.StatusOrder, Created = SqlFunc.Desc(t1.Created) })
                .ToPageListAsync(requestBody.PageIndex, requestBody.PageSize, total);
            return await GetCrewDynamicsAsync(rr, total);

        }
        /// <summary>
        /// 获取结果集
        /// </summary>
        /// <param name="rr"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        private async Task<PageResult<SearchCrewDynamics>> GetCrewDynamicsAsync(List<SearchCrewDynamics> rr, int total)
        {
            PageResult<SearchCrewDynamics> rt = new();

            var ownShipTable = await _dbContext.Queryable<OwnerShip>().Where(t => rr.Select(x => x.OnBoard).Contains(t.BusinessId.ToString())).ToListAsync();
            var countryTable = await _dbContext.Queryable<CountryRegion>().Where(t => rr.Select(x => x.Country).Contains(t.BusinessId)).ToListAsync();
            //上下船次数
            var bids = rr.Select(y => y.BId).ToList();
            /*var disembark = await _dbContext.Queryable<DepartureApplication>()
                .Where(x => x.IsDelete == 1 && x.ApproveStatus == ApproveStatus.PExamination && bids.Contains(x.UserId.ToString()))
                .ToListAsync();*/
            var disembark = await _dbContext.Queryable<DepartureApplyUser>()
                .InnerJoin<DepartureApply>((dau, da) => dau.ApplyCode == da.ApplyCode)
                .Where((dau, da) => dau.IsDelete == 1 && da.ApproveStatus == ApproveStatus.Pass && bids.Contains(dau.UserId.ToString()))
                .ToListAsync();
            foreach (var u in rr)
            {
                u.OnBoardName = ownShipTable.FirstOrDefault(x => x.BusinessId.ToString() == u.OnBoard)?.ShipName;
                u.CountryName = countryTable.FirstOrDefault(x => x.BusinessId == u.Country)?.Name;
                u.ShipTypeName = EnumUtil.GetDescription(u.ShipType);
                u.Age = _baseService.CalculateAgeFromIdCard(u.CardId);
                var db = disembark.Where(x => x.UserId.ToString() == u.BId).ToList();
                u.BoardingNums = db.Count();
                u.DisembarkNums = u.BoardingNums == 0 ? 0 : u.BoardingNums - 1;
                /*var dk = disembark.Where(x => x.UserId.ToString() == u.BId).OrderByDescending(x => x.RealDisembarkTime).FirstOrDefault();
                if (dk != null && dk.RealDisembarkTime != null && dk.RealReturnShipTime != null)
                    u.HolidayDays = TimeHelper.GetTimeSpan(dk.RealDisembarkTime.Value, dk.RealReturnShipTime.Value).Days + 1;*/
                u.OnStatus = EnumUtil.GetDescription(_baseService.ShipUserStatus(u.LBoardingTime, u.LDisembarkTime, u.DeleteResonEnum));
            }

            rt.List = rr.ToList();
            rt.TotalCount = total;
            return rt;
        }


        #endregion
    }
}
