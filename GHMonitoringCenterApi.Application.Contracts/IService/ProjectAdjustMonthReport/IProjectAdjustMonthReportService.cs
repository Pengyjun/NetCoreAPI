using GHMonitoringCenterApi.Application.Contracts.Dto.ProjectAdjustMonthReport;
using GHMonitoringCenterApi.Domain.Shared;
using GHMonitoringCenterApi.Domain.Shared.Util;
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

        Task<ResponseAjaxResult<ProjectAdjustResponseDto>> SearchProjectAdjustMonthReportAsync(string projectId);


        /// <summary>
        /// 保存开累数调整数值
        /// </summary>
        /// <param name="projectAdjustResponseDto"></param>
        /// <returns></returns>

        Task<ResponseAjaxResult<bool>> SearchProjectAdjustMonthReportAsync(ProjectAdjustResponseDto projectAdjustResponseDto);
    }
}
