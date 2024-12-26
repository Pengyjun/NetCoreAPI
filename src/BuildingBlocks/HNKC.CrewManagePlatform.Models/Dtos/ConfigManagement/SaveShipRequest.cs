using HNKC.CrewManagePlatform.Models.CommonRequest;
using HNKC.CrewManagePlatform.Models.Enums;

namespace HNKC.CrewManagePlatform.Models.Dtos.ConfigManagement
{
    /// <summary>
    /// 
    /// </summary>
    public class SaveShipRequest
    {
        /// <summary>
        /// 1 新增 2修改
        /// </summary>
        public int Type { get; set; }
        public string? Id { get; set; }
        public string? ShipId { get; set; }
        public string? ShipName { get; set; }
        public string? Company { get; set; }
        public string? Country { get; set; }
        public ShipTypeEnum ShipType { get; set; }
        public string? Project { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ShipRequest : PageRequest
    {
        public string? ShipId { get; set; }
        public string? ShipType { get; set; }
    }
}
