using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project
{
    /// <summary>
    /// 请求Dto
    /// </summary>
    public class ProjectTypRequsetDto
    {
        /// <summary>
        /// 模糊查询
        /// </summary>
        public string? Name { get; set; }
    }
}
