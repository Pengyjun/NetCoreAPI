using HNKC.CrewManagePlatform.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Pinyin4net;
namespace HNKC.CrewManagePlatform.Utils
{


    /// <summary>
    /// 常用工具类集合
    /// </summary>
    public class Utils
    {
        #region 获取IP地址
        public static string GetIP()
        {
            var ip4 = HttpContentAccessFactory.Current.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrEmpty(ip4))
            {
                ip4 = HttpContentAccessFactory.Current.Request.Headers["X-Forwarded-Proto"].FirstOrDefault();
            }
            if (string.IsNullOrEmpty(ip4))
            {
                ip4 = HttpContentAccessFactory.Current.Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            }
            return ip4;
        }
        #endregion

        #region 获取拼音首字母
        public static string GetPinyinInitials(string chineseName)
        {
            string initials = string.Empty;
            foreach (var ch in chineseName)
            {
                var pinyinArray = PinyinHelper.ToHanyuPinyinStringArray(ch);
                if (pinyinArray != null && pinyinArray.Length > 0)
                {
                    initials += pinyinArray[0][0];  // 提取拼音的首字母
                }
            }
            return initials.ToUpper();  // 返回大写首字母
        }
        #endregion

    }
}
