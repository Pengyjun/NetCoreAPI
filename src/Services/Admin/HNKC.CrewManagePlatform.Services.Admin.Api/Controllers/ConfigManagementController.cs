using HNKC.CrewManagePlatform.Models.CommonRequest;
using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Dtos;
using HNKC.CrewManagePlatform.Models.Dtos.ConfigManagement;
using HNKC.CrewManagePlatform.Services.Interface.ConfigManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HNKC.CrewManagePlatform.Services.Admin.Api.Controllers
{
    /// <summary>
    /// 配置管理控制器 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ConfigManagementController : BaseController
    {
        private IConfigManagementService _configManagementService;
        /// <summary>
        /// 注入
        /// </summary>
        public ConfigManagementController(IConfigManagementService configManagementService)
        {
            this._configManagementService = configManagementService;
        }

        //public async Task<IActionResult> SearchShipAsync()
        //{

        //}
        /// <summary>
        /// 保存船舶
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        [HttpPost("SaveShip")]
        public async Task<Result> SaveShipAsync([FromBody] SaveShipRequest requestBody)
        {
            return await _configManagementService.SaveShipAsync(requestBody);
        }
        /// <summary>
        /// 船舶管理列表
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpGet("SearchShip")]
        public async Task<IActionResult> SearchShipAsync([FromQuery] ShipRequest requestBody)
        {
            var data = await _configManagementService.SearchShipAsync(requestBody);
            return Ok(data);
        }
        /// <summary>
        /// 删除船舶
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("DeleteShip")]
        public async Task<Result> DeleteShipAsync([FromQuery] string id)
        {
            return await _configManagementService.DeleteShipAsync(id);
        }
        /// <summary>
        /// 提醒配置列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchRemindSetting")]
        public async Task<Result> SearchRemindSettingAsync()
        {
            return await _configManagementService.SearchRemindSettingAsync();
        }
        /// <summary>
        /// 保存提醒配置
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        [HttpPost("SaveRemindSetting")]
        public async Task<Result> SaveRemindSettingAsync([FromBody] RemindRequest requestBody)
        {
            return await _configManagementService.SaveRemindSettingAsync(requestBody);
        }
    }
}
