using HNKC.CrewManagePlatform.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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

    }
}
