using GHMonitoringCenterApi.Domain.Enums;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{

    /// <summary>
    /// 任务记录
    /// </summary>
    [SugarTable("t_jobrecord", IsDisabledDelete = true)]
    public class JobRecord : BaseEntity<Guid>
    {
        /// <summary>
        /// 任务Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid JobId { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime OperateTime { get; set; }

        /// <summary>
        /// 操作人Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid OperatorId { get; set; }

        /// <summary>
        /// 操作人名称
        /// </summary>
        [SugarColumn(Length = 200)]
        public string? OperatorName { get; set; }

        /// <summary>
        /// 操作内容
        /// </summary>
        [SugarColumn(Length = 200)]
        public string? OperateContent { get; set; }

        /// <summary>
        /// 审批状态 1：驳回，2：通过
        /// </summary>
        public JobApproveStatus ApproveStatus { get; set; }

        /// <summary>
        /// 驳回原因
        /// </summary>
        [SugarColumn(Length = 500)]
        public string? RejectReason { get; set; }
    }
}
