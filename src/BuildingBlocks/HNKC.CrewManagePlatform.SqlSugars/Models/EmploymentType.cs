using SqlSugar;

namespace HNKC.CrewManagePlatform.SqlSugars.Models
{
    /// <summary>
    /// 用工类型
    /// </summary>
    [SugarTable("t_employmenttype", IsDisabledDelete = true, TableDescription = "用工类型")]
    public class EmploymentType : BaseEntity<long>
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
