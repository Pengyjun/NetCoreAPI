using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using UtilsSharp;
using HNKC.CrewManagePlatform.Utils;
using HNKC.CrewManagePlatform.Common;
using HNKC.CrewManagePlatform.Services.Interface.AuditLog;


namespace HNKC.CrewManagePlatform.Services.Admin.Api.Filters
{
    /// <summary>
    /// 记录请求信息拦截器
    /// </summary>
    public class RequestInfoFilter : IAsyncActionFilter
    {
        public IAuditLogService auditLogService { get; set; }
        public RequestInfoFilter(IAuditLogService auditLogService)
        {
            this.auditLogService = auditLogService;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var actionName = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ControllerName;
            #region 请求参数
            var requestParame = string.Empty;
            var requestMethod = context.HttpContext.Request.Method.ToUpper();
            if (requestMethod == "GET")
            {
                if (context.HttpContext.Request.QueryString.HasValue && !string.IsNullOrWhiteSpace(context.HttpContext.Request.QueryString.Value))
                {
                    requestParame = context.HttpContext.Request.QueryString.Value.Replace("?", "").Trim();

                }
            }
            else if (requestMethod == "POST")
            {
                try
                {
                    if (context.HttpContext.Request.ContentType != null && context.HttpContext.Request.ContentType.IndexOf("multipart/form-data") < 0)
                    {
                        context.HttpContext.Request.Body.Seek(0, SeekOrigin.Begin);
                        using (var reader = new StreamReader(context.HttpContext.Request.Body, Encoding.UTF8))
                        {
                            requestParame = await reader.ReadToEndAsync();
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
            #endregion

            #region 记录审计日志
            await auditLogService.SetupParameAsync(actionName, requestParame);
            #endregion

            await next();
        }
    }
}
