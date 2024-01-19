using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto
{
    /// <summary>
    /// 重置model属性定义
    /// <para>适应业务存在置空的功能</para>
    /// </summary>
    public  interface  IResetModelProperty
    {
        /// <summary>
        ///  重置model属性
        /// </summary>
        void ResetModelProperty();
    }
}
