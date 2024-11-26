using GDCMasterDataReceiveApi.Domain.Shared.Enums;

namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.GovernanceData
{
    /// <summary>
    /// 数据质量报告dto
    /// </summary>
    public class DataReportDto
    {
    }
    /// <summary>
    /// 数据质量报告
    /// </summary>
    public class DataReportRequestDto : BaseRequestDto
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string? Table { get; set; }
    }
    /// <summary>
    /// ///数据质量响应体
    /// </summary>
    public class DataReportResponseDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string? Id { get; set; }
        /// <summary>
        /// 规则名称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 规则类型
        /// </summary>
        public GruleType Type { get; set; }
        /// <summary>
        /// 数据源
        /// </summary>
        public string? Soure { get; set; }
        /// <summary>
        /// 数据表
        /// </summary>
        public string? Table { get; set; }
        /// <summary>
        /// 检查字段
        /// </summary>
        public string? Column { get; set; }
        /// <summary>
        /// 规则级别
        /// </summary>
        public GruleGradeType Grade { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string? Status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? CreateTime { get; set; }
    }
}
