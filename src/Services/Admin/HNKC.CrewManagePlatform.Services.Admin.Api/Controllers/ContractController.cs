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
        /// <summary>
        /// 职务晋升列表
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        [HttpGet("SearchPromotion")]
        public async Task<IActionResult> SearchPromotionAsync([FromQuery] PromotionRequest requestBody)
        {
            var data = await _contractService.SearchPromotionAsync(requestBody);
            return Ok(data);
        }
        /// <summary>
        /// 职务晋升
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        [HttpPost("SavePromotion")]
        public async Task<Result> SavePromotionAsync([FromBody] PositionPromotion requestBody)
        {
            return await _contractService.SavePromotionAsync(requestBody);
        }
        /// <summary>
        /// 培训记录
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        [HttpGet("SearchTrainingRecord")]
        public async Task<IActionResult> SearchTrainingRecordAsync([FromQuery] TrainingRecordRequest requestBody)
        {
            var data = await _contractService.SearchTrainingRecordAsync(requestBody);
            return Ok(data);
        }
        /// <summary>
        /// 保存培训记录
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        [HttpPost("SaveTrainingRecord")]
        public async Task<Result> SaveTrainingRecordAsync([FromBody] SaveTrainingRecord requestBody)
        {
            return await _contractService.SaveTrainingRecordAsync(requestBody);
        }
        /// <summary>
        /// 年度考核列表
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        [HttpGet("SearchYearCheck")]
        public async Task<IActionResult> SearchYearCheckAsync([FromQuery] YearCheckRequest requestBody)
        {
            var data = await _contractService.SearchYearCheckAsync(requestBody);
            return Ok(data);
        }
        /// <summary>
        /// 年度考核
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        [HttpPost("SaveYearCheck")]
        public async Task<Result> SaveYearCheckAsync([FromBody] SaveYearCheck requestBody)
        {
            return await _contractService.SaveYearCheckAsync(requestBody);
        }
    }
}
