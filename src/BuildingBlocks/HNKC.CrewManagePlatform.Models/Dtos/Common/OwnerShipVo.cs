using HNKC.CrewManagePlatform.Models.Enums;

namespace HNKC.CrewManagePlatform.Models.Dtos.Common
{
    /// <summary>
    /// 自有船舶Vo
    /// </summary>
    public class OwnerShipVo
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public string? Id { get; set; }

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
        /// 所属国家名称
        /// </summary>
        public string? CountryName { get; set; }

        /// <summary>
        /// 所属公司ID
        /// </summary>
        public Guid? Company { get; set; }

        /// <summary>
        /// 所属公司名称
        /// </summary>
        public string? CompanyName { get; set; }

        /// <summary>
        /// 船舶类型
        /// </summary>
        public ShipTypeEnum ShipType { get; set; }

        /// <summary>
        /// 船舶类型名称
        /// </summary>
        public string? ShipTypeName { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string? ProjectName { get; set; }
    }
}
