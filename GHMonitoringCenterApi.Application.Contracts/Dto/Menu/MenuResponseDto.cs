using GHMonitoringCenterApi.Domain.Shared.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Menu
{


    /// <summary>
    /// 菜单响应DTO
    /// </summary>
    public class MenuResponseDto: TreeNode<MenuResponseDto>
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 菜單Url
        /// </summary>
        public string?  Url { get; set; }

        /// <summary>
        /// 菜单Icon
        /// </summary>
        public string? Icon { get; set; }
        /// <summary>
        /// 排序字段
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsSelect { get; set; }

        /// <summary>
        /// 选中的子节点数组
        /// </summary>
        public List<int> SelectNodes { get; set; }

    }
}
