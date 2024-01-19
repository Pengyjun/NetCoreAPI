using GHMonitoringCenterApi.Application.Contracts.Dto.Enums;
using GHMonitoringCenterApi.Domain.Enums;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilsSharp;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report
{

    /// <summary>
    /// 船舶返回对象 for 月报
    /// </summary>
    public class ShipsForReportResponseDto
    {
        /// <summary>
        /// 搜索的填报月份
        /// </summary>
        public int DateMonth { get; set; }

        /// <summary>
        /// 搜索的填报月份（时间格式）
        /// </summary>
        public DateTime DateMonthTime
        {
            get
            {
                ConvertHelper.TryParseFromDateMonth(DateMonth, out DateTime monthTime);
                return monthTime;
            }
        }

        /// <summary>
        /// 船舶集合
        /// </summary>
        public ResShipForReport[] Ships { get; set; } = new ResShipForReport[0];

        /// <summary>
        /// 船舶返回对象 for 月报
        /// </summary>
        public class ResShipForReport
        {
            /// <summary>
            /// 项目Id
            /// </summary>
            public Guid ProjectId { get; set; }

            /// <summary>
            /// 船舶Id
            /// </summary>
            public Guid ShipId { get; set; }

            /// <summary>
            /// 船舶来源类型
            /// </summary>
            public ShipType ShipType { get; set; }

            /// <summary>
            /// 备注
            /// </summary>
            public string? Remarks { get; set; }

            /// <summary>
            /// 填报月份（例：202304，注解：上月的26-本月的25，例：2023.3.26-2023.04.25）
            /// </summary>
            public int? DateMonth { get; set; }

            /// <summary>
            /// 填报状态
            /// </summary>
            public FillReportStatus FillReportStatus { get; set; }

            /// <summary>
            /// 项目名称
            /// </summary>
            public string? ProjectName { get; set; }

            /// <summary>
            /// 船舶来源类型名称(注：前端界面（船舶来源）绑定字段)
            /// </summary>
            public string? ShipTypeName { get { return EnumExtension.GetEnumDescription(ShipType); } }

            /// <summary>
            /// 船舶类型名称
            /// </summary>
            public string? ShipKindTypeName { get; set; }

            /// <summary>
            /// 船舶名称
            /// </summary>
            public string? ShipName { get; set; }

            /// <summary>
            /// 填报月份时间格式（前端可以根据需求获取年，月拼接）
            /// </summary>
            public DateTime? DateMonthTime
            {
                get
                {
                    if (DateMonth == null)
                    {
                        return null;
                    }
                    ConvertHelper.TryParseFromDateMonth((int)DateMonth, out DateTime monthTime);
                    return monthTime;
                }
            }
        }
    }

}
