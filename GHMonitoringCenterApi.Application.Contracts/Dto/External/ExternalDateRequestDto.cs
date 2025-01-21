using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.External
{
    /// <summary>
    /// 对外接口请求类  范围查询
    /// </summary>
    public class ExternalDateRequestDto : BaseRequestDto
    {
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
    }
}
