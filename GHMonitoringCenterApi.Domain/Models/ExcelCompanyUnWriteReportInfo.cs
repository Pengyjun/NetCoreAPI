

using SqlSugar;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// Excel 项目生产数据存在不完整部分主要是以下项目未填报 
    /// </summary>
    [SugarTable("t_excelcompanyunwritereportinfo", IsDisabledDelete = true)]
    public class ExcelCompanyUnWriteReportInfo : BaseEntity<Guid>
    {
        ///<summary>
        /// 公司名称
        /// </summary>
        [SugarColumn(Length = 20)]
        public string UnitName { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        [SugarColumn(Length = 200)]
        public string ProjectName { get; set; }
        /// <summary>
        /// 未填报次数
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int Count { get; set; } = 0;
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
