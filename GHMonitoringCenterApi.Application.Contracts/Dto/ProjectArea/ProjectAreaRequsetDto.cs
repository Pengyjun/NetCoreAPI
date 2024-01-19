using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.ProjectArea
{
    /// <summary>
    /// 项目区域请求Dto
    /// </summary>
    public class ProjectAreaRequsetDto
    {
        /// <summary>
        /// 模糊查询
        /// </summary>
        public string? Name { get; set; }
    }
}
