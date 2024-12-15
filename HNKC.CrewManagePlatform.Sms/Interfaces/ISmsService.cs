
using HNKC.CrewManagePlatform.Sms.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.CrewManagePlatform.Sms.Interfaces
{
    /// <summary>
    /// 短信服务接口
    /// </summary>
    public interface ISmsService
    {
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="phones">接收短信的手机号码。格式：国内短信：无任何前缀的11位手机号码，例如1381111****。多个手机号码使用英文","隔开，最多支持一次提交200个手机号码。</param>
        /// <param name="smsParam">短信参数</param>       
        /// <returns></returns>
        Task<SmsApiResult> SendSmsAsync(SmsRequest smsParam);

        /// <summary>
        /// 查询短信发送状态
        /// </summary>
        /// <param name="phones"></param>
        /// <param name="smsParam"></param>
        /// <returns></returns>
        Task<SmsApiResult> QuerySendDetailsAsync(SmsRequest smsParam);
    }
}
