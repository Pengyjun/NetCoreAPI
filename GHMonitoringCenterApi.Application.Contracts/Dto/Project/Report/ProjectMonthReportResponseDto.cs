using GHMonitoringCenterApi.Application.Contracts.Dto.Enums;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilsSharp;
using static GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report.ProjectMonthReportResponseDto;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report
{
    /// <summary>
    /// 项目月报返回结果
    /// </summary>
    public class ProjectMonthReportResponseDto : ProjectMonthReportDto<ResTreeProjectWBSDetailDto, ResMonthReportDetail>
    {

        #region 新增字段
        /// <summary>
        /// 本年甲方确认产值
        /// </summary>

        public decimal CurrentYearOffirmProductionValue { get; set; }
        /// <summary>
        /// 开累甲方确认产值
        /// </summary>
        public decimal TotalYearKaileaOffirmProductionValue { get; set; }
        /// <summary>
        /// 本年甲方付款金额 
        /// </summary>
        public decimal CurrenYearCollection { get; set; }
        /// <summary>
        /// 开累甲方付款金额
        /// </summary>
        public decimal TotalYearCollection { get; set; }
        #endregion

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
        public decimal? ContractQuantity
        {
            get
            {
                return TreeDetails.Sum(t => t.ContractQuantity ?? 0);
            }
        }

        /// <summary>
        /// 合同额
        /// </summary>
        public decimal? ContractAmount
        {
            get
            {
                return TreeDetails.Sum(t => t.ContractAmount ?? 0);
            }
        }

       

        /// <summary>
        /// 本年外包支出(元)
        /// </summary>
        public decimal YearOutsourcingExpensesAmount
        {
            get
            {
                return TreeDetails.Sum(t => t.YearOutsourcingExpensesAmount);
            }
        }
        /// <summary>
        /// 本年完成工程量(方)
        /// </summary>
        public decimal YearCompletedQuantity
        {
            get
            {
                return TreeDetails.Sum(t => t.YearCompletedQuantity);
            }
        }

        /// <summary>
        /// 本年完成产值（元）
        /// </summary>
        public decimal YearCompleteProductionAmount
        {
            get
            {
                return TreeDetails.Sum(t => t.YearCompleteProductionAmount);
            }
        }

        /// <summary>
        /// 累计外包支出(元)
        /// </summary>
        public decimal TotalOutsourcingExpensesAmount
        {
            get
            {
                return TreeDetails.Sum(t => t.TotalOutsourcingExpensesAmount); ;
            }
        }

        /// <summary>
        /// 累计完成工程量(方)
        /// </summary>
        public decimal TotalCompletedQuantity
        {
            get
            {
                return TreeDetails.Sum(t => t.TotalCompletedQuantity);
            }
        }

        /// <summary>
        /// 累计完成产值（元）
        /// </summary>
        public decimal TotalCompleteProductionAmount
        {
            get
            {
                return TreeDetails.Sum(t => t.TotalCompleteProductionAmount);
            }
        }

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
        /// 树状明细
        /// </summary>
        public class ResTreeProjectWBSDetailDto : TreeProjectWBSDetailDto<ResTreeProjectWBSDetailDto, ResMonthReportDetail>
        {



            

            /// <summary>
            /// 本月外包支出
            /// </summary>
            public decimal OutsourcingExpensesAmount
            {
                get
                {
                    return Children.Sum(t => t.OutsourcingExpensesAmount) + ReportDetails.Sum(t => t.OutsourcingExpensesAmount ?? 0);
                }
            }

            /// <summary>
            /// 本月完成工程量(方
            /// </summary>
            public decimal CompletedQuantity
            {
                get
                {
                    return Children.Sum(t => t.CompletedQuantity) + ReportDetails.Sum(t => t.CompletedQuantity ?? 0);
                }
            }

            /// <summary>
            /// 本月完成产值
            /// </summary>
            public decimal CompleteProductionAmount
            {
                get
                {
                    return Children.Sum(t => t.CompleteProductionAmount) + ReportDetails.Sum(t => t.CompleteProductionAmount ?? 0);
                }
            }

            /// <summary>
            /// 本年外包支出
            /// </summary>
            public decimal YearOutsourcingExpensesAmount
            {
                get
                {
                    return ReportDetails.Sum(t => t.YearOutsourcingExpensesAmount) + Children.Sum(t => t.YearOutsourcingExpensesAmount);
                }
            }
            /// <summary>
            /// 本年完成工程量(方)
            /// </summary>
            public decimal YearCompletedQuantity
            {
                get
                {
                    return ReportDetails.Sum(t => t.YearCompletedQuantity) + Children.Sum(t => t.YearCompletedQuantity);
                }
            }

            /// <summary>
            /// 本年完成产值（元）
            /// </summary>
            public decimal YearCompleteProductionAmount
            {
                get
                {
                    return ReportDetails.Sum(t => t.YearCompleteProductionAmount) + Children.Sum(t => t.YearCompleteProductionAmount);
                }
            }

            /// <summary>
            /// 累计外包支出(元)
            /// </summary>
            public decimal TotalOutsourcingExpensesAmount
            {
                get
                {
                    return ReportDetails.Sum(t => t.TotalOutsourcingExpensesAmount) + Children.Sum(t => t.TotalOutsourcingExpensesAmount);
                }
            }

            /// <summary>
            /// 累计完成工程量(方)
            /// </summary>
            public decimal TotalCompletedQuantity
            {
                get
                {
                    return ReportDetails.Sum(t => t.TotalCompletedQuantity) + Children.Sum(t => t.TotalCompletedQuantity);
                }
            }

            /// <summary>
            /// 累计完成产值（元）
            /// </summary>
            public decimal TotalCompleteProductionAmount
            {
                get
                {
                    return ReportDetails.Sum(t => t.TotalCompleteProductionAmount) + Children.Sum(t => t.TotalCompleteProductionAmount);
                }
            }

            /// <summary>
            /// 最后一级是否存在填报数据
            /// </summary>
            public bool IsHasReportDetails
            {
                get
                {
                    // 最后一级有填报数据
                    if((!Children.Any())&& ReportDetails.Any())
                    {
                        return true;
                    }
                    else if(Children.Any(t=>t.IsHasReportDetails))
                    {
                        return true;
                    }
                    return false;
                }
            }
        }

        /// <summary>
        /// 项目月报明细
        /// </summary>
        public class ResMonthReportDetail : MonthReportDetailDto<ResMonthReportDetail>
        {

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
        }

    }
}
