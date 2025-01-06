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
        /// <summary>
        /// 历史产值月报列表
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        [HttpGet("SearchHistoryProjectMonthRep")]
        public async Task<ResponseAjaxResult<List<HistoryProjectMonthReportResponseDto>>> SearchHistoryProjectMonthRepAsync([FromQuery] HistoryProjectMonthReportRequestDto requestBody)
        {
            return await projectService.SearchHistoryProjectMonthRepAsync(requestBody);
        }
        /// <summary>
        /// 获取指定用户权限
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetPermissions")]
        public ResponseAjaxResult<bool> GetPermissionsByUser()
        {
            ResponseAjaxResult<bool> rt = new();
            if (CurrentUser.Account == "2016146340" || CurrentUser.Account == "2022002687") rt.Data = true;
            else rt.Data = false;
            return rt;
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
            List<ExcelProductionConvertTable> excelComplete = new();
            List<ExcelPlanConvertTable> excelPlan = new();
            #region 2019年度在建项目产值完成情况与计划对比表                   
            var rows = MiniExcel
                        .Query(filePath)                      // 读取 Excel 文件
                        .Skip(6)                               // 跳过前6行，从第7行开始
                        .Select(row => new
                        {
                            ColumnB = row.B,   // B 列
                            ColumnBL = row.BL,  // BL 列
                            ColumnK = row.K == null ? 0M : (decimal)row.K,
                            ColumnL = row.L == null ? 0M : (decimal)row.L,
                            ColumnO = row.O == null ? 0M : (decimal)row.O,
                            ColumnP = row.P == null ? 0M : (decimal)row.P,
                            ColumnS = row.S == null ? 0M : (decimal)row.S,
                            ColumnT = row.T == null ? 0M : (decimal)row.T,
                            ColumnW = row.W == null ? 0M : (decimal)row.W,
                            ColumnX = row.X == null ? 0M : (decimal)row.X,
                            ColumnAA = row.AA == null ? 0M : (decimal)row.AA,
                            ColumnAB = row.AB == null ? 0M : (decimal)row.AB,
                            ColumnAE = row.AE == null ? 0M : (decimal)row.AE,
                            ColumnAF = row.AF == null ? 0M : (decimal)row.AF,
                            ColumnAI = row.AI == null ? 0M : (decimal)row.AI,
                            ColumnAJ = row.AJ == null ? 0M : (decimal)row.AJ,
                            ColumnAM = row.AM == null ? 0M : (decimal)row.AM,
                            ColumnAN = row.AN == null ? 0M : (decimal)row.AN,
                            ColumnAQ = row.AQ == null ? 0M : (decimal)row.AQ,
                            ColumnAR = row.AR == null ? 0M : (decimal)row.AR,
                            ColumnAU = row.AU == null ? 0M : (decimal)row.AU,
                            ColumnAV = row.AV == null ? 0M : (decimal)row.AV,
                            ColumnAY = row.AY == null ? 0M : (decimal)row.AY,
                            ColumnAZ = row.AZ == null ? 0M : (decimal)row.AZ,
                            ColumnBC = row.BC == null ? 0M : (decimal)row.BC,
                            ColumnBD = row.BD == null ? 0M : (decimal)row.BD
                        })
                        .ToList();
            rows = rows.Where(x => !string.IsNullOrEmpty(x.ColumnBL)).ToList();

            foreach (var row in rows)
            {
                for (int i = 0; i < 12; i++)
                {
                    switch (i)
                    {
                        case 0:
                            excelComplete.Add(new ExcelProductionConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnB,
                                ProjectId = row.ColumnBL,
                                OneCompleteValue = row.ColumnL,
                                DateMonth = 201901,
                                Year = 2019
                            });
                            excelPlan.Add(new ExcelPlanConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnB,
                                ProjectId = row.ColumnBL,
                                OnePlanProductionValue = row.ColumnK,
                                DateMonth = 201901,
                                Year = 2019
                            });
                            break;
                        case 1:
                            excelComplete.Add(new ExcelProductionConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnB,
                                ProjectId = row.ColumnBL,
                                TwoCompleteValue = row.ColumnP,
                                Year = 2019,
                                DateMonth = 201902
                            });
                            excelPlan.Add(new ExcelPlanConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnB,
                                ProjectId = row.ColumnBL,
                                TwoPlanProductionValue = row.ColumnO,
                                DateMonth = 201902,
                                Year = 2019
                            });
                            break;
                        case 2:
                            excelComplete.Add(new ExcelProductionConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnB,
                                ProjectId = row.ColumnBL,
                                ThreeCompleteValue = row.ColumnT,
                                Year = 2019,
                                DateMonth = 201903
                            });
                            excelPlan.Add(new ExcelPlanConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnB,
                                ProjectId = row.ColumnBL,
                                ThreePlanProductionValue = row.ColumnS,
                                DateMonth = 201903,
                                Year = 2019
                            });
                            break;
                        case 3:
                            excelComplete.Add(new ExcelProductionConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnB,
                                ProjectId = row.ColumnBL,
                                FourCompleteValue = row.ColumnX,
                                Year = 2019,
                                DateMonth = 201904
                            });
                            excelPlan.Add(new ExcelPlanConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnB,
                                ProjectId = row.ColumnBL,
                                FourPlanProductionValue = row.ColumnW,
                                DateMonth = 201904,
                                Year = 2019
                            });
                            break;
                        case 4:
                            excelComplete.Add(new ExcelProductionConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnB,
                                ProjectId = row.ColumnBL,
                                FiveCompleteValue = row.ColumnAB,
                                Year = 2019,
                                DateMonth = 201905
                            });
                            excelPlan.Add(new ExcelPlanConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnB,
                                ProjectId = row.ColumnBL,
                                FivePlanProductionValue = row.ColumnAA,
                                DateMonth = 201905,
                                Year = 2019
                            });
                            break;
                        case 5:
                            excelComplete.Add(new ExcelProductionConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnB,
                                ProjectId = row.ColumnBL,
                                SixCompleteValue = row.ColumnAF,
                                Year = 2019,
                                DateMonth = 201906
                            });
                            excelPlan.Add(new ExcelPlanConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnB,
                                ProjectId = row.ColumnBL,
                                SixPlanProductionValue = row.ColumnAE,
                                DateMonth = 201906,
                                Year = 2019
                            });
                            break;
                        case 6:
                            excelComplete.Add(new ExcelProductionConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnB,
                                ProjectId = row.ColumnBL,
                                SevenCompleteValue = row.ColumnAJ,
                                Year = 2019,
                                DateMonth = 201907
                            });
                            excelPlan.Add(new ExcelPlanConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnB,
                                ProjectId = row.ColumnBL,
                                SevenPlanProductionValue = row.ColumnAI,
                                DateMonth = 201907,
                                Year = 2019
                            });
                            break;
                        case 7:
                            excelComplete.Add(new ExcelProductionConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnB,
                                ProjectId = row.ColumnBL,
                                EightCompleteValue = row.ColumnAN,
                                Year = 2019,
                                DateMonth = 201908
                            });
                            excelPlan.Add(new ExcelPlanConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnB,
                                ProjectId = row.ColumnBL,
                                EightPlanProductionValue = row.ColumnAM,
                                DateMonth = 201908,
                                Year = 2019
                            });
                            break;
                        case 8:
                            excelComplete.Add(new ExcelProductionConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnB,
                                ProjectId = row.ColumnBL,
                                NineCompleteValue = row.ColumnAR,
                                Year = 2019,
                                DateMonth = 201909
                            });
                            excelPlan.Add(new ExcelPlanConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnB,
                                ProjectId = row.ColumnBL,
                                NinePlanProductionValue = row.ColumnAQ,
                                DateMonth = 201909,
                                Year = 2019
                            });
                            break;
                        case 9:
                            excelComplete.Add(new ExcelProductionConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnB,
                                ProjectId = row.ColumnBL,
                                TenCompleteValue = row.ColumnAV,
                                Year = 2019,
                                DateMonth = 201910
                            });
                            excelPlan.Add(new ExcelPlanConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnB,
                                ProjectId = row.ColumnBL,
                                TenPlanProductionValue = row.ColumnAU,
                                DateMonth = 201910,
                                Year = 2019
                            });
                            break;
                        case 10:
                            excelComplete.Add(new ExcelProductionConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnB,
                                ProjectId = row.ColumnBL,
                                ElevenCompleteValue = row.ColumnAZ,
                                Year = 2019,
                                DateMonth = 201911
                            });
                            excelPlan.Add(new ExcelPlanConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnB,
                                ProjectId = row.ColumnBL,
                                ElevenPlanProductionValue = row.ColumnAY,
                                DateMonth = 201911,
                                Year = 2019
                            });
                            break;
                        case 11:
                            excelComplete.Add(new ExcelProductionConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnB,
                                ProjectId = row.ColumnBL,
                                TwelveCompleteValue = row.ColumnBD,
                                Year = 2019,
                                DateMonth = 201912
                            });
                            excelPlan.Add(new ExcelPlanConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnB,
                                ProjectId = row.ColumnBL,
                                TwelvePlanProductionValue = row.ColumnBC,
                                DateMonth = 201912,
                                Year = 2019
                            });
                            break;
                    }
                }
            }
            #endregion

            #region 2020年年度在建项目产值完成情况与计划对比分析表                   
            var rows2 = MiniExcel
                        .Query(filePath2)                      // 读取 Excel 文件
                        .Skip(7)                               // 跳过前7行，从第8行开始
                        .Select(row => new
                        {
                            ColumnB = row.B,   // B 列
                            ColumnBK = row.BK,  // Bk 列
                            ColumnK = row.K == null ? 0M : (decimal)row.K,
                            ColumnL = row.L == null ? 0M : (decimal)row.L,
                            ColumnO = row.O == null ? 0M : (decimal)row.O,
                            ColumnP = row.P == null ? 0M : (decimal)row.P,
                            ColumnS = row.S == null ? 0M : (decimal)row.S,
                            ColumnT = row.T == null ? 0M : (decimal)row.T,
                            ColumnW = row.W == null ? 0M : (decimal)row.W,
                            ColumnX = row.X == null ? 0M : (decimal)row.X,
                            ColumnAA = row.AA == null ? 0M : (decimal)row.AA,
                            ColumnAB = row.AB == null ? 0M : (decimal)row.AB,
                            ColumnAE = row.AE == null ? 0M : (decimal)row.AE,
                            ColumnAF = row.AF == null ? 0M : (decimal)row.AF,
                            ColumnAI = row.AI == null ? 0M : (decimal)row.AI,
                            ColumnAJ = row.AJ == null ? 0M : (decimal)row.AJ,
                            ColumnAM = row.AM == null ? 0M : (decimal)row.AM,
                            ColumnAN = row.AN == null ? 0M : (decimal)row.AN,
                            ColumnAQ = row.AQ == null ? 0M : (decimal)row.AQ,
                            ColumnAR = row.AR == null ? 0M : (decimal)row.AR,
                            ColumnAV = row.AV == null ? 0M : (decimal)row.AV,
                            ColumnAW = row.AW == null ? 0M : (decimal)row.AW,
                            ColumnBA = row.BA == null ? 0M : (decimal)row.BA,
                            ColumnBB = row.BB == null ? 0M : (decimal)row.BB,
                            ColumnBF = row.BF == null ? 0M : (decimal)row.BF,
                            ColumnBG = row.BG == null ? 0M : (decimal)row.BG
                        })
                        .ToList();
            rows2 = rows2.Where(x => !string.IsNullOrEmpty(x.ColumnBK)).ToList();
            foreach (var row in rows2)
            {
                for (int i = 0; i < 12; i++)
                {
                    switch (i)
                    {
                        case 0:
                            excelComplete.Add(new ExcelProductionConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnB,
                                ProjectId = row.ColumnBK,
                                OneCompleteValue = row.ColumnL,
                                Year = 2020,
                                DateMonth = 202001
                            });
                            excelPlan.Add(new ExcelPlanConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnB,
                                ProjectId = row.ColumnBK,
                                OnePlanProductionValue = row.ColumnK,
                                Year = 2020,
                                DateMonth = 202001
                            });
                            break;
                        case 1:
                            excelComplete.Add(new ExcelProductionConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnB,
                                ProjectId = row.ColumnBK,
                                TwoCompleteValue = row.ColumnP,
                                Year = 2020,
                                DateMonth = 202002
                            });
                            excelPlan.Add(new ExcelPlanConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnB,
                                ProjectId = row.ColumnBK,
                                TwoPlanProductionValue = row.ColumnO,
                                Year = 2020,
                                DateMonth = 202002
                            });
                            break;
                        case 2:
                            excelComplete.Add(new ExcelProductionConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnB,
                                ProjectId = row.ColumnBK,
                                ThreeCompleteValue = row.ColumnT,
                                Year = 2020,
                                DateMonth = 202003
                            });
                            excelPlan.Add(new ExcelPlanConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnB,
                                ProjectId = row.ColumnBK,
                                ThreePlanProductionValue = row.ColumnS,
                                Year = 2020,
                                DateMonth = 202003
                            });
                            break;
                        case 3:
                            excelComplete.Add(new ExcelProductionConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnB,
                                ProjectId = row.ColumnBK,
                                FourCompleteValue = row.ColumnX,
                                Year = 2020,
                                DateMonth = 202004
                            });
                            excelPlan.Add(new ExcelPlanConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnB,
                                ProjectId = row.ColumnBK,
                                FourPlanProductionValue = row.ColumnW,
                                Year = 2020,
                                DateMonth = 202004
                            });
                            break;
                        case 4:
                            excelComplete.Add(new ExcelProductionConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnB,
                                ProjectId = row.ColumnBK,
                                FiveCompleteValue = row.ColumnAB,
                                Year = 2020,
                                DateMonth = 202005
                            });
                            excelPlan.Add(new ExcelPlanConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnB,
                                ProjectId = row.ColumnBK,
                                FivePlanProductionValue = row.ColumnAA,
                                Year = 2020,
                                DateMonth = 202005
                            });
                            break;
                        case 5:
                            excelComplete.Add(new ExcelProductionConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnB,
                                ProjectId = row.ColumnBK,
                                SixCompleteValue = row.ColumnAF,
                                Year = 2020,
                                DateMonth = 202006
                            });
                            excelPlan.Add(new ExcelPlanConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnB,
                                ProjectId = row.ColumnBK,
                                SixPlanProductionValue = row.ColumnAE,
                                Year = 2020,
                                DateMonth = 202006
                            });
                            break;
                        case 6:
                            excelComplete.Add(new ExcelProductionConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnB,
                                ProjectId = row.ColumnBK,
                                SevenCompleteValue = row.ColumnAJ,
                                Year = 2020,
                                DateMonth = 202007
                            });
                            excelPlan.Add(new ExcelPlanConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnB,
                                ProjectId = row.ColumnBK,
                                SevenPlanProductionValue = row.ColumnAI,
                                Year = 2020,
                                DateMonth = 202007
                            });
                            break;
                        case 7:
                            excelComplete.Add(new ExcelProductionConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnB,
                                ProjectId = row.ColumnBK,
                                EightCompleteValue = row.ColumnAN,
                                Year = 2020,
                                DateMonth = 202008
                            });
                            excelPlan.Add(new ExcelPlanConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnB,
                                ProjectId = row.ColumnBK,
                                EightPlanProductionValue = row.ColumnAM,
                                Year = 2020,
                                DateMonth = 202008
                            });
                            break;
                        case 8:
                            excelComplete.Add(new ExcelProductionConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnB,
                                ProjectId = row.ColumnBK,
                                NineCompleteValue = row.ColumnAR,
                                Year = 2020,
                                DateMonth = 202009
                            });
                            excelPlan.Add(new ExcelPlanConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnB,
                                ProjectId = row.ColumnBK,
                                NinePlanProductionValue = row.ColumnAQ,
                                Year = 2020,
                                DateMonth = 202009
                            });
                            break;
                        case 9:
                            excelComplete.Add(new ExcelProductionConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnB,
                                ProjectId = row.ColumnBK,
                                TenCompleteValue = row.ColumnAW,
                                Year = 2020,
                                DateMonth = 202010
                            });
                            excelPlan.Add(new ExcelPlanConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnB,
                                ProjectId = row.ColumnBK,
                                TenPlanProductionValue = row.ColumnAV,
                                Year = 2020,
                                DateMonth = 202010
                            });
                            break;
                        case 10:
                            excelComplete.Add(new ExcelProductionConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnB,
                                ProjectId = row.ColumnBK,
                                ElevenCompleteValue = row.ColumnBB,
                                Year = 2020,
                                DateMonth = 202011
                            });
                            excelPlan.Add(new ExcelPlanConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnB,
                                ProjectId = row.ColumnBK,
                                ElevenPlanProductionValue = row.ColumnBA,
                                Year = 2020,
                                DateMonth = 202011
                            });
                            break;
                        case 11:
                            excelComplete.Add(new ExcelProductionConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnB,
                                ProjectId = row.ColumnBK,
                                TwelveCompleteValue = row.ColumnBG,
                                Year = 2020,
                                DateMonth = 202012
                            });
                            excelPlan.Add(new ExcelPlanConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnB,
                                ProjectId = row.ColumnBK,
                                TwelvePlanProductionValue = row.ColumnBF,
                                Year = 2020,
                                DateMonth = 202012
                            });
                            break;
                    }

                }
            }
            #endregion

            #region 2021年12月份在建项目产值完成情况与计划对比分析表                   
            var rows3 = MiniExcel
                        .Query(filePath3)                      // 读取 Excel 文件
                        .Skip(9)                               // 跳过前9行，从第10行开始
                        .Select(row => new
                        {
                            ColumnC = row.C,   // C 列
                            ColumnAM = row.AM, // AM 列
                            ColumnJ = row.J == null ? 0M : (decimal)row.J,
                            ColumnK = row.K == null ? 0M : (decimal)row.K,
                            ColumnL = row.L == null ? 0M : (decimal)row.L,
                            ColumnM = row.M == null ? 0M : (decimal)row.M,
                            ColumnN = row.N == null ? 0M : (decimal)row.N,
                            ColumnO = row.O == null ? 0M : (decimal)row.O,
                            ColumnP = row.P == null ? 0M : (decimal)row.P,
                            ColumnQ = row.Q == null ? 0M : (decimal)row.Q,
                            ColumnR = row.R == null ? 0M : (decimal)row.R,
                            ColumnS = row.S == null ? 0M : (decimal)row.S,
                            ColumnT = row.T == null ? 0M : (decimal)row.T,
                            ColumnU = row.U == null ? 0M : (decimal)row.U,
                            ColumnV = row.V == null ? 0M : (decimal)row.V,
                            ColumnW = row.W == null ? 0M : (decimal)row.W,
                            ColumnX = row.X == null ? 0M : (decimal)row.X,
                            ColumnY = row.Y == null ? 0M : (decimal)row.Y,
                            ColumnZ = row.Z == null ? 0M : (decimal)row.Z,
                            ColumnAA = row.AA == null ? 0M : (decimal)row.AA,
                            ColumnAB = row.AB == null ? 0M : (decimal)row.AB,//十月完成产值  未存在十月计划
                            //ColumnAW = row.AW,
                            ColumnAD = row.AD == null ? 0M : (decimal)row.AD,
                            ColumnAF = row.AF == null ? 0M : (decimal)row.AF,
                            ColumnAH = row.AH == null ? 0M : (decimal)row.AH,
                            ColumnAI = row.AI == null ? 0M : (decimal)row.AI
                        })
                        .ToList();
            rows3 = rows3.Where(x => !string.IsNullOrEmpty(x.ColumnAM)).ToList();
            foreach (var row in rows3)
            {
                for (int i = 0; i < 12; i++)
                {
                    switch (i)
                    {
                        case 0:
                            excelComplete.Add(new ExcelProductionConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnC,
                                ProjectId = row.ColumnAM,
                                OneCompleteValue = row.ColumnK,
                                Year = 2021,
                                DateMonth = 202101
                            });
                            excelPlan.Add(new ExcelPlanConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnC,
                                ProjectId = row.ColumnAM,
                                OnePlanProductionValue = row.ColumnJ,
                                Year = 2021,
                                DateMonth = 202101
                            });
                            break;
                        case 1:
                            excelComplete.Add(new ExcelProductionConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnC,
                                ProjectId = row.ColumnAM,
                                TwoCompleteValue = row.ColumnM,
                                Year = 2021,
                                DateMonth = 202102
                            });
                            excelPlan.Add(new ExcelPlanConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnC,
                                ProjectId = row.ColumnAM,
                                TwoPlanProductionValue = row.ColumnL,
                                Year = 2021,
                                DateMonth = 202102
                            });
                            break;
                        case 2:
                            excelComplete.Add(new ExcelProductionConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnC,
                                ProjectId = row.ColumnAM,
                                ThreeCompleteValue = row.ColumnO,
                                Year = 2021,
                                DateMonth = 202103
                            });
                            excelPlan.Add(new ExcelPlanConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnC,
                                ProjectId = row.ColumnAM,
                                ThreePlanProductionValue = row.ColumnN,
                                Year = 2021,
                                DateMonth = 202103
                            });
                            break;
                        case 3:
                            excelComplete.Add(new ExcelProductionConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnC,
                                ProjectId = row.ColumnAM,
                                FourCompleteValue = row.ColumnQ,
                                Year = 2021,
                                DateMonth = 202104
                            });
                            excelPlan.Add(new ExcelPlanConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnC,
                                ProjectId = row.ColumnAM,
                                FourPlanProductionValue = row.ColumnP,
                                Year = 2021,
                                DateMonth = 202104
                            });
                            break;
                        case 4:
                            excelComplete.Add(new ExcelProductionConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnC,
                                ProjectId = row.ColumnAM,
                                FiveCompleteValue = row.ColumnS,
                                Year = 2021,
                                DateMonth = 202105
                            });
                            excelPlan.Add(new ExcelPlanConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnC,
                                ProjectId = row.ColumnAM,
                                FivePlanProductionValue = row.ColumnR,
                                Year = 2021,
                                DateMonth = 202105
                            });
                            break;
                        case 5:
                            excelComplete.Add(new ExcelProductionConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnC,
                                ProjectId = row.ColumnAM,
                                SixCompleteValue = row.ColumnU,
                                Year = 2021,
                                DateMonth = 202106
                            });
                            excelPlan.Add(new ExcelPlanConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnC,
                                ProjectId = row.ColumnAM,
                                SixPlanProductionValue = row.ColumnT,
                                Year = 2021,
                                DateMonth = 202106
                            });
                            break;
                        case 6:
                            excelComplete.Add(new ExcelProductionConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnC,
                                ProjectId = row.ColumnAM,
                                SevenCompleteValue = row.ColumnW,
                                Year = 2021,
                                DateMonth = 202107
                            });
                            excelPlan.Add(new ExcelPlanConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnC,
                                ProjectId = row.ColumnAM,
                                SevenPlanProductionValue = row.ColumnV,
                                Year = 2021,
                                DateMonth = 202107
                            });
                            break;
                        case 7:
                            excelComplete.Add(new ExcelProductionConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnC,
                                ProjectId = row.ColumnAM,
                                EightCompleteValue = row.ColumnY,
                                Year = 2021,
                                DateMonth = 202108
                            });
                            excelPlan.Add(new ExcelPlanConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnC,
                                ProjectId = row.ColumnAM,
                                EightPlanProductionValue = row.ColumnX,
                                Year = 2021,
                                DateMonth = 202108
                            });
                            break;
                        case 8:
                            excelComplete.Add(new ExcelProductionConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnC,
                                ProjectId = row.ColumnAM,
                                NineCompleteValue = row.ColumnAA,
                                Year = 2021,
                                DateMonth = 202109
                            });
                            excelPlan.Add(new ExcelPlanConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnC,
                                ProjectId = row.ColumnAM,
                                NinePlanProductionValue = row.ColumnZ,
                                Year = 2021,
                                DateMonth = 202109
                            });
                            break;
                        case 9:
                            excelComplete.Add(new ExcelProductionConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnC,
                                ProjectId = row.ColumnAM,
                                TenCompleteValue = row.ColumnAB,
                                Year = 2021,
                                DateMonth = 202110
                            });
                            excelPlan.Add(new ExcelPlanConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnC,
                                ProjectId = row.ColumnAM,
                                //TenPlanProductionValue = row.ColumnAV
                                Year = 2021,
                                DateMonth = 202110
                            });
                            break;
                        case 10:
                            excelComplete.Add(new ExcelProductionConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnC,
                                ProjectId = row.ColumnAM,
                                ElevenCompleteValue = row.ColumnAF,
                                Year = 2021,
                                DateMonth = 202111
                            });
                            excelPlan.Add(new ExcelPlanConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnC,
                                ProjectId = row.ColumnAM,
                                ElevenPlanProductionValue = row.ColumnAD,
                                Year = 2021,
                                DateMonth = 202111
                            });
                            break;
                        case 11:
                            excelComplete.Add(new ExcelProductionConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnC,
                                ProjectId = row.ColumnAM,
                                TwelveCompleteValue = row.ColumnAI,
                                Year = 2021,
                                DateMonth = 202112
                            });
                            excelPlan.Add(new ExcelPlanConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnC,
                                ProjectId = row.ColumnAM,
                                TwelvePlanProductionValue = row.ColumnAH,
                                Year = 2021,
                                DateMonth = 202112
                            });
                            break;
                    }
                }
            }
            #endregion

            #region 2023年12月份在建项目产值完成情况与计划对比分析表                                      
            var rows4 = MiniExcel
                        .Query(filePath4)                      // 读取 Excel 文件
                        .Skip(10)                               // 跳过前10行，从第11行开始
                        .Select(row => new
                        {
                            ColumnC = row.C,   // C 列
                            ColumnAX = row.AX,  // AM 列
                            ColumnL = row.L == null ? 0M : (decimal)row.L,
                            ColumnM = row.M == null ? 0M : (decimal)row.M,
                            ColumnO = row.O == null ? 0M : (decimal)row.O,
                            ColumnP = row.P == null ? 0M : (decimal)row.P,
                            ColumnR = row.R == null ? 0M : (decimal)row.R,
                            ColumnS = row.S == null ? 0M : (decimal)row.S,
                            ColumnU = row.U == null ? 0M : (decimal)row.U,
                            ColumnV = row.V == null ? 0M : (decimal)row.V,
                            ColumnX = row.X == null ? 0M : (decimal)row.X,
                            ColumnY = row.Y == null ? 0M : (decimal)row.Y,
                            ColumnAA = row.AA == null ? 0M : (decimal)row.AA,
                            ColumnAB = row.AB == null ? 0M : (decimal)row.AB,
                            ColumnAD = row.AD == null ? 0M : (decimal)row.AD,
                            ColumnAE = row.AE == null ? 0M : (decimal)row.AE,
                            ColumnAG = row.AG == null ? 0M : (decimal)row.AG,
                            ColumnAH = row.AH == null ? 0M : (decimal)row.AH,
                            ColumnAJ = row.AJ == null ? 0M : (decimal)row.AJ,
                            ColumnAK = row.AK == null ? 0M : (decimal)row.AK,
                            ColumnAM = row.AM == null ? 0M : (decimal)row.AM,
                            ColumnAN = row.AN == null ? 0M : (decimal)row.AN,
                            ColumnAP = row.AP == null ? 0M : (decimal)row.AP,
                            ColumnAQ = row.AQ == null ? 0M : (decimal)row.AQ,
                            ColumnAS = row.AS == null ? 0M : (decimal)row.AS,
                            ColumnAT = row.AT == null ? 0M : (decimal)row.AT
                        })
                        .ToList();
            rows4 = rows4.Where(x => !string.IsNullOrEmpty(x.ColumnAX)).ToList();
            foreach (var row in rows4)
            {
                for (int i = 0; i < 12; i++)
                {
                    switch (i)
                    {
                        case 0:
                            excelComplete.Add(new ExcelProductionConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnC,
                                ProjectId = row.ColumnAX,
                                OneCompleteValue = row.ColumnM,
                                Year = 2023,
                                DateMonth = 202301
                            });
                            excelPlan.Add(new ExcelPlanConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnC,
                                ProjectId = row.ColumnAX,
                                OnePlanProductionValue = row.ColumnL,
                                Year = 2023,
                                DateMonth = 202301
                            });
                            break;
                        case 1:
                            excelComplete.Add(new ExcelProductionConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnC,
                                ProjectId = row.ColumnAX,
                                TwoCompleteValue = row.ColumnP,
                                Year = 2023,
                                DateMonth = 202302
                            });
                            excelPlan.Add(new ExcelPlanConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnC,
                                ProjectId = row.ColumnAX,
                                TwoPlanProductionValue = row.ColumnO,
                                Year = 2023,
                                DateMonth = 202302
                            });
                            break;
                        case 2:
                            excelComplete.Add(new ExcelProductionConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnC,
                                ProjectId = row.ColumnAX,
                                ThreeCompleteValue = row.ColumnS,
                                Year = 2023,
                                DateMonth = 202303
                            });
                            excelPlan.Add(new ExcelPlanConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnC,
                                ProjectId = row.ColumnAX,
                                ThreePlanProductionValue = row.ColumnR,
                                Year = 2023,
                                DateMonth = 202303
                            });
                            break;
                        case 3:
                            excelComplete.Add(new ExcelProductionConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnC,
                                ProjectId = row.ColumnAX,
                                FourCompleteValue = row.ColumnV,
                                Year = 2023,
                                DateMonth = 202304
                            });
                            excelPlan.Add(new ExcelPlanConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnC,
                                ProjectId = row.ColumnAX,
                                FourPlanProductionValue = row.ColumnU,
                                Year = 2023,
                                DateMonth = 202304
                            });
                            break;
                        case 4:
                            excelComplete.Add(new ExcelProductionConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnC,
                                ProjectId = row.ColumnAX,
                                FiveCompleteValue = row.ColumnY,
                                Year = 2023,
                                DateMonth = 202305
                            });
                            excelPlan.Add(new ExcelPlanConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnC,
                                ProjectId = row.ColumnAX,
                                FivePlanProductionValue = row.ColumnX,
                                Year = 2023,
                                DateMonth = 202305
                            });
                            break;
                        case 5:
                            excelComplete.Add(new ExcelProductionConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnC,
                                ProjectId = row.ColumnAX,
                                SixCompleteValue = row.ColumnAB,
                                Year = 2023,
                                DateMonth = 202306
                            });
                            excelPlan.Add(new ExcelPlanConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnC,
                                ProjectId = row.ColumnAX,
                                SixPlanProductionValue = row.ColumnAA,
                                Year = 2023,
                                DateMonth = 202306
                            });
                            break;
                        case 6:
                            excelComplete.Add(new ExcelProductionConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnC,
                                ProjectId = row.ColumnAX,
                                SevenCompleteValue = row.ColumnAE,
                                Year = 2023,
                                DateMonth = 202307
                            });
                            excelPlan.Add(new ExcelPlanConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnC,
                                ProjectId = row.ColumnAX,
                                SevenPlanProductionValue = row.ColumnAD,
                                Year = 2023,
                                DateMonth = 202307
                            });
                            break;
                        case 7:
                            excelComplete.Add(new ExcelProductionConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnC,
                                ProjectId = row.ColumnAX,
                                EightCompleteValue = row.ColumnAH,
                                Year = 2023,
                                DateMonth = 202308
                            });
                            excelPlan.Add(new ExcelPlanConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnC,
                                ProjectId = row.ColumnAX,
                                EightPlanProductionValue = row.ColumnAG,
                                Year = 2023,
                                DateMonth = 202308
                            });
                            break;
                        case 8:
                            excelComplete.Add(new ExcelProductionConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnC,
                                ProjectId = row.ColumnAX,
                                NineCompleteValue = row.ColumnAK,
                                Year = 2023,
                                DateMonth = 202309
                            });
                            excelPlan.Add(new ExcelPlanConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnC,
                                ProjectId = row.ColumnAX,
                                NinePlanProductionValue = row.ColumnAJ,
                                Year = 2023,
                                DateMonth = 202309
                            });
                            break;
                        case 9:
                            excelComplete.Add(new ExcelProductionConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnC,
                                ProjectId = row.ColumnAX,
                                TenCompleteValue = row.ColumnAN,
                                Year = 2023,
                                DateMonth = 202310
                            });
                            excelPlan.Add(new ExcelPlanConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnC,
                                ProjectId = row.ColumnAX,
                                TenPlanProductionValue = row.ColumnAM,
                                Year = 2023,
                                DateMonth = 202310
                            });

                            break;
                        case 10:
                            excelComplete.Add(new ExcelProductionConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnC,
                                ProjectId = row.ColumnAX,
                                ElevenCompleteValue = row.ColumnAQ,
                                Year = 2023,
                                DateMonth = 202311
                            });
                            excelPlan.Add(new ExcelPlanConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnC,
                                ProjectId = row.ColumnAX,
                                ElevenPlanProductionValue = row.ColumnAP,
                                Year = 2023,
                                DateMonth = 202311
                            });
                            break;
                        case 11:
                            excelComplete.Add(new ExcelProductionConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnC,
                                ProjectId = row.ColumnAX,
                                TwelveCompleteValue = row.ColumnAT,
                                Year = 2023,
                                DateMonth = 202312
                            });
                            excelPlan.Add(new ExcelPlanConvertTable
                            {
                                Id = GuidUtil.Next(),
                                Name = row.ColumnC,
                                ProjectId = row.ColumnAX,
                                TwelvePlanProductionValue = row.ColumnAS,
                                Year = 2023,
                                DateMonth = 202312
                            });
                            break;
                    }
                }

            }
            #endregion

            #region 2022年12月份在建项目产值完成情况与计划对比分析表                                      
            var rows5 = MiniExcel
                        .Query(filePath5)                      // 读取 Excel 文件
                        .Skip(10)                               // 跳过前10行，从第11行开始
                        .Select(row => new
                        {
                            ColumnC = row.C,   // C 列
                            ColumnAP = row.AP,  // AP 列
                            ColumnL = row.L,
                            ColumnM = row.M,
                            ColumnN = row.N,
                            ColumnO = row.O,
                            ColumnP = row.P,
                            ColumnQ = row.Q,
                            ColumnR = row.R,
                            ColumnS = row.S,
                            ColumnT = row.T,
                            ColumnU = row.U,
                            ColumnW = row.W,
                            ColumnX = row.X,
                            ColumnY = row.Y,
                            ColumnZ = row.Z,
                            ColumnAA = row.AA,
                            ColumnAB = row.AB,
                            ColumnAC = row.AC,
                            ColumnAD = row.AD,
                            ColumnAE = row.AE,
                            ColumnAF = row.AF,
                            ColumnAG = row.AG,
                            ColumnAH = row.AH,
                            ColumnAJ = row.AJ,
                            ColumnAK = row.AK
                        })
                        .ToList();
            rows5 = rows5.Where(x => !string.IsNullOrEmpty(x.ColumnAP)).ToList();


            foreach (var row in rows5)
            {
                for (int i = 0; i < 12; i++)
                {
                    try
                    {

                        switch (i)
                        {
                            case 0:
                                excelComplete.Add(new ExcelProductionConvertTable
                                {
                                    Id = GuidUtil.Next(),
                                    Name = row.ColumnC,
                                    ProjectId = row.ColumnAP,
                                    OneCompleteValue = Convert.ToDecimal(row.ColumnM),
                                    Year = 2022,
                                    DateMonth = 202201
                                });
                                excelPlan.Add(new ExcelPlanConvertTable
                                {
                                    Id = GuidUtil.Next(),
                                    Name = row.ColumnC,
                                    ProjectId = row.ColumnAP,
                                    OnePlanProductionValue = Convert.ToDecimal(row.ColumnL),
                                    Year = 2022,
                                    DateMonth = 202201
                                });
                                break;
                            case 1:
                                excelComplete.Add(new ExcelProductionConvertTable
                                {
                                    Id = GuidUtil.Next(),
                                    Name = row.ColumnC,
                                    ProjectId = row.ColumnAP,
                                    TwoCompleteValue = Convert.ToDecimal(row.ColumnO),
                                    Year = 2022,
                                    DateMonth = 202202
                                });
                                excelPlan.Add(new ExcelPlanConvertTable
                                {
                                    Id = GuidUtil.Next(),
                                    Name = row.ColumnC,
                                    ProjectId = row.ColumnAP,
                                    TwoPlanProductionValue = Convert.ToDecimal(row.ColumnN),
                                    Year = 2022,
                                    DateMonth = 202202
                                });
                                break;
                            case 2:
                                excelComplete.Add(new ExcelProductionConvertTable
                                {
                                    Id = GuidUtil.Next(),
                                    Name = row.ColumnC,
                                    ProjectId = row.ColumnAP,
                                    ThreeCompleteValue = Convert.ToDecimal(row.ColumnQ),
                                    Year = 2022,
                                    DateMonth = 202203
                                });
                                excelPlan.Add(new ExcelPlanConvertTable
                                {
                                    Id = GuidUtil.Next(),
                                    Name = row.ColumnC,
                                    ProjectId = row.ColumnAP,
                                    ThreePlanProductionValue = Convert.ToDecimal(row.ColumnP),
                                    Year = 2022,
                                    DateMonth = 202203
                                });
                                break;
                            case 3:
                                excelComplete.Add(new ExcelProductionConvertTable
                                {
                                    Id = GuidUtil.Next(),
                                    Name = row.ColumnC,
                                    ProjectId = row.ColumnAP,
                                    FourCompleteValue = Convert.ToDecimal(row.ColumnS),
                                    Year = 2022,
                                    DateMonth = 202204
                                });
                                excelPlan.Add(new ExcelPlanConvertTable
                                {
                                    Id = GuidUtil.Next(),
                                    Name = row.ColumnC,
                                    ProjectId = row.ColumnAP,
                                    FourPlanProductionValue = Convert.ToDecimal(row.ColumnR),
                                    Year = 2022,
                                    DateMonth = 202204
                                });
                                break;
                            case 4:
                                excelComplete.Add(new ExcelProductionConvertTable
                                {
                                    Id = GuidUtil.Next(),
                                    Name = row.ColumnC,
                                    ProjectId = row.ColumnAP,
                                    FiveCompleteValue = Convert.ToDecimal(row.ColumnU),
                                    Year = 2022,
                                    DateMonth = 202205
                                });
                                excelPlan.Add(new ExcelPlanConvertTable
                                {
                                    Id = GuidUtil.Next(),
                                    Name = row.ColumnC,
                                    ProjectId = row.ColumnAP,
                                    FivePlanProductionValue = Convert.ToDecimal(row.ColumnT),
                                    Year = 2022,
                                    DateMonth = 202205
                                });
                                break;
                            case 5:
                                excelComplete.Add(new ExcelProductionConvertTable
                                {
                                    Id = GuidUtil.Next(),
                                    Name = row.ColumnC,
                                    ProjectId = row.ColumnAP,
                                    SixCompleteValue = Convert.ToDecimal(row.ColumnX),
                                    Year = 2022,
                                    DateMonth = 202206
                                });
                                excelPlan.Add(new ExcelPlanConvertTable
                                {
                                    Id = GuidUtil.Next(),
                                    Name = row.ColumnC,
                                    ProjectId = row.ColumnAP,
                                    SixPlanProductionValue = Convert.ToDecimal(row.ColumnW),
                                    Year = 2022,
                                    DateMonth = 202206
                                });
                                break;
                            case 6:
                                excelComplete.Add(new ExcelProductionConvertTable
                                {
                                    Id = GuidUtil.Next(),
                                    Name = row.ColumnC,
                                    ProjectId = row.ColumnAP,
                                    SevenCompleteValue = Convert.ToDecimal(row.ColumnZ),
                                    Year = 2022,
                                    DateMonth = 202207
                                });
                                excelPlan.Add(new ExcelPlanConvertTable
                                {
                                    Id = GuidUtil.Next(),
                                    Name = row.ColumnC,
                                    ProjectId = row.ColumnAP,
                                    SevenPlanProductionValue = Convert.ToDecimal(row.ColumnY),
                                    Year = 2022,
                                    DateMonth = 202207
                                });
                                break;
                            case 7:
                                excelComplete.Add(new ExcelProductionConvertTable
                                {
                                    Id = GuidUtil.Next(),
                                    Name = row.ColumnC,
                                    ProjectId = row.ColumnAP,
                                    EightCompleteValue = Convert.ToDecimal(row.ColumnAB),
                                    Year = 2022,
                                    DateMonth = 202208
                                });
                                excelPlan.Add(new ExcelPlanConvertTable
                                {
                                    Id = GuidUtil.Next(),
                                    Name = row.ColumnC,
                                    ProjectId = row.ColumnAP,
                                    EightPlanProductionValue = Convert.ToDecimal(row.ColumnAA),
                                    Year = 2022,
                                    DateMonth = 202208
                                });
                                break;
                            case 8:
                                excelComplete.Add(new ExcelProductionConvertTable
                                {
                                    Id = GuidUtil.Next(),
                                    Name = row.ColumnC,
                                    ProjectId = row.ColumnAP,
                                    NineCompleteValue = Convert.ToDecimal(row.ColumnAD),
                                    Year = 2022,
                                    DateMonth = 202209
                                });
                                excelPlan.Add(new ExcelPlanConvertTable
                                {
                                    Id = GuidUtil.Next(),
                                    Name = row.ColumnC,
                                    ProjectId = row.ColumnAP,
                                    NinePlanProductionValue = Convert.ToDecimal(row.ColumnAC),
                                    Year = 2022,
                                    DateMonth = 202209
                                });
                                break;
                            case 9:
                                excelComplete.Add(new ExcelProductionConvertTable
                                {
                                    Id = GuidUtil.Next(),
                                    Name = row.ColumnC,
                                    ProjectId = row.ColumnAP,
                                    TenCompleteValue = Convert.ToDecimal(row.ColumnAF),
                                    Year = 2022,
                                    DateMonth = 202210
                                });
                                excelPlan.Add(new ExcelPlanConvertTable
                                {
                                    Id = GuidUtil.Next(),
                                    Name = row.ColumnC,
                                    ProjectId = row.ColumnAP,
                                    TenPlanProductionValue = Convert.ToDecimal(row.ColumnAE),
                                    Year = 2022,
                                    DateMonth = 202210
                                });
                                break;
                            case 10:
                                excelComplete.Add(new ExcelProductionConvertTable
                                {
                                    Id = GuidUtil.Next(),
                                    Name = row.ColumnC,
                                    ProjectId = row.ColumnAP,
                                    ElevenCompleteValue = Convert.ToDecimal(row.ColumnAH),
                                    Year = 2022,
                                    DateMonth = 202211
                                });
                                excelPlan.Add(new ExcelPlanConvertTable
                                {
                                    Id = GuidUtil.Next(),
                                    Name = row.ColumnC,
                                    ProjectId = row.ColumnAP,
                                    ElevenPlanProductionValue = Convert.ToDecimal(row.ColumnAG),
                                    Year = 2022,
                                    DateMonth = 202211
                                });
                                break;
                            case 11:
                                excelComplete.Add(new ExcelProductionConvertTable
                                {
                                    Id = GuidUtil.Next(),
                                    Name = row.ColumnC,
                                    ProjectId = row.ColumnAP,
                                    TwelveCompleteValue = Convert.ToDecimal(row.ColumnAK),
                                    Year = 2022,
                                    DateMonth = 202212
                                });
                                excelPlan.Add(new ExcelPlanConvertTable
                                {
                                    Id = GuidUtil.Next(),
                                    Name = row.ColumnC,
                                    ProjectId = row.ColumnAP,
                                    TwelvePlanProductionValue = Convert.ToDecimal(row.ColumnAJ),
                                    Year = 2022,
                                    DateMonth = 202212
                                });
                                break;
                        }

                    }
                    catch (Exception ex)
                    {

                        throw;
                    }
                }
            }
            #endregion
            var complete = excelComplete.ToList();
            var plan = excelPlan.ToList();
            //_db.Insertable(complete).ExecuteCommand();
            //_db.Insertable(plan).ExecuteCommand();


            //var resList = JsonConvert.DeserializeObject<List<bb>>(jsonObject);
            //var ss = resList.Sum(x => x.UnitPrice * x.CompletedQuantity);
            //return projectService.aa();
            return true;
        }

        #endregion
    }
}
