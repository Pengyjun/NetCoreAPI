using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.ResourceManagement
{
    /// <summary>
    /// 获取船舶修理滚动计划表
    /// </summary>
    public class SearchShipRepairRollingResponseDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid? Id { get; set; }
        /// <summary>
        /// 船舶 Id
        /// </summary>
        public Guid? ShipId { get; set; }
        /// <summary>
        /// 船舶名称
        /// </summary>
        public string? ShipName { get; set; }
        /// <summary>
        /// 船舶类型Id
        /// </summary>
        public Guid? ShipTypeId { get; set; }
        /// <summary>
        /// 船舶类型名称
        /// </summary>
        public string? ShipTypeName { get; set; }
        /// <summary>
        /// 修理类别
        /// </summary>
        public string? RepairCategory { get; set; }
        /// <summary>
        /// 主要修理内容 
        /// </summary>
        public string? MainRepairContent { get; set; }
        /// <summary>
        /// 修理地点
        /// </summary>
        public string? RepairLocation { get; set; }
        /// <summary>
        /// 计划修期
        /// </summary>
        public int? PlannedRepairPeriod { get; set; }
        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime? ScheduledStartTime { get; set; }
        /// <summary>
        /// 计划结束时间
        /// </summary>
        public DateTime? ScheduledEndTime { get; set; }
        /// <summary>
        /// 实际开始时间
        /// </summary>
        public DateTime? ActualStartTime { get; set; }
        /// <summary>
        /// 预计结束时间
        /// </summary>
        public DateTime? ExpectEndTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string? Remarks { get; set; }
        /// <summary>
        /// 船舶排序
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 负责机务名称
        /// </summary>
        public string? MaintenanceName { get; set; }
    }
}
