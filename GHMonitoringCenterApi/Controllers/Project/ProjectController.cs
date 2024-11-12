using GHMonitoringCenterApi.Application.Contracts.Dto;
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

namespace GHMonitoringCenterApi.Controllers.Project
{
    /// <summary>
    /// 项目相关控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectController : BaseController
    {

        #region 依赖注入
        public ISqlSugarClient _db { get; set; }
        public IProjectService projectService { get; set; }

        public IProjectReportService projectReportService { get; set; }
        public IProjectDepartMentService projectDepartMentService { get; set; }

        public IProjectShipMovementsService projectShipMovementsService { get; set; }

        public IProjectApproverService projectApproverService { get; set; }
        public IMonthReportForProjectService _monthReportForProjectService { get; set; }


        public ProjectController(ISqlSugarClient db, IProjectService projectService, IProjectReportService projectReportService, IProjectDepartMentService projectDepartMentService, IProjectShipMovementsService projectShipMovementsService, IProjectApproverService projectApproverService, IMonthReportForProjectService monthReportForProjectService)
        {
            this.projectService = projectService;
            this.projectReportService = projectReportService;
            this.projectDepartMentService = projectDepartMentService;
            this.projectShipMovementsService = projectShipMovementsService;
            this.projectApproverService = projectApproverService;
            this._monthReportForProjectService = monthReportForProjectService;
            this._db = db;
        }
        #endregion

        /// <summary>
        /// 分页查询项目列表
        /// </summary>
        /// <param name="searchRequestDto"></param>
        /// <returns></returns>
        [HttpGet("SearchProject")]
        public async Task<ResponseAjaxResult<List<ProjectResponseDto>>> SearchProjectAsync([FromQuery] ProjectSearchRequestDto searchRequestDto)
        {
            return await projectService.SearchProjectAsync(searchRequestDto);
        }
        /// <summary>
        /// 获取项目详情
        /// </summary>
        /// <param name="basePrimaryRequestDto"></param>
        /// <returns></returns>
        [HttpGet("SearchProjectDetail")]
        public async Task<ResponseAjaxResult<ProjectDetailResponseDto>> SearchProjectDetailAsync([FromQuery] BasePrimaryRequestDto basePrimaryRequestDto)
        {
            return await projectService.SearchProjectDetailAsync(basePrimaryRequestDto);
        }
        /// <summary>
        /// 新增或修改项目
        /// </summary>
        /// <param name="addOrUpdateProjectRequestDto"></param>
        /// <returns></returns>
        [HttpPost("SaveProject")]
        [UnitOfWork]
        public async Task<ResponseAjaxResult<bool>> AddORUpdateProjectDetailAsync([FromBody] AddOrUpdateProjectRequestDto addOrUpdateProjectRequestDto)
        {
            #region 日志信息
            LogInfo logDto = new LogInfo()
            {
                Id = GuidUtil.Increment(),
                OperationId = CurrentUser.Id,
                OperationName = CurrentUser.Name,
                DataId = addOrUpdateProjectRequestDto.Id

            };
            if (addOrUpdateProjectRequestDto.RequestType == true)
            {
                logDto.BusinessModule = "/项目业务数据/项目信息清单/新增";
                logDto.BusinessRemark = "/项目业务数据/项目信息清单/新增";
            }
            else
            {
                logDto.BusinessModule = "/项目业务数据/项目信息清单/修改";
                logDto.BusinessRemark = "/项目业务数据/项目信息清单/修改";
            }

            #endregion
            return await projectService.AddORUpdateProjectDetailAsync(addOrUpdateProjectRequestDto, logDto);
        }

        /// <summary>
        /// 删除项目
        /// </summary>
        /// <param name="basePrimaryRequestDto"></param>
        /// <returns></returns>
        [HttpPost("RemoveProject")]
        public async Task<ResponseAjaxResult<bool>> RemoveProjectAsync([FromBody] BasePrimaryRequestDto basePrimaryRequestDto)
        {
            return await projectService.RemoveProjectAsync(basePrimaryRequestDto);
        }

        /// <summary>
        /// 获取上传文件返回的数据
        /// </summary>
        [HttpPost("GetStreamUpdateFile")]
        public async Task<ResponseAjaxResult<UploadResponseDto>> GetStreamUpdateFileAsync()
        {
            ResponseAjaxResult<UploadResponseDto> responseAjaxResult = new ResponseAjaxResult<UploadResponseDto>();
            responseAjaxResult = await StreamUpdateFileAsync("ProjectDayReportFileSize", "ProjectDayReportFileType");
            if (responseAjaxResult.Data == null)
            {
                return responseAjaxResult;
            }
            responseAjaxResult.Success(ResponseMessage.OPERATION_UPLOAD_SUCCESS);
            return responseAjaxResult;
        }


        /// <summary>
        ///  查询projectwbs树
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchProjectWBSTree")]
        public async Task<ResponseAjaxResult<List<ProjectWBSResponseDto>>> SearchProjectWBSTree([FromQuery] BasePrimaryRequestDto requestDto)
        {
            return await projectService.SearchProjectWBSTree(requestDto.Id);
        }

        /// <summary>
        /// 根据经纬度获取地点区域
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet("SearchAreaRegion")]
        public async Task<ResponseAjaxResult<ProjectAreaRegionResponseDto>> SearchProjectAreaRegion([FromQuery] ProjectLonLatRequestDto requestDto)
        {
            return await projectService.SearchProjectAreaRegion(requestDto);
        }

