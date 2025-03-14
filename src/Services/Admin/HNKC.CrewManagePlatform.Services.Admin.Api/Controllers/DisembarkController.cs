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
        /// 获取离船申请列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("ApplyList")]
        public async Task<IActionResult> DepartureApplyListAsync([FromQuery] DepartureApplyQuery query)
        {
            var data = await _disembarkService.DepartureApplyListAsync(query);
            return Ok(data);
        }

        /// <summary>
        /// 提交离船申请
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        [HttpPost("SubmitApply")]
        [Transactional]
        public async Task<Result> SubmitDepartureApply([FromBody] DepartureApplyDto requestBody)
        {
            return await _disembarkService.SubmitDepartureApplyAsync(requestBody);
        }

        /// <summary>
        /// 执行审批
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        [HttpPost("SubmitApprove")]
        [Transactional]
        public async Task<Result> SubmitApprove([FromBody] SubmitApproveRequestDto requestBody)
        {
            return await _disembarkService.SubmitApprove(requestBody);
        }

        /// <summary>
        /// 填报实际离船时间
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        [HttpPost("RegisterActualTime")]
        public async Task<Result> RegisterActualTime([FromBody] RegisterActualTimeDto requestBody)
        {
            return await _disembarkService.RegisterActualTime(requestBody);
        }

        /*/// <summary>
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
        }*/

        /// <summary>
        /// 申请用户下拉列表
        /// </summary>
        /// <param name="shipId"></param>
        /// <returns></returns>
        [HttpGet("ShipUserList")]
        public async Task<Result> ShipUserListAsync(Guid shipId)
        {
            return await _disembarkService.ShipUserListAsync(shipId);
        }

        /// <summary>
        /// 审批用户下拉列表
        /// </summary>
        /// <param name="shipId"></param>
        /// <returns></returns>
        [HttpGet("approveUserList")]
        public async Task<Result> CrewRotaApproveUserListAsync(Guid shipId)
        {
            return await _disembarkService.ApproveUserListAsync(shipId);
        }
    }
}