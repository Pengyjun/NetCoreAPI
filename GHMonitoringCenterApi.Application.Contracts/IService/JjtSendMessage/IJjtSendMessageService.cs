using CDC.MDM.Core.Common.Util;
using GHMonitoringCenterApi.Application.Contracts.Dto;
using GHMonitoringCenterApi.Application.Contracts.Dto.JjtSendMsg;
using GHMonitoringCenterApi.Application.Contracts.Dto.Ship;
using GHMonitoringCenterApi.Domain.Shared;
using Microsoft.AspNetCore.Http;

namespace GHMonitoringCenterApi.Application.Contracts.IService.JjtSendMessage
{
    public interface IJjtSendMessageService
    {
        /// <summary>
        /// 交建通单发
        /// </summary>
        /// <returns></returns>
        ResponseAjaxResult<bool> MessageSending(SingleMessageTemplateRequestDto singleMessageTemplateRequestDto);
        /// <summary>
        /// jjt发送消息数值 卡片信息提示广航生产运营监控日报
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> JjtSendTextCardMsgForGHDayRep(bool isFirst);
        /// <summary>
        ///  jjt发送消息  项目异常预警数据
        /// </summary>
        /// <returns></returns>
        //Task<ResponseAjaxResult<bool>> JjtSendTextMsgForGHDayRep();
        /// <summary>
        /// 获取交建通发送消息列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<JjtSendMsgResponseDto>>> GetJjtSendMsgResponseSearchAsync(JjtSendMsgRequestDto requestDto);
        /// <summary>
        /// 编辑交建通发送内容
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> ModifyJjtSendMsgContentAsync(JjtSendMsgDataRequestDto requestDto);
        /// <summary>
        /// 获取交建通分配用户列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<JjtSendMsgUsersResponseDto>>> GetSearchJjtSendMsgUsersAsync(JjtSendMsgUsersRequestDto requestDto);
        /// <summary>
        /// 获取交建通未分配用户列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<JjtSendMsgUsersResponseDto>>> GetSearchJjtNotSendMsgUsersAsync(JjtSendMsgUsersRequestDto requestDto);
        /// <summary>
        /// 增改交建通发消息用户
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> InsOrModifyJjtSendMsgUsersAsync(JjtSendMsgModifyUsersRequestDto requestDto);
        /// <summary>
        /// 获取卡片消息详情
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<JjtSendMsgDetailsResponse>> JjtSendTextCardMsgDetailsAsync();
        /// <summary>
        /// 文件上上传
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> UploadJjtSendTextDetailsImage(IFormFile file);
        /// <summary>
        /// 项目日报未报通知
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> JjtSendTextMsgForProjectDayRep();
        /// <summary>
        /// 项目安监日报未报通知
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> JjtSendTextMsgForProjectSafeDayRep();
        /// <summary>
        /// 交建通发送项目船舶日报通知消息
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> JjtSendTextMsgForProjectShipDayRep();
        /// <summary>
        /// 交建通发送项目月报未报通知消息
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> JjtSendTextMsgForProjectMonthRep();


        /// <summary>
        /// 新版交建通发消息 监控运营中心图片消息
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<JjtSendMessageMonitoringDayReportResponseDto>> JjtTextCardMsgDetailsAsync(int dateDay = 0,bool flag=true);

        /// <summary>
        /// 交建公司生产日报推送
        /// </summary>
        /// <param name="dateDay"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<JjtSendMessageMonitoringDayReportResponseDto>> JjtDayReportPushAsync(int dateDay = 0);

     
        /// <summary>
        /// 获取自有船舶日报卡片消息详情
        /// </summary>
        /// <param name="dateDay"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<JjtOwnShipDayRepDto>> JjtOwnShipTextCardMsgDetailsAsync(int dateDay = 0);

        /// <summary>
        /// 项目日报未填报通知
        /// </summary>
        /// <param name="isTimingTask">是否是定时任务调用</param>
        /// <param name="isFirst">是否是第一次执行</param>
        /// <returns></returns>

        Task<ResponseAjaxResult<bool>> ProjectUnDayReportNotifAsync(bool isTimingTask, bool isFirst);

        /// <summary>
        /// 生产监控系统使用接口 其他系统不适用  本系统也不使用
        /// </summary>
        /// <returns></returns>

        //Task<CompanyDayProductionValueResponseDto> SearchCompanyProductionValueAsync();

    }
}
