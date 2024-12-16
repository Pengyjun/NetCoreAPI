using HNKC.CrewManagePlatform.Models.CommonRequest;

namespace HNKC.CrewManagePlatform.Models.Dtos.PullResult
{
    /// <summary>
    /// 
    /// </summary>
    public class PullRequestDto : PageRequest
    {
        /// <summary>
        /// 拉取的开始时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 拉取的结束时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }
    }
}
