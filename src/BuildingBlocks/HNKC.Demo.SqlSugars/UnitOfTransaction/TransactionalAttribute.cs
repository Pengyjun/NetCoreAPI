using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

<<<<<<<< HEAD:src/BuildingBlocks/HNKC.CrewManagePlatform.SqlSugars/UnitOfTransaction/TransactionalAttribute.cs
namespace HNKC.CrewManagePlatform.SqlSugars.UnitOfTransaction
========
namespace HNKC.Demo.SqlSugars.UnitOfTransaction
>>>>>>>> 7fd224848dc4910963de00d8c3a15a3418dc1847:src/BuildingBlocks/HNKC.Demo.SqlSugars/UnitOfTransaction/TransactionalAttribute.cs
{
    /// <summary>
    /// 事务标签
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class TransactionalAttribute : Attribute
    {
    }
}
