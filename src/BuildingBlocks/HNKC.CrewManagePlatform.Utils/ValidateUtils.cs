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
        public static bool ValidatePhone(string? phone)
        {
            if (string.IsNullOrEmpty(phone)) return false;

            // 手机号正则表达式（中国手机号）
            string pattern = @"^1[3-9]\d{9}$";
            return Regex.IsMatch(phone, pattern);
        }
        // 正则表达式：匹配中国家庭固话号码
        private static readonly string PhonePattern = @"^(0\d{2,3})-(\d{7,8})$";
        /// <summary>
        /// 中国家庭固话号码
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public static bool ValidatePhoneNumber(string? phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                return false;
            }

            // 使用正则表达式进行匹配
            Regex regex = new Regex(PhonePattern);
            return regex.IsMatch(phoneNumber);
        }
    }
}
