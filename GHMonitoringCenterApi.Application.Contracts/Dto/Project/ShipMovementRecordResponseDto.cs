using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project
{


    /// <summary>
    /// /船舶进退场记录响应时间
    /// </summary>
    public class ShipMovementRecordResponseDto
    {
        /// <summary>
        /// 船舶进退场ID  关联ShipMovement 表的主键
        /// </summary>
        public Guid ShipMovementId { get; set; }
        /// <summary>
        /// 船舶ID
        /// </summary>
        public Guid ShipId { get; set; }
        /// <summary>
        ///船舶名称
        /// </summary>
        public string? ShipName { get; set; }
        /// <summary>
        /// 项目ID
        /// </summary>
        public Guid ProjectId { get; set; }

        /// <summary>
        ///公司ID
        /// </summary>
        public Guid CompayId { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string? ProjectName { get; set; }

        /// <summary>
        /// 进场时间  
        /// </summary>
        public DateTime? EnterTime { get; set; }

        /// <summary>
        /// 退场时间
        /// </summary>
        public DateTime? QuitTime { get; set; }

        /// <summary>
        ///  0是未进场状态 1是进场 2是退场
        /// </summary>
        public int Status { get; set; }

        public string Remark { get; set; }

    }
}
