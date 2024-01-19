using GHMonitoringCenterApi.Application.Contracts.Dto.Ship;
using GHMonitoringCenterApi.Application.Contracts.IService.Ship;
using GHMonitoringCenterApi.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GHMonitoringCenterApi.Controllers.Ship
{
    /// <summary>
    /// 船舶相关控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ShipController : BaseController
    {
        #region 依赖注入

        /// <summary>
        /// 依赖注入
        /// </summary>
        public IShipService _shipService { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shipService"></param>
        public ShipController(IShipService shipService)
        {
            this._shipService = shipService;
        }
        #endregion

        /// <summary>
        /// 船机成本维护列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet("SearchShipCostMaintenance")]
        public async Task<ResponseAjaxResult<List<ShipCostMaintenanceResponseDto>>> SearchShipCostMaintenanceAsync([FromQuery] ShipCostMaintenanceRequestDto requestDto)
        {
            return await _shipService.SearchShipCostMaintenanceAsync(requestDto);
        }

        /// <summary>
        /// 船舶 and 分类 列表返回
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchOwnShipAndShipTypeAsync")]
        public async Task<ResponseAjaxResult<SearchOwnShipAndShipTypeDto>> SearchOwnShipAndShipTypeAsync()
        {
            return await _shipService.SearchOwnShipAndShipTypeAsync();
        }

        /// <summary>
        /// 船舶成本维护增改
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("AddOrModifyShipCostMaintenance")]
        public async Task<ResponseAjaxResult<bool>> AddOrModifyShipCostMaintenanceAsync([FromBody] AddOrModifyShipCostMaintenaceRequestDto requestDto)
        {
            return await _shipService.AddOrModifyShipCostMaintenanceAsync(requestDto);
        }

        /// <summary>
        /// 船舶成本维护删除
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("DeleteShipCostMaintenance")]
        public async Task<ResponseAjaxResult<bool>> DeleteShipCostMaintenanceAsync([FromBody] DeleteShipCostMaintenanceRequestDto requestDto)
        {
            return await _shipService.DeleteShipCostMaintenanceAsync(requestDto);
        }
    }
}
