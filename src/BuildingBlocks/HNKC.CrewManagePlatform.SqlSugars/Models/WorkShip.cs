using SqlSugar;

namespace HNKC.CrewManagePlatform.SqlSugars.Models
{
    /// <summary>
    /// 任职船舶
    /// </summary>
    [SugarTable("t_workship", IsDisabledDelete = true, TableDescription = "任职船舶")]
    public class WorkShip : BaseEntity<long>
    {
        /// <summary>
        /// 所在船舶
        /// </summary>
        [SugarColumn(Length = 50, ColumnDescription = "所在船舶")]
        public string? OnShip { get; set; }
        /// <summary>
        /// 在船职务
        /// </summary>
        [SugarColumn(Length = 100, ColumnDescription = "在船职务")]
        public string? Postition { get; set; }
        /// <summary>
        /// 上船日期
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime", ColumnDescription = "上船日期")]
        public DateTime WorkShipStartTime { get; set; }
        /// <summary>
        /// 下船日期
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime", ColumnDescription = "下船日期")]
        public DateTime? WorkShipEndTime { get; set; }
        /// <summary>
        /// 所在项目
        /// </summary>
        [SugarColumn(Length = 256, ColumnDescription = "所在项目")]
        public string? ProjectName { get; set; }
        /// <summary>
        /// 所在国家
        /// </summary>
        [SugarColumn(Length = 36, ColumnDescription = "所在国家")]
        public Guid? Country { get; set; }
        /// <summary>
        /// 关联键
        /// </summary>
        [SugarColumn(Length = 36, ColumnDescription = "关联键")]
        public Guid? WorkShipId { get; set; }
    }
}
