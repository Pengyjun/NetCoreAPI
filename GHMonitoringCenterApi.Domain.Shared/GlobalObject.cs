using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Shared
{
    /// <summary>
    /// 全局对象
    /// </summary>
    public  class GlobalObject
    {
        /// <summary>
        /// 当前登录用户
        /// </summary>
        public CurrentUser CurrentUser { get; set; } = new() { };
    }
}
