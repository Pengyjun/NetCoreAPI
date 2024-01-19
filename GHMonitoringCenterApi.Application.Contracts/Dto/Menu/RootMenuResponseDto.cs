using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Menu
{


    /// <summary>
    /// 获取所有根菜单响应DTO
    /// </summary>
    public class RootMenuResponseDto
    {
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 根菜单ID
        /// </summary>
        public int Mid { get; set; }
        /// <summary>
        /// 父菜单ID
        /// </summary>
        public int ParentId { get; set; }
    }
}
