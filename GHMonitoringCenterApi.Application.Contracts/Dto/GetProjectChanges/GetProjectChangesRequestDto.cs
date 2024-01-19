using GHMonitoringCenterApi.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.GetProjectChanges
{
    /// <summary>
    /// 请求Dto
    /// </summary>
    public class GetProjectChangesRequestDto: BaseRequestDto
    {
        /// <summary>
        /// 项目Id
        /// </summary>
        public string? ProjectId { get; set; }
        /// <summary>
        /// 模块名称
        /// </summary>
        public LogSystemModuleType ModuleType { get; set; }
    }
}
