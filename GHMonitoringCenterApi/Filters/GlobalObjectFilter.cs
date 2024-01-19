using GHMonitoringCenterApi.Application.Contracts.IService.User;
using GHMonitoringCenterApi.Application.Service.User;
using GHMonitoringCenterApi.Controllers;
using GHMonitoringCenterApi.Domain.Shared;
using GHMonitoringCenterApi.Domain.Shared.Const;
using GHMonitoringCenterApi.Domain.Shared.Enums;
using GHMonitoringCenterApi.Domain.Shared.Util;
using GHMonitoringCenterApi.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Reflection;
using UtilsSharp;

namespace GHMonitoringCenterApi.Filters
{
    /// <summary>
    /// 全局对象过滤器
    /// </summary>
    public class GlobalObjectFilter : ActionFilterAttribute
    {
        private readonly GlobalObject _globalObject;

        /// <summary>
        /// 全局对象-注入全局对象
        /// </summary>
        /// <param name="globalObject">全局对象</param>
        public GlobalObjectFilter(GlobalObject globalObject)
        {
            _globalObject = globalObject;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var controller = context.Controller as BaseController;
            _globalObject.CurrentUser = GetCurrentUser(context);
            var isAuth = context.IsAuth();
            if (isAuth)
            {

                var isAllow = (((ControllerActionDescriptor)context.ActionDescriptor).MethodInfo).GetCustomAttributes<AllowAnonymousAttribute>(true).Any();
                if (isAllow)
                {
                    await next();
                }

                if (controller != null && !isAllow)
                {
                    if (_globalObject.CurrentUser != null && _globalObject.CurrentUser.Id != Guid.Empty)
                    {
                        controller.CurrentUser = _globalObject.CurrentUser;
                        await next();
                    }
                    else
                    {
                        context.HttpContext.Response.Clear();
                        context.HttpContext.Response.StatusCode = 401;
                    }
                }
            }
            else
            {
                await next();
            }
        }
        /// <summary>
        /// 函数执行前
        /// </summary>
        //public override void OnActionExecuting(ActionExecutingContext context)
        //{
        //    var controller = context.Controller as BaseController;
        //    _globalObject.CurrentUser = GetCurrentUser(context);
        //    if(controller!=null)
        //    {
        //        controller.CurrentUser = _globalObject.CurrentUser;
        //    }
        //    base.OnActionExecuting(context);
        //}

        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <returns></returns>
        private CurrentUser GetCurrentUser(ActionExecutingContext context)
        {
            var isAuth = context.IsAuth();
            if (!isAuth)
            {
                return new CurrentUser();
            }
            var request = context.HttpContext.Request;
            var head = request.Headers["Authorization"].ToString();
            if (!string.IsNullOrWhiteSpace(head))
            {
                head = head.Replace("Bearer", "").Trim();
                var userService = request.HttpContext.RequestServices.GetService<IUserService>();
                if (userService != null)
                {
                    return userService.GetUserInfoAsync(head);
                }
            }

            return new CurrentUser();
        }
    }
}
