using SqlSugar;

namespace HNKC.CrewManagePlatform.SqlSugars.Models
{
    /// <summary>
    /// 船舶项目关联
    /// </summary>
    [SugarTable("t_shipprojectrelation", IsDisabledDelete = true, TableDescription = "船舶项目关联")]
    public class ShipProjectRelation : BaseEntity<long>
    {
        /// <summary>
        /// 船舶业务关联键
        /// </summary>
        [SugarColumn(Length = 36, ColumnDescription = "船舶业务关联键")]
        public Guid? RelationShipId { get; set; }
        /// <summary>
        /// 项目
        /// </summary>
        [SugarColumn(Length = 200, ColumnDescription = "项目")]
        public string? ProjectName { get; set; }

    }
}
