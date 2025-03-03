using SqlSugar;
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
    public class RecordPushDayReportResponseDto
    {
        /// <summary>
        /// 日期
        /// </summary>
        public int DateDay { get; set; }
        /// <summary>
        /// json结果集
        /// </summary>
        public string? Json { get; set; }

        public int Index { get; set; }
    }
}
