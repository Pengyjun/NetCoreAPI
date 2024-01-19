using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project
{
    /// <summary>
    /// 完工项目信息
    /// </summary>
    public class ClosingProjectResponseDto
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        public string? ProjectName { get; set; }
        /// <summary>
        /// 完工日期
        /// </summary>
        public DateTime? ClosingDateTime { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string? CompanyName { get; set; }
    }
}
