using GHMonitoringCenterApi.Application.Contracts.Dto.ProjectAdjustMonthReport;
using GHMonitoringCenterApi.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.IService.ProjectAdjustMonthReport
{
    /// <summary>
    /// 项目开累数调整服务层
    /// </summary>
    public interface IProjectAdjustMonthReportService
    {

        Task<ResponseAjaxResult<List<ProjectAdjustProductionValueResponseDto>>> SearchProjectAdjustMonthReportAsync(string projectId);
    }
}
