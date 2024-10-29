using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using Microsoft.Extensions.Logging;
using HNKC.Exceptions;
using HNKC.Models.CommonResult;
using HNKC.Web.ActionResults;

namespace HNKC.Web.Filters
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
