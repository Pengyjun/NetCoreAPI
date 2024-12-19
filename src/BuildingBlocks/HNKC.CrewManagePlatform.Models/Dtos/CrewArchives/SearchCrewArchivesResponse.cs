namespace HNKC.CrewManagePlatform.Models.Dtos.CrewArchives
{
    /// <summary>
    /// 列表响应
    /// </summary>
    public class SearchCrewArchivesResponse
    {
        /// <summary>
        /// 用户名称
        /// </summary>
        public string? UserName { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }
        /// <summary>
        /// 在职状态
        /// </summary>
        public string? OnStatus { get; set; }
        /// <summary>
        /// 船舶类型
        /// </summary>
        public string? ShipType { get; set; }
        /// <summary>
        /// 职工号
        /// </summary>
        public string? WorkNumber { get; set; }
        /// <summary>
        /// 所在船舶
        /// </summary>
        public string? OnShip { get; set; }
        /// <summary>
        /// 所在国家
        /// </summary>
        public string? OnCountry { get; set; }
        /// <summary>
        /// 用工形式
        /// </summary>
        public string? EmploymentType { get; set; }
        /// <summary>
        /// 第一适任
        /// </summary>
        public string? FCertificate { get; set; }
        /// <summary>
        /// 第二适任
        /// </summary>
        public string? SCertificate { get; set; }
        /// <summary>
        /// 服务簿
        /// </summary>
        public string? ServiceBook { get; set; }
        /// <summary>
        /// 船员类型
        /// </summary>
        public string? CrewTypee { get; set; }
        /// <summary>
        /// 技能证书
        /// </summary>
        public string? SkillsCertificate { get; set; }
        /// <summary>
        /// 特设证书
        /// </summary>
        public string? SpecialCertificate { get; set; }
        /// <summary>
        /// 控制恢复还是删除按钮 默认0删除 1恢复
        /// </summary>
        public int BtnType { get; set; }
    }
}
