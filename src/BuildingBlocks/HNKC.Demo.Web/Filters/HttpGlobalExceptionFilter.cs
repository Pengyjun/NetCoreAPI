using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using Microsoft.Extensions.Logging;
<<<<<<<< HEAD:src/BuildingBlocks/HNKC.CrewManagePlatform.Web/Filters/HttpGlobalExceptionFilter.cs
using HNKC.CrewManagePlatform.Exceptions;
using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Web.ActionResults;

namespace HNKC.CrewManagePlatform.Web.Filters
========
using HNKC.Demo.Exceptions;
using HNKC.Demo.Models.CommonResult;
using HNKC.Demo.Web.ActionResults;

namespace HNKC.Demo.Web.Filters
>>>>>>>> 7fd224848dc4910963de00d8c3a15a3418dc1847:src/BuildingBlocks/HNKC.Demo.Web/Filters/HttpGlobalExceptionFilter.cs
{
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var loggerFactory = context.HttpContext?.RequestServices?.GetService(typeof(ILoggerFactory)) as ILoggerFactory;
            var logger = loggerFactory.CreateLogger(GetType());
            switch (context.Exception)
            {
                case ResultMessageException:
                    context.Result = new OkObjectResult(Result.Fail(context.Exception.Message));
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                    context.ExceptionHandled = true;
                    break;
                case UnauthorizedException:
                    logger.LogError(JsonConvert.SerializeObject(context.Exception));
                    context.Result = new UnauthorizedObjectResult(Result.NoAuth(context.Exception.Message));
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    context.ExceptionHandled = true;
                    break;
                default:
                    logger.LogError(JsonConvert.SerializeObject(context.Exception));
                    context.Result = new InternalServerErrorObjectResult(Result.Fail("服务器错误,请重试."));
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    return;
            }
        }
    }
}
