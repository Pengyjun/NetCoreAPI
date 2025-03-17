using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Dtos.Disembark;
using HNKC.CrewManagePlatform.Services.Interface.Disembark;
using HNKC.CrewManagePlatform.Services.Interface.ShipWatch;
using HNKC.CrewManagePlatform.SqlSugars.UnitOfTransaction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HNKC.CrewManagePlatform.Services.Admin.Api.Controllers
{
    /// <summary>
    /// 排班控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CrewRotaController : BaseController
    {
        private readonly ICrewRotaService _crewRotaService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="crewRotaService"></param>
        public CrewRotaController(ICrewRotaService crewRotaService)
        {
            this._crewRotaService = crewRotaService;
        }

        /// <summary>
        /// 保存船舶排班
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        [HttpPost("SaveCrewRota")]
        [Transactional]
        public async Task<Result> SaveCrewRotaAsync([FromBody] SaveSchedulingRequest requestBody)
        {
            return await _crewRotaService.SaveCrewRotaAsync(requestBody);
        }

        /// <summary>
        /// 船员船舶排班回显
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        [HttpGet("CrewRotaList")]
        public async Task<Result> CrewRotaListAsync([FromQuery] SchedulingRequest requestBody)
        {
            return await _crewRotaService.CrewRotaListAsync(requestBody);
        }

        /// <summary>
        /// 船舶排班用户下拉列表
        /// </summary>
        /// <param name="shipId"></param>
        /// <returns></returns>
        [HttpGet("CrewRotaUserList")]
        public async Task<Result> CrewRotaUserListAsync(Guid shipId)
        {
            return await _crewRotaService.CrewRotaUserListAsync(shipId);
        }

        /// <summary>
        /// 值班管理列表
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        public async Task<IActionResult> SearchCrewRotaAsync(SearchCrewRotaRequest requestBody)
        {
            var data = await _crewRotaService.SearchCrewRotaAsync(requestBody);
            return Ok(data);
        }
    }
}