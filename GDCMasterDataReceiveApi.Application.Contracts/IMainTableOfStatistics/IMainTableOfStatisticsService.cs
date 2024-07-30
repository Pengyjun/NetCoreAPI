using GDCMasterDataReceiveApi.Application.Contracts.Dto.MainTableOfStatistics;
using GDCMasterDataReceiveApi.Domain.Shared;
using GDCMasterDataReceiveApi.Domain.Shared.Annotation;

namespace GDCMasterDataReceiveApi.Application.Contracts.IMainTableOfStatistics
{
    /// <summary>
    /// 统计数据库 每天增改量接口
    /// </summary>
    [DependencyInjection]
    public interface IMainTableOfStatisticsService
    {
        /// <summary>
        /// 统计当前模式所有表（当前指定数据库） 
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        ResponseAjaxResult<bool> InsertModifyHourIncrementalData(MainTableOfStatisticsRequestDto requestDto);
    }
}
