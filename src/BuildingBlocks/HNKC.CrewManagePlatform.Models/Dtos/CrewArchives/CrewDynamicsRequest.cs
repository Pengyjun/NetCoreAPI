using HNKC.CrewManagePlatform.Models.CommonRequest;

namespace HNKC.CrewManagePlatform.Models.Dtos.CrewArchives
{
    /// <summary>
    /// 船员动态
    /// </summary>
    public class CrewDynamicsRequest : PageRequest
    {
        /// <summary>
        /// 在船天数 默认0 
        /// </summary>
        public int BoardingDays { get; set; } = 0;
        /// <summary>
        /// 休假天数 默认0 
        /// </summary>
        public int HolidayDays { get; set; } = 0;
        /// <summary>
        /// 统计开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 统计结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
    }
}
