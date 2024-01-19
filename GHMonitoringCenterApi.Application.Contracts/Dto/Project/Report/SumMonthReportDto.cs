using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report
{
    /// <summary>
    ///项目月报根据项目统计Dto
    /// </summary>
    public class SumMonthReportDto
    {
        /// <summary>
        /// 项目id
        /// </summary>
        public Guid ProjectId { get; set; }

        /// <summary>
        /// 填报年份
        /// </summary>
        public int DateYear { get; set; }

        /// <summary>
        /// 完成工程量(方)
        /// </summary>
        public decimal CompletedQuantity { get; set; }

        /// <summary>
        /// 完成产值（元）
        /// </summary>
        public decimal CompleteProductionAmount { get; set; }

        /// <summary>
        /// 甲方确认产值（元）
        /// </summary>
        public decimal PartyAConfirmedProductionAmount { get; set; }

        /// <summary>
        /// 甲方付款金额(元)
        /// </summary>
        public decimal PartyAPayAmount { get; set; }

        /// <summary>
        /// 实际成本（元）
        /// </summary>
        public decimal CostAmount { get; set; }

        /// <summary>
        /// 估算成本（元）
        /// </summary>
        public decimal EstimateCostAmount { get; set; }

        /// <summary>
        ///  完成产值（元，美元，欧元等）
        /// </summary>
        public decimal CurrencyCompleteProductionAmount { get; set; }

        /// <summary>
        /// 外包支出
        /// </summary>
        public decimal OutsourcingExpensesAmount { get; set; }
    }
}
