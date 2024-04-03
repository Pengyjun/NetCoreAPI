using CH.Simple.Models.CommonResult;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CH.Simple.Web.Filters
{
    public class APIResultFilter: IResultFilter
    {
        public void OnResultExecuted(ResultExecutedContext context)
        {

        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            //ObjectResult
            if (context.Result is ObjectResult objectResult)
            {
                if (objectResult.StatusCode.HasValue)
                    switch (objectResult.StatusCode.Value)
                    {
                        case 200:
                            if (objectResult.Value is not Result)
                            {
                                context.Result = new OkObjectResult(Result.Success(data: objectResult.Value, message: "获取数据成功"));
                            }
                            break;
                        case 401:
                            context.Result = new OkObjectResult(Result.NoAuth());
                            break;
                        case 400:
                            if (!context.ModelState.IsValid)
                            {
                                var result = context.ModelState.Keys
                                        .SelectMany(key => context.ModelState[key].Errors.Select(x => x.ErrorMessage))
                                        .ToList();
                                context.Result = new OkObjectResult(Result.Fail(result[0]));
                            }
                            break;
                        case 500:
                            context.Result = new OkObjectResult(Result.Fail("服务器异常"));
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
                        context.Result = new OkObjectResult(Result.Success(message: "响应成功"));
                        break;
                    case 401:
                        context.Result = new OkObjectResult(Result.NoAuth());
                        break;
                    case 400:
                        if (!context.ModelState.IsValid)
                        {
                            var result = context.ModelState.Keys
                                    .SelectMany(key => context.ModelState[key].Errors.Select(x => x.ErrorMessage))
                                    .ToList();
                            context.Result = new OkObjectResult(Result.Fail(result[0]));
                        }
                        break;
                    case 500:
                        context.Result = new OkObjectResult(Result.Fail("服务器异常"));
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
