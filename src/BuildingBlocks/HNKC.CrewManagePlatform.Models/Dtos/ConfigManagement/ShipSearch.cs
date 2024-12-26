using HNKC.CrewManagePlatform.Models.Enums;

namespace HNKC.CrewManagePlatform.Models.Dtos.ConfigManagement
{
    public class ShipSearch
    {
        public string? Id { get; set; }

        public string? ShipId { get; set; }
        public string? ShipName { get; set; }
        public string? Country { get; set; }
        public string? CountryName { get; set; }
        public string? Company { get; set; }
        public string? CompanyName { get; set; }
        public ShipTypeEnum ShipType { get; set; }
        public string? ShipTypeName { get; set; }
        public string? ProjectName { get; set; }
    }
}
