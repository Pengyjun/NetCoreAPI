using GHMonitoringCenterApi.Application.Contracts.IService.User;
using GHMonitoringCenterApi.Domain.Shared;
using GHMonitoringCenterApi.Domain.Shared.Const;
using GHMonitoringCenterApi.Domain.Shared.Enums;
using GHMonitoringCenterApi.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SqlSugar;

namespace GHMonitoringCenterApi.Filters
{

    /// <summary>
    ///此过滤器来拦截用户是否有操作权限
    /// </summary>
    public class UserOperationAuthorityFilter : IAsyncActionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> logger;
        public UserOperationAuthorityFilter(ILogger<GlobalExceptionFilter> logger)
        {
            this.logger = logger;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            ResponseAjaxResult<bool> responseAjaxResult = new Domain.Shared.ResponseAjaxResult<bool>();
            try
            {
                var isAuth = context.IsAuth();
                if(!isAuth)
                {
                    await next();
                    return;
                }
                var isAllow = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
                if (isAllow)
                {
                    await next();
                    return;
                }
                var userService = context.HttpContext.RequestServices.GetService<IUserService>();
                var token = context.HttpContext.Request.Headers["Authorization"].ToString();
                if (!string.IsNullOrWhiteSpace(token))
                {
                    token = token.Replace("Bearer", "").Trim();
                    if (userService != null)
                    {
                      var userInfo=userService.GetUserInfoAsync(token);
                        //获取当前角色的操作权限
                        if (userInfo.CurrentLoginOperationType == 1 && context.HttpContext.Request.Method == "GET")
                        {
                            await next();
                        }
                        else if (userInfo.CurrentLoginOperationType == 0)
                        {
                            await next();
                        }
                        else {
                            responseAjaxResult.Fail(message: ResponseMessage.DATA_NOAUTHORITYOPERATE_FAIL, httpStatusCode: HttpStatusCode.NoAuthorityOperateFail);
                            context.HttpContext.Response.Clear();
                            context.HttpContext.Response.ContentType= "application/json;charset=utf-8";
                            context.HttpContext.Response.StatusCode =(int) HttpStatusCode.Success;
                            await context.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(responseAjaxResult, new JsonSerializerSettings()
                            {
                                //首字母小写问题
                                ContractResolver = new CamelCasePropertyNamesContractResolver()
                            }));
                            return;
                        }
                       
                    }
                }

            }
            catch (Exception ex)
            {
                logger.LogError($"判断用户是否有操作权限出现错误:{ex}");
            }
        }
    }
}
