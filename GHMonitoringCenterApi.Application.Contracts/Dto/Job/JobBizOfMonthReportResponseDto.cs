using GHMonitoringCenterApi.Application.Contracts.Dto.Project;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Job
{
    /// <summary>
    /// 任务-项目月报业务返回结果
    /// </summary>
    public  class JobBizOfMonthReportResponseDto : JobBizResponseDto<ProjectMonthReportResponseDto>
    {

    }
}
