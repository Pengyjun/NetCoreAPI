using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.SystemUpdatLog
{
    /// <summary>
    /// 查询系统日志相应Dto
    /// </summary>
    public class SearchSystemUpdatLogRequsetDto: BaseRequestDto
    {
        
        /// <summary>
        /// 起始时间
        /// </summary>       
        public DateTime? StartingTime { get; set; }
        
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set;}
    }
}
