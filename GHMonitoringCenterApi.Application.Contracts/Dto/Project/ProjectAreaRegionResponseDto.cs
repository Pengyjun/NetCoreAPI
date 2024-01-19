using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project
{
    /// <summary>
    /// 地点区域响应类
    /// </summary>
    public class ProjectAreaRegionResponseDto
    {
        /// <summary>
        /// 地点Id
        /// </summary>
        public Guid? AreaId { get; set; }
        /// <summary>
        /// 地点名称
        /// </summary>
        public string? AreaName { get; set; }
        /// <summary>
        /// 区域Id
        /// </summary>
        public Guid? RegionId { get; set; }
        /// <summary>
        /// 区域名称
        /// </summary>
        public string? RegionName { get; set; }
    }
}
