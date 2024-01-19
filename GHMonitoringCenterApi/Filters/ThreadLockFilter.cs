using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Concurrent;
using System.IO;
using System.Reflection;
using GHMonitoringCenterApi.Helper;
using Microsoft.AspNetCore.Mvc;
using GHMonitoringCenterApi.Domain.Shared;
using GHMonitoringCenterApi.Domain.Shared.Enums;

namespace GHMonitoringCenterApi.Filters
{

    /// <summary>
    /// 线程锁（单线程运行锁,不适应与分布式服务项目）
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ThreadLockFilter : ActionFilterAttribute
    {
        /// <summary>
        /// 函数锁集合
        /// </summary>
        private readonly static ConcurrentDictionary<MethodInfo, bool> _lockMethodInfos = new ConcurrentDictionary<MethodInfo, bool>();

        public ThreadLockFilter()
        {
            Order = 2;
        }

        /// <summary>
        /// 上锁
        /// </summary>
        public bool LockMethod(MethodInfo methodInfo)
        {
            if (_lockMethodInfos.GetOrAdd(methodInfo, (key) => { return false; }))
            {
                return false;
            }
            lock (_lockMethodInfos)
            {
                if (_lockMethodInfos[methodInfo])
                {
                    return false;
                }
                _lockMethodInfos[methodInfo] = true;
                return true;
            }
        }

        /// <summary>
        /// 拦截上锁
        /// </summary>
        /// <returns></returns>
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var methodInfo = context.GetMethodInfo();
            if (!LockMethod(methodInfo))
            {
                var path = context.HttpContext.Request.Path;
                context.Result = new ObjectResult(new ResponseAjaxResult<bool>().FailResult(HttpStatusCode.Occupied, $"接口[{path}]正在执行,请稍后再试"));
                return;
            }
            await next();
            UnLockMethod(methodInfo);
        }

        /// <summary>
        /// 解锁
        /// </summary>
        public void UnLockMethod(MethodInfo methodInfo)
        {
            _lockMethodInfos[methodInfo] = false;
        }
    }
}
