using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Dtos.PullResult;

namespace HNKC.CrewManagePlatform.Services.Interface.PullResult
{
    /// <summary>
    /// 数据字典接口
    /// </summary>
    public interface IDataDictionaryService
    {
        /// <summary>
        /// 获取主数据值域&基础数据
        /// </summary>
        /// <param name="pullRequestDto"></param>
        /// <returns></returns>
        Task<Result> SaveValueDomainAsync(PullRequestDto pullRequestDto);
    }
}
