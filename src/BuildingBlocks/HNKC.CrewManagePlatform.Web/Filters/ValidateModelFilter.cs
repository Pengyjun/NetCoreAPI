﻿using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

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

        }
    }
}
