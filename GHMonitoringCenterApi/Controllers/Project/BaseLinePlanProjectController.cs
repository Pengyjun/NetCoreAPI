using GHMonitoringCenterApi.Application.Contracts.Dto;
using GHMonitoringCenterApi.Application.Contracts.Dto.Job;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project.Approver;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project.MonthReportForProject;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project.ShipMovements;
using GHMonitoringCenterApi.Application.Contracts.Dto.ProjectDepartMent;
using GHMonitoringCenterApi.Application.Contracts.Dto.ProjectPlanProduction;
using GHMonitoringCenterApi.Application.Contracts.Dto.ProjectProductionReport;
using GHMonitoringCenterApi.Application.Contracts.Dto.Ship;
using GHMonitoringCenterApi.Application.Contracts.Dto.Upload;
using GHMonitoringCenterApi.Application.Contracts.IService.Job;
using GHMonitoringCenterApi.Application.Contracts.IService.Project;
using GHMonitoringCenterApi.CustomAttribute;
using GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Domain.Shared;
using GHMonitoringCenterApi.Domain.Shared.Const;
using GHMonitoringCenterApi.Domain.Shared.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniExcelLibs;
using SqlSugar;
using UtilsSharp;

namespace GHMonitoringCenterApi.Controllers.Project
{
    /// <summary>
    /// 基准计划相关控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BaseLinePlanProjectController : BaseController
    {

        #region 依赖注入
        public ISqlSugarClient _db { get; set; }
        public IBaseLinePlanProjectService baseLinePlanProjectService { get; set; }

        public IProjectService projectService { get; set; }

        private readonly IJobService _jobService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="projectService"></param>
        /// <param name="baseLinePlanProjectService"></param>
        public BaseLinePlanProjectController(ISqlSugarClient db, IBaseLinePlanProjectService baseLinePlanProjectService, IProjectService projectService, IJobService jobService)
        {
            this.baseLinePlanProjectService = baseLinePlanProjectService;
            this.projectService = projectService;
            this._db = db;
            _jobService = jobService;
        }
        #endregion


        /// <summary>
        /// 查询公司在手项目清单下拉框选择
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpGet("SearchCompanyProjectPullDown")]
        [AllowAnonymous]
        public async Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchCompanyProjectPullDownAsync()
        {
            return await projectService.SearchCompanyProjectPullDownAsync();
        }


        #region 基准计划
        /// <summary>
        /// 基准计划列表详情
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        [HttpGet("SearchBaseLinePlanProjectAnnualProductionDetails")]
        public async Task<ResponseAjaxResult<SearchBaseLinePlanProjectAnnualProduction>> SearchBaseLinePlanProjectAnnualProductionDetailsAsync([FromQuery] SearchBaseLinePlanProjectAnnualProductionRequest requestBody)
        {
            return await baseLinePlanProjectService.SearchBaseLinePlanProjectAnnualProductionDetailsAsync(requestBody);
        }

        /// <summary>
        /// 基准列表
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        [HttpGet("SearchBaseLinePlanProjectAnnualProduction")]
        public async Task<ResponseAjaxResult<List<SearchBaseLinePlanProjectAnnualProductionDto>>> SearchBaseLinePlanProjectAnnualProductionAsync([FromQuery] SearchBaseLinePlanProjectAnnualProductionRequest requestBody)
        {
            return await baseLinePlanProjectService.SearchBaseLinePlanProjectAnnualProductionAsync(requestBody);
        }

        /// <summary>
        /// 计划基准
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        [HttpGet("SearchBaseLinePlanComparison")]
        public async Task<ResponseAjaxResult<BaseLinePlanprojectComparisonRequestDto>> SearchBaseLinePlanComparisonAsync([FromQuery] SearchBaseLinePlanprojectComparisonRequestDtoRequest requestBody)
        {
            return await baseLinePlanProjectService.SearchBaseLinePlanComparisonAsync(requestBody);
        }

