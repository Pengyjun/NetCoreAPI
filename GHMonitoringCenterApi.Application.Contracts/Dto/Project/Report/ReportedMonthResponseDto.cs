using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report
{
    /// <summary>
    /// 已填报月份返回对象
    /// </summary>
    public class ReportedMonthResponseDto
    {
        /// <summary>
        /// 当前最大填报月份
        /// </summary>
        public string MaxReportMonth { get; set; }

        /// <summary>
        /// 已填报月份集合
        /// </summary>
        public List<string> ReportedMonth { get; set; }
    }
}
