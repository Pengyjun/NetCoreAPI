using SqlSugar;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 项目产值计划表
    /// </summary>
    [SugarTable("t_projectplanproduction", IsDisabledDelete = true)]
    public class ProjectPlanProduction:BaseEntity<Guid>
    {
        /// <summary>
        /// 项目id
        /// </summary>
        [SugarColumn(Length =36)]
        public Guid ProjectId { get; set; }
        /// <summary>
        /// 公司ID
        /// </summary>
        [SugarColumn(Length =36)]
        public Guid CompanyId { get; set; }
        /// <summary>
        /// 年份
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int Year { get; set; }
        /// <summary>
        /// 第一月计划产值
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? OnePlanProductionValue { get; set; }
    
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? TwoPlanProductionValue { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? ThreePlanProductionValue { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? FourPlanProductionValue { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? FivePlanProductionValue { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? SixPlanProductionValue { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? SevenPlanProductionValue { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? EightPlanProductionValue { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? NinePlanProductionValue { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? TenPlanProductionValue { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? ElevenPlanProductionValue { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? TwelvePlanProductionValue { get; set; }

        /// <summary>
        /// 计划工程量 按季度导入  第一月
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? OnePlannedQuantities { get; set; }
        /// <summary>
        /// 计划工程量   第二月
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,8")]
        public decimal? TwoPlannedQuantities { get; set; }
        /// <summary>
        /// 计划工程量  第三月
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? ThreePlannedQuantities { get; set; }
        /// <summary>
        /// 计划工程量  第四月
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? FourPlannedQuantities { get; set; }
        /// <summary>
        /// 计划工程量  第五月
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? FivPlannedQuantities { get; set; }
        /// <summary>
        /// 计划工程量  第六月
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? SixPlannedQuantities { get; set; }
        /// <summary>
        /// 计划工程量  第七月
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? SevPlannedQuantities { get; set; }
        /// <summary>
        /// 计划工程量  第八月
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? EigPlannedQuantities { get; set; }
        /// <summary>
        /// 计划工程量  第九月
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? NinPlannedQuantities { get; set; }
        /// <summary>
        /// 计划工程量  第十月
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? TenPlannedQuantities { get; set; }
        /// <summary>
        /// 计划工程量  第十一月
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? ElePlannedQuantities { get; set; }
        /// <summary>
        /// 计划工程量  第十二月
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? TwePlannedQuantities { get; set; }

    }
}