        /// <summary>
        /// 保存基准信息
        /// </summary>
        /// <returns></returns>
        [HttpPost("SaveBaseLinePlanProjectAnnualProduction")]
        public async Task<ResponseAjaxResult<bool>> SaveBaseLinePlanProjectAnnualProductionAsync([FromBody] List<SearchBaseLinePlanProjectAnnualProductionDto>? requestBody)
        {
            return await baseLinePlanProjectService.SaveBaseLinePlanProjectAnnualProductionAsync(requestBody);
        }

        /// <summary>
        /// 项目 船舶
        /// </summary>
        /// <returns></returns>
        [HttpGet("BaseAnnualProduction")]
        public async Task<ResponseAjaxResult<BaseLinePlanAnnualProduction>> BaseLinePlanAnnualProductionAsync()
        {
            return await baseLinePlanProjectService.BaseLinePlanAnnualProductionAsync();
        }

        /// <summary>
        /// 基准列表 分子公司
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        [HttpGet("SearchSubsidiaryCompaniesProjectProduction")]
        public async Task<ResponseAjaxResult<List<SearchSubsidiaryCompaniesProjectProductionDto>>> SearchSubsidiaryCompaniesProjectProductionAsync([FromQuery] SearchBaseLinePlanProjectAnnualProductionRequest requestBody)
        {
            return await baseLinePlanProjectService.SearchSubsidiaryCompaniesProjectProductionAsync(requestBody);
        }

        /// <summary>
        /// 基准列表 分子公司项目合计
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        [HttpGet("SearchBaseLinePlanProjectAnnualProductionSum")]
        public async Task<ResponseAjaxResult<SearchBaseLinePlanProjectAnnualProduction>> SearchBaseLinePlanProjectAnnualProductionSumAsync([FromQuery] SearchBaseLinePlanProjectAnnualProductionRequest requestBody)
        {
            return await baseLinePlanProjectService.SearchBaseLinePlanProjectAnnualProductionSumAsync(requestBody);
        }



        /// <summary>
        /// 局工程部界面 项目界面
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        [HttpGet("SearchBureauProjectProduction")]
        public async Task<ResponseAjaxResult<List<SearchSubsidiaryCompaniesProjectProductionDto>>> SearchBureauProjectProductionAsync([FromQuery] SearchBaseLinePlanProjectAnnualProductionRequest requestBody)
        {
            return await baseLinePlanProjectService.SearchBureauProjectProductionAsync(requestBody);
        }

        /// <summary>
        /// 局工程部界面 项目界面 项目合计汇总
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        [HttpGet("SearchBureauProjectProductionAsyncSum")]
        public async Task<ResponseAjaxResult<SearchBaseLinePlanProjectAnnualProduction>> SearchBureauProjectProductionAsyncSumAsync([FromQuery] SearchBaseLinePlanProjectAnnualProductionRequest requestBody)
        {
            return await baseLinePlanProjectService.SearchBureauProjectProductionAsyncSumAsync(requestBody);
        }


        /// <summary>
        /// 局工程部界面 公司界面
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        [HttpGet("SearchCompaniesProjectProduction")]
        public async Task<ResponseAjaxResult<List<SearchCompaniesProjectProductionDto>>> SearchCompaniesProjectProductionAsync([FromQuery] SearchBaseLinePlanProjectAnnualProductionRequest requestBody)
        {
            return await baseLinePlanProjectService.SearchCompaniesProjectProductionAsync(requestBody);
        }

        #endregion

        /// <summary>
        /// 计划基准列表
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        [HttpGet("SearchBaseLinePlanAncomparison")]
        public async Task<ResponseAjaxResult<List<BaseLinePlanAncomparisonResponseDto>>> SearchBaseLinePlanAncomparisonAsync([FromQuery] BaseLinePlanAncomparisonRequsetDto requestBody)
        {
            return await baseLinePlanProjectService.SearchBaseLinePlanAncomparisonAsync(requestBody);
        }

