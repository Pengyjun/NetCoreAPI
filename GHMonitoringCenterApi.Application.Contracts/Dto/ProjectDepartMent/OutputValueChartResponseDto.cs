using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.ProjectDepartMent
{
    /// <summary>
    /// 项目部产值chart图
    /// </summary>
    public class OutputValueChartResponseDto
    {
        /// <summary>
        /// 计划产值（万元）
        /// </summary>
        public decimal PlanValue { get; set; }
        /// <summary>
        /// 实际产值（万元）
        /// </summary>
        public decimal ActualValue { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public string DateValue { get; set; }
    }
}
