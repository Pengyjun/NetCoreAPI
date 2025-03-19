using HNKC.CrewManagePlatform.Models.CommonRequest;
using HNKC.CrewManagePlatform.Models.Dtos.CrewArchives;
using HNKC.CrewManagePlatform.Models.Enums;

namespace HNKC.CrewManagePlatform.Models.Dtos.Contract
{
    /// <summary>
    /// 
    /// </summary>
    public class YearCheckRequest : PageRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public string? Year { get; set; }
        /// <summary>
        /// 0全部 1已考核  2 未考核
        /// </summary>
        public int CheckStatus { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class SaveYearCheck
    {
        /// <summary>
        /// 
        /// </summary>
        public string? BId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// 考核结果
        /// </summary>
        public CheckEnum CheckType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<UploadResponse>? Scans { get; set; }
    }
}