        /// <summary>
        /// 计划基准列表新
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        [HttpGet("SearchBaseLinePlanAncomparisonNew")]
        public async Task<ResponseAjaxResult<List<SearchSubsidiaryCompaniesProjectProductionDto>>> SearchBaseLinePlanAncomparisonNewAsync([FromQuery] BaseLinePlanAncomparisonRequsetDto requestBody)
        {
            return await baseLinePlanProjectService.SearchBaseLinePlanAncomparisonNewAsync(requestBody);
        }

        ///// <summary>
        ///// 基准计划审核
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet("BaseLinePlanProjectApprove")]
        //[AllowAnonymous]
        //public async Task<ResponseAjaxResult<bool>> BaseLinePlanProjectApproveAsync([FromQuery] int isApprove, [FromQuery] string? id)
        //{
        //    return await baseLinePlanProjectService.BaseLinePlanProjectApproveAsync(isApprove, id);
        //}

        #region 项目基准计划
        /// <summary>
        /// 项目基准计划导入模板下载
        /// </summary>
        /// <returns></returns>
        [HttpGet("BasePlanLineDownloadTemplate")]
        public async Task<IActionResult> BasePlanLineProjectDownloadTemplateAsync()
        {
            var templatePath = $"Template/Excel/BasePlanLineProjectTemplate.xlsx";
            var importData = new
            {
                title = DateTime.Now.Year + "年项目基准计划",
                ProjectPlan = new List<ProjectPlanResponseDto>()
            };
            return await ExcelTemplateImportAsync(templatePath, importData, importData.title);
        }

        /// <summary>
        /// 项目基准计划导入
        /// </summary>
        /// <returns></returns>
        [HttpPost("BasePlanLineProjectImport")]
        public async Task<ResponseAjaxResult<BaseLineImportOutput>> BasePlanLineProjectImportAsync()
        {

            //[FromBody] 
            BaseLinePlanprojectImportDto input = new BaseLinePlanprojectImportDto();
            ResponseAjaxResult<BaseLineImportOutput> responseAjaxResult = new ResponseAjaxResult<BaseLineImportOutput>();
            var streamUpdateFile = await StreamUpdateFileAsync();
            var streamData = streamUpdateFile.Data;
            if (streamUpdateFile != null)
            {
                var savePath = AppsettingsHelper.GetValue("UpdateItem:SavePath");
                var path = savePath + streamData.Name;
                var rows = MiniExcel.Query<BaseLinePlanProjectAnnualProductionImport>(path, startCell: "A2").ToList();
                if (rows != null)
                {
                    var insert = await baseLinePlanProjectService.BaseLinePlanProjectAnnualProductionImport(rows, input);
                    responseAjaxResult.Data = insert.Data;
                    responseAjaxResult.Success(ResponseMessage.OPERATION_IMPORTEXCEL_SUCCESS);
                    return responseAjaxResult;
                }
            }
            else
            {
                responseAjaxResult.Fail(ResponseMessage.DEVICE_IMPORT_FAIL);
            }
            return responseAjaxResult;
        }
        #endregion


        /// <summary>
        /// 查询基准计划下拉框选择
        /// </summary>
        /// <param name="requsetDto"></param>
        /// <returns></returns>
        [HttpGet("SearchBaseLinePlanOptions")]
        [AllowAnonymous]
        public async Task<ResponseAjaxResult<List<BaseLinePlanSelectOptiong>>> SearchBaseLinePlanOptionsAsync([FromQuery] BaseLinePlanAncomparisonRequsetDto requsetDto)
        {
            return await baseLinePlanProjectService.SearchBaseLinePlanOptionsAsync(requsetDto);
        }

        /// <summary>
        /// 提交基准计划审批
        /// </summary>
        /// <returns></returns>
        [HttpPost("SubmitJobOfBaseLinePlan")]
        [UnitOfWork]
        public async Task<ResponseAjaxResult<bool>> SubmitJobOfBaseLinePlanAsync([FromBody] SubmitJobOfBaseLinePlanRequestDto model)
        {
            return await _jobService.SubmitJobAsync(model);
        }

    }
}
