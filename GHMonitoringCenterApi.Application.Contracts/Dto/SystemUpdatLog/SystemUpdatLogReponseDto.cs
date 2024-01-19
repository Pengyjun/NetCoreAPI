using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.SystemUpdatLog
{
    /// <summary>
    /// 系统日志返回Dto
    /// </summary>
    public class SystemUpdatLogReponseDto
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Time { get; set; }
        /// <summary>
        /// 系统日志内容
        /// </summary>
        public string? LogContent { get; set; }
    }
    
}
