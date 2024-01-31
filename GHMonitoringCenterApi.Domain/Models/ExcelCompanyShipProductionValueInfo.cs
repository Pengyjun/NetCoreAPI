using SqlSugar;
namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// Excel各个公司自有船施工产值情况
    /// </summary>
    [SugarTable("t_excelcompanyshipproductionvalueinfo", IsDisabledDelete = true)]
    public class ExcelCompanyShipProductionValueInfo : BaseEntity<Guid>
    {
        /// <summary>
        /// 船舶类型
        /// </summary>
        [SugarColumn(Length = 50)]
        public string ShipTypeName { get; set; }
        /// <summary>
        /// 当日产值
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,4)")]
        public decimal DayProductionValue { get; set; }
        /// <summary>
        /// 当日运转小时
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,4)")]
        public decimal DayTurnHours { get; set; }
        /// <summary>
        /// 当年累计产值
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,4)")]
        public decimal YearTotalProductionValue { get; set; }
        /// <summary>
        /// 当年累计运转小时
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,4)")]
        public decimal YearTotalTurnHours { get; set; }
        /// <summary>
        /// 时间利用率
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,4)")]
        public decimal TimePercent { get; set; }
        /// <summary>
        /// 单位排序
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int ShipTypeDesc { get; set; }
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
