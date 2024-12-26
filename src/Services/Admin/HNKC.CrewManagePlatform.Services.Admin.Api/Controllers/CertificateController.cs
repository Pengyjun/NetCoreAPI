using HNKC.CrewManagePlatform.Models.Dtos.Contract;
using HNKC.CrewManagePlatform.Services.Interface.Certificate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HNKC.CrewManagePlatform.Services.Admin.Api.Controllers
{
    /// <summary>
    /// 证书控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CertificateController : BaseController
    {
        private readonly ICertificateService _certificateService;
        /// <summary>
        /// 注入
        /// </summary>
        /// <param name="certificateService"></param>
        public CertificateController(ICertificateService certificateService)
        {
            this._certificateService = certificateService;
        }
        /// <summary>
        /// 证书列表
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        [HttpGet("SearchCertificate")]
        public async Task<IActionResult> SearchCertificateAsync([FromQuery] CertificateRequest requestBody)
        {
            var data = await _certificateService.SearchCertificateAsync(requestBody);
            return Ok(data);
        }
    }
}
