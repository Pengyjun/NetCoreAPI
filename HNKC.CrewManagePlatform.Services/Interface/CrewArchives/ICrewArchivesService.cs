using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Dtos.CrewArchives;

namespace HNKC.CrewManagePlatform.Services.Interface.CrewArchives
{
    /// <summary>
    /// 船员档案
    /// </summary>
    public interface ICrewArchivesService
    {
        /// <summary>
        /// 首页占比 及 数量 统计
        /// </summary>
        /// <returns></returns>
        Task<CrewArchivesResponse> CrewArchivesCountAsync();

        /// <summary>
        /// 船员档案列表
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        Task<PageResult<SearchCrewArchivesResponse>> SearchCrewArchivesAsync(SearchCrewArchivesRequest requestBody);
    }
}
