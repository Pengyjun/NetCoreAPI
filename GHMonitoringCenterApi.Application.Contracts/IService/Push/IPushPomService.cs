using GHMonitoringCenterApi.Domain.Shared;
using GHMonitoringCenterApi.Domain.Models;

namespace GHMonitoringCenterApi.Application.Contracts.IService.Push
{
    /// <summary>
    /// 推送到Pom的业务层
    /// </summary>
    public interface IPushPomService
    {
        /// <summary>
        /// 推送项目信息
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> PushProjectAsync();

        /// <summary>
        ///推送项目月报
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> PushMonthReportsAsync();
        /// <summary>
        /// 推送项目月报特殊字段
        /// </summary>
        /// <param name="monthRepId"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> PushSpecialFieldsMonthReportsAsync(Guid monthRepId);

        /// <summary>
        /// 自有船舶月报
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> PushOwnerShipMonthReportsAsync();

        /// <summary>
        /// 推送分包船舶月报
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> PushSubShipMonthReportsAsync();

        /// <summary>
        ///推送船舶日报
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> PushShipDayReportsAsync();

        /// <summary>
        ///推送项目日报
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> PushDayReportsAsync();

        /// <summary>
        ///推送安监日报
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> PushSafeDayReportsAsync();

        /// <summary>
        ///推送分包船舶
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> PushSubShipsAsync();

        /// <summary>
        ///推送生产日报数据
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> PushJjtTextCardMsgDetailsAsync();

        /// <summary>
        ///同步审核数据
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> PushApproveDataAsync();
    }

}
