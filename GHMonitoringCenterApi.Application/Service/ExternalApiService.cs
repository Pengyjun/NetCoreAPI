using GHMonitoringCenterApi.Application.Contracts.Dto;
using GHMonitoringCenterApi.Application.Contracts.Dto.ConstructionProjectDaily;
using GHMonitoringCenterApi.Application.Contracts.Dto.EquipmentManagement;
using GHMonitoringCenterApi.Application.Contracts.Dto.External;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project.ShipMovements;
using GHMonitoringCenterApi.Application.Contracts.Dto.ProjectProductionReport;
using GHMonitoringCenterApi.Application.Contracts.Dto.Ship;
using GHMonitoringCenterApi.Application.Contracts.Dto.Word;
using GHMonitoringCenterApi.Application.Contracts.IService;
using GHMonitoringCenterApi.Application.Contracts.IService.EquipmentManagement;
using GHMonitoringCenterApi.Application.Contracts.IService.Project;
using GHMonitoringCenterApi.Application.Contracts.IService.ProjectProductionReport;
using GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Domain.Shared;
using GHMonitoringCenterApi.Domain.Shared.Const;
using HNKC.OperationLogsAPI.Dto.ResponseDto;
using Microsoft.AspNetCore.Mvc;
using SkiaSharp;
using SqlSugar;
using SqlSugar.Extensions;
using UtilsSharp;
using static GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report.MonthtReportsResponseDto;

