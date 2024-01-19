using GHMonitoringCenterApi.Application.Contracts.Dto;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project.ShipMovements;
using GHMonitoringCenterApi.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.IService.Project
{
    /// <summary>
    /// 项目-船舶动向业务层
    /// </summary>
    public interface IProjectShipMovementsService
    {
        /// <summary>
        /// 新增船舶进出场
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> AddShipMovementAsync(AddShipMovementRequestDto model);

        /// <summary>
        /// 更改船舶进出场
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> ChangeShipMovementStatusAsync(ChangeShipMovementStatusRequestDto model);

        /// <summary>
        ///  移除船舶进出场
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> RemoveShipMovementAsync(RemoveShipMovementRequestDto model);

        /// <summary>
        /// 项目-搜索船舶动向列表
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<ShipMovementResponseDto>>> SearchShipMovementsAsync(ShipMovementsRequestDto model);

        /// <summary>
        /// 项目-搜索进场船舶（自有船舶）
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<EnterShipsResponseDto>> SearchEnterShipsAsync(EnterShipsRequestDto model);

    }
}
