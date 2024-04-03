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
            var objectResult = context.Result as ObjectResult;
            if (objectResult != null)
                if (objectResult.StatusCode.HasValue)
                {
                    switch (objectResult.StatusCode.Value)
                    {
                        case 200:
                            if (objectResult.Value is not Result)
                            {
                                if (objectResult.Value is string)
                                    context.Result = new OkObjectResult(Result.Success(message: objectResult.Value.ToString()));
                                else
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
                    }
                }
        }
    }
}
