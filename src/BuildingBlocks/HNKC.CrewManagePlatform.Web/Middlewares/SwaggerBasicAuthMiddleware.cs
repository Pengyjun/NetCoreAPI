using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace HNKC.CrewManagePlatform.Web.Middlewares
{
    public class SwaggerBasicAuthMiddleware
    {
        private readonly RequestDelegate next;

        private readonly string swaggerUserName = "admin";
        private readonly string swaggerPassword = "Zjhn#112233";

        public SwaggerBasicAuthMiddleware(RequestDelegate next)
        {
            this.next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/swagger"))
            {
                string authHeader = context.Request.Headers["Authorization"];
                if (authHeader != null && authHeader.StartsWith("Basic"))
                {
                    // Get the credentials from request header
                    var header = AuthenticationHeaderValue.Parse(authHeader);
                    var inBytes = Convert.FromBase64String(header.Parameter);
                    var credentials = Encoding.UTF8.GetString(inBytes).Split(':');
                    var username = credentials[0];
                    var password = credentials[1];
                    // validate credentials
                    if (username.Equals(swaggerUserName) && password.Equals(swaggerPassword))
                    {
                        await next.Invoke(context).ConfigureAwait(false);
                        return;
                    }
                }
                //告知服务器端需要进行Basic认证
                context.Response.Headers["WWW-Authenticate"] = "Basic";
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            }
            else
            {
                await next.Invoke(context).ConfigureAwait(false);

                //await next.Invoke(context);
            }
        }

    }

    public static class SwaggerBasicAuthExtensions
    {
        /// <summary>
        /// 使用异常中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseSwaggerBasicAuth(this IApplicationBuilder app)
        {
            return app.UseMiddleware<SwaggerBasicAuthMiddleware>();
        }
    }
}
