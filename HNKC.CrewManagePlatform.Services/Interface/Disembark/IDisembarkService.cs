using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Dtos.Disembark;

namespace HNKC.CrewManagePlatform.Services.Interface.Disembark
{
    /// <summary>
    /// 离船
    /// </summary>
    public interface IDisembarkService
    {
        /// <summary>
        /// 保存离船申请
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        Task<Result> SaveDisembarkAsync(DisembarkRequest requestBody);
        /// <summary>
        /// 离船申请列表
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        Task<PageResult<SearchDisembark>> SearchDisembarkAsync(SearchDisembarkRequest requestBody);
    }
}