namespace GHMonitoringCenterApi.Application.Service
{
    /// <summary>
    /// 对外接口实现层
    /// </summary>
    public class ExternalApiService : IExternalApiService
    {
        /// <summary>
        /// 上下文注入
        /// </summary>
        private readonly ISqlSugarClient _dbContext;
        /// <summary>
        /// 全局对象
        /// </summary>
        private readonly GlobalObject _globalObject;
        /// <summary>
        /// 当前登录用户
        /// </summary>
        private CurrentUser _currentUser { get { return _globalObject.CurrentUser; } set { } }
        /// <summary>
        /// 项目船舶日报相关接口注入
        /// 项目日报相关接口注入
        /// </summary>
        private IProjectProductionReportService _projectProductionReportService { get; set; }
        /// <summary>
        /// 船舶进退场接口注入
        /// </summary>
        private IProjectShipMovementsService _shipMovementService { get; set; }
        /// <summary>
        /// 项目船舶月报接口注入
        /// 项目月报接口注入
        /// </summary>
        private IProjectReportService _projectReportService { get; set; }
        /// <summary>
        /// 注入设备管理层
        /// </summary>
        private IEquipmentManagementService _eqipment { get; set; }
        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="sqlSugarClient"></param>
        /// <param name="projectProductionReportService"></param>
        /// <param name="projectReportService"></param>
        /// <param name="globalObject"></param>
        /// <param name="shipMovementService"></param>
        public ExternalApiService(ISqlSugarClient sqlSugarClient, IProjectProductionReportService projectProductionReportService, IProjectReportService projectReportService, GlobalObject globalObject, IProjectShipMovementsService shipMovementService, IEquipmentManagementService eqipment)
        {
            this._dbContext = sqlSugarClient;
            this._projectProductionReportService = projectProductionReportService;
            this._projectReportService = projectReportService;
            this._globalObject = globalObject;
            this._shipMovementService = shipMovementService;
            this._eqipment = eqipment;
        }
        /// <summary>
        /// s获取人员信息
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<UserInfos>>> GetUserInfosAsync()
        {
            var responseAjaxResult = new ResponseAjaxResult<List<UserInfos>>();
            var data = await _dbContext.Queryable<Domain.Models.User>()
                .Where(x => x.IsDelete == 1)
                .Select(x => new UserInfos
                {
                    Id = x.Id,
                    CompanyId = x.CompanyId,
                    DepartmentId = x.DepartmentId,
                    GroupCode = x.GroupCode,
                    IdentityCard = x.IdentityCard,
                    LoginAccount = x.LoginAccount,
                    LoginName = x.LoginName,
                    Name = x.Name,
                    Number = x.Number,
                    Phone = x.Phone,
                    PomId = x.PomId
                })
                .ToListAsync();

            responseAjaxResult.Count = data.Count;
            responseAjaxResult.SuccessResult(data, ResponseMessage.OPERATION_SUCCESS);
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取机构信息
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<InstutionInfos>>> GetInstutionInfosAsync()
        {
            var responseAjaxResult = new ResponseAjaxResult<List<InstutionInfos>>();
            var data = await _dbContext.Queryable<Domain.Models.Institution>()
                .Where(x => x.IsDelete == 1)
                .Select(x => new InstutionInfos
                {
                    Gpoid = x.Gpoid,
                    Grade = x.Grade,
                    Grule = x.Grule,
                    Id = x.Id,
                    Name = x.Name,
                    Ocode = x.Ocode,
                    Oid = x.Oid,
                    Orule = x.Orule,
                    Poid = x.Poid,
                    PomId = x.PomId,
                    Shortname = x.Shortname,
                    Sno = x.Sno,
                    Status = x.Status

                })
                .ToListAsync();

            responseAjaxResult.Count = data.Count;
            responseAjaxResult.SuccessResult(data, ResponseMessage.OPERATION_SUCCESS);
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取项目信息
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<ProjectInfos>>> GetProjectInfosAsync()
        {
            var responseAjaxResult = new ResponseAjaxResult<List<ProjectInfos>>();
            var data = await _dbContext.Queryable<Domain.Models.Project>()
                .Where(x => x.IsDelete == 1)
                .Select(x => new ProjectInfos
                {
                    Administrator = x.Administrator,
                    Amount = x.Amount,
                    BidWinningDate = x.BidWinningDate,
                    BudgetaryReasons = x.BudgetaryReasons,
                    BudgetInterestRate = x.BudgetInterestRate,
                    Category = x.Category,
                    ClassifyStandard = x.ClassifyStandard,
                    Code = x.MasterCode,
                    CommencementTime = x.CommencementTime,
                    CompanyId = x.CompanyId,
                    CompilationTime = x.CompilationTime,
                    CompleteOutput = x.CompleteOutput,
                    CompleteQuantity = x.CompleteQuantity,
                    CompletionDate = x.CompletionDate,
                    CompletionTime = x.CompletionTime,
                    Constructor = x.Constructor,
                    ContractChangeInfo = x.ContractChangeInfo,
                    ContractMeaPayProp = x.ContractMeaPayProp,
                    DurationInformation = x.DurationInformation,
                    ECAmount = x.ECAmount,
                    EndContractDuration = x.EndContractDuration,
                    Id = x.Id,
                    IsMajor = x.IsMajor,
                    IsStrength = x.IsStrength,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    Name = x.Name,
                    PomId = x.PomId,
                    ProjectDept = x.ProjectDept,
                    ProjectDeptAddress = x.ProjectDeptAddress,
                    ProjectLocation = x.ProjectLocation,
                    Quantity = x.Quantity,
                    QuantityRemarks = x.QuantityRemarks,
                    ReclamationArea = x.ReclamationArea,
                    Remarks = x.Remarks,
                    ReportFormer = x.ReportFormer,
                    ReportForMertel = x.ReportForMertel,
                    ShortName = x.ShortName,
                    ShutdownDate = x.ShutdownDate,
                    ShutDownReason = x.ShutDownReason,
                    SocietySpecEffect = x.SocietySpecEffect,
                    StartContractDuration = x.StartContractDuration,
                    Tag = x.Tag,
                    Tag2 = x.Tag2,
                    ProjectTypeId = x.TypeId,
                    RegionId = x.RegionId,
                    MasterProjectId = x.MasterProjectId
                })
                .ToListAsync();

            responseAjaxResult.Count = data.Count;
            responseAjaxResult.SuccessResult(data, ResponseMessage.OPERATION_SUCCESS);
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取区域信息
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<ProjectAreaInfos>>> GetAreaInfosAsync()
        {
            var responseAjaxResult = new ResponseAjaxResult<List<ProjectAreaInfos>>();
            var resList = await _dbContext.Queryable<ProjectArea>()
                .Where(x => x.IsDelete == 1)
                .Select(x => new ProjectAreaInfos
                {
                    Id = x.AreaId,
                    Name = x.Name,
                    Code = x.Code,
                    CreateTime = x.CreateTime
                })
            .ToListAsync();

            responseAjaxResult.Count = resList.Count;
            responseAjaxResult.SuccessResult(resList, ResponseMessage.OPERATION_SUCCESS);
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取项目干系人列表
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<ProjectLeaderInfos>>> GetProjectLeaderInfosAsync()
        {
            var responseAjaxResult = new ResponseAjaxResult<List<ProjectLeaderInfos>>();
            var data = await _dbContext.Queryable<Domain.Models.ProjectLeader>()
                .Where(x => x.IsDelete == 1)
                .Select(x => new ProjectLeaderInfos
                {
                    AssistantManagerId = x.AssistantManagerId,
                    BeginDate = x.BeginDate,
                    EndDate = x.EndDate,
                    Id = x.Id,
                    IsPresent = x.IsPresent,
                    PomId = x.PomId,
                    ProjectId = x.ProjectId,
                    Remarks = x.Remarks,
                    Type = x.Type
                })
                .ToListAsync();

            responseAjaxResult.Count = data.Count;
            responseAjaxResult.SuccessResult(data, ResponseMessage.OPERATION_SUCCESS);
            return responseAjaxResult;
        }
        /// <summary>
        /// 项目干系单位
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<ProjectOrg>>> GetProjectOrgInfosAsync()
        {
            var responseAjaxResult = new ResponseAjaxResult<List<ProjectOrg>>();
            var data = await _dbContext.Queryable<ProjectOrg>()
                .Where(x => x.IsDelete == 1)
                .ToListAsync();

            responseAjaxResult.Count = data.Count;
            responseAjaxResult.SuccessResult(data, ResponseMessage.OPERATION_SUCCESS);
            return responseAjaxResult;
        }
        /// <summary>
        /// 往来单位数据
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<DealingUnit>>> GetDealingUnitAsync(int pageIndex, int pageSize)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<DealingUnit>>();
            RefAsync<int> total = 0;
            var data = await _dbContext.Queryable<DealingUnit>()
                .Where(x => x.IsDelete == 1)
                .ToPageListAsync(pageIndex, pageSize, total);

            responseAjaxResult.Count = total;
            responseAjaxResult.SuccessResult(data, ResponseMessage.OPERATION_SUCCESS);
            return responseAjaxResult;
        }
        /// <summary>
        /// 项目状态
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<ProjectStatusInfos>>> GetProjectStatusInfosAsync()
        {
            var responseAjaxResult = new ResponseAjaxResult<List<ProjectStatusInfos>>();
            var data = await _dbContext.Queryable<Domain.Models.ProjectStatus>()
                .Where(x => x.IsDelete == 1)
                .Select(x => new ProjectStatusInfos
                {
                    Code = x.Code,
                    Id = x.Id,
                    Name = x.Name,
                    Remarks = x.Remarks,
                    Sequence = x.Sequence,
                    StatusId = x.StatusId
                })
                .ToListAsync();

            responseAjaxResult.Count = data.Count;
            responseAjaxResult.SuccessResult(data, ResponseMessage.OPERATION_SUCCESS);
            return responseAjaxResult;
        }
        /// <summary>
        /// 项目类型信息
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<ProjectTypeInfos>>> GetProjectTypeInfosAsync()
        {
            var responseAjaxResult = new ResponseAjaxResult<List<ProjectTypeInfos>>();
            var data = await _dbContext.Queryable<Domain.Models.ProjectType>()
                .Where(x => x.IsDelete == 1)
                .Select(x => new ProjectTypeInfos
                {
                    Id = x.Id,
                    Code = x.Code,
                    PomId = x.PomId,
                    Remarks = x.Remarks,
                    Name = x.Name
                })
                .ToListAsync();

            responseAjaxResult.Count = data.Count;
            responseAjaxResult.SuccessResult(data, ResponseMessage.OPERATION_SUCCESS);
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取清单类型数据集
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<ShipCommResponseDto>>> GetListTypesAsync()
        {
            var responseAjaxResult = new ResponseAjaxResult<List<ShipCommResponseDto>>();

            var resList = await _dbContext.Queryable<DictionaryTable>()
                .Where(x => x.IsDelete == 1 && x.TypeNo == 9)
                .Select(x => new ShipCommResponseDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Type = x.Type,
                    Remark = x.Remark
                })
                .ToListAsync();

            responseAjaxResult.SuccessResult(resList, ResponseMessage.OPERATION_SUCCESS);
            responseAjaxResult.Count = resList.Count;
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取工艺方式数据集
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<ShipCommResponseDto>>> GetProcessMethodsAsync()
        {
            var responseAjaxResult = new ResponseAjaxResult<List<ShipCommResponseDto>>();

            var resList = await _dbContext.Queryable<ShipWorkMode>()
                .Where(x => x.IsDelete == 1)
                .Select(x => new ShipCommResponseDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    PomId = x.PomId,
                    Remark = "工艺方式"
                })
                .ToListAsync();

            responseAjaxResult.SuccessResult(resList, ResponseMessage.OPERATION_SUCCESS);
            responseAjaxResult.Count = resList.Count;
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取疏浚吹填分类数据集
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<ShipCommResponseDto>>> GetReclamationClassificationAsync()
        {
            var responseAjaxResult = new ResponseAjaxResult<List<ShipCommResponseDto>>();

            var resList = await _dbContext.Queryable<ShipWorkType>()
                .Where(x => x.IsDelete == 1)
                .Select(x => new ShipCommResponseDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    PomId = x.PomId,
                    Remark = "吹填分类"
                })
                .ToListAsync();

            responseAjaxResult.SuccessResult(resList, ResponseMessage.OPERATION_SUCCESS);
            responseAjaxResult.Count = resList.Count;
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取工况级别数据集
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<ShipCommResponseDto>>> GetWorkingConditionLevelAsync()
        {
            var responseAjaxResult = new ResponseAjaxResult<List<ShipCommResponseDto>>();

            var resList = await _dbContext.Queryable<WaterCarriage>()
                .Where(x => x.IsDelete == 1)
                .Select(x => new ShipCommResponseDto
                {
                    Id = x.Id,
                    Name = x.Remarks,
                    PomId = x.PomId,
                    Remark = "工况级别",
                    Type = Convert.ToInt32(x.Grade)
                })
                .ToListAsync();

            responseAjaxResult.SuccessResult(resList, ResponseMessage.OPERATION_SUCCESS);
            responseAjaxResult.Count = resList.Count;
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取船舶动态数据集
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<ShipCommResponseDto>>> GetShipDynamicAsync()
        {
            var responseAjaxResult = new ResponseAjaxResult<List<ShipCommResponseDto>>();

            var resList = await _dbContext.Queryable<DictionaryTable>()
                .Where(x => x.IsDelete == 1 && x.TypeNo == 10)
                .Select(x => new ShipCommResponseDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Remark = x.Remark,
                    Type = x.Type
                })
                .ToListAsync();

            responseAjaxResult.SuccessResult(resList, ResponseMessage.OPERATION_SUCCESS);
            responseAjaxResult.Count = resList.Count;
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取船舶日报
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<ShipDayReports>>> GetShipDayReportsAsync(ShipDayReportsRequestDto requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<ShipDayReports>>();
            var resList = new List<ShipDayReports>();
            var searchRequestDto = new ShipDailyRequestDto()
            {
                ShipPingId = requestDto.ShipPingId,
                PageIndex = requestDto.PageIndex,
                PageSize = requestDto.PageSize,
                StartTime = requestDto.StartTime,
                EndTime = requestDto.EndTime,
                IsDuiWai = true,
                ShipName = requestDto.ShipName
            };

            var responseData = await _projectProductionReportService.SearchShipDayReportAsync(searchRequestDto);
            var shipsDayReportInfos = responseData.Data.shipsDayReportInfos;
            foreach (var item in shipsDayReportInfos)
            {
                resList.Add(new ShipDayReports
                {
                    ConstructionEfficiency = item.ConstructionEfficiency,
                    DateDay = item.DateDay,
                    EstimatedCostAmount = item.EstimatedCostAmount,
                    EstimatedOutputAmount = item.EstimatedOutputAmount,
                    Id = item.Id,
                    NonProductionStoppage = item.NonProductionStoppage,
                    OilConsumption = item.OilConsumption,
                    PipelineLength = item.PipeLineLength,
                    ProductionOperatingTime = item.ProductionOperatingTime,
                    ProductionStoppage = item.ProductionStoppage,
                    ProjectId = item.ProjectId,
                    ShipId = item.ShipId,
                    ShipReportedProduction = item.ShipReportedProduction,
                    TimeAvailability = item.TimeAvailability,
                    BlowingWater = item.BlowingWater,
                    BlowShore = item.BlowShore,
                    Dredge = item.Dredge,
                    Sail = item.Sail,
                    SedimentDisposal = item.SedimentDisposal,
                    ShipState = item.ShipState,
                    ShipStateName = item.ShipStateName,
                    UpdateTime = item.UpdateTime
                });
            }

            var pageData = resList.Skip((requestDto.PageIndex - 1) * requestDto.PageSize).Take(requestDto.PageSize).ToList();
            responseAjaxResult.Count = responseData.Count;
            responseAjaxResult.SuccessResult(pageData, ResponseMessage.OPERATION_SUCCESS);
            return responseAjaxResult;
        }
        /// <summary>
        /// 自有船舶月报数据集
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<ShipMonthReports>>> GetSearchOwnShipMonthRepAsync(ShipMonthRequestDto requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<ShipMonthReports>>();
            var resList = new List<ShipMonthReports>();
            var monthRepRequest = new MonthRepRequestDto()
            {
                InEndDate = requestDto.InEndDate,
                InStartDate = requestDto.InStartDate,
                PageIndex = requestDto.PageIndex,
                PageSize = requestDto.PageSize,
                ShipName = requestDto.ShipId,
                IsDuiWai = true
            };

            var responseData = await _projectReportService.GetSearchOwnShipMonthRepAsync(monthRepRequest, 1);
            var searchOwnShipMonthReps = responseData.Data.searchOwnShipMonthReps;
            foreach (var item in searchOwnShipMonthReps)
            {
                resList.Add(new ShipMonthReports
                {
                    Id = item.Id,
                    ShipId = item.OwnShipId,
                    EnterTime = item.EnterTime,
                    GKJBId = item.GKJBId,
                    GYFSId = item.GYFSId,
                    MonthOutputVal = item.MonthOutputVal,
                    MonthQuantity = item.MonthQuantity,
                    MonthWorkDays = item.MonthWorkDays,
                    MonthWorkHours = item.MonthWorkHours,
                    ShipName = item.OwnShipName,
                    ProjectId = item.ProjectId,
                    QDLXId = item.QDLXId,
                    QDLXName = item.ContractTypeName,
                    QuitTime = item.QuitTime,
                    SJCTId = item.SJCTId,
                    UpdateTime = item.UpdateTime,
                    DateMonth = item.SubmitDate,
                    DigDeep = item.DigDeep,
                    BlowingDistance = item.BlowingDistance,
                    HaulDistance = item.HaulDistance
                });
            }

            var pageData = resList.Skip((requestDto.PageIndex - 1) * requestDto.PageSize).Take(requestDto.PageSize).ToList();
            responseAjaxResult.Count = responseData.Count;
            responseAjaxResult.SuccessResult(pageData, ResponseMessage.OPERATION_SUCCESS);
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取分包船舶月报数据集
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<SubShipMonthReports>>> GetSearchSubShipMonthRepAsync(ShipMonthRequestDto requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<SubShipMonthReports>>();
            var resList = new List<SubShipMonthReports>();
            var monthRepRequest = new MonthRepRequestDto()
            {
                InEndDate = requestDto.InEndDate,
                InStartDate = requestDto.InStartDate,
                PageIndex = requestDto.PageIndex,
                PageSize = requestDto.PageSize,
                ShipName = requestDto.ShipId,
                IsDuiWai = true
            };

            var responseData = await _projectReportService.GetSearchSubShipMonthRepAsync(monthRepRequest, 1);
            var data = responseData.Data;
            foreach (var item in data)
            {
                resList.Add(new SubShipMonthReports
                {
                    Id = item.Id,
                    EnterTime = item.EnterTime,
                    ProjectId = item.ProjectId,
                    QuitTime = item.QuitTime,
                    ShipDynamic = item.ShipDynamic,
                    SubShipId = item.SubShipId,
                    UpdateTime = item.UpdateTime
                });
            }

            var pageData = resList.Skip((requestDto.PageIndex - 1) * requestDto.PageSize).Take(requestDto.PageSize).ToList();
            responseAjaxResult.Count = responseData.Count;
            responseAjaxResult.SuccessResult(pageData, ResponseMessage.OPERATION_SUCCESS);
            return responseAjaxResult;
        }
        /// <summary>
        /// 船舶信息
        /// </summary>
        /// <param name="shipType">船舶类型 自有1：分包2</param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<ShipInfos>>> GetShipInfosAsync(int shipType)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<ShipInfos>>();
            var resList = new List<ShipInfos>();

            switch (shipType)
            {
                case 1:
                    resList = await _dbContext.Queryable<OwnerShip>()
                        .Where(x => x.IsDelete == 1)
                        .Select(x => new ShipInfos
                        {
                            Id = x.Id,
                            Mmsi = x.Mmsi,
                            PomId = x.PomId,
                            ShipName = x.Name,
                            ShipTypeId = x.TypeId
                        })
                        .ToListAsync();
                    break;
                case 2:
                    resList = await _dbContext.Queryable<SubShip>()
                        .Where(x => x.IsDelete == 1)
                        .Select(x => new ShipInfos
                        {
                            Id = x.Id,
                            Mmsi = x.Mmsi,
                            PomId = x.PomId,
                            ShipName = x.Name,
                            ShipTypeId = x.TypeId
                        })
                        .ToListAsync();
                    break;
            }

            responseAjaxResult.Count = resList.Count;
            responseAjaxResult.SuccessResult(resList, ResponseMessage.OPERATION_SUCCESS);
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取项目日报
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<DayReportInfo>>> GetSearchDayReportAsync(DayReportRequestDto requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<DayReportInfo>>();
            var searchRequestDto = new ProductionSafetyRequestDto
            {
                ProjectName = requestDto.ProjectName,
                ProjectStatusId = requestDto.ProjectStatusId,
                StartTime = requestDto.StartTime,
                EndTime = requestDto.EndTime,
                IsDuiWai = true,
                PageIndex = requestDto.PageIndex,
                PageSize = requestDto.PageSize
            };

            var responseData = await _projectProductionReportService.SearchDayReportAsync(searchRequestDto);
            var resList = responseData.Data.dayReportInfos;

            responseAjaxResult.Count = responseData.Count;
            responseAjaxResult.SuccessResult(resList, ResponseMessage.OPERATION_SUCCESS);
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取项目月报
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<MonthtReportDto>>> GetMonthReportInfosAsync(MonthReportInfosRequestDto requestDto)
        {

            #region 旧版
            var responseAjaxResult = new ResponseAjaxResult<List<MonthtReportDto>>();
            var model = new MonthtReportsRequstDto
            {
                ProjectName = requestDto.ProjectName,
                StartTime = requestDto.StartTime,
                EndTime = requestDto.EndTime,
                IsConvert = requestDto.IsConvert,
                PageIndex = requestDto.PageIndex,
                PageSize = requestDto.PageSize,
                IsDuiWai = true
            };

            var responseData = await _projectReportService.SearchMonthReportsAsync(model);
            var resList = responseData.Data.Reports;
            responseAjaxResult.Data = resList;
            responseAjaxResult.Count = responseData.Count;
            responseAjaxResult.Success();
            return responseAjaxResult;
            #endregion

            #region 参数校验
            //当前月份
            var month = string.Empty;
            if (requestDto.StartTime == null || requestDto.EndTime == null)
            {
                if (DateTime.Now.Day >= 27)
                {
                    requestDto.StartTime = DateTime.Now.ToString("yyyy-MM-26 00:00:00").ObjToDate();
                }
                else
                {
                    requestDto.StartTime = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-26 00:00:00").ObjToDate();
                }
                month = requestDto.StartTime.Value.Month.ToString();
            }
            else
            {
                if (requestDto.EndTime.Value.Day > 26)
                {
                    month = requestDto.StartTime.Value.Month.ToString();
                }
                else
                {
                    month = requestDto.EndTime.Value.AddMonths(-1).Month.ToString();
                }


            }
            if (month.Length == 1)
            {
                month = "0" + month;
            }
            var currentMonth = int.Parse(DateTime.Now.Year + month);
            //当年开始时间
            var yearStartIime = DateTime.Now.ToString("yyyy01").ObjToInt();
            var yearEndTime = DateTime.Now.ToString("yyyy12").ObjToInt();

            #endregion

            #region MyRegion


            //var responseAjaxResult = new ResponseAjaxResult<List<MonthtReportDto>>();
            //List<MonthtReportDto> monthtReportDtos = new List<MonthtReportDto>();
            //var projectMonthList = await _dbContext.Queryable<MonthReport>().Where(x => x.IsDelete == 1)
            //    .WhereIF(requestDto.StartTime != null && requestDto.EndTime != null, x => (x.CreateTime >= requestDto.StartTime.Value && x.CreateTime <= requestDto.EndTime.Value)
            //    || (x.UpdateTime >= requestDto.StartTime.Value && x.UpdateTime <= requestDto.EndTime.Value)).ToListAsync();

            //var projectHistoryList = await _dbContext.Queryable<ProjectMonthReportHistory>().Where(x => x.IsDelete == 1).ToListAsync();


            //var projectValueHistoryList = await _dbContext.Queryable<ProjectHistoryData>().Where(x => x.IsDelete == 1).ToListAsync();
            //if (projectMonthList.Any())
            //{
            //    foreach (var item in projectMonthList)
            //    {
            //        //当月产值
            //        //var currentProjectMonthRepost = projectMonthList.Where(x =>x.ProjectId == item.ProjectId).FirstOrDefault();
            //        //当年完成产值
            //        var yearCompletProductionValue = projectMonthList.Where(x => x.ProjectId == item.ProjectId
            //        && x.DateMonth >= yearStartIime && x.DateMonth <= yearEndTime).ToList();
            //        //累计完成成本
            //        var yearTotalCompletProductionValue = projectMonthList.Where(x => x.ProjectId == item.ProjectId
            //       ).ToList();
            //        monthtReportDtos.Add(new MonthtReportDto()
            //        {
            //            Id = item.Id,
            //            DateMonth = item.DateMonth,
            //            ProjectId = item.ProjectId,
            //            AccomplishQuantities = item.CompletedQuantity,
            //            YearAccomplishQuantities = yearCompletProductionValue.Sum(x => x.CompletedQuantity),
            //            AccumulativeQuantities = yearTotalCompletProductionValue.Sum(x => x.CompletedQuantity) + projectValueHistoryList.Where(x => x.ProjectId == item.ProjectId && x.AccumulatedProduction != null).Sum(x => x.AccumulatedProduction.Value),

            //            RecognizedValue = item.PartyAConfirmedProductionAmount,
            //            YearRecognizedValue = yearCompletProductionValue.Sum(x => x.PartyAConfirmedProductionAmount),
            //            CumulativeValue = yearTotalCompletProductionValue.Sum(x => x.PartyAConfirmedProductionAmount) + projectHistoryList.Where(x => x.ProjectId == item.ProjectId && x.KaileiOwnerConfirmation != null).Sum(x => x.KaileiOwnerConfirmation.Value),

            //            PaymentAmount = item.PartyAPayAmount,
            //            YearPaymentAmount = yearCompletProductionValue.Sum(x => x.PartyAPayAmount),
            //            CumulativePaymentAmount = yearTotalCompletProductionValue.Sum(x => x.PartyAPayAmount) + projectHistoryList.Where(x => x.ProjectId == item.ProjectId && x.KaileiProjectPayment != null).Sum(x => x.KaileiProjectPayment.Value),


            //            AccomplishValue = item.CompleteProductionAmount,// currentProjectMonthRepost.CompleteProductionAmount,
            //            YearAccomplishValue = yearCompletProductionValue.Sum(x => x.CompleteProductionAmount),
            //            //YearAccomplishCost = yearCompletProductionValue.Sum(x => x.CompleteProductionAmount),
            //            // CumulativeAccomplishCost = yearTotalCompletProductionValue.Sum(x => x.CompleteProductionAmount),

            //            CumulativeCompleted = yearTotalCompletProductionValue.Sum(x => x.CompleteProductionAmount) + projectValueHistoryList.Where(x => x.ProjectId == item.ProjectId
            //            && x.AccumulatedOutputValue != null).Sum(x => x.AccumulatedOutputValue.Value),
            //            OutsourcingExpensesAmount = item.OutsourcingExpensesAmount,
            //        });

            //    }
            //}

            //responseAjaxResult.Data = monthtReportDtos;
            //responseAjaxResult.Count = monthtReportDtos.Count;
            //responseAjaxResult.Success();
            //return responseAjaxResult;
            #endregion

        }
        /// <summary>
        /// 获取船舶进退场
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<ShipMovementResponseDto>>> GetShipMovementAsync(ShipMovementsRequestDto model)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<ShipMovementResponseDto>>();

            var responseData = await _shipMovementService.SearchShipMovementsAsync(model);
            var resList = responseData.Data;

            responseAjaxResult.Count = responseData.Count;
            responseAjaxResult.SuccessResult(resList, ResponseMessage.OPERATION_SUCCESS);
            return responseAjaxResult;
        }

        #region 返回全表信息
        /// <summary>
        /// 获取全表字段项目信息
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<Project>>> GetProjectsTableAsync(ExternalRequestDto requestDto)
        {
            /***
             * 数据初始化
             */
            var responseAjaxResult = new ResponseAjaxResult<List<Project>>();
            requestDto.TimeValidatableObject();
            var sTime = requestDto.StartTimeValue;
            var eTime = requestDto.EndTimeValue;

            /***
             * 数据读取
             */
            var projectData = await _dbContext.Queryable<Project>()
                .Where(x => x.IsDelete == 1)
                .ToListAsync();
            projectData = projectData
                .Where(x => string.IsNullOrEmpty(x.UpdateTime.ToString()) || x.UpdateTime == DateTime.MinValue ?
                  x.CreateTime >= sTime && x.CreateTime <= eTime
                : x.UpdateTime >= sTime && x.UpdateTime <= eTime)
                .ToList();
            var CurrencyConverter = await _dbContext.Queryable<CurrencyConverter>().Where(x => x.IsDelete == 1 && x.Year == DateTime.Now.Year).ToListAsync();

            foreach (var item in projectData)
            {
                if (item.Category == 1)
                {
                    var isExist = CurrencyConverter.Where(x => x.CurrencyId == item.CurrencyId.Value.ToString()).FirstOrDefault();
                    if (isExist != null)
                    {
                        item.Amount = item.Amount.Value * isExist.ExchangeRate;
                        item.ECAmount = item.ECAmount.Value * isExist.ExchangeRate;
                    }
                }
            }
            responseAjaxResult.Count = projectData.Count;
            responseAjaxResult.SuccessResult(projectData);
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取全表字段公司机构
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<Institution>>> GetInstitutionTableAsync(ExternalRequestDto requestDto)
        {
            /***
             * 数据初始化
             */
            var responseAjaxResult = new ResponseAjaxResult<List<Institution>>();
            requestDto.TimeValidatableObject();
            var sTime = requestDto.StartTimeValue;
            var eTime = requestDto.EndTimeValue;

            /***
             * 数据读取
             */
            var institutionData = await _dbContext.Queryable<Institution>()
                .Where(x => x.IsDelete == 1)
                .ToListAsync();
            institutionData = institutionData
                .Where(x => string.IsNullOrEmpty(x.UpdateTime.ToString()) || x.UpdateTime == DateTime.MinValue ?
                  x.CreateTime >= sTime && x.CreateTime <= eTime
                : x.UpdateTime >= sTime && x.UpdateTime <= eTime)
                .ToList();

            responseAjaxResult.Count = institutionData.Count;
            responseAjaxResult.SuccessResult(institutionData);
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取全表字段项目类型
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>

        public async Task<ResponseAjaxResult<List<ProjectType>>> GetProjectTypeTableAsync(ExternalRequestDto requestDto)
        {
            /***
             * 数据初始化
             */
            var responseAjaxResult = new ResponseAjaxResult<List<ProjectType>>();
            requestDto.TimeValidatableObject();
            var sTime = requestDto.StartTimeValue;
            var eTime = requestDto.EndTimeValue;
            /***
           * 数据读取
           */
            var projectTypeData = await _dbContext.Queryable<ProjectType>()
                .Where(x => x.IsDelete == 1)
                .ToListAsync();
            projectTypeData = projectTypeData
                .Where(x => string.IsNullOrEmpty(x.UpdateTime.ToString()) || x.UpdateTime == DateTime.MinValue ?
                  x.CreateTime >= sTime && x.CreateTime <= eTime
                : x.UpdateTime >= sTime && x.UpdateTime <= eTime)
                .ToList();

            responseAjaxResult.Count = projectTypeData.Count;
            responseAjaxResult.SuccessResult(projectTypeData);
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取全表字段项目状态
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<ProjectStatus>>> GetProjectStatusTableAsync(ExternalRequestDto requestDto)
        {
            /***
             * 数据初始化
             */
            var responseAjaxResult = new ResponseAjaxResult<List<ProjectStatus>>();
            requestDto.TimeValidatableObject();
            var sTime = requestDto.StartTimeValue;
            var eTime = requestDto.EndTimeValue;
            /***
           * 数据读取
           */
            var projectStatusData = await _dbContext.Queryable<ProjectStatus>()
                .Where(x => x.IsDelete == 1)
                .ToListAsync();
            projectStatusData = projectStatusData
                .Where(x => string.IsNullOrEmpty(x.UpdateTime.ToString()) || x.UpdateTime == DateTime.MinValue ?
                  x.CreateTime >= sTime && x.CreateTime <= eTime
                : x.UpdateTime >= sTime && x.UpdateTime <= eTime)
                .ToList();

            responseAjaxResult.Count = projectStatusData.Count;
            responseAjaxResult.SuccessResult(projectStatusData);
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取全表字段项目规模
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<ProjectScale>>> GetProjectScaleTableAsync(ExternalRequestDto requestDto)
        {
            /***
             * 数据初始化
             */
            var responseAjaxResult = new ResponseAjaxResult<List<ProjectScale>>();
            requestDto.TimeValidatableObject();
            var sTime = requestDto.StartTimeValue;
            var eTime = requestDto.EndTimeValue;
            /***
           * 数据读取
           */
            var projectScaleData = await _dbContext.Queryable<ProjectScale>()
                .Where(x => x.IsDelete == 1)
                .ToListAsync();
            projectScaleData = projectScaleData
                .Where(x => string.IsNullOrEmpty(x.UpdateTime.ToString()) || x.UpdateTime == DateTime.MinValue ?
                  x.CreateTime >= sTime && x.CreateTime <= eTime
                : x.UpdateTime >= sTime && x.UpdateTime <= eTime)
                .ToList();

            responseAjaxResult.Count = projectScaleData.Count;
            responseAjaxResult.SuccessResult(projectScaleData);
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取全表字段施工地点
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<Province>>> GetProjectProvinceTableAsync(ExternalRequestDto requestDto)
        {
            /***
             * 数据初始化
             */
            var responseAjaxResult = new ResponseAjaxResult<List<Province>>();
            requestDto.TimeValidatableObject();
            var sTime = requestDto.StartTimeValue;
            var eTime = requestDto.EndTimeValue;
            /***
           * 数据读取
           */
            var projectProvinceData = await _dbContext.Queryable<Province>()
                .Where(x => x.IsDelete == 1)
                .ToListAsync();
            projectProvinceData = projectProvinceData
                .Where(x => string.IsNullOrEmpty(x.UpdateTime.ToString()) || x.UpdateTime == DateTime.MinValue ?
                  x.CreateTime >= sTime && x.CreateTime <= eTime
                : x.UpdateTime >= sTime && x.UpdateTime <= eTime)
                .ToList();

            responseAjaxResult.Count = projectProvinceData.Count;
            responseAjaxResult.SuccessResult(projectProvinceData);
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取全表字段施工区域
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<ProjectArea>>> GetProjectAreaTableAsync(ExternalRequestDto requestDto)
        {
            /***
             * 数据初始化
             */
            var responseAjaxResult = new ResponseAjaxResult<List<ProjectArea>>();
            requestDto.TimeValidatableObject();
            var sTime = requestDto.StartTimeValue;
            var eTime = requestDto.EndTimeValue;
            /***
           * 数据读取
           */
            var projectAreaData = await _dbContext.Queryable<ProjectArea>()
                .Where(x => x.IsDelete == 1)
                .ToListAsync();
            projectAreaData = projectAreaData
                .Where(x => string.IsNullOrEmpty(x.UpdateTime.ToString()) || x.UpdateTime == DateTime.MinValue ?
                  x.CreateTime >= sTime && x.CreateTime <= eTime
                : x.UpdateTime >= sTime && x.UpdateTime <= eTime)
                .ToList();

            responseAjaxResult.Count = projectAreaData.Count;
            responseAjaxResult.SuccessResult(projectAreaData);
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取全表字段自有船舶
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<OwnerShip>>> GetOwnerShipTableAsync(ExternalRequestDto requestDto)
        {
            /***
             * 数据初始化
             */
            var responseAjaxResult = new ResponseAjaxResult<List<OwnerShip>>();
            requestDto.TimeValidatableObject();
            var sTime = requestDto.StartTimeValue;
            var eTime = requestDto.EndTimeValue;
            /***
           * 数据读取
           */
            var ownshipData = await _dbContext.Queryable<OwnerShip>()
                .Where(x => x.IsDelete == 1)
                .ToListAsync();
            ownshipData = ownshipData
                .Where(x => string.IsNullOrEmpty(x.UpdateTime.ToString()) || x.UpdateTime == DateTime.MinValue ?
                  x.CreateTime >= sTime && x.CreateTime <= eTime
                : x.UpdateTime >= sTime && x.UpdateTime <= eTime)
                .ToList();

            responseAjaxResult.Count = ownshipData.Count;
            responseAjaxResult.SuccessResult(ownshipData);
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取全表字段分包船舶
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<SubShip>>> GetSubShipTableAsync(ExternalRequestDto requestDto)
        {
            /***
             * 数据初始化
             */
            var responseAjaxResult = new ResponseAjaxResult<List<SubShip>>();
            requestDto.TimeValidatableObject();
            var sTime = requestDto.StartTimeValue;
            var eTime = requestDto.EndTimeValue;
            /***
           * 数据读取
           */
            var subShipData = await _dbContext.Queryable<SubShip>()
                .Where(x => x.IsDelete == 1)
                .ToListAsync();
            subShipData = subShipData
                .Where(x => string.IsNullOrEmpty(x.UpdateTime.ToString()) || x.UpdateTime == DateTime.MinValue ?
                  x.CreateTime >= sTime && x.CreateTime <= eTime
                : x.UpdateTime >= sTime && x.UpdateTime <= eTime)
                .ToList();

            responseAjaxResult.Count = subShipData.Count;
            responseAjaxResult.SuccessResult(subShipData);
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取全表字段船级社
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<ShipClassic>>> GetShipClassicTableAsync(ExternalRequestDto requestDto)
        {
            /***
             * 数据初始化
             */
            var responseAjaxResult = new ResponseAjaxResult<List<ShipClassic>>();
            requestDto.TimeValidatableObject();
            var sTime = requestDto.StartTimeValue;
            var eTime = requestDto.EndTimeValue;
            /***
           * 数据读取
           */
            var shipClassicData = await _dbContext.Queryable<ShipClassic>()
                .Where(x => x.IsDelete == 1)
                .ToListAsync();
            shipClassicData = shipClassicData
                .Where(x => string.IsNullOrEmpty(x.UpdateTime.ToString()) || x.UpdateTime == DateTime.MinValue ?
                  x.CreateTime >= sTime && x.CreateTime <= eTime
                : x.UpdateTime >= sTime && x.UpdateTime <= eTime)
                .ToList();

            responseAjaxResult.Count = shipClassicData.Count;
            responseAjaxResult.SuccessResult(shipClassicData);
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取全表字段船舶类型
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<ShipPingType>>> GetShipPingTypeTableAsync(ExternalRequestDto requestDto)
        {
            /***
             * 数据初始化
             */
            var responseAjaxResult = new ResponseAjaxResult<List<ShipPingType>>();
            requestDto.TimeValidatableObject();
            var sTime = requestDto.StartTimeValue;
            var eTime = requestDto.EndTimeValue;
            /***
           * 数据读取
           */
            var shipTypeData = await _dbContext.Queryable<ShipPingType>()
                .Where(x => x.IsDelete == 1)
                .ToListAsync();
            shipTypeData = shipTypeData
                .Where(x => string.IsNullOrEmpty(x.UpdateTime.ToString()) || x.UpdateTime == DateTime.MinValue ?
                  x.CreateTime >= sTime && x.CreateTime <= eTime
                : x.UpdateTime >= sTime && x.UpdateTime <= eTime)
                .ToList();

            responseAjaxResult.Count = shipTypeData.Count;
            responseAjaxResult.SuccessResult(shipTypeData);
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取全表字段船舶状态
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<ShipStatus>>> GetShipStatusTableAsync(ExternalRequestDto requestDto)
        {
            /***
             * 数据初始化
             */
            var responseAjaxResult = new ResponseAjaxResult<List<ShipStatus>>();
            requestDto.TimeValidatableObject();
            var sTime = requestDto.StartTimeValue;
            var eTime = requestDto.EndTimeValue;
            /***
           * 数据读取
           */
            var shipStatusData = await _dbContext.Queryable<ShipStatus>()
                .Where(x => x.IsDelete == 1)
                .ToListAsync();
            shipStatusData = shipStatusData
                .Where(x => string.IsNullOrEmpty(x.UpdateTime.ToString()) || x.UpdateTime == DateTime.MinValue ?
                  x.CreateTime >= sTime && x.CreateTime <= eTime
                : x.UpdateTime >= sTime && x.UpdateTime <= eTime)
                .ToList();

            responseAjaxResult.Count = shipStatusData.Count;
            responseAjaxResult.SuccessResult(shipStatusData);
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取全表字段船舶进退场
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<ShipMovement>>> GetShipMovementTableAsync(ExternalRequestDto requestDto)
        {
            /***
             * 数据初始化
             */
            var responseAjaxResult = new ResponseAjaxResult<List<ShipMovement>>();
            requestDto.TimeValidatableObject();
            var sTime = requestDto.StartTimeValue;
            var eTime = requestDto.EndTimeValue;
            /***
           * 数据读取
           */
            var shipMovementData = await _dbContext.Queryable<ShipMovement>()
                .Where(x => x.IsDelete == 1)
                .ToListAsync();
            shipMovementData = shipMovementData
                .Where(x => string.IsNullOrEmpty(x.UpdateTime.ToString()) || x.UpdateTime == DateTime.MinValue ?
                  x.CreateTime >= sTime && x.CreateTime <= eTime
                : x.UpdateTime >= sTime && x.UpdateTime <= eTime)
                .ToList();

            responseAjaxResult.Count = shipMovementData.Count;
            responseAjaxResult.SuccessResult(shipMovementData);
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取全表字段项目产值计划
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<ProjectPlanProduction>>> GetProjectPlanProductionAsync()
        {
            var responseAjaxRestult = new ResponseAjaxResult<List<ProjectPlanProduction>>();
            var result = await _dbContext.Queryable<ProjectPlanProduction>()
                .Where(x => x.IsDelete == 1)
                .ToListAsync();

            responseAjaxRestult.Count = result.Count;
            responseAjaxRestult.SuccessResult(result);
            return responseAjaxRestult;

        }
        /// <summary>
        /// 获取全表字段水上设备
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<SearchEquipmentManagementResponseDto>>> GetSearchEquipmentManagementAsync(ExternalRequestDto requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<SearchEquipmentManagementResponseDto>>();
            var searchEquipmentManagementRequestDto = new SearchEquipmentManagementRequestDto()
            {
                IsDuiWai = true,
                StarTime = requestDto.StartTime,
                EndTime = requestDto.EndTime,
                DeviceType = 1
            };

            var responseData = await _eqipment.SearchEquipmentManagementAsync(searchEquipmentManagementRequestDto);
            var res = responseData.Data;

            responseAjaxResult.Count = responseData.Count;
            responseAjaxResult.SuccessResult(res);
            return responseAjaxResult;
        }

        public async Task<ResponseAjaxResult<List<ProjectStatusChangResponse>>> SearchProjectChangeList()
        {
            ResponseAjaxResult<List<ProjectStatusChangResponse>> responseAjaxResult = new ResponseAjaxResult<List<ProjectStatusChangResponse>>();
            List<ProjectStatusChangResponse> projectStatusChangResponses = new List<ProjectStatusChangResponse>();
            var list = _dbContext.Queryable<StartWorkRecord>()
               .Where((it) => it.BeforeStatus != it.AfterStatus)
                .GroupBy(it => it.ProjectId)
                .Select(it => new
                {
                    name = it.ProjectId,
                    CreateTime = SqlFunc.AggregateMax(it.CreateTime)
                })
                .MergeTable()
                .LeftJoin<StartWorkRecord>((a, b) => a.name == b.ProjectId)
                  .Where((a, b) => a.CreateTime == b.CreateTime)
                .Select((a, b) => b).ToList();
            foreach (var item in list)
            {
                projectStatusChangResponses.Add(new ProjectStatusChangResponse()
                {
                    AfterStautsId = item.AfterStatus,
                    BeforeStautsId = item.BeforeStatus,
                    Id = item.ProjectId.Value,
                    Time = item.CreateTime.Value
                });
            }
            responseAjaxResult.Count = projectStatusChangResponses.Count;
            responseAjaxResult.Data = projectStatusChangResponses;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取危大工程项
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<DangerousDetails>>> DangerousDetailsAsync()
        {
            ResponseAjaxResult<List<DangerousDetails>> rt = new();
            var rr = await _dbContext.Queryable<DangerousDetails>()
                .Where(x => x.IsDelete == 1)
                .ToListAsync();

            rt.SuccessResult(rr);
            return rt;
        }
        /// <summary>
        /// 施工日志
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<DayReportConstruction>>> GetDayReportConstructionAsync(ExternalRequestDto requestDto)
        {
            ResponseAjaxResult<List<DayReportConstruction>> rt = new();
            var rr = await _dbContext.Queryable<DayReportConstruction>()
                .Where(x => x.IsDelete == 1)
                .ToListAsync();
            rr = rr
                .Where(x => string.IsNullOrEmpty(x.UpdateTime.ToString()) || x.UpdateTime == DateTime.MinValue ?
                  x.CreateTime >= requestDto.StartTime && x.CreateTime <= requestDto.EndTime
                : x.UpdateTime >= requestDto.StartTime && x.UpdateTime <= requestDto.EndTime)
                .ToList();

            rt.SuccessResult(rr);
            return rt;
        }
        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseExternalRequest"></param>
        /// <returns></returns>

        public async Task<CompanyDayProductionValueResponseDto> SearchCompanyProductionValueAsync(BaseExternalRequestDto baseExternalRequest)
        {
            CompanyDayProductionValueResponseDto companyDayProductionValueResponseDto = new CompanyDayProductionValueResponseDto()
            {
                CompanyItems = new List<CompanyItem>()
            };

            var projectList= await _dbContext.Queryable<Project>().Where(x => x.IsDelete == 1).ToListAsync();
            var commonDataList = await _dbContext.Queryable<ProductionMonitoringOperationDayReport>().Where(x => x.IsDelete == 1&&x.Type==1&&!string.IsNullOrWhiteSpace(x.Name)&&x.Name!= "广航局总体").OrderBy(x=>x.Sort).ToListAsync();
            if (projectList.Count > 0)
            {
                //各个公司总产值
                List<CompanyItem> companys = new List<CompanyItem>();
                var allDayReport = await _dbContext.Queryable<DayReport>().Where(x => x.IsDelete == 1&&x.DateDay>=20241226).ToListAsync();
                 var dayReportList= allDayReport.Where(x => x.IsDelete == 1 &&(x.CreateTime>=SqlFunc.ToDate(baseExternalRequest.StartTime)&&x.CreateTime<= SqlFunc.ToDate(baseExternalRequest.EndTime)|| x.UpdateTime >= SqlFunc.ToDate(baseExternalRequest.StartTime) && x.UpdateTime <= SqlFunc.ToDate (baseExternalRequest.EndTime)))
                .ToList();
                var diffTime = TimeHelper.GetTimeSpan(baseExternalRequest.StartTime.ObjToDate(), baseExternalRequest.EndTime.ObjToDate()).Days;
                for (int i = 0; i <=diffTime; i++)
                {
                    foreach (var item in commonDataList)
                    {
                       var dateDay=int.Parse( baseExternalRequest.StartTime.ObjToDate().AddDays(i-1).ToString("yyyyMMdd"));
                        var companyProjectIds = projectList.Where(x => x.CompanyId == item.ItemId).Select(x => x.Id).ToList();
                        var dayProductionValue = dayReportList.Where(x => x.DateDay == dateDay && companyProjectIds.Contains(x.ProjectId)).Sum(x => x.DayActualProductionAmount);

                        var yearProductionValue = dayReportList.Where(x => x.DateDay <= dateDay && companyProjectIds.Contains(x.ProjectId)).Sum(x => x.DayActualProductionAmount);

                        companys.Add(new CompanyItem()
                        { 
                            DateDay = baseExternalRequest.StartTime.ObjToDate().AddDays(i).ToString(),
                            CompanyDayProductionValue = dayProductionValue,
                            YearCompanyProductionValue = yearProductionValue,
                            CompanyId = item.ItemId,
                            
                        }) ;
                    }
                }
               
                companyDayProductionValueResponseDto.CompanyItems = companys.Where(x => x.CompanyId != null).ToList();
            }
            return companyDayProductionValueResponseDto;
        }
    }
}
