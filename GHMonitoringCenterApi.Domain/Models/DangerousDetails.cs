using SqlSugar;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 危大工程明细
    /// </summary>
    [SugarTable("t_dangerousdetails", IsDisabledDelete = true)]
    public class DangerousDetails : BaseEntity<Guid>
    {
        /// <summary>
        /// 项目id
        /// </summary>
        [SugarColumn(Length = 36)]
        public string? ProjectId { get; set; }
        /// <summary>
        /// 项目编码
        /// </summary>
        [SugarColumn(Length = 50)]
        public string? ProjectCode { get; set; }
        /// <summary>
        /// 危大工程项名称
        /// </summary>
        [SugarColumn(Length = 800)]
        public string? FangAnName { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        [SugarColumn(Length = 50)]
        public string? Status { get; set; }
        /// <summary>
        /// 方案类型
        /// </summary>
        [SugarColumn(Length = 50)]
        public string? FangAnKind { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(Length = 50)]
        public string? FangAnStatus { get; set; }
        /// <summary>
        /// 管理单位
        /// </summary>
        [SugarColumn(Length = 256)]
        public string ManageDepartment { get; set; } = string.Empty;
        /// <summary>
        /// 方案类型
        /// </summary>
        [SugarColumn(Length = 256)]
        public string FangAnType { get; set; } = string.Empty;
        /// <summary>
        /// 方案状态
        /// </summary>
        [SugarColumn(Length = 256)]
        public string? FangAnState { get; set; } = string.Empty;
        /// <summary>
        /// 方案编制时间
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime? CreatedTime { get; set; }
        /// <summary>
        /// 方案审批完成时间
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime? FinishTime { get; set; }
        /// <summary>
        /// 方案开工时间
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 方案完工时间
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 是否异常
        /// </summary>
        [SugarColumn(Length = 256)]
        public string ErrorOrNot { get; set; } = string.Empty;
        /// <summary>
        /// 方案附件ID
        /// </summary>
        [SugarColumn(Length = 800)]
        public string? FangAnFileObjectID { get; set; }
        /// <summary>
        /// 专家意见附件ID
        /// </summary>
        [SugarColumn(Length = 800)]
        public string? ExpertOpinionFileObjectID { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        [SugarColumn(Length = 256)]
        public string ApprovalState { get; set; } = string.Empty;
    }
}
