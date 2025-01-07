using SqlSugar;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 每年假日数据
    /// </summary>
    [SugarTable("t_yearholidaysetting", IsDisabledDelete = true)]
    public class YearHolidaySetting : BaseEntity<Guid>
    {
        /// <summary>
        /// 节假日
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// 是否是节假日 true 是
        /// </summary>
        public bool IsOffDay {  get; set; }
    }
}
