using HNKC.CrewManagePlatform.Models.Dtos.CrewArchives;
using HNKC.CrewManagePlatform.Models.Enums;

namespace HNKC.CrewManagePlatform.Models.Dtos.Contract
{
    /// <summary>
    /// 证书列表
    /// </summary>
    public class CertificateSearch
    {
        /// <summary>
        /// 用户业务id
        /// </summary>
        public string? BId { get; set; }
        /// <summary>
        /// 处理增改的业务主键id
        /// </summary>
        public string? Id { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string? UserName { get; set; }
        /// <summary>
        /// 船舶类型
        /// </summary>
        public ShipTypeEnum ShipType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? ShipTypeName { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }
        /// <summary>
        /// 职工号
        /// </summary>
        public string? WorkNumber { get; set; }
        /// <summary>
        /// 所在国家
        /// </summary>
        public string? Country { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? CountryName { get; set; }
        /// <summary>
        /// 在船职务
        /// </summary>
        public string? OnBoardPositionName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? OnBoardPosition { get; set; }
        /// <summary>
        /// 所在船舶
        /// </summary>
        public string? OnBoard { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? OnBoardName { get; set; }
        /// <summary>
        /// 证书类型
        /// </summary>
        public string? CertificateType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? CertificateTypeName { get; set; }
        /// <summary>
        /// 有效期
        /// </summary>
        public string? EffectiveTime { get; set; }
        /// <summary>
        /// 第一适任
        /// </summary>
        public string? FPosition { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? FPositionName { get; set; }
        /// <summary>
        /// 第二适任
        /// </summary>
        public string? SPosition { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? SPositionName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? OnStatus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? CardId { get; set; }
        /// <summary>
        /// 到期天数
        /// </summary>
        public int DueDays { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public CrewStatusEnum DeleteResonEnum { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime WorkShipStartTime { get; set; }
    }
}
