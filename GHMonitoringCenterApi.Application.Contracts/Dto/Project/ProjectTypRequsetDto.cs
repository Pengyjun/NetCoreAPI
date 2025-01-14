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

        /// <summary>
        /// 启动备注模式  1是启动   其他未启动
        /// </summary>
        public int EnableRemark { get; set; }
    }
}
