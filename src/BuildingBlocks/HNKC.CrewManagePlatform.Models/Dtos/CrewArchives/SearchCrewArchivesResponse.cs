using HNKC.CrewManagePlatform.Models.Enums;

namespace HNKC.CrewManagePlatform.Models.Dtos.CrewArchives
{
    /// <summary>
    /// 列表响应
    /// </summary>
    public class SearchCrewArchivesResponse : BaseResponse
    {
        /// <summary>
        /// 用户名称
        /// </summary>
        public string? UserName { get; set; }
        /// <summary>
        /// 身份证
        /// </summary>
        public string? CardId { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }
        /// <summary>
        /// 在职状态
        /// </summary>
        public CrewStatusEnum OnStatus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? OnStatusName { get; set; }
        /// <summary>
        /// 船舶类型
        /// </summary>
        public ShipTypeEnum ShipType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? ShipTypeName { get; set; }
        /// <summary>
        /// 职工号
        /// </summary>
        public string? WorkNumber { get; set; }
        /// <summary>
        /// 所在船舶
        /// </summary>
        public string? OnBoard { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? OnBoardName { get; set; }
        /// <summary>
        /// 所在国家
        /// </summary>
        public Guid? OnCountry { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? OnCountryName { get; set; }
        /// <summary>
        /// 用工形式
        /// </summary>
        public string? EmploymentType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? EmploymentTypeName { get; set; }
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
        /// 服务簿
        /// </summary>
        public ServiceBookEnum ServiceBookType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? ServiceBookName { get; set; }
        /// <summary>
        /// 船员类型
        /// </summary>
        public string? CrewType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? CrewTypeName { get; set; }
        /// <summary>
        /// 技能证书
        /// </summary>
        public List<string>? SkillsCertificate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? SkillsCertificateName { get; set; }
        /// <summary>
        /// 特设证书
        /// </summary>
        public List<string>? SpecialCertificate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? SpecialCertificateName { get; set; }
        /// <summary>
        /// 控制恢复还是删除按钮 默认0 1恢复
        /// </summary>
        public int BtnType { get; set; }
        /// <summary>
        /// 删除原因
        /// </summary>
        public CrewStatusEnum DeleteReson { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public int IsDelete { get; set; }

        /// <summary>
        /// 上船日期
        /// </summary>
        public DateTime WorkShipStartTime { get; set; }
        /// <summary>
        /// 下船日期
        /// </summary>
        public DateTime? WorkShipEndTime { get; set; }
    }
}
