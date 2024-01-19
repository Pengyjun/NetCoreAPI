using GHMonitoringCenterApi.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Job
{
    /// <summary>
    /// 任务审批记录返回
    /// </summary>
    public class JobRecordsResponseDto
    {
        /// <summary>
        /// 审批记录集合
        /// </summary>
        public ResJobRecord[]? Records { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public class ResJobRecord
        {
            /// <summary>
            /// 操作人
            /// </summary>
           public string? OperatorName { get; set; }

            /// <summary>
            /// 操作内容
            /// </summary>
            public string? OperateContent { get; set; }

            /// <summary>
            /// 审批状态 0：无状态 1：驳回，2：通过
            /// </summary>
            public JobApproveStatus ApproveStatus { get; set; }

            /// <summary>
            /// 驳回原因
            /// </summary>
            public string? RejectReason { get; set; }

            /// <summary>
            /// 操作时间
            /// </summary>
            public DateTime? OperateTime { get; set; }
        }
    }
}
