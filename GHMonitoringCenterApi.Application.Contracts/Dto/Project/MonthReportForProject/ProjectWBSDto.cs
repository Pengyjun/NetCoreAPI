using GHMonitoringCenterApi.Domain.Enums;
using GHMonitoringCenterApi.Domain.Shared;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project.MonthReportForProject
{
    /// <summary>
    /// wbs转换响应dto
    /// </summary>
    public class ProjectWBSDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 项目Id
        /// </summary>
        public string? ProjectId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid? CurrencyId { get; set; }
        /// <summary>
        /// 日报主键id
        /// </summary>
        public Guid? DayReportId { get; set; }
        /// <summary>
        /// 合同工程量
        /// </summary>
        public decimal? EngQuantity { get; set; }
        /// <summary>
        /// Pid
        /// </summary>
        public string? Pid { get; set; }
        /// <summary>
        /// 子集id
        /// </summary>
        public string? KeyId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 施工分类名称
        /// </summary>
        public string? ProjectWBSName { get; set; }
        /// <summary>
        /// 做业务区分字段
        /// month
        /// </summary>
        public ValueEnumType ValueType { get; set; }
        /// <summary>
        /// 列表行允许删除 true允许删除  false不允许删除
        /// </summary>
        public bool IsAllowDelete { get; set; }
        /// <summary>
        /// 是否是日报数据 是的话 不清0
        /// </summary>
        public bool IsDayRep { get; set; } = false;
        /// <summary>
        /// 填报月份
        /// </summary>
        public int DateMonth { get; set; }
        /// <summary>
        /// 填报年份
        /// </summary>
        public int DateYear { get; set; }

        /// <summary>
        /// 子节点
        /// </summary>
        public List<ProjectWBSDto> Children { get; set; } = new List<ProjectWBSDto> { };
        /// <summary>
        /// 最后资源节点
        /// </summary>
        public List<ProjectWBSDto> ReportDetails { get; set; } = new List<ProjectWBSDto> { };

        #region 详细节点字段
        /// <summary>
        /// 本月完成产值
        /// </summary>
        [Newtonsoft.Json.JsonConverter(typeof(NullToDefaultConverter<decimal>))]
        public decimal CompleteProductionAmount { get; set; }
        /// <summary>
        /// 本月完成工程量(方)
        /// </summary>
        public decimal CompletedQuantity { get; set; }
        /// <summary>
        /// 施工性质
        /// </summary>
        public int? ConstructionNature { get; set; }
        /// <summary>
        /// 施工性质名称
        /// </summary>
        public string? ConstructionNatureName { get; set; }
        /// <summary>
        /// 合同产值
        /// </summary>
        public decimal? ContractAmount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal? ContractQuantity { get; set; }
        /// <summary>
        /// 详细月报主键id
        /// </summary>
        public Guid? DetailId { get; set; }
        /// <summary>
        /// 产值属性
        /// </summary>
        [Newtonsoft.Json.JsonConverter(typeof(NullToDefaultConverter<int>))]
        public ConstructionOutPutType OutPutType { get; set; }
        /// <summary>
        /// 产值属性名称
        /// </summary>
        public string? OutPutTypeName { get; set; }
        /// <summary>
        /// 本月外包支出
        /// </summary>
        public decimal OutsourcingExpensesAmount { get; set; }
        /// <summary>
        /// 项目WBSId
        /// </summary>
        public Guid ProjectWBSId { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
        /// <summary>
        /// 资源：船舶Id(PomId)
        /// </summary>
        public Guid? ShipId { get; set; }
        /// <summary>
        /// 船舶名称
        /// </summary>
        public string? ShipName { get; set; }
        /// <summary>
        /// 累计完成产值
        /// </summary>
        public decimal TotalCompleteProductionAmount { get; set; } = 0M;
        /// <summary>
        /// 累计完成工程量
        /// </summary>
        public decimal TotalCompletedQuantity { get; set; } = 0M;
        /// <summary>
        /// 累计外包支出
        /// </summary>
        public decimal TotalOutsourcingExpensesAmount { get; set; } = 0M;
        /// <summary>
        /// 单价
        /// </summary>
        public decimal? UnitPrice { get; set; }
        /// <summary>
        /// 本年完成产值
        /// </summary>
        public decimal YearCompleteProductionAmount { get; set; }
        /// <summary>
        /// 本年完成工程量
        /// </summary>
        public decimal YearCompletedQuantity { get; set; }
        /// <summary>
        /// 本年外包支出
        /// </summary>
        public decimal YearOutsourcingExpensesAmount { get; set; }
        /// <summary>
        /// 本月外包支出(实际)
        /// </summary>
        public decimal? ActualOutAmount { get; set; } = 0M;
        /// <summary>
        /// 本月完成工程量(方)(实际)
        /// </summary>
        public decimal? ActualCompQuantity { get; set; } = 0M;
        /// <summary>
        /// 本月完成产值(实际)
        /// </summary>
        public decimal? ActualCompAmount { get; set; } = 0M;
        /// <summary>
        /// 实际人民币产值
        /// </summary>
        public decimal? RMBHValue { get; set; } = 0M;
        /// <summary>
        /// 实际人民币外包支出
        /// </summary>
        public decimal? RMBHOutValue { get; set; } = 0M;

        #region 原值
        /// <summary>
        /// 原值工程量
        /// </summary>
        public decimal? OldHQuantity { get; set; } = 0M;
        /// <summary>
        /// 历史外包支出(美元、欧元等)
        /// </summary>
        public decimal? OldCurrencyHOutValue { get; set; } = 0M;
        /// <summary>
        /// 历史完成产值(美元、欧元等)
        /// </summary>
        public decimal? OldCurrencyHValue { get; set; } = 0M;
        #endregion
        #endregion
        /// <summary>
        /// wbs使用
        /// </summary>
        public int IsDelete { get; set; }

        #region 下一步详细字段

        /// <summary>
        /// 本月甲方确认产值（元）
        /// </summary>
        public decimal? PartyAConfirmedProductionAmount { get; set; }

        /// <summary>
        /// 本月甲方确认产值（元）
        /// </summary>
        public decimal? PartyAPayAmount { get; set; }

        /// <summary>
        /// 本月应收金额（元）
        /// </summary>
        public decimal? ReceivableAmount { get; set; }

        /// <summary>
        /// 进度偏差主因
        /// </summary>
        public DeviationReason? ProgressDeviationReason { get; set; }

        /// <summary>
        /// 主要形象进度描述
        /// </summary>
        public string? ProgressDescription { get; set; }

        /// <summary>
        /// 本月实际成本（元）
        /// </summary>
        public decimal? CostAmount { get; set; }

        /// <summary>
        /// 成本偏差主因
        /// </summary>
        public DeviationReason? CostDeviationReason { get; set; }

        /// <summary>
        /// 下月估算成本（元）
        /// </summary>
        public decimal? NextMonthEstimateCostAmount { get; set; }

        /// <summary>
        /// 进度偏差原因简述
        /// </summary>
        public string? ProgressDeviationDescription { get; set; }

        /// <summary>
        /// 成本偏差原因简述
        /// </summary>
        public string? CostDeviationDescription { get; set; }

        /// <summary>
        /// 需公司协调事项 
        /// </summary>
        public string? CoordinationMatters { get; set; }

        /// <summary>
        ///存在问题简述
        /// </summary>
        public string? ProblemDescription { get; set; }

        /// <summary>
        /// 解决措施简述 
        /// </summary>
        public string? SolveProblemDescription { get; set; }

        #endregion
    }
}
