using SqlSugar;
namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 项目产值强度表格
    /// </summary>
    [SugarTable("t_excelprojectintensity", IsDisabledDelete = true)]
    public class ExcelProjectIntensity : BaseEntity<Guid>
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        [SugarColumn(Length = 300)]
        public string Name { get; set; }
        /// <summary>
        /// 当月日均计划
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal? PlanDayProduciton { get; set; }
        /// <summary>
        /// 当日实际产值
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal? DayProduciton { get; set; }
        /// <summary>
        /// 当日产值完成率 
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal? CompleteDayProducitonRate { get; set; }
        /// <summary>
        /// 当日产值强度较低原因
        /// </summary>
        [SugarColumn(Length = 300)]
        public string? DayProductionIntensityDesc { get; set; }
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
