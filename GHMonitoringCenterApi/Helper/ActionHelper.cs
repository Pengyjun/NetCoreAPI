using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;

namespace GHMonitoringCenterApi.Helper
{
    /// <summary>
    /// Action帮助类
    /// </summary>
    public static  class ActionHelper
    {
        /// <summary>
        /// 获取函数
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static MethodInfo GetMethodInfo(this FilterContext context)
        {
            return ((ControllerActionDescriptor)context.ActionDescriptor).MethodInfo;
        }

        /// <summary>
        /// 是否授权
        /// </summary>
        /// <returns></returns>
        public static bool IsAuth(this ActionExecutingContext context)
        {
           return  context.Controller.GetType().GetCustomAttributes<AuthorizeAttribute>(true).Any()
                || GetMethodInfo(context).GetCustomAttributes<AuthorizeAttribute>(true).Any();
        }
    }
}

