using GHMonitoringCenterApi.Application.Contracts.Dto;
using GHMonitoringCenterApi.Application.Contracts.Dto.ResourceManagement;
using GHMonitoringCenterApi.Application.Contracts.IService.ResourceManagement;
using GHMonitoringCenterApi.CustomAttribute;
using GHMonitoringCenterApi.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GHMonitoringCenterApi.Controllers.ResourceManagement
{
    /// <summary>
    /// 船舶资源管理
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ResourceManagementController : BaseController
    {
        public IResourceManagementService resourceManagementService { get; set; }
        public ResourceManagementController(IResourceManagementService resourceManagementService)
        {
            this.resourceManagementService = resourceManagementService;
        }
        /// <summary>
        /// 获取资源列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchResources")]
        public async Task<ResponseAjaxResult<List<SearchShipTabulationRequestDto>>> SearchResourcesAsync([FromQuery] SearchShipTabulationResponseDto searchShipTabulationResponseDto)
        {
            return await resourceManagementService.SearchResourcesAsync(searchShipTabulationResponseDto);
        }
        /// <summary>
        /// 保存资源
        /// </summary>
        /// <param name="saveShipPingTabulationResponseDto"></param>
        /// <returns></returns>
        [HttpPost("SaveResources")]
        public async Task<ResponseAjaxResult<bool>> SaveResourcesAsync([FromBody] SaveShipPingTabulationResponseDto saveShipPingTabulationResponseDto)
        {
            return await resourceManagementService.SaveResourcesAsync(saveShipPingTabulationResponseDto);
        }
        /// <summary>
        /// 删除资源
        /// </summary>
        /// <returns></returns>
        [HttpPost("DeleteResources")]
        [UnitOfWork]
        public async Task<ResponseAjaxResult<bool>> DeleteShipPingTabulationAsync([FromBody] BasePrimaryRequestDto basePrimaryRequestDto)
        {
            return await resourceManagementService.DeleteResourcesAsync(basePrimaryRequestDto);
        }
        /// <summary>
        /// 获取船舶修理滚动计划表
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchShipRepairRolling")]
        public async Task<ResponseAjaxResult<List<SearchShipRepairRollingResponseDto>>> SearchShipRepairRolling()
        {
            return await resourceManagementService.SearchShipRepairRolling();
        }
        /// <summary>
        /// 保存船舶修理滚动计划表
        /// </summary>
        /// <returns></returns>
        [HttpPost("SaveShipRepairRolling")]
        [UnitOfWork]
        public async Task<ResponseAjaxResult<bool>> SaveShipRepairRolling(SaveShipRepairRollingRequestDto saveShipRepairRollingRequestDto)
        {
            return await resourceManagementService.SaveShipRepairRolling(saveShipRepairRollingRequestDto);
        }
    }
}
