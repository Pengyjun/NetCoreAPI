using SqlSugar;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 水运工况级别
    /// </summary>
    [SugarTable("t_watercarriage", IsDisabledDelete = true)]
    public class WaterCarriage : BaseEntity<Guid>
    {
        /// <summary>
        /// Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid PomId { get; set; }
        /// <summary>
        /// 等级
        /// </summary>
        [SugarColumn(Length = 255)]
        public string? Grade { get; set; }
        /// <summary>
        /// 工况
        /// </summary>
        [SugarColumn(Length = 255)]
        public string? Remarks { get; set; }
    }
}
