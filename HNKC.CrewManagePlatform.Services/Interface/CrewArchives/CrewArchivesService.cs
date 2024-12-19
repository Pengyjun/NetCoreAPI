using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Dtos;
using HNKC.CrewManagePlatform.Models.Dtos.CrewArchives;
using HNKC.CrewManagePlatform.Models.Enums;
using HNKC.CrewManagePlatform.SqlSugars.Models;
using HNKC.CrewManagePlatform.Utils;
using SqlSugar;
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
        public async Task<Result> SearchCrewArchivesAsync(SearchCrewArchivesRequest requestBody)
        {
            RefAsync<int> total = 0;

            //名称相关不赋值
            var rt = await _dbContext.Queryable<User>()
                .WhereIF(!string.IsNullOrWhiteSpace(requestBody.KeyWords), t => t.Name.Contains(requestBody.KeyWords) || t.CardId.Contains(requestBody.KeyWords)
                || t.Phone.Contains(requestBody.KeyWords) || t.WorkNumber.Contains(requestBody.KeyWords))
                .LeftJoin<WorkShip>((t, ws) => t.BusinessId == ws.BusinessId)
                .LeftJoin<CrewType>((t, ws, ct) => t.BusinessId == ct.BusinessId)
                .LeftJoin<CertificateOfCompetency>((t, ws, ct, coc) => t.BusinessId == coc.BusinessId)
                .LeftJoin<SkillCertificates>((t, ws, ct, coc, sf) => t.BusinessId == sf.BusinessId)
                .LeftJoin<OwnerShip>((t, ws, ct, coc, sf, ow) => ws.OnShip == sf.BusinessId.ToString())
                .LeftJoin<EducationalBackground>((t, ws, ct, coc, sf, ow, eb) => eb.BusinessId == t.BusinessId)
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
                    EmploymentType = t.EmploymentId,
                    ServiceBookType = t.ServiceBookType,
                    CrewType = t.CrewType,
                    IsDelete = t.IsDelete,
                    DeleteReson = t.DeleteReson
                })
                .OrderByDescending(t => t.IsDelete)
                .ToListAsync();
            return Result.Success(await GetResult(requestBody, total, rt));
        }
        /// <summary>
        /// 获取列表结果集
        /// </summary>
        /// <param name="requestBody"></param>
        /// <param name="total"></param>
        /// <param name="rt"></param>
        /// <returns></returns>
        private async Task<PageResult<SearchCrewArchivesResponse>> GetResult(SearchCrewArchivesRequest requestBody, RefAsync<int> total, List<SearchCrewArchivesResponse> rt)
        {
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
                var emptab = await _dbContext.Queryable<EmploymentType>().Where(t => t.IsDelete == 1 && emptCodes.Contains(t.Code)).ToListAsync();
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
                    foreach (var s in spctab.Where(x => x.BusinessId == t.BId))
                    {
                        var val = (int)s.SpecialEquipsCertificateType;
                        spctabs.Add(val.ToString());
                        spctabNames += EnumUtil.GetDescription(s.SpecialEquipsCertificateType) + "、";
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
                            var country = countrytab.FirstOrDefault(x => ownerShips.Country == t.BId.ToString());
                            t.OnCountry = ownerShips.Country;
                            t.OnCountryName = country?.Name;
                        }
                    }
                    t.ShipTypeName = EnumUtil.GetDescription(t.ShipType);
                    t.EmploymentTypeName = emptab.FirstOrDefault(x => x.Code == t.EmploymentType)?.Name;
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

            return new PageResult<SearchCrewArchivesResponse>()
            {
                List = rt.Skip((requestBody.PageIndex - 1) * requestBody.PageSize).Take(requestBody.PageSize),
                PageIndex = requestBody.PageIndex,
                PageSize = requestBody.PageSize,
                TotalCount = total
            };
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
        public async Task<Result> CrewArchivesCountAsync()
        {
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

            return Result.Success(new CrewArchivesResponse
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
            });
        }
        /// <summary>
        /// 数据保存
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        public async Task<Result> SaveDataAsync(CrewArchivesRequest requestBody)
        {
            if (requestBody.BId == Guid.Empty || string.IsNullOrWhiteSpace(requestBody.BId.ToString())) { return await InsertDataAsync(requestBody); }
            else { return await UpdateDataAsync(requestBody); }
        }
        /// <summary>
        /// 数据新增
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        private async Task<Result> InsertDataAsync(CrewArchivesRequest requestBody)
        {
            #region 基本信息
            User userInfo = new();
            List<FamilyUser> hus = new();
            List<EmergencyContacts> ecs = new();

            var uId = GuidUtil.Next();
            if (requestBody.BaseInfoDto != null)
            {
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
                    EntryTime = requestBody.BaseInfoDto.EntryTime,
                    EntryScans = requestBody.BaseInfoDto.EntryScans,
                    EmploymentId = requestBody.BaseInfoDto.EmploymentId,
                    LaborCompany = requestBody.BaseInfoDto.LaborCompany,
                    ContarctMain = requestBody.BaseInfoDto.ContarctMain,
                    ContarctType = requestBody.BaseInfoDto.ContarctType,
                    StartTime = requestBody.BaseInfoDto.StartTime,
                    EndTime = requestBody.BaseInfoDto.EndTime
                };
                //家庭成员
                if (requestBody.BaseInfoDto.HomeUser != null && requestBody.BaseInfoDto.HomeUser.Any())
                {
                    foreach (var item in requestBody.BaseInfoDto.HomeUser)
                    {
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
                    foreach (var item in requestBody.BaseInfoDto.EmergencyContacts)
                    {
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
                            VisaType = item.VisaType
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

            return Result.Success("新增成功");
        }
        /// <summary>
        /// 数据修改
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        private async Task<Result> UpdateDataAsync(CrewArchivesRequest requestBody)
        {
            return Result.Success("修改成功");
        }
        /// <summary>
        /// 基本下拉列表
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<Result> GetDropDownListAsync(int type)
        {
            switch (type)
            {
                case 1://籍贯
                    return await GetNativePlaceListAsync();
                case 2://民族
                    return await GetNationListAsync();
                case 3://船员类型
                    return await GetCrewTypeListAsync();
                case 4://船舶
                    return await GetOwnerShipListAsync();
                case 5://培训类型
                    return await GetTrainingListAsync();
                case 6://国家
                    return await GetCountryListAsync();
                case 7://家庭关系
                    return GetFamilyRelationList();
                case 8://船舶类型
                    return GetShipTypeList();
                case 9://服务簿类型
                    return GetServiceBookList();
                case 10://签证类型
                    return GetVisaTypeList();
                case 11://技能证书
                    return GetCertificateTypeList();
                case 12://学历类型
                    return GetQualificationTypeList();
                case 13://学历
                    return GetQualificationList();
                case 14://合同类型
                    return GetContractList();
                case 15://考核类型
                    return GetCheckList();
                case 16://船舶状态
                    return GetShipStateEnumList();
            }
            return Result.Success("获取成功");
        }
        /// <summary>
        /// 籍贯
        /// </summary>
        /// <returns></returns>
        private async Task<Result> GetNativePlaceListAsync()
        {
            var rt = await _dbContext.Queryable<AdministrativeDivision>().Where(t => t.IsDelete == 1 && t.SupRegionalismCode == "0").Select(t => new DropDownResponse
            {
                Key = t.BusinessId.ToString(),
                Value = t.Name,
            }).ToListAsync();
            return Result.Success(rt);
        }
        /// <summary>
        /// 民族
        /// </summary>
        /// <returns></returns>
        private async Task<Result> GetNationListAsync()
        {
            var rt = await _dbContext.Queryable<Nation>().Where(t => t.IsDelete == 1).Select(t => new DropDownResponse
            {
                Key = t.BusinessId.ToString(),
                Value = t.Name,
            }).ToListAsync();
            return Result.Success(rt);
        }
        /// <summary>
        /// 船员类型
        /// </summary>
        /// <returns></returns>
        private async Task<Result> GetCrewTypeListAsync()
        {
            var rt = await _dbContext.Queryable<CrewType>().Where(t => t.IsDelete == 1).Select(t => new DropDownResponse
            {
                Key = t.BusinessId.ToString(),
                Value = t.Name,
            }).ToListAsync();
            return Result.Success(rt);
        }
        /// <summary>
        /// 船舶
        /// </summary>
        /// <returns></returns>
        private async Task<Result> GetOwnerShipListAsync()
        {
            var rt = await _dbContext.Queryable<OwnerShip>().Where(t => t.IsDelete == 1).Select(t => new DropDownResponse
            {
                Key = t.BusinessId.ToString(),
                Value = t.ShipName,
            }).ToListAsync();
            return Result.Success(rt);
        }
        /// <summary>
        /// 培训类型
        /// </summary>
        /// <returns></returns>
        private async Task<Result> GetTrainingListAsync()
        {
            var rt = await _dbContext.Queryable<TrainingType>().Where(t => t.IsDelete == 1).Select(t => new DropDownResponse
            {
                Key = t.BusinessId.ToString(),
                Value = t.Name,
            }).ToListAsync();
            return Result.Success(rt);
        }
        /// <summary>
        /// 国家
        /// </summary>
        /// <returns></returns>
        private async Task<Result> GetCountryListAsync()
        {
            var rt = await _dbContext.Queryable<CountryRegion>().Where(t => t.IsDelete == 1).Select(t => new DropDownResponse
            {
                Key = t.BusinessId.ToString(),
                Value = t.Name,
            }).ToListAsync();
            return Result.Success(rt);
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
                                                       Value = GetEnumDescription(x)
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
                                                       Value = GetEnumDescription(x)
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
                                                       Value = GetEnumDescription(x)
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
                                                       Value = GetEnumDescription(x)
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
                                                       Value = GetEnumDescription(x)
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
                                                       Value = GetEnumDescription(x)
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
                                                       Value = GetEnumDescription(x)
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
                                                       Value = GetEnumDescription(x)
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
                                                       Value = GetEnumDescription(x)
                                                   })
                                                   .ToList();
            return Result.Success(enumConvertList);
        }
        /// <summary>
        /// 船舶状态
        /// </summary>
        /// <returns></returns>
        private static Result GetShipStateEnumList()
        {
            var enumConvertList = Enum.GetValues(typeof(ShipStateEnum))
                                                   .Cast<ShipStateEnum>()
                                                   .Select(x => new DropDownResponse
                                                   {
                                                       Key = ((int)x).ToString(),
                                                       Value = GetEnumDescription(x)
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
    }
}
