using GHMonitoringCenterApi.Application.Contracts.Dto;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project.Approver;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project.ShipMovements;
using GHMonitoringCenterApi.Application.Contracts.Dto.ProjectDepartMent;
using GHMonitoringCenterApi.Application.Contracts.Dto.ProjectPlanProduction;
using GHMonitoringCenterApi.Application.Contracts.Dto.ProjectProductionReport;
using GHMonitoringCenterApi.Application.Contracts.Dto.Ship;
using GHMonitoringCenterApi.Application.Contracts.Dto.Upload;
using GHMonitoringCenterApi.Application.Contracts.IService.Project;
using GHMonitoringCenterApi.CustomAttribute;
using GHMonitoringCenterApi.Domain.Shared;
using GHMonitoringCenterApi.Domain.Shared.Const;
using GHMonitoringCenterApi.Domain.Shared.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public IProjectService projectService { get; set; }

        public IProjectReportService projectReportService { get; set; }
        public IProjectDepartMentService projectDepartMentService { get; set; }

        public IProjectShipMovementsService projectShipMovementsService { get; set; }

        public IProjectApproverService projectApproverService { get; set; }


        public ProjectController(IProjectService projectService, IProjectReportService projectReportService, IProjectDepartMentService projectDepartMentService, IProjectShipMovementsService projectShipMovementsService, IProjectApproverService projectApproverService)
        {
            this.projectService = projectService;
            this.projectReportService = projectReportService;
            this.projectDepartMentService = projectDepartMentService;
            this.projectShipMovementsService = projectShipMovementsService;
            this.projectApproverService = projectApproverService;
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
        public async Task<ResponseAjaxResult<ProjectMonthReportResponseDto>> SearchProjectMonthReportAsync([FromQuery] ProjectMonthReportRequestDto model)
        {
            return await projectReportService.SearchProjectMonthReportAsync(model);
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
        public async Task<ResponseAjaxResult<List<CompanyProjectDetailedResponseDto>>> SearchCompanyProjectListAsync()
        {
            return await projectService.SearchCompanyProjectListAsync();
        }
    }
}
