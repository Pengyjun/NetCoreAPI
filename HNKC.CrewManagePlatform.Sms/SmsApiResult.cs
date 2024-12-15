namespace HNKC.CrewManagePlatform.Sms
{


    /// <summary>
    /// 手机短信发送接口
    /// </summary>
    public class SmsApiResult
    {
        /// <summary>
        /// 请求状态码。
        //返回OK代表请求被成功接收。
        // 实际短信发送状态，通过配置短信状态报告回调地址异步返回，错误码详见错误码列表。
        /// </summary>
        public string? Code { get; set; }
        /// <summary>
        /// 状态码描述。
        /// </summary>
        public string? Message { get; set; }
        /// <summary>
        /// 请求ID。多个手机号码的请求ID使用","隔开。
        /// </summary>
        public string? RequestId { get; set; }

        public bool IsSuccess { get; set; }
        /// <summary>
        /// 短信运营方返回的短信发送结果
        /// </summary>
        public string? Data { get; set; }

        public static SmsApiResult Fail(string requestId, string? data = null)
        {
            return new SmsApiResult
            {
                IsSuccess = false,
                RequestId = requestId,
                Data = data
            };
        }

        public static SmsApiResult Success(string requestId, string? data = null)
        {
            return new SmsApiResult
            {
                IsSuccess = true,
                RequestId = requestId,
                Data = data
            };
        }
    }
}
