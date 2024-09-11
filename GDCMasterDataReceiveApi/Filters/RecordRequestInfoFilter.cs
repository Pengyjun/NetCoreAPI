using GDCMasterDataReceiveApi.Domain.Shared;
using GDCMasterDataReceiveApi.Domain.Shared.Utils;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Text;
using UtilsSharp;

namespace GDCMasterDataReceiveApi.Filters
{

    /// <summary>
    /// 记录请求信息拦截器
    /// </summary>
    public class RecordRequestInfoFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            var method = actionDescriptor.MethodInfo;
            var httpContext = context.HttpContext;

            //获取请求参数
            #region 请求参数
            RecordRequestInfo recordRequestInfo = new RecordRequestInfo()
            {
                RequestInfo = new RequestInfo(),
                SqlExecInfos = new List<SqlExecInfo>(),
                BrowserInfo = new BrowserInfo(),
                Exceptions = new Exceptions()
            };
            var currentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff");
            var requestMethod = context.HttpContext.Request.Method.ToUpper();
            if (requestMethod == "GET")
            {
                if (context.HttpContext.Request.QueryString.HasValue && !string.IsNullOrWhiteSpace(context.HttpContext.Request.QueryString.Value))
                {
                    recordRequestInfo.RequestInfo.Input= context.HttpContext.Request.QueryString.Value.Replace("?", "").TrimAll();

                }
            }
            else if (requestMethod == "POST")
            {
                try
                {
                    if (context.HttpContext.Request.ContentType!=null&&context.HttpContext.Request.ContentType.IndexOf("multipart/form-data") < 0)
                    {
                        context.HttpContext.Request.Body.Seek(0, SeekOrigin.Begin);
                        using (var reader = new StreamReader(context.HttpContext.Request.Body, Encoding.UTF8))
                        {
                            recordRequestInfo.RequestInfo.Input = await reader.ReadToEndAsync();
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
            #endregion

            #region 是否开启审计日志
            if (Convert.ToBoolean(AppsettingsHelper.GetValue("AuditLogs:IsOpen")))
            {
                recordRequestInfo.Id = Guid.NewGuid();
                //recordRequestInfo.ClientIpAddress = IpHelper.GetClientIp();
                recordRequestInfo.HttpMethod = requestMethod;
                recordRequestInfo.ApplicationName = "GDCMasterDataReceiveApi";
                recordRequestInfo.Url = context.HttpContext.Request.Path;
                recordRequestInfo.RequestTime = currentTime;
                recordRequestInfo.ActionMethodName = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ControllerName;
                recordRequestInfo.BrowserInfo.Browser = context.HttpContext.Request.Headers["User-Agent"].ToString();
                CacheHelper cache = new CacheHelper();
                //cache.Set(context.HttpContext.TraceIdentifier.ToLower(), JsonConvert.SerializeObject(recordRequestInfo), 30);
                int cacheSeconds = int.Parse(AppsettingsHelper.GetValue("Redis:DefaultKeyCacheSeconds"));
                await RedisUtil.Instance.SetAsync(context.HttpContext.TraceIdentifier.ToLower(), JsonConvert.SerializeObject(recordRequestInfo), cacheSeconds);
            }
            #endregion
            await next();
        }

    }
}
