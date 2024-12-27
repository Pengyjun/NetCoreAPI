using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Dtos.Contract;

namespace HNKC.CrewManagePlatform.Services.Interface.Certificate
{
    /// <summary>
    /// 证书
    /// </summary>
    public interface ICertificateService
    {
        /// <summary>
        /// 证书列表
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        Task<PageResult<CertificateSearch>> SearchCertificateAsync(CertificateRequest requestBody);
        /// <summary>
        /// 续签
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        Task<Result> SaveCertificateAsync(CertificateRenewal requestBody);
    }
}
