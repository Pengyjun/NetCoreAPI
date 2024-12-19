using SqlSugar;

namespace HNKC.CrewManagePlatform.SqlSugars.Models
{
    /// <summary>
    /// 航区
    /// </summary>
    [SugarTable("t_navigationarea", IsDisabledDelete = true, TableDescription = "航区")]
    public class NavigationArea : BaseEntity<long>
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
