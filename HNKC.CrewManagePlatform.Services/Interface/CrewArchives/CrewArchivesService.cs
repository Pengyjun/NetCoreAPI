using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Dtos.CrewArchives;
using HNKC.CrewManagePlatform.SqlSugars.Models;
using HNKC.CrewManagePlatform.Utils;
using SqlSugar;
using System.Linq.Expressions;
using UtilsSharp.Shared.Standard;

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
        /// <summary>
        /// 船员档案列表
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        public async Task<PageResult<SearchCrewArchivesResponse>> SearchCrewArchivesAsync(SearchCrewArchivesRequest requestBody)
        {
            PageResult<SearchCrewArchivesResponse> resResult = new();
            RefAsync<int> total = 0;

            var maintab = await _dbContext.Queryable<User>()
                .OrderByDescending(x => x.IsDelete)
                .ToPageListAsync(requestBody.PageIndex, requestBody.PageSize, total);

            if (maintab.Any())
            {
                List<SearchCrewArchivesResponse> rt = new();

                //所在船舶

                //所在国家

                //用工类型
                var emptCodes = maintab.Select(x => x.EmploymentId).ToList();
                var emptab = await _dbContext.Queryable<EmploymentType>().Where(t => t.IsDelete == 1 && emptCodes.Contains(t.Code)).ToListAsync();
                //第一适任
                var uIds = maintab.Select(x => x.BusinessId).ToList();
                var firtab = await _dbContext.Queryable<CertificateOfCompetency>().Where(t => uIds.Contains(t.UserId)).ToListAsync();
                //第二适任

                foreach (var t in maintab)
                {
                    rt.Add(new SearchCrewArchivesResponse
                    {
                        UserName = t.Name,
                        BtnType = t.IsDelete == 0 ? 1 : 0,
                        ShipType = EnumUtil.GetDescription(t.ShipType),
                        WorkNumber = t.WorkNumber,
                        OnBoard = t.OnBoard,
                        EmploymentType = emptab.FirstOrDefault(x => x.Code == t.EmploymentId)?.Name,
                        CrewTypee = t.CrewType,
                        FCertificate = t.CrewType
                    });
                }
            }
            //状态、年龄
            return resResult;
        }
    }
}
