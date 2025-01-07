using SqlSugar;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Timing
{
    /// <summary>
    /// 接收京威危大工程方案清单
    /// </summary>
    public class SpecialConstructionResponse
    {
        public string? Code { get; set; }
        public Datas? Data { get; set; }
    }

    public class Datas
    {
        public RspHeader? RspHeader { get; set; }
        public RspBody? RspBody { get; set; }
    }
    public class RspHeader
    {
        public string Status { get; set; }
        public string Msg { get; set; }
    }
    public class RspBody
    {
        public string AllRowCount { get; set; }
        public string AllPageCount { get; set; }
        public List<ListResult>? List { get; set; }
    }

    public class ListResult
    {
        /// <summary>
        /// 项目id
        /// </summary>
        public string? ProjectId { get; set; }
        /// <summary>
        /// 项目编码
        /// </summary>
        public string? ProjectCode { get; set; }
        /// <summary>
        /// 危大工程项名称
        /// </summary>
        public string? FangAnName { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string? Status { get; set; }
        /// <summary>
        /// 外部系统方案主键
        /// </summary>
        public string? SystemId { get; set; }
        /// <summary>
        /// 方案类型
        /// </summary>
        public string? FangAnKind { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? FangAnStatus { get; set; }
        /// <summary>
        /// 管理单位
        /// </summary>
        public string ManageDepartment { get; set; } = string.Empty;
        /// <summary>
        /// 方案类型
        /// </summary>
        public string FangAnType { get; set; } = string.Empty;
        /// <summary>
        /// 方案状态
        /// </summary>
        public string? FangAnState { get; set; } = string.Empty;
        /// <summary>
        /// 方案编制时间
        /// </summary>
        public DateTime? CreatedTime { get; set; }
        /// <summary>
        /// 方案审批完成时间
        /// </summary>
        public DateTime? FinishTime { get; set; }
        /// <summary>
        /// 方案开工时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 方案完工时间
        /// </summary>
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
