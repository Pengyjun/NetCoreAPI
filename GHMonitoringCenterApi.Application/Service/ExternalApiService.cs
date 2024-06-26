using GHMonitoringCenterApi.Application.Contracts.Dto;
using GHMonitoringCenterApi.Application.Contracts.Dto.ConstructionProjectDaily;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report;
using GHMonitoringCenterApi.Application.Contracts.Dto.ProjectProductionReport;
using GHMonitoringCenterApi.Application.Contracts.Dto.Ship;
using GHMonitoringCenterApi.Application.Contracts.IService;
using GHMonitoringCenterApi.Application.Contracts.IService.Project;
using GHMonitoringCenterApi.Application.Contracts.IService.ProjectProductionReport;
using GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Domain.Shared;
using GHMonitoringCenterApi.Domain.Shared.Const;
using GHMonitoringCenterApi.Domain.Shared.Util;
using NPOI.POIFS.Crypt.Dsig;
using SqlSugar;
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
        /// 项目船舶月报接口注入
        /// 项目月报接口注入
        /// </summary>
        private IProjectReportService _projectReportService { get; set; }
        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="sqlSugarClient"></param>
        /// <param name="projectProductionReportService"></param>
        /// <param name="projectReportService"></param>
        /// <param name="globalObject"></param>
        public ExternalApiService(ISqlSugarClient sqlSugarClient, IProjectProductionReportService projectProductionReportService, IProjectReportService projectReportService, GlobalObject globalObject)
        {
            this._dbContext = sqlSugarClient;
            this._projectProductionReportService = projectProductionReportService;
            this._projectReportService = projectReportService;
            this._globalObject = globalObject;
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
                    Code = x.Code,
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
                    RegionId = x.RegionId
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
                    UpdateTime = item.UpdateTime
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
                IsDuiWai = true
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
            var responseAjaxResult = new ResponseAjaxResult<List<MonthtReportDto>>();
            var model = new MonthtReportsRequstDto
            {
                ProjectName = requestDto.ProjectName,
                StartTime = requestDto.StartTime,
                EndTime = requestDto.EndTime,
                IsConvert = requestDto.IsConvert,
                IsDuiWai = true
            };

            var responseData = await _projectReportService.SearchMonthReportsAsync(model);
            var resList = responseData.Data.Reports;

            responseAjaxResult.Count = responseData.Count;
            responseAjaxResult.SuccessResult(resList, ResponseMessage.OPERATION_SUCCESS);
            return responseAjaxResult;
        }
    }
}
