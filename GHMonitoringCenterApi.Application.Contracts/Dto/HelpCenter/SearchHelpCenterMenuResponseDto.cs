using GHMonitoringCenterApi.Domain.Shared.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.HelpCenter
{
    /// <summary>
    /// 获取帮助中心菜单返回Dto
    /// </summary>
    public class SearchHelpCenterMenuResponseDto: TreeNodeParentId<SearchHelpCenterMenuResponseDto>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 详情
        /// </summary>
        public string Details { get; set; }
        /// <summary>
        /// 是否重用 true 是  false 否
        /// </summary>
        public bool Reutilizando { get; set; }
    }
}
