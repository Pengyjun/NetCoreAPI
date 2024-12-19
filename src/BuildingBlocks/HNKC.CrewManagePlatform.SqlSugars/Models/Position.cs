using SqlSugar;

namespace HNKC.CrewManagePlatform.SqlSugars.Models
{
    /// <summary>
    /// 适任职务
    /// </summary>
    [SugarTable("t_position", IsDisabledDelete = true, TableDescription = "适任职务")]
    public class Position : BaseEntity<long>
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
    }
}
