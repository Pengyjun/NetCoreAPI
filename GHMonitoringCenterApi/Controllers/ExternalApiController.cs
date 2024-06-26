﻿using GHMonitoringCenterApi.Application.Contracts.Dto;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project.ShipMovements;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project;
using GHMonitoringCenterApi.Application.Contracts.Dto.ProjectProductionReport;
using GHMonitoringCenterApi.Application.Contracts.IService;
using GHMonitoringCenterApi.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report.MonthtReportsResponseDto;

namespace GHMonitoringCenterApi.Controllers

{
    /// <summary>
    /// 对外接口控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        [AllowAnonymous]//跳过鉴权
        public async Task<ResponseAjaxResult<List<UserInfos>>> GetUserInfosAsync()
            => await _externalApiService.GetUserInfosAsync();
        /// <summary>
        /// 获取机构
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetInstutionInfos")]
        [AllowAnonymous]//跳过鉴权
        public async Task<ResponseAjaxResult<List<InstutionInfos>>> GetInstutionInfosAsync()
            => await _externalApiService.GetInstutionInfosAsync();
        /// <summary>
        /// 获取项目信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetProjectInfos")]
        [AllowAnonymous]//跳过鉴权
        public async Task<ResponseAjaxResult<List<ProjectInfos>>> GetProjectInfosAsync()
            => await _externalApiService.GetProjectInfosAsync();
        /// <summary>
        /// 获取项目区域信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAreaInfos")]
        [AllowAnonymous]//跳过鉴权
        public async Task<ResponseAjaxResult<List<ProjectAreaInfos>>> GetAreaInfosAsync()
            => await _externalApiService.GetAreaInfosAsync();
        /// <summary>
        /// 获取项目干系人
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetProjectLeaderInfos")]
        [AllowAnonymous]//跳过鉴权
        public async Task<ResponseAjaxResult<List<ProjectLeaderInfos>>> GetProjectLeaderInfosAsync()
            => await _externalApiService.GetProjectLeaderInfosAsync();
        /// <summary>
        /// 获取项目信息状态
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetProjectStatusInfos")]
        [AllowAnonymous]//跳过鉴权
        public async Task<ResponseAjaxResult<List<ProjectStatusInfos>>> GetProjectStatusInfosAsync()
            => await _externalApiService.GetProjectStatusInfosAsync();
        /// <summary>
        /// 获取项目类型信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetProjectTypeInfos")]
        [AllowAnonymous]//跳过鉴权
        public async Task<ResponseAjaxResult<List<ProjectTypeInfos>>> GetProjectTypeInfosAsync()
            => await _externalApiService.GetProjectTypeInfosAsync();
        /// <summary>
        /// 获取清单类型
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetListTypes")]
        [AllowAnonymous]//跳过鉴权
        public async Task<ResponseAjaxResult<List<ShipCommResponseDto>>> GetListTypesAsync()
            => await _externalApiService.GetListTypesAsync();
        /// <summary>
        /// 获取工艺方式
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetProcessMethods")]
        [AllowAnonymous]//跳过鉴权
        public async Task<ResponseAjaxResult<List<ShipCommResponseDto>>> GetProcessMethodsAsync()
            => await _externalApiService.GetProcessMethodsAsync();
        /// <summary>
        /// 获取疏浚吹填分类
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetReclamationClassification")]
        [AllowAnonymous]//跳过鉴权
        public async Task<ResponseAjaxResult<List<ShipCommResponseDto>>> GetReclamationClassificationAsync()
            => await _externalApiService.GetReclamationClassificationAsync();
        /// <summary>
        /// 获取工况级别
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetWorkingConditionLevel")]
        [AllowAnonymous]//跳过鉴权
        public async Task<ResponseAjaxResult<List<ShipCommResponseDto>>> GetWorkingConditionLevelAsync()
            => await _externalApiService.GetWorkingConditionLevelAsync();
        /// <summary>
        /// 获取船舶动态
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetShipDynamics")]
        [AllowAnonymous]//跳过鉴权
        public async Task<ResponseAjaxResult<List<ShipCommResponseDto>>> GetShipDynamicAsync()
            => await _externalApiService.GetShipDynamicAsync();
        /// <summary>
        /// 船舶信息
        /// </summary>
        /// <param name="shipType">船舶类型 自有1：分包2</param>
        /// <returns></returns>
        [HttpGet("GetShipInfos")]
        [AllowAnonymous]//跳过鉴权
        public async Task<ResponseAjaxResult<List<ShipInfos>>> GetShipInfosAsync([FromQuery] int shipType)
            => await _externalApiService.GetShipInfosAsync(shipType);
        /// <summary>
        /// 获取船舶日报
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet("GetShipDayReports")]
        [AllowAnonymous]//跳过鉴权
        public async Task<ResponseAjaxResult<List<ShipDayReports>>> GetShipDayReportsAsync([FromQuery] ShipDayReportsRequestDto requestDto)
            => await _externalApiService.GetShipDayReportsAsync(requestDto);
        /// <summary>
        /// 获取自有船舶月报
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet("GetOwnShipMonthReps")]
        [AllowAnonymous]//跳过鉴权
        public async Task<ResponseAjaxResult<List<ShipMonthReports>>> GetSearchOwnShipMonthRepAsync([FromQuery] ShipMonthRequestDto requestDto)
            => await _externalApiService.GetSearchOwnShipMonthRepAsync(requestDto);
        /// <summary>
        /// 获取分包船舶月报
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet("GetSearchSubShipMonthRep")]
        [AllowAnonymous]//跳过鉴权
        public async Task<ResponseAjaxResult<List<SubShipMonthReports>>> GetSearchSubShipMonthRepAsync([FromQuery] ShipMonthRequestDto requestDto)
            => await _externalApiService.GetSearchSubShipMonthRepAsync(requestDto);
        /// <summary>
        /// 获取项目日报
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet("GetSearchDayReport")]
        [AllowAnonymous]//跳过鉴权
        public async Task<ResponseAjaxResult<List<DayReportInfo>>> GetSearchDayReportAsync([FromQuery] DayReportRequestDto requestDto)
            => await _externalApiService.GetSearchDayReportAsync(requestDto);
        /// <summary>
        /// 获取项目月报
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet("GetMonthReportInfos")]
        [AllowAnonymous]//跳过鉴权
        public async Task<ResponseAjaxResult<List<MonthtReportDto>>> GetMonthReportInfosAsync([FromQuery] MonthReportInfosRequestDto requestDto)
            => await _externalApiService.GetMonthReportInfosAsync(requestDto);
        /// <summary>
        /// 获取船舶进退场
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet("GetShipMovement")]
        [AllowAnonymous]//跳过鉴权
        public async Task<ResponseAjaxResult<List<ShipMovementResponseDto>>> GetShipMovementAsync([FromQuery] ShipMovementsRequestDto model)
            => await _externalApiService.GetShipMovementAsync(model);
    }
}
