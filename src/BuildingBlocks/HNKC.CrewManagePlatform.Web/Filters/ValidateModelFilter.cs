using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using HNKC.CrewManagePlatform.Models.CommonResult;
using System.Net;
using HNKC.CrewManagePlatform.Models.Enums;

namespace HNKC.CrewManagePlatform.Web.Filters

{
    public class ValidateModelFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var objectResult = context.Result as ObjectResult;
                var result = context.ModelState.Keys
                        .SelectMany(key => context.ModelState[key].Errors.Select(x => x.ErrorMessage))
                        .ToList();
                context.Result = new ObjectResult(result[0]);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            try
            {
                context.ExceptionHandled = true;
                context.Result = new JsonResult(Result.NoAuth(context.Exception.Message));
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            }
            catch (Exception ex)
            {

                
            }
        }
    }
}
