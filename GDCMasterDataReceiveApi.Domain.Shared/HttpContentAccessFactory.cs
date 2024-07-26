using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace GDCMasterDataReceiveApi.Domain.Shared
{

    /// <summary>
    /// 访问httpContext的工厂
    /// </summary>
    public class HttpContentAccessFactory
    {
        public static IServiceCollection services { get; set; }
        public static  HttpContext Current
        {
            get
            {
                var factory = services.BuildServiceProvider().GetService(typeof(IHttpContextAccessor));
                return ((HttpContextAccessor)factory).HttpContext;
            }
        }
        /// <summary>
        /// 获取当前用户Token
        /// </summary>
        public static string GetUserToken
        {
            get
            {
                var factory = services.BuildServiceProvider().GetService(typeof(IHttpContextAccessor));
                var token= ((HttpContextAccessor)factory).HttpContext.Request.Headers["Authorization"].ToString();
                if (!string.IsNullOrWhiteSpace(token))
                {
                  token = token.Replace("Bearer", "").Trim();
                }
                return token;
            }
        }

    }
}
