using HNKC.CrewManagePlatform.Sms.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.IMPS.Sms.Options
{
    /// <summary>
    /// 天翼云通信帮助类
    /// <see cref="https://www.ctyun.cn/document/10020426/10021549"/>
    /// </summary>
    public static class CtyunHelper
    {

        /// <summary>
        /// 构造一个eop-date的时间戳
        /// </summary>
        /// <param name="format"></param>
        /// <param name="useUtc"></param>
        /// <returns></returns>
        public static string ConstructTimestamp( string format= "yyyyMMddTHHmmssZ", bool useUtc = false)
        {
            return useUtc ? DateTime.UtcNow.ToString(format) : DateTime.Now.ToString(format);         
        }

        /// <summary>
        /// 构造请求流水号
        /// </summary>
        /// <returns></returns>
        public static string GenerateRequestId()
        {
            return Guid.NewGuid().ToString();
        }
       
        /// <summary>
        /// 构造待签名字符串
        /// </summary>
        /// <param name="headers"></param>
        /// <param name="bodyJsonStr"></param>
        /// <param name="queryParams"></param>
        /// <returns></returns>
        public static string BuildSignatureString(string requestId, string eopDate, string bodyJsonStr, Dictionary<string, string>? queryParams)
        {

            var headerString = $"ctyun-eop-request-id:{requestId}\neop-date:{eopDate}\n";
          
            var sortedQuery = queryParams?.OrderBy(q => q.Key).Select(q => $"{q.Key}={q.Value}").ToList();
            string? queryString = sortedQuery is null ? null : string.Join("&", sortedQuery);
           
            string bodySha256 = Sha256(bodyJsonStr);
           
            return $"{headerString}\n{queryString}\n{bodySha256}";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <param name="phone"></param>
        /// <param name="type">群发还是个人发生  1是群发 2是个人发送</param>
        /// <returns></returns>
        public static SortedDictionary<string, string> GetSortedBody(SmsRequest param, string phone,int  type)
        {
            var sortDic = new SortedDictionary<string, string>
                {

                    { "action", param.Action },                      //固定参数
                    { "phoneNumber",  phone},                        //请填写接收短信的目标手机号，多个手机号使用英文逗号分开  
                    { "signName", param.SignName },                   //请填写您在控制台上申请并通过的短信签名。  
                    { "templateCode", param.TemplateCode.Where(x=>x.Type==type).Select(x=>x.Value).First()},         //请填写您在控制台上申请并通过的短信模板，此模板为测试专用模板，可直接进行测试
                    //{ "templateParam", param.TemplateParamJson}, //请填写短信模板对应的模板参数和值。此值为测试模板的变量及参数，可直接使用
                    //{ "extendCode", "" },                         //可选，非必填
                    //{ "sessionId", "" }                           //可选，非必填
                };
            if (!string.IsNullOrWhiteSpace(param.TemplateParam))
            {
                sortDic.Add("templateParam", param.TemplateParam);
            }

            //if (!string.IsNullOrWhiteSpace(param.ExtendCode))
            //{
            //    sortDic.Add("extendCode", param.ExtendCode);
            //}

            //if (!string.IsNullOrWhiteSpace(param.SessionId))
            //{
            //    sortDic.Add("sessionId", param.SessionId);
            //}

            return sortDic;
        }


        /// <summary>
        /// 生成签名串
        /// </summary>
        /// <param name="securityKey"></param>
        /// <param name="accessKey"></param>
        /// <param name="signString"></param>
        /// <param name="eopDate"></param>
        /// <returns></returns>
        public static string GenerateSignature(string securityKey,string accessKey, string signString, string eopDate)
        {
            var time = eopDate[..8];

            var ktime = HmacSHA256(Encoding.UTF8.GetBytes(eopDate), Encoding.UTF8.GetBytes(securityKey));
            var kAk = HmacSHA256(Encoding.UTF8.GetBytes(accessKey), ktime);
            var kdate = HmacSHA256(Encoding.UTF8.GetBytes(time), kAk);
            var signature = Convert.ToBase64String(HmacSHA256(Encoding.UTF8.GetBytes(signString), kdate));

            return signature;
        }

       
        /// <summary>
        /// 构建请求头
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="eopDate"></param>
        /// <param name="requestId"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        public static Dictionary<string, string> BuildRequestHeaders(string accessKey, string eopDate, string requestId, string signature)
        {
            return new Dictionary<string, string>
            {
                { "eop-date", eopDate },
                { "ctyun-eop-request-id", requestId },
                { "eop-Authorization", $"{accessKey} Headers=ctyun-eop-request-id;eop-date Signature={signature}" }
            };
        }

        /// <summary>
        /// 构建默认请求头
        /// </summary>
        /// <param name="eopDate"></param>
        /// <param name="requestId"></param>
        /// <returns></returns>
        public static Dictionary<string, string> BuildDefaultHeaders(string eopDate, string requestId)
        { 
            return new Dictionary<string, string>
            {
                { "ctyun-eop-request-id", requestId },
                { "eop-date", eopDate }
            };
        }



        private static byte[] HmacSHA256(byte[] data, byte[] signKey)
        {         
            using HMACSHA256 mac = new(signKey);
            byte[] hash = mac.ComputeHash(data);
            return hash;
        }

        private static string Sha256(string data)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            var hash = SHA256.HashData(bytes);

            var strBuilder = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                strBuilder.Append(hash[i].ToString("x2"));
            }
            return strBuilder.ToString();
        }

    }
}
