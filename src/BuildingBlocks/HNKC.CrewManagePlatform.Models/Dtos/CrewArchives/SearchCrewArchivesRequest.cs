using HNKC.CrewManagePlatform.Models.CommonRequest;
using HNKC.CrewManagePlatform.Models.Enums;

namespace HNKC.CrewManagePlatform.Models.Dtos.CrewArchives
{
    /// <summary>
    /// 列表请求入参
    /// </summary>
    public class SearchCrewArchivesRequest : PageRequest
    {
        /// <summary>
        /// 所在船舶
        /// </summary>
        public string? OnBoard { get; set; }
        /// <summary>
        /// 历史履职
        /// </summary>
        public string? HistoryOnBoard { get; set; }
        /// <summary>
        /// 船员类型
        /// </summary>
        public string? CrewType { get; set; }
        /// <summary>
        /// 船员状态
        /// </summary>
        public List<string>? Staus { get; set; }
        /// <summary>
        /// 船舶类型
        /// </summary>
        public List<string>? ShipTypes { get; set; }
        /// <summary>
        /// 服务薄类型
        /// </summary>
        public List<string>? ServiceBooks { get; set; }
        /// <summary>
        /// 第一适任证 
        /// </summary>
        public List<string>? FPosition { get; set; }
        /// <summary>
        /// 第二适任证 
        /// </summary>
        public List<string>? SPosition { get; set; }
        /// <summary>
        /// 培训证书 true代表选中
        /// </summary>
        public bool TrainingCertificate { get; set; }
        /// <summary>
        /// Z01有效日期
        /// </summary>
        public bool Z01Effective { get; set; }
        /// <summary>
        /// Z07有效日期
        /// </summary>
        public bool Z07Effective { get; set; }
        /// <summary>
        /// Z08有效日期
        /// </summary>
        public bool Z08Effective { get; set; }
        /// <summary>
        /// Z04有效日期
        /// </summary>
        public bool Z04Effective { get; set; }
        /// <summary>
        /// Z05有效日期
        /// </summary>
        public bool Z05Effective { get; set; }
        /// <summary>
        /// Z02有效日期
        /// </summary>
        public bool Z02Effective { get; set; }
        /// <summary>
        /// Z06有效日期
        /// </summary>
        public bool Z06Effective { get; set; }
        /// <summary>
        /// Z09有效日期
        /// </summary>
        public bool Z09Effective { get; set; }
        /// <summary>
        /// 技能证书
        /// </summary>
        public List<string>? CertificateTypes { get; set; }
        /// <summary>
        /// 学历类型
        /// </summary>
        public List<string>? QualificationTypes { get; set; }
        /// <summary>
        /// 学历：全日制...
        /// </summary>
        public List<string>? Qualifications { get; set; }
    }
}
