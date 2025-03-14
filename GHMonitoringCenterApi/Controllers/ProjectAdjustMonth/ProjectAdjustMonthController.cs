using GHMonitoringCenterApi.Application.Contracts.Dto.ProjectMasterData;
using GHMonitoringCenterApi.Application.Contracts.Dto;
using GHMonitoringCenterApi.Application.Contracts.IService.ProjectAdjustMonthReport;
using GHMonitoringCenterApi.Application.Service.ProjectMasterData;
using GHMonitoringCenterApi.Domain.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GHMonitoringCenterApi.Application.Contracts.Dto.ProjectAdjustMonthReport;
using Microsoft.AspNetCore.Authorization;

namespace GHMonitoringCenterApi.Controllers.ProjectAdjustMonth
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectAdjustMonthController : BaseController
    {
       
        #region 依赖注入
        public IProjectAdjustMonthReportService  projectAdjustMonthReportService { get; set; }
        public ProjectAdjustMonthController(IProjectAdjustMonthReportService projectAdjustMonthReportService)
        {
            this.projectAdjustMonthReportService = projectAdjustMonthReportService;
        }
        #endregion

        /// <summary>
        /// 开累数调整
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpGet("SearchProjectAdjustMonthReport")]
        public async Task<ResponseAjaxResult<ProjectAdjustResponseDto>> SearchProjectAdjustMonthReportAsync([FromQuery] string  projectId)
        {
            return await projectAdjustMonthReportService.SearchProjectAdjustMonthReportAsync(projectId);
        }
    }
}
