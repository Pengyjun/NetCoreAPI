
using GHMonitoringCenterApi.Application.Contracts.Dto.Ship;
using GHMonitoringCenterApi.Domain.Shared;

namespace GHMonitoringCenterApi.Application.Contracts.IService.Ship
{
    /// <summary>
    /// 船舶成本维护接口
    /// </summary>
    public interface IShipService
    {
        /// <summary>
        /// 船机成本维护列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<ShipCostMaintenanceResponseDto>>> SearchShipCostMaintenanceAsync(ShipCostMaintenanceRequestDto requestDto);

        /// <summary>
        /// 船舶 and 分类 列表返回
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<SearchOwnShipAndShipTypeDto>> SearchOwnShipAndShipTypeAsync();

        /// <summary>
        /// 船舶成本维护增改
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> AddOrModifyShipCostMaintenanceAsync(AddOrModifyShipCostMaintenaceRequestDto requestDto);

        /// <summary>
        /// 船舶成本维护删除
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> DeleteShipCostMaintenanceAsync(DeleteShipCostMaintenanceRequestDto requestDto);

    }
}
