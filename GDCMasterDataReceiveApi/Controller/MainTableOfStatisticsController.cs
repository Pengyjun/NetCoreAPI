using GDCMasterDataReceiveApi.Application.Contracts.Dto.MainTableOfStatistics;
using GDCMasterDataReceiveApi.Application.Contracts.IMainTableOfStatistics;
using GDCMasterDataReceiveApi.Domain.Shared;
using GDCMasterDataReceiveApi.Domain.Shared.Annotation;
using Microsoft.AspNetCore.Mvc;

namespace GDCMasterDataReceiveApi.Controller
{
    /// <summary>
    /// 统计数据库每日增改量
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MainTableOfStatisticsController : BaseController
    {
        /// <summary>
        /// 接口服务引入
        /// </summary>
        public readonly IMainTableOfStatisticsService _mainTableOfStatisticsService;
        /// <summary>
        /// 接口注入
        /// </summary>
        public MainTableOfStatisticsController(IMainTableOfStatisticsService mainTableOfStatisticsService)
        {
            _mainTableOfStatisticsService = mainTableOfStatisticsService;
        }
        /// <summary>
        /// 统计当前模式所有表（当前指定数据库 按小时更新）
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("InsertModifyHourIncrementalDmData")]
        [UnitOfWork]
        public ResponseAjaxResult<bool> InsertModifyHourIncrementalDmData([FromBody] MainTableOfStatisticsRequestDto requestDto)
            => _mainTableOfStatisticsService.InsertModifyHourIncrementalData(requestDto);
        /// <summary>
        /// 统计当前模式所有表（Mysql当前指定数据库）
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("InsertModifyHourIncrementalMysqlData")]
        [UnitOfWork]
        public async Task<ResponseAjaxResult<bool>> InsertModifyHourIncrementalDataAsync([FromBody] MainTableOfStatisticsMysqlRequestDto requestDto)
            => await _mainTableOfStatisticsService.InsertModifyHourIncrementalData(requestDto);
    }
}
