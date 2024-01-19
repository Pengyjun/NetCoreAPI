
using Newtonsoft.Json;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.JjtSendMsg
{
    /// <summary>
    /// 交建通消息提醒列表
    /// </summary>
    public class JjtSendMsgResponseDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public Guid? Id { get; set; }
        /// <summary>
        /// 消息内容
        /// </summary>
        public string? MsgContent { get; set; }
        /// <summary>
        /// 1 生产运营监控日报
        /// </summary>
        public string? TextTypeContent { get; set; }
        /// <summary>
        /// 已提醒人员
        /// </summary>
        public List<string>? UserName { get; set; }
        /// <summary>
        /// 预提醒人员
        /// </summary>
        public List<string>? ExpectUserName { get; set; }
        /// <summary>
        /// 第一次提醒时间
        /// </summary>
        public string? FTime { get; set; }
        /// <summary>
        /// 第二次提醒时间
        /// </summary>
        public string? STime { get; set; }
        /// <summary>
        /// 当天
        /// </summary>
        public int NowDay { get; set; }
        /// <summary>
        /// 发送次数
        /// </summary>
        public int SendCount { get; set; }

    }
}
