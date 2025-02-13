using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.ShipPlan
{

    /// <summary>
    /// 根据船舶计划产值生成图的响应DTO
    /// </summary>
    public class ShipPlanImageResponseDto
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Guid  Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 历时天数
        /// </summary>
        public int Days { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string? StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string? EndTime { get; set; }
        /// <summary>
        /// 资源名称
        /// </summary>
        public string? ShipName { get; set; }
    }
}
