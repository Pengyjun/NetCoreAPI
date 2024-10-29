using HNKC.Demo.Models.CommonResult;
using HNKC.Demo.Web.ActionResults;
using HNKC.Demo.Web.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HNKC.Demo.Web.Filters
{
    public class APIResultFilter : IResultFilter
    {
        public void OnResultExecuted(ResultExecutedContext context)
        {

        }


        public void OnResultExecuting(ResultExecutingContext context)
        {
            var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            var method = actionDescriptor.MethodInfo;

            // 如果定义无需进入过滤器，则跳过
            if (method.IsDefined(typeof(UnResultFilterAttribute), true))
            {
                return;
            }
            //ObjectResult
            if (context.Result is ObjectResult objectResult)
            {
                if (objectResult.StatusCode.HasValue)
                    switch (objectResult.StatusCode.Value)
                    {
                        case 200:
                            if (objectResult is OkMessageObjectResult)
                                context.Result = new OkObjectResult(Result.Success(message: objectResult.Value.ToString()));
                            else
                                context.Result = new OkObjectResult(Result.Success(data: objectResult.Value, message: "获取数据成功"));
                            break;
                        case 401:
                            context.Result = new UnauthorizedObjectResult(Result.NoAuth());
                            break;
                        case 400:
                            if (!context.ModelState.IsValid)
                            {
                                var result = context.ModelState.Keys
                                        .SelectMany(key => context.ModelState[key].Errors.Select(x => x.ErrorMessage))
                                        .ToList();
                                context.Result = new BadRequestObjectResult(Result.Fail(result[0], 400));
                            }
                            break;
                        case 500:
                            context.Result = new InternalServerErrorObjectResult(Result.Fail("服务器异常"));
                            break;
                        default:
                            break;
                    }
            }
            //StatusCodeResult
            else if (context.Result is StatusCodeResult statusCodeResult)
            {
                switch (statusCodeResult.StatusCode)
                {
                    case 200:
                        context.Result = new OkObjectResult(Result.Success(message: "调用成功"));
                        break;
                    case 401:
                        context.Result = new UnauthorizedObjectResult(Result.NoAuth());
                        break;
                    case 400:
                        if (!context.ModelState.IsValid)
                        {
                            var result = context.ModelState.Keys
                                    .SelectMany(key => context.ModelState[key].Errors.Select(x => x.ErrorMessage))
                                    .ToList();
                            context.Result = new BadRequestObjectResult(Result.Fail(result[0]));
                        }
                        break;
                    case 500:
                        context.Result = new InternalServerErrorObjectResult(Result.Fail("服务器异常"));
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
