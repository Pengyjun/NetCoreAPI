using GHMonitoringCenterApi.Application.Contracts.Dto;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project.ExcelImport;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report;
using GHMonitoringCenterApi.Application.Contracts.Dto.ProjectPlanProduction;
using GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Domain.Shared;

namespace GHMonitoringCenterApi.Application.Contracts.IService.Project
{


    /// <summary>
    /// 项目业务接口层
    /// </summary>
    public interface IProjectService
    {

        #region 分页获取项目列表

        /// <summary>
        /// 分页获取项目列表
        /// </summary>
        /// <param name="searchRequestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<ProjectResponseDto>>> SearchProjectAsync(ProjectSearchRequestDto searchRequestDto);
        /// <summary>
        /// 获取项目列表导出缺失字段数据
        /// </summary>
        /// <param name="searchRequestDto"></param>
        /// <returns></returns>
        Task<List<ProjectResponseDto>> GetAllSearchProjectAsync(ProjectSearchRequestDto searchRequestDto);

        /// <summary>
        /// 获取项目列表导出数据
        /// </summary>
        /// <param name="searchRequestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<ProjectExcelSearchsResponseDto>> GetProjectExcelAsync(ProjectSearchRequestDto searchRequestDto);
        #endregion

        #region 获取项目详情
        /// <summary>
        /// 获取项目详情
        /// </summary>
        /// <param name="basePrimaryRequestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<ProjectDetailResponseDto>> SearchProjectDetailAsync(BasePrimaryRequestDto basePrimaryRequestDto);
        #endregion

        #region 新增或修改项目
        /// <summary>
        /// 新增或修改项目
        /// </summary>
        /// <param name="addOrUpdateProjectRequestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> AddORUpdateProjectDetailAsync(AddOrUpdateProjectRequestDto addOrUpdateProjectRequestDto, LogInfo logInfo);
        #endregion

        #region 删除项目
        /// <summary>
        /// 删除项目
        /// </summary>
        /// <param name="basePrimaryRequestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> RemoveProjectAsync(BasePrimaryRequestDto basePrimaryRequestDto);
        #endregion

        #region 查询ProjectWBS树
        /// <summary>
        /// 查询ProjectWBS树
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<ProjectWBSResponseDto>>> SearchProjectWBSTree(Guid ProjectId);
        #endregion

        #region 新增或修改ProjectWBS树
        /// <summary>
        /// 新增或修改ProjectWBS树
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> SaveProjectWBSTreeAsync(AddOrUpdateProjectWBSRequsetDto addOrUpdateProjectWBSRequestDto, LogInfo logInfo);
        #endregion
        #region 删除ProjectWBS树
        /// <summary>
        /// 删除ProjectWBS树
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> DeleteProjectWBSTreeAsync(Guid Id);
        /// <summary>
        /// 删除ProjectWBS树 校验是否写过项目月报
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> DeleteProjectWBSTreeValidatableAsync(DeleteProjectWBSValidatableDto requestDto);
        #endregion
        #region 根据经纬度获取地点区域
        /// <summary>
        /// 根据经纬度获取地点区域
        /// </summary>
        /// <param name="lon">经度</param>
        /// <param name="lat">纬度</param>
        /// <returns></returns>
        Task<ResponseAjaxResult<ProjectAreaRegionResponseDto>> SearchProjectAreaRegion(ProjectLonLatRequestDto requestDto);
        #endregion

        /// <summary>
        /// 保存项目-项目结构树
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> SaveProjectWBSTreeBizAsync(SaveProjectWBSRequestDto model);

        /// <summary>
        /// 查询项目-项目结构树
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<ProjectWBSComposeResponseDto>> SearchProjectWBSTreeBizAsync(Guid projectId);
        /// <summary>
        /// 获取登陆人未填相关日报弹框信息
        /// </summary>
        /// <param name="currentUser">当前登陆人</param>
        /// <returns></returns>
        Task<ResponseAjaxResult<BasePullDownResponseDto>> GetNotFillBulletBoxAsync(CurrentUser currentUser);

