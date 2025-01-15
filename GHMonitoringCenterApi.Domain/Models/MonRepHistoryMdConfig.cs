using SqlSugar;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 2024月报明细开累数据修复配置
    /// </summary>
    [SugarTable("t_monrephistorymdconfig", IsDisabledDelete = true)]
    public class MonRepHistoryMdConfig : BaseEntity<Guid>
    {
        /// <summary>
        /// 1 启用
        /// </summary>
        [SugarColumn(ColumnDataType = "bit")]
        public bool Enable { get; set; }
        /// <summary>
        /// 限制开始时间
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 限制结束时间
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 限制用户  *所有 否则拆分, 
        /// </summary>
        [SugarColumn(Length = 800)]
        public string? UserId { get; set; }
    }
}
