using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Dtos;
using HNKC.CrewManagePlatform.Models.Dtos.CrewArchives;
using HNKC.CrewManagePlatform.Models.Enums;
using HNKC.CrewManagePlatform.SqlSugars.Models;
using HNKC.CrewManagePlatform.Utils;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace HNKC.CrewManagePlatform.Services.Interface.CrewArchives
{
    /// <summary>
    /// 船员档案
    /// </summary>
    public class CrewArchivesService : ICrewArchivesService
    {
        private ISqlSugarClient _dbContext { get; set; }
        /// <summary>
        /// 注入
        /// </summary>
        /// <param name="dbContext"></param>
        public CrewArchivesService(ISqlSugarClient dbContext)
        {
            this._dbContext = dbContext;
        }
        #region 船员档案列表
        /// <summary>
        /// 船员档案列表
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        public async Task<ResponsePageResult<List<SearchCrewArchivesResponse>>> SearchCrewArchivesAsync(SearchCrewArchivesRequest requestBody)
        {
            //名称相关不赋值
            var rt = await _dbContext.Queryable<User>()
                .WhereIF(!string.IsNullOrWhiteSpace(requestBody.KeyWords), t => t.Name.Contains(requestBody.KeyWords) || t.CardId.Contains(requestBody.KeyWords)
                || t.Phone.Contains(requestBody.KeyWords) || t.WorkNumber.Contains(requestBody.KeyWords))
                .WhereIF(!string.IsNullOrWhiteSpace(requestBody.Name), t => t.Name.Contains(requestBody.Name))
                .WhereIF(!string.IsNullOrWhiteSpace(requestBody.CardId), t => t.CardId.Contains(requestBody.CardId))
                .WhereIF(!string.IsNullOrWhiteSpace(requestBody.WorkNumber), t => t.WorkNumber.Contains(requestBody.WorkNumber))
                .WhereIF(!string.IsNullOrWhiteSpace(requestBody.Phone), t => t.Phone.Contains(requestBody.Phone))
                .WhereIF(!string.IsNullOrWhiteSpace(requestBody.OnBoard), t => t.OnBoard.Contains(requestBody.OnBoard))
                .WhereIF(!string.IsNullOrWhiteSpace(requestBody.CrewType), t => t.CrewType.Contains(requestBody.CrewType))
                .LeftJoin<WorkShip>((t, ws) => t.BusinessId == ws.BusinessId)
                .LeftJoin<CrewType>((t, ws, ct) => t.BusinessId == ct.BusinessId)
                .LeftJoin<CertificateOfCompetency>((t, ws, ct, coc) => t.BusinessId == coc.BusinessId)
                .LeftJoin<SkillCertificates>((t, ws, ct, coc, sf) => t.BusinessId == sf.BusinessId)
                .LeftJoin<OwnerShip>((t, ws, ct, coc, sf, ow) => ws.OnShip == sf.BusinessId.ToString())
                .LeftJoin<EducationalBackground>((t, ws, ct, coc, sf, ow, eb) => eb.BusinessId == t.BusinessId)
                .WhereIF(!string.IsNullOrWhiteSpace(requestBody.HistoryOnBoard), (t, ws, ct, coc, sf, ow, eb) => ws.OnShip.Contains(requestBody.HistoryOnBoard))
                .WhereIF(requestBody.ShipTypes != null && requestBody.ShipTypes.Any(), (t, ws, ct, coc, sf, ow, eb) => requestBody.ShipTypes.Contains(((int)ow.ShipType).ToString()))
                .WhereIF(requestBody.ServiceBooks != null && requestBody.ServiceBooks.Any(), (t, ws, ct, coc, sf, ow, eb) => requestBody.ServiceBooks.Contains(((int)t.ServiceBookType).ToString()))
                .WhereIF(requestBody.FPosition != null && requestBody.FPosition.Any(), (t, ws, ct, coc, sf, ow, eb) => requestBody.FPosition.Contains(coc.FPosition))
                .WhereIF(requestBody.SPosition != null && requestBody.SPosition.Any(), (t, ws, ct, coc, sf, ow, eb) => requestBody.SPosition.Contains(coc.SPosition))
                .WhereIF(requestBody.TrainingCertificate, (t, ws, ct, coc, sf, ow, eb) => !string.IsNullOrWhiteSpace(coc.TrainingCertificate))
                .WhereIF(requestBody.Z01Effective, (t, ws, ct, coc, sf, ow, eb) => !string.IsNullOrWhiteSpace(coc.Z01EffectiveTime.ToString()))
                .WhereIF(requestBody.Z07Effective, (t, ws, ct, coc, sf, ow, eb) => !string.IsNullOrWhiteSpace(coc.Z07EffectiveTime.ToString()))
                .WhereIF(requestBody.Z08Effective, (t, ws, ct, coc, sf, ow, eb) => !string.IsNullOrWhiteSpace(coc.Z08EffectiveTime.ToString()))
                .WhereIF(requestBody.Z04Effective, (t, ws, ct, coc, sf, ow, eb) => !string.IsNullOrWhiteSpace(coc.Z04EffectiveTime.ToString()))
                .WhereIF(requestBody.Z05Effective, (t, ws, ct, coc, sf, ow, eb) => !string.IsNullOrWhiteSpace(coc.Z05EffectiveTime.ToString()))
                .WhereIF(requestBody.Z02Effective, (t, ws, ct, coc, sf, ow, eb) => !string.IsNullOrWhiteSpace(coc.Z02EffectiveTime.ToString()))
                .WhereIF(requestBody.Z06Effective, (t, ws, ct, coc, sf, ow, eb) => !string.IsNullOrWhiteSpace(coc.Z06EffectiveTime.ToString()))
                .WhereIF(requestBody.Z09Effective, (t, ws, ct, coc, sf, ow, eb) => !string.IsNullOrWhiteSpace(coc.Z09EffectiveTime.ToString()))
                .WhereIF(requestBody.SeamanCertificate, (t, ws, ct, coc, sf, ow, eb) => !string.IsNullOrWhiteSpace(coc.SeamanCertificate))
                .WhereIF(requestBody.PassportCertificate, (t, ws, ct, coc, sf, ow, eb) => !string.IsNullOrWhiteSpace(coc.PassportCertificate))
                .WhereIF(requestBody.HealthCertificate, (t, ws, ct, coc, sf, ow, eb) => !string.IsNullOrWhiteSpace(coc.HealthCertificate))
                .WhereIF(requestBody.CertificateTypes != null && requestBody.CertificateTypes.Any(), (t, ws, ct, coc, sf, ow, eb) => requestBody.CertificateTypes.Contains(((int)sf.SkillCertificateType).ToString()))
                .WhereIF(requestBody.QualificationTypes != null && requestBody.QualificationTypes.Any(), (t, ws, ct, coc, sf, ow, eb) => requestBody.QualificationTypes.Contains(((int)eb.QualificationType).ToString()))
                .WhereIF(requestBody.Qualifications != null && requestBody.Qualifications.Any(), (t, ws, ct, coc, sf, ow, eb) => requestBody.Qualifications.Contains(((int)eb.Qualification).ToString()))
                .Select((t, ws, ct, coc, sf, ow, eb) => new SearchCrewArchivesResponse
                {
                    BId = t.BusinessId,
                    BtnType = t.IsDelete == 0 ? 1 : 0,
                    UserName = t.Name,
                    CardId = t.CardId,
                    ShipType = t.ShipType,
                    WorkNumber = t.WorkNumber,
                    //EmploymentType = t.EmploymentId,
                    ServiceBookType = t.ServiceBookType,
                    CrewType = t.CrewType,
                    IsDelete = t.IsDelete,
                    DeleteReson = t.DeleteReson
                })
                .OrderByDescending(t => t.IsDelete)
                .ToListAsync();
            return await GetResult(requestBody, rt);
        }
        /// <summary>
        /// 获取列表结果集
        /// </summary>
        /// <param name="requestBody"></param>
        /// <param name="rt"></param>
        /// <returns></returns>
        private async Task<ResponsePageResult<List<SearchCrewArchivesResponse>>> GetResult(SearchCrewArchivesRequest requestBody, List<SearchCrewArchivesResponse> rt)
        {
            ResponsePageResult<List<SearchCrewArchivesResponse>> rr = new();
            if (rt.Any())
            {
                var uIds = rt.Select(x => x.BId).ToList();
                //在职状态
                var onBoardtab = await _dbContext.Queryable<WorkShip>().Where(t => t.IsDelete == 1 && uIds.Contains(t.BusinessId)).ToListAsync();
                var ownerShipstab = await _dbContext.Queryable<OwnerShip>().Where(t => t.IsDelete == 1).ToListAsync();
                //国家地区
                var countrytab = await _dbContext.Queryable<CountryRegion>().Where(t => t.IsDelete == 1).ToListAsync();
                //船员类型
                var crewTypetab = await _dbContext.Queryable<CrewType>().Where(t => t.IsDelete == 1).ToListAsync();
                //用工类型
                var emptCodes = rt.Select(x => x.EmploymentType).ToList();
                var emptab = await _dbContext.Queryable<EmploymentType>().Where(t => t.IsDelete == 1 && emptCodes.Contains(t.BusinessId.ToString())).ToListAsync();
                //第一适任 第二适任
                var firSectab = await _dbContext.Queryable<CertificateOfCompetency>().Where(t => uIds.Contains(t.BusinessId)).ToListAsync();
                var firSecPosition = await _dbContext.Queryable<Position>().Where(t => t.IsDelete == 1).ToListAsync();
                //技能证书
                var sctab = await _dbContext.Queryable<SkillCertificates>().Where(t => t.IsDelete == 1 && uIds.Contains(t.BusinessId)).ToListAsync();
                //特设证书
                var spctab = await _dbContext.Queryable<SpecialEquips>().Where(t => t.IsDelete == 1 && uIds.Contains(t.BusinessId)).ToListAsync();

                //名称赋值
                foreach (var t in rt)
                {
                    var sctabNames = "";//技能证书
                    var sctabs = new List<string>();//技能证书
                    var spctabNames = "";//特设证书
                    var spctabs = new List<string>();//特设证书
                    foreach (var s in sctab.Where(x => x.BusinessId == t.BId))
                    {
                        var val = (int)s.SkillCertificateType;
                        sctabs.Add(val.ToString());
                        sctabNames += EnumUtil.GetDescription(s.SkillCertificateType) + "、";
                    }
                    if (!string.IsNullOrEmpty(sctabNames))
                    {
                        sctabNames = sctabNames.Substring(0, sctabNames.Length - 1);
                    }
                    foreach (var s in spctab.Where(x => x.BusinessId == t.BId))
                    {
                        var val = (int)s.SpecialEquipsCertificateType;
                        spctabs.Add(val.ToString());
                        spctabNames += EnumUtil.GetDescription(s.SpecialEquipsCertificateType) + "、";
                    }
                    if (!string.IsNullOrEmpty(spctabNames))
                    {
                        spctabNames = spctabNames.Substring(0, spctabNames.Length - 1);
                    }
                    var ob = onBoardtab.Where(x => x.BusinessId == t.BId).OrderByDescending(x => x.Created).FirstOrDefault();
                    if (ob != null)
                    {
                        //所在船舶
                        var ownerShips = ownerShipstab.FirstOrDefault(x => ob.OnShip == x.BusinessId.ToString());
                        t.OnBoardName = ownerShips?.ShipName;
                        t.OnBoard = ob.OnShip;
                        if (ownerShips != null)
                        {
                            //所在国家
                            var country = countrytab.FirstOrDefault(x => ownerShips.Country == x.BusinessId.ToString());
                            t.OnCountry = ownerShips.Country;
                            t.OnCountryName = country?.Name;
                        }
                    }
                    t.ShipTypeName = EnumUtil.GetDescription(t.ShipType);
                    t.EmploymentTypeName = emptab.FirstOrDefault(x => x.BusinessId.ToString() == t.EmploymentType)?.Name;
                    t.CrewTypeName = crewTypetab.FirstOrDefault(x => t.CrewType == x.BusinessId.ToString())?.Name;
                    t.FCertificate = firSectab?.FirstOrDefault(x => x.BusinessId == t.BId)?.FPosition;
                    if (t.FCertificate != null) t.FCertificateName = firSecPosition.FirstOrDefault(x => x.BusinessId.ToString() == t.FCertificate)?.Name;
                    t.SCertificate = firSectab?.FirstOrDefault(x => x.BusinessId == t.BId)?.SPosition;
                    if (t.SCertificate != null) t.SCertificateName = firSecPosition.FirstOrDefault(x => x.BusinessId.ToString() == t.SCertificate)?.Name;
                    t.ServiceBookName = EnumUtil.GetDescription(t.ServiceBookType);
                    t.SkillsCertificateName = sctabNames;
                    t.SkillsCertificate = sctabs;
                    t.SpecialCertificate = spctabs;
                    t.SpecialCertificateName = spctabNames;
                    t.Age = CalculateAgeFromIdCard(t.CardId);
                    t.OnStatus = ob == null ? CrewStatusEnum.Normal : ShipUserStatus(ob.WorkShipEndTime, ob.HolidayTime, t.DeleteReson);
                    t.OnStatusName = ob == null ? null : EnumUtil.GetDescription(ShipUserStatus(ob.WorkShipEndTime, ob.HolidayTime, t.DeleteReson));
                }
            }

            return rr.SuccessPageResult(rt.Skip((requestBody.PageIndex - 1) * requestBody.PageSize).Take(requestBody.PageSize).ToList(), requestBody.PageIndex, requestBody.PageSize, rt.Count);
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
        /// <param name="holidayTime">休假时间</param>
        /// <param name="deleteResonEnum">是否删除</param>
        /// <returns></returns>
        private static CrewStatusEnum ShipUserStatus(DateTime departureTime, DateTime? holidayTime, CrewStatusEnum deleteResonEnum)
        {
            var status = new CrewStatusEnum();
            if (deleteResonEnum != CrewStatusEnum.Normal)
            {
                //删除：管理人员手动操作，包括离职、调离和退休，优先于其他任何状态
                status = deleteResonEnum;
            }
            else if (holidayTime.HasValue)
            {
                //休假：提交离船申请且经审批同意后，按所申请离船、归船日期设置为休假状态，优先于在岗、待岗状态
                status = CrewStatusEnum.XiuJia;
            }
            else if (departureTime <= DateTime.Now)
            {
                //在岗、待岗:船员登记时必填任职船舶数据，看其中最新的任职船舶上船时间和下船时间，在此时间内为在岗状态，否则为待岗状态
                status = CrewStatusEnum.Normal;
            }
            return status;
        }

        #endregion
        /// <summary>
        /// 首页占比及统计数
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseResult<CrewArchivesResponse>> CrewArchivesCountAsync()
        {
            ResponseResult<CrewArchivesResponse> rr = new();

            var udtab = await _dbContext.Queryable<User>().ToListAsync();
            var udtabIds = udtab.Select(x => x.BusinessId).ToList();
            var onBoard = await _dbContext.Queryable<WorkShip>().Where(t => t.IsDelete == 1 && udtabIds.Contains(t.BusinessId)).ToListAsync();

            var totalCount = udtab.Count();//总数

            var onDutyCount = onBoard.Where(x => x.WorkShipEndTime <= DateTime.Now).Select(x => x.BusinessId).Distinct().Count();//在船数
            var onDutyProp = totalCount == 0 ? 0 : Convert.ToInt32(onDutyCount / totalCount);

            var waitCount = onBoard.Where(x => x.WorkShipEndTime > DateTime.Now).Select(x => x.BusinessId).Distinct().Count();//待岗数
            var waitProp = totalCount == 0 ? 0 : Convert.ToInt32(waitCount / totalCount);

            var holidayCount = onBoard.Where(x => x.HolidayTime > DateTime.Now).Select(x => x.BusinessId).Distinct().Count();//休假数
            var holidayProp = totalCount == 0 ? 0 : Convert.ToInt32(holidayCount / totalCount);

            var otherCount = udtab.Where(x => x.DeleteReson != CrewStatusEnum.Normal && x.DeleteReson != CrewStatusEnum.XiuJia).Count();//离调退
            var otherProp = totalCount == 0 ? 0 : Convert.ToInt32(otherCount / totalCount);

            return rr.SuccessResult(new CrewArchivesResponse
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
            }, 1);
        }

        #region 数据增改
        /// <summary>
        /// 数据保存
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        public async Task<ResponseResult<bool>> SaveDataAsync(CrewArchivesRequest requestBody)
        {
            if (requestBody.BId == Guid.Empty || string.IsNullOrWhiteSpace(requestBody.BId.ToString())) { return await InsertDataAsync(requestBody); }
            else { return await UpdateDataAsync(requestBody); }
        }
        /// <summary>
        /// 数据新增
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        private async Task<ResponseResult<bool>> InsertDataAsync(CrewArchivesRequest requestBody)
        {
            ResponseResult<bool> rr = new();

            try
            {
                #region 基本信息
                await _dbContext.Ado.BeginTranAsync();

                User userInfo = new();
                List<FamilyUser> hus = new();
                List<EmergencyContacts> ecs = new();
                UserEntryInfo userEntry = new();
                var uId = GuidUtil.Next();
                if (requestBody.BaseInfoDto != null)
                {
                    //校验用户已经存在
                    var existUi = await _dbContext.Queryable<User>().FirstAsync(t => t.IsDelete == 1 && requestBody.BaseInfoDto.WorkNumber == t.WorkNumber || requestBody.BaseInfoDto.Phone == t.Phone || requestBody.BaseInfoDto.CardId == t.CardId);
                    if (existUi != null)
                    {
                        rr.Fail("用户存在");
                        return rr;
                    }
                    userInfo = new()
                    {
                        Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                        BuildAddress = requestBody.BaseInfoDto.BuildAddress,
                        BusinessId = uId,
                        CardId = requestBody.BaseInfoDto.CardId,
                        WorkNumber = requestBody.BaseInfoDto.WorkNumber,
                        Phone = requestBody.BaseInfoDto.Phone,
                        CrewPhoto = requestBody.BaseInfoDto.PhotoScans,
                        IdCardScans = requestBody.BaseInfoDto.IdCardScans,
                        PoliticalStatus = requestBody.BaseInfoDto.PoliticalStatus,
                        NativePlace = requestBody.BaseInfoDto.NativePlace,
                        Nation = requestBody.BaseInfoDto.Nation,
                        HomeAddress = requestBody.BaseInfoDto.HomeAddress,
                        ShipType = requestBody.BaseInfoDto.ShipType,
                        CrewType = requestBody.BaseInfoDto.CrewType,
                        ServiceBookType = requestBody.BaseInfoDto.ServiceBookType,
                        OnBoard = requestBody.BaseInfoDto.OnBoard,
                        PositionOnBoard = requestBody.BaseInfoDto.PositionOnBoard,
                        ContarctType = requestBody.BaseInfoDto.ContarctType,
                        Name = requestBody.BaseInfoDto.Name
                    };
                    //劳务合同
                    if (requestBody.BaseInfoDto.UserEntryInfo != null)
                    {
                        userEntry = new UserEntryInfo
                        {
                            BusinessId = userInfo.BusinessId,
                            Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                            ContarctMain = requestBody.BaseInfoDto.UserEntryInfo.ContarctMain,
                            EndTime = requestBody.BaseInfoDto.UserEntryInfo.EndTime,
                            EntryScans = requestBody.BaseInfoDto.UserEntryInfo.EntryScans,
                            EntryTime = requestBody.BaseInfoDto.UserEntryInfo.EntryTime,
                            LaborCompany = requestBody.BaseInfoDto.UserEntryInfo.LaborCompany,
                            EmploymentId = requestBody.BaseInfoDto.UserEntryInfo.EmploymentId
                        };
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
                                rr.Fail("家庭成员已经绑定过，请先删除该成员/注销");
                                return rr;
                            }
                            hus.Add(new FamilyUser
                            {
                                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                                BusinessId = uId,
                                Phone = item.Phone,
                                RelationShip = item.RelationShip,
                                UserName = item.UserName,
                                WorkUnit = item.WorkUnit
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
                                rr.Fail("应急联系人已经绑定过，请先删除该成员/注销");
                                return rr;
                            }
                            ecs.Add(new EmergencyContacts
                            {
                                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                                BusinessId = uId,
                                Phone = item.Phone,
                                RelationShip = item.RelationShip,
                                UserName = item.UserName,
                                WorkUnit = item.WorkUnit
                            });
                        }
                    }
                }
                await _dbContext.Insertable(userInfo).ExecuteCommandAsync();
                await _dbContext.Insertable(userEntry).ExecuteCommandAsync();
                await _dbContext.Insertable(hus).ExecuteCommandAsync();
                await _dbContext.Insertable(ecs).ExecuteCommandAsync();
                #endregion

                #region 适任及证书
                CertificateOfCompetency coc = new();
                List<VisaRecords> vrs = new();
                List<SkillCertificates> sfs = new();
                List<SpecialEquips> ses = new();
                if (requestBody.CertificateOfCompetencyDto != null)
                {
                    coc = new CertificateOfCompetency
                    {
                        BusinessId = uId,
                        Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                        FCertificate = requestBody.CertificateOfCompetencyDto.FCertificate,
                        FNavigationArea = requestBody.CertificateOfCompetencyDto.FNavigationArea,
                        FPosition = requestBody.CertificateOfCompetencyDto.FPosition,
                        FSignTime = requestBody.CertificateOfCompetencyDto.FSignTime,
                        FEffectiveTime = requestBody.CertificateOfCompetencyDto.FEffectiveTime,
                        FScans = requestBody.CertificateOfCompetencyDto.FScans,
                        SCertificate = requestBody.CertificateOfCompetencyDto.SCertificate,
                        SNavigationArea = requestBody.CertificateOfCompetencyDto.SNavigationArea,
                        SPosition = requestBody.CertificateOfCompetencyDto.SPosition,
                        SSignTime = requestBody.CertificateOfCompetencyDto.SSignTime,
                        SEffectiveTime = requestBody.CertificateOfCompetencyDto.SEffectiveTime,
                        SScans = requestBody.CertificateOfCompetencyDto.SScans,
                        TrainingCertificate = requestBody.CertificateOfCompetencyDto.TrainingCertificate,
                        TrainingSignTime = requestBody.CertificateOfCompetencyDto.TrainingSignTime,
                        TrainingScans = requestBody.CertificateOfCompetencyDto.TrainingScans,
                        Z01EffectiveTime = requestBody.CertificateOfCompetencyDto.Z01EffectiveTime,
                        Z07EffectiveTime = requestBody.CertificateOfCompetencyDto.Z07EffectiveTime,
                        Z08EffectiveTime = requestBody.CertificateOfCompetencyDto.Z08EffectiveTime,
                        Z04EffectiveTime = requestBody.CertificateOfCompetencyDto.Z04EffectiveTime,
                        Z05EffectiveTime = requestBody.CertificateOfCompetencyDto.Z05EffectiveTime,
                        Z02EffectiveTime = requestBody.CertificateOfCompetencyDto.Z02EffectiveTime,
                        Z06EffectiveTime = requestBody.CertificateOfCompetencyDto.Z06EffectiveTime,
                        Z09EffectiveTime = requestBody.CertificateOfCompetencyDto.Z09EffectiveTime,
                        HealthCertificate = requestBody.CertificateOfCompetencyDto.HealthCertificate,
                        HealthSignTime = requestBody.CertificateOfCompetencyDto.HealthSignTime,
                        HealthEffectiveTime = requestBody.CertificateOfCompetencyDto.HealthEffectiveTime,
                        HealthScans = requestBody.CertificateOfCompetencyDto.HealthScans,
                        SeamanCertificate = requestBody.CertificateOfCompetencyDto.SeamanCertificate,
                        SeamanSignTime = requestBody.CertificateOfCompetencyDto.SeamanSignTime,
                        SeamanEffectiveTime = requestBody.CertificateOfCompetencyDto.SeamanEffectiveTime,
                        SeamanScans = requestBody.CertificateOfCompetencyDto.SeamanScans,
                        PassportCertificate = requestBody.CertificateOfCompetencyDto.PassportCertificate,
                        PassportSignTime = requestBody.CertificateOfCompetencyDto.PassportSignTime,
                        PassportEffectiveTime = requestBody.CertificateOfCompetencyDto.PassportEffectiveTime,
                        PassportScans = requestBody.CertificateOfCompetencyDto.PassportScans
                    };

                    //签证记录
                    if (requestBody.CertificateOfCompetencyDto.VisaRecords != null && requestBody.CertificateOfCompetencyDto.VisaRecords.Any())
                    {
                        foreach (var item in requestBody.CertificateOfCompetencyDto.VisaRecords)
                        {
                            vrs.Add(new VisaRecords
                            {
                                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                                BusinessId = uId,
                                Country = item.Country,
                                DueTime = item.DueTime,
                                VisaType = item.VisaType,
                            });
                        }
                    }
                    //技能证书
                    if (requestBody.CertificateOfCompetencyDto.SkillCertificates != null && requestBody.CertificateOfCompetencyDto.SkillCertificates.Any())
                    {
                        foreach (var item in requestBody.CertificateOfCompetencyDto.SkillCertificates)
                        {
                            sfs.Add(new SkillCertificates
                            {
                                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                                BusinessId = uId,
                                SkillCertificateType = item.SkillCertificateType,
                                SkillScans = item.SkillScans
                            });
                        }
                    }
                    //特种设备证书
                    if (requestBody.CertificateOfCompetencyDto.SpecialEquips != null && requestBody.CertificateOfCompetencyDto.SpecialEquips.Any())
                    {
                        foreach (var item in requestBody.CertificateOfCompetencyDto.SpecialEquips)
                        {
                            ses.Add(new SpecialEquips
                            {
                                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                                BusinessId = uId,
                                SpecialEquipsCertificateType = item.SpecialEquipsCertificateType,
                                SpecialEquipsEffectiveTime = item.SpecialEquipsEffectiveTime,
                                SpecialEquipsScans = item.SpecialEquipsScans
                            });
                        }
                    }
                }
                await _dbContext.Insertable(coc).ExecuteCommandAsync();
                await _dbContext.Insertable(vrs).ExecuteCommandAsync();
                await _dbContext.Insertable(sfs).ExecuteCommandAsync();
                await _dbContext.Insertable(ses).ExecuteCommandAsync();
                #endregion

                #region 学历信息
                List<EducationalBackground> ebs = new();
                if (requestBody.EducationalBackgroundDto != null)
                {
                    if (requestBody.EducationalBackgroundDto.QualificationInfos != null && requestBody.EducationalBackgroundDto.QualificationInfos.Any())
                    {
                        foreach (var item in requestBody.EducationalBackgroundDto.QualificationInfos)
                        {
                            ebs.Add(new EducationalBackground
                            {
                                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                                BusinessId = uId,
                                School = item.School,
                                Major = item.Major,
                                Qualification = item.Qualification,
                                EndTime = item.EndTime,
                                QualificationScans = item.QualificationScans,
                                QualificationType = item.QualificationType,
                                StartTime = item.StartTime
                            });
                        }
                    }
                }
                await _dbContext.Insertable(ebs).ExecuteCommandAsync();
                #endregion

                #region 职务晋升
                List<Promotion> pts = new();
                if (requestBody.PromotionDto != null)
                {
                    if (requestBody.PromotionDto.Promotions != null && requestBody.PromotionDto.Promotions.Any())
                    {
                        foreach (var item in requestBody.PromotionDto.Promotions)
                        {
                            pts.Add(new Promotion()
                            {
                                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                                BusinessId = uId,
                                OnShip = item.OnShip,
                                Postition = item.Postition,
                                PromotionTime = item.PromotionTime,
                                PromotionScan = item.PromotionScan
                            });
                        }
                    }
                }
                await _dbContext.Insertable(pts).ExecuteCommandAsync();
                #endregion

                #region 任职船舶
                List<WorkShip> wss = new();
                if (requestBody.WorkShipDto != null)
                {
                    if (requestBody.WorkShipDto.WorkShips != null && requestBody.WorkShipDto.WorkShips.Any())
                    {
                        foreach (var item in requestBody.WorkShipDto.WorkShips)
                        {
                            wss.Add(new WorkShip
                            {
                                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                                BusinessId = uId,
                                OnShip = item.OnShip,
                                WorkShipStartTime = item.WorkShipStartTime,
                                WorkShipEndTime = item.WorkShipEndTime,
                                HolidayTime = item.HolidayTime,
                                Postition = item.Postition,
                                OnBoardTime = item.OnBoardTime
                            });
                        }
                    }
                }
                await _dbContext.Insertable(wss).ExecuteCommandAsync();
                #endregion

                #region 培训记录
                List<TrainingRecord> trs = new();
                if (requestBody.TrainingRecordDto != null)
                {
                    if (requestBody.TrainingRecordDto.TrainingRecords != null && requestBody.TrainingRecordDto.TrainingRecords.Any())
                    {
                        foreach (var item in requestBody.TrainingRecordDto.TrainingRecords)
                        {
                            trs.Add(new TrainingRecord
                            {
                                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                                BusinessId = uId,
                                TrainingScan = item.TrainingScan,
                                TrainingTime = item.TrainingTime,
                                TrainingType = item.TrainingType
                            });
                        }
                    }
                }
                await _dbContext.Insertable(trs).ExecuteCommandAsync();
                #endregion

                #region 年度考核
                List<YearCheck> ycs = new();
                if (requestBody.YearCheckDto != null)
                {
                    if (requestBody.YearCheckDto.YearChecks != null && requestBody.YearCheckDto.YearChecks.Any())
                    {
                        foreach (var item in requestBody.YearCheckDto.YearChecks)
                        {
                            ycs.Add(new YearCheck
                            {
                                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                                BusinessId = uId,
                                CheckType = item.CheckType,
                                TrainingScan = item.TrainingScan,
                                TrainingTime = item.TrainingTime
                            });
                        }
                    }
                }
                await _dbContext.Insertable(ycs).ExecuteCommandAsync();
                #endregion

                // 提交事务
                await _dbContext.Ado.CommitTranAsync();
            }
            catch (Exception ex)
            {
                // 捕获异常，回滚事务
                await _dbContext.Ado.RollbackTranAsync();
                // 可以根据需求处理异常，比如日志记录
                Console.WriteLine($"失败: {ex.Message}");
                throw;
            }
            return rr.SuccessResult(true);
        }
        /// <summary>
        /// 数据修改
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        private async Task<ResponseResult<bool>> UpdateDataAsync(CrewArchivesRequest requestBody)
        {
            ResponseResult<bool> rr = new();

            #region 基本信息
            if (requestBody.BId == Guid.Empty || string.IsNullOrWhiteSpace(requestBody.BId.ToString()))
            {
                rr.Fail("业务主键不能为空");
                return rr;
            }

            var userInfo = await _dbContext.Queryable<User>().FirstAsync(t => t.BusinessId == requestBody.BId);
            if (userInfo != null)
            {
                try
                {
                    await _dbContext.Ado.BeginTranAsync();

                    List<FamilyUser> hus = new();
                    List<EmergencyContacts> ecs = new();
                    UserEntryInfo userEntry = new();
                    if (requestBody.BaseInfoDto != null)
                    {
                        #region 
                        userInfo.BuildAddress = requestBody.BaseInfoDto.BuildAddress;
                        userInfo.CardId = requestBody.BaseInfoDto.CardId;
                        userInfo.WorkNumber = requestBody.BaseInfoDto.WorkNumber;
                        userInfo.Phone = requestBody.BaseInfoDto.Phone;
                        userInfo.CrewPhoto = requestBody.BaseInfoDto.PhotoScans;
                        userInfo.IdCardScans = requestBody.BaseInfoDto.IdCardScans;
                        userInfo.PoliticalStatus = requestBody.BaseInfoDto.PoliticalStatus;
                        userInfo.NativePlace = requestBody.BaseInfoDto.NativePlace;
                        userInfo.Nation = requestBody.BaseInfoDto.Nation;
                        userInfo.HomeAddress = requestBody.BaseInfoDto.HomeAddress;
                        userInfo.ShipType = requestBody.BaseInfoDto.ShipType;
                        userInfo.CrewType = requestBody.BaseInfoDto.CrewType;
                        userInfo.ServiceBookType = requestBody.BaseInfoDto.ServiceBookType;
                        userInfo.OnBoard = requestBody.BaseInfoDto.OnBoard;
                        userInfo.PositionOnBoard = requestBody.BaseInfoDto.PositionOnBoard;
                        userInfo.ContarctType = requestBody.BaseInfoDto.ContarctType;
                        userInfo.Name = requestBody.BaseInfoDto.Name;
                        #endregion

                        //劳务合同
                        if (requestBody.BaseInfoDto.UserEntryInfo != null)
                        {
                            userEntry = await _dbContext.Queryable<UserEntryInfo>().Where(t => t.IsDelete == 1 && t.BusinessId == userInfo.BusinessId).OrderByDescending(t => t.Created).FirstAsync();
                            if (userEntry != null)
                            {
                                userEntry.ContarctMain = requestBody.BaseInfoDto.UserEntryInfo.ContarctMain;
                                userEntry.EndTime = requestBody.BaseInfoDto.UserEntryInfo.EndTime;
                                userEntry.EntryScans = requestBody.BaseInfoDto.UserEntryInfo.EntryScans;
                                userEntry.EntryTime = requestBody.BaseInfoDto.UserEntryInfo.EntryTime;
                                userEntry.LaborCompany = requestBody.BaseInfoDto.UserEntryInfo.LaborCompany;
                                userEntry.EmploymentId = requestBody.BaseInfoDto.UserEntryInfo.EmploymentId;
                            }
                        }
                        //家庭成员
                        if (requestBody.BaseInfoDto.HomeUser != null && requestBody.BaseInfoDto.HomeUser.Any())
                        {
                            hus = await _dbContext.Queryable<FamilyUser>().Where(t => t.BusinessId == userInfo.BusinessId).ToListAsync();
                            foreach (var item in requestBody.BaseInfoDto.HomeUser)
                            {
                                var ef = hus.FirstOrDefault(x => x.Id.ToString() == item.Id);
                                if (ef != null)
                                {
                                    ef.Phone = item.Phone;
                                    ef.RelationShip = item.RelationShip;
                                    ef.UserName = item.UserName;
                                    ef.WorkUnit = item.WorkUnit;
                                }
                            }
                        }
                        //应急联系人
                        if (requestBody.BaseInfoDto.EmergencyContacts != null && requestBody.BaseInfoDto.EmergencyContacts.Any())
                        {
                            ecs = await _dbContext.Queryable<EmergencyContacts>().Where(t => t.BusinessId == userInfo.BusinessId).ToListAsync();
                            foreach (var item in requestBody.BaseInfoDto.EmergencyContacts)
                            {
                                var ef = ecs.FirstOrDefault(x => x.Id.ToString() == item.Id);
                                if (ef != null)
                                {
                                    ef.Phone = item.Phone;
                                    ef.RelationShip = item.RelationShip;
                                    ef.UserName = item.UserName;
                                    ef.WorkUnit = item.WorkUnit;
                                }
                            }
                        }
                    }
                    await _dbContext.Updateable(userInfo).ExecuteCommandAsync();
                    await _dbContext.Updateable(userEntry).ExecuteCommandAsync();
                    await _dbContext.Updateable(hus).ExecuteCommandAsync();
                    await _dbContext.Updateable(ecs).ExecuteCommandAsync();
                    #endregion

                    #region 适任及证书
                    CertificateOfCompetency coc = new();
                    List<VisaRecords> vrs = new();
                    List<SkillCertificates> sfs = new();
                    List<SpecialEquips> ses = new();
                    if (requestBody.CertificateOfCompetencyDto != null)
                    {
                        coc = await _dbContext.Queryable<CertificateOfCompetency>().FirstAsync(t => t.IsDelete == 1 && t.BusinessId == userInfo.BusinessId);
                        #region 
                        coc.FCertificate = requestBody.CertificateOfCompetencyDto.FCertificate;
                        coc.FNavigationArea = requestBody.CertificateOfCompetencyDto.FNavigationArea;
                        coc.FPosition = requestBody.CertificateOfCompetencyDto.FPosition;
                        coc.FSignTime = requestBody.CertificateOfCompetencyDto.FSignTime;
                        coc.FEffectiveTime = requestBody.CertificateOfCompetencyDto.FEffectiveTime;
                        coc.FScans = requestBody.CertificateOfCompetencyDto.FScans;
                        coc.SCertificate = requestBody.CertificateOfCompetencyDto.SCertificate;
                        coc.SNavigationArea = requestBody.CertificateOfCompetencyDto.SNavigationArea;
                        coc.SPosition = requestBody.CertificateOfCompetencyDto.SPosition;
                        coc.SSignTime = requestBody.CertificateOfCompetencyDto.SSignTime;
                        coc.SEffectiveTime = requestBody.CertificateOfCompetencyDto.SEffectiveTime;
                        coc.SScans = requestBody.CertificateOfCompetencyDto.SScans;
                        coc.TrainingCertificate = requestBody.CertificateOfCompetencyDto.TrainingCertificate;
                        coc.TrainingSignTime = requestBody.CertificateOfCompetencyDto.TrainingSignTime;
                        coc.TrainingScans = requestBody.CertificateOfCompetencyDto.TrainingScans;
                        coc.Z01EffectiveTime = requestBody.CertificateOfCompetencyDto.Z01EffectiveTime;
                        coc.Z07EffectiveTime = requestBody.CertificateOfCompetencyDto.Z07EffectiveTime;
                        coc.Z08EffectiveTime = requestBody.CertificateOfCompetencyDto.Z08EffectiveTime;
                        coc.Z04EffectiveTime = requestBody.CertificateOfCompetencyDto.Z04EffectiveTime;
                        coc.Z05EffectiveTime = requestBody.CertificateOfCompetencyDto.Z05EffectiveTime;
                        coc.Z02EffectiveTime = requestBody.CertificateOfCompetencyDto.Z02EffectiveTime;
                        coc.Z06EffectiveTime = requestBody.CertificateOfCompetencyDto.Z06EffectiveTime;
                        coc.Z09EffectiveTime = requestBody.CertificateOfCompetencyDto.Z09EffectiveTime;
                        coc.HealthCertificate = requestBody.CertificateOfCompetencyDto.HealthCertificate;
                        coc.HealthSignTime = requestBody.CertificateOfCompetencyDto.HealthSignTime;
                        coc.HealthEffectiveTime = requestBody.CertificateOfCompetencyDto.HealthEffectiveTime;
                        coc.HealthScans = requestBody.CertificateOfCompetencyDto.HealthScans;
                        coc.SeamanCertificate = requestBody.CertificateOfCompetencyDto.SeamanCertificate;
                        coc.SeamanSignTime = requestBody.CertificateOfCompetencyDto.SeamanSignTime;
                        coc.SeamanEffectiveTime = requestBody.CertificateOfCompetencyDto.SeamanEffectiveTime;
                        coc.SeamanScans = requestBody.CertificateOfCompetencyDto.SeamanScans;
                        coc.PassportCertificate = requestBody.CertificateOfCompetencyDto.PassportCertificate;
                        coc.PassportSignTime = requestBody.CertificateOfCompetencyDto.PassportSignTime;
                        coc.PassportEffectiveTime = requestBody.CertificateOfCompetencyDto.PassportEffectiveTime;
                        coc.PassportScans = requestBody.CertificateOfCompetencyDto.PassportScans;
                        #endregion

                        //签证记录
                        if (requestBody.CertificateOfCompetencyDto.VisaRecords != null && requestBody.CertificateOfCompetencyDto.VisaRecords.Any())
                        {
                            vrs = await _dbContext.Queryable<VisaRecords>().Where(t => t.BusinessId == userInfo.BusinessId).ToListAsync();
                            foreach (var item in requestBody.CertificateOfCompetencyDto.VisaRecords)
                            {
                                var vr = vrs.FirstOrDefault(x => x.Id.ToString() == item.Id);
                                if (vr != null)
                                {
                                    vr.Country = item.Country;
                                    vr.DueTime = item.DueTime;
                                    vr.VisaType = item.VisaType;
                                }
                            }
                        }
                        //技能证书
                        if (requestBody.CertificateOfCompetencyDto.SkillCertificates != null && requestBody.CertificateOfCompetencyDto.SkillCertificates.Any())
                        {
                            sfs = await _dbContext.Queryable<SkillCertificates>().Where(t => t.IsDelete == 1 && t.BusinessId == userInfo.BusinessId).ToListAsync();
                            foreach (var item in requestBody.CertificateOfCompetencyDto.SkillCertificates)
                            {
                                var sf = sfs.FirstOrDefault(x => x.Id.ToString() == item.Id);
                                if (sf != null)
                                {
                                    sf.SkillCertificateType = item.SkillCertificateType;
                                    sf.SkillScans = item.SkillScans;
                                }
                            }
                        }
                        //特种设备证书
                        if (requestBody.CertificateOfCompetencyDto.SpecialEquips != null && requestBody.CertificateOfCompetencyDto.SpecialEquips.Any())
                        {
                            ses = await _dbContext.Queryable<SpecialEquips>().Where(t => t.IsDelete == 1 && t.BusinessId == userInfo.BusinessId).ToListAsync();
                            foreach (var item in requestBody.CertificateOfCompetencyDto.SpecialEquips)
                            {
                                var se = ses.FirstOrDefault(x => x.Id.ToString() == item.Id);
                                if (se != null)
                                {
                                    se.SpecialEquipsCertificateType = item.SpecialEquipsCertificateType;
                                    se.SpecialEquipsEffectiveTime = item.SpecialEquipsEffectiveTime;
                                    se.SpecialEquipsScans = item.SpecialEquipsScans;
                                }
                            }
                        }
                    }
                    await _dbContext.Updateable(coc).ExecuteCommandAsync();
                    await _dbContext.Updateable(vrs).ExecuteCommandAsync();
                    await _dbContext.Updateable(sfs).ExecuteCommandAsync();
                    await _dbContext.Updateable(ses).ExecuteCommandAsync();
                    #endregion

                    #region 学历信息
                    List<EducationalBackground> ebs = new();
                    if (requestBody.EducationalBackgroundDto != null)
                    {
                        if (requestBody.EducationalBackgroundDto.QualificationInfos != null && requestBody.EducationalBackgroundDto.QualificationInfos.Any())
                        {
                            ebs = await _dbContext.Queryable<EducationalBackground>().Where(t => t.IsDelete == 1 && userInfo.BusinessId == t.BusinessId).ToListAsync();
                            foreach (var item in requestBody.EducationalBackgroundDto.QualificationInfos)
                            {
                                var eb = ebs.FirstOrDefault(x => x.Id.ToString() == item.Id);
                                if (eb != null)
                                {
                                    eb.School = item.School;
                                    eb.Major = item.Major;
                                    eb.Qualification = item.Qualification;
                                    eb.EndTime = item.EndTime;
                                    eb.QualificationScans = item.QualificationScans;
                                    eb.QualificationType = item.QualificationType;
                                    eb.StartTime = item.StartTime;
                                }
                            }
                        }
                    }
                    await _dbContext.Updateable(ebs).ExecuteCommandAsync();
                    #endregion

                    #region 职务晋升
                    List<Promotion> pts = new();
                    if (requestBody.PromotionDto != null)
                    {
                        if (requestBody.PromotionDto.Promotions != null && requestBody.PromotionDto.Promotions.Any())
                        {
                            pts = await _dbContext.Queryable<Promotion>().Where(t => t.BusinessId == userInfo.BusinessId).ToListAsync();
                            foreach (var item in requestBody.PromotionDto.Promotions)
                            {
                                var po = pts.FirstOrDefault(x => x.Id.ToString() == item.Id);
                                if (po != null)
                                {
                                    po.OnShip = item.OnShip;
                                    po.Postition = item.Postition;
                                    po.PromotionTime = item.PromotionTime;
                                    po.PromotionScan = item.PromotionScan;
                                }
                            }
                        }
                    }
                    await _dbContext.Updateable(pts).ExecuteCommandAsync();
                    #endregion

                    #region 任职船舶
                    List<WorkShip> wss = new();
                    if (requestBody.WorkShipDto != null)
                    {
                        if (requestBody.WorkShipDto.WorkShips != null && requestBody.WorkShipDto.WorkShips.Any())
                        {
                            wss = await _dbContext.Queryable<WorkShip>().Where(t => t.IsDelete == 1 && userInfo.BusinessId == t.BusinessId).ToListAsync();
                            foreach (var item in requestBody.WorkShipDto.WorkShips)
                            {
                                var ws = wss.FirstOrDefault(x => x.Id.ToString() == item.Id);
                                if (ws != null)
                                {
                                    ws.OnShip = item.OnShip;
                                    ws.WorkShipStartTime = item.WorkShipStartTime;
                                    ws.WorkShipEndTime = item.WorkShipEndTime;
                                    ws.HolidayTime = item.HolidayTime;
                                    ws.Postition = item.Postition;
                                    ws.OnBoardTime = item.OnBoardTime;
                                }
                            }
                        }
                    }
                    await _dbContext.Updateable(wss).ExecuteCommandAsync();
                    #endregion

                    #region 培训记录
                    List<TrainingRecord> trs = new();
                    if (requestBody.TrainingRecordDto != null)
                    {
                        if (requestBody.TrainingRecordDto.TrainingRecords != null && requestBody.TrainingRecordDto.TrainingRecords.Any())
                        {
                            trs = await _dbContext.Queryable<TrainingRecord>().Where(t => t.BusinessId == userInfo.BusinessId).ToListAsync();
                            foreach (var item in requestBody.TrainingRecordDto.TrainingRecords)
                            {
                                var tr = trs.FirstOrDefault(x => x.Id.ToString() == item.Id);
                                if (tr != null)
                                {
                                    tr.TrainingScan = item.TrainingScan;
                                    tr.TrainingTime = item.TrainingTime;
                                    tr.TrainingType = item.TrainingType;
                                }
                            }
                        }
                    }
                    await _dbContext.Updateable(trs).ExecuteCommandAsync();
                    #endregion

                    #region 年度考核
                    List<YearCheck> ycs = new();
                    if (requestBody.YearCheckDto != null)
                    {
                        if (requestBody.YearCheckDto.YearChecks != null && requestBody.YearCheckDto.YearChecks.Any())
                        {
                            ycs = await _dbContext.Queryable<YearCheck>().Where(t => t.IsDelete == 1 && t.BusinessId == userInfo.BusinessId).ToListAsync();
                            foreach (var item in requestBody.YearCheckDto.YearChecks)
                            {
                                var yc = ycs.FirstOrDefault(x => x.Id.ToString() == item.Id);
                                if (yc != null)
                                {
                                    yc.CheckType = item.CheckType;
                                    yc.TrainingScan = item.TrainingScan;
                                    yc.TrainingTime = item.TrainingTime;
                                }
                            }
                        }
                    }
                    await _dbContext.Updateable(ycs).ExecuteCommandAsync();
                    #endregion

                    // 提交事务
                    await _dbContext.Ado.CommitTranAsync();
                }
                catch (Exception ex)
                {
                    // 捕获异常，回滚事务
                    await _dbContext.Ado.RollbackTranAsync();
                    // 可以根据需求处理异常，比如日志记录
                    Console.WriteLine($"失败: {ex.Message}");
                    throw;
                }
                return rr.SuccessResult(true);
            }
            return rr.SuccessResult(true, 0, "无用户/该用户已被删除");
        }
        #endregion

        #region 下拉列表信息
        /// <summary>
        /// 基本下拉列表
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<ResponseResult<List<DropDownResponse>>> GetDropDownListAsync(int type)
        {
            ResponseResult<List<DropDownResponse>> rt = new();
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
            }
            return rt;
        }
        /// <summary>
        /// 籍贯
        /// </summary>
        /// <returns></returns>
        private async Task<ResponseResult<List<DropDownResponse>>> GetNativePlaceListAsync()
        {
            ResponseResult<List<DropDownResponse>> rt = new();
            var rr = await _dbContext.Queryable<AdministrativeDivision>().Where(t => t.IsDelete == 1 && t.SupRegionalismCode == "0").Select(t => new DropDownResponse
            {
                Key = t.BusinessId.ToString(),
                Value = t.Name,
            }).ToListAsync();
            return rt.SuccessResult(rr, rr.Count);
        }
        /// <summary>
        /// 民族
        /// </summary>
        /// <returns></returns>
        private async Task<ResponseResult<List<DropDownResponse>>> GetNationListAsync()
        {
            ResponseResult<List<DropDownResponse>> rt = new();
            var rr = await _dbContext.Queryable<Nation>().Where(t => t.IsDelete == 1).Select(t => new DropDownResponse
            {
                Key = t.BusinessId.ToString(),
                Value = t.Name,
            }).ToListAsync();
            return rt.SuccessResult(rr, rr.Count);
        }
        /// <summary>
        /// 船员类型
        /// </summary>
        /// <returns></returns>
        private async Task<ResponseResult<List<DropDownResponse>>> GetCrewTypeListAsync()
        {
            ResponseResult<List<DropDownResponse>> rt = new();
            var rr = await _dbContext.Queryable<CrewType>().Where(t => t.IsDelete == 1).Select(t => new DropDownResponse
            {
                Key = t.BusinessId.ToString(),
                Value = t.Name,
            }).ToListAsync();
            return rt.SuccessResult(rr, rr.Count);
        }
        /// <summary>
        /// 船舶
        /// </summary>
        /// <returns></returns>
        private async Task<ResponseResult<List<DropDownResponse>>> GetOwnerShipListAsync()
        {
            ResponseResult<List<DropDownResponse>> rt = new();
            var rr = await _dbContext.Queryable<OwnerShip>().Where(t => t.IsDelete == 1).Select(t => new DropDownResponse
            {
                Key = t.BusinessId.ToString(),
                Value = t.ShipName,
            }).ToListAsync();
            return rt.SuccessResult(rr, rr.Count);
        }
        /// <summary>
        /// 培训类型
        /// </summary>
        /// <returns></returns>
        private async Task<ResponseResult<List<DropDownResponse>>> GetTrainingListAsync()
        {
            ResponseResult<List<DropDownResponse>> rt = new();
            var rr = await _dbContext.Queryable<TrainingType>().Where(t => t.IsDelete == 1).Select(t => new DropDownResponse
            {
                Key = t.BusinessId.ToString(),
                Value = t.Name,
            }).ToListAsync();
            return rt.SuccessResult(rr, rr.Count);
        }
        /// <summary>
        /// 政治面貌
        /// </summary>
        /// <returns></returns>
        private async Task<ResponseResult<List<DropDownResponse>>> GetPoliticalListAsync()
        {
            ResponseResult<List<DropDownResponse>> rt = new();
            var rr = await _dbContext.Queryable<Political>().Where(t => t.IsDelete == 1).Select(t => new DropDownResponse
            {
                Key = t.BusinessId.ToString(),
                Value = t.Name,
            }).ToListAsync();
            return rt.SuccessResult(rr, rr.Count);
        }
        /// <summary>
        /// 用工形式
        /// </summary>
        /// <returns></returns>
        private async Task<ResponseResult<List<DropDownResponse>>> GetEmploymentTypeListAsync()
        {
            ResponseResult<List<DropDownResponse>> rt = new();
            var rr = await _dbContext.Queryable<EmploymentType>().Where(t => t.IsDelete == 1).Select(t => new DropDownResponse
            {
                Key = t.BusinessId.ToString(),
                Value = t.Name,
            }).ToListAsync();
            return rt.SuccessResult(rr, rr.Count);
        }
        /// <summary>
        /// 国家
        /// </summary>
        /// <returns></returns>
        private async Task<ResponseResult<List<DropDownResponse>>> GetCountryListAsync()
        {
            ResponseResult<List<DropDownResponse>> rt = new();
            List<DropDownResponse> rr = new();
            rr = await _dbContext.Queryable<CountryRegion>().Where(t => t.IsDelete == 1).Select(t => new DropDownResponse
            {
                Key = t.BusinessId.ToString(),
                Value = t.Name,
            }).ToListAsync();
            return rt.SuccessResult(rr, rr.Count);
        }
        /// <summary>
        /// 适任职务
        /// </summary>
        /// <returns></returns>
        private async Task<ResponseResult<List<DropDownResponse>>> GetPositionListAsync()
        {
            ResponseResult<List<DropDownResponse>> rt = new();
            List<DropDownResponse> rr = new();
            rr = await _dbContext.Queryable<Position>().Where(t => t.IsDelete == 1).Select(t => new DropDownResponse
            {
                Key = t.BusinessId.ToString(),
                Value = t.Name,
            }).ToListAsync();
            return rt.SuccessResult(rr, rr.Count);
        }
        /// <summary>
        /// 家庭关系
        /// </summary>
        /// <returns></returns>
        private static ResponseResult<List<DropDownResponse>> GetFamilyRelationList()
        {
            ResponseResult<List<DropDownResponse>> rt = new();
            var enumConvertList = Enum.GetValues(typeof(FamilyRelationEnum))
                                                   .Cast<FamilyRelationEnum>()
                                                   .Select(x => new DropDownResponse
                                                   {
                                                       Key = ((int)x).ToString(),
                                                       Value = GetEnumDescription(x)
                                                   })
                                                   .ToList();
            return rt.SuccessResult(enumConvertList, enumConvertList.Count);
        }
        /// <summary>
        /// 船舶类型
        /// </summary>
        /// <returns></returns>
        private static ResponseResult<List<DropDownResponse>> GetShipTypeList()
        {
            ResponseResult<List<DropDownResponse>> rt = new();
            var enumConvertList = Enum.GetValues(typeof(ShipTypeEnum))
                                                   .Cast<ShipTypeEnum>()
                                                   .Select(x => new DropDownResponse
                                                   {
                                                       Key = ((int)x).ToString(),
                                                       Value = GetEnumDescription(x)
                                                   })
                                                   .ToList();
            return rt.SuccessResult(enumConvertList, enumConvertList.Count);
        }
        /// <summary>
        /// 服务簿类型
        /// </summary>
        /// <returns></returns>
        private static ResponseResult<List<DropDownResponse>> GetServiceBookList()
        {
            ResponseResult<List<DropDownResponse>> rt = new();
            var enumConvertList = Enum.GetValues(typeof(ServiceBookEnum))
                                                   .Cast<ServiceBookEnum>()
                                                   .Select(x => new DropDownResponse
                                                   {
                                                       Key = ((int)x).ToString(),
                                                       Value = GetEnumDescription(x)
                                                   })
                                                   .ToList();
            return rt.SuccessResult(enumConvertList, enumConvertList.Count);
        }
        /// <summary>
        /// 签证类型
        /// </summary>
        /// <returns></returns>
        private static ResponseResult<List<DropDownResponse>> GetVisaTypeList()
        {
            ResponseResult<List<DropDownResponse>> rt = new();
            var enumConvertList = Enum.GetValues(typeof(VisaTypeEnum))
                                                   .Cast<VisaTypeEnum>()
                                                   .Select(x => new DropDownResponse
                                                   {
                                                       Key = ((int)x).ToString(),
                                                       Value = GetEnumDescription(x)
                                                   })
                                                   .ToList();
            return rt.SuccessResult(enumConvertList, enumConvertList.Count);
        }
        /// <summary>
        /// 技能证书
        /// </summary>
        /// <returns></returns>
        private static ResponseResult<List<DropDownResponse>> GetCertificateTypeList()
        {
            ResponseResult<List<DropDownResponse>> rt = new();
            var enumConvertList = Enum.GetValues(typeof(CertificateTypeEnum))
                                                   .Cast<CertificateTypeEnum>()
                                                   .Select(x => new DropDownResponse
                                                   {
                                                       Key = ((int)x).ToString(),
                                                       Value = GetEnumDescription(x)
                                                   })
                                                   .ToList();
            return rt.SuccessResult(enumConvertList, enumConvertList.Count);
        }
        /// <summary>
        /// 学历类型
        /// </summary>
        /// <returns></returns>
        private static ResponseResult<List<DropDownResponse>> GetQualificationTypeList()
        {
            ResponseResult<List<DropDownResponse>> rt = new();
            var enumConvertList = Enum.GetValues(typeof(QualificationTypeEnum))
                                                   .Cast<QualificationTypeEnum>()
                                                   .Select(x => new DropDownResponse
                                                   {
                                                       Key = ((int)x).ToString(),
                                                       Value = GetEnumDescription(x)
                                                   })
                                                   .ToList();
            return rt.SuccessResult(enumConvertList, enumConvertList.Count);
        }
        /// <summary>
        /// 学历
        /// </summary>
        /// <returns></returns>
        private static ResponseResult<List<DropDownResponse>> GetQualificationList()
        {
            ResponseResult<List<DropDownResponse>> rt = new();
            var enumConvertList = Enum.GetValues(typeof(QualificationEnum))
                                                   .Cast<QualificationEnum>()
                                                   .Select(x => new DropDownResponse
                                                   {
                                                       Key = ((int)x).ToString(),
                                                       Value = GetEnumDescription(x)
                                                   })
                                                   .ToList();
            return rt.SuccessResult(enumConvertList, enumConvertList.Count);
        }
        /// <summary>
        /// 合同
        /// </summary>
        /// <returns></returns>
        private static ResponseResult<List<DropDownResponse>> GetContractList()
        {
            ResponseResult<List<DropDownResponse>> rt = new();
            var enumConvertList = Enum.GetValues(typeof(ContractEnum))
                                                   .Cast<ContractEnum>()
                                                   .Select(x => new DropDownResponse
                                                   {
                                                       Key = ((int)x).ToString(),
                                                       Value = GetEnumDescription(x)
                                                   })
                                                   .ToList();
            return rt.SuccessResult(enumConvertList, enumConvertList.Count);
        }
        /// <summary>
        /// 考核
        /// </summary>
        /// <returns></returns>
        private static ResponseResult<List<DropDownResponse>> GetCheckList()
        {
            ResponseResult<List<DropDownResponse>> rt = new();
            var enumConvertList = Enum.GetValues(typeof(CheckEnum))
                                                   .Cast<CheckEnum>()
                                                   .Select(x => new DropDownResponse
                                                   {
                                                       Key = ((int)x).ToString(),
                                                       Value = GetEnumDescription(x)
                                                   })
                                                   .ToList();
            return rt.SuccessResult(enumConvertList, enumConvertList.Count);
        }
        /// <summary>
        /// 船舶状态
        /// </summary>
        /// <returns></returns>
        private static ResponseResult<List<DropDownResponse>> GetShipStateList()
        {
            ResponseResult<List<DropDownResponse>> rt = new();
            var enumConvertList = Enum.GetValues(typeof(ShipStateEnum))
                                                   .Cast<ShipStateEnum>()
                                                   .Select(x => new DropDownResponse
                                                   {
                                                       Key = ((int)x).ToString(),
                                                       Value = GetEnumDescription(x)
                                                   })
                                                   .ToList();
            return rt.SuccessResult(enumConvertList, enumConvertList.Count);
        }
        /// <summary>
        /// 船员状态
        /// </summary>
        /// <returns></returns>
        private static ResponseResult<List<DropDownResponse>> GetCrewStatusList()
        {
            ResponseResult<List<DropDownResponse>> rt = new();
            var enumConvertList = Enum.GetValues(typeof(CrewStatusEnum))
                                                   .Cast<CrewStatusEnum>()
                                                   .Select(x => new DropDownResponse
                                                   {
                                                       Key = ((int)x).ToString(),
                                                       Value = GetEnumDescription(x)
                                                   })
                                                   .ToList();
            return rt.SuccessResult(enumConvertList, enumConvertList.Count);
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
        public async Task<ResponseResult<EducationalBackgroundDetails>> GetEducationalBackgroundDetailsAsync(string bId)
        {
            ResponseResult<EducationalBackgroundDetails> rt = new();
            EducationalBackgroundDetails ur = new();

            var userInfo = await GetUserInfoAsync(bId);
            var edutab = await _dbContext.Queryable<EducationalBackground>().Where(t => t.IsDelete == 1 && t.BusinessId == userInfo.BusinessId).ToListAsync();
            //获取文件
            List<string> fileIds = new();
            foreach (var file in edutab)
            {
                var ids = file.QualificationScans?.Split(",").ToList();
                if (ids != null && ids.Any())
                {
                    fileIds.AddRange(ids);
                }
            }
            var files = await _dbContext.Queryable<Files>().Where(t => fileIds.Contains(t.BusinessId.ToString())).ToListAsync();
            List<QualificationForDetails> qd = new();
            foreach (var item in edutab)
            {
                var edFile = files.Where(x => item.QualificationScans.Split(",").Contains(x.BusinessId.ToString()))
                    .Select(x => new FileInfosForDetails
                    {
                        Id = x.Id.ToString(),
                        FileSize = x.FileSize,
                        FileType = x.FileType,
                        Name = x.Name,
                        OriginName = x.OriginName,
                        SuffixName = x.SuffixName
                    })
                    .ToList();
                qd.Add(new QualificationForDetails
                {
                    StartTime = item.StartTime,
                    EndTime = item.EndTime,
                    Major = item.Major,
                    QualificationType = EnumUtil.GetDescription(item.QualificationType),
                    School = item.School,
                    Qualification = EnumUtil.GetDescription(item.Qualification),
                    QualificationScans = edFile
                });
            }
            ur.QualificationInfos = qd;

            return rt.SuccessResult(ur);
        }
        /// <summary>
        /// 备注
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseResult<NotesDetails>> GetNotesDetailsAsync(string bId)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 职务晋升
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        public async Task<ResponseResult<PromotionDetails>> GetPromotionDetailsAsync(string bId)
        {
            ResponseResult<PromotionDetails> rt = new();
            PromotionDetails ur = new();

            var userInfo = await GetUserInfoAsync(bId);
            var promotion = await _dbContext.Queryable<Promotion>().Where(t => t.IsDelete == 1 && t.BusinessId == userInfo.BusinessId).OrderByDescending(x => x.PromotionTime).ToListAsync();
            //获取文件
            List<string> fileIds = new();
            foreach (var file in promotion)
            {
                var ids = file.PromotionScan?.Split(",").ToList();
                if (ids != null && ids.Any())
                {
                    fileIds.AddRange(ids);
                }
            }
            var files = await _dbContext.Queryable<Files>().Where(t => fileIds.Contains(t.BusinessId.ToString())).ToListAsync();
            var ownships = await _dbContext.Queryable<OwnerShip>().Where(t => t.IsDelete == 1).Select(x => new { x.BusinessId, x.ShipName, x.Country }).ToListAsync();
            var positions = await _dbContext.Queryable<Position>().Where(t => t.IsDelete == 1).Select(x => new { x.BusinessId, x.Name }).ToListAsync();
            List<PromotionsForDetails> pd = new();
            foreach (var item in promotion)
            {
                var ids = item.PromotionScan?.Split(",").ToList();
                var pdFiles = files.Where(x => ids.Contains(x.BusinessId.ToString()))
                     .Select(x => new FileInfosForDetails
                     {
                         Id = x.Id.ToString(),
                         FileSize = x.FileSize,
                         FileType = x.FileType,
                         Name = x.Name,
                         OriginName = x.OriginName,
                         SuffixName = x.SuffixName
                     })
                    .ToList();
                pd.Add(new PromotionsForDetails
                {
                    OnShip = ownships.FirstOrDefault(x => x.BusinessId.ToString() == item.OnShip)?.ShipName,
                    Postition = positions.FirstOrDefault(x => x.BusinessId.ToString() == item.Postition)?.Name,
                    PromotionTime = item.PromotionTime,
                    PromotionScans = pdFiles
                });
            }
            ur.Promotions = pd;

            return rt.SuccessResult(ur);
        }
        /// <summary>
        /// 培训记录
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        public async Task<ResponseResult<TrainingRecordDetails>> GetTrainingRecordDetailsAsync(string bId)
        {
            ResponseResult<TrainingRecordDetails> rt = new();
            TrainingRecordDetails ur = new();

            var userInfo = await GetUserInfoAsync(bId);

            var traningRecord = await _dbContext.Queryable<TrainingRecord>().Where(t => t.IsDelete == 1 && t.BusinessId == userInfo.BusinessId).OrderByDescending(x => x.TrainingTime).ToListAsync();
            var trainType = await _dbContext.Queryable<TrainingType>().Where(t => t.IsDelete == 1 && t.BusinessId == userInfo.BusinessId).Select(x => new { x.BusinessId, x.Name }).ToListAsync();
            //获取文件
            List<string> fileIds = new();
            foreach (var file in traningRecord)
            {
                var ids = file.TrainingScan?.Split(",").ToList();
                if (ids != null && ids.Any())
                {
                    fileIds.AddRange(ids);
                }
            }
            var files = await _dbContext.Queryable<Files>().Where(t => fileIds.Contains(t.BusinessId.ToString())).ToListAsync();
            List<TrainingRecordsForDetails> td = new();
            foreach (var item in traningRecord)
            {
                var trFile = files.Where(x => item.TrainingScan.Split(",").Contains(x.BusinessId.ToString()))
                    .Select(x => new FileInfosForDetails
                    {
                        Id = x.Id.ToString(),
                        FileSize = x.FileSize,
                        FileType = x.FileType,
                        Name = x.Name,
                        OriginName = x.OriginName,
                        SuffixName = x.SuffixName
                    })
                    .ToList();
                td.Add(new TrainingRecordsForDetails
                {
                    TrainingTime = item.TrainingTime,
                    TrainingType = trainType.FirstOrDefault(x => x.BusinessId.ToString() == item.TrainingType)?.Name,
                    TrainingScans = trFile
                });
            }
            ur.TrainingRecords = td;

            return rt.SuccessResult(ur);
        }
        /// <summary>
        /// 任职船舶
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        public async Task<ResponseResult<WorkShipDetails>> GetWorkShipDetailsAsync(string bId)
        {
            ResponseResult<WorkShipDetails> rt = new();
            WorkShipDetails ur = new();

            var userInfo = await GetUserInfoAsync(bId);
            var workShips = await _dbContext.Queryable<WorkShip>().Where(t => t.IsDelete == 1 && t.BusinessId == userInfo.BusinessId).OrderByDescending(t => t.WorkShipEndTime).ToListAsync();
            var ownships = await _dbContext.Queryable<OwnerShip>().Where(t => t.IsDelete == 1).Select(x => new { x.BusinessId, x.ShipName, x.Country }).ToListAsync();
            var positions = await _dbContext.Queryable<Position>().Where(t => t.IsDelete == 1).Select(x => new { x.BusinessId, x.Name }).ToListAsync();

            List<WorkShipsForDetails> wd = new();
            foreach (var item in workShips)
            {
                DateTime date1 = new DateTime(item.WorkShipStartTime.Year, item.WorkShipStartTime.Month, item.WorkShipStartTime.Day);
                DateTime date2 = new DateTime(item.WorkShipEndTime.Year, item.WorkShipEndTime.Month, item.WorkShipEndTime.Day);
                var days = (date2 - date1).Days + 1;
                wd.Add(new WorkShipsForDetails
                {
                    WorkShipEndTime = item.WorkShipEndTime,
                    HolidayTime = item.HolidayTime,
                    OnBoardTime = item.OnBoardTime,
                    OnShip = ownships.FirstOrDefault(x => x.BusinessId.ToString() == item.OnShip)?.ShipName,
                    Postition = positions.FirstOrDefault(x => x.BusinessId.ToString() == item.Postition)?.Name,
                    WorkShipStartTime = item.WorkShipStartTime,
                    OnBoardDay = days,
                    Holiday = 0
                });
            }
            ur.WorkShips = wd;

            return rt.SuccessResult(ur);
        }
        /// <summary>
        /// 年度考核
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        public async Task<ResponseResult<YearCheckDetails>> GetYearCheckDetailAsync(string bId)
        {
            ResponseResult<YearCheckDetails> rt = new();
            YearCheckDetails ur = new();

            var userInfo = await GetUserInfoAsync(bId);

            var yearChecks = await _dbContext.Queryable<YearCheck>().Where(t => t.IsDelete == 1 && t.BusinessId == userInfo.BusinessId).OrderByDescending(x => x.TrainingTime).ToListAsync();
            //获取文件
            List<string> fileIds = new();
            foreach (var file in yearChecks)
            {
                var ids = file.TrainingScan?.Split(",").ToList();
                if (ids != null && ids.Any())
                {
                    fileIds.AddRange(ids);
                }
            }
            var files = await _dbContext.Queryable<Files>().Where(t => fileIds.Contains(t.BusinessId.ToString())).ToListAsync();
            List<YearChecksForDetails> yc = new();
            foreach (var item in yearChecks)
            {
                var ycFile = files.Where(x => item.TrainingScan.Split(",").Contains(x.BusinessId.ToString()))
                    .Select(x => new FileInfosForDetails
                    {
                        Id = x.Id.ToString(),
                        FileSize = x.FileSize,
                        FileType = x.FileType,
                        Name = x.Name,
                        OriginName = x.OriginName,
                        SuffixName = x.SuffixName
                    })
                    .ToList();
                yc.Add(new YearChecksForDetails
                {
                    TrainingTime = item.TrainingTime,
                    CheckType = EnumUtil.GetDescription(item.CheckType),
                    TrainingScans = ycFile
                });
            }
            ur.YearChecks = yc;

            return rt.SuccessResult(ur);
        }
        /// <summary>
        /// 适任证书
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        public async Task<ResponseResult<CertificateOfCompetencyDetails>> GetCertificateOfCompetencyDetailsAsync(string bId)
        {
            ResponseResult<CertificateOfCompetencyDetails> rt = new();
            CertificateOfCompetencyDetails ur = new();

            var userInfo = await GetUserInfoAsync(bId);
            var cerOfComp = await _dbContext.Queryable<CertificateOfCompetency>().FirstAsync(t => t.IsDelete == 1 && t.BusinessId == userInfo.BusinessId);
            if (cerOfComp != null)
            {
                #region 简易字段匹配
                ur.FCertificate = cerOfComp.FCertificate;
                ur.FSignTime = cerOfComp.FSignTime;
                ur.FEffectiveTime = cerOfComp.FEffectiveTime;
                if (cerOfComp.FEffectiveTime.HasValue)
                {
                    DateTime date1 = new DateTime(ur.FEffectiveTime.Value.Year, ur.FEffectiveTime.Value.Month, ur.FEffectiveTime.Value.Day);
                    DateTime date2 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    ur.FEffectiveCountdown = (date1 - date2).Days + 1;
                }
                ur.SCertificate = cerOfComp.SCertificate;
                ur.SSignTime = cerOfComp.SSignTime;
                ur.SEffectiveTime = cerOfComp.SEffectiveTime;
                ur.SEffectiveTime = cerOfComp.SEffectiveTime;
                if (cerOfComp.SEffectiveTime.HasValue)
                {
                    DateTime date1 = new DateTime(ur.SEffectiveTime.Value.Year, ur.SEffectiveTime.Value.Month, ur.SEffectiveTime.Value.Day);
                    DateTime date2 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    ur.SEffectiveCountdown = (date1 - date2).Days + 1;
                }
                ur.TrainingCertificate = cerOfComp.TrainingCertificate;
                ur.TrainingSignTime = cerOfComp.TrainingSignTime;
                ur.Z01EffectiveTime = cerOfComp.Z01EffectiveTime;
                ur.Z07EffectiveTime = cerOfComp.Z07EffectiveTime;
                ur.Z08EffectiveTime = cerOfComp.Z08EffectiveTime;
                ur.Z04EffectiveTime = cerOfComp.Z04EffectiveTime;
                ur.Z05EffectiveTime = cerOfComp.Z05EffectiveTime;
                ur.Z02EffectiveTime = cerOfComp.Z02EffectiveTime;
                ur.Z06EffectiveTime = cerOfComp.Z06EffectiveTime;
                ur.Z09EffectiveTime = cerOfComp.Z09EffectiveTime;
                ur.HealthCertificate = cerOfComp.HealthCertificate;
                ur.HealthSignTime = cerOfComp.HealthSignTime;
                ur.HealthEffectiveTime = cerOfComp.HealthEffectiveTime;
                ur.SeamanCertificate = cerOfComp.SeamanCertificate;
                ur.SeamanSignTime = cerOfComp.SeamanSignTime;
                ur.SeamanEffectiveTime = cerOfComp.SeamanEffectiveTime;
                ur.PassportCertificate = cerOfComp.PassportCertificate;
                ur.PassportSignTime = cerOfComp.PassportSignTime;
                ur.PassportEffectiveTime = cerOfComp.PassportEffectiveTime;
                #endregion
                //航区
                var navigationarea = await _dbContext.Queryable<NavigationArea>().Where(t => t.IsDelete == 1).ToListAsync();
                ur.FNavigationArea = navigationarea.FirstOrDefault(x => x.BusinessId.ToString() == cerOfComp.FNavigationArea)?.Name;
                ur.SNavigationArea = navigationarea.FirstOrDefault(x => x.BusinessId.ToString() == cerOfComp.SNavigationArea)?.Name;
                //适任职务
                var position = await _dbContext.Queryable<Position>().Where(t => t.IsDelete == 1).ToListAsync();
                ur.FPosition = position.FirstOrDefault(x => x.BusinessId.ToString() == cerOfComp.FPosition)?.Name;
                ur.SPosition = position.FirstOrDefault(x => x.BusinessId.ToString() == cerOfComp.SPosition)?.Name;
                //技能证书
                var skillcf = await _dbContext.Queryable<SkillCertificates>().Where(t => t.IsDelete == 1 && t.BusinessId == userInfo.BusinessId).ToListAsync();
                List<string> skillFileIds = new();
                foreach (var item in skillcf)
                {
                    var ids = item.SkillScans?.Split(",").ToList();
                    if (ids != null && ids.Any())
                    {
                        skillFileIds.AddRange(ids);
                    }
                }
                //特种设备证书
                var specFill = await _dbContext.Queryable<SpecialEquips>().Where(t => t.IsDelete == 1 && t.BusinessId == userInfo.BusinessId).ToListAsync();
                List<string> specfilesIds = new();
                foreach (var item in specFill)
                {
                    var ids = item.SpecialEquipsScans?.Split(",").ToList();
                    if (ids != null && ids.Any())
                    {
                        specfilesIds.AddRange(ids);
                    }
                }
                //当前适任证书所有文件
                List<string> curFilesIds = new();
                curFilesIds.AddRange(cerOfComp.FScans.Split(",").ToList());
                curFilesIds.AddRange(cerOfComp.SScans.Split(",").ToList());
                curFilesIds.AddRange(cerOfComp.TrainingScans.Split(",").ToList());
                curFilesIds.AddRange(cerOfComp.HealthScans.Split(",").ToList());
                curFilesIds.AddRange(cerOfComp.SeamanScans.Split(",").ToList());
                curFilesIds.AddRange(cerOfComp.PassportScans.Split(",").ToList());
                curFilesIds.AddRange(skillFileIds);
                curFilesIds.AddRange(specfilesIds);
                var files = await _dbContext.Queryable<Files>().Where(t => t.IsDelete == 1 && curFilesIds.Contains(t.BusinessId.ToString())).ToListAsync();

                #region 扫描件
                var fScans = files.Where(x => cerOfComp.FScans.Split(",").ToList().Contains(x.BusinessId.ToString()))
                    .Select(x => new FileInfosForDetails
                    {
                        Id = x.Id.ToString(),
                        FileSize = x.FileSize,
                        FileType = x.FileType,
                        Name = x.Name,
                        OriginName = x.OriginName,
                        SuffixName = x.SuffixName
                    })
                    .ToList();
                var sScans = files.Where(x => cerOfComp.SScans.Split(",").ToList().Contains(x.BusinessId.ToString()))
                    .Select(x => new FileInfosForDetails
                    {
                        Id = x.Id.ToString(),
                        FileSize = x.FileSize,
                        FileType = x.FileType,
                        Name = x.Name,
                        OriginName = x.OriginName,
                        SuffixName = x.SuffixName
                    })
                    .ToList();
                var trainingScans = files.Where(x => cerOfComp.TrainingScans.Split(",").ToList().Contains(x.BusinessId.ToString()))
                   .Select(x => new FileInfosForDetails
                   {
                       Id = x.Id.ToString(),
                       FileSize = x.FileSize,
                       FileType = x.FileType,
                       Name = x.Name,
                       OriginName = x.OriginName,
                       SuffixName = x.SuffixName
                   })
                   .ToList();
                var healthScans = files.Where(x => cerOfComp.HealthScans.Split(",").ToList().Contains(x.BusinessId.ToString()))
                   .Select(x => new FileInfosForDetails
                   {
                       Id = x.Id.ToString(),
                       FileSize = x.FileSize,
                       FileType = x.FileType,
                       Name = x.Name,
                       OriginName = x.OriginName,
                       SuffixName = x.SuffixName
                   })
                   .ToList();
                var seamanScans = files.Where(x => cerOfComp.SeamanScans.Split(",").ToList().Contains(x.BusinessId.ToString()))
                   .Select(x => new FileInfosForDetails
                   {
                       Id = x.Id.ToString(),
                       FileSize = x.FileSize,
                       FileType = x.FileType,
                       Name = x.Name,
                       OriginName = x.OriginName,
                       SuffixName = x.SuffixName
                   })
                   .ToList();
                var passportScans = files.Where(x => cerOfComp.PassportScans.Split(",").ToList().Contains(x.BusinessId.ToString()))
                   .Select(x => new FileInfosForDetails
                   {
                       Id = x.Id.ToString(),
                       FileSize = x.FileSize,
                       FileType = x.FileType,
                       Name = x.Name,
                       OriginName = x.OriginName,
                       SuffixName = x.SuffixName
                   })
                   .ToList();
                //技能证书
                List<SkillCertificatesForDetails> sk = new();
                foreach (var item in skillcf)
                {
                    var skFiles = files.Where(x => x.BusinessId.ToString() == item.SkillScans)
                        .Select(x => new FileInfosForDetails
                        {
                            Id = x.Id.ToString(),
                            FileSize = x.FileSize,
                            FileType = x.FileType,
                            Name = x.Name,
                            OriginName = x.OriginName,
                            SuffixName = x.SuffixName
                        })
                        .FirstOrDefault();

                    sk.Add(new SkillCertificatesForDetails
                    {
                        Id = item.Id.ToString(),
                        SkillCertificateType = EnumUtil.GetDescription(item.SkillCertificateType),
                        SkillScans = skFiles
                    });
                }
                //特种设备证书
                List<SpecialEquipsForDetails> sp = new();
                foreach (var item in specFill)
                {
                    var spFiles = files.Where(x => x.BusinessId.ToString() == item.SpecialEquipsScans)
                        .Select(x => new FileInfosForDetails
                        {
                            Id = x.Id.ToString(),
                            FileSize = x.FileSize,
                            FileType = x.FileType,
                            Name = x.Name,
                            OriginName = x.OriginName,
                            SuffixName = x.SuffixName
                        })
                        .FirstOrDefault();

                    sp.Add(new SpecialEquipsForDetails
                    {
                        Id = item.Id.ToString(),
                        AnnualReviewTime = item.AnnualReviewTime,
                        SpecialEquipsEffectiveTime = item.SpecialEquipsEffectiveTime,
                        SpecialEquipsCertificateType = EnumUtil.FetchDescription(item.SpecialEquipsCertificateType),
                        SpecialEquipsScans = spFiles
                    });
                }
                #endregion

                ur.FScans = fScans;
                ur.SScans = sScans;
                ur.TrainingScans = trainingScans;
                ur.HealthScans = healthScans;
                ur.SeamanScans = seamanScans;
                ur.PassportScans = passportScans;
                ur.SkillCertificates = sk;
                ur.SpecialEquips = sp;

                //签证记录
                var visarecords = await _dbContext.Queryable<VisaRecords>().Where(t => t.IsDelete == 1 && t.BusinessId == userInfo.BusinessId).ToListAsync();
                var countrys = await _dbContext.Queryable<CountryRegion>().Where(t => t.IsDelete == 1).Select(x => new { x.BusinessId, x.Name }).ToListAsync();
                List<VisaRecordsForDetails> vfd = new();
                foreach (var item in visarecords)
                {
                    vfd.Add(new VisaRecordsForDetails
                    {
                        Id = item.Id.ToString(),
                        Country = countrys.FirstOrDefault(x => x.BusinessId.ToString() == item.Country)?.Name,
                        DueTime = item.DueTime,
                        VisaType = EnumUtil.GetDescription(item.VisaType),
                        IsDue = item.DueTime >= DateTime.Now ? false : true
                    });
                }
                ur.VisaRecords = vfd;
            }
            return rt.SuccessResult(ur);

        }
        /// <summary>
        /// 劳务详情
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        public async Task<ResponseResult<LaborServicesInfoDetails>> GetLaborServicesDetailsAsync(string bId)
        {
            ResponseResult<LaborServicesInfoDetails> rt = new();
            LaborServicesInfoDetails ur = new();

            var userInfo = await GetUserInfoAsync(bId);
            //入职材料
            var userEntryInfo = await _dbContext.Queryable<UserEntryInfo>().Where(t => t.IsDelete == 1 && t.BusinessId == userInfo.BusinessId).OrderByDescending(x => x.EntryTime).ToListAsync();
            var filesIds = userEntryInfo.Select(x => x.EntryScans).ToList();
            List<string> fIds = new();
            foreach (var f in filesIds)
            {
                var ids = f == null ? null : f.Split(",").ToList();
                if (ids != null && ids.Any())
                {
                    fIds.AddRange(ids);
                }
            }
            //用工类型
            var empType = await _dbContext.Queryable<EmploymentType>().Where(t => t.IsDelete == 1).ToListAsync();
            //读取用户入职的所有文件资料
            var files = await _dbContext.Queryable<Files>().Where(t => t.IsDelete == 1 && fIds.Contains(t.BusinessId.ToString())).ToListAsync();
            List<UserEntryInfosForDetails> uEntry = new();
            foreach (var item in userEntryInfo)
            {
                var cfileIds = item.EntryScans.Split(",").ToList();
                var userEntryFiles = files.Where(x => cfileIds.Contains(x.BusinessId.ToString()))
                    .Select(x => new FileInfosForDetails
                    {
                        Id = x.Id.ToString(),
                        FileSize = x.FileSize,
                        FileType = x.FileType,
                        Name = x.Name,
                        OriginName = x.OriginName,
                        SuffixName = x.SuffixName
                    })
                    .ToList();
                uEntry.Add(new UserEntryInfosForDetails
                {
                    Id = item.Id.ToString(),
                    EntryScans = userEntryFiles,
                    ContarctMain = item.ContarctMain,
                    EntryDate = item.EntryTime.ToString("yyyy/MM/dd") + "~" + item.EndTime.ToString("yyyy/MM/dd"),
                    LaborCompany = item.LaborCompany,
                    EmploymentName = empType.FirstOrDefault(x => x.BusinessId.ToString() == item.EmploymentId)?.Name,
                    Staus = item.EndTime >= DateTime.Now ? "进行中" : "已结束"
                });
            }

            //最新入职时间
            var entryDateContent = userEntryInfo.FirstOrDefault()?.EntryTime.ToString("yyyy/MM/dd") + "~" + userEntryInfo.FirstOrDefault()?.EndTime.ToString("yyyy/MM/dd");
            //最新入职文件
            var newEntryFilesIds = userEntryInfo.FirstOrDefault()?.EntryScans.Split(",").ToList();
            var newFiles = files.Where(x => newEntryFilesIds.Contains(x.BusinessId.ToString()))
                .Select(x => new FileInfosForDetails
                {
                    Id = x.Id.ToString(),
                    FileSize = x.FileSize,
                    FileType = x.FileType,
                    Name = x.Name,
                    OriginName = x.OriginName,
                    SuffixName = x.SuffixName
                })
                .ToList();
            ur = new LaborServicesInfoDetails
            {
                EntryTime = userEntryInfo.FirstOrDefault()?.EntryTime.ToString("yyyy/MM/dd"),
                EntryMaterial = newFiles,
                UserEntryInfosForDetails = uEntry
            };

            return rt.SuccessResult(ur);
        }
        /// <summary>
        /// 基本信息
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        public async Task<ResponseResult<BaseInfoDetails>> GetBasesicDetailsAsync(string bId)
        {
            ResponseResult<BaseInfoDetails> rt = new();
            BaseInfoDetails ur = new();

            var userInfo = await GetUserInfoAsync(bId);
            #region 匹配简易信息
            ur.Name = userInfo.Name;
            ur.WorkNumber = userInfo.WorkNumber;
            ur.Phone = userInfo.Phone;
            ur.CardId = userInfo.CardId;
            ur.HomeAddress = userInfo.HomeAddress;
            ur.BuildAddress = userInfo.BuildAddress;
            ur.ShipTypeName = EnumUtil.GetDescription(userInfo.ShipType);
            ur.ServiceBookType = EnumUtil.GetDescription(userInfo.ServiceBookType);
            ur.ShipTypeName = EnumUtil.GetDescription(userInfo.ShipType);
            #endregion

            #region 匹配链表字段
            //所在船舶
            var onBoard = await _dbContext.Queryable<OwnerShip>().Where(t => t.BusinessId.ToString() == userInfo.OnBoard).FirstAsync();
            ur.OnBoardName = onBoard?.ShipName;
            //用户状态
            if (userInfo.IsDelete == 1)
            {
                ur.StatusName = EnumUtil.GetDescription(userInfo.DeleteReson);//删除原因获取用户状态
            }
            var userWorkShip = await _dbContext.Queryable<WorkShip>().Where(t => t.IsDelete == 1).OrderByDescending(t => t.Created).FirstAsync();
            if (userWorkShip != null)
            {
                //有休假时间  休假
                if (userWorkShip.HolidayTime != DateTime.MinValue && !string.IsNullOrWhiteSpace(userWorkShip.HolidayTime.ToString()))
                {
                    ur.StatusName = userWorkShip.HolidayTime > DateTime.Now ? EnumUtil.GetDescription(CrewStatusEnum.XiuJia) : "";
                }

                //在岗 待岗
                ur.StatusName = userWorkShip.WorkShipEndTime > DateTime.Now ? EnumUtil.GetDescription(CrewStatusEnum.Normal) : EnumUtil.GetDescription(CrewStatusEnum.DaiGang);

                //当前船舶任职时间
                ur.CurrentShipEntryTime = string.IsNullOrWhiteSpace(userWorkShip.WorkShipStartTime.ToString()) || userWorkShip.WorkShipStartTime == DateTime.MinValue ? "" : userWorkShip.WorkShipStartTime.ToString("yyyy/MM/dd") + "~" + userWorkShip.WorkShipEndTime.ToString("yyyy/MM/dd");
            }
            var userScans = await _dbContext.Queryable<Files>().Where(t => t.IsDelete == 1 && userInfo.BusinessId == t.BusinessId).ToListAsync();
            //照片
            var userPhoto = userScans.FirstOrDefault(x => x.BusinessId.ToString() == userInfo.CrewPhoto);
            ur.PhotoScans = userPhoto == null ? null : new FileInfosForDetails
            {
                Id = userPhoto.Id.ToString(),
                FileSize = userPhoto.FileSize,
                FileType = userPhoto.FileType,
                Name = userPhoto.Name,
                OriginName = userPhoto.OriginName,
                SuffixName = userPhoto.SuffixName
            };
            //扫描件
            var idCardsIds = userInfo.IdCardScans.Split(",").ToList();
            List<FileInfosForDetails> ui = new();
            foreach (var item in userScans.Where(t => t.IsDelete == 1 && idCardsIds.Contains(t.BusinessId.ToString())))
            {
                ui.Add(new FileInfosForDetails
                {
                    Id = item.Id.ToString(),
                    FileSize = item.FileSize,
                    FileType = item.FileType,
                    Name = item.Name,
                    OriginName = item.OriginName,
                    SuffixName = item.SuffixName
                });
            }
            ur.IdCardScans = ui;
            //政治面貌
            var politicalStatus = await _dbContext.Queryable<Political>().FirstAsync(t => t.IsDelete == 1 && userInfo.PoliticalStatus == t.BusinessId.ToString());
            ur.PoliticalStatusName = politicalStatus?.Name;
            //籍贯
            var nanivePlace = await _dbContext.Queryable<AdministrativeDivision>().FirstAsync(t => t.IsDelete == 1 && t.SupRegionalismCode == "0" && t.BusinessId.ToString() == userInfo.NativePlace);
            ur.NativePlaceName = nanivePlace?.Name;
            //民族
            var nation = await _dbContext.Queryable<Nation>().FirstAsync(t => t.IsDelete == 1 && t.BusinessId.ToString() == userInfo.Nation);
            ur.NationName = nation?.Name;
            //船员类型
            var crewType = await _dbContext.Queryable<CrewType>().FirstAsync(t => t.IsDelete == 1 && t.BusinessId.ToString() == userInfo.CrewType);
            ur.CrewTypeName = crewType?.Name;
            //家庭成员
            var familyUser = await _dbContext.Queryable<FamilyUser>().Where(t => t.IsDelete == 1 && t.BusinessId == userInfo.BusinessId).ToListAsync();
            List<UserInfosForDetails> fu = new();
            foreach (var item in familyUser)
            {
                fu.Add(new UserInfosForDetails
                {
                    Id = item.Id.ToString(),
                    Phone = item.Phone,
                    RelationShip = EnumUtil.GetDescription(item.RelationShip),
                    UserName = item.UserName,
                    WorkUnit = item.WorkUnit
                });
            }
            ur.HomeUser = fu;
            //紧急联系人
            var ecUser = await _dbContext.Queryable<EmergencyContacts>().Where(t => t.IsDelete == 1 && t.BusinessId == userInfo.BusinessId).ToListAsync();
            List<UserInfosForDetails> eu = new();
            foreach (var item in ecUser)
            {
                eu.Add(new UserInfosForDetails
                {
                    Id = item.Id.ToString(),
                    Phone = item.Phone,
                    RelationShip = EnumUtil.GetDescription(item.RelationShip),
                    UserName = item.UserName,
                    WorkUnit = item.WorkUnit
                });
            };
            ur.EmergencyContacts = eu;
            #endregion
            return rt.SuccessResult(ur);
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

    }
}
