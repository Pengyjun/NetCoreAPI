using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Menu
{
    /// <summary>
    /// 添加首页菜单用户权限
    /// </summary>
    public class AddHomeUserRequestDto
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// 类型 0:临时性  1:永久性
        /// </summary>
        public int Type { get; set; }
    }
}
