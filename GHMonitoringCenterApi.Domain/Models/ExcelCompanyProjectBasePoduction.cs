using SqlSugar;
namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// Excel各个公司基本项目情况
    /// </summary>
    [SugarTable("t_excelcompanyprojectbasepoduction", IsDisabledDelete = true)]
    public class ExcelCompanyProjectBasePoduction : BaseEntity<Guid>
    {
        /// <summary>
        /// 业务单位
        /// </summary>
        [SugarColumn(Length = 50)]
        public string UnitName { get; set; }
        /// <summary>
        ///合同项目数量
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int OnContractProjectCount { get; set; } = 0;
        /// <summary>
        /// 在建项目数量
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int OnBuildProjectCount { get; set; } = 0;
        /// <summary>
        /// 停缓建数量
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int StopBuildProjectCount { get; set; } = 0;
        /// <summary>
        /// 未开工数量
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int NotWorkCount { get; set; } = 0;
        /// <summary>
        /// 在建数量占比
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,4)")]
        public decimal BuildCountPercent { get; set; }
        /// <summary>
        /// 设备数量
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int FacilityCount { get; set; } = 0;
        /// <summary>
        /// 工人数量
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int WorkerCount { get; set; } = 0;
        /// <summary>
        /// 危大施工项数量
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int RiskWorkCount { get; set; } = 0;
        /// <summary>
        /// 单位排序
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int UnitDesc { get; set; }
        /// <summary>
        /// 年份
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int Year {  get; set; }
        /// <summary>
        /// 月份
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int Month {  get; set; }
        /// <summary>
        /// 天
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int DateDay {  get; set; }
    }
}
