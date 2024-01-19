using GHMonitoringCenterApi.Domain.Enums;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report
{
    /// <summary>
    /// 未填报的船舶返回对象
    /// </summary>
    public class UnReportShipResponseDto
    {
        /// <summary>
        /// 项目Id
        /// </summary>
        public Guid? ProjectId { get; set; }

        /// <summary>
        /// 船舶Id
        /// </summary>
        public Guid ShipId { get; set; }

        /// <summary>
        /// 项目类型 id
        /// </summary>
        public Guid? ProjectTypeId { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string? ProjectName { get; set; }

        /// <summary>
        /// 未填报日期
        /// </summary>
        public int DateDay { get; set; }
        /// <summary>
        /// 未填报日期 datetime格式
        /// </summary>
        public DateTime NoDateTime { get; set; }
        /// <summary>
        /// 船舶类型Id
        /// </summary>
        public Guid? ShipKindTypeId { get; set; }

        /// <summary>
        /// 船舶类型名称
        /// </summary>
        public string? ShipKindTypeName { get; set; }

        /// <summary>
        /// 船舶名称
        /// </summary>
        public string? ShipName { get; set; }

        /// <summary>
        /// 项目日报Id
        /// </summary>
        public Guid? DayReportId { get; set; }

        /// <summary>
        /// 项目创建日期
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 进场时间
        /// </summary>
        public DateTime? EnterTime { get; set; }


        /// <summary>
        /// 船舶日报类型（1：关联项目，2：不关联项目）
        /// </summary>
        public ShipDayReportType ShipDayReportType { get; set; }

        /// <summary>
        /// 船舶日报类型名称
        /// </summary>
        public string ShipDayReportTypeName
        {
            get
            {
                return ShipDayReportType == ShipDayReportType.ProjectShip ? "关联" : "非关联";
            }
        }

        /// <summary>
        /// 未填报日期（时间格式）
        /// </summary>
        public DateTime DateDayTime
        {
            get
            {
                ConvertHelper.TryConvertDateTimeFromDateDay(DateDay, out DateTime dayTime);
                return dayTime;
            }
        }
    }
}
