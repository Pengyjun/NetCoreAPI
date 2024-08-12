using GHMonitoringCenterApi.Application.Contracts.Dto.Project.MonthReportForProject;

namespace GHMonitoringCenterApi.Application.Contracts.IService.Project
{
    /// <summary>
    /// 项目月报重写接口
    /// </summary>
    public interface IMonthReportForProjectService
    {
        Task<List<ConveretTreeForProjectWBSDto>> WBSConvertTree(Guid projectId, int dateMonth);
    }
}
