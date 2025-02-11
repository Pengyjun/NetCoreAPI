using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.ShipPlan
{

    /// <summary>
    /// 船舶年度计划请求DTO
    /// </summary>
    public class ShipPlanRequestDto:BaseRequestDto
    {

        /// <summary>
        /// 船舶名称
        /// </summary>

        public string? ShipName { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>

        public string? ProjectName { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>

        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>

        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 年份
        /// </summary>

        public int? Year { get; set; } = DateTime.Now.Year;
    }
}
