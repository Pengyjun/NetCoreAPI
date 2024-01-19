using GHMonitoringCenterApi.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Job
{
    /// <summary>
    /// 任务审批人Dto
    /// </summary>
    public abstract  class JobApproverDto
    {
        /// <summary>
        /// 审批人Id
        /// </summary>
        public Guid ApproverId { get; set; }

        /// <summary>
        /// 审批层级
        /// </summary>
        public ApproveLevel ApproveLevel { get; set; }
    }
}
