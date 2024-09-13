using GDCMasterDataReceiveApi.Application.Contracts.Dto;
using GDCMasterDataReceiveApi.Domain.Shared;
using GDCMasterDataReceiveApi.Domain.Shared.Annotation;

namespace GDCMasterDataReceiveApi.Application.Contracts.IService.ISearchService
{
    /// <summary>
    /// 统计增量数据接口
    /// </summary>
    [DependencyInjection]
    public interface IIncrementalDataSearchService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<IncrementalDataDto>> GetIncrementalSearchAsync(IncrementalSearchRequestDto requestDto);
    }
}
