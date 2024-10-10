using GDCMasterDataReceiveApi.Application.Contracts.IService.IReceiveDHDataService;
using GDCMasterDataReceiveApi.Domain.Shared;
using Microsoft.AspNetCore.Mvc;

namespace GDCMasterDataReceiveApi.Controller
{
    /// <summary>
    /// 对接DH相关数据
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiveDHDataController : BaseController
    {
        private readonly IReceiveDHDataService _receiveDHDataService;
        /// <summary>
        /// 
        /// </summary>
        public ReceiveDHDataController(IReceiveDHDataService receiveDHDataService)
        {
            this._receiveDHDataService = receiveDHDataService;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("ReceiveDHOrganzation")]
        public async Task<ResponseAjaxResult<bool>> ReceiveOrganzationAsync()
        {
            return await _receiveDHDataService.ReceiveOrganzationAsync();
        }
    }
}
