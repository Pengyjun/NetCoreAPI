using HNKC.CrewManagePlatform.Models.CommonRequest;
using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Dtos.ConfigManagement;
using Microsoft.AspNetCore.Mvc;

namespace HNKC.CrewManagePlatform.Services.Interface.ConfigManagement
{
    /// <summary>
    /// 配置管理
    /// </summary>
    public interface IConfigManagementService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="shipRequest"></param>
        /// <returns></returns>
        Task<Result> SaveShipAsync(SaveShipRequest shipRequest);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        Task<PageResult<ShipSearch>> SearchShipAsync(ShipRequest requestBody);
        /// <summary>
        /// 提醒配置列表
        /// </summary>
        /// <returns></returns>
        Task <Result> SearchRemindSettingAsync();
        /// <summary>
        /// 保存提醒配置
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        Task<Result> SaveRemindSettingAsync(RemindRequest requestBody);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Result> DeleteShipAsync(string id);
    }
}
