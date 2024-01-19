using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report
{
    /// <summary>
    /// 统计项目计划Dto
    /// </summary>
    public  class SumProjectPlannedDto
    {
        /// <summary>
        /// 计划产值(元)
        /// </summary>
        public decimal PlannedOutputValue { get; set; }

        /// <summary>
        /// 计划工程量（方）
        /// </summary>
        public decimal PlannedQuantities { get; set; }
    }
}
