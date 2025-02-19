using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GHMonitoringCenterApi.Filters
{
    /// <summary>
    /// 自定义鉴权
    /// </summary>

    public class CustomFixedAuthAttribute : Attribute, IAuthorizationFilter
    {
        private const string Token = "yJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9";

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue("API-KEY", out var apiKey) || apiKey != Token)
            {
                context.Result = new UnauthorizedResult(); // 返回 401 Unauthorized
            }
        }
    }
}
