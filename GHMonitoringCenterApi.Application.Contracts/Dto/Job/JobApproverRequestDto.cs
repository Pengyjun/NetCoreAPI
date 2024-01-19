using GHMonitoringCenterApi.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Job
{
    /// <summary>
    /// 任务审批人请求Dto
    /// </summary>
    public class JobApproverRequestDto
    {
        /// <summary>
        /// 任务Id
        /// </summary>
        public Guid? JobId { get; set; }

        /// <summary>
        /// 项目Id
        /// </summary>
        public Guid ProjectId { get; set; }

        /// <summary>
        /// 业务模块
        /// </summary>
        public BizModule BizModule { get; set; }
    }
}
