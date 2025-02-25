using CDC.MDM.Core.Common.Util;
using GHMonitoringCenterApi.Application.Contracts.Dto;
using GHMonitoringCenterApi.Application.Contracts.Dto.JjtSendMsg;
using GHMonitoringCenterApi.Application.Contracts.Dto.Ship;
using GHMonitoringCenterApi.Application.Contracts.IService.JjtSendMessage;
using GHMonitoringCenterApi.CustomAttribute;
using GHMonitoringCenterApi.Domain.Shared;
using GHMonitoringCenterApi.Domain.Shared.Const;
using GHMonitoringCenterApi.Domain.Shared.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using UtilsSharp;
using Wangkanai.Detection.Models;
using Wangkanai.Detection.Services;
using static ICSharpCode.SharpZipLib.Zip.ExtendedUnixData;
namespace GHMonitoringCenterApi.Controllers.JjtSendMessage
{
    /// <summary>
    /// 交建通消息
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class JjtSendMessageController : BaseController
    {
        #region 依赖注入
        public IJjtSendMessageService jjtSendMessageService { get; set; }
        public IDetectionService  detectionService { get; set; }
        public JjtSendMessageController(IJjtSendMessageService jjtSendMessageService, IDetectionService detectionService)
        {
            this.jjtSendMessageService = jjtSendMessageService;
            this.detectionService = detectionService;
        }
        #endregion

        /// <summary>
        /// 交建通推送消息(单发或群发)
        /// </summary>
        /// <returns></returns>
        [HttpPost("MessageSending")]
        public ResponseAjaxResult<bool> MessageSendingAsync([FromBody] SingleMessageTemplateRequestDto singleMessageTemplateRequestDto)
        {
            return jjtSendMessageService.MessageSending(singleMessageTemplateRequestDto);
        }
        /// <summary>
        /// jjt发送消息数值 卡片信息提示广航生产运营监控日报
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("MsgTextCardForGHDayRep")]
        public Task<ResponseAjaxResult<bool>> JjtSendTextCardMsgForGHDayRep([FromQuery] bool isFirst)
        {
            return jjtSendMessageService.JjtSendTextCardMsgForGHDayRep(isFirst);
        }
        /// <summary>
        ///  jjt发送消息  项目异常预警数据
        /// </summary>
        /// <returns></returns>
        //[HttpPost("MsgTextForGHDayRep")]
        //public Task<ResponseAjaxResult<bool>> JjtSendTextMsgForGHDayRep()
        //{
        //    return jjtSendMessageService.JjtSendTextMsgForGHDayRep();
        //}
        /// <summary>
        /// 获取交建通发送消息列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet("GetSearchJjtMsg")]
        public async Task<ResponseAjaxResult<List<JjtSendMsgResponseDto>>> GetJjtSendMsgResponseSearchAsync([FromQuery] JjtSendMsgRequestDto requestDto)
        {
            return await jjtSendMessageService.GetJjtSendMsgResponseSearchAsync(requestDto);
        }
        /// <summary>
        /// 编辑交建通发送内容
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("ModifyJjtMsgContent")]
        public async Task<ResponseAjaxResult<bool>> ModifyJjtSendMsgContentAsync([FromBody] JjtSendMsgDataRequestDto requestDto)
        {
            return await jjtSendMessageService.ModifyJjtSendMsgContentAsync(requestDto);
        }
        /// <summary>
        /// 获取交建通分配用户列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet("GetJjtSendMsgUsers")]
        public async Task<ResponseAjaxResult<List<JjtSendMsgUsersResponseDto>>> GetSearchJjtSendMsgUsersAsync([FromQuery] JjtSendMsgUsersRequestDto requestDto)
        {
            return await jjtSendMessageService.GetSearchJjtSendMsgUsersAsync(requestDto);
        }
        /// <summary>
        /// 获取交建通未分配用户列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet("GetJjtNotSendMsgUsers")]
        public async Task<ResponseAjaxResult<List<JjtSendMsgUsersResponseDto>>> GetSearchJjtNotSendMsgUsersAsync([FromQuery] JjtSendMsgUsersRequestDto requestDto)
        {
            return await jjtSendMessageService.GetSearchJjtNotSendMsgUsersAsync(requestDto);
        }
        /// <summary>
        /// 增改交建通发消息用户
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("InsOrModifyJjtMsgUsers")]
        [UnitOfWork]
        public async Task<ResponseAjaxResult<bool>> InsOrModifyJjtSendMsgUsersAsync([FromBody] JjtSendMsgModifyUsersRequestDto requestDto)
        {
            return await jjtSendMessageService.InsOrModifyJjtSendMsgUsersAsync(requestDto);
        }
        // <summary>
        /// 获取卡片消息详情
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetJjtSendMsgDetails")]
        public async Task<ResponseAjaxResult<JjtSendMsgDetailsResponse>> JjtSendTextCardMsgDetailsAsync()
        {
            return await jjtSendMessageService.JjtSendTextCardMsgDetailsAsync();
        }
        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("GetImage")]
        public async Task<ResponseAjaxResult<bool>> UploadJjtSendTextDetailsImage(IFormFile file)
        {
            return await jjtSendMessageService.UploadJjtSendTextDetailsImage(file);
        }
        /// <summary>
        /// 发送未报项目日报消息
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("NoFillDayRep")]
        public async Task<ResponseAjaxResult<bool>> JjtSendTextMsgForProjectNoDayRep(bool isTimingTask, bool isFirst)
        {
            //return await jjtSendMessageService.JjtSendTextMsgForProjectDayRep();
            return await jjtSendMessageService.ProjectUnDayReportNotifAsync(isTimingTask, isFirst);
        }
        /// <summary>
        /// 发送未报安监日报消息
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("NoFillSafeRep")]
        public async Task<ResponseAjaxResult<bool>> JjtSendTextMsgForProjectSafeDayRep()
        {
            return await jjtSendMessageService.JjtSendTextMsgForProjectSafeDayRep();
        }
        /// <summary>
        /// 发送未报船舶日报消息
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("NoFillShipRep")]
        public async Task<ResponseAjaxResult<bool>> JjtSendTextMsgForProjectShipDayRep()
        {
            return await jjtSendMessageService.JjtSendTextMsgForProjectShipDayRep();
        }
        /// <summary>
        /// 交建通发送项目月报未报通知消息
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("NoFillProjectMonthRep")]
        public async Task<ResponseAjaxResult<bool>> JjtSendTextMsgForProjectMonthRep()
        {
            return await jjtSendMessageService.JjtSendTextMsgForProjectMonthRep();
        }


        /// <summary>
        /// 获取卡片消息详情 新版本  这个参数没有实际以及   方便调试使用
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetJjtTextCardMsgDetails")]
        [AllowAnonymous]
        public async Task<ResponseAjaxResult<JjtSendMessageMonitoringDayReportResponseDto>> JjtTextCardMsgDetailsAsync(int dateDay = 0)
        {
            if (detectionService.Device.Type == Device.Mobile)
            {
                return await jjtSendMessageService.JjtTextCardMsgDetailsAsync(dateDay, true);
            }
            else {
                return await jjtSendMessageService.JjtTextCardMsgDetailsAsync(dateDay);
            }
            
        }

        /// <summary>
        /// 交建公司日报推送
        /// </summary>
        /// <param name="dateDay"></param>
        /// <returns></returns>

        [HttpGet("JjtDayReportPush")]
        [AllowAnonymous]
        public async Task<ResponseAjaxResult<JjtSendMessageMonitoringDayReportResponseDto>> JjtDayReportPushAsync(int dateDay = 0)
        {
            return await jjtSendMessageService.JjtDayReportPushAsync(dateDay);
        }
        /// <summary>
        /// 获取自有船舶日报卡片消息详情
        /// </summary>
        /// <param name="dateDay"></param>
        /// <returns></returns>
        [HttpGet("GetJjtOwnShipTextCardMsgDetails")]
        [AllowAnonymous]
        public async Task<ResponseAjaxResult<JjtOwnShipDayRepDto>> JjtOwnShipTextCardMsgDetailsAsync(int dateDay = 0)
        {
            return await jjtSendMessageService.JjtOwnShipTextCardMsgDetailsAsync(dateDay);
        }



        #region 撤回消息  交建通每天发送生产日报数据（可撤回）
        /// <summary>
        /// 撤回消息
        /// </summary>
        /// <param name="hour">如果撤回的是九点的  则传9  撤回是10点的 则传10</param>
        /// <returns></returns>
        [HttpGet("RecallJjtMessage")]
        [AllowAnonymous]
        public async Task<ResponseAjaxResult<bool>> WithdrawJjtMessageAsync(int hour)
        {
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            try
            {
                var currentDay = DateTime.Now.ToString("yyyyMMdd");
                var url = AppsettingsHelper.GetValue("JjtPushMesssage:WithdrawMessage");
                WebHelper webHelper = new WebHelper();
                List<string> jobIds = new List<string>();
                if (hour == 9)
                {
                    jobIds.Add(await RedisUtil.Instance.GetAsync(currentDay + "one"));
                }
                else if (hour == 10)
                {
                    jobIds.Add(await RedisUtil.Instance.GetAsync(currentDay + "two"));
                    jobIds.Add(await RedisUtil.Instance.GetAsync(currentDay + "three"));
                    jobIds.Add(await RedisUtil.Instance.GetAsync(currentDay + "four"));
                }
                else if (hour >= 11 && hour <= 23)
                {
                    jobIds.Add(await RedisUtil.Instance.GetAsync(currentDay + "five"));
                }
                foreach (var item in jobIds)
                {
                    url = url.Replace("@jobId", item);
                    var responseResult = await webHelper.DoGetAsync(url);
                    var flag = Convert.ToBoolean(responseResult.Result);
                    responseAjaxResult.Data = flag;
                    if (flag)
                    {
                        responseAjaxResult.Success(ResponseMessage.OPERATION_RECALL_SUCCESS);
                    }
                    else
                    {
                        responseAjaxResult.Fail(ResponseMessage.OPERATION_RECALL_FAIL, Domain.Shared.Enums.HttpStatusCode.RecallFail);

                    }
                }
            }
            catch (Exception )
            {
                responseAjaxResult.Data = false;
                responseAjaxResult.Fail(ResponseMessage.OPERATION_RECALL_FAIL, Domain.Shared.Enums.HttpStatusCode.RecallFail);
            }
            return responseAjaxResult;
        }
        #endregion


       
    }
}
