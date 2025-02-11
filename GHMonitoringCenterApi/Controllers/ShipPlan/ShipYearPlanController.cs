using GHMonitoringCenterApi.Application.Contracts.Dto.ShipPlan;
using GHMonitoringCenterApi.Application.Contracts.IService.Push;
using GHMonitoringCenterApi.Application.Contracts.IService.ShipPlan;
using GHMonitoringCenterApi.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GHMonitoringCenterApi.Controllers.ShipPlan
{
    /// <summary>
    /// 船舶年初计划产值控制器
    /// </summary>
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ShipYearPlanController : ControllerBase
    {

        public  IShipPlanService shipPlanService;

        /// <summary>
        /// 依赖注入
        /// </summary>
        public ShipYearPlanController(IShipPlanService shipPlanService)
        {
            this.shipPlanService = shipPlanService;
        }

        /// <summary>
        /// 保存船舶年初计划
        /// </summary>
        /// <param name="saveShipPlanRequestDto"></param>
        /// <returns></returns>
        [HttpPost("SaveShipPlan")]
        public async Task<ResponseAjaxResult<bool>> SaveShipPlanAsync([FromBody]SaveShipPlanRequestDto saveShipPlanRequestDto)
        {
            return await shipPlanService.SaveShipPlanAsync(saveShipPlanRequestDto);
        }


        /// <summary>
        /// 保存船舶年初完成产值
        /// </summary>
        /// <param name="saveShipPlanRequestDto"></param>
        /// <returns></returns>
        [HttpPost("SaveShipComplete")]
        public async Task<ResponseAjaxResult<bool>> SaveShipCompleteAsync([FromBody] SaveShipCompleteRequestDto  saveShipCompleteRequestDto)
        {
            return await shipPlanService.SaveShipCompleteAsync(saveShipCompleteRequestDto);
        }


        /// <summary>
        /// 搜索船舶年度计划列表
        /// </summary>
        /// <param name="shipPlanRequestDto"></param>
        /// <returns></returns>
        [HttpGet("SearchShipPlan")]
        public async Task<ResponseAjaxResult<List<ShipPlanResponseDto>>> SearchShipPlanAsync([FromQuery]ShipPlanRequestDto shipPlanRequestDto)
        {
            return await shipPlanService.SearchShipPlanAsync(shipPlanRequestDto);
        }



        /// <summary>
        /// 搜索船舶完成产值列表
        /// </summary>
        /// <param name="shipPlanRequestDto"></param>
        /// <returns></returns>
        [HttpGet("SearchShiComplete")]
        public async Task<ResponseAjaxResult<List<ShipCompleteResponseDto>>> SearchShiCompleteAsync([FromQuery] ShipCompleteRequestDto shipPlanRequestDto)
        {
            return await shipPlanService.SearchShiCompleteAsync(shipPlanRequestDto);
        }
    }
}
