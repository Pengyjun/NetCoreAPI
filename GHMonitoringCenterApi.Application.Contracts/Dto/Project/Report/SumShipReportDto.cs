using GHMonitoringCenterApi.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report
{
    /// <summary>
    /// 统计船舶dto
    /// </summary>
    public  class SumShipReportDto
    {
        /// <summary>
        /// 项目Id
        /// </summary>
        public Guid ProjectId { get; set; }

        /// <summary>
        /// 船舶Id
        /// </summary>
        public Guid ShipId { get; set; }

        /// <summary>
        /// 船舶类型
        /// </summary>
        public ShipType ShipType { get; set; }

        /// <summary>
        /// 生产运转（小时）
        /// </summary>
        public decimal WorkingHours { get; set; }
}
}
