using GHMonitoringCenterApi.Domain.Enums;
using SqlSugar;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 项目月报明细表
    /// </summary>
    [SugarTable("t_monthreportdetail", IsDisabledDelete = true)]
    public class MonthReportDetail : BaseEntity<Guid>
    {
        /// <summary>
        /// 月报Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid MonthReportId { get; set; }

        /// <summary>
        /// 项目id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid ProjectId { get; set; }

        /// <summary>
        /// 填报月份
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int DateMonth { get; set; }

        /// <summary>
        /// 填报年份
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int DateYear { get; set; }

        /// <summary>
        /// 施工分类Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid ProjectWBSId { get; set; }

        /// <summary>
        /// 产值属性（自有：1，分包：2,分包-自有: 4,）
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public ConstructionOutPutType OutPutType { get; set; }

        /// <summary>
        /// 船舶Id(PomId)
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid ShipId { get; set; }
        /// <summary>
        /// 船舶名称
        /// </summary>
        [SugarColumn(Length = 256)]
        public string? ShipName { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// 施工性质
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int? ConstructionNature { get; set; }

        /// <summary>
        /// 本月外包支出
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal OutsourcingExpensesAmount { get; set; }

        /// <summary>
        /// 本月完成工程量(方)
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal CompletedQuantity { get; set; }

        /// <summary>
        /// 本月完成产值（元）
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal CompleteProductionAmount { get; set; }

        /// <summary>
        /// 本月外包支出(偏差本币)
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal? DeviationOutAmount { get; set; }
        ///// <summary>
        ///// 本月外包支出(偏差外币)
        ///// </summary>
        //[SugarColumn(ColumnDataType = "decimal(20,8)")]
        //public decimal? CurrencyDeviationOutAmount { get; set; }
        /// <summary>
        /// 本月外包支出(实际本币)
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal? ActualOutAmount { get; set; }
        ///// <summary>
        ///// 本月外包支出(实际外币)
        ///// </summary>
        //[SugarColumn(ColumnDataType = "decimal(20,8)")]
        //public decimal? CurrencyActualOutAmount { get; set; }
        /// <summary>
        /// 本月完成工程量(方)(偏差)
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal? DeviationCompQuantity { get; set; }
        /// <summary>
        /// 本月完成工程量(方)(实际)
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal? ActualCompQuantity { get; set; }
        /// <summary>
        /// 本月完成产值(偏差本币)
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal? DeviationCompAmount { get; set; }
        ///// <summary>
        ///// 本月完成产值(偏差外币)
        ///// </summary>
        //[SugarColumn(ColumnDataType = "decimal(20,8)")]
        //public decimal? CurrencyDeviationCompAmount { get; set; }
        /// <summary>
        /// 本月完成产值(实际本币)
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal? ActualCompAmount { get; set; }
        ///// <summary>
        ///// 本月完成产值(实际外币)
        ///// </summary>
        //[SugarColumn(ColumnDataType = "decimal(20,8)")]
        //public decimal? CurrencyActualCompAmount { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(Length = 800)]
        public string? Remark { get; set; }
    }
}
