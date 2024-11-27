using GDCMasterDataReceiveApi.Domain.Shared.Enums;
using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 数据规则配置表
    /// </summary>
    [SugarTable("t_datagrulesetting", IsDisabledDelete = true)]
    public class DataGruleSetting : BaseEntity<long>
    {
        /// <summary>
        /// 规则名称
        /// </summary>
        [SugarColumn(Length = 200)]
        public string? Name { get; set; }
        /// <summary>
        /// 规则类型
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public GruleType Type { get; set; }
        /// <summary>
        /// 数据源
        /// </summary>
        [SugarColumn(Length = 200)]
        public string? Soure { get; set; }
        /// <summary>
        /// 数据表
        /// </summary>
        [SugarColumn(Length = 200)]
        public string? Table { get; set; }
        /// <summary>
        /// 表名称
        /// </summary>
        [SugarColumn(Length = 200)]
        public string? TableName { get; set; }
        /// <summary>
        /// 检查字段
        /// </summary>
        [SugarColumn(Length = 200)]
        public string? Column { get; set; }
        /// <summary>
        /// 检查字段名称
        /// </summary>
        [SugarColumn(Length = 200)]
        public string? ColumnName { get; set; }
        /// <summary>
        /// 规则级别
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public GruleGradeType Grade { get; set; }
        /// <summary>
        /// 状态 1启用
        /// </summary>
        [SugarColumn(Length = 200)]
        public string? Status { get; set; }
    }
}
