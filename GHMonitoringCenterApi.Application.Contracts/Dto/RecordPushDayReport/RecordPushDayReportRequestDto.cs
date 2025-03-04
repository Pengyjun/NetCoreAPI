using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.RecordPushDayReport
{
    /// <summary>
    /// 每天推送日报结果记录
    /// </summary>
    public class RecordPushDayReportRequestDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string StartDateDay { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string EndDateDay { get; set; }
    }
}
