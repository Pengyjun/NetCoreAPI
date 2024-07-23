using GHMonitoringCenterApi.Application.Contracts.Dto;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project.ShipMovements;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project;
using GHMonitoringCenterApi.Application.Contracts.Dto.ProjectProductionReport;
using GHMonitoringCenterApi.Domain.Shared;
using static GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report.MonthtReportsResponseDto;
using GHMonitoringCenterApi.Application.Contracts.Dto.External;
using Model = GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Application.Contracts.Dto.EquipmentManagement;

namespace GHMonitoringCenterApi.Application.Contracts.IService
{
    /// <summary>
    /// 对外接口
    /// </summary>
    public interface IExternalApiService
    {
        /// <summary>
        /// 获取人员
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<UserInfos>>> GetUserInfosAsync();
        /// <summary>
        /// 获取机构
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<InstutionInfos>>> GetInstutionInfosAsync();
        /// <summary>
        /// 获取项目信息
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<ProjectInfos>>> GetProjectInfosAsync();
        /// <summary>
        /// 获取区域信息
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<ProjectAreaInfos>>> GetAreaInfosAsync();
        /// <summary>
        /// 获取项目干系人
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<ProjectLeaderInfos>>> GetProjectLeaderInfosAsync();
        /// <summary>
        /// 项目状态
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<ProjectStatusInfos>>> GetProjectStatusInfosAsync();
        /// <summary>
        /// 项目类型信息
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<ProjectTypeInfos>>> GetProjectTypeInfosAsync();
        /// <summary>
        /// 获取合同清单类型
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<ShipCommResponseDto>>> GetListTypesAsync();
        /// <summary>
        /// 获取工艺方式数据集
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<ShipCommResponseDto>>> GetProcessMethodsAsync();
        /// <summary>
        /// 获取疏浚吹填分类数据集
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<ShipCommResponseDto>>> GetReclamationClassificationAsync();
        /// <summary>
        /// 获取工况级别数据集
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<ShipCommResponseDto>>> GetWorkingConditionLevelAsync();
        /// <summary>
        /// 获取船舶动态数据集
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<ShipCommResponseDto>>> GetShipDynamicAsync();
        /// <summary>
        /// 船舶信息
        /// </summary>
        /// <param name="shipType">船舶类型 自有1：分包2</param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<ShipInfos>>> GetShipInfosAsync(int shipType);
        /// <summary>
        /// 获取船舶日报
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<ShipDayReports>>> GetShipDayReportsAsync(ShipDayReportsRequestDto requestDto);
        /// <summary>
        /// 获取船舶月报
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<ShipMonthReports>>> GetSearchOwnShipMonthRepAsync(ShipMonthRequestDto requestDto);
        /// <summary>
        /// 获取分包船舶月报
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<SubShipMonthReports>>> GetSearchSubShipMonthRepAsync(ShipMonthRequestDto requestDto);
        /// <summary>
        /// 获取项目日报
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<DayReportInfo>>> GetSearchDayReportAsync(DayReportRequestDto requestDto);
        /// <summary>
        /// 获取项目月报
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<MonthtReportDto>>> GetMonthReportInfosAsync(MonthReportInfosRequestDto requestDto);
        /// <summary>
        /// 船舶进退场
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<ShipMovementResponseDto>>> GetShipMovementAsync(ShipMovementsRequestDto model);
        #region 返回全表字段信息
        /// <summary>
        /// 获取全表字段项目信息
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<Model.Project>>> GetProjectsTableAsync(ExternalRequestDto requestDto);
        /// <summary>
        /// 获取全表字段公司机构
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<Institution>>> GetInstitutionTableAsync(ExternalRequestDto requestDto);
        /// <summary>
        /// 获取全表字段项目类型
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<ProjectType>>> GetProjectTypeTableAsync(ExternalRequestDto requestDto);
        /// <summary>
        /// 获取全表字段项目状态
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<ProjectStatus>>> GetProjectStatusTableAsync(ExternalRequestDto requestDto);
        /// <summary>
        /// 获取全表字段项目规模
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<ProjectScale>>> GetProjectScaleTableAsync(ExternalRequestDto requestDto);
        /// <summary>
        /// 获取全表字段施工地点
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<Province>>> GetProjectProvinceTableAsync(ExternalRequestDto requestDto);
        /// <summary>
        /// 获取全表字段施工区域
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<ProjectArea>>> GetProjectAreaTableAsync(ExternalRequestDto requestDto);
        /// <summary>
        /// 获取全表字段自有船舶
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<OwnerShip>>> GetOwnerShipTableAsync(ExternalRequestDto requestDto);
        /// <summary>
        /// 获取全表字段分包船舶
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<SubShip>>> GetSubShipTableAsync(ExternalRequestDto requestDto);
        /// <summary>
        /// 获取全表字段船级社
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<ShipClassic>>> GetShipClassicTableAsync(ExternalRequestDto requestDto);
        /// <summary>
        /// 获取全表字段船舶类型
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<ShipPingType>>> GetShipPingTypeTableAsync(ExternalRequestDto requestDto);
        /// <summary>
        /// 获取全表字段船舶状态
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<ShipStatus>>> GetShipStatusTableAsync(ExternalRequestDto requestDto);
        /// <summary>
        /// 获取全表字段船舶进退场
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<ShipMovement>>> GetShipMovementTableAsync(ExternalRequestDto requestDto);
        /// <summary>
        /// 获取全表字段项目产值计划
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<ProjectPlanProduction>>> GetProjectPlanProductionAsync();
        /// <summary>
        /// 获取全表字段水上设备
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<SearchEquipmentManagementResponseDto>>> GetSearchEquipmentManagementAsync(ExternalRequestDto requestDto);
        #endregion
    }
}
