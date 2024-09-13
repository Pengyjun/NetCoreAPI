using GDCMasterDataReceiveApi.Application.Contracts.Dto;
using GDCMasterDataReceiveApi.Application.Contracts.IService.ISearchService;
using GDCMasterDataReceiveApi.Domain.Shared;
using Microsoft.AspNetCore.Mvc;

namespace GDCMasterDataReceiveApi.Controller
{
    /// <summary>
    /// 增量数据统计
    /// </summary>
    [ApiController]
    public class IncrementalDataController : BaseController
    {
        private readonly IIncrementalDataSearchService _incrementalDataSearchService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="incrementalDataSearchService"></param>
        public IncrementalDataController(IIncrementalDataSearchService incrementalDataSearchService)
        {
            this._incrementalDataSearchService = incrementalDataSearchService;
        }
        /// <summary>
        /// 操作增量数据（定时接口/手动调用）
        /// </summary>
        /// <param name="tableNames">表名</param>
        /// <returns></returns>
        [HttpPost("/api/OperateData")]
        public async Task<ResponseAjaxResult<bool>> OperateDataAsync([FromBody] List<string>? tableNames)
        {
            return await _incrementalDataSearchService.OperateDataAsync(tableNames);
        }
        /// <summary>
        /// 获取每日增量数据
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet("/api/GetIncrementalSearch")]
        public async Task<ResponseAjaxResult<IncrementalDataDto>> GetIncrementalSearchAsync([FromQuery] IncrementalSearchRequestDto requestDto)
        {
            return await _incrementalDataSearchService.GetIncrementalSearchAsync(requestDto);
        }
    }
}
