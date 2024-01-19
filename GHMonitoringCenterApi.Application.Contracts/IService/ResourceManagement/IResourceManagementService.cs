using GHMonitoringCenterApi.Application.Contracts.Dto;
using GHMonitoringCenterApi.Application.Contracts.Dto.ResourceManagement;
using GHMonitoringCenterApi.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.IService.ResourceManagement
{
    /// <summary>
    /// 资源业务接口层
    /// </summary>
    public interface IResourceManagementService
    {
        /// <summary>
        /// 获取船舶列表
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<SearchShipTabulationRequestDto>>> SearchResourcesAsync(SearchShipTabulationResponseDto searchShipTabulationResponseDto);
        /// <summary>
        /// 保存船舶列表
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> SaveResourcesAsync(SaveShipPingTabulationResponseDto saveShipPingTabulationResponseDto);
        /// <summary>
        /// 删除船舶列表
        /// </summary>
        /// <param name="saveShipPingTabulationResponseDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> DeleteResourcesAsync(BasePrimaryRequestDto basePrimaryRequestDto);
        /// <summary>
        /// 获取船舶修理滚动计划表
        /// </summary>
        /// <param name="saveShipPingTabulationResponseDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<SearchShipRepairRollingResponseDto>>> SearchShipRepairRolling();
        /// <summary>
        /// 获取船舶修理滚动计划表
        /// </summary>
        /// <param name="saveShipPingTabulationResponseDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> SaveShipRepairRolling(SaveShipRepairRollingRequestDto saveShipRepairRollingRequestDto);
        /// <summary>
        /// 船舶排序
        /// </summary>
        /// <param name="ownShipId"></param>
        /// <returns></returns>
        int GetOwnShipSort(Guid? ownShipId);
    }
}
