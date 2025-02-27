using GHMonitoringCenterApi.Application.Contracts.Dto;
using GHMonitoringCenterApi.Application.Contracts.Dto.ConstructionLog;
using GHMonitoringCenterApi.Application.Contracts.Dto.EquipmentManagement;
using GHMonitoringCenterApi.Application.Contracts.Dto.External;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project.ShipMovements;
using GHMonitoringCenterApi.Application.Contracts.Dto.ProjectProductionReport;
using GHMonitoringCenterApi.Application.Contracts.Dto.Word;
using GHMonitoringCenterApi.Application.Contracts.IService;
using GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Domain.Shared;
using GHMonitoringCenterApi.Filters;
using HNKC.OperationLogsAPI.Dto.ResponseDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report.MonthtReportsResponseDto;
using Model = GHMonitoringCenterApi.Domain.Models;

namespace GHMonitoringCenterApi.Controllers

{
    /// <summary>
    /// 对外接口控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(CustomFixedAuthAttribute))]
    public class ExternalApiController : BaseController
    {
        /// <summary>
        /// 接口注入
        /// </summary>
        public readonly IExternalApiService _externalApiService;
        /// <summary>
        /// 依赖注入
        /// </summary>
        public ExternalApiController(IExternalApiService externalApiService)
        {
            this._externalApiService = externalApiService;
        }
        /// <summary>
        /// 获取人员
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetUserInfos")]

        public async Task<ResponseAjaxResult<List<UserInfos>>> GetUserInfosAsync()
            => await _externalApiService.GetUserInfosAsync();
        /// <summary>
        /// 获取机构
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetInstutionInfos")]

        public async Task<ResponseAjaxResult<List<InstutionInfos>>> GetInstutionInfosAsync()
            => await _externalApiService.GetInstutionInfosAsync();
        /// <summary>
        /// 获取项目信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetProjectInfos")]

        public async Task<ResponseAjaxResult<List<ProjectInfos>>> GetProjectInfosAsync()
            => await _externalApiService.GetProjectInfosAsync();
        /// <summary>
        /// 获取项目区域信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAreaInfos")]

        public async Task<ResponseAjaxResult<List<ProjectAreaInfos>>> GetAreaInfosAsync()
            => await _externalApiService.GetAreaInfosAsync();
        /// <summary>
        /// 获取项目干系人
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetProjectLeaderInfos")]

        public async Task<ResponseAjaxResult<List<ProjectLeaderInfos>>> GetProjectLeaderInfosAsync()
            => await _externalApiService.GetProjectLeaderInfosAsync();
        /// <summary>
        /// 获取项目干系单位
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetProjectOrgInfos")]

        public async Task<ResponseAjaxResult<List<ProjectOrg>>> GetProjectOrgInfosAsync()
            => await _externalApiService.GetProjectOrgInfosAsync();
        /// <summary>
        /// 获取往来单位
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetDealingUnit")]

        public async Task<ResponseAjaxResult<List<DealingUnit>>> GetDealingUnitAsync([FromQuery] int pageIndex, [FromQuery] int pageSize)
            => await _externalApiService.GetDealingUnitAsync(pageIndex, pageSize);
        /// <summary>
        /// 获取项目信息状态
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetProjectStatusInfos")]

        public async Task<ResponseAjaxResult<List<ProjectStatusInfos>>> GetProjectStatusInfosAsync()
            => await _externalApiService.GetProjectStatusInfosAsync();
        /// <summary>
        /// 获取项目类型信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetProjectTypeInfos")]

        public async Task<ResponseAjaxResult<List<ProjectTypeInfos>>> GetProjectTypeInfosAsync()
            => await _externalApiService.GetProjectTypeInfosAsync();
        /// <summary>
        /// 获取清单类型
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetListTypes")]

        public async Task<ResponseAjaxResult<List<ShipCommResponseDto>>> GetListTypesAsync()
            => await _externalApiService.GetListTypesAsync();
        /// <summary>
        /// 获取工艺方式
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetProcessMethods")]

        public async Task<ResponseAjaxResult<List<ShipCommResponseDto>>> GetProcessMethodsAsync()
            => await _externalApiService.GetProcessMethodsAsync();
        /// <summary>
        /// 获取疏浚吹填分类
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetReclamationClassification")]

        public async Task<ResponseAjaxResult<List<ShipCommResponseDto>>> GetReclamationClassificationAsync()
            => await _externalApiService.GetReclamationClassificationAsync();
        /// <summary>
        /// 获取工况级别
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetWorkingConditionLevel")]

        public async Task<ResponseAjaxResult<List<ShipCommResponseDto>>> GetWorkingConditionLevelAsync()
            => await _externalApiService.GetWorkingConditionLevelAsync();
        /// <summary>
        /// 获取船舶动态
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetShipDynamics")]