        /// <summary>
        /// 获取币种汇率
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<ProjectCurrencyResponseDto>> GetCurrentcyRate(Guid currencyId);
        /// <summary>
        /// 币种切换判断
        /// </summary>
        /// <param name="IsConvert"></param>
        /// <returns></returns>
        decimal GetAmount(bool IsConvert, Guid? currencyId, decimal? amount, decimal? exchangeRate);
        /// <summary>
        /// 项目与报表负责人列表
        /// </summary>
        /// <param name="searchRequestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<ProjectResponseDto>>> ProjectAboutStatementAsync(ProjectAboutStatmentRequestDto searchRequestDto);
        /// <summary>
        /// 修改项目与报表负责人关系
        /// </summary>
        /// <param name="searchRequestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> UpdateProjectAboutStatementAsync(RetortformerRequestDto searchRequestDto);

        /// <summary>
        /// 分包船舶列表
        /// </summary>
        /// <param name="sub"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<SubShipUserResponseDto>>> SearchSubShipAndUserAsync(SubShipUserRequestDto sub);

        /// <summary>
        /// 新增或修改分包船舶信息
        /// </summary>
        /// <param name="subShip"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> SaveSubShipUserAsync(AddSubShipUserRequestDto subShip);

        /// <summary>
        /// 删除分包船舶信息
        /// </summary>
        /// <param name="sub"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> RemoveSubShipUserAsync(SubShipRequestDto sub);

        /// <summary>
        /// 保存项目产值计划
        /// </summary>
        /// <param name="projectPlanRequestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> SaveProjectPlanAsync(SaveProjectPlanRequestDto projectPlanRequestDto);

        /// <summary>
        /// 保存项目产值计划excel导入的数据
        /// </summary>
        /// <param name="projectPlanExcelImportResponseDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> SaveProjectPlanExcelImportAsync(List<ProjectPlanInfo> projectPlanExcelImportResponseDto);


        /// <summary>
        /// 查询项目计划列表
        /// </summary>
        /// <param name="projectPlanRequestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<ProjectPlanResponseDto>> SearchProjectPlanAsync(MonthlyPlanRequestDto monthlyPlanRequestDto);

        /// <summary>
        /// 查询项目计划详情
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<ProjectPlanDetailResponseDto>> SearchProjectPlanDetailAsync(ProjectPlanSearchRequestDto requestDto);


        /// <summary>
        /// 查询公司在手项目列表
        /// </summary>
        /// <param name="companyProjectDetailsdRequestDto"></param>
        /// <returns></returns>

        Task<ResponseAjaxResult<List<CompanyProjectDetailedResponseDto>>> SearchCompanyProjectListAsync(CompanyProjectDetailsdRequestDto companyProjectDetailsdRequestDto);

        /// <summary>
        /// 公司在手项目下拉框选择
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> SearchCompanyProjectPullDownAsync();

        /// <summary>
        /// 获取开停工记录时间
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<StartWorkResponseDto>>> SearchStartListAsync(Guid projectid, int pageIndex, int pageSize);



        /// <summary>
        /// 获取开停工记录时间
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<ShipMovementRecordResponseDto>>> SearchShipMovementAsync(Guid shipId, int pageIndex, int pageSize);

        /// <summary>
        /// 撤回本月项目月报  
        /// </summary>
        /// <param name="id">项目月报</param>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> RevocationProjectMonthAsync(Guid id);
        /// <summary>
        /// 历史产值月报列表
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<HistoryProjectMonthReportResponseDto>>> SearchHistoryProjectMonthRepAsync(HistoryProjectMonthReportRequestDto requestBody);
        /// <summary>
        /// 保存历史产值月报
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> SaveHistoryProjectMonthReportAsync(HistoryProjectMonthReportRequestParam requestBody);
        /// <summary>
        /// 月报编辑按钮权限控制 列表
        /// </summary>
        /// <returns></returns>
        bool BtnEditMonthlyReport(int type, int date, List<BtnEditMonthlyReportPermission> permissions);
        Task<ResponseAjaxResult<List<BtnEditMonthlyReportSearch>>> BtnEditMonthlyReportSearchAsync(BaseRequestDto requestBody);
        Task<ResponseAjaxResult<bool>> SaveBtnEditReportAsync(SaveBtnEditMonthlyReport requestBody);
        bool aa();
        bool GetHolidays();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<SearchProjectAnnualProduction>> SearchProjectAnnualProductionAsync(SearchProjectAnnualProductionRequest requestBody);
    }
}
