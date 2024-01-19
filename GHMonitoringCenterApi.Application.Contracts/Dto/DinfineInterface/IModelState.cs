using GHMonitoringCenterApi.Application.Contracts.Dto.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto
{
    /// <summary>
    /// Model状态接口定义
    /// <para>适应业务存在监听数据状态的功能</para>
    /// </summary>
    public interface  IModelState
    {
        /// <summary>
        /// 业务数据状态（0:无状态，1：新增，2：修改，3：删除）
        /// </summary>
         ModelState State { get; set; }
    }
}
