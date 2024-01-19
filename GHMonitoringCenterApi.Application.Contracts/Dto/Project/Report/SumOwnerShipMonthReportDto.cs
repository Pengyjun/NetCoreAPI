using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report
{
    /// <summary>
    /// 统计船舶月报
    /// </summary>
    public  class SumOwnerShipMonthReportDto
    {
        /// <summary>
        /// 项目Id
        /// </summary>
        public Guid ProjectId { get; set; }

        /// <summary>
        /// 自有船舶Id
        /// </summary>
        public Guid ShipId { get; set; }

        /// <summary>
        /// 产量（方）
        /// </summary>
        public decimal Production { get; set; }

        /// <summary>
        /// 产值（元）
        /// </summary>
        public decimal ProductionAmount { get; set; }
    }
}
