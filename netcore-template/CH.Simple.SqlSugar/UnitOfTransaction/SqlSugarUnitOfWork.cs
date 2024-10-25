using Microsoft.AspNetCore.Mvc.Filters;
using SqlSugar;

namespace CH.Simple.SqlSugar
{
    /// <summary>
    /// 工作单元实现
    /// </summary>
    public class SqlSugarUnitOfWork : IUnitOfWork
    {
        private readonly ISqlSugarClient _sqlSugarClient;

        public SqlSugarUnitOfWork(ISqlSugarClient sqlSugarClient)
        {
            _sqlSugarClient = sqlSugarClient;
        }
        public void BeginTransaction(ActionExecutingContext context)
        {
            _sqlSugarClient.AsTenant().BeginTran();
        }

        public void CommitTransaction(ActionExecutedContext resultContext)
        {
            _sqlSugarClient.AsTenant().CommitTran();
        }

        public void OnCompleted(ActionExecutingContext context, ActionExecutedContext resultContext)
        {
            _sqlSugarClient.Dispose();
        }

        public void RollbackTransaction(ActionExecutedContext resultContext)
        {
            _sqlSugarClient.AsTenant().RollbackTran();
        }
    }

    /// <summary>
    /// 工作单元接口
    /// </summary>

    public interface IUnitOfWork
    {
        /// <summary>
        /// 开启工作单元处理
        /// </summary>
        /// <param name="context"></param>
        /// <param name="unitOfWork"></param>
        void BeginTransaction(ActionExecutingContext context);

        /// <summary>
        /// 提交工作单元处理
        /// </summary>
        /// <param name="resultContext"></param>
        /// <param name="unitOfWork"></param>
        void CommitTransaction(ActionExecutedContext resultContext);

        /// <summary>
        /// 回滚工作单元处理
        /// </summary>
        /// <param name="resultContext"></param>
        /// <param name="unitOfWork"></param>
        void RollbackTransaction(ActionExecutedContext resultContext);

        /// <summary>
        /// 执行完毕（无论成功失败）
        /// </summary>
        /// <param name="context"></param>
        /// <param name="resultContext"></param>
        void OnCompleted(ActionExecutingContext context, ActionExecutedContext resultContext);

    }
}
