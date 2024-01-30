using SqlSugar;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// Excel各个公司基本产值情况
    /// </summary>
    [SugarTable("t_excelcompanybasepoductionvalue", IsDisabledDelete = true)]
    public class ExcelCompanyBasePoductionValue : BaseEntity<Guid>
    {
        /// <summary>
        /// 业务单位
        /// </summary>
        [SugarColumn(Length = 50)]
        public string UnitName { get; set; }
        /// <summary>
        /// 当日产值 万元
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,4)")]
        public decimal DayOutputVal { get; set; }
        /// <summary>
        /// 年累产值
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,4)")]
        public decimal YearProjectAccumulateOutputVal { get; set; }
        /// <summary>
        /// 年度产值占比 占广航局
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,4)")]
        public decimal YearAccumulateOutputValProp { get; set; }
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
