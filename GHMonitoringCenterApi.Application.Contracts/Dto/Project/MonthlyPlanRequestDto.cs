using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project
{
    /// <summary>
    /// 月度计划列表请求Dto
    /// </summary>
    public class MonthlyPlanRequestDto : BaseRequestDto
    {
        /// <summary>
        /// 公司Id
        /// </summary>
        public Guid? CompanyId { get; set; }
    }
}
