using GDCMasterDataReceiveApi.Application.Contracts.Dto;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.IncrementalData;
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



        /// <summary>
        /// 首页各类主数据统计数量
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<EachMainDataCountResponseDto>> SearchEachMainDataCountAsync();



        /// <summary>
        /// 首页各个公司主数据统计数量
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<EachCompanyMainDataCountResponseDto>>> SearchEachCompanyMainDataCountAsync(int  type);


        /// <summary>
        /// 首页各个主数据增长数量
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<EachMainDataCountResponseDto>>> SearchEachDayMainDataCountAsync(string startTime, string endTime);


        /// <summary>
        /// 首页各个主数据增长数量
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<EachAPIInterdaceCountResponseDto>>> SearchCallInterfaceCountAsync(string timeStr, string timeEnd,int type);
    }
}
