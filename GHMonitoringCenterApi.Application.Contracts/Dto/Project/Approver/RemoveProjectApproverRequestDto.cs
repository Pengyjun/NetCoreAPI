using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project.Approver
{
    /// <summary>
    /// 移除项目审批人请求
    /// </summary>
    public  class RemoveProjectApproverRequestDto
    {
        /// <summary>
        /// 项目审批人Id
        /// </summary>
        public Guid ProjectApproverId { get; set; }
    }
}
