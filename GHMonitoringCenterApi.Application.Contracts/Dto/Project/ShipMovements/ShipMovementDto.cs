using GHMonitoringCenterApi.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project.ShipMovements
{
    /// <summary>
    /// 船舶动向dto
    /// </summary>
    public abstract  class ShipMovementDto
    {
        /// <summary>
        /// 项目Id
        /// </summary>
        public Guid ProjectId { get; set; }

        /// <summary>
        /// 船舶Id（PomId）
        /// </summary>
        public Guid ShipId { get; set; }

        /// <summary>
        /// 船舶类型（1：自有船舶，2：分包船舶）
        /// </summary>
        public ShipType ShipType { get; set; }

        /// <summary>
        /// 进场时间
        /// </summary>
        public DateTime? EnterTime { get; set; }

        /// <summary>
        ///备注
        /// </summary>
        public string? Remarks { get; set; }
    }
}
