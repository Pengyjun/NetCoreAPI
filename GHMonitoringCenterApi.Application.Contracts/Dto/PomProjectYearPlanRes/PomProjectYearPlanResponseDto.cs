using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto
{
    public class PomProjectYearPlanResponseDto
    {
        /// <summary>
        /// 项目Id
        /// </summary>
        public string ProjectId { get; set; }
        /// <summary>
        /// 填报时间
        /// </summary>
        public DateTime DateTime { get; set; }
    }
}
