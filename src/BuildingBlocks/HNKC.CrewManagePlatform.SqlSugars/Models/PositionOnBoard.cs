using HNKC.CrewManagePlatform.Models.Enums;
using SqlSugar;

namespace HNKC.CrewManagePlatform.SqlSugars.Models
{
    /// <summary>
    /// 在船职务
    /// </summary>
    [SugarTable("t_positionboard", IsDisabledDelete = true, TableDescription = "在船职务")]
    public class PositionOnBoard : BaseEntity<long>
    {
        /// <summary>
        /// 编码 
        /// </summary>
        [SugarColumn(Length = 50, ColumnDescription = "编码")]
        public string? Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [SugarColumn(Length = 50, ColumnDescription = "名称")]
        public string? Name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(Length = 50, ColumnDescription = "备注")]
        public string? Remark { get; set; }
        /// <summary>
        /// 船员类型
        /// </summary>
        [SugarColumn(ColumnDataType = "int", ColumnDescription = "船员类型", DefaultValue = "0")]
        public int Type { get; set; }
        /// <summary>
        /// 部门类型
        /// </summary>
        [SugarColumn(ColumnDataType = "int", ColumnDescription = "部门类型", DefaultValue = "0")]
        public RotaEnum RotaType { get; set; }
    }
}
