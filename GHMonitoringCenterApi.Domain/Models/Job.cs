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
    /// 任务
    /// </summary>
    [SugarTable("t_job", IsDisabledDelete = true)]
   
    public class Job : BaseEntity<Guid>
    {
        /// <summary>
        /// 项目Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid ProjectId { get; set; }

        /// <summary>
        /// （当前）审批状态  1：驳回，2：通过
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public JobApproveStatus ApproveStatus { get; set; }

        /// <summary>
        /// （当前）审批人
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? ApproverId { get; set; }

        /// <summary>
        /// （当前）审批层级（1：一级审批）
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public ApproveLevel ApproveLevel { get; set; }

        /// <summary>
        /// （当前）审批时间
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime? ApproveTime { get; set; }

        /// <summary>
        /// （当前）驳回原因
        /// </summary>
        [SugarColumn(Length = 500)]
        public string? RejectReason { get; set; }

        /// <summary>
        /// 业务模块 1:项目结构
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public BizModule BizModule { get; set; }

        /// <summary>
        /// 业务数据
        /// </summary>
        [SugarColumn(ColumnDataType = "longtext")]
        public string? BizData { get; set; }

        /// <summary>
        /// 业务结果编码
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int ? BizHandleCode { get; set; }

        /// <summary>
        /// 业务处理信息
        /// </summary>
        [SugarColumn(ColumnDataType = "text")]
        public string? BizHandleMessage { get; set; }

        /// <summary>
        /// 任务是否结束
        /// </summary>
        [SugarColumn(ColumnDataType = "bit")]
        public bool IsFinish { get; set; }

        /// <summary>
        /// 结束审批层级
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public ApproveLevel FinishApproveLevel { get; set; }

        /// <summary>
        /// 月份
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int DateMonth { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int DateDay { get; set; }

        /// <summary>
        /// 是否越级审批
        /// </summary>
        [SugarColumn(ColumnDataType = "bit")]
        public bool IsPassLevelApprove{ get; set; }

        /// <summary>
        /// 提交时间
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime SubmitTime { get; set; }

    }
}
