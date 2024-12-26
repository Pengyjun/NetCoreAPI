using HNKC.CrewManagePlatform.Models.CommonRequest;
using HNKC.CrewManagePlatform.Models.Dtos.CrewArchives;

namespace HNKC.CrewManagePlatform.Models.Dtos.Contract
{
    /// <summary>
    /// 
    /// </summary>
    public class PromotionRequest : PageRequest
    {
        /// <summary>
        /// 职务
        /// </summary>
        public string? Position { get; set; }
    }
    /// <summary>
    /// 职务晋升
    /// </summary>
    public class PositionPromotion : BaseRequest
    {
        /// <summary>
        /// 主键id  用来做增改判断
        /// </summary>
        public string? Id { get; set; }
        /// <summary>
        /// 所在船舶
        /// </summary>
        public string? OnShip { get; set; }
        /// <summary>
        /// 调动任职
        /// </summary>
        public string? Postition { get; set; }
        /// <summary>
        /// 调动时间
        /// </summary>
        public DateTime? PromotionTime { get; set; }
        /// <summary>
        /// 调单文件 
        /// </summary>
        public List<UploadResponse>? PromotionScan { get; set; }
    }
}
