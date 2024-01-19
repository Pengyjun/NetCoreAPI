using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.ConstructionLog
{
    /// <summary>
    /// 获取已填报日志日期响应Dto
    /// </summary>
    public class SearchCompletedConstructionLogRequestDto
    {
        /// <summary>
        /// 已填报日志日期
        /// </summary>
        public string? Time { get; set; }
        /// <summary>
        /// 已填报日志日期DateDay
        /// </summary>
        public int? DateDayTime { get; set; }


    }
}
