using GHMonitoringCenterApi.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilsSharp;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project.ShipMovements
{

    /// <summary>
    ///进场船舶返回对象
    /// </summary>
    public class EnterShipsResponseDto
    {

        /// <summary>
        /// 搜索的填报时间
        /// </summary>
        public DateTime DateDayTime { get; set; }

        /// <summary>
        /// 船舶集合
        /// </summary>
        public ResEnterShipDto[] Ships { get; set; } = new ResEnterShipDto[0];

        /// <summary>
        ///进场船舶返回对象
        /// </summary>
        public class ResEnterShipDto
        {

            /// <summary>
            /// 搜索的填报时间
            /// </summary>
            public DateTime DateDayTime { get; set; }

            /// <summary>
            /// 项目Id
            /// </summary>
            public Guid ProjectId { get; set; }

            /// <summary>
            /// 船舶Id（PomId）
            /// </summary>
            public Guid ShipId { get; set; }
            /// <summary>
            /// 船舶所属公司
            /// </summary>
            public Guid? ShipCompanyId { get; set; }

            /// <summary>
            /// 船舶类型名称
            /// </summary>
            public string? ShipKindTypeName { get; set; }

            /// <summary>
            /// 船舶名称
            /// </summary>
            public string? ShipName { get; set; }
            /// <summary>
            /// 已填写过的船舶 关联的项目名称
            /// </summary>
            public string? ProjectName { get; set; }

            /// <summary>
            /// 进场时间
            /// </summary>
            public DateTime? EnterTime { get; set; }

            /// <summary>
            /// 填报时间
            /// </summary>
            public DateTime? FillReportTime { get; set; }

            /// <summary>
            /// 是否填报
            /// </summary>
            //public bool IsFillReport { get; set; }
            /// <summary>
            /// 0无需填报  1未填报  2已填报
            /// </summary>
            public int FillReportStatus { get; set; }
            /// <summary>
            /// 是否关联项目 0全部信息  1关联 2不关联 
            /// </summary>
            public int AssociationProject { get; set; }
        }
    }

}
