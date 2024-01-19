using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.HelpCenter
{
    /// <summary>
    /// 获取用户手册预览及下载网址响应Dto
    /// </summary>
    public class SearchWebsiteResponseDto
    {
        /// <summary>
        /// 用户手册
        /// </summary>
        public string? UserManual { get; set; }
    }
}
