using CH.Simple.Web.ActionResults;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CH.Simple.Exceptions;
using CH.Simple.Utils;
using CH.Simple.Web.Models;

namespace CH.Simple.Web.Filters
{
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception.GetType() == typeof(SimpleException))
            {
                context.Result = new OkObjectResult(Result.Fail(context.Exception.Message));
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
            }
            else
            {
                LogUtil.Error(JsonConvert.SerializeObject(context.Exception));
                context.Result = new InternalServerErrorObjectResult(Result.Fail("服务器错误,请重试."));
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return;
            }
            context.ExceptionHandled = true;
        }
    }
}
