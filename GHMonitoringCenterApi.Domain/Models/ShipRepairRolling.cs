using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 船舶修理滚动计划表
    /// </summary>
    [SugarTable("t_shiprepairrolling", IsDisabledDelete = true)]
    public class ShipRepairRolling : BaseEntity<Guid>
    {
        /// <summary>
        /// 船舶Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid ShipId { get; set; }
        /// <summary>
        /// 修理类别
        /// </summary>
        [SugarColumn(Length = 255)]
        public string? RepairCategory { get; set; }
        /// <summary>
        /// 负责机务
        /// </summary>
        [SugarColumn(Length = 255)]
        public string? MaintenanceName { get; set; }
        /// <summary>
        /// 主要修理内容 
        /// </summary>
        [SugarColumn(Length = 2000)]
        public string? MainRepairContent { get; set; }
        /// <summary>
        /// 修理地点
        /// </summary>
        [SugarColumn(Length = 2000)]
        public string? RepairLocation { get; set; }
        /// <summary>
        /// 计划修期
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int? PlannedRepairPeriod{ get; set; }
        /// <summary>
        /// 计划开始时间
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime? ScheduledStartTime { get; set; }
        /// <summary>
        /// 计划结束时间
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime? ScheduledEndTime { get; set; }
        /// <summary>
        /// 实际开始时间
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime? ActualStartTime { get; set; }
        /// <summary>
        /// 实际结束时间
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime? ExpectEndTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(Length = 2000)]
        public string? Remarks { get; set; }
    }
}
