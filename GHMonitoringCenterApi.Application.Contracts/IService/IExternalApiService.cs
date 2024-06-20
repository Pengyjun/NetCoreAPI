using GHMonitoringCenterApi.Application.Contracts.Dto;
using GHMonitoringCenterApi.Application.Contracts.Dto.ProjectProductionReport;
using GHMonitoringCenterApi.Domain.Shared;
using static GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report.MonthtReportsResponseDto;

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
    }
}
