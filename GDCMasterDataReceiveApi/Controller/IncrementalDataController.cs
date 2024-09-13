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
        /// 操作数据
        /// </summary>
        /// <returns></returns>
        [HttpPost("/api/OperateData")]
        public async Task<ResponseAjaxResult<bool>> OperateDataAsync([FromBody] List<string>? tableNames)
        {
            return await _incrementalDataSearchService.OperateDataAsync(tableNames);
        }
    }
}
