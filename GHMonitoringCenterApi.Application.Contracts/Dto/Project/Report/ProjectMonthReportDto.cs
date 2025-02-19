using GHMonitoringCenterApi.Application.Contracts.Dto.Enums;
using GHMonitoringCenterApi.Domain.Enums;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report
{
    /// <summary>
    /// 项目月报Dto
    /// </summary>
    public abstract class ProjectMonthReportDto<TTreeDetail, TDetail> where TTreeDetail : TreeProjectWBSDetailDto<TTreeDetail, TDetail> where TDetail : MonthReportDetailDto<TDetail>
    {
        /// <summary>
        /// 产值累计
        /// </summary>
        public decimal TotalProductionAmount { get; set; }

        /// <summary>
        /// 项目Id
        /// </summary>
        public Guid ProjectId { get; set; }

        /// <summary>
        /// 填报月份（例：202304，注解：上月的26-本月的25，例：2023.3.26-2023.04.25）
        /// </summary>
        public int DateMonth { get; set; }

        /// <summary>
        /// 本月外包支出
        /// </summary>
        public decimal? OutsourcingExpensesAmount { get; set; }

        /// <summary>
        /// 本月完成工程量(方)
        /// </summary>
        public decimal? CompletedQuantity { get; set; }

        /// <summary>
        /// 本月完成产值
        /// </summary>
        public decimal? CompleteProductionAmount { get; set; }


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

        /// <summary>
        /// 树状明细
        /// </summary>
        public TTreeDetail[] TreeDetails { get; set; } = new TTreeDetail[0];

        /// <summary>
        /// 解析所有明细集合
        /// </summary>
        /// <returns></returns>
        public List<TDetail> ResolveDetails()
        {
            var reqDetails = new List<TDetail>();
            MapDetails(TreeDetails, reqDetails);
            return reqDetails;
        }

        /// <summary>
        /// 适配返回明细集合
        /// </summary>
        private void MapDetails(TTreeDetail[] nodes, List<TDetail> resDetails)
        {
            foreach (var node in nodes)
            {
                resDetails.AddRange(node.ReportDetails ?? new List<TDetail>());
                if (node.Children != null && node.Children.Any())
                {
                    MapDetails(node.Children, resDetails);
                }
            }
        }
    }
    /// <summary>
    /// 树状WBS
    /// </summary>
    public abstract class TreeProjectWBSDetailDto<TTreeDetail, TDetail> where TTreeDetail : TreeProjectWBSDetailDto<TTreeDetail, TDetail> where TDetail : MonthReportDetailDto<TDetail>
    {
        /// <summary>
        /// 施工分类Id
        /// </summary>
        public Guid ProjectWBSId { get; set; }

        /// <summary>
        /// keyId
        /// </summary>
        public string? KeyId { get; set; }

        /// <summary>
        /// Pid
        /// </summary>
        public string? Pid { get; set; }

        /// <summary>
        /// 施工分类名称
        /// </summary>
        public string? ProjectWBSName { get; set; }

        /// <summary>
        /// 单价(元)
        /// </summary>
        public decimal? UnitPrice { get; set; }

        /// <summary>
        /// 合同工程量
        /// </summary>
        public decimal? ContractQuantity { get; set; }

        /// <summary>
        /// 合同额
        /// </summary>
        public decimal? ContractAmount { get; set; }


        /// <summary>
        /// 子级集合
        /// </summary>
        public TTreeDetail[] Children { get; set; } = new TTreeDetail[0];

        /// <summary>
        /// 月报明细
        /// </summary>
        public List<TDetail> ReportDetails { get; set; } = new List<TDetail>();

    }

    /// <summary>
    /// 项目月报树状明细
    /// </summary>
    public abstract class MonthReportDetailDto<TDetail> where TDetail : MonthReportDetailDto<TDetail>
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
        /// <summary>
        /// 明细Id
        /// </summary>
        public Guid? DetailId { get; set; }

        /// <summary>
        /// 施工分类Id
        /// </summary>
        public Guid ProjectWBSId { get; set; }

        /// <summary>
        /// 产值属性(1:自有，2：分包，4：分包-自有)
        /// </summary>
        public ConstructionOutPutType? OutPutType { get; set; }

        /// <summary>
        /// 船舶Id(PomId)
        /// </summary>
        public Guid? ShipId { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public decimal? UnitPrice { get; set; } = 0M;

        /// <summary>
        /// 施工性质
        /// </summary>
        public int? ConstructionNature { get; set; }

        /// <summary>
        /// 合同工程量
        /// </summary>
        public decimal? ContractQuantity { get; set; }

        /// <summary>
        /// 合同额
        /// </summary>
        public decimal? ContractAmount { get; set; }

        /// <summary>
        /// 本月外包支出
        /// </summary>
        public decimal? OutsourcingExpensesAmount { get; set; }

        /// <summary>
        /// 本月完成工程量(方)
        /// </summary>
        public decimal? CompletedQuantity { get; set; } = 0M;

        /// <summary>
        /// 本月完成产值
        /// </summary>
        public decimal? CompleteProductionAmount { get; set; }

        /// <summary>
        /// 船舶名称
        /// </summary>
        public string? ShipName { get; set; }

        /// <summary>
        /// 施工性质名称
        /// </summary>
        public string? ConstructionNatureName { get; set; }

        /// <summary>
        /// 本年外包支出(元)
        /// </summary>
        public decimal YearOutsourcingExpensesAmount { get; set; }

        /// <summary>
        /// 本年完成工程量(方)
        /// </summary>
        public decimal YearCompletedQuantity { get; set; }

        /// <summary>
        /// 本年完成产值（元）
        /// </summary>
        public decimal YearCompleteProductionAmount { get; set; }

        /// <summary>
        /// 累计外包支出(元)
        /// </summary>
        public decimal TotalOutsourcingExpensesAmount { get; set; }

        /// <summary>
        /// 累计完成工程量(方)
        /// </summary>
        public decimal TotalCompletedQuantity { get; set; }

        /// <summary>
        /// 累计完成产值（元）
        /// </summary>
        public decimal TotalCompleteProductionAmount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsAllowDelete { get; set; }
    }
}
