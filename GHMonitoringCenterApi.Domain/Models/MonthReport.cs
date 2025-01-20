using GHMonitoringCenterApi.Domain.Enums;
using SqlSugar;

namespace GHMonitoringCenterApi.Domain.Models
{

    /// <summary>
    /// 项目月报表
    /// </summary>
    [SugarTable("t_monthreport", IsDisabledDelete = true)]
    public class MonthReport : BaseEntity<Guid>
    {
        /// <summary>
        /// 项目id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid ProjectId { get; set; }

        /// <summary>
        /// 填报月份（例：202304，注解：上月的26-本月的25，例：2023.3.26-2023.04.25）
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int DateMonth { get; set; }

        /// <summary>
        /// 填报年份
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int DateYear { get; set; }

        /// <summary>
        /// 本月完成工程量(方)
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal CompletedQuantity { get; set; }

        /// <summary>
        /// 本月完成产值(元)
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal CompleteProductionAmount { get; set; }

        /// <summary>
        /// 本月外包支出（元）
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal OutsourcingExpensesAmount { get; set; }

        /// <summary>
        /// 本月甲方确认产值（元）
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal PartyAConfirmedProductionAmount { get; set; }

        /// <summary>
        /// 本月甲方付款金额(元)
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal PartyAPayAmount { get; set; }

        /// <summary>
        /// 月度应收账款(元)
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal ReceivableAmount { get; set; }

        /// <summary>
        /// 进度偏差主因
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public DeviationReason ProgressDeviationReason { get; set; }

        /// <summary>
        /// 进度偏差原因简述
        /// </summary>
        [SugarColumn(Length = 2000)]
        public string? ProgressDeviationDescription { get; set; }

        /// <summary>
        /// 进度偏差原因简述推送pom
        /// </summary>
        [SugarColumn(Length = 2000)]
        public string? ProgressDeviationDescriptionPushPom { get; set; }

        /// <summary>
        /// 主要形象进度描述
        /// </summary>
        [SugarColumn(Length = 2000)]
        public string? ProgressDescription { get; set; }
        /// <summary>
        /// 主要形象进度描述推送pom
        /// </summary>
        [SugarColumn(Length = 2000)]
        public string? ProgressDescriptionPushPom { get; set; }

        /// <summary>
        /// 本月实际成本（元）
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal CostAmount { get; set; }

        /// <summary>
        /// 成本偏差主因
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public DeviationReason CostDeviationReason { get; set; }

        /// <summary>
        /// 成本偏差原因简述
        /// </summary>
        [SugarColumn(Length = 2000)]
        public string? CostDeviationDescription { get; set; }

        /// <summary>
        /// 本月估算成本（元）
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal MonthEstimateCostAmount { get; set; }

        /// <summary>
        /// 下月估算成本（元）
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal NextMonthEstimateCostAmount { get; set; }

        /// <summary>
        /// 币种Id（默认人民币）
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid CurrencyId { get; set; }

        /// <summary>
        /// 本月完成产值(币种：元，美元，欧元等)
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal CurrencyCompleteProductionAmount { get; set; }
        /// <summary>
        /// 外包支出 (币种：元，美元，欧元等)
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal CurrencyOutsourcingExpensesAmount { get; set; } = 0;

        /// <summary>
        /// 币种汇率
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,4)", DefaultValue = "1")]
        public decimal CurrencyExchangeRate { get; set; }

        /// <summary>
        ///暂存数据是否生效(字段作废)
        /// </summary>
        [Obsolete]
        [SugarColumn(ColumnDataType = "bit")]
        public bool IsEffectStaging { get; set; }

        /// <summary>
        /// 需公司协调事项 
        /// </summary>
        [SugarColumn(ColumnDataType = "text")]
        public string? CoordinationMatters { get; set; }

        /// <summary>
        ///存在问题简述
        /// </summary>
        [SugarColumn(ColumnDataType = "text")]
        public string? ProblemDescription { get; set; }

        /// <summary>
        /// 解决措施简述 
        /// </summary>
        [SugarColumn(ColumnDataType = "text")]
        public string? SolveProblemDescription { get; set; }

        /// <summary>
        /// 相关描述是否推送pom
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int IsPushPom { get; set; }

        /// <summary>
        ///状态
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public MonthReportStatus Status { get; set; }

        /// <summary>
        /// 状态文本
        /// </summary>
        [SugarColumn(Length = 200)]
        public string? StatusText { get; set; }

        /// <summary>
        /// 任务Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? JobId { get; set; }

        /// <summary>ss
        /// 驳回原因
        /// </summary>
        [SugarColumn(Length = 500)]
        public string? RejectReason { get; set; }
        /// <summary>
        /// 实际人民币产值（用户 输入）
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal? RMBHValue { get; set; } = 0M;
        /// <summary>
        /// 实际人民币外包支出（用户 输入）
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal? RMBHOutValue { get; set; } = 0M;
    }
}
