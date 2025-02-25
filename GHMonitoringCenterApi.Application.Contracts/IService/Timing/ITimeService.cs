using GHMonitoringCenterApi.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.IService.Timing
{

    /// <summary>
    /// 定时向POM系统拉取数据  接口层
    /// </summary>
    public interface ITimeService
    {
        /// <summary>
        /// 获取远程服务响应数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parame">根据不同参数获取不同类型的数据</param>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> GetServiceResponse<T>(int parame = 1);
        /// <summary>
        /// 定时同步每个公司的每天的完成产值汇总 
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> SynchronizationDayProductionAsync();

        /// <summary>
        /// 每月26号凌晨12点刷新项目信息  供日报推送使用
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> SynchronizationProjectAsync();

        Task<string> SynchronizationDealUnitsync();

        Task<string> SynchronizationDealUnitNewsync();
        Task<bool> GetSpecialConstruction();
    }
}
