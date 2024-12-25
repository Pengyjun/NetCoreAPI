using SqlSugar;

namespace HNKC.CrewManagePlatform.SqlSugars.Models
{
    /// <summary>
    /// 在船职务
    /// </summary>
    [SugarTable("t_positiononboard", IsDisabledDelete = true, TableDescription = "在船职务")]
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
        /// 关联键
        /// </summary>
        [SugarColumn(Length = 36, ColumnDescription = "关联键")]
        public string? ForeignKey { get; set; }
    }
}
