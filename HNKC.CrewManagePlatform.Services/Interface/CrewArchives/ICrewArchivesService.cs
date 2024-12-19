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
        Task<Result> CrewArchivesCountAsync();
        /// <summary>
        /// 获取基本下拉列表
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<Result> GetDropDownListAsync(int type);

        /// <summary>
        /// 数据保存
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        Task<Result> SaveDataAsync(CrewArchivesRequest requestBody);
        /// <summary>
        /// 船员档案列表
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        Task<Result> SearchCrewArchivesAsync(SearchCrewArchivesRequest requestBody);
    }
}
