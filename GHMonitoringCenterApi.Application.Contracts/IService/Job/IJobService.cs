using GHMonitoringCenterApi.Application.Contracts.Dto.Job;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project.MonthReportForProject;
using GHMonitoringCenterApi.Domain.Shared;

namespace GHMonitoringCenterApi.Application.Contracts.IService.Job
{
    /// <summary>
    ///  任务中心业务层
    /// </summary>
    public interface IJobService
    {
        /// <summary>
        /// 提交任务
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> SubmitJobAsync<TBiz>(SubmitJobRequestDto<TBiz> model) where TBiz : class;

        /// <summary>
        /// 移除任务
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> ReomveJobAsync(ReomveJobRequestDto model);

        /// <summary>
        /// 获取任务列表
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<JobResponseDto>>> SearchJobsAsync(JobsRequestDto model);

        /// <summary>
        /// 审批任务
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> ApproveJobAsync(ApproveJobRequestDto model);

        /// <summary>
        /// 获取审批流程
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<JobApproveStepsResponseto>> SearchJobApproveStepsAsync(JobApproveStepsRequestDto model);

        /// <summary>
        /// 搜索任务创建审批记录
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<JobRecordsResponseDto>> SearchJobRecordsAsync(JobRecordsRequestDto model);

        /// <summary>
        /// 我提交的审批任务（草稿箱）
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<JobResponseDto>>> SearchSubmittedJobsAsync(SubmittedJobsRequestDto model);

        /// <summary>
        /// 搜索任务消息(包含:审核人未审核的任务条数)
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<JobNoticeResponseDto>> SearchJobNoticeAsync();

        /// <summary>
        /// 搜索任务的业务数据
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<JobBizResponseDto<TBizResult>>> SearchJobBizAsync<TBizResult>(JobBizRequestDto model) where TBizResult : class;

        /// <summary>
        /// 搜索任务的业务数据-版本2
        /// </summary>
        /// <returns></returns>
        //Task<ResponseAjaxResult<JobBizResponseDto<TBizResult>>> SearchJobBizV2Async<TBizResult>(JobBizRequestV2Dto model) where TBizResult : class;
        Task<ResponseAjaxResult<MonthReportForProjectResponseDto>> SearchMonthReportAsync(JobBizRequestV2Dto model);

        /// <summary>
        /// 搜索任务任务审批人列表
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<JobApproverResponseDto>>> SearchJobApproversAsync(JobApproverRequestDto model);

        /// <summary>
        /// 搜索下一级审批人集合
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<ResJobApproverDto>>> SearchJobNextApproversAsync(JobApproverRequestDto model);
    }
}
