using MiniExcelLibs.Attributes;
using SqlSugar;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 计划基准
    /// </summary>
    [SugarTable("t_baselineplancomparison", IsDisabledDelete = true)]
    public class BaseLinePlanAncomparison : BaseEntity<Guid>
    {

        [ExcelColumnName("1月计划")]
        /// <summary>
        /// 1月产值
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal JanuaryProductionValue { get; set; }

        [ExcelColumnName("2月计划")]
        /// <summary>
        /// 2月产值
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal FebruaryProductionValue { get; set; }

        [ExcelColumnName("3月计划")]
        /// <summary>
        /// 3月产值
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal MarchProductionValue { get; set; }

        [ExcelColumnName("4月计划")]
        /// <summary>
        /// 4月产值
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal AprilProductionValue { get; set; }

        [ExcelColumnName("5月计划")]
        /// <summary>
        /// 5月产值
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal MayProductionValue { get; set; }

        [ExcelColumnName("6月计划")]
        /// <summary>
        /// 6月产值
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal JuneProductionValue { get; set; }

        [ExcelColumnName("7月计划")]
        /// <summary>
        /// 7月产值
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal JulyProductionValue { get; set; }

        [ExcelColumnName("8月计划")]
        /// <summary>
        /// 8月产值
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal AugustProductionValue { get; set; }

        [ExcelColumnName("9月计划")]
        /// <summary>
        /// 9月产值
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal SeptemberProductionValue { get; set; }

        [ExcelColumnName("10月计划")]
        /// <summary>
        /// 10月产值
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal OctoberProductionValue { get; set; }

        [ExcelColumnName("11月计划")]
        /// <summary>
        /// 11月产值
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal NovemberProductionValue { get; set; }

        [ExcelColumnName("12月计划")]
        /// <summary>
        /// 12月产值
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal DecemberProductionValue { get; set; }

        [ExcelColumnName("主数据编码")]
        /// <summary>
        /// 项目id
        /// </summary>
        [SugarColumn(Length = 36)]
        public string Code { get; set; }


        [ExcelColumnName("项目名称")]
        /// <summary>
        /// 项目id
        /// </summary>
        [SugarColumn(Length = 36)]
        public string ProjectName { get; set; }

        [ExcelColumnName("管理单位")]
        /// <summary>
        /// 所属公司
        /// </summary>
        [SugarColumn(Length = 36)]
        public string Company { get; set; }
    }
}
