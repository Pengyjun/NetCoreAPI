using HNKC.CrewManagePlatform.Models.Enums;
using SqlSugar;

namespace HNKC.CrewManagePlatform.SqlSugars.Models
{
    /// <summary>
    /// 自有船舶
    /// </summary>
    [SugarTable("t_ownership", IsDisabledDelete = true, TableDescription = "自有船舶")]
    public class OwnerShip : BaseEntity<long>
    {
        /// <summary>
        /// 船舶名称
        /// </summary>
        [SugarColumn(Length = 200, ColumnDescription = "船舶名称")]
        public string? ShipName { get; set; }
        /// <summary>
        /// 所属国家
        /// </summary>
        [SugarColumn(Length = 200, ColumnDescription = "所属国家")]
        public string? Country { get; set; }
        /// <summary>
        /// 所属公司
        /// </summary>
        [SugarColumn(Length = 200, ColumnDescription = "所属公司")]
        public string? Company { get; set; }
        /// <summary>
        /// 船舶类型
        /// </summary>
        [SugarColumn(ColumnDataType = "int", ColumnDescription = "船舶类型", DefaultValue = "0")]
        public ShipTypeEnum ShipType { get; set; }
    }
}
