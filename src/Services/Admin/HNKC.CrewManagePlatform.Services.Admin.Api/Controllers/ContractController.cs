using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Dtos.Contract;
using HNKC.CrewManagePlatform.Services.Interface.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HNKC.CrewManagePlatform.Services.Admin.Api.Controllers
{
    /// <summary>
    /// 合同控制器 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ContractController : BaseController
    {
        private readonly IContractService _contractService;
        /// <summary>
        /// 注入
        /// </summary>
        /// <param name="contractService"></param>
        public ContractController(IContractService contractService)
        {
            this._contractService = contractService;
        }
        ///// <summary>
        ///// 合同列表
        ///// </summary>
        ///// <param name="requestBody"></param>
        ///// <returns></returns>
        //[HttpGet("SearchContract")]
        //public async Task<ResponsePageResult<List<ContractSearch>>> SearchContractAsync([FromQuery] ContractRequest requestBody)
        //{
        //    return await _contractService.SearchContractAsync(requestBody);
        //}

    }
}
