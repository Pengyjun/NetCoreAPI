using SqlSugar;
namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// Excel船舶产值
    /// </summary>
    [SugarTable("t_excelshipproductionvalue", IsDisabledDelete = true)]
    public class ExcelShipProductionValue : BaseEntity<Guid>
    {
        /// <summary>
        /// 船舶名称
        /// </summary>
        [SugarColumn(Length = 50)]
        public string ShipName { get; set; }
        /// <summary>
        /// 船舶当日产值
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,4)")]
        public decimal ShipDayOutput { get; set; }
        /// <summary>
        /// 船舶年累产值
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,4)")]
        public decimal ShipYearOutput { get; set; }
        /// <summary>
        /// 时间利用率
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,4)")]
        public decimal TimePercent { get; set; }
        /// <summary>
        /// 年份
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int Year { get; set; }
        /// <summary>
        /// 月份
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int Month { get; set; }
        /// <summary>
        /// 天
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int DateDay { get; set; }
    }
}
