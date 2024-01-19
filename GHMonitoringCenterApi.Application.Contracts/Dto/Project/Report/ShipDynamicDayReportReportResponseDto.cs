using GHMonitoringCenterApi.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report
{
    /// <summary>
    /// 船舶动态日报结果对象
    /// </summary>
    public  class ShipDynamicDayReportReportResponseDto
    {
        /// <summary>
        /// 填报日期（例：20230401）
        /// </summary>
        public int DateDay { get; set; }

        /// <summary>
        /// 船舶Id（PomId）
        /// </summary>
        public Guid ShipId { get; set; }

        /// <summary>
        /// 船舶状态
        /// </summary>
        public DynamicShipState? ShipState { get; set; }

        /// <summary>
        /// 本月实际目的港Id
        /// </summary>
        public Guid? MonthDestinationPortId { get; set; }

        /// <summary>
        /// 本月实际出发港Id
        /// </summary>
        public Guid? MonthDeparturePortId { get; set; }

        /// <summary>
        /// 本月实际航行距离
        /// </summary>
        public decimal? MonthNavigationalDistance { get; set; }

        /// <summary>
        /// 本月实际调遣小时
        /// </summary>
        public decimal? MonthDispatchsHours { get; set; }

        /// <summary>
        /// 本月实际拖轮/半潜船名称
        /// </summary>
        public string? MonthShipName { get; set; }

        /// <summary>
        /// 修理所在港口Id
        /// </summary>
        public Guid? RepairByPortId { get; set; }

        /// <summary>
        /// 待命所在港口Id
        /// </summary>
        public Guid? StandbyPortId { get; set; }

        /// <summary>
        /// 本月实际目的港名称
        /// </summary>
        public string? MonthDestinationPortName { get; set; }

        /// <summary>
        /// 本月实际出发港名称
        /// </summary>
        public string  ? MonthDeparturePortName { get; set; }

        /// <summary>
        /// 修理所在港口名称
        /// </summary>
        public string? RepairByPortName { get; set; }

        /// <summary>
        /// 待命所在港口名称
        /// </summary>
        public string? StandbyPortName { get; set; }

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

    }
}
