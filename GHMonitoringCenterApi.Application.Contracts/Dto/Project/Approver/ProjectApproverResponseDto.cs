using GHMonitoringCenterApi.Domain.Enums;
using GHMonitoringCenterApi.Domain.Shared.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project.Approver
{
    /// <summary>
    /// 返回项目审批人Dto
    /// </summary>
    public  class ProjectApproverResponseDto:ProjectApproverDto
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        public string? ProjectName { get; set; }

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
