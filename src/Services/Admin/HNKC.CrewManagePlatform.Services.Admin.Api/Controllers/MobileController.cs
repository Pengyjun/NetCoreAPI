using HNKC.CrewManagePlatform.Models.Dtos.Salary;
using HNKC.CrewManagePlatform.Models.Dtos;
using HNKC.CrewManagePlatform.Services.Interface.Salary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HNKC.CrewManagePlatform.Utils;

namespace HNKC.CrewManagePlatform.Services.Admin.Api.Controllers
{
    /// <summary>
    /// 移动端
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MobileController : BaseController
    {
        public ISalaryService salaryService { get; set; }
        public MobileController(ISalaryService salaryService)
        {
            this.salaryService = salaryService;
        }

        /// <summary>
        /// 短信链接查询
        /// </summary>
        /// <param name="sgin"></param>
        /// <returns></returns>
        [HttpGet("FindUserInfo")]
        public async Task<IActionResult> FindUserInfoAsync([FromQuery]string sign)
        {
            var data =await salaryService.FindUserInfoAsync(sign);
            return Ok(data);
        }
    }
}
