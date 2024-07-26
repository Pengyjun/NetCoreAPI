using Newtonsoft.Json;
using System.Text.Encodings.Web;
namespace GDCMasterDataReceiveApi.Domain.Shared.WebSecurity
{

    /// <summary>
    /// web安全工具类
    /// </summary>
    public static class WebSecurityUtil
    {
        private static readonly HtmlEncoder _htmlEncoder = HtmlEncoder.Default;

        /// <summary>
        /// 字符串过滤html 可以有效防止XSS攻击
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string UserInputFilterByXSS(this string input)
        {
            return _htmlEncoder.Encode(input);
        }

        /// <summary>
        /// 对象过滤html 可以有效防止XSS攻击
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string UserInputFilterByXSS(this object input)
        {
            return _htmlEncoder.Encode(JsonConvert.SerializeObject(input));
        }
    }
}
