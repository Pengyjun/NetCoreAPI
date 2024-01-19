using SqlSugar;
namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 船舶年度计划产值  与项目年度计划不同  123为一季度查询日期为yyyy-1-1 当前月为1月取一月计划  为二月时取二月 456为二季度 查询日期为yyyy-3-1  为四月时取四月
    /// </summary>
    [SugarTable("t_shipyearplan", IsDisabledDelete = true)]
    public class ShipYearPlan : BaseEntity<Guid>
    {
        /// <summary>
        /// 船舶id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? ShipId { get; set; }
        /// <summary>
        /// 船舶名称
        /// </summary>
        [SugarColumn(Length = 100)]
        public string ShipName { get; set; }
        /// <summary>
        /// 计划产值 第一月   
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal OnePlannedOutputValue { get; set; }
        /// <summary>
        /// 计划产值   第二月
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal TwoPlannedOutputValue { get; set; }
        /// <summary>
        /// 计划产值  第三月
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal ThreePlannedOutputValue { get; set; }
        /// <summary>
        /// 计划产值  第四月
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal FourPlannedOutputValue { get; set; }
        /// <summary>
        /// 计划产值  第五月
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal FivPlannedOutputValue { get; set; }
        /// <summary>
        /// 计划产值  第六月
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal SixPlannedOutputValue { get; set; }
        /// <summary>
        /// 计划产值  第七月
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal SevPlannedOutputValue { get; set; }
        /// <summary>
        /// 计划产值  第八月
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal EigPlannedOutputValue { get; set; }
        /// <summary>
        /// 计划产值  第九月
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal NinPlannedOutputValue { get; set; }
        /// <summary>
        /// 计划产值  第十月
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal TenPlannedOutputValue { get; set; }
        /// <summary>
        /// 计划产值  第十一月
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal ElePlannedOutputValue { get; set; }
        /// <summary>
        /// 计划产值  第十二月
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal TwePlannedOutputValue { get; set; }
        /// <summary>
        /// 提交日期
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime SubmitYear { get; set; }
    }
}
