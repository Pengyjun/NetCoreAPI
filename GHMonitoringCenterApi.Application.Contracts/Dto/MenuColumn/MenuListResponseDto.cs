using GHMonitoringCenterApi.Domain.Shared.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.MenuColumn
{
    /// <summary>
    /// 菜单列相应Dto
    /// </summary>
    public class MenuListResponseDto: TreeNode<MenuListResponseDto>
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid? Value { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        public string? Label { get; set; }
        /// <summary>
        /// 菜单URL
        /// </summary>
        public string? Url { get; set; }
        /// <summary>
        /// ICON图标
        /// </summary>
        public string? Icon { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int? Sort { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? Createtime { get; set; }
        /// <summary>
        /// 更新ID
        /// </summary>
        public Guid? Updateid { get; set; }
        /// <summary>
        /// 删除
        /// </summary>
        public int? Isdelete { get; set; }

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
