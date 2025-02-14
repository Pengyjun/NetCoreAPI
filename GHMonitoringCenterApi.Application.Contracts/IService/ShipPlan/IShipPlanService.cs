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

        /// <summary>
        /// 根据用户填的船舶计划数据 生产图  
        /// </summary>
        /// <param name="type">1是项目为中心   2是船舶为中心</param>
        /// <returns></returns>
        Task<ResponseAjaxResult<ShipPlanImageResponseDto>> SearchShipPlanImagesAsync(int type);


        /// <summary>
        ///根据船舶完成产值 生成图
        /// </summary>
        /// <param name="type">是自有年度产值计划与完成对比图 </param>
        /// <returns></returns>
        Task<ResponseAjaxResult<ShipPlanCompleteResponseDto>> SearchShipCompleteImagesAsync();

    }
}
