using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace CH.Simple.SqlSugar
{
    /// <summary>
    /// 事务过滤器
    /// </summary>
    public class TransactionalFilter : IAsyncActionFilter, IOrderedFilter
    {
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
            if (!method.IsDefined(typeof(TransactionalAttribute), true))
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
