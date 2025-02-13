using GHMonitoringCenterApi.Application.Contracts.Dto.ShipPlan;
using GHMonitoringCenterApi.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.IService.ShipPlan
{

   /// <summary>
   /// 接口层
   /// </summary>
    public interface IShipPlanService
    {


        /// <summary>
        /// 保存船舶年度计划
        /// </summary>
        /// <param name="shipPlanRequestDto"></param>
        /// <returns></returns>

        Task<ResponseAjaxResult<bool>> SaveShipPlanAsync(SaveShipPlanRequestDto  saveShipPlanRequestDto);



        /// <summary>
        /// 保存船舶年度完成产值
        /// </summary>
        /// <param name="shipPlanRequestDto"></param>
        /// <returns></returns>

        Task<ResponseAjaxResult<bool>> SaveShipCompleteAsync(SaveShipCompleteRequestDto  saveShipCompleteRequestDto);


        /// <summary>
        /// 搜索船舶年度计划列表
        /// </summary>
        /// <param name="shipPlanRequestDto"></param>
        /// <returns></returns>

        Task<ResponseAjaxResult<List<ShipPlanResponseDto>>> SearchShipPlanAsync(ShipPlanRequestDto shipPlanRequestDto);


        /// <summary>
        /// 搜索船舶年度完成列表
        /// </summary>
        /// <param name="shipPlanRequestDto"></param>
        /// <returns></returns>

        Task<ResponseAjaxResult<List<ShipCompleteResponseDto>>> SearchShiCompleteAsync(ShipCompleteRequestDto shipPlanRequestDto);





    }
}
