using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto
{
    /// <summary>
    /// 操作人
    /// </summary>
    public interface IOperator
    {
        /// <summary>
        /// 操作人Id
        /// </summary>
        Guid? OperatorId { get; }

        /// <summary>
        /// 操作人名称
        /// </summary>
        string? OperatorName { get;}

        /// <summary>
        /// 设置操作人
        /// </summary>
        void SetOperator(Guid? operatorId, string? operatorName);
 
    }
}
