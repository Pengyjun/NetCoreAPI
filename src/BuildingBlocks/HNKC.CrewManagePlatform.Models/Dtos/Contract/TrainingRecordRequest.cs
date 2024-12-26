using HNKC.CrewManagePlatform.Models.CommonRequest;

namespace HNKC.CrewManagePlatform.Models.Dtos.Contract
{
    /// <summary>
    /// 培训记录
    /// </summary>
    public class TrainingRecordRequest : PageRequest
    {
        /// <summary>
        /// 填报开始时间
        /// </summary>
        public string? StartTime { get; set; }
        /// <summary>
        /// 填报结束时间
        /// </summary>
        public string? EndTime { get; set; }
        /// <summary>
        /// 填报类型
        /// </summary>
        public string? TraningType { get; set; }
        /// <summary>
        /// 填报人
        /// </summary>
        public string? UserId { get; set; }
    }
}
