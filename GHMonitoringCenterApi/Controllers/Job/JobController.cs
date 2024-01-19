using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GHMonitoringCenterApi.Application.Contracts.IService.Job;
using GHMonitoringCenterApi.Domain.Shared;
using GHMonitoringCenterApi.Application.Contracts.Dto.Job;
using GHMonitoringCenterApi.CustomAttribute;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report;
using GHMonitoringCenterApi.Domain.Enums;

namespace GHMonitoringCenterApi.Controllers.Job
{
    /// <summary>
    /// 任务中心
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class JobController : BaseController
    {

        #region 依赖注入

        /// <summary>
        /// 任务中心业务层
        /// </summary>
        private readonly IJobService _jobService;

        public JobController(IJobService jobService)
        {
            _jobService = jobService;
        }

        #endregion


        /// <summary>
        /// 搜索项目结构树业务数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchJobBizOfProjectWBS")]
        public async Task<ResponseAjaxResult<JobBizResponseDto<ProjectWBSComposeResponseDto>>> SearchJobBizOfProjectWBSAsync([FromQuery] JobBizOfProjectWBSRequsetDto model)
        {
            return await _jobService.SearchJobBizAsync<ProjectWBSComposeResponseDto>(model);
        }

        /// <summary>
        /// 提交任务-项目结构变化
        /// </summary>
        /// <returns></returns>
        [HttpPost("SubmitJobOfProjectWBS")]
        [UnitOfWork]
        public async Task<ResponseAjaxResult<bool>> SubmitJobOfProjectWBSAsync([FromBody] SubmitJobOfProjectWBSRequestDto model)
        {
            return await _jobService.SubmitJobAsync(model);
        }

        /// <summary>
        /// 搜索项目月报
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchJobBizOfMonthReport")]
        public async Task<ResponseAjaxResult<JobBizResponseDto<ProjectMonthReportResponseDto>>> SearchJobBizOfMonthReportAsync([FromQuery] JobBizRequestV2Dto model)
        {
            model.BizModule = BizModule.MonthReport;
            return await _jobService.SearchJobBizV2Async<ProjectMonthReportResponseDto>(model);
        }

        /// <summary>
        /// 提交任务-项目月报
        /// </summary>
        /// <returns></returns>
        [HttpPost("SubmitJobOfMonthReport")]
        [UnitOfWork]
        public async Task<ResponseAjaxResult<bool>> SubmitJobOfMonthReportAsync([FromBody] SubmitJobOfMonthReportRequestDto model)
        {
            return await _jobService.SubmitJobAsync(model);
        }

        /// <summary>
        /// 移除任务
        /// </summary>
        /// <returns></returns>
        [HttpPost("ReomveJob")]
        public async Task<ResponseAjaxResult<bool>> ReomveJobAsync([FromBody] ReomveJobRequestDto model)
        {
            return await _jobService.ReomveJobAsync(model);
        }

        /// <summary>
        /// 搜索任务列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchJobs")]
        public async Task<ResponseAjaxResult<List<JobResponseDto>>> SearchJobsAsync([FromQuery] JobsRequestDto model)
        {
            return await _jobService.SearchJobsAsync(model);
        }

        /// <summary>
        /// 审批任务
        /// </summary>
        /// <returns></returns>
        [HttpPost("ApproveJob")]
        [UnitOfWork]
        public async Task<ResponseAjaxResult<bool>> ApproveJobAsync([FromBody] ApproveJobRequestDto model)
        {
            return await _jobService.ApproveJobAsync(model);
        }

        /// <summary>
        /// 审批任务-项目月报
        /// </summary>
        /// <returns></returns>
        [HttpPost("ApproveJobOfMonthReport")]
        [UnitOfWork]
        public async Task<ResponseAjaxResult<bool>> ApproveJobOfMonthReportAsync([FromBody] ApproveJobOfMonthReportRequestDto model)
        {
            return await _jobService.ApproveJobAsync(model);
        }

        /// <summary>
        /// 搜索审批流程
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchJobApproveSteps")]
        public async Task<ResponseAjaxResult<JobApproveStepsResponseto>> SearchJobApproveStepsAsync([FromQuery] JobApproveStepsRequestDto model)
        {
            return await _jobService.SearchJobApproveStepsAsync(model);
        }

        /// <summary>
        /// 搜索任务创建审批记录
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchJobRecords")]
        public async Task<ResponseAjaxResult<JobRecordsResponseDto>> SearchJobRecordsAsync([FromQuery] JobRecordsRequestDto model)
        {
            return await _jobService.SearchJobRecordsAsync(model);
        }

        /// <summary>
        ///  搜索我提交的审批任务（草稿箱）
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchSubmittedJobs")]
        public async Task<ResponseAjaxResult<List<JobResponseDto>>> SearchSubmittedJobsAsync([FromQuery] SubmittedJobsRequestDto model)
        {
            return await _jobService.SearchSubmittedJobsAsync(model);
        }

        /// <summary>
        /// 搜索任务消息(包含:审核人未审核的任务条数) 
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchJobNotice")]
        public async Task<ResponseAjaxResult<JobNoticeResponseDto>> SearchJobNoticeAsync()
        {
             return await _jobService.SearchJobNoticeAsync();
        }

        /// <summary>
        /// 搜索任务任务审批人列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchJobApprovers")]
        public async Task<ResponseAjaxResult<List<JobApproverResponseDto>>> SearchJobApproversAsync([FromQuery]JobApproverRequestDto model)
        {
            return await _jobService.SearchJobApproversAsync(model);
        }

        /// <summary>
        /// 搜索下一级审批人集合
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchJobNextApprovers")]
        public async Task<ResponseAjaxResult<List<ResJobApproverDto>>> SearchJobNextApproversAsync([FromQuery] JobApproverRequestDto model)
        {
            return await _jobService.SearchJobNextApproversAsync(model);
        }


    }
}
