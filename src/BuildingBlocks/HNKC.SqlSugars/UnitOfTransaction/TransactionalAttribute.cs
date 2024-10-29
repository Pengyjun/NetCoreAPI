using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.SqlSugars.UnitOfTransaction
{
    /// <summary>
    /// 事务标签
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class TransactionalAttribute : Attribute
    {
    }
}
