using SqlSugar;
namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 项目产值完成排名
    /// </summary>
    [SugarTable("t_excelprojectrank", IsDisabledDelete = true)]
    public class ExcelProjectRank : BaseEntity<Guid>
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        [SugarColumn(Length = 300)]
        public string ProjectName { get; set; }
        /// <summary>
        ///当日产值
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal DayActualValue { get; set; }
        /// <summary>
        /// 当年计划产值
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal CurrentYearPlanProductionValue { get; set; }
        /// <summary>
        /// 当年完成产值
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal CurrentYearCompleteProductionValue { get; set; }
        /// <summary>
        /// 完成率
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal CompleteRate { get; set; }
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
