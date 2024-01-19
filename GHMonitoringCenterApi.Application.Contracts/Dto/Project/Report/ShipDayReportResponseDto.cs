using GHMonitoringCenterApi.Domain.Enums;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project
{
    /// <summary>
    /// 船舶日报返回结果
    /// </summary>
    public class ShipDayReportResponseDto: ShipDayReportDto
    {
       
        /// <summary>
        /// 船舶名称
        /// </summary>
        public string? ShipName { get; set; }

        /// <summary>
        /// 船舶类型
        /// </summary>
        public Guid? ShipKindTypeId { get; set; }

        /// <summary>
        /// 船舶类型名称
        /// </summary>
        public string? ShipKindTypeName { get; set; }

        /// <summary>
        /// 所在港口名称
        /// </summary>
        public string? PortName { get; set; }

        /// <summary>
        /// 日期(时间格式)
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

        /// <summary>
        /// 是否存在警示
        /// </summary>
        public bool HasWaning { get; set; }

        /// <summary>
        /// 警示消息
        /// </summary>
        public string? WaningMessage{ get; set; }
    }
}
