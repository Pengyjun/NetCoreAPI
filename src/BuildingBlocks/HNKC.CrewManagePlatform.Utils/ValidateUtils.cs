using System.Text.RegularExpressions;

namespace HNKC.CrewManagePlatform.Utils
{
    public class ValidateUtils
    {
        /// <summary>
        /// 校验手机号
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static bool ValidatePhone(string phone)
        {
            if (string.IsNullOrEmpty(phone)) return false;

            // 手机号正则表达式（中国手机号）
            string pattern = @"^1[3-9]\d{9}$";
            return Regex.IsMatch(phone, pattern);
        }
    }
}
