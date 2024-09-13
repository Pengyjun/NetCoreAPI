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
