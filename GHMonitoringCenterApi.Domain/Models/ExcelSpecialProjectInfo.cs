using SqlSugar;
namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// Excel 特殊情况
    /// </summary>
    [SugarTable("t_excelspecialprojectinfo", IsDisabledDelete = true)]
    public class ExcelSpecialProjectInfo : BaseEntity<Guid>
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        [SugarColumn(Length = 200)]
        public string? SourceMatter { get; set; }
        /// <summary>
        /// 类型  1异常预警 2 嘉奖通报 3提醒事项
        /// </summary>
        [SugarColumn(Length = 30)]
        public string? TypeDescription { get; set; }
        /// <summary>
        /// 类型 1异常预警 2 嘉奖通报 3提醒事项
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int Type { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [SugarColumn(Length = 500)]
        public string? Description { get; set; }
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
