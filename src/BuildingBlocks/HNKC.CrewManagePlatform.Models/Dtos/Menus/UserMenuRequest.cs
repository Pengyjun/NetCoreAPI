using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.CrewManagePlatform.Models.Dtos.Menus
{

    /// <summary>
    /// 添加菜单请求DTO
    /// </summary>
    public class UserMenuRequest
    {
        /// <summary>
        /// 当前节点的父节点   如果是根节点  传0
        /// </summary>
        public int ParentId { get; set; } = 0;

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 接口地址
        /// </summary>
        public string? Url { get; set; }

        /// <summary>
        /// 组件地址
        /// </summary>
        public string? ComponentUrl { get; set; }
        /// <summary>
        /// 图标名称
        /// </summary>
        public string? Icon { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int? Sort { get; set; } = 0;

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
