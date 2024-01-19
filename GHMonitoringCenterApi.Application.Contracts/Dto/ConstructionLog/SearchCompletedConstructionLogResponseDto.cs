using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.ConstructionLog
{
    /// <summary>
    /// 获取已填报日志日期请求Dto
    /// </summary>
    public class SearchCompletedConstructionLogResponseDto
    {
        /// <summary>
        /// 项目Id
        /// </summary>
        public Guid? ProjectId { get; set; }
        /// <summary>
        /// 年月日
        /// </summary>
        public DateTime? Time { get; set; }
    }
}
