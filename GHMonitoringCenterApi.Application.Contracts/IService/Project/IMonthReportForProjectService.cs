using GHMonitoringCenterApi.Application.Contracts.Dto.Project;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project.MonthReportForProject;
using GHMonitoringCenterApi.Domain.Shared;

namespace GHMonitoringCenterApi.Application.Contracts.IService.Project
{
    /// <summary>
    /// 项目月报重写接口
    /// </summary>
    public interface IMonthReportForProjectService
    {

        /// <summary>
        /// 月报产包构成列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<MonthReportForProjectResponseDto>> SearchMonthReportForProjectAsync(ProjectMonthReportRequestDto model);
        /// <summary>
        /// 修改月报开累数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> SaveMonthReportForProjectHistoryAsync(SaveMonthReportForProjectHistoryDto model);

    }
}
