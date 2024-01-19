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
    /// 日报统计
    /// </summary>
    public  class SumDayReportDto
    {
        /// <summary>
        /// 项目Id
        /// </summary>
        public Guid ProjectId { get; set; }

        /// <summary>
        /// 填报日期
        /// </summary>
        public int DateDay { get; set; }

        /// <summary>
        /// 项目分类Id
        /// </summary>
        public Guid ProjectWBSId { get; set; }

        /// <summary>
        /// 船舶Id
        /// </summary>
        public Guid? ShipId { get; set; }

        /// <summary>
        /// 产值属性
        /// </summary>
        public ConstructionOutPutType OutPutType { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// 外包支出
        /// </summary>
        public decimal OutsourcingExpensesAmount { get; set; }

        /// <summary>
        /// 施工性质
        /// </summary>
        public int? ConstructionNature { get; set; }

        /// <summary>
        /// 产量
        /// </summary>
        public decimal Production { get; set; }

        /// <summary>
        /// 产值金额（元）
        /// </summary>
        public decimal ProductionAmount { get; set; }

        /// <summary>
        /// 产值金额(币种：元,美元，欧元)
        /// </summary>
        public decimal CProductionAmount { get; set; }

        /// <summary>
        /// 已完成合同额度(元)
        /// </summary>
        public decimal CompleteContractAmount { get; set; }
    }
}