        /// <summary>
        /// 新增或修改ProjectWBS树
        /// </summary>
        /// <returns></returns>
        [HttpPost("SaveProjectWBSTree")]
        [UnitOfWork]
        public async Task<ResponseAjaxResult<bool>> SaveProjectWBSTreeAsync([FromBody] AddOrUpdateProjectWBSRequsetDto addOrUpdateProjectWBSRequestDto)
        {
            #region 日志信息
            LogInfo logDto = new LogInfo()
            {
                Id = GuidUtil.Increment(),
                OperationId = CurrentUser.Id,
                OperationName = CurrentUser.Name,
                DataId = addOrUpdateProjectWBSRequestDto.ProjectId.ToGuid()
            };
            if (addOrUpdateProjectWBSRequestDto.RequestType == true)
            {
                logDto.BusinessModule = "/系统管理/项目结构设置/新增wbs树";
                logDto.BusinessRemark = "/系统管理/项目结构设置/新增wbs树";
            }
            else
            {
                logDto.BusinessModule = "/系统管理/项目结构设置/修改wbs树";
                logDto.BusinessRemark = "/系统管理/项目结构设置/修改wbs树";
            }
            #endregion
            return await projectService.SaveProjectWBSTreeAsync(addOrUpdateProjectWBSRequestDto, logDto);
        }
        /// <summary>
        /// 删除ProjectWBS树  校验是否曾经填写过
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("DeleteProjectWBSTreeValidatable")]
        [UnitOfWork]
        public async Task<ResponseAjaxResult<bool>> DeleteProjectWBSTreeValidatableAsync([FromBody] DeleteProjectWBSValidatableDto requestDto)
        {
            return await projectService.DeleteProjectWBSTreeValidatableAsync(requestDto);
        }
        /// <summary>
        /// 删除ProjectWBS树
        /// </summary>
        /// <returns></returns>
        [HttpPost("DeleteProjectWBSTree")]
        [UnitOfWork]
        public async Task<ResponseAjaxResult<bool>> DeleteProjectWBSTreeAsync([FromBody] DeleteProjectWBSRequestDto deleteProjectWBSRequestDto)
        {
            return await projectService.DeleteProjectWBSTreeAsync(deleteProjectWBSRequestDto.Id);
        }

        /// <summary>
        /// 搜索一个项目日报
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchProjectDayReport")]
        public async Task<ResponseAjaxResult<ProjectDayReportResponseDto>> SearchProjectDayReportAsync([FromQuery] ProjectDayReportRequestDto model)
        {
            return await projectReportService.SearchProjectDayReportAsync(model);
        }

        /// <summary>
        /// 保存项目日报
        /// </summary>
        /// <returns></returns>
        [HttpPost("SaveProjectDayReport")]
        [UnitOfWork]
        public async Task<ResponseAjaxResult<bool>> SaveProjectDayReportAsync([FromBody] AddOrUpdateDayReportRequestDto model)
        {
            return await projectReportService.SaveProjectDayReportAsync(model);
        }

        /// <summary>
        /// 获取项目部相关产值
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetOutputValue")]
        public async Task<ResponseAjaxResult<OutputValueResponseDto>> GetOutputValueAsync([FromQuery] OutputValueRequestDto model)
        {
            return await projectDepartMentService.GetOutputValueV2Async(model);
        }

        /// <summary>
        /// 项目部产值chart图
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        [HttpGet("GetOutputValueChart")]
        public async Task<ResponseAjaxResult<List<OutputValueChartResponseDto>>> GetOutputValueChartAsync([FromQuery] OutputChartRequestDto chartRequestDto)
        {
            return await projectDepartMentService.GetOutputValueChartAsync(chartRequestDto);
        }

        /// <summary>
        /// 获取用户的项目Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetUserProjectId")]
        public async Task<ResponseAjaxResult<GetUserProjectIdResponseDto>> GetUserProjectIdAsync()
        {
            return await projectDepartMentService.GetUserProjectIdAsync();
        }

        /// <summary>
        /// 新增或修改一个安监日报
        /// </summary>
        /// <returns></returns>
        [HttpPost("SaveSafeSupervisionDayReport")]
        [UnitOfWork]
        public async Task<ResponseAjaxResult<bool>> SaveSafeSupervisionDayReportAsync([FromBody] AddOrUpdateSafeSupervisionDayReportRequestDto model)
        {
            return await projectReportService.SaveSafeSupervisionDayReportAsync(model);
        }

        /// <summary>
        /// 搜索一个安监日报
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchSafeSupervisionDayReport")]
        public async Task<ResponseAjaxResult<SafeSupervisionDayReportResponseDto>> SearchSafeSupervisionDayReportAsync([FromQuery] SafeSupervisionDayReportRequestDto model)
        {
            return await projectReportService.SearchSafeSupervisionDayReportAsync(model);
        }

        /// <summary>
        /// 新增或修改一个船舶日报
        /// </summary>
        /// <returns></returns>
        [HttpPost("SaveShipDayReport")]
        [UnitOfWork]
        public async Task<ResponseAjaxResult<bool>> SaveShipDayReportAsync([FromBody] SaveShipDayReportRequestDto model)
        {
            return await projectReportService.SaveShipDayReportAsync(model);
        }

        /// <summary>
        /// 搜索一个船舶日报
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchShipDayReport")]
        public async Task<ResponseAjaxResult<ShipDayReportResponseDto>> SearchShipDayReportAsync([FromQuery] ShipDayReportRequestDto model)
        {
            return await projectReportService.SearchShipDayReportAsync(model);
        }
        /// <summary>
        /// 获取登陆人未填相关日报弹框信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetNotFillReportMsg")]
        public async Task<ResponseAjaxResult<BasePullDownResponseDto>> GetNotFillBulletBoxAsync()
        {
            return await projectService.GetNotFillBulletBoxAsync(CurrentUser);
        }

        /// <summary>
        /// 新增船舶进出场
        /// </summary>
        /// <returns></returns>
        [HttpPost("AddShipMovement")]
        public async Task<ResponseAjaxResult<bool>> AddShipMovementAsync([FromBody] AddShipMovementRequestDto model)
        {
            return await projectShipMovementsService.AddShipMovementAsync(model);
        }

        /// <summary>
        /// 更改船舶进出场
        /// </summary>
        /// <returns></returns>
        [HttpPost("ChangeShipMovementStatus")]
        public async Task<ResponseAjaxResult<bool>> ChangeShipMovementStatusAsync([FromBody] ChangeShipMovementStatusRequestDto model)
        {
            return await projectShipMovementsService.ChangeShipMovementStatusAsync(model);
        }

        /// <summary>
        ///  移除船舶进出场
        /// </summary>
        /// <returns></returns>
        [HttpPost("RemoveShipMovement")]
        public async Task<ResponseAjaxResult<bool>> RemoveShipMovementAsync([FromBody] RemoveShipMovementRequestDto model)
        {
            return await projectShipMovementsService.RemoveShipMovementAsync(model);
        }

        /// <summary>
        /// 项目-搜索船舶进出场列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchShipMovements")]
        public async Task<ResponseAjaxResult<List<ShipMovementResponseDto>>> SearchShipMovementsAsync([FromQuery] ShipMovementsRequestDto model)
        {
            return await projectShipMovementsService.SearchShipMovementsAsync(model);
        }

        /// <summary>
        /// 搜索进场船舶（自有船舶）
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchEnterShips")]
        public async Task<ResponseAjaxResult<EnterShipsResponseDto>> SearchEnterShipsAsync([FromQuery] EnterShipsRequestDto model)
        {
            return await projectShipMovementsService.SearchEnterShipsAsync(model);
        }

        /// <summary>
        /// 搜索一条项目月报
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchProjectMonthReport")]
        //public async Task<ResponseAjaxResult<ProjectMonthReportResponseDto>> SearchProjectMonthReportAsync([FromQuery] ProjectMonthReportRequestDto model)
        public async Task<ResponseAjaxResult<MonthReportForProjectResponseDto>> SearchMonthReportForProjectAsync([FromQuery] ProjectMonthReportRequestDto model)
        {
            //return await projectReportService.SearchProjectMonthReportAsync(model);
            return await _monthReportForProjectService.SearchMonthReportForProjectAsync(model);
        }

        /// <summary>
        ///保存一条项目月报
        /// </summary>
        /// <returns></returns>
        [HttpPost("SaveProjectMonthReport")]
        public async Task<ResponseAjaxResult<bool>> SaveProjectMonthReportAsync([FromBody] SaveProjectMonthReportRequestDto model)
        {
            return await projectReportService.SaveProjectMonthReportAsync(model);
        }

        /// <summary>
        /// 获取币种汇率
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet("GetCurrentcyRate")]
        public async Task<ResponseAjaxResult<ProjectCurrencyResponseDto>> GetCurrentcyRate([FromQuery] BasePrimaryRequestDto dto)
        {
            return await projectService.GetCurrentcyRate(dto.Id);
        }

        /// <summary>
        /// 搜索项目月报列表
        /// </summary>
        [HttpGet("SearchMonthReports")]
        public async Task<ResponseAjaxResult<MonthtReportsResponseDto>> SearchMonthReportsAsync([FromQuery] MonthtReportsRequstDto model)
        {
            model.IsFullExport = false;
            return await projectReportService.SearchMonthReportsAsync(model);
        }

        /// <summary>
        /// 保存一条自有船舶月报
        /// </summary>
        /// <returns></returns>
        [HttpPost("SaveOwnerShipMonthReport")]
        public async Task<ResponseAjaxResult<bool>> SaveOwnerShipMonthReportAsync([FromBody] SaveOwnerShipMonthReportRequestDto model)
        {
            return await projectReportService.SaveOwnerShipMonthReportAsync(model);
        }

        /// <summary>
        /// 搜索一条自有船舶月报
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchOwnerShipMonthReport")]
        public async Task<ResponseAjaxResult<OwnerShipMonthReportResponseDto>> SearchOwnerShipMonthReportAsync([FromQuery] OwnerShipMonthReportRequestDto model)
        {
            return await projectReportService.SearchOwnerShipMonthReportAsync(model);
        }

        /// <summary>
        /// 搜索月报船舶列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchShipsForMonthReport")]
        public async Task<ResponseAjaxResult<ShipsForReportResponseDto>> SearchShipsForMonthReportAsync([FromQuery] ShipsForReportRequestDto model)
        {
            return await projectReportService.SearchShipsForMonthReportAsync(model);
        }

        /// <summary>
        /// 保存一条分包船舶月报
        /// </summary>
        /// <returns></returns>
        [HttpPost("SaveSubShipMonthReport")]
        public async Task<ResponseAjaxResult<bool>> SaveSubShipMonthReportAsync([FromBody] SaveSubShipMonthReportRequestDto model)
        {
            return await projectReportService.SaveSubShipMonthReportAsync(model);
        }

        /// <summary>
        /// 搜索一条分包船舶月报
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchSubShipMonthReport")]
        public async Task<ResponseAjaxResult<SubShipMonthReportResponseDto>> SearchSubShipMonthReportAsync([FromQuery] SubShipMonthReportRequestDto model)
        {
            return await projectReportService.SearchSubShipMonthReportAsync(model);
        }
        /// <summary>
        /// 搜索未填项目日报的项目列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchUnReportProjects")]
        public async Task<ResponseAjaxResult<List<UnReportProjectResponseDto>>> SearchUnReportProjectsAsync([FromQuery] UnReportProjectsRequestDto model)
        {
            model.IsFullExport = false;
            return await projectReportService.SearchUnReportProjectsAsync(model);
        }

        /// <summary>
        /// 搜索未填安监日报的项目列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchUnReportSafeProjects")]
        public async Task<ResponseAjaxResult<List<UnReportProjectResponseDto>>> SearchUnReportSafeProjectsAsync([FromQuery] UnReportProjectsRequestDto model)
        {
            model.IsFullExport = false;
            return await projectReportService.SearchUnReportSafeProjectsAsync(model);
        }

        /// <summary>
        /// 搜索未填船舶日报的船舶列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchUnReportShips")]
        public async Task<ResponseAjaxResult<List<UnReportShipResponseDto>>> SearchUnReportShipsAsync([FromQuery] UnReportShipsRequestDto model)
        {
            model.IsFullExport = false;
            return await projectReportService.SearchUnReportShipsAsync(model);
        }



        #region 未填报  产值日报 安监日报 船舶日报 交建通消息提醒
        /// <summary>
        /// 未填报  产值日报 安监日报 船舶日报 交建通消息提醒
        /// Type  等于1是产值日报未填报通知   2是船舶日报  3是安监日报
        /// </summary>
        /// <param name="baseKeyWordsRequestDto">Type  等于1是产值日报未填报通知   2是船舶日报  3是安监日报</param>
        /// <returns></returns>
        [HttpGet("UnReportJjtNotif")]
        public async Task<ResponseAjaxResult<bool>> UnReportJjtNotifAsync([FromQuery] BaseKeyWordsRequestDto baseKeyWordsRequestDto)
        {
            return await projectReportService.UnReportJjtNotifAsync(baseKeyWordsRequestDto.Type);

        }
        #endregion

        /// <summary>
        /// 项目与报表负责人列表
        /// </summary>
        /// <param name="searchRequestDto"></param>
        /// <returns></returns>
        [HttpGet("ProjectAboutStatement")]
        public async Task<ResponseAjaxResult<List<ProjectResponseDto>>> ProjectAboutStatementAsync([FromQuery] ProjectAboutStatmentRequestDto searchRequestDto)
        {
            return await projectService.ProjectAboutStatementAsync(searchRequestDto);
        }
        /// <summary>
        /// 修改项目与报表负责人关系
        /// </summary>
        /// <param name="searchRequestDto"></param>
        /// <returns></returns>
        [HttpPost("UpdateProjectAboutStatement")]
        public async Task<ResponseAjaxResult<bool>> UpdateProjectAboutStatementAsync([FromBody] RetortformerRequestDto searchRequestDto)
        {
            return await projectService.UpdateProjectAboutStatementAsync(searchRequestDto);
        }
        /// <summary>
        /// 分包船舶列表
        /// </summary>
        /// <param name="sub"></param>
        /// <returns></returns>
        [HttpGet("SearchSubShipAndUser")]
        public async Task<ResponseAjaxResult<List<SubShipUserResponseDto>>> SearchSubShipAndUserAsync([FromQuery] SubShipUserRequestDto sub)
        {
            return await projectService.SearchSubShipAndUserAsync(sub);
        }
        /// <summary>
        /// 新增或修改分包船舶信息
        /// </summary>
        /// <param name="subShip"></param>
        /// <returns></returns>
        [HttpPost("SaveUserSubShip")]
        public async Task<ResponseAjaxResult<bool>> SaveSubShipUserAsync([FromBody] AddSubShipUserRequestDto subShip)
        {
            return await projectService.SaveSubShipUserAsync(subShip);
        }
        /// <summary>
        /// 删除分包船舶信息
        /// </summary>
        /// <param name="sub"></param>
        /// <returns></returns>
        [HttpPost("RemoveUserSubShip")]
        public async Task<ResponseAjaxResult<bool>> RemoveSubShipUserAsync([FromBody] SubShipRequestDto sub)
        {
            return await projectService.RemoveSubShipUserAsync(sub);
        }
        /// <summary>
        /// 自有船舶月报列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet("GetSearchOwnShipMonthRep")]
        public async Task<ResponseAjaxResult<SearchOwnShipMonthRepResponseDto>> GetSearchOwnShipMonthRepAsync([FromQuery] MonthRepRequestDto requestDto)
        {
            return await projectReportService.GetSearchOwnShipMonthRepAsync(requestDto, 1);
        }
        /// <summary>
        /// 分包船舶月报列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet("GetSearchSubShipMonthRep")]
        public async Task<ResponseAjaxResult<List<SearchSubShipMonthRepResponseDto>>> GetSearchSubShipMonthRepAsync([FromQuery] MonthRepRequestDto requestDto)
        {
            return await projectReportService.GetSearchSubShipMonthRepAsync(requestDto, 1);
        }
        /// <summary>
        /// 自有船舶未报月报列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetSearchNotFillOwnShipMonthRep")]
        public async Task<ResponseAjaxResult<List<NotFillOwnShipSearchResponseDto>>> GetSearchNotFillOwnShipSearchResponseDto([FromQuery] NotFillOwnShipRequestDto requestDto)
        {
            return await projectReportService.GetSearchNotFillOwnShipSearchResponseDto(requestDto);
        }

        /// <summary>
        ///暂存项目月报
        /// </summary>
        /// <returns></returns>
        [HttpPost("StagingMonthReport")]
        public async Task<ResponseAjaxResult<bool>> StagingMonthReportAsync(StagingMonthReportRequestDto model)
        {
            return await projectReportService.StagingMonthReportAsync(model);
        }
        /// <summary>
        /// 项目月报未填报列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetSearchNotFillProjectMonthRep")]
        public async Task<ResponseAjaxResult<List<UnReportProjectRepResponseDto>>> GetSearchNotFillProjectMonthRepAsync([FromQuery] ProjectMonthRepRequestDto projectMonthRepRequestDto)
        {
            return await projectReportService.GetSearchNotFillProjectMonthRepAsync(projectMonthRepRequestDto);
        }


        /// <summary>
        /// 保存项目计划信息
        /// </summary>
        /// <returns></returns>
        [HttpPost("SaveProjectPlan")]
        public async Task<ResponseAjaxResult<bool>> SaveProjectPlanAsync([FromBody] SaveProjectPlanRequestDto saveProjectPlanRequestDto)
        {
            return await projectService.SaveProjectPlanAsync(saveProjectPlanRequestDto);
        }

        /// <summary>
        /// 搜索项目计划表
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchProjectPlan")]
        public async Task<ResponseAjaxResult<ProjectPlanResponseDto>> SearchProjectPlanAsync([FromQuery] MonthlyPlanRequestDto monthlyPlanRequestDto)
        {
            return await projectService.SearchProjectPlanAsync(monthlyPlanRequestDto);
        }

        /// <summary>
        /// 查询项目计划详情
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet("SearchProjectPlanDetail")]
        public async Task<ResponseAjaxResult<ProjectPlanDetailResponseDto>> SearchProjectPlanDetailAsync([FromQuery] ProjectPlanSearchRequestDto requestDto)
        {
            return await projectService.SearchProjectPlanDetailAsync(requestDto);
        }

        /// <summary>
        /// 项目月报导出Wrod
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchMonthReportProjectWord")]
        public async Task<ResponseAjaxResult<List<MonthReportProjectWordResponseDto>>> SearchMonthReportProjectWordAsync([FromQuery] MonthtReportsRequstDto model)
        {
            return await projectReportService.SearchMonthReportProjectWordAsync(model);
        }
        /// <summary>
        /// 修改项目月报特殊字段推送
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("ModifyPushMonthReportSpecialFields")]

        public async Task<ResponseAjaxResult<bool>> ModifyPushMonthReportSpeailFiledAsync([FromBody] ModifyPushMonthReportSpecialFieldsRequestDto requestDto)
        {
            return await projectReportService.UpdatePushMonthReportSpeailFiledAsync(requestDto);
        }

        /// <summary>
        /// 保存项目审批人
        /// </summary>
        /// <returns></returns>
        [HttpPost("SaveProjectApprover")]
        public async Task<ResponseAjaxResult<bool>> SaveProjectApproverAsync([FromBody] SaveProjectApproverRequestDto model)
        {
            return await projectApproverService.SaveProjectApproverAsync(model);
        }

        /// <summary>
        /// 移除项目审批人
        /// </summary>
        /// <returns></returns>
        [HttpPost("RemoveProjectApprover")]
        public async Task<ResponseAjaxResult<bool>> RemoveProjectApproverAsync([FromBody] RemoveProjectApproverRequestDto model)
        {
            return await projectApproverService.RemoveProjectApproverAsync(model);
        }

        /// <summary>
        /// 获取项目审批人列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetProjectApprovers")]
        public async Task<ResponseAjaxResult<List<ProjectApproverResponseDto>>> GetProjectApproversAsync([FromQuery] ProjectApproverRequestDto model)
        {
            return await projectApproverService.GetProjectApproversAsync(model);
        }

        /// <summary>
        /// 获取项目带班生产动态
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetProjectShiftProduction")]
        [AllowAnonymous]
        public async Task<ResponseAjaxResult<ProjectShiftProductionResponseDto>> GetProjectShiftProductionAsync()
        {
            return await projectReportService.GetProjectShiftProductionAsync();
        }
        /// <summary>
        /// 获取已填报月份集合
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchReportedMonths")]
        public async Task<ResponseAjaxResult<ReportedMonthResponseDto>> SearchReportedMonthsAsync([FromQuery] ReportedMonthRequestDto model)
        {
            return await projectReportService.SearchReportedMonthsAsync(model);
        }


        /// <summary>
        /// 查询公司在手项目清单
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchCompanyProjectList")]
        [AllowAnonymous]
        public async Task<ResponseAjaxResult<List<CompanyProjectDetailedResponseDto>>> SearchCompanyProjectListAsync([FromQuery] CompanyProjectDetailsdRequestDto companyProjectDetailsdRequestDto)
        {
            return await projectService.SearchCompanyProjectListAsync(companyProjectDetailsdRequestDto);
        }

        /// <summary>
        /// 查询公司在手项目清单导出Excel
        /// </summary>
        /// <returns></returns>
        [HttpGet("ImportCompanyProjectList")]
        [AllowAnonymous]
        public async Task<IActionResult> ImportCompanyProjectListAsync([FromQuery] CompanyProjectDetailsdRequestDto companyProjectDetailsdRequestDto)
        {
            //var tempPath = "D:\\projectconllection\\dotnet\\szgh\\GHMonitoringCenterApi.Domain.Shared\\Template\\Excel\\CompanyOnProjectTemplate.xlsx";
            //var tempPath = "E:\\project\\HNKC.SZGHAPI\\GHMonitoringCenterApi.Domain.Shared\\Template\\Excel\\CompanyOnProjectTemplate.xlsx";
            var tempPath = "Template/Excel/CompanyOnProjectTemplate.xlsx";
            var data = await projectService.SearchCompanyProjectListAsync(companyProjectDetailsdRequestDto);
            var value = new
            {
                Year = DateTime.Now.Year,
                result = data.Data
            };
            return await ExcelTemplateImportAsync(tempPath, value, $"{DateTime.Now.Year}年公司在手项目清单");
        }

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

        /// <summary>
        /// 获取开停工记录
        /// </summary>
        /// <param name="baseRequestDto"></param>
        ///// <returns></returns>
        [HttpGet("SearchStartList")]
        public async Task<ResponseAjaxResult<List<StartWorkResponseDto>>> SearchStartList([FromQuery] StartWorkRequestDto startWorkRequestDto)
        {
            return await projectService.SearchStartListAsync(startWorkRequestDto.Id, startWorkRequestDto.PageIndex, startWorkRequestDto.PageSize);
        }


        /// <summary>
        /// 获取船舶进退场记录
        /// </summary>
        /// <param name="baseRequestDto"></param>
        ///// <returns></returns>
        [HttpGet("SearchShipMovement")]
        public async Task<ResponseAjaxResult<List<ShipMovementRecordResponseDto>>> SearchShipMovementAsync([FromQuery] StartWorkRequestDto startWorkRequestDto)
        {
            return await projectService.SearchShipMovementAsync(startWorkRequestDto.Id, startWorkRequestDto.PageIndex, startWorkRequestDto.PageSize);
        }

        /// <summary>
        /// 撤回项目月报
        /// </summary>
        /// <param name="basePrimaryRequestDto"></param>
        /// <returns></returns>
        [HttpGet("RevocationProjectMonth")]
        [UnitOfWork]
        public async Task<ResponseAjaxResult<bool>> RevocationProjectMonthAsync([FromQuery] BasePrimaryRequestDto basePrimaryRequestDto)
        {
            return await projectService.RevocationProjectMonthAsync(basePrimaryRequestDto.Id);
        }

        #region  新的项目月报列表
        [HttpPost("aa")]
        [AllowAnonymous]
        public bool aa([FromBody] string a)
        {
            // Excel 文件路径
            string filePath = @"C:\Users\pyej0\Desktop\2019年度产值完成情况与计划对比表（公布）改).xlsx";
            string filePath2 = @"C:\Users\pyej0\Desktop\2020年12月份产值完成情况与计划对比分析表（含排序后）改.xlsx";
            string filePath3 = @"C:\Users\pyej0\Desktop\2021年12月份产值完成情况与计划对比分析表2.xlsx";
            string filePath4 = @"C:\Users\pyej0\Desktop\2023年12月份产值完成情况与计划对比分析表（公布）改后(1).xlsx";
            string filePath5 = @"C:\Users\pyej0\Desktop\2022年12月份产值完成情况与计划对比分析表（公布）改后(1).xlsx";

            //数据写入中间表 
            List<ExcelConvertTable> excelVal = new();
            #region 2019年度在建项目产值完成情况与计划对比表                   
            var rows = MiniExcel
                        .Query(filePath)                      // 读取 Excel 文件
                        .Skip(6)                               // 跳过前6行，从第7行开始
                        .Select(row => new
                        {
                            ColumnA = row.A,   // A 列
                            ColumnB = row.B,   // B 列
                            ColumnG = row.G,   // G 列
                            ColumnBL = row.BL  // BL 列
                        })
                        .ToList();
            rows = rows.Where(x => !string.IsNullOrWhiteSpace(x.ColumnBL)).ToList();
            foreach (var row in rows)
            {
                excelVal.Add(new ExcelConvertTable
                {
                    Id = GuidUtil.Next().ToString(),
                    Name = row.ColumnB,
                    ProjectId = row.ColumnBL,
                    Val = (decimal)row.ColumnG,
                    Year = 2019
                });
            }
            #endregion

            #region 2020年年度在建项目产值完成情况与计划对比分析表                   
            var rows2 = MiniExcel
                        .Query(filePath2)                      // 读取 Excel 文件
                        .Skip(7)                               // 跳过前7行，从第8行开始
                        .Select(row => new
                        {
                            ColumnA = row.A,   // A 列
                            ColumnB = row.B,   // B 列
                            ColumnG = row.G,   // G 列
                            ColumnBK = row.BK  // Bk 列
                        })
                        .ToList();
            rows2 = rows2.Where(x => !string.IsNullOrWhiteSpace(x.ColumnBK)).ToList();
            foreach (var row in rows2)
            {
                excelVal.Add(new ExcelConvertTable
                {
                    Id = GuidUtil.Next().ToString(),
                    Name = row.ColumnB,
                    ProjectId = row.ColumnBK,
                    Val = (decimal)row.ColumnG,
                    Year = 2020
                });
            }
            #endregion

            #region 2021年12月份在建项目产值完成情况与计划对比分析表                   
            var rows3 = MiniExcel
                        .Query(filePath3)                      // 读取 Excel 文件
                        .Skip(9)                               // 跳过前9行，从第10行开始
                        .Select(row => new
                        {
                            ColumnB = row.B,   // B 列
                            ColumnC = row.C,   // C 列
                            ColumnH = row.H,   // H 列
                            ColumnAM = row.AM  // AM 列
                        })
                        .ToList();
            rows3 = rows3.Where(x => !string.IsNullOrWhiteSpace(x.ColumnAM)).ToList();
            foreach (var row in rows3)
            {
                excelVal.Add(new ExcelConvertTable
                {
                    Id = GuidUtil.Next().ToString(),
                    Name = row.ColumnC,
                    ProjectId = row.ColumnAM,
                    Val = (decimal)row.ColumnH,
                    Year = 2021
                });
            }
            #endregion

            #region 2023年12月份在建项目产值完成情况与计划对比分析表                                      
            var rows4 = MiniExcel
                        .Query(filePath4)                      // 读取 Excel 文件
                        .Skip(10)                               // 跳过前10行，从第11行开始
                        .Select(row => new
                        {
                            ColumnB = row.B,   // B 列
                            ColumnC = row.C,   // C 列
                            ColumnI = row.I,   // I 列
                            ColumnAX = row.AX  // AM 列
                        })
                        .ToList();
            rows4 = rows4.Where(x => !string.IsNullOrWhiteSpace(x.ColumnAX)).ToList();
            foreach (var row in rows4)
            {
                excelVal.Add(new ExcelConvertTable
                {
                    Id = GuidUtil.Next().ToString(),
                    Name = row.ColumnC,
                    ProjectId = row.ColumnAX,
                    Val = (decimal)row.ColumnI,
                    Year = 2023
                });
            }
            #endregion

            #region 2022年12月份在建项目产值完成情况与计划对比分析表                                      
            var rows5 = MiniExcel
                        .Query(filePath5)                      // 读取 Excel 文件
                        .Skip(10)                               // 跳过前10行，从第11行开始
                        .Select(row => new
                        {
                            ColumnB = row.B,   // B 列
                            ColumnC = row.C,   // C 列
                            ColumnJ = row.J,   // J 列
                            ColumnAP = row.AP  // AP 列
                        })
                        .ToList();
            rows5 = rows5.Where(x => !string.IsNullOrWhiteSpace(x.ColumnAP)).ToList();
            foreach (var row in rows5)
            {
                excelVal.Add(new ExcelConvertTable
                {
                    Id = GuidUtil.Next().ToString(),
                    Name = row.ColumnC,
                    ProjectId = row.ColumnAP,
                    Val = (decimal)row.ColumnJ,
                    Year = 2022
                });
            }
            #endregion

            _db.Insertable(excelVal).ExecuteCommand();
            //var resList = JsonConvert.DeserializeObject<List<bb>>(jsonObject);
            //var ss = resList.Sum(x => x.UnitPrice * x.CompletedQuantity);
            //return projectService.aa();
            return true;
        }

        #endregion
    }
}
