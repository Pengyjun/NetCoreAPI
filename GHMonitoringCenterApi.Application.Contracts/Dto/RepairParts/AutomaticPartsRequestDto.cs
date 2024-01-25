using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.RepairParts
{
    /// <summary>
    /// 自动统计请求
    /// </summary>
    public class AutomaticPartsRequestDto
    {
        /// <summary>
        /// 项目编号（入账年份）
        /// </summary>
        public string? ProjectCode { get; set; }
    }
}
