


using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Encodings.Web;
using HNKC.CrewManagePlatform.Sms.Interfaces;
using HNKC.CrewManagePlatform.Sms.Model;
using HNKC.IMPS.Sms.Options;
using UtilsSharp;

namespace HNKC.CrewManagePlatform.Sms.Services
{
    /// <summary>
    /// 天翼云短信服务
    /// </summary>
    /// <param name="clientFactory"></param>
    public class CtyunSmsService(IHttpClientFactory clientFactory) : ISmsService
    {
        private const int MAX_PHONE_NUM = 200;

        
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="smsParam"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<SmsApiResult> SendSmsAsync(SmsRequest smsParam)
        {
            #region 参数验证
            if (smsParam is not SmsRequest parame)
            {
                return SmsApiResult.Fail("参数异常");
            }

            if (string.IsNullOrWhiteSpace(smsParam.PhoneNumber))
            {
                return SmsApiResult.Fail("请输入正确的手机号");
            }

            var phoneArr = smsParam.PhoneNumber.Split(',');

            if (phoneArr.Length > MAX_PHONE_NUM)
            {
                return SmsApiResult.Fail($"最多支持一次提交{MAX_PHONE_NUM}个手机号码");
            }

            #endregion

            try
            {
                var requestId = CtyunHelper.GenerateRequestId(); //生成请求id
                var eopDate = CtyunHelper.ConstructTimestamp(); //生成时间戳
                var type = phoneArr.Length > 1 ? 2 : 1;//1是群发 2是个人发
                var requestBody = CtyunHelper.GetSortedBody(parame, smsParam.PhoneNumber, type);
                string body = JsonSerializer.Serialize(requestBody,
                              new JsonSerializerOptions
                              {
                                  PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                                  Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                              });

                string signString = CtyunHelper.BuildSignatureString(requestId, eopDate, body, null);

                string signature = CtyunHelper.GenerateSignature(AppsettingsHelper.GetValue("CtyunSms:SecurityKey"), AppsettingsHelper.GetValue("CtyunSms:AccessKey"), signString, eopDate);

                var requestHeaders = CtyunHelper.BuildRequestHeaders(AppsettingsHelper.GetValue("CtyunSms:AccessKey"), eopDate, requestId, signature);


                var client = clientFactory.CreateClient();

                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                foreach (var item in requestHeaders)
                {
                    client.DefaultRequestHeaders.Add(item.Key, item.Value);
                }

                var data = new StringContent(body, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(AppsettingsHelper.GetValue("CtyunSms:SendSmsUrl"), data);

                string responseStr = await response.Content.ReadAsStringAsync();

                /*
                  成功时：  {"requestId":"cstfrb1prv7qcjqhb3bg","statusCode":10000,"message":"成功"}

                 失败时：   {"requestId":"c6f1cd6d8a404533b461c13a2ab5dfb6","error":"10009","code":"10009","message":"ctyun-EOP: signature verification failed","eopErrCode":"10009","statusCode":"CTAPI_10009","returnObj":{}}
                 */

                Console.WriteLine(responseStr);

                if (responseStr.Contains("成功"))
                {
                    return SmsApiResult.Success(requestId, responseStr);
                }
                else
                {
                    return SmsApiResult.Fail(requestId, responseStr);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return SmsApiResult.Fail("", ex.ToString());
            }
        }
        /// <summary>
        /// 短信查看结果
        /// </summary>
        /// <param name="smsParam"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<SmsApiResult> QuerySendDetailsAsync(SmsRequest smsParam)
        {
            throw new NotImplementedException();
        }
    }
}
