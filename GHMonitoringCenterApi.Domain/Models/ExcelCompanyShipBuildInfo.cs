using SqlSugar;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// Excel各个公司自有船施工运转情况（数量）
    /// </summary>
    [SugarTable("t_excelcompanyshipbuildinfo", IsDisabledDelete = true)]
    public class ExcelCompanyShipBuildInfo : BaseEntity<Guid>
    {
        /// <summary>
        /// 船舶类型名称
        /// </summary>
        [SugarColumn(Length = 50)]
        public string ShipTypeName { get; set; }
        /// <summary>
        /// 每个类别船舶的数量
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int Count { get; set; } = 0;
        /// <summary>
        /// 施工数量
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int BuildCount { get; set; } = 0;
        /// <summary>
        /// 检修数量
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int ReconditionCount { get; set; } = 0;
        /// <summary>
        /// 调遣数量
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int AssignCount { get; set; } = 0;
        /// <summary>
        /// 待命数量
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int AwaitCount { get; set; } = 0;
        /// <summary>
        /// 开工率
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,4)")]
        public decimal BuildPercent { get; set; }
        /// <summary>
        /// 船舶类型排序
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
