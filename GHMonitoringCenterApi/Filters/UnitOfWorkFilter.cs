using GHMonitoringCenterApi.CustomAttribute;
using GHMonitoringCenterApi.SqlSugarCore;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GHMonitoringCenterApi.Filters
{
    public class UnitOfWorkFilter : IAsyncActionFilter, IOrderedFilter
    {
        //private readonly ILogger<UnitOfWorkFilter> _logger;
        //public UnitOfWorkFilter(ILogger<UnitOfWorkFilter> logger)
        //{
        //    this._logger = logger;
        //}
        /// <summary>
        /// 过滤器排序
        /// </summary>
        internal const int FilterOrder = 999;

        /// <summary>
        /// 排序属性
        /// </summary>
        public int Order => FilterOrder;

        /// <summary>
        /// 拦截请求
        /// </summary>
        /// <param name="context">动作方法上下文</param>
        /// <param name="next">中间件委托</param>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            var method = actionDescriptor.MethodInfo;

            var httpContext = context.HttpContext;

            // 如果没有定义工作单元过滤器，则跳过
            if (!method.IsDefined(typeof(UnitOfWorkAttribute), true))
            {
                _ = await next();

                return;
            }
            var _unitOfWork = httpContext.RequestServices.GetRequiredService<IUnitOfWork>();

            _unitOfWork.BeginTransaction(context);

            // 获取执行 Action 结果
            var resultContext = await next();

            if (resultContext == null || resultContext.Exception == null)
            {
                // 调用提交事务方法
                _unitOfWork.CommitTransaction(resultContext);
            }
            else
            {
                _unitOfWork.RollbackTransaction(resultContext);
            }

            _unitOfWork.OnCompleted(context, resultContext);

        }
    }
}
