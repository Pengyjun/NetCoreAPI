using SqlSugar;
namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// Excel各单位填报情况
    /// </summary>
    [SugarTable("t_excelcompanywritereportinfo", IsDisabledDelete = true)]
    public class ExcelCompanyWriteReportInfo : BaseEntity<Guid>
    {
        /// <summary>
        /// 公司名称
        /// </summary>
        [SugarColumn(Length = 50)]
        public string CompanyName { get; set; }
        /// <summary>
        /// 在建项目数量
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int OnBulidCount { get; set; } = 0;
        /// <summary>
        /// 未填报数量
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int UnReportCount { get; set; } = 0;
        /// <summary>
        /// 填报率
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,4)")]
        public decimal WritePercent { get; set; }
        /// <summary>
        /// 数据质量程度 几颗星
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int QualityLevel { get; set; } = 0;
        /// <summary>
        /// 单位排序
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int UnitDesc { get; set; }
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
