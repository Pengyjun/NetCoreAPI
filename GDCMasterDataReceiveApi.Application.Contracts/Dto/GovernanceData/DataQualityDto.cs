using GDCMasterDataReceiveApi.Domain.Shared.Enums;

namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.GovernanceData
{

    /// <summary>
    /// 数据质量dto
    /// </summary>
    public class DataQualityDto
    {

    }
    /// <summary>
    /// 
    /// </summary>
    public class DataQualityResponseDto
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
        /// 小时数
        /// </summary>
        public int Hour { get; set; }
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
        /// 表名
        /// </summary>
        public string? TableName { get; set; }
        /// <summary>
        /// 检查字段
        /// </summary>
        public string? Column { get; set; }
        /// <summary>
        /// 列名
        /// </summary>
        public string? ColumnName { get; set; }
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
    /// <summary>
    /// 
    /// </summary>
    public class DataQualityRequestDto : BaseRequestDto
    {
        /// <summary>
        /// 1完整性 2唯一性 3有效性
        /// </summary>
        public int Type { get; set; }
    }
    /// <summary>
    /// 保存规则配置
    /// </summary>
    public class DataQualityDtoConvert
    {
        /// <summary>
        /// 
        /// </summary>
        public string? Id { set; get; }
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
        /// 表名
        /// </summary>
        public string? TableName { get; set; }
        /// <summary>
        /// 检查字段
        /// </summary>
        public string? Column { get; set; }
        /// <summary>
        /// 列名
        /// </summary>
        public string? ColumnName { get; set; }
        /// <summary>
        /// 规则级别
        /// </summary>
        public GruleGradeType Grade { get; set; }
        /// <summary>
        /// 状态 1启用
        /// </summary>
        public string? Status { get; set; }
        /// <summary>
        /// 小时数（及时性）
        /// </summary>
        public int Hour { get; set; }
    }
    /// <summary>
    /// 保存数据规则配置
    /// </summary>
    public class SaveDataQualityDto
    {
        /// <summary>
        /// 1增2改3删
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DataQualityDtoConvert DQ { get; set; }
    }
}
