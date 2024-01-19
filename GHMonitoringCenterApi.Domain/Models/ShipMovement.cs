using GHMonitoringCenterApi.Domain.Enums;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 船舶进出场
    /// </summary>
    [SugarTable("t_shipmovement", IsDisabledDelete = true)]
    public  class ShipMovement : BaseEntity<Guid>
    {
        /// <summary>
        /// 项目id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid ProjectId { get; set; }

        /// <summary>
        /// 船舶Id（PomId）
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid ShipId { get; set; }

        /// <summary>
        /// 船舶类型（1：自有船舶，2：分包船舶）
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public ShipType ShipType { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public ShipMovementStatus Status { get; set; }

        /// <summary>
        /// 进场时间
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime? EnterTime { get; set; }

        /// <summary>
        /// 退场时间
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime? QuitTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(Length = 2000)]
        public string? Remarks { get; set; }

    }
}
