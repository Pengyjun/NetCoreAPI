using GHMonitoringCenterApi.Domain.Enums;
using GHMonitoringCenterApi.Domain.Models;
using SqlSugar;
using UtilsSharp;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report
{

    /// <summary>
    /// 项目月报集合Dto
    /// </summary>
    public class MonthtReportsResponseDto
    {
        
        /// <summary>
        /// 合计
        /// </summary>
        public TotalMonthtReportsDto Total { get; set; }

        /// <summary>
        /// 项目月报列表
        /// </summary>
        public List<MonthtReportDto> Reports { get; set; }

        /// <summary>
        /// 汇总月报明细
        /// </summary>
        public class TotalMonthtReportsDto
        {
            /// <summary>
            /// 本月完成工程量
            /// </summary>
            public decimal AccomplishQuantities { get; set; }

            /// <summary>
            /// 本月完成产值
            /// </summary>
            public decimal AccomplishValue { get; set; }

            /// <summary>
            /// 本月甲方确认产值
            /// </summary>

            public decimal RecognizedValue { get; set; }

            /// <summary>
            /// 本月付款金额
            /// </summary>
            public decimal PaymentAmount { get; set; }

            /// <summary>
            /// 本月外包支出
            /// </summary>
            public decimal OutsourcingExpensesAmount { get; set; }
        }


        /// <summary>
        /// 项目月报响应Dto
        /// </summary>
        public class MonthtReportDto
        {
            /// <summary>
            /// 是否显示撤回按钮
            /// </summary>
            public bool IsShowRecall { get; set; }
            /// <summary>
            /// Id
            /// </summary>
            public Guid Id { get; set; }
            /// <summary>
            /// 项目id
            /// </summary>
            public Guid ProjectId { get; set; }

            /// <summary>
            /// 填报月份
            /// </summary>
            public int DateMonth { get; set; }

            /// <summary>
            /// 填报月份(时间格式)
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
            /// 填报月份(字符串格式)
            /// </summary>
            public string StrMonthTime
            {
                get
                {

                    return DateMonthTime.ToString("yyyy-MM");
                }
            }

            /// <summary>
            /// 项目类型Id
            /// </summary>
            public Guid? ProjectTypeId { get; set; }

            /// <summary>
            /// 项目汇率
            /// </summary>
            public decimal? ExchangeRate { get; set; }

            /// <summary>
            /// 传统、新兴工程类别标签（0：传统；1：新兴）
            /// </summary>
            public int Tag { get; set; }

            /// <summary>
            /// 现汇、投资工程类别标签（0：现汇；1：投资）
            /// </summary>
            public int Tag2 { get; set; }

            /// <summary>
            /// 项目类别 0 境内  1 境外
            /// </summary>
            public int Category { get; set; }

            /// <summary>
            /// 施工地点Id
            /// </summary>
            public Guid? AreaId { get; set; }

            /// <summary>
            /// 施工区域 id
            /// </summary>
            public Guid? RegionId { get; set; }

            /// <summary>
            /// 公司id
            /// </summary>
            public Guid? CompanyId { get; set; }
            /// <summary>
            /// 项目名称
            /// </summary>
            public string? Name { get; set; }

            /// <summary>
            /// 项目状态Id
            /// </summary>
            public Guid? StatusId { get; set; }

            /// <summary>
            /// 项目施工资质Id
            /// </summary>
            public Guid? ConstructionQualificationId { get; set; }

            /// <summary>
            /// 行业分类标准
            /// </summary>
            public string? ClassifyStandard { get; set; }

            /// <summary>
            /// 业务分类
            /// </summary>
            public string? ProjectType { get; set; }
            /// <summary>
            /// 现汇投资
            /// </summary>
            public string? Label { get { return Tag2 == 0 ? "现汇" : "投资"; } }
            /// <summary>
            /// 在建完工
            /// </summary>
            public string? State { get; set; }
            /// <summary>
            /// 所属区域
            /// </summary>
            public string? Area { get; set; }
            /// <summary>
            /// (省域/国别)
            /// </summary>
            public string? Nationality { get; set; }
            /// <summary>
            /// 市区
            /// </summary>
            public string? District { get; set; }

            /// <summary>
            /// 施工资质
            /// </summary>
            public string? Construction { get; set; }
            /// <summary>
            /// 合同额(元)
            /// </summary>
            public decimal? ContractAmount { get; set; }
            /// <summary>
            /// 有效合同额
            /// </summary>
            public decimal? ValidContractAmount { get; set; }

            /// <summary>
            /// 剩余合同额(合同额-累计产值)
            /// </summary>
            public decimal ResidueContractAmount { get; set; }

            /// <summary>
            /// 主要工程量概况说明
            /// </summary>
            public string? Remarks { get; set; }

            /// <summary>
            /// 本月计划工程量
            /// </summary>
            public decimal PlanQuantities { get; set; }
            /// <summary>
            /// 本月完成工程量
            /// </summary>
            public decimal AccomplishQuantities { get; set; }
            /// <summary>
            /// 本月进度偏差(本月计划工程量-本月完成工程量)
            /// </summary>
            public decimal DeviationQuantities { get; set; }
            /// <summary>
            /// 年度计划工程量
            /// </summary>
            public decimal YearPlanQuantities { get; set; }
            /// <summary>
            /// 年度完成工程量
            /// </summary>
            public decimal YearAccomplishQuantities { get; set; }
            /// <summary>
            /// 年度进度偏差
            /// </summary>
            public decimal YearDeviation { get; set; }
            /// <summary>
            /// 累计完成工程量
            /// </summary>
            public decimal AccumulativeQuantities { get; set; }
            /// <summary>
            /// 本月计划产值
            /// </summary>
            public decimal PlanOutPutValue { get; set; }
            /// <summary>
            /// 本月完成产值
            /// </summary>
            public decimal AccomplishValue { get; set; }

            /// <summary>
            /// 本月产值进度偏差
            /// </summary>
            public decimal ScheduleOutPutValue { get; set; }

            /// <summary>
            /// 主要形象进度描述
            /// </summary>
            public string? ProgressDescription { get; set; }
            /// <summary>
            /// 年度计划产值
            /// </summary>
            public decimal YearPlanOutPutValue { get; set; }
            /// <summary>
            /// 年度完成产值
            /// </summary>
            public decimal YearAccomplishValue { get; set; }

            /// <summary>
            /// 年度产值偏差
            /// </summary>
            public decimal YearAnnualOutputValue { get; set; }

            /// <summary>
            /// 累计完成产值
            /// </summary>
            public decimal CumulativeCompleted { get; set; }

            /// <summary>
            /// 甲方确认产值
            /// </summary>
            public decimal RecognizedValue { get; set; }
            /// <summary>
            /// 年度甲方确认产值
            /// </summary>
            public decimal YearRecognizedValue { get; set; }

            /// <summary>
            /// 累计甲方确认产值
            /// </summary>
            public decimal CumulativeValue { get; set; }

            /// <summary>
            /// 付款金额
            /// </summary>
            public decimal PaymentAmount { get; set; }
            /// <summary>
            /// 年付款金额
            /// </summary>
            public decimal YearPaymentAmount { get; set; }

            /// <summary>
            /// 累计付款金额
            /// </summary>
            public decimal CumulativePaymentAmount { get; set; }

            /// <summary>
            /// 进度偏差主因
            /// </summary>
            public DeviationReason ProgressDeviationReasonType { get; set; }

            /// <summary>
            ///进度偏差原因简述推送pom 
            /// </summary>
            public string? ProgressDeviationDescriptionPushPom { get; set; }
            /// <summary>
            /// 主要形象进度描述推送pom
            /// </summary>
            public string? ProgressDescriptionPushPom { get; set; }

            /// <summary>
            /// 推送状态
            /// </summary>
            public int PushStatus { get; set; }

            /// <summary>
            /// 当前登陆人是否是陈翠
            /// </summary>
            public bool IsAdmin { get; set; }


            /// <summary>
            /// 进度偏差主因
            /// </summary>
            public string? ProgressDeviationReason { get { return EnumExtension.GetEnumDescription(ProgressDeviationReasonType); } }

            /// <summary>
            /// 进度偏差原因简述
            /// </summary>
            public string? ProgressDeviationDescription { get; set; }

            /// <summary>
            /// 标后预算毛利率
            /// </summary>
            public decimal? BudgetInterestRate { get; set; }

            /// <summary>
            /// 未编制标后预算原因
            /// </summary>
            public string? BudgetaryReasons { get; set; }

            /// <summary>
            /// 计划完成编制时间
            /// </summary>
            public DateTime? CompilationTime { get; set; }

            /// <summary>
            /// 本月预计成本
            /// </summary>
            public decimal ProjectedCost { get; set; }

            /// <summary>
            /// 本月完成成本
            /// </summary>
            public decimal AccomplishCost { get; set; }
            /// <summary>
            /// 本月成本偏差
            /// </summary>
            public decimal CostDeviation { get; set; }

            /// <summary>
            /// 年度预计成本
            /// </summary>
            public decimal YearProjectedCost { get; set; }
            /// <summary>
            /// 年度完成成本
            /// </summary>
            public decimal YearAccomplishCost { get; set; }
            /// <summary>
            /// 年度成本偏差
            /// </summary>
            public decimal YearCostDeviation { get; set; }

            /// <summary>
            /// 标后预算 预计成本
            /// </summary>
            public decimal PostmarkProjectedCost { get; set; }

            /// <summary>
            /// 项目累计完成成本
            /// </summary>
            public decimal CumulativeAccomplishCost { get; set; }

            /// <summary>
            /// 标后预算成本偏差
            /// </summary>
            public decimal PostmarkCostDeviation { get; set; }

            /// <summary>
            /// 实际毛利率
            /// </summary>
            public decimal EffectiveGrossMargin { get; set; }

            /// <summary>
            /// 毛利率偏差((实际毛利率-标后预算毛利率)/标后预算毛利率)
            /// </summary>
            public decimal? GrossMarginDeviation { get; set; }

            /// <summary>
            /// 成本偏差主因（枚举）
            /// </summary>
            public DeviationReason CostDeviationReasonType { get; set; }

            /// <summary>
            /// 成本偏差主因（文本）
            /// </summary>
            public string CostDeviationReason { get { return EnumExtension.GetEnumDescription(CostDeviationReasonType); } }

            /// <summary>
            /// 成本偏差原因简述
            /// </summary>
            public string? CostDeviationDescription { get; set; }

            /// <summary>
            /// 二级单位
            /// </summary>
            public string? TweUnit { get; set; }

            /// <summary>
            /// 三级单位
            /// </summary>
            public string? ThreeUnit { get; set; }

            /// <summary>
            /// 管理人员
            /// </summary>
            public int AdministrationNumber { get; set; }

            /// <summary>
            /// 施工人员
            /// </summary>
            public int ConstructionNumber { get; set; }

            /// <summary>
            /// 业主单位名称
            /// </summary>
            public string? OwnerUnitName { get; set; }

            /// <summary>
            /// 新签合同乙方名称
            /// </summary>
            public string? SecondPartyName { get; set; }

            /// <summary>
            /// 甲方单位
            /// </summary>
            public string? FirstPartyUnit { get; set; }

            /// <summary>
            /// 负责人
            /// </summary>
            public string? PersonAdministration { get; set; }
            /// <summary>
            /// 联系方式
            /// </summary>
            public string? PhoneAdministration { get; set; }

            /// <summary>
            /// 报表负责人
            /// </summary>
            public string? PersonReportForms { get; set; }
            /// <summary>
            /// 报表联系方式
            /// </summary>
            public string? PhoneReportForms { get; set; }

            /// <summary>
            /// 一级行业分类代码
            /// </summary>
            public string? OneCode { get; set; }
            /// <summary>
            /// 一级行业分类
            /// </summary>
            public string? OneClassify { get; set; }

            /// <summary>
            /// 三级行业分类代码
            /// </summary>
            public string? TweCode { get; set; }
            /// <summary>
            /// 二级行业分类
            /// </summary>
            public string? TweClassify { get; set; }
            /// <summary>
            /// 三级行业分类代码
            /// </summary>
            public string? ThreeCode { get; set; }

            /// <summary>
            /// 三级行业分类
            /// </summary>
            public string? ThreeClassify { get; set; }

            /// <summary>
            /// 编码
            /// </summary>
            public string? Code { get; set; }

            /// <summary>
            ///  合同约定的计量支付比例(%)
            /// </summary>
            public decimal ContractMeaPayProp { get; set; }

            /// <summary>
            /// 支付滞后比例
            /// </summary>
            public decimal? PaymentLagRatio { get; set; }

            /// <summary>
            /// 税率
            /// </summary>
            public decimal? TaxRate { get; set; }

            /// <summary>
            /// 状态文本
            /// </summary>
            public string? StatusText { get; set; }

            /// <summary>
            /// 本月外包支出（元）
            /// </summary>
            public decimal OutsourcingExpensesAmount { get; set; }

            /// <summary>
            /// 本年外包支出（元）
            /// </summary>
            public decimal YearOutsourcingExpensesAmount { get; set; }

            /// <summary>
            /// 累计外包支出（元）
            /// </summary>
            public decimal CumulativeOutsourcingExpensesAmount { get; set; }
        }
    }

}
