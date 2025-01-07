using GHMonitoringCenterApi.Domain.Enums;
using SqlSugar;

namespace GHMonitoringCenterApi.Domain.Models
{

    /// <summary>
    /// 项目日报表
    /// </summary>
    [SugarTable("t_dayreport", IsDisabledDelete = true)]
    [SugarIndex("INDEX_UQ_KEY", nameof(ProjectId), OrderByType.Asc, nameof(DateDay), OrderByType.Asc, true)]
    public class DayReport : BaseEntity<Guid>
    {
        /// <summary>
        /// 项目id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid ProjectId { get; set; }

        /// <summary>
        /// 文件Id(组合逗号分割)
        /// </summary>
        [SugarColumn(Length = 2000)]
        public string? FileId { get; set; }

        /// <summary>
        /// 天气（映射字典表）
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int Weather { get; set; }

        /// <summary>
        /// 现场施工人员数量
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int SiteConstructionPersonNum { get; set; }

        /// <summary>
        /// 现场管理人员
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int SiteManagementPersonNum { get; set; }

        /// <summary>
        /// 施工设备
        /// </summary>
        [SugarColumn(Length = 2000)]
        [Obsolete]
        public string? ConstructionDevice { get; set; }

        /// <summary>
        /// 施工备注（提示：请输入项目管理工作、存在问题）
        /// </summary>
        [SugarColumn(ColumnDataType = "text")]
        public string? ConstructionRemarks { get; set; }

        /// <summary>
        /// 其他记录
        /// </summary>
        [SugarColumn(Length = 2000)]
        public string? OtherRecord { get; set; }

        /// <summary>
        /// 日报进程状态(1:填写步骤中，2：已提交)
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public DayReportProcessStatus ProcessStatus { get; set; }

        /// <summary>
        /// 填报日期（例：20230401）
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int DateDay { get; set; }

        /// <summary>
        /// 项目负责人
        /// </summary>
        [SugarColumn(Length = 200)]
        public string? TeamLeader { get; set; }

        /// <summary>
        /// 班组长确认时间
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime? TeamLeaderConfirmTime { get; set; }

        /// <summary>
        /// 特殊事项报告（0：无,1:异常预警，2：嘉奖通报，3：提醒事项）
        /// </summary>
        [SugarColumn(ColumnDataType = "int", DefaultValue = "0")]
        public int IsHaveProductionWarning { get; set; }

        /// <summary>
        /// 特殊事项报告内容
        /// </summary>
        [SugarColumn(Length = 1000)]
        public string? ProductionWarningContent { get; set; }

        /// <summary>
        /// 春节停工计划（1：春节期间不停工，2：春节期间停工）
        /// </summary>
        [SugarColumn(ColumnDataType = "int", DefaultValue = "0")]
        public WorkStatusOfSpringFestival WorkStatusOfSpringFestival { get; set; }

        /// <summary>
        /// 春节开始停工时间
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime? StartStopWorkOfSpringFestival { get; set; }

        /// <summary>
        /// 春节结束停工时间
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime? EndStopWorkOfSpringFestival { get; set; }

        /// <summary>
        /// 已完成合同金额(元)
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)", DefaultValue = "0")]
        public decimal CompleteAmount { get; set; }

        /// <summary>
        /// 当月计划产值（元）
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)", DefaultValue = "0")]
        public decimal MonthPlannedProductionAmount { get; set; }

        /// <summary>
        /// 当日实际产值（元）
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal DayActualProductionAmount { get; set; }

        /// <summary>
        /// 当日实际产值（币种：元，美元，欧元等)
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal CurrencyDayProductionAmount { get; set; }

        /// <summary>
        /// 当日产量（方）
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal DayActualProduction { get; set; }

        /// <summary>
        /// 外包支出（元）
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal OutsourcingExpensesAmount { get; set; }

        /// <summary>
        /// 币种Id（默认人民币）
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid CurrencyId { get; set; }

        /// <summary>
        /// 币种汇率
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,4)", DefaultValue = "1")]
        public decimal CurrencyExchangeRate { get; set; }

        /// <summary>
        /// 当月实际产值（元）
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal MonthProductionAmount { get; set; }

        /// <summary>
        /// 年度实际产值（元）
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal YearProductionAmount { get; set; }

        /// <summary>
        /// 累计实际产值（元）
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal CumulativeProductionAmount { get; set; }

        /// <summary>
        ///  投入设备（台）
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]

        public int ConstructionDeviceNum { get; set; }

        /// <summary>
        ///  危大工程施工（项）
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int HazardousConstructionNum { get; set; }
        /// <summary>
        /// 危大工程明细
        /// </summary>
        [SugarColumn(ColumnDataType = "text")]
        public string? HazardousConstructionDetails { get; set; }

        /// <summary>
        /// 产能较低原因
        /// <para>低于当日计划产值80%</para>
        /// </summary>
        [SugarColumn(ColumnDataType = "text")]
        public string? LowProductionReason { get; set; }

        /// <summary>
        /// 陆域9人以上作业地点（处）
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int LandWorkplace { get; set; }

        /// <summary>
        /// 带班领导
        /// </summary>
        [SugarColumn(Length = 50)]
        public string? ShiftLeader { get; set; }

        /// <summary>
        /// 带班领导电话
        /// </summary>
        [SugarColumn(Length = 50)]
        public string? ShiftLeaderPhone { get; set; }

        /// <summary>
        /// 陆域3-9人以上作业地点（处）
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int FewLandWorkplace { get; set; }

        /// <summary>
        /// 现场船舶（艘）
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int SiteShipNum { get; set; }

        /// <summary>
        /// 在船人员（人）
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int OnShipPersonNum { get; set; }

        /// <summary>
        ///  简述当日危大工程施工内容
        /// </summary>
        [SugarColumn(ColumnDataType = "text")]
        public string? HazardousConstructionDescription { get; set; }

        /// <summary>
        /// 是否是节假日
        /// </summary>
        [SugarColumn(ColumnDataType = "bit")]
        public bool IsHoliday { get; set; }
        /// <summary>
        /// 偏差预警
        /// </summary>
        [SugarColumn(ColumnDataType = "text")]
        public string? DeviationWarning { get; set; }

        /// <summary>
        /// 是否偏低    1 是偏高  0是偏低
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int? IsLow { get; set; }
    }
}