        public async Task<ResponseAjaxResult<List<ShipCommResponseDto>>> GetShipDynamicAsync()
            => await _externalApiService.GetShipDynamicAsync();
        /// <summary>
        /// 船舶信息
        /// </summary>
        /// <param name="shipType">船舶类型 自有1：分包2</param>
        /// <returns></returns>
        [HttpGet("GetShipInfos")]

        public async Task<ResponseAjaxResult<List<ShipInfos>>> GetShipInfosAsync([FromQuery] int shipType)
            => await _externalApiService.GetShipInfosAsync(shipType);
        /// <summary>
        /// 获取船舶日报
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet("GetShipDayReports")]

        public async Task<ResponseAjaxResult<List<ShipDayReports>>> GetShipDayReportsAsync([FromQuery] ShipDayReportsRequestDto requestDto)
            => await _externalApiService.GetShipDayReportsAsync(requestDto);
        /// <summary>
        /// 获取自有船舶月报
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet("GetOwnShipMonthReps")]

        public async Task<ResponseAjaxResult<List<ShipMonthReports>>> GetSearchOwnShipMonthRepAsync([FromQuery] ShipMonthRequestDto requestDto)
            => await _externalApiService.GetSearchOwnShipMonthRepAsync(requestDto);
        /// <summary>
        /// 获取分包船舶月报
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet("GetSearchSubShipMonthRep")]

        public async Task<ResponseAjaxResult<List<SubShipMonthReports>>> GetSearchSubShipMonthRepAsync([FromQuery] ShipMonthRequestDto requestDto)
            => await _externalApiService.GetSearchSubShipMonthRepAsync(requestDto);
        /// <summary>
        /// 获取项目日报
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet("GetSearchDayReport")]
        public async Task<ResponseAjaxResult<List<DayReportInfo>>> GetSearchDayReportAsync([FromQuery] DayReportRequestDto requestDto)
            => await _externalApiService.GetSearchDayReportAsync(requestDto);

        /// <summary>
        /// 获取项目日报(包含施工日志数据)
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet("GetSearchDayReportConstruct")]
        public async Task<ResponseAjaxResult<List<DayReportInfo>>> GetSearchDayReportConstructAsync([FromQuery] DayReportRequestDto requestDto)
            => await _externalApiService.GetSearchDayReportConstructAsync(requestDto);
        /// <summary>
        /// 获取项目月报
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet("GetMonthReportInfos")]
        public async Task<ResponseAjaxResult<List<MonthtReportDto>>> GetMonthReportInfosAsync([FromQuery] MonthReportInfosRequestDto requestDto)
            => await _externalApiService.GetMonthReportInfosAsync(requestDto);
        /// <summary>
        /// 获取船舶进退场
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet("GetShipMovement")]

        public async Task<ResponseAjaxResult<List<ShipMovementResponseDto>>> GetShipMovementAsync([FromQuery] ShipMovementsRequestDto model)
            => await _externalApiService.GetShipMovementAsync(model);

        #region 返全表信息
        /// <summary>
        /// 获取项目信息全量字段表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet("GetProjectsTable")]

        public async Task<ResponseAjaxResult<List<Model.Project>>> GetProjectsTableAsync([FromQuery] ExternalRequestDto requestDto)
            => await _externalApiService.GetProjectsTableAsync(requestDto);
        /// <summary>
        /// 获取公司机构全量字段表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet("GetInstitutionTable")]

        public async Task<ResponseAjaxResult<List<Institution>>> GetInstitutionTableAsync([FromQuery] ExternalRequestDto requestDto)
            => await _externalApiService.GetInstitutionTableAsync(requestDto);
        /// <summary>
        /// 获取项目类型全量字段表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet("GetProjectTypeTable")]

        public async Task<ResponseAjaxResult<List<ProjectType>>> GetProjectTypeTableAsync([FromQuery] ExternalRequestDto requestDto)
            => await _externalApiService.GetProjectTypeTableAsync(requestDto);
        /// <summary>
        /// 获取项目状态全量字段表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet("GetProjectStatusTable")]

        public async Task<ResponseAjaxResult<List<ProjectStatus>>> GetProjectStatusTableAsync([FromQuery] ExternalRequestDto requestDto)
            => await _externalApiService.GetProjectStatusTableAsync(requestDto);
        /// <summary>
        /// 获取项目规模全量字段表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet("GetProjectScaleTable")]

        public async Task<ResponseAjaxResult<List<ProjectScale>>> GetProjectScaleTableAsync([FromQuery] ExternalRequestDto requestDto)
            => await _externalApiService.GetProjectScaleTableAsync(requestDto);
        /// <summary>
        /// 获取施工地点全量字段表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet("GetProjectProvinceTable")]

        public async Task<ResponseAjaxResult<List<Province>>> GetProjectProvinceTableAsync([FromQuery] ExternalRequestDto requestDto)
            => await _externalApiService.GetProjectProvinceTableAsync(requestDto);
        /// <summary>
        /// 获取项目区域全量字段表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet("GetProjectAreaTable")]

        public async Task<ResponseAjaxResult<List<ProjectArea>>> GetProjectAreaTableAsync([FromQuery] ExternalRequestDto requestDto)
            => await _externalApiService.GetProjectAreaTableAsync(requestDto);
        /// <summary>
        /// 获取自有船舶全量字段表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet("GetOwnerShipTable")]

        public async Task<ResponseAjaxResult<List<OwnerShip>>> GetOwnerShipTableAsync([FromQuery] ExternalRequestDto requestDto)
            => await _externalApiService.GetOwnerShipTableAsync(requestDto);
        /// <summary>
        /// 获取分包船舶全量字段表
        /// </summary>
        [HttpGet("GetSubShipTable")]

        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<SubShip>>> GetSubShipTableAsync([FromQuery] ExternalRequestDto requestDto)
            => await _externalApiService.GetSubShipTableAsync(requestDto);
        /// <summary>
        /// 获取船级社全量字段表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet("GetShipClassicTable")]

        public async Task<ResponseAjaxResult<List<ShipClassic>>> GetShipClassicTableAsync([FromQuery] ExternalRequestDto requestDto)
            => await _externalApiService.GetShipClassicTableAsync(requestDto);
        /// <summary>
        /// 获取船舶类型全量字段表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet("GetShipPingTypeTable")]

        public async Task<ResponseAjaxResult<List<ShipPingType>>> GetShipPingTypeTableAsync([FromQuery] ExternalRequestDto requestDto)
            => await _externalApiService.GetShipPingTypeTableAsync(requestDto);
        /// <summary>
        /// 获取船舶状态全量字段表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet("GetShipStatusTable")]

        public async Task<ResponseAjaxResult<List<ShipStatus>>> GetShipStatusTableAsync([FromQuery] ExternalRequestDto requestDto)
            => await _externalApiService.GetShipStatusTableAsync(requestDto);
        /// <summary>
        /// 获取船舶进退场全量字段表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet("GetShipMovementTable")]

        public async Task<ResponseAjaxResult<List<ShipMovement>>> GetShipMovementTableAsync([FromQuery] ExternalRequestDto requestDto)
            => await _externalApiService.GetShipMovementTableAsync(requestDto);
        /// <summary>
        /// 获取全表字段项目产值计划
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetProjectPlanProduction")]

        public async Task<ResponseAjaxResult<List<ProjectPlanProduction>>> GetProjectPlanProductionAsync()
            => await _externalApiService.GetProjectPlanProductionAsync();
        /// <summary>
        /// 获取全表字段水上设备
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet("GetSearchEquipmentManagement")]

        public async Task<ResponseAjaxResult<List<SearchEquipmentManagementResponseDto>>> GetSearchEquipmentManagementAsync([FromQuery] ExternalRequestDto requestDto)
            => await _externalApiService.GetSearchEquipmentManagementAsync(requestDto);


        [HttpGet("SearchProjectChangeList")]

        public async Task<ResponseAjaxResult<List<ProjectStatusChangResponse>>> SearchProjectChangeList()
           => await _externalApiService.SearchProjectChangeList();
        /// <summary>
        /// 危大工程
        /// </summary>
        /// <returns></returns>
        [HttpGet("DangerousDetails")]

        public async Task<ResponseAjaxResult<List<DangerousDetails>>> DangerousDetailsAsync()
          => await _externalApiService.DangerousDetailsAsync();
        /// <summary>
        /// 获取全表字段施工日志
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet("GetDayReportConstruction")]

        public async Task<ResponseAjaxResult<List<DayReportConstruction>>> GetDayReportConstructionAsync([FromQuery] ExternalRequestDto requestDto)
          => await _externalApiService.GetDayReportConstructionAsync(requestDto);

        /// <summary>
        /// 获取施工日志列表 对外提供
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchExternalConstructionLog")]

        public async Task<ResponseAjaxResult<List<ConstructionLogResponseDto>>> SearchExternalConstructionLogAsync([FromQuery] ConstructionLogRequestDto constructionLogRequestDto)
            => await _externalApiService.SearchExternalConstructionLogAsync(constructionLogRequestDto);

        /// <summary>
        /// 获取施工日志详情
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet("GetDayReportConstructionDetails")]

        public async Task<ResponseAjaxResult<List<SearchConstructionLoDetailsgResponseDto>>> GetDayReportConstructionDetailAsync([FromQuery] ExternalDateRequestDto requestDto)
            => await _externalApiService.GetDayReportConstructionDetailAsync(requestDto);
        #endregion


        /// <summary>
        /// 生产运营监控系统使用接口  获取数据同步问题  
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchCompanyProductionValue")]
        public async Task<CompanyDayProductionValueResponseDto> SearchCompanyProductionValueAsync([FromQuery] BaseExternalRequestDto baseExternalRequestDto)
        {
            return await _externalApiService.SearchCompanyProductionValueAsync(baseExternalRequestDto);
        }
    }
}
