using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.HelpCenter
{
    /// <summary>
    /// 获取帮助中心菜单请求Dto
    /// </summary>
    public class SearchHelpCenterDetailsResponseDto
    {
        /// <summary>
        /// 帮助中心详情
        /// </summary>
        public string? Details { get; set; }
        /// <summary>
        /// 是否重用 1是 0是否
        /// </summary>
        public bool Reutilizando { get; set; }
    }
}
