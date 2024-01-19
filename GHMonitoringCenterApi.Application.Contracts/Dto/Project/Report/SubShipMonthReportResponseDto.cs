using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report
{
    /// <summary>
    /// 分包船舶月报返回对象
    /// </summary>
    public  class SubShipMonthReportResponseDto:SubShipMonthReportDto
    {


        /// <summary>
        /// 填报月份时间格式（前端可以根据需求获取年，月拼接）
        /// </summary>
        public DateTime DateMonthTime
        {
            get
            {
                ConvertHelper.TryParseFromDateMonth(DateMonth, out DateTime monthTime);
                return monthTime;
            }
        }

    }
}
