using GHMonitoringCenterApi.Application.Contracts.Dto;
using GHMonitoringCenterApi.Application.Contracts.Dto.EquipmentManagement;
using GHMonitoringCenterApi.Application.Contracts.IService.EquipmentManagement;
using GHMonitoringCenterApi.CustomAttribute;
using GHMonitoringCenterApi.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GHMonitoringCenterApi.Controllers.EquipmentManagement
{
    /// <summary>
    /// 设备管理
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EquipmentManagementController : BaseController
    {
        #region 依赖注入
        IEquipmentManagementService equipmentManagementService { get; set; }

        public EquipmentManagementController(IEquipmentManagementService equipmentManagementService)
        {
            this.equipmentManagementService = equipmentManagementService;
        }
        #endregion

        /// <summary>
        /// 获取设备信息
        /// </summary>
        /// <param name="searchEquipmentManagementRequestDto"></param>
        /// <returns></returns>
        [HttpGet("SearchEquipmentManagement")]
        public async Task<ResponseAjaxResult<List<SearchEquipmentManagementResponseDto>>> SearchEquipmentManagementAsync([FromQuery] SearchEquipmentManagementRequestDto searchEquipmentManagementRequestDto)
        {
            return await equipmentManagementService.SearchEquipmentManagementAsync(searchEquipmentManagementRequestDto);
        }
        /// <summary>
        /// 保存设备信息
        /// </summary>
        /// <param name="searchEquipmentManagementRequestDto"></param>
        /// <returns></returns>
        [HttpPost("SaveEquipmentManagement")]
        [UnitOfWork]
        public async Task<ResponseAjaxResult<bool>> SaveEquipmentManagementAsync([FromBody] SaveEquipmentManagementRequestDto saveEquipmentManagementRequestDto)
        {
            return await equipmentManagementService.SaveEquipmentManagementAsync(saveEquipmentManagementRequestDto);
        }
        /// <summary>
        /// 水上设备月报自动生成
        /// </summary>
        /// <param name="searchEquipmentManagementRequestDto"></param>
        /// <returns></returns>
        [HttpPost("AddMarineEquipment")]
        [AllowAnonymous]
        public async Task<ResponseAjaxResult<bool>> AddMarineEquipmentAsync()
        {
            return await equipmentManagementService.AddMarineEquipmentAsync();
        }
        /// <summary>
        /// 特种设备月报自动生成
        /// </summary>
        /// <param name="searchEquipmentManagementRequestDto"></param>
        /// <returns></returns>
        [HttpPost("AddMSpecialEquipment")]
        [AllowAnonymous]
        public async Task<ResponseAjaxResult<bool>> AddMSpecialEquipmentAsync()
        {
            return await equipmentManagementService.AddMSpecialEquipmentAsync();
        }
        /// <summary>
        /// 陆域设备月报自动生成
        /// </summary>
        /// <param name="searchEquipmentManagementRequestDto"></param>
        /// <returns></returns>
        [HttpPost("AddLandEquipment")]
        [AllowAnonymous]
        public async Task<ResponseAjaxResult<bool>> AddLandEquipmentAsync()
        {
            return await equipmentManagementService.AddLandEquipmentAsync();
        }
        /// <summary>
        /// 删除设备信息
        /// </summary>
        /// <param name="searchEquipmentManagementRequestDto"></param>
        /// <returns></returns>
        [HttpPost("DeleteMarineEquipment")]
        [UnitOfWork]
        public async Task<ResponseAjaxResult<bool>> DeleteMarineEquipmentAsync([FromBody] BasePullDownResponseDto basePullDownResponseDto)
        {
            return await equipmentManagementService.DeleteMarineEquipmentAsync(basePullDownResponseDto);
        }
    }
}
