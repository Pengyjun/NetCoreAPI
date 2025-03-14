using GHMonitoringCenterApi.Domain.Enums;
using GHMonitoringCenterApi.Domain.Shared.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.BaseLinePlan
{
    /// <summary>
    /// 返回基准计划审批人Dto
    /// </summary>
    public  class BaseLinePlanApproverResponseDto : BaseLinePlanApproverDto
    {
        /// <summary>
        /// 基准计划名称
        /// </summary>
        public string? BaseLinePlanName { get; set; }

        /// <summary>
        /// 审批人名称
        /// </summary>
        public string? ApproverName { get; set; }

        /// <summary>
        /// 审批人电话号码
        /// </summary>
        public string? ApproverPhone { get; set; }

        /// <summary>
        /// 审批层级名称
        /// </summary>
        public string ApproveLevelName { get { return ApproveLevel.ToDescription(); } }

        /// <summary>
        /// 是否是默认审批人
        /// </summary>
        public bool IsDefaultApprover { get; set; }
    }
}
