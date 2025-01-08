using HNKC.CrewManagePlatform.Models.Enums;

namespace HNKC.CrewManagePlatform.Models.Dtos.Disembark
{
    /// <summary>
    /// 离船申请列表
    /// </summary>
    public class SearchDisembark
    {
        /// <summary>
        /// 业务id主键
        /// </summary>
        public Guid? BId { get; set; }
        /// <summary>
        /// 申请人
        /// </summary>
        public Guid? UserId { get; set; }
        /// <summary>
        /// 申请人
        /// </summary>
        public string? UserName { get; set; }
        /// <summary>
        /// 工号
        /// </summary>
        public string? WorkNumber { get; set; }
        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime? Time { get; set; }
        /// <summary>
        /// 离船时间
        /// </summary>
        public DateTime? DisembarkTime { get; set; }
        /// <summary>
        /// 归船时间
        /// </summary>
        public DateTime? ReturnShipTime { get; set; }
        /// <summary>
        /// 实际离船时间
        /// </summary>
        public DateTime? RealDisembarkTime { get; set; }
        /// <summary>
        /// 实际归船时间
        /// </summary>
        public DateTime? RealReturnShipTime { get; set; }
        /// <summary>
        /// 休假天数
        /// </summary>
        public int HolidayNums { get; set; }
        /// <summary>
        /// 船舶
        /// </summary>
        public Guid? ShipId { get; set; }
        /// <summary>
        /// 船舶
        /// </summary>
        public string? ShipName { get; set; }
        /// <summary>
        /// 船舶类型
        /// </summary>
        public ShipTypeEnum ShipType { get; set; }
        /// <summary>
        /// 船舶类型
        /// </summary>
        public string? ShipTypeName { get; set; }
        /// <summary>
        /// 所在国家
        /// </summary>
        public Guid? Country { get; set; }
        /// <summary>
        /// 所在国家
        /// </summary>
        public string? CountryName { get; set; }
        /// <summary>
        /// 所属公司
        /// </summary>
        public Guid? Company { get; set; }
        /// <summary>
        /// 所属公司
        /// </summary>
        public string? CompanyName { get; set; }
        /// <summary>
        /// 所在项目
        /// </summary>
        public string? ProjectName { get; set; }
        /// <summary>
        /// 审批状态
        /// </summary>
        public ApproveStatus ApproveStatus { get; set; }
        /// <summary>
        /// 审批状态
        /// </summary>
        public string? ApproveStatusName { get; set; }
        /// <summary>
        /// 离船日期
        /// </summary>
        public DateTime? DisembarkDate { get; set; }
    }
}
