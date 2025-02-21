using GHMonitoringCenterApi.Application.Contracts.Dto.ConstructionProjectDaily;
using GHMonitoringCenterApi.Application.Contracts.Dto.ProjectProductionReport;
using GHMonitoringCenterApi.Application.Contracts.IService.ProjectProductionReport;
using GHMonitoringCenterApi.CustomAttribute;
using GHMonitoringCenterApi.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GHMonitoringCenterApi.Controllers.ProjectProductionReport
{
    /// <summary>
    /// 在建项目生产相关数据
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectProductionReportController : BaseController
    {

        #region 依赖注入
        public IProjectProductionReportService projectProductionReportService { get; set; }
        public ProjectProductionReportController(IProjectProductionReportService projectProductionReportService)
        {
            this.projectProductionReportService = projectProductionReportService;
        }
        #endregion

        /// <summary>
        /// 分页查询产值日报列表
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpGet("SearchDayReports")]
        public async Task<ResponseAjaxResult<DayReportResponseDto>> SearchDayReportAsync([FromQuery] ProductionSafetyRequestDto searchRequestDto)
        {
            return await projectProductionReportService.SearchDayReportAsync(searchRequestDto);
        }

        /// <summary>
        /// 分页查询船舶日报列表
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpGet("SearchShipDayReport")]
        public async Task<ResponseAjaxResult<ShipsDayReportResponseDto>> SearchShipDayReportAsync([FromQuery] ShipDailyRequestDto searchRequestDto)
        {
            return await projectProductionReportService.SearchShipDayReportAsync(searchRequestDto);
        }

        /// <summary>
        /// 分页查询安监日报列表
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpGet("SearchSafeDayReport")]
        public async Task<ResponseAjaxResult<SafeDayReportResponseDto>> SearchSafeDayReportAsync([FromQuery] ProductionSafetyRequestDto searchRequestDto)
        {
            return await projectProductionReportService.SearchSafeDayReportAsync(searchRequestDto);
        }


        /// <summary>
        /// 日报产值偏差列表
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpGet("SearchDayReportDeviationList")]
        public async Task<ResponseAjaxResult<List<DayReportDeviationListResponseDto>>> SearchDayReportDeviationListAsync([FromQuery] DayReportDeviationRequestDto requestDto)
        {
            return await projectProductionReportService.SearchDayReportDeviationListAsync(requestDto);
        }

        /// <summary>
        /// 日报偏差值变更
        /// </summary>
        /// <returns></returns>
        [HttpPost("UpdateDayReportDeviation")]
        [UnitOfWork]
        public async Task<ResponseAjaxResult<bool>> UpdateDayReportDeviationAsync([FromBody] UpdateDayDeviationRequestDto requestDto)
        {
            return await projectProductionReportService.UpdateDayReportDeviationAsync(requestDto);
        }

    }
}
