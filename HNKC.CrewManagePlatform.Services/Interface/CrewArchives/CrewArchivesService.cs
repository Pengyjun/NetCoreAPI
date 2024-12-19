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

            var maintab = await _dbContext.Queryable<User>()
                .OrderByDescending(x => x.IsDelete)
                .ToPageListAsync(requestBody.PageIndex, requestBody.PageSize, total);


            return await GetResult(maintab, requestBody, total);
        }
        /// <summary>
        /// 获取列表结果集
        /// </summary>
        /// <param name="maintab"></param>
        /// <param name="requestBody"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        private async Task<PageResult<SearchCrewArchivesResponse>> GetResult(List<User> maintab, SearchCrewArchivesRequest requestBody, RefAsync<int> total)
        {
            List<SearchCrewArchivesResponse> rt = new();

            if (maintab.Any())
            {
                var uIds = maintab.Select(x => x.BusinessId).ToList();

                //所在船舶

                //所在国家

                //在职状态
                var onBoard = await _dbContext.Queryable<WorkShip>().Where(t => t.IsDelete == 1 && uIds.Contains(t.BusinessId)).OrderByDescending(t => t.Created).FirstAsync();
                //用工类型
                var emptCodes = maintab.Select(x => x.EmploymentId).ToList();
                var emptab = await _dbContext.Queryable<EmploymentType>().Where(t => t.IsDelete == 1 && emptCodes.Contains(t.Code)).ToListAsync();
                //第一适任 第二适任
                var firSectab = await _dbContext.Queryable<CertificateOfCompetency>().Where(t => uIds.Contains(t.BusinessId)).ToListAsync();
                //技能证书
                var sctab = await _dbContext.Queryable<SkillCertificates>().Where(t => t.IsDelete == 1 && uIds.Contains(t.BusinessId)).ToListAsync();
                //特设证书
                var spctab = await _dbContext.Queryable<SpecialEquips>().Where(t => t.IsDelete == 1 && uIds.Contains(t.BusinessId)).ToListAsync();

                foreach (var t in maintab)
                {
                    var sctabNames = "";//技能证书
                    var spctabNames = "";//特设证书
                    foreach (var s in sctab.Where(x => x.BusinessId == t.BusinessId)) { sctabNames += EnumUtil.GetDescription(s.SkillCertificateType) + "、"; }
                    foreach (var s in spctab.Where(x => x.BusinessId == t.BusinessId)) { spctabNames += EnumUtil.GetDescription(s.SpecialEquipsCertificateType) + "、"; }

                    rt.Add(new SearchCrewArchivesResponse
                    {
                        UserName = t.Name,
                        BtnType = t.IsDelete == 0 ? 1 : 0,
                        ShipType = EnumUtil.GetDescription(t.ShipType),
                        WorkNumber = t.WorkNumber,
                        EmploymentType = emptab.FirstOrDefault(x => x.Code == t.EmploymentId)?.Name,
                        CrewTypee = t.CrewType,
                        FCertificate = firSectab?.FirstOrDefault(x => x.BusinessId == t.BusinessId)?.FPosition,
                        SCertificate = firSectab?.FirstOrDefault(x => x.BusinessId == t.BusinessId)?.SPosition,
                        ServiceBook = EnumUtil.GetDescription(t.ServiceBookType),
                        Id = t.BusinessId,
                        SkillsCertificate = sctabNames,
                        SpecialCertificate = spctabNames,
                        Age = CalculateAgeFromIdCard(t.CardId),
                        OnStatus = ShipUserStatus(onBoard.WorkShipStartTime, onBoard.WorkShipEndTime, onBoard.HolidayTime, t.DeleteReson),

                    });
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
        /// <param name="boardTime">上船时间</param>
        /// <param name="departureTime">下船时间</param>
        /// <param name="holidayTime">休假时间</param>
        /// <param name="deleteResonEnum">是否删除</param>
        /// <returns></returns>
        private static string ShipUserStatus(DateTime boardTime, DateTime departureTime, DateTime? holidayTime, DeleteResonEnum deleteResonEnum)
        {
            if (deleteResonEnum != DeleteResonEnum.Normal)
            {
                //删除：管理人员手动操作，包括离职、调离和退休，优先于其他任何状态
                return EnumUtil.GetDescription(deleteResonEnum);
            }
            else if (holidayTime.HasValue)
            {
                //休假：提交离船申请且经审批同意后，按所申请离船、归船日期设置为休假状态，优先于在岗、待岗状态
                return EnumUtil.GetDescription(DeleteResonEnum.XiuJia);
            }
            else if (boardTime >= DateTime.Now && departureTime <= DateTime.Now)
            {
                //在岗、待岗:船员登记时必填任职船舶数据，看其中最新的任职船舶上船时间和下船时间，在此时间内为在岗状态，否则为待岗状态
                return EnumUtil.GetDescription(DeleteResonEnum.Normal); ;
            }
            return "";
        }
        #endregion
    }
}
