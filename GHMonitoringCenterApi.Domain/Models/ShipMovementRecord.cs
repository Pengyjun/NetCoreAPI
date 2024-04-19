using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{

    /// <summary>
    /// 船舶进退场 记录表
    /// </summary>
    [SugarTable("t_shipmovementrecord", IsDisabledDelete = true)]
    public class ShipMovementRecord:BaseEntity<Guid>
    {
        /// <summary>
        /// 船舶进退场ID  关联ShipMovement 表的主键
        /// </summary>
        public Guid ShipMovementId { get; set; }
        /// <summary>
        /// 船舶ID
        /// </summary>
        [SugarColumn(Length =50)]
        public Guid ShipId { get; set; }
        /// <summary>
        /// 项目ID
        /// </summary>
        [SugarColumn(Length = 50)]
        public Guid ProjectId { get; set; }

        /// <summary>
        ///公司ID
        /// </summary>
        [SugarColumn(Length = 50)]
        public Guid CompayId { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        [SugarColumn(Length = 200)]
        public string? ProjectName { get; set; }


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
        ///  0是为进场状态 1是进场 2是退场
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int Status { get; set; }
    }
}
