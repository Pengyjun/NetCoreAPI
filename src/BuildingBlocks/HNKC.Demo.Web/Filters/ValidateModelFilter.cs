using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

<<<<<<<< HEAD:src/BuildingBlocks/HNKC.CrewManagePlatform.Web/Filters/ValidateModelFilter.cs
namespace HNKC.CrewManagePlatform.Web.Filters
========
namespace HNKC.Demo.Web.Filters
>>>>>>>> 7fd224848dc4910963de00d8c3a15a3418dc1847:src/BuildingBlocks/HNKC.Demo.Web/Filters/ValidateModelFilter.cs
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

        }
    }
}
