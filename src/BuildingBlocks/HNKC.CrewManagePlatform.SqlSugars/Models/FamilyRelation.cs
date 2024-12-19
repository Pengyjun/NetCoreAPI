using SqlSugar;

namespace HNKC.CrewManagePlatform.SqlSugars.Models
{
    /// <summary>
    /// 家庭关系
    /// </summary>
    [SugarTable("t_familyrelation", IsDisabledDelete = true, TableDescription = "家庭关系")]
    public class FamilyRelation : BaseEntity<long>
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
