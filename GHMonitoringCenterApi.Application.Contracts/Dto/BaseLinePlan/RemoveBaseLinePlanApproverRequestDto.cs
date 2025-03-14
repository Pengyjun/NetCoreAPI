using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.BaseLinePlan
{
    /// <summary>
    /// 移除项目审批人请求
    /// </summary>
    public class RemoveBaseLinePlanApproverRequestDto
    {
        /// <summary>
        /// 基准计划审批人Id
        /// </summary>
        public Guid BaseLinePlanApproverId { get; set; }
    }
}
