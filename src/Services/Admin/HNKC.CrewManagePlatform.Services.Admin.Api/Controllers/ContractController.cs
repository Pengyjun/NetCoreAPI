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
        /// <summary>
        /// 合同列表
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        [HttpGet("SearchContract")]
        public async Task<IActionResult> SearchContractAsync([FromQuery] ContractRequest requestBody)
        {
            var data = await _contractService.SearchContractAsync(requestBody);
            return Ok(data);
        }
        /// <summary>
        /// 合同续签
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        [HttpPost("SaveContract")]
        public async Task<Result> SaveContractAsync([FromBody] ConntractRenewal requestBody)
        {
            return await _contractService.SaveContractAsync(requestBody);
        }

    }
}
