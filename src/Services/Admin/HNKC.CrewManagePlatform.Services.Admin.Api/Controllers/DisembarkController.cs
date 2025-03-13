using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Dtos.Disembark;
using HNKC.CrewManagePlatform.Services.Interface.Disembark;
using HNKC.CrewManagePlatform.SqlSugars.UnitOfTransaction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HNKC.CrewManagePlatform.Services.Admin.Api.Controllers
{
    /// <summary>
    /// 离船控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DisembarkController : BaseController
    {
        private readonly IDisembarkService _disembarkService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="disembarkService"></param>
        public DisembarkController(IDisembarkService disembarkService)
        {
            this._disembarkService = disembarkService;
        }
        /// <summary>
        /// 保存离船申请
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        [HttpPost("SaveDisembark")]
        [Transactional]
        public async Task<Result> SaveCrewDisembarkAsync([FromBody] DisembarkRequest requestBody)
        {
            return await _disembarkService.SaveCrewDisembarkAsync(requestBody);
        }
        /// <summary>
        /// 离船申请列表
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        [HttpGet("SearchDisembark")]
        public async Task<IActionResult> SearchCrewDisembarkAsync([FromQuery] SearchDisembarkRequest requestBody)
        {
            var data = await _disembarkService.SearchCrewDisembarkAsync(requestBody);
            return Ok(data);
        }
        /// <summary>
        /// 保存船舶排班
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        [HttpPost("SaveCrewRota")]
        [Transactional]
        public async Task<Result> SaveCrewRotaAsync([FromBody] SaveSchedulingRequest requestBody)
        {
            return await _disembarkService.SaveCrewRotaAsync(requestBody);
        }
        /// <summary>
        /// 船员船舶排班回显
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        [HttpGet("CrewRotaList")]
        public async Task<Result> CrewRotaListAsync([FromQuery] SchedulingRequest requestBody)
        {
            return await _disembarkService.CrewRotaListAsync(requestBody);
        }
        /// <summary>
        /// 船舶排班用户下拉列表
        /// </summary>
        /// <param name="shipId"></param>
        /// <returns></returns>
        [HttpGet("CrewRotaUserList")]
        public async Task<Result> CrewRotaUserListAsync(Guid shipId)
        {
            return await _disembarkService.CrewRotaUserListAsync(shipId);
        }
        /// <summary>
        /// 值班管理列表
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        [HttpGet("SearchCrewRota")]
        public async Task<IActionResult> SearchCrewRotaAsync([FromQuery] SearchCrewRotaRequest requestBody)
        {
            var data = await _disembarkService.SearchCrewRotaAsync(requestBody);
            return Ok(data);
        }
        /// <summary>
        /// 年休假计划列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet("SearchAnnualLeavePlan")]
        public async Task<IActionResult> SearchAnnualLeavePlanAsync([FromQuery] AnnualLeavePlanRequestDto requestDto)
        {
            var data = await _disembarkService.SearchAnnualLeavePlanAsync(requestDto);
            return Ok(data);
        }
    }
}
