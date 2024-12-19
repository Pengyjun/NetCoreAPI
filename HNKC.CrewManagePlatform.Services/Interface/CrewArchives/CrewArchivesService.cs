using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Dtos.CrewArchives;
using HNKC.CrewManagePlatform.Models.Enums;
using HNKC.CrewManagePlatform.SqlSugars.Models;
using HNKC.CrewManagePlatform.Utils;
using SqlSugar;
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
        public async Task<PageResult<SearchCrewArchivesResponse>> SearchCrewArchivesAsync(SearchCrewArchivesRequest requestBody)
        {
            RefAsync<int> total = 0;

            var rt = await _dbContext.Queryable<User>()
                .Select(t => new SearchCrewArchivesResponse //名称相关不赋值
                {
                    Id = t.BusinessId,
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
                .OrderByDescending(x => x.IsDelete)
                .ToPageListAsync(requestBody.PageIndex, requestBody.PageSize, total);

            return await GetResult(requestBody, total, rt);
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
                var uIds = rt.Select(x => x.Id).ToList();
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
                    foreach (var s in sctab.Where(x => x.BusinessId == t.Id))
                    {
                        var val = (int)s.SkillCertificateType;
                        sctabs.Add(val.ToString());
                        sctabNames += EnumUtil.GetDescription(s.SkillCertificateType) + "、";
                    }
                    foreach (var s in spctab.Where(x => x.BusinessId == t.Id))
                    {
                        var val = (int)s.SpecialEquipsCertificateType;
                        spctabs.Add(val.ToString());
                        spctabNames += EnumUtil.GetDescription(s.SpecialEquipsCertificateType) + "、";
                    }
                    var ob = onBoardtab.Where(x => x.BusinessId == t.Id).OrderByDescending(x => x.Created).FirstOrDefault();
                    if (ob != null)
                    {
                        //所在船舶
                        var ownerShips = ownerShipstab.FirstOrDefault(x => ob.OnShip == x.BusinessId.ToString());
                        t.OnBoardName = ownerShips?.ShipName;
                        t.OnBoard = ob.OnShip;
                        if (ownerShips != null)
                        {
                            //所在国家
                            var country = countrytab.FirstOrDefault(x => ownerShips.Country == t.Id.ToString());
                            t.OnCountry = ownerShips.Country;
                            t.OnCountryName = country?.Name;
                        }
                    }
                    t.ShipTypeName = EnumUtil.GetDescription(t.ShipType);
                    t.EmploymentTypeName = emptab.FirstOrDefault(x => x.Code == t.EmploymentType)?.Name;
                    t.CrewTypeName = crewTypetab.FirstOrDefault(x => t.CrewType == x.BusinessId.ToString())?.Name;
                    t.FCertificate = firSectab?.FirstOrDefault(x => x.BusinessId == t.Id)?.FPosition;
                    if (t.FCertificate != null) t.FCertificateName = firSecPosition.FirstOrDefault(x => x.BusinessId.ToString() == t.FCertificate)?.Name;
                    t.SCertificate = firSectab?.FirstOrDefault(x => x.BusinessId == t.Id)?.SPosition;
                    if (t.SCertificate != null) t.SCertificateName = firSecPosition.FirstOrDefault(x => x.BusinessId.ToString() == t.SCertificate)?.Name;
                    t.ServiceBookName = EnumUtil.GetDescription(t.ServiceBookType);
                    t.SkillsCertificateName = sctabNames;
                    t.SkillsCertificate = sctabs;
                    t.SpecialCertificate = spctabs;
                    t.SpecialCertificateName = spctabNames;
                    t.Age = CalculateAgeFromIdCard(t.CardId);
                    t.OnStatus = ob == null ? DeleteResonEnum.Normal : ShipUserStatus(ob.WorkShipEndTime, ob.HolidayTime, t.DeleteReson);
                    t.OnStatusName = ob == null ? null : EnumUtil.GetDescription(ShipUserStatus(ob.WorkShipEndTime, ob.HolidayTime, t.DeleteReson));
                }
            }

            return new PageResult<SearchCrewArchivesResponse>()
            {
                List = rt,
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
        private static DeleteResonEnum ShipUserStatus(DateTime departureTime, DateTime? holidayTime, DeleteResonEnum deleteResonEnum)
        {
            var status = new DeleteResonEnum();
            if (deleteResonEnum != DeleteResonEnum.Normal)
            {
                //删除：管理人员手动操作，包括离职、调离和退休，优先于其他任何状态
                status = deleteResonEnum;
            }
            else if (holidayTime.HasValue)
            {
                //休假：提交离船申请且经审批同意后，按所申请离船、归船日期设置为休假状态，优先于在岗、待岗状态
                status = DeleteResonEnum.XiuJia;
            }
            else if (departureTime <= DateTime.Now)
            {
                //在岗、待岗:船员登记时必填任职船舶数据，看其中最新的任职船舶上船时间和下船时间，在此时间内为在岗状态，否则为待岗状态
                status = DeleteResonEnum.Normal;
            }
            return status;
        }

        #endregion
        /// <summary>
        /// 首页占比及统计数
        /// </summary>
        /// <returns></returns>
        public async Task<CrewArchivesResponse> CrewArchivesCountAsync()
        {
            var udtab = await _dbContext.Queryable<User>().ToListAsync();
            var udtabIds = udtab.Select(x => x.BusinessId).ToList();
            var onBoard = await _dbContext.Queryable<WorkShip>().Where(t => t.IsDelete == 1 && udtabIds.Contains(t.BusinessId)).ToListAsync();

            var totalCount = udtab.Count();//总数

            var onDutyCount = onBoard.Where(x => x.WorkShipEndTime <= DateTime.Now).Select(x => x.BusinessId).Distinct().Count();//在船数
            var onDutyProp = totalCount == 0 ? 0 + "%" : Convert.ToInt32(onDutyCount / totalCount) + "%";

            var waitCount = onBoard.Where(x => x.WorkShipEndTime > DateTime.Now).Select(x => x.BusinessId).Distinct().Count();//待岗数
            var waitProp = totalCount == 0 ? 0 + "%" : Convert.ToInt32(waitCount / totalCount) + "%";

            var holidayCount = onBoard.Where(x => x.HolidayTime > DateTime.Now).Select(x => x.BusinessId).Distinct().Count();//休假数
            var holidayProp = totalCount == 0 ? 0 + "%" : Convert.ToInt32(holidayCount / totalCount) + "%";

            var otherCount = udtab.Where(x => x.DeleteReson != DeleteResonEnum.Normal && x.DeleteReson != DeleteResonEnum.XiuJia).Count();//离调退
            var otherProp = totalCount == 0 ? 0 + "%" : Convert.ToInt32(otherCount / totalCount) + "%";

            return new CrewArchivesResponse()
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
        }

    }
}
