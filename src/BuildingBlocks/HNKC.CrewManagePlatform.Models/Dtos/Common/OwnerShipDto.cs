using HNKC.CrewManagePlatform.Models.CommonRequest;
using HNKC.CrewManagePlatform.Models.Enums;

namespace HNKC.CrewManagePlatform.Models.Dtos.Common
{
    /// <summary>
    /// 自有船舶Dto
    /// </summary>
    public class OwnerShipDto : PageRequest
    {
        /// <summary>
        /// 船舶ID
        /// </summary>
        public string? ShipId { get; set; }

        /// <summary>
        /// 船舶名称
        /// </summary>
        public string? ShipName { get; set; }

        /// <summary>
        /// 所属国家ID
        /// </summary>
        public Guid? Country { get; set; }

        /// <summary>
        /// 所属公司ID
        /// </summary>
        public Guid? Company { get; set; }

        /// <summary>
        /// 船舶类型
        /// </summary>
        public ShipTypeEnum ShipType { get; set; }
    }
}
