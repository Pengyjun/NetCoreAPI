using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Dtos.Disembark;
using HNKC.CrewManagePlatform.Services.Interface.Disembark;
using HNKC.CrewManagePlatform.SqlSugars.UnitOfTransaction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HNKC.CrewManagePlatform.Services.Admin.Api.Controllers
{
    /// <summary>
    /// 离船控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DisembarkController : BaseController
    {
        private readonly IDisembarkService _disembarkService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="disembarkService"></param>
        public DisembarkController(IDisembarkService disembarkService)
        {
            this._disembarkService = disembarkService;
        }
        /// <summary>
        /// 保存离船申请
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        [HttpPost("SaveDisembark")]
        [Transactional]
        public async Task<Result> SaveDisembarkAsync([FromBody] DisembarkRequest requestBody)
        {
            return await _disembarkService.SaveDisembarkAsync(requestBody);
        }
        /// <summary>
        /// 离船申请列表
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        [HttpGet("SearchDisembark")]
        public async Task<IActionResult> SearchDisembarkAsync([FromQuery] SearchDisembarkRequest requestBody)
        {
            var data = await _disembarkService.SearchDisembarkAsync(requestBody);
            return Ok(data);
        }
    }
}
