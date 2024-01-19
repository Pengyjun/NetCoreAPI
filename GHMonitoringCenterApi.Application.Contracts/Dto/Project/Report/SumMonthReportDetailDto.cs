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
    /// 项目月报明细统计
    /// </summary>
    public  class SumMonthReportDetailDto
    {
        /// <summary>
        /// 项目Id
        /// </summary>
        public Guid ProjectId { get; set; }

        /// <summary>
        /// 年份
        /// </summary>
        public int DateYear { get; set; }

        /// <summary>
        /// 月份
        /// </summary>
        public int DateMonth { get; set; }

        /// <summary>
        /// 施工分类Id
        /// </summary>
        public Guid ProjectWBSId { get; set; }

        /// <summary>
        /// 产值属性(1:自有，2：分包，4：分包-自有)
        /// </summary>
        public ConstructionOutPutType OutPutType { get; set; }

        /// <summary>
        /// 船舶Id(PomId)
        /// </summary>
        public Guid? ShipId { get; set; }

        /// <summary>
        /// 单价(元)
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// 施工性质
        /// </summary>
        public int? ConstructionNature { get; set; }

        /// <summary>
        /// 外包支出(元)
        /// </summary>
        public decimal OutsourcingExpensesAmount { get; set; }

        /// <summary>
        /// 完成工程量(方)
        /// </summary>
        public decimal CompletedQuantity { get; set; }

        /// <summary>
        /// 完成产值（元）
        /// </summary>
        public decimal CompleteProductionAmount { get; set; }

        /// <summary>
        /// 完成产值（元/美元/欧元等）
        /// </summary>
        public decimal CurrencyCompleteProductionAmount { get; set; }
    }
}
