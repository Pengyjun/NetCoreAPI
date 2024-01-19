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
    /// 任务审批人
    /// </summary>
    [SugarTable("t_jobapprover", IsDisabledDelete = true)]
    [SugarIndex("INDEX_JA_UQ_KEY", nameof(JobId), OrderByType.Asc,nameof(ApproverId), OrderByType.Asc, nameof(ApproveLevel), OrderByType.Asc)]
    public class JobApprover : BaseEntity<Guid>
    {
        /// <summary>
        /// 任务Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid JobId { get; set; }

        /// <summary>
        /// 审批人Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid ApproverId { get; set; }

        /// <summary>
        /// 审批层级（1：一级审批）
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public ApproveLevel ApproveLevel { get; set; }

        /// <summary>
        /// 审核人任务状态 1:待办，2：已办
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public JobStatus ApproverJobStatus { get; set; }

        /// <summary>
        ///  审批状态  1：驳回，2：通过
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public JobApproveStatus ApproveStatus { get; set; }

        /// <summary>
        /// 审批时间
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime? ApproveTime { get; set; } 
    }
}
