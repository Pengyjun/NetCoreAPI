using System.Text.RegularExpressions;

namespace HNKC.CrewManagePlatform.Utils
{
    /// <summary>
    /// 身份证校验
    /// </summary>
    public class IdCardUtils
    {
        /// <summary>
        /// 身份证校验
        /// </summary>
        /// <param name="idCard"></param>
        /// <returns></returns>
        public static bool ValidateIdCard(string idCard)
        {
            if (string.IsNullOrEmpty(idCard)) return false;

            // 校验长度和数字格式
            if (idCard.Length != 18 || !Regex.IsMatch(idCard, @"^\d{17}(\d|X)$"))
                return false;

            // 校验出生日期是否合法
            string birthDateStr = idCard.Substring(6, 8); // 从第7位到第14位为出生日期
            DateTime birthDate;
            if (!DateTime.TryParseExact(birthDateStr, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out birthDate))
                return false;

            // 校验校验位
            return CheckIdCardChecksum(idCard);
        }
        /// <summary>
        /// 校验身份证最后一位校验位
        /// </summary>
        /// <param name="idCard"></param>
        /// <returns></returns>
        private static bool CheckIdCardChecksum(string idCard)
        {
            // 系统加权因子
            int[] weight = { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };
            // 校验码映射表
            char[] checkDigits = { '1', '0', 'X', '9', '8', '7', '6', '5', '4', '3', '2' };

            int sum = 0;
            for (int i = 0; i < 17; i++)
            {
                sum += int.Parse(idCard[i].ToString()) * weight[i];
            }

            int mod = sum % 11;
            char expectedCheckDigit = checkDigits[mod];
            return idCard[17] == expectedCheckDigit;
        }
    }
}
