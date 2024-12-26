using HNKC.CrewManagePlatform.Models.CommonRequest;
using HNKC.CrewManagePlatform.Models.Enums;

namespace HNKC.CrewManagePlatform.Models.Dtos.Contract
{
    /// <summary>
    /// 证书
    /// </summary>
    public class CertificateRequest : PageRequest
    {
        /// <summary>
        /// 证书类型
        /// </summary>
        public CertificatesEnum? Certificates { get; set; }
    }
}
