using GHMonitoringCenterApi.Application.Contracts.IService.User;
using GHMonitoringCenterApi.Controllers;
using GHMonitoringCenterApi.Domain.Shared;
using GHMonitoringCenterApi.Domain.Shared.Const;
using GHMonitoringCenterApi.Domain.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GHMonitoringCenterApi.Middleware
{
	/// <summary>
	/// 针对Authorize特性 
	/// </summary>
	public class CustomAuthorizationMiddleware : IAuthorizationMiddlewareResultHandler
	{
        public async Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult)
		{
			var endPoint = context.GetEndpoint();
			var controllerActionDescriptor = (ControllerActionDescriptor)endPoint.Metadata
			  .ToList().FirstOrDefault(d => d is ControllerActionDescriptor);
			var controllerName = controllerActionDescriptor.ControllerName;
			var actionName = controllerActionDescriptor.ActionName;

           
            //判断是否有匿名特性
            if (controllerActionDescriptor != null)
            {
                var isAllow = controllerActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
                if (isAllow)
                {
                    await next(context);
                }
            }
            ResponseAjaxResult<object> responseAjaxResult = new ResponseAjaxResult<object>()
            {
            };
            if (context.User.Identity == null)
            {
                responseAjaxResult.Fail(message: ResponseMessage.OPERATION_Token_FAIL, httpStatusCode: HttpStatusCode.TokenError);
                context.Response.Clear();
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync(JsonConvert.SerializeObject(responseAjaxResult));
                return;
            }
            if (context.User.Identity != null && !context.User.Identity.IsAuthenticated)
            {
                responseAjaxResult.Fail(message: ResponseMessage.OPERATION_Token_FAIL, httpStatusCode: HttpStatusCode.TokenError);
                context.Response.Clear();
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync(JsonConvert.SerializeObject(responseAjaxResult,new JsonSerializerSettings()
                {
                    //首字母小写问题
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }));
                return;
            }
            else
            {
                //通过
                await next(context);
            }
           
        }
    }
}
