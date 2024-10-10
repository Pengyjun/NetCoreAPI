using GDCMasterDataReceiveApi.Application.Contracts.IService.IReceiveDHDataService;
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


    }
}
