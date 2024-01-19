using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GHMonitoringCenterApi.Domain.Enums;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project
{
    /// <summary>
    /// 安监日报返回结果
    /// </summary>
    public class SafeSupervisionDayReportResponseDto:SafeSupervisionDayReportDto
    {
        /// <summary>
        /// 填报日期(时间格式)
        /// </summary>
        public DateTime? DateDayTime
        {
            get
            {
                if (ConvertHelper.TryConvertDateTimeFromDateDay(DateDay, out DateTime dayTime))
                {
                    return dayTime;
                }
                return null;
            }
        }

        /// <summary>
        /// 是否可提交
        /// </summary>
        public bool IsCanSubmit { get; set; }
    }
}
