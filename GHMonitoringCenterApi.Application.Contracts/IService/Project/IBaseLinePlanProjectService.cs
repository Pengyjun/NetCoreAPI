using GHMonitoringCenterApi.Application.Contracts.Dto;
using GHMonitoringCenterApi.Application.Contracts.Dto.EquipmentManagement;
using GHMonitoringCenterApi.Application.Contracts.Dto.Job;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project.ExcelImport;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report;
using GHMonitoringCenterApi.Application.Contracts.Dto.ProjectPlanProduction;
using GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Domain.Shared;

namespace GHMonitoringCenterApi.Application.Contracts.IService.Project
{


    /// <summary>
    /// 基准计划业务接口层
    /// </summary>
    public interface IBaseLinePlanProjectService
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<SearchBaseLinePlanProjectAnnualProduction>> SearchBaseLinePlanProjectAnnualProductionDetailsAsync(SearchBaseLinePlanProjectAnnualProductionRequest requestBody);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<SearchBaseLinePlanProjectAnnualProductionDto>>> SearchBaseLinePlanProjectAnnualProductionAsync(SearchBaseLinePlanProjectAnnualProductionRequest requestBody);

        /// <summary>
        /// 保存基准计划
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> SaveBaseLinePlanProjectAnnualProductionAsync(SaveBaseLinePlanProjectDto? requestBody);

        /// <summary>
        /// 提交基准计划
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> SubmitBaseLinePlanProjectAnnualProductionAsync(SubmitBaseLinePlanProjectDto? requestBody);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<BaseLinePlanAnnualProduction>> BaseLinePlanAnnualProductionAsync();


        /// <summary>
        /// 分子公司
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<SearchSubsidiaryCompaniesProjectProductionDto>>> SearchSubsidiaryCompaniesProjectProductionAsync(SearchBaseLinePlanProjectAnnualProductionRequest requestBody);


        /// <summary>
        /// 分子公司合计 暂时废弃
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<SearchBaseLinePlanProjectAnnualProduction>> SearchBaseLinePlanProjectAnnualProductionSumAsync(SearchBaseLinePlanProjectAnnualProductionRequest requestBody);


        /// <summary>
        /// 局工程部 项目界面
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<SearchSubsidiaryCompaniesProjectProductionDto>>> SearchBureauProjectProductionAsync(SearchBaseLinePlanProjectAnnualProductionRequest requestBody);



        /// <summary>
        /// 局工程部项目合计
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<SearchBaseLinePlanProjectAnnualProduction>> SearchBureauProjectProductionAsyncSumAsync(SearchBaseLinePlanProjectAnnualProductionRequest requestBody);


        /// <summary>
        /// 局工程部  公司界面
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<SearchCompaniesProjectProductionDto>>> SearchCompaniesProjectProductionAsync(SearchBaseLinePlanProjectAnnualProductionRequest requestBody);



        /// <summary>
        /// 计划基准
        /// </summary>
        /// <param name="requsetDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<BaseLinePlanAncomparisonResponseDto>>> SearchBaseLinePlanAncomparisonAsync(BaseLinePlanAncomparisonRequsetDto requsetDto);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<BaseLinePlanprojectComparisonRequestDto>> SearchBaseLinePlanComparisonAsync(SearchBaseLinePlanprojectComparisonRequestDtoRequest requestBody);


        /// <summary>
        /// 项目基准计划导入
        /// </summary>
        /// <param name="imports"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<BaseLineImportOutput>> BaseLinePlanProjectAnnualProductionImport(List<BaseLinePlanProjectAnnualProductionImport> imports, BaseLinePlanprojectImportDto import);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="isApprove"></param>
        /// <param name="id"></param>
        /// <returns></returns>

        //Task<ResponseAjaxResult<bool>> BaseLinePlanProjectApproveAsync(BaseLinePlanProject input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> BaseLinePlanProjectApprove(SearchSubsidiaryCompaniesProjectProductionDto input);

        /// <summary>
        /// 基准计划新
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<SearchSubsidiaryCompaniesProjectProductionDto>>> SearchBaseLinePlanAncomparisonNewAsync(BaseLinePlanAncomparisonRequsetDto requestBody);


        /// <summary>
        /// 计划基准导出
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<BaseLinePlanExcelOutputDto>>> BaseLinePlanExcelOutPutAsync(BaseLinePlanAncomparisonRequsetDto requestBody);

        /// <summary>
        /// 基准计划下拉框
        /// </summary>
        /// <param name="requsetDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<BaseLinePlanSelectOptiong>>> SearchBaseLinePlanOptionsAsync(BaseLinePlanAncomparisonRequsetDto requsetDto);

        /// <summary>
        /// 获取用户审批人列表
        /// </summary>
        /// <param name="requsetDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<ApproveUsersResponseDto>>> SearchBaseLinePlanApproveUsersAsync(ApproveUsersRequsetDto requsetDto);


        /// <summary>
        /// 获取项目列表
        /// </summary>
        /// <param name="requsetDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<BaseLinePlanProjectResponseDto>>> SearchBaseLinePlanProjectAsync(SearchBaseLinePlanProjectRequsetDto input);

        /// <summary>
        /// 编辑是否基准计划填报期
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> ToggleBaseLinePlanPeriodAsync(string input);

        /// <summary>
        /// 获取是否基准计划填报期
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<string>> GetBaseLinePlanPeriodStatusAsync();

    }
}
