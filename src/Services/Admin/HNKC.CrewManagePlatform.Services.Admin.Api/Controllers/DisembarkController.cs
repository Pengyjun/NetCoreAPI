using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Dtos.Disembark;
using HNKC.CrewManagePlatform.Services.Interface.Disembark;
using HNKC.CrewManagePlatform.SqlSugars.UnitOfTransaction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

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
            query.type = 0;
            var data = await _disembarkService.DepartureApplyListAsync(query);
            return Ok(data);
        }

        /// <summary>
        /// 获取离船审批列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("ApproveList")]
        public async Task<IActionResult> DepartureApproveListAsync([FromQuery] DepartureApplyQuery query)
        {
            query.type = 1;
            var data = await _disembarkService.DepartureApplyListAsync(query);
            return Ok(data);
        }

        /// <summary>
        /// 查询离船申请单详情
        /// </summary>
        /// <param name="applyCode"></param>
        /// <returns></returns>
        [HttpGet("DepartureApplyDetail")]
        public async Task<IActionResult> DepartureApplyDetail(Guid applyCode)
        {
            var data = await _disembarkService.DepartureApplyDetail(applyCode);
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


        /// <summary>
        /// 年休计划  获取船舶人员信息
        /// </summary>
        /// <param name="ShipId"></param>
        /// <returns></returns>
        [HttpGet("SearchLeavePlanUser")]
        public async Task<Result> SearchLeavePlanUserAsync([FromQuery] SearchLeavePlanUserRequestDto requestDto)
        {
            return await _disembarkService.SearchLeavePlanUserAsync(requestDto);
        }

        /// <summary>
        /// 新增或修改年休计划
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("SaveLeavePlanUserVacation")]
        [Transactional]
        public async Task<Result> SaveLeavePlanUserVacationAsync([FromBody] AddLeavePlanVacationRequestDto requestDto)
        {
            return await _disembarkService.SaveLeavePlanUserVacationAsync(requestDto);
        }

        /// <summary>
        /// 获取休假日期详情
        /// </summary>
        /// <returns></returns>
        [HttpPost("SearchLeaveDetail")]
        public async Task<Result> SearchLeaveDetailAsync([FromBody] SearchLeavePlanUserRequestDto requestDto)
        {
            return await _disembarkService.SearchLeaveDetailAsync(requestDto);
        }

        /// <summary>
        /// 年休假规则验证
        /// </summary>
        /// <returns></returns>
        [HttpPost("LeaveCheckRule")]
        public async Task<Result> LeaveCheckRuleAsync([FromBody] LeaveCheckRuleRequestDto requestDto)
        {
            return await _disembarkService.LeaveCheckRuleAsync(requestDto);
        }
        /// <summary>
        /// 获取适任证书下拉
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchCertificateSelect")]
        public async Task<Result> SearchCertificateSelectAsync()
        {
            return await _disembarkService.SearchCertificateSelectAsync();
        }

        /// <summary>
        /// 获取船舶定员标准
        /// </summary>
        /// <returns></returns>
        [HttpPost("SearchShipPersonStandard")]
        public async Task<Result> SearchShipPersonStandardAsync([FromBody] ShipPersonRequestDto requestDto)
        {
            return await _disembarkService.SearchShipPersonStandardAsync(requestDto);
        }
    }
}