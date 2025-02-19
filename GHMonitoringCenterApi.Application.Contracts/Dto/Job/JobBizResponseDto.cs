using GHMonitoringCenterApi.Domain.Enums;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Job
{
    /// <summary>
    /// 任务业务返回
    /// </summary>
    public class JobBizResponseDto<TBizResult> where TBizResult : class
    {
        /// <summary>
        /// 任务Id
        /// </summary>
        public Guid? JobId { get; set; }

        /// <summary>
        /// 项目Id
        /// </summary>
        public Guid? ProjectId { get; set; }

        /// <summary>
        /// 业务模块
        /// </summary>
        public BizModule BizModule { get; set; }

        /// <summary>
        /// 业务数据对象
        /// </summary>
        public TBizResult? BizData { get; set; }

        /// <summary>
        /// 审批人集合
        /// </summary>
        public ResJobApproverDto[] Approvers { get; set; } = new ResJobApproverDto[0];

        /// <summary>
        /// 下一级审批人集合
        /// </summary>
        public ResJobApproverDto[] NextApprovers { get; set; } = new ResJobApproverDto[0];

    }
}
