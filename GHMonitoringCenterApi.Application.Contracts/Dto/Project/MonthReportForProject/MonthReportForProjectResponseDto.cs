using GHMonitoringCenterApi.Application.Contracts.Dto.Enums;
using GHMonitoringCenterApi.Domain.Enums;
using UtilsSharp;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project.MonthReportForProject
{
    /// <summary>
    /// 新的项目月报产包构成列表详细响应dto
    /// </summary>
    public class MonthReportForProjectResponseDto
    {
        #region 新增字段
        /// <summary>
        /// 本年甲方确认产值
        /// </summary>
        public decimal CurrentYearOffirmProductionValue { get; set; } = 0M;
        /// <summary>
        /// 开累甲方确认产值
        /// </summary>
        public decimal TotalYearKaileaOffirmProductionValue { get; set; } = 0M;
        /// <summary>
        /// 本年甲方付款金额 
        /// </summary>
        public decimal CurrenYearCollection { get; set; } = 0M;
        /// <summary>
        /// 开累甲方付款金额
        /// </summary>
        public decimal TotalYearCollection { get; set; } = 0M;
        /// <summary>
        /// 历史外包支出（累计至今使用）
        /// </summary>
        public decimal? HOutValue { get; set; } = 0M;
        /// <summary>
        /// 历史工程量（累计至今使用）
        /// </summary>
        public decimal? HQuantity { get; set; } = 0M;
        /// <summary>
        /// 历史完成产值
        /// </summary>
        public decimal? HValue { get; set; } = 0M;
        /// <summary>
        /// 外币完成产值
        /// </summary>
        public decimal? CurrencyHValue { get; set; } = 0M;
        /// <summary>
        /// 外币外包支出
        /// </summary>
        public decimal? CurrencyOutHValue { get; set; } = 0M;
        #endregion
        /// <summary>
        /// 填报月份（例：202304，注解：上月的26-本月的25，例：2023.3.26-2023.04.25）
        /// </summary>
        public int DateMonth { get; set; }
        /// <summary>
        /// 填报月份时间格式（前端可以根据需求获取年，月拼接）
        /// </summary>
        public DateTime DateMonthTime
        {
            get
            {
                ConvertHelper.TryParseFromDateMonth(DateMonth, out DateTime monthTime);
                return monthTime;
            }
        }
        /// <summary>
        /// 合同工程量
        /// </summary>
        public decimal? ContractQuantity { get; set; } = 0M;
        /// <summary>
        /// 合同额
        /// </summary>
        public decimal? ContractAmount { get; set; } = 0M;
        /// <summary>
        /// 本年外包支出(元)
        /// </summary>
        public decimal YearOutsourcingExpensesAmount { get; set; } = 0M;
        /// <summary>
        /// 本年完成工程量(方)
        /// </summary>
        public decimal YearCompletedQuantity { get; set; } = 0M;
        /// <summary>
        /// 本年完成产值（元）
        /// </summary>
        public decimal YearCompleteProductionAmount { get; set; } = 0M;
        /// <summary>
        /// 累计外包支出(元)
        /// </summary>
        public decimal TotalOutsourcingExpensesAmount { get; set; } = 0M;
        /// <summary>
        /// 累计完成工程量(方)
        /// </summary>
        public decimal TotalCompletedQuantity { get; set; } = 0M;
        /// <summary>
        /// 累计完成产值（元）
        /// </summary>
        public decimal TotalCompleteProductionAmount { get; set; } = 0M;
        /// <summary>
        /// 是否可以提交/保存
        /// </summary>
        public bool IsCanSubmit { get; set; }
        /// <summary>
        /// 是否可以暂存
        /// </summary>
        public bool IsCanStaging { get; set; }

        /// <summary>
        /// 任务提交类型
        /// </summary>
        public JobSubmitType JobSubmitType { get; set; }

        /// <summary>
        /// 本月外包支出
        /// </summary>
        public decimal OutsourcingExpensesAmount { get; set; }

        /// <summary>
        /// 本月完成工程量(方
        /// </summary>
        public decimal CompletedQuantity { get; set; } = 0M;
        /// <summary>
        /// 本月完成产值
        /// </summary>
        public decimal CompleteProductionAmount { get; set; } = 0M;
        /// <summary>
        /// 产值属性(1:自有，2：分包，4：分包-自有)
        /// </summary>
        public ConstructionOutPutType? OutPutType { get; set; }
        /// <summary>
        /// 产值属性名称
        /// </summary>
        public string? OutPutTypeName
        {
            get
            {
                if (OutPutType != null)
                {
                    return EnumExtension.GetEnumDescription(OutPutType);
                }
                return null;
            }
        }
        /// <summary>
        /// 资源树（船舶）
        /// </summary>
        public List<ProjectWBSDto>? TreeDetails { get; set; }
        /// <summary>
        /// 产值累计
        /// </summary>
        public decimal TotalProductionAmount { get; set; } = 0M;
        /// <summary>
        /// 项目Id
        /// </summary>
        public Guid ProjectId { get; set; }
        /// <summary>
        /// 本月甲方确认产值（元）
        /// </summary>
        public decimal? PartyAConfirmedProductionAmount { get; set; } = 0M;
        /// <summary>
        /// 本月甲方确认产值（元）
        /// </summary>
        public decimal? PartyAPayAmount { get; set; } = 0M;
        /// <summary>
        /// 本月应收金额（元）
        /// </summary>
        public decimal? ReceivableAmount { get; set; } = 0M;
        /// <summary>
        /// 下月滚动计划
        /// </summary>
        public decimal RollingPlanForNextMonth { get; set; } = 0M;
        /// <summary>
        /// 相关描述是否推送pom
        /// </summary>
        public bool IsPushPom { get; set; }
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
        public decimal? CostAmount { get; set; } = 0M;
        /// <summary>
        /// 成本偏差主因
        /// </summary>
        public DeviationReason? CostDeviationReason { get; set; }
        /// <summary>
        /// 下月估算成本（元）
        /// </summary>
        public decimal? NextMonthEstimateCostAmount { get; set; } = 0M;
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
        /// <summary>
        /// 是否来源于暂存数据
        /// </summary>
        public bool IsFromStaging { get; set; }
        /// <summary>
        /// 项目类别 0 境内  1 境外
        /// </summary>
        public int ProjectCategory { get; set; }
        /// <summary>
        /// 币种Id
        /// </summary>
        public Guid CurrencyId { get; set; }
        /// <summary>
        /// 币种汇率
        /// </summary>
        public decimal CurrencyExchangeRate { get; set; } = 1m;
        /// <summary>
        /// 是否是非施工类项目
        /// </summary>
        public bool IsNonConstruction { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public ModelState ModelState { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string? ProjectName { get; set; }
        /// <summary>
        ///状态
        /// </summary>
        public MonthReportStatus Status { get; set; }
        /// <summary>
        /// 状态文本
        /// </summary>
        public string? StatusText { get; set; }
        /// <summary>
        /// 任务Id
        /// </summary>
        public Guid? JobId { get; set; }

        #region top置顶行历史数202306
        /// <summary>
        /// 置顶数据名称
        /// </summary>
        public string? TopTitleName { get; set; }
        /// <summary>
        /// 历史外包支出(美元、欧元等)
        /// </summary>
        public decimal? TopCurrencyHOutValue { get; set; } = 0M;
        /// <summary>
        /// 历史工程量
        /// </summary>
        public decimal? TopHQuantity { get; set; } = 0M;
        /// <summary>
        /// 历史完成产值(美元、欧元等)
        /// </summary>
        public decimal? TopCurrencyHValue { get; set; } = 0M;
        /// <summary>
        /// 实际人民币产值
        /// </summary>
        public decimal? TopRMBHValue { get; set; } = 0M;
        /// <summary>
        /// 实际人民币外包支出
        /// </summary>
        public decimal? TopRMBHOutValue { get; set; } = 0M;
        /// <summary>
        /// 给前端计算好的外币开累数（产值）
        /// </summary>
        public decimal RMBTotalAmount { get; set; } = 0M;
        /// <summary>
        /// 给前端计算好的外币开累数（外包支出）
        /// </summary>
        public decimal RMBTotalOutAmount { get; set; } = 0M;

        #region 原值
        /// <summary>
        /// 工程量
        /// </summary>
        public decimal? OldHQuantity { get; set; } = 0M;
        /// <summary>
        /// 历史外包支出(人民币)
        /// </summary>
        public decimal? OldHOutValue { get; set; } = 0M;
        /// <summary>
        /// 历史外包支出(美元、欧元等)
        /// </summary>
        public decimal? OldCurrencyHOutValue { get; set; } = 0M;
        /// <summary>
        /// 历史完成产值(人民币)
        /// </summary>
        public decimal? OldHValue { get; set; } = 0M;
        /// <summary>
        /// 历史完成产值(美元、欧元等)
        /// </summary>
        public decimal? OldCurrencyHValue { get; set; } = 0M;
        #endregion
        #endregion
    }
}
