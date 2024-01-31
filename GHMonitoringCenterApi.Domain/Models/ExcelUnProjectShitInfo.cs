using SqlSugar;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// Excel 项目带班生产动态（未填报）
    /// </summary>
    [SugarTable("t_excelunprojectshitinfo", IsDisabledDelete = true)]
    public class ExcelUnProjectShitInfo : BaseEntity<Guid>
    {
        /// <summary>
		/// 公司名称
		/// </summary>
        [SugarColumn(Length = 50)]

        public string UnitName { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        [SugarColumn(Length = 200)]
        public string ProjectName { get; set; }
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
