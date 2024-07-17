using AutoMapper;
using CDC.MDM.Core.Common.Util;
using GHMonitoringCenterApi.Application.Contracts.Dto.Common;
using GHMonitoringCenterApi.Application.Contracts.Dto.Enums;
using GHMonitoringCenterApi.Application.Contracts.Dto.File;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report;
using GHMonitoringCenterApi.Application.Contracts.Dto.ProjectProductionReport;
using GHMonitoringCenterApi.Application.Contracts.Dto.Ship;
using GHMonitoringCenterApi.Application.Contracts.Dto.UnReport;
using GHMonitoringCenterApi.Application.Contracts.IService;
using GHMonitoringCenterApi.Application.Contracts.IService.BizAuthorize;
using GHMonitoringCenterApi.Application.Contracts.IService.Project;
using GHMonitoringCenterApi.Application.Contracts.IService.Push;
using GHMonitoringCenterApi.Domain.Enums;
using GHMonitoringCenterApi.Domain.IRepository;
using GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Domain.Shared;
using GHMonitoringCenterApi.Domain.Shared.Const;
using GHMonitoringCenterApi.Domain.Shared.Enums;
using GHMonitoringCenterApi.Domain.Shared.Util;
using Newtonsoft.Json;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using SqlSugar;
using SqlSugar.Extensions;
using System.Data;
using System.Text.RegularExpressions;
using UtilsSharp;
using Model = GHMonitoringCenterApi.Domain.Models;

namespace GHMonitoringCenterApi.Application.Service.Projects
{
    /// <summary>
    ///  项目-填报业务层
    /// </summary>
    public class ProjectReportService : IProjectReportService
    {
        #region 注入实例

        /// <summary>
        /// 项目
        /// </summary>
        private readonly IBaseRepository<Project> _dbProject;

        /// <summary>
        /// 推送到Pom的业务层
        /// </summary>
        private readonly IPushPomService _iPushPomService;

        /// <summary>
        /// 项目日报
        /// </summary>
        private readonly IBaseRepository<DayReport> _dbDayReport;

        /// <summary>
        ///项目日报-施工日志
        /// </summary>
        private readonly IBaseRepository<DayReportConstruction> _dbDayReportConstruction;

        /// <summary>
        /// 文件
        /// </summary>
        private readonly IBaseRepository<Files> _dbFile;

        /// <summary>
        /// 往来单位
        /// </summary>
        private readonly IBaseRepository<DealingUnit> _dbDealingUnit;

        /// <summary>
        /// 分包船舶
        /// </summary>
        private readonly IBaseRepository<SubShip> _dbSubShip;

        /// <summary>
        /// 自有船舶
        /// </summary>
        private readonly IBaseRepository<OwnerShip> _dbOwnerShip;

        /// <summary>
        /// 施工分类
        /// </summary>
        private readonly IBaseRepository<ProjectWBS> _dbProjectWBS;

        /// <summary>
        /// 字典表
        /// </summary>
        private readonly IBaseRepository<DictionaryTable> _dbDictionaryTable;

        /// <summary>
        /// 用户表
        /// </summary>
        private readonly IBaseRepository<Domain.Models.User> _dbUser;

        /// <summary>
        /// 月份交叉表
        /// </summary>

        private readonly IBaseRepository<CrossMonth> _dbcrossMonth;

        /// <summary>
        /// 安监日报
        /// </summary>
        private readonly IBaseRepository<SafeSupervisionDayReport> _dbSafeSupervisionDayReport;

        /// <summary>
        /// 船舶日报
        /// </summary>
        private readonly IBaseRepository<ShipDayReport> _dbShipDayReport;

        /// <summary>
        /// 港口
        /// </summary>
        private readonly IBaseRepository<PortData> _dbPortData;

        /// <summary>
        /// 项目月报
        /// </summary>
        private readonly IBaseRepository<MonthReport> _dbMonthReport;

        /// <summary>
        /// 项目月报-明细
        /// </summary>
        private readonly IBaseRepository<MonthReportDetail> _dbMonthReportDetail;

        /// <summary>
        /// 船舶动态月报
        /// </summary>
        private readonly IBaseRepository<ShipDynamicMonthReport> _dbShipDynamicMonthReport;

        /// <summary>
        /// 船舶动态月报明细
        /// </summary>
        private readonly IBaseRepository<ShipDynamicMonthReportDetail> _dbShipDynamicMonthReportDetail;

        /// <summary>
        /// 分包船舶月报
        /// </summary>
        private readonly IBaseRepository<SubShipMonthReport> _dbSubShipMonthReport;

        /// <summary>
        /// 自有船舶月报
        /// </summary>
        private readonly IBaseRepository<OwnerShipMonthReport> _dbOwnerShipMonthReport;

        /// <summary>
        /// 自有船舶月报施工土质
        /// </summary>
        private readonly IBaseRepository<OwnerShipMonthReportSoil> _dbOwnerShipMonthReportSoil;

        /// <summary>
        /// 船舶进出场
        /// </summary>
        private readonly IBaseRepository<ShipMovement> _dbShipMovement;

        /// <summary>
        /// 船舶类型
        /// </summary>
        private readonly IBaseRepository<ShipPingType> _dbShipPingType;

        /// <summary>
        /// 年度计划
        /// </summary>
        private readonly IBaseRepository<ProjectPlanProduction> _dbProjectAnnualPlan;

        /// <summary>
        /// 日期
        /// </summary>
        private readonly IBaseRepository<CrossDateDay> _dbCrossDateDay;

        /// <summary>
        /// 项目类型
        /// </summary>
        private readonly IBaseRepository<ProjectType> _dbProjectType;

        /// <summary>
        /// 项目状态
        /// </summary>
        private readonly IBaseRepository<ProjectStatus> _dbProjectStatus;

        /// <summary>
        /// 机构（公司）
        /// </summary>
        private readonly IBaseRepository<Institution> _dbInstitution;

        /// <summary>
        /// 省份
        /// </summary>
        private readonly IBaseRepository<Province> _dbProvince;

        /// <summary>
        /// 区域
        /// </summary>
        private readonly IBaseRepository<ProjectArea> _dbProjectArea;

        /// <summary>
        /// 项目干系单位
        /// </summary>
        private readonly IBaseRepository<ProjectOrg> _dbProjectOrg;

        /// <summary>
        /// 项目施工资质
        /// </summary>
        private readonly IBaseRepository<ConstructionQualification> _dbConstructionQualification;

        /// <summary>
        /// 项目行业分类标准
        /// </summary>
        private readonly IBaseRepository<IndustryClassification> _dbIndustryClassification;

        /// <summary>
        /// 暂存业务表
        /// </summary>
        private readonly IBaseRepository<StagingData> _dbStagingData;

        /// <summary>
        /// 项目领导班子
        /// </summary>
        private readonly IBaseRepository<ProjectLeader> _dbProjectLeader;

        /// <summary>
        /// 税率
        /// </summary>
        private readonly IBaseRepository<CurrencyConverter> _dbCurrencyConverter;

        /// <summary>
        /// 任务
        /// </summary>
        private readonly IBaseRepository<Domain.Models.Job> _dbJob;


        /// <summary>
        /// 对象变更服务
        /// </summary>
        private readonly IEntityChangeService _entityChangeService;

        /// <summary>
        /// 基础业务层
        /// </summary>
        private readonly IBaseService _baseService;

        /// <summary>
        /// 业务授权层
        /// </summary>
        private readonly IBizAuthorizeService _bizAuthorizeService;

        /// <summary>
        /// 匹配
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// 全局对象
        /// </summary>
        private readonly GlobalObject _globalObject;

        /// <summary>
        /// 数据库上下文
        /// </summary>
        public ISqlSugarClient _dbContext;
        #endregion

        /// <summary>
        /// 当前登录用户
        /// </summary>
        private CurrentUser _currentUser { get { return _globalObject.CurrentUser; } }


        /// <summary>
        /// 构造
        /// </summary>
        public ProjectReportService(IBaseRepository<Project> dbProject
            , IBaseRepository<DayReport> dbDayReport
            , IBaseRepository<DayReportConstruction> dbDayReportConstruction
            , IBaseRepository<Files> dbFile
            , IBaseRepository<DealingUnit> dbDealingUnit
            , IBaseRepository<SubShip> dbSubShip
            , IBaseRepository<OwnerShip> dbOwnerShip
            , IBaseRepository<ProjectWBS> dbProjectWBS
            , IBaseRepository<Domain.Models.User> dbUser
            , IBaseRepository<DictionaryTable> dbDictionaryTable
            , IBaseRepository<SafeSupervisionDayReport> dbSafeSupervisionDayReport
            , IBaseRepository<ShipDayReport> dbShipDayReport
            , IBaseRepository<PortData> dbPortData
            , IBaseRepository<MonthReport> dbMonthReport
            , IBaseRepository<MonthReportDetail> dbMonthReportDetail
            , IBaseRepository<ShipDynamicMonthReport> dbShipDynamicMonthReport
            , IBaseRepository<ShipDynamicMonthReportDetail> dbShipDynamicMonthReportDetail
            , IBaseRepository<SubShipMonthReport> dbSubShipMonthReport
            , IBaseRepository<OwnerShipMonthReport> dbOwnerShipMonthReport
            , IBaseRepository<OwnerShipMonthReportSoil> dbOwnerShipMonthReportSoil
            , IBaseRepository<ShipMovement> dbShipMovement
            , IBaseRepository<ShipPingType> dbShipPingType
            , IBaseRepository<ProjectPlanProduction> dbProjectAnnualPlan
            , IBaseRepository<CrossDateDay> dbCrossDateDay
            , IBaseRepository<ProjectType> dbProjectType
            , IBaseRepository<ProjectStatus> dbProjectStatus
            , IBaseRepository<Institution> dbInstitution
            , IBaseRepository<Province> dbProvince
            , IBaseRepository<CrossMonth> dbCrossMonth
            , IBaseRepository<ProjectArea> dbProjectArea
            , IBaseRepository<ProjectOrg> dbProjectOrg
            , IBaseRepository<IndustryClassification> dbIndustryClassification
            , IBaseRepository<ProjectLeader> dbProjectLeader
            , IBaseRepository<ConstructionQualification> dbConstructionQualification
            , IBaseRepository<StagingData> dbStagingData
            , IBaseRepository<CurrencyConverter> dbCurrencyConverter
            , IBaseRepository<Domain.Models.Job> dbJob
            , IEntityChangeService entityChangeService
            , IBaseService baseService
            , IBizAuthorizeService bizAuthorizeService
            , IMapper mapper
            , GlobalObject globalObject
            , ISqlSugarClient dbContext
            , IPushPomService iPushPomService
            )
        {

            _dbProject = dbProject;
            _dbDayReport = dbDayReport;
            _dbDayReportConstruction = dbDayReportConstruction;
            _dbFile = dbFile;
            _dbcrossMonth = dbCrossMonth;
            _dbSubShip = dbSubShip;
            _dbDealingUnit = dbDealingUnit;
            _dbOwnerShip = dbOwnerShip;
            _dbProjectWBS = dbProjectWBS;
            _dbUser = dbUser;
            _dbDictionaryTable = dbDictionaryTable;
            _dbSafeSupervisionDayReport = dbSafeSupervisionDayReport;
            _dbShipDayReport = dbShipDayReport;
            _dbPortData = dbPortData;
            _dbMonthReport = dbMonthReport;
            _dbMonthReportDetail = dbMonthReportDetail;
            _dbShipDynamicMonthReport = dbShipDynamicMonthReport;
            _dbShipDynamicMonthReportDetail = dbShipDynamicMonthReportDetail;
            _dbSubShipMonthReport = dbSubShipMonthReport;
            _dbOwnerShipMonthReport = dbOwnerShipMonthReport;
            _dbOwnerShipMonthReportSoil = dbOwnerShipMonthReportSoil;
            _dbShipMovement = dbShipMovement;
            _dbShipPingType = dbShipPingType;
            _dbProjectAnnualPlan = dbProjectAnnualPlan;
            _dbCrossDateDay = dbCrossDateDay;
            _dbProjectType = dbProjectType;
            _dbProjectStatus = dbProjectStatus;
            _dbInstitution = dbInstitution;
            _dbProvince = dbProvince;
            _dbProjectArea = dbProjectArea;
            _dbProjectOrg = dbProjectOrg;
            _dbConstructionQualification = dbConstructionQualification;
            _dbIndustryClassification = dbIndustryClassification;
            _dbProjectLeader = dbProjectLeader;
            _dbStagingData = dbStagingData;
            _dbCurrencyConverter = dbCurrencyConverter;
            _dbJob = dbJob;
            _entityChangeService = entityChangeService;
            _baseService = baseService;
            _bizAuthorizeService = bizAuthorizeService;
            _mapper = mapper;
            _globalObject = globalObject;
            _dbContext = dbContext;
            _iPushPomService = iPushPomService;
        }

        #region 项目日报

        /// <summary>
        /// 新增或修改一个日报
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> SaveProjectDayReportAsync(AddOrUpdateDayReportRequestDto model)
        {
            model.ResetModelProperty();
            var result = new ResponseAjaxResult<bool>();
            if (!ConvertHelper.TryConvertDateTimeFromDateDay(model.DateDay, out DateTime dayTime))
            {
                return result.FailResult(HttpStatusCode.SaveFail, "日期时间格式不正确");
            }
            var project = await GetProjectPartAsync(model.ProjectId);
            if (project == null)
            {
                return result.FailResult(HttpStatusCode.DataNotEXIST, ResponseMessage.DATA_NOTEXIST_PROJECT);
            }
            var dayReport = await GetDayReportAsync(model.ProjectId, model.DateDay);
            if (!VerifyOfDayReport(model.DateDay, dayReport))
            {
                return result.FailResult(HttpStatusCode.NotAllowChange, ResponseMessage.NOTALLOW_CHANGE_DAYREPORT);
            }
            // 是否是新增的日报
            bool isAddedDayReport = false;
            if (dayReport == null)
            {
                isAddedDayReport = true;
                model.IsHoliday = IsHoliday(model.DateDay);
                dayReport = new DayReport() { Id = GuidUtil.Next(), ProjectId = model.ProjectId, DateDay = model.DateDay, ProcessStatus = DayReportProcessStatus.Steping, CreateId = _currentUser.Id, IsHoliday = model.IsHoliday };
                await _dbDayReport.AsInsertable(dayReport).EnableDiffLogEvent(NewLogInfo(EntityType.DayReport, dayReport.Id, ModelState.Add)).ExecuteCommandAsync();
            }
            else
            {
                dayReport.UpdateId = _currentUser.Id;
            }
            // 对象状态
            var modelState = isAddedDayReport ? ModelState.Add : ModelState.Update;
            // 步骤1 编辑施工日志
            if (model.Step == DayReportProcessStep.DayReportConstructionFinish && model.Construction != null)
            {
                // 币种/汇率
                var currencyId = model.Construction.CurrencyId;
                var currencyExchangeRate = model.Construction.CurrencyExchangeRate;
                // 重新计算施工记录日产值（元）统一单位
                foreach (var dayReportConstruction in model.Construction.DayReportConstructions)
                {
                    dayReportConstruction.ActualDailyProductionAmount = dayReportConstruction.UnitPrice * dayReportConstruction.ActualDailyProduction * currencyExchangeRate;
                }
                // 前一天日期
                var lastDateTime = dayTime.AddDays(-1);
                var lastDateDay = lastDateTime.ToDateDay();
                var startMonthTime = GetMonthStartDayTime(dayTime);
                var startYearTime = GetYearStartDayTime(dayTime);
                // 统计月份产值
                var sumMonthReport = await SumDayReportAsync(model.ProjectId, startMonthTime.ToDateDay(), lastDateDay);
                // 统计年产值
                var sumYearReport = await SumDayReportAsync(model.ProjectId, startYearTime.ToDateDay(), lastDateDay);
                // 统计全部产值
                var sumTotalDayReport = await SumDayReportAsync(model.ProjectId, DateTime.MinValue.ToDateDay(), lastDateDay);
                // 统计计划产值
                var sumMonthProjectPlanned = await SumMonthProjectPlannedAsync(model.ProjectId, dayTime);
                var addConstructions = new List<DayReportConstruction>();
                var updateConstructions = new List<DayReportConstruction>();
                var removeConstructions = new List<DayReportConstruction>();
                if (isAddedDayReport)
                {
                    addConstructions = _mapper.Map(model.Construction.DayReportConstructions, addConstructions);
                }
                else
                {
                    var reqConstructions = model.Construction.DayReportConstructions;
                    var oldDayReportConstructions = await GetDayReportConstructionsAsync(dayReport.Id);
                    var exsitConstructionIds = oldDayReportConstructions.Where(t => reqConstructions.Any(r => r.DayReportConstructionId == t.Id)).Select(t => (Guid?)t.Id).ToArray();
                    var addReqConstructions = reqConstructions.Where(t => !exsitConstructionIds.Contains(t.DayReportConstructionId)).ToList();
                    addConstructions = _mapper.Map(addReqConstructions, addConstructions);
                    removeConstructions = oldDayReportConstructions.Where(t => !exsitConstructionIds.Contains(t.Id)).ToList();
                    updateConstructions = oldDayReportConstructions.Where(t => exsitConstructionIds.Contains(t.Id)).ToList();
                }
                // 数据填充
                addConstructions.ForEach(construction =>
                {
                    construction.Id = GuidUtil.Next();
                    construction.CreateId = _currentUser.Id;
                    construction.DayReportId = dayReport.Id;
                    construction.DateDay = dayReport.DateDay;
                    construction.ProjectId = dayReport.ProjectId;
                });
                updateConstructions.ForEach(construction =>
                {
                    var updateReqConstruction = model.Construction.DayReportConstructions.Single(t => t.DayReportConstructionId == construction.Id);
                    _mapper.Map(updateReqConstruction, construction);
                    construction.UpdateId = _currentUser.Id;
                });
                removeConstructions.ForEach(construction =>
                {
                    construction.DeleteId = _currentUser.Id;
                    construction.IsDelete = 0;
                });
                _mapper.Map(model.Construction, dayReport);
                // 实际日产量/产值/外包支出
                dayReport.DayActualProduction = model.Construction.DayReportConstructions.Sum(t => t.ActualDailyProduction);
                dayReport.DayActualProductionAmount = model.Construction.DayReportConstructions.Sum(t => t.ActualDailyProductionAmount);
                dayReport.CurrencyDayProductionAmount = model.Construction.DayReportConstructions.Sum(t => t.UnitPrice * t.ActualDailyProduction);
                dayReport.OutsourcingExpensesAmount = model.Construction.DayReportConstructions.Sum(t => t.OutsourcingExpensesAmount) * currencyExchangeRate;

                // 币种/汇率
                dayReport.CurrencyId = currencyId;
                dayReport.CurrencyExchangeRate = currencyExchangeRate;
                // 月/年/累计产值(元)
                var dayProductionAmount = dayReport.DayActualProductionAmount;
                dayReport.MonthProductionAmount = sumMonthReport.ProductionAmount + dayProductionAmount;
                dayReport.YearProductionAmount = sumYearReport.ProductionAmount + dayProductionAmount;
                dayReport.CompleteAmount = sumTotalDayReport.ProductionAmount + dayProductionAmount;
                dayReport.CumulativeProductionAmount = sumTotalDayReport.ProductionAmount + dayProductionAmount;
                // 当月计划产值（元）
                dayReport.MonthPlannedProductionAmount = sumMonthProjectPlanned.PlannedOutputValue;
                if (removeConstructions.Any())
                {
                    await _dbDayReportConstruction.AsUpdateable(removeConstructions).UpdateColumns(t => new { t.IsDelete, t.DeleteId, t.DeleteTime }).EnableDiffLogEvent(NewLogInfo(EntityType.DayReport, dayReport.Id, modelState)).ExecuteCommandAsync();
                }
                if (updateConstructions.Any())
                {
                    await _dbDayReportConstruction.AsUpdateable(updateConstructions).EnableDiffLogEvent(NewLogInfo(EntityType.DayReport, dayReport.Id, modelState)).ExecuteCommandAsync();
                }
                if (addConstructions.Any())
                {

                    await _dbDayReportConstruction.AsInsertable(addConstructions).EnableDiffLogEvent(NewLogInfo(EntityType.DayReport, dayReport.Id, modelState)).ExecuteCommandAsync();
                }
                await _dbDayReport.AsUpdateable(dayReport).EnableDiffLogEvent(NewLogInfo(EntityType.DayReport, dayReport.Id, modelState)).ExecuteCommandAsync();
            }
            // 步骤2 编辑上传视频文件
            else if (model.Step == DayReportProcessStep.DayReportUploadFileFinish && model.FileInfo != null && model.FileInfo.Files != null)
            {
                var fileIds = dayReport.FileId.ObjToString().Split(",");
                var newReqFiles = model.FileInfo.Files.Where(t => !fileIds.Contains(t.FileId.ObjToString())).ToArray();
                if (newReqFiles.Any())
                {
                    await SaveFilesAsync(newReqFiles);
                }
                dayReport.FileId = string.Join(",", model.FileInfo.Files.Select(t => t.FileId.ToString()).ToArray());
                await _dbDayReport.AsUpdateable(dayReport).UpdateColumns(t => new { t.FileId, t.UpdateId, t.UpdateTime }).EnableDiffLogEvent(NewLogInfo(EntityType.DayReport, dayReport.Id, modelState)).ExecuteCommandAsync();
            }
            // 步骤3 编辑日报信息
            else if (model.Step == DayReportProcessStep.DayReportFinish && model.DayReport != null)
            {
                _mapper.Map(model.DayReport, dayReport);
                dayReport.ProcessStatus = DayReportProcessStatus.Submited;
                await _dbDayReport.AsUpdateable(dayReport).EnableDiffLogEvent(NewLogInfo(EntityType.DayReport, dayReport.Id, modelState)).ExecuteCommandAsync();
            }
            // 只有已提交的日报 做数据记录数据变更
            if (dayReport.ProcessStatus == DayReportProcessStatus.Submited)
            {
                await _entityChangeService.RecordEntitysChangeAsync(EntityType.DayReport, dayReport.Id);
            }
            return result.SuccessResult(true, ResponseMessage.OPERATION_SAVE_SUCCESS);
        }

        /// <summary>
        /// 搜索一个日报
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<ProjectDayReportResponseDto>> SearchProjectDayReportAsync(ProjectDayReportRequestDto model)
        {
            model.ResetModelProperty();
            var result = new ResponseAjaxResult<ProjectDayReportResponseDto>();
            var now = DateTime.Now;
            int dateDay = model.DateDay ?? now.AddDays(-1).ToDateDay();
            var holidayConfig = await _dbContext.Queryable<HolidayConfig>().Where(x => x.IsDelete == 1).FirstAsync();

            //上一天日期
            int lastDateDay = 0;
            if (ConvertHelper.TryConvertDateTimeFromDateDay(dateDay, out DateTime dayTime))
            {
                lastDateDay = dayTime.AddDays(-1).ToDateDay();
            }
            var project = await GetProjectPartAsync(model.ProjectId);
            if (project == null)
            {
                return result.FailResult(HttpStatusCode.DataNotEXIST, ResponseMessage.DATA_NOTEXIST_PROJECT);
            }
            var dayReport = await GetDayReportAsync(model.ProjectId, dateDay);
            var resDayReport = new ProjectDayReportResponseDto();
            // 如日报对象不存在 返回默认空日报对象
            if (dayReport == null)
            {
                //是否取前一天数据
                if (model.IsTakeBeforeDayData)
                {
                    var lastDayReport = await GetDayReportAsync(model.ProjectId, lastDateDay);
                    if (lastDayReport != null)
                    {
                        var lastDayReportConstructions = await GetDayReportConstructionsAsync(lastDayReport.Id);
                        var resLastDayConstructions = await MapResConstructionsAsync(model.ProjectId, lastDayReportConstructions);
                        resLastDayConstructions.ForEach(item =>
                        {
                            item.DayReportConstructionId = null;
                            item.ActualDailyProductionAmount = null;
                            item.ActualDailyProduction = null;
                            item.OutsourcingExpensesAmount = null;
                        });
                        _mapper.Map(lastDayReport, resDayReport.Construction);
                        resDayReport.Construction.DayReportConstructions = resLastDayConstructions.ToArray();
                    }
                }
                // 币种/汇率
                var currencyId = project.CurrencyId ?? CommonData.RMBCurrencyId;
                var currencyExchangeRate = 1m;
                if (currencyId != CommonData.RMBCurrencyId)
                {
                    currencyExchangeRate = await GetCurrencyRateAsync(currencyId);
                }
                resDayReport.Construction.CurrencyId = currencyId;
                resDayReport.Construction.CurrencyExchangeRate = currencyExchangeRate;
                resDayReport.Construction.IsShowWorkStatusOfSpringFestival = IsShowWorkStatusOfSpringFestival(dayTime);
                resDayReport.CreateUserName = _currentUser.Name;
                resDayReport.IsHoliday = holidayConfig.IsHoliday == 1; //true;// IsHoliday(dateDay);

            }
            else
            {

                // 步骤一（施工日志） 数据填充
                var dayReportConstructions = await GetDayReportConstructionsAsync(dayReport.Id);
                var resConstructions = await MapResConstructionsAsync(model.ProjectId, dayReportConstructions);
                _mapper.Map(dayReport, resDayReport.Construction);
                resDayReport.Construction.DayReportConstructions = resConstructions.ToArray();
                // 步骤二（影像文件） 数据填充
                if (!string.IsNullOrWhiteSpace(dayReport.FileId))
                {
                    var fileIds = dayReport.FileId.Split(",");
                    var findFileIds = dayReport.FileId.Split(",").Where(t => Guid.TryParse(t, out Guid fileId)).Select(t => t.ToGuid()).ToArray();
                    var files = await GetFilesAsync(findFileIds);
                    var resFiles = new List<ProjectDayReportResponseDto.ResFileInfo>();
                    foreach (var fileId in fileIds)
                    {
                        var file = files.FirstOrDefault(t => t.Id.ToString() == fileId);
                        var resFile = new ProjectDayReportResponseDto.ResFileInfo()
                        {
                            FileId = file?.Id,
                            Name = file?.Name,
                            OriginName = file?.OriginName,
                            SuffixName = file?.SuffixName,
                            Url = GetFullFileUrl(file)
                        };
                        resFiles.Add(resFile);
                    }
                    resDayReport.FileInfo.Files = resFiles.ToArray();
                }
                // 步骤三（日报信息）-填充
                _mapper.Map(dayReport, resDayReport.DayReport);
                if (dayReport.CreateId != null)
                {
                    var user = await GetUserPartAsync((Guid)dayReport.CreateId);
                    resDayReport.CreateUserName = user?.Name;
                }
                resDayReport.ProcessStatus = dayReport.ProcessStatus;
                resDayReport.IsHoliday = holidayConfig.IsHoliday == 1;// true; //dayReport.IsHoliday;
            }

            #region 修改汇率
            if (resDayReport != null && resDayReport.Construction != null)
            {
                if (resDayReport.Construction.CurrencyId != Guid.Empty && resDayReport.Construction.CurrencyId != CommonData.RMBCurrencyId)
                {
                    resDayReport.Construction.CurrencyExchangeRate = await GetCurrencyRateAsync(resDayReport.Construction.CurrencyId);
                }
            }
            #endregion

            // 统计月度计划产值
            var sumMonthProjectPlanned = await SumMonthProjectPlannedAsync(model.ProjectId, dayTime);
            resDayReport.Construction.DayPlannedProductionAmount = Math.Round(sumMonthProjectPlanned.PlannedOutputValue / 30.5m, 2);
            resDayReport.DateDay = dateDay;
            resDayReport.ProjectId = model.ProjectId;
            resDayReport.ProjectName = project.Name;
            resDayReport.ProjectCategory = project.Category;
            resDayReport.IsCanSubmit = VerifyOfDayReport(dateDay, dayReport);
            return result.SuccessResult(resDayReport);
        }

        /// <summary>
        /// 填充施工记录
        /// </summary>
        private async Task<List<ProjectDayReportResponseDto.ResConstruction>> MapResConstructionsAsync(Guid projectId, List<DayReportConstruction> dayReportConstructions)
        {
            if (!dayReportConstructions.Any())
            {
                return new List<ProjectDayReportResponseDto.ResConstruction>();
            }
            var resConstructions = _mapper.Map<List<ProjectDayReportResponseDto.ResConstruction>>(dayReportConstructions);
            var ownerShipIds = resConstructions.Where(t => t.OwnerShipId != null).Select(t => (Guid)t.OwnerShipId).ToArray();
            var subShipIds = resConstructions.Where(t => t.SubShipId != null).Select(t => (Guid)t.SubShipId).ToArray();
            var projectWBSIds = resConstructions.Where(t => t.ProjectWBSId != null).Select(t => (Guid)t.ProjectWBSId).ToArray();
            var ownerShipParts = await GetShipResourcesAsync(ownerShipIds, ConstructionOutPutType.Self);
            ownerShipParts.AddRange(await GetShipResourcesAsync(ownerShipIds, ConstructionOutPutType.SubPackage));
            var subShipParts = await GetShipResourcesAsync(subShipIds, ConstructionOutPutType.SubPackage);
            var projectWBSParts = await GetProjectWBSListAsync(projectId);
            resConstructions.ForEach(item =>
            {
                if (item.OwnerShipId != null)
                {
                    item.OwnerShipName = ownerShipParts.FirstOrDefault(t => t.ShipId == item.OwnerShipId)?.ShipName;
                }
                if (item.SubShipId != null)
                {
                    item.SubShipName = subShipParts.FirstOrDefault(t => t.ShipId == item.SubShipId)?.ShipName;
                }
                if (item.ProjectWBSId != null)
                {
                    var thisProjectWBSPart = projectWBSParts.FirstOrDefault(t => t.Id == (Guid)item.ProjectWBSId);
                    if (thisProjectWBSPart != null)
                    {
                        item.ProjectWBSName = GetProjectWBSLevelName(projectWBSParts, thisProjectWBSPart.Name, thisProjectWBSPart.Pid);
                        item.ProjectWBSLevel1Name = GetProjectWBSLevel1Name(projectWBSParts, thisProjectWBSPart);
                    }
                }
            });
            return resConstructions;
        }

        /// <summary>
        /// 验证填报日期
        /// </summary>
        /// <returns></returns>
        private bool VerifyOfDayReport(int dateDay, DayReport? report)
        {
            var now = DateTime.Now;
            // 填报日期大于下个月时间日期节点则验证失败
            if (dateDay > now.AddMonths(1).ToDateDay())
            {
                return false;
            }
            // 如果存在日报 与 日报填报时间小于今天 与 创建时间小于今天则验证时验证失败
            //if (report != null && report.ProcessStatus == DayReportProcessStatus.Submited && dateDay < now.ToDateDay() && (report.CreateTime == null || report.CreateTime.Value.Date < now.Date))
            //{
            //    return false;
            //}
            return true;
        }

        /// <summary>
        /// 是否显示，春节停工计划 
        /// <para>备注1：true:表示当前时间在12月23-1月15范围内，false ：不在12月23-1月15范围内</para>
        /// <para>备注2：true：显示 【春节停工计划 】,fasle：不显示【春节停工计划】 </para>
        /// </summary>
        private bool IsShowWorkStatusOfSpringFestival(DateTime dayTime)
        {
            var chineseDate = dayTime.ToChineseDate();
            if (chineseDate >= new DateTime(chineseDate.Year, 12, 23) && chineseDate < new DateTime(chineseDate.Year + 1, 1, 1))
            {
                return true;
            }
            else if (chineseDate >= new DateTime(chineseDate.Year, 1, 1) && chineseDate < new DateTime(chineseDate.Year, 1, 16))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 是否是节假日
        /// </summary>
        /// <returns></returns>
        private bool IsHoliday(int dateDay)
        {
            var isHoliday = _dbCrossDateDay.GetFirst(t => t.DateDay == dateDay)?.IsHoliday;
            return isHoliday ?? false;
        }

        /// <summary>
        /// 搜索未填日报的项目列表
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<UnReportProjectResponseDto>>> SearchUnReportProjectsAsync(UnReportProjectsRequestDto model)
        {
            model.ResetModelProperty();
            var result = new ResponseAjaxResult<List<UnReportProjectResponseDto>>();
            var startDay = ((DateTime)model.StartTime).ToDateDay();
            var endDay = ((DateTime)model.EndTime).ToDateDay();
            //获取当前用户授权数据
            var userAuthForData = await GetCurrentUserAuthForDataAsync();
            //解析属性标签
            ResolveTagNames(model.TagName, out List<int> categoryList, out List<int> tagList, out List<int> tag2List);
            RefAsync<int> total = 0;
            var list = new List<UnReportProjectResponseDto>();
            var query = _dbProject.AsQueryable()
                .InnerJoin(_dbCrossDateDay.AsQueryable(), (p, c) => c.DateDay >= startDay && c.DateDay <= endDay)
                .LeftJoin(_dbDayReport.AsQueryable(), (p, c, d) => p.Id == d.ProjectId && c.DateDay == d.DateDay && d.DateDay >= startDay && d.DateDay <= endDay && d.IsDelete == 1)
                .LeftJoin<ProjectChangeRecord>((p, c, d, r) => p.Id == r.ProjectId && c.DateDayTime >= r.StartTime && c.DateDayTime < r.EndTime)
                .Where((p, c, d) => p.IsDelete == 1 && CommonData.ConstructionStatus.Contains(p.StatusId))
                .WhereIF(!userAuthForData.IsAdmin, (p, c, d) => userAuthForData.CompanyIds.Contains(p.ProjectDept))
                .WhereIF(tagList.Any(), (p, c, d) => tagList.Contains(p.Tag))
                .WhereIF(tag2List.Any(), (p, c, d) => tag2List.Contains(p.Tag2))
                .WhereIF(categoryList.Any(), (p, c, d) => categoryList.Contains(p.Category))
                .WhereIF(model.CompanyId != null, (p, c, d) => p.CompanyId == model.CompanyId)
                .WhereIF(model.ProjectDept != null, (p, c, d) => p.ProjectDept == model.ProjectDept)
                .WhereIF(model.ProjectStatusId != null && model.ProjectStatusId.Any(), (p, c, d) => model.ProjectStatusId.Contains(p.StatusId))
                .WhereIF(model.ProjectRegionId != null, (p, c, d) => p.RegionId == model.ProjectRegionId)
                .WhereIF(model.ProjectTypeId != null, (p, c, d) => p.TypeId == model.ProjectTypeId)
                .WhereIF(!string.IsNullOrWhiteSpace(model.ProjectName), (p, c, d) => SqlFunc.Contains(p.Name, model.ProjectName))
                .Select((p, c, d, r) => new UnReportProjectResponseDto
                {
                    ProjectId = p.Id,
                    ProjectName = p.Name,
                    ProjectTypeId = p.TypeId == null ? Guid.Empty : p.TypeId,
                    Category = p.Category,
                    CompanyId = p.CompanyId,
                    StatusId = p.StatusId,
                    DateDay = c.DateDay,
                    CreateTime = p.CreateTime,
                    DayReportId = d.Id,
                    ChangeStateId = r.Id
                });
            query = _dbContext.Queryable(query).Where(t => t.DayReportId == null && t.ProjectTypeId != CommonData.NoConstrutionProjectType && t.ChangeStateId == null).OrderBy(t => new { t.ProjectId, t.DateDay });
            //list = model.IsFullExport ? await query.ToListAsync() : await query.ToPageListAsync(model.PageIndex, model.PageSize, total);
            //获取停工记录
            var resultList = await query.ToListAsync();
            var unList = new List<UnReportProjectResponseDto>();
            total = resultList.Where(x => x.CreateTime != null && x.DateDay >= x.CreateTime.Value.ToDateDay()).ToList().Count;
            total += resultList.Where(x => x.CreateTime == null).ToList().Count();
            var valueList = resultList.Where(x => x.CreateTime != null && x.DateDay >= x.CreateTime.Value.ToDateDay()).ToList();
            unList.AddRange(valueList);
            var createTimeNullList = resultList.Where(x => x.CreateTime == null).ToList();
            unList.AddRange(createTimeNullList);
            unList = model.IsFullExport ? unList : unList.Skip((model.PageIndex - 1) * model.PageSize).Take(model.PageSize).ToList();
            var companyIds = unList.Select(t => t.CompanyId).Distinct().ToArray();
            var companys = await GetInstitutionsAsync(companyIds);
            var projectTypes = await GetProjectTypesAsync();
            var projectStatusList = await GetProjectStatusListAsync();
            unList.ForEach(item =>
            {
                item.CompanyName = companys.FirstOrDefault(t => t.PomId == item.CompanyId)?.Name;
                item.ProjectTypeName = projectTypes.FirstOrDefault(t => t.PomId == item.ProjectTypeId)?.Name;
                item.StatusName = projectStatusList.FirstOrDefault(t => t.StatusId == item.StatusId)?.Name;
            });
            return result.SuccessResult(unList, total);
        }

        //获取停工记录  暂时不用
        public async Task<List<UnReportProjectResponseDto>> GetUnReportList(List<UnReportProjectResponseDto> list, int startDay, int endDay)
        {
            var changeList = await _dbContext.Queryable<ProjectChangeRecord>().ToListAsync();
            var projectIds = list.Select(x => x.ProjectId).GroupBy(x => x).ToList();
            var dayList = await _dbContext.Queryable<CrossDateDay>().Where(x => x.DateDay >= startDay && x.DateDay <= endDay).ToListAsync();
            var resultList = new List<UnReportProjectResponseDto>();
            foreach (var item in projectIds)
            {
                List<int> timeList = new List<int>();
                var change = changeList.Where(x => x.ProjectId == item.Key).ToList();
                foreach (var item2 in change)
                {
                    //获取该项目停工的日期段
                    var timeSlot = dayList.Where(x => x.DateDayTime >= item2.StartTime && x.DateDayTime < item2.EndTime).Select(x => x.DateDay).ToList();
                    timeList.AddRange(timeSlot);
                }
                var value = list.Where(x => x.ProjectId == item.Key && !timeList.Contains(x.DateDay)).ToList();
                resultList.AddRange(value);
            }
            return resultList;
        }


        /// <summary>
        /// 获取一个日报
        /// </summary>
        /// <param name="projectId">项目Id</param>
        /// <param name="dateDay">日期</param>
        /// <returns></returns>
        private async Task<DayReport?> GetDayReportAsync(Guid projectId, int dateDay)
        {
            return await _dbDayReport.AsQueryable().Where(t => t.ProjectId == projectId && t.DateDay == dateDay && t.IsDelete == 1).FirstAsync();
        }


        /// <summary>
        /// 获取日报-施工日志集合
        /// </summary>
        /// <param name="dayReportId">日报id</param>
        /// <returns></returns>
        private async Task<List<DayReportConstruction>> GetDayReportConstructionsAsync(Guid dayReportId)
        {
            return await _dbDayReportConstruction.AsQueryable().Where(t => t.DayReportId == dayReportId && t.IsDelete == 1).ToListAsync();
        }

        /// <summary>
        ///项目-统计
        /// </summary>
        /// <returns></returns>
        private async Task<SumDayReportDto> SumDayReportAsync(Guid projectId, int startDateDay, int endDateDay)
        {
            var sumDayReport = await _dbDayReport.AsQueryable().Where(t => t.ProjectId == projectId && t.ProcessStatus == DayReportProcessStatus.Submited && t.DateDay >= startDateDay && t.DateDay <= endDateDay && t.IsDelete == 1)
                .GroupBy(t => t.ProjectId).Select(t => new SumDayReportDto()
                {
                    ProductionAmount = SqlFunc.AggregateSum(t.DayActualProductionAmount),
                }).FirstAsync();
            sumDayReport = sumDayReport ?? new SumDayReportDto();
            sumDayReport.ProjectId = projectId;
            return sumDayReport;
        }

        /// <summary>
        ///项目-资源-统计
        /// </summary>
        /// <returns></returns>
        private async Task<List<SumDayReportDto>> SumDayReportBySourceAsync(Guid projectId, int startDateDay, int endDateDay)
        {
            var query = _dbDayReportConstruction.AsQueryable()
                .InnerJoin(_dbDayReport.AsQueryable(), (t, d) => t.DayReportId == d.Id && d.ProcessStatus == DayReportProcessStatus.Submited && d.IsDelete == 1)
                .Where((t, d) => d.ProjectId == projectId && d.DateDay >= startDateDay && d.DateDay <= endDateDay && t.IsDelete == 1)
                .Select((t, d) => new { t.ProjectId, t.ProjectWBSId, t.OutPutType, ShipId = (t.OutPutType == ConstructionOutPutType.SubPackage ? t.SubShipId : t.OwnerShipId), t.ConstructionNature, t.ActualDailyProduction, t.ActualDailyProductionAmount, t.OutsourcingExpensesAmount, t.UnitPrice });
            return await _dbContext.Queryable(query).GroupBy(c => new { c.ProjectId, c.ProjectWBSId, c.OutPutType, c.ShipId, c.UnitPrice, c.ConstructionNature })
                .Select(c => new SumDayReportDto()
                {
                    ProjectId = c.ProjectId,
                    ProjectWBSId = c.ProjectWBSId,
                    ShipId = c.ShipId,
                    OutPutType = c.OutPutType,
                    UnitPrice = c.UnitPrice,
                    ConstructionNature = c.ConstructionNature,
                    OutsourcingExpensesAmount = SqlFunc.AggregateSum(c.OutsourcingExpensesAmount),
                    Production = SqlFunc.AggregateSum(c.ActualDailyProduction)
                }).ToListAsync();
        }

        #endregion

        #region 安监日报

        /// <summary>
        /// 新增或修改一个安监日报
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> SaveSafeSupervisionDayReportAsync(AddOrUpdateSafeSupervisionDayReportRequestDto model)
        {
            model.ResetModelProperty();
            var result = new ResponseAjaxResult<bool>();
            var project = await GetProjectPartAsync(model.ProjectId);
            if (project == null)
            {
                return result.FailResult(HttpStatusCode.DataNotEXIST, ResponseMessage.DATA_NOTEXIST_PROJECT);
            }
            var safeDayReport = await GetSafeSupervisionDayReportAsync(model.ProjectId, model.DateDay);
            if (!VerifyOfSafeDayReport(model.DateDay, safeDayReport))
            {
                return result.FailResult(HttpStatusCode.NotAllowChange, ResponseMessage.NOTALLOW_CHANGE_SAFEDAYREPORT);
            }
            bool isAddedSafeDayReport = false;
            if (safeDayReport == null)
            {
                safeDayReport = new SafeSupervisionDayReport()
                {
                    Id = GuidUtil.Next(),
                    CreateId = _currentUser.Id
                };
                isAddedSafeDayReport = true;
            }
            else
            {
                safeDayReport.UpdateId = _currentUser.Id;
            }
            // 对象状态
            _mapper.Map(model, safeDayReport);
            if (isAddedSafeDayReport)
            {
                await _dbSafeSupervisionDayReport.AsInsertable(safeDayReport).EnableDiffLogEvent(NewLogInfo(EntityType.SafeDayReport, safeDayReport.Id, ModelState.Add)).ExecuteCommandAsync();
            }
            else
            {
                await _dbSafeSupervisionDayReport.AsUpdateable(safeDayReport).EnableDiffLogEvent(NewLogInfo(EntityType.SafeDayReport, safeDayReport.Id, ModelState.Update)).ExecuteCommandAsync();
            }
            //记录安监变更
            await _entityChangeService.RecordEntitysChangeAsync(EntityType.SafeDayReport, safeDayReport.Id);
            return result.SuccessResult(true, ResponseMessage.OPERATION_SAVE_SUCCESS);
        }

        /// <summary>
        /// 搜索一个安监日报
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<SafeSupervisionDayReportResponseDto>> SearchSafeSupervisionDayReportAsync(SafeSupervisionDayReportRequestDto model)
        {
            model.ResetModelProperty();
            var result = new ResponseAjaxResult<SafeSupervisionDayReportResponseDto>();
            int dateDay = model.DateDay ?? DateTime.Now.AddDays(-1).ToDateDay();
            //上一天日期
            int lastDateDay = 0;
            if (ConvertHelper.TryConvertDateTimeFromDateDay(dateDay, out DateTime time))
            {
                lastDateDay = time.AddDays(-1).ToDateDay();
            }
            var resSafeDayReport = new SafeSupervisionDayReportResponseDto();
            var safeDayReport = await GetSafeSupervisionDayReportAsync(model.ProjectId, dateDay);
            // 如日报对象不存在 返回默认空对象
            if (safeDayReport == null)
            {
                // 是否取前一天数据
                if (model.IsTakeBeforeDayData)
                {
                    var lastSafeDayReport = await GetSafeSupervisionDayReportAsync(model.ProjectId, lastDateDay);
                    if (lastSafeDayReport != null)
                    {
                        _mapper.Map(lastSafeDayReport, resSafeDayReport);
                    }
                }
                resSafeDayReport.ProjectId = model.ProjectId;
                resSafeDayReport.DateDay = dateDay;
            }
            else
            {
                _mapper.Map(safeDayReport, resSafeDayReport);
            }
            resSafeDayReport.IsCanSubmit = VerifyOfSafeDayReport(dateDay, safeDayReport);
            return result.SuccessResult(resSafeDayReport);
        }


        /// <summary>
        /// 验证安监填报日期
        /// </summary>
        /// <returns></returns>
        private bool VerifyOfSafeDayReport(int dateDay, SafeSupervisionDayReport? report)
        {
            var now = DateTime.Now;
            // 填报日期大于下个月时间日期节点则验证失败
            if (dateDay > now.AddMonths(1).ToDateDay())
            {
                return false;
            }
            // 如果存在日报 与 日报填报时间小于今天 与 创建时间小于今天则验证时验证失败
            //if (report != null && dateDay < now.ToDateDay() && (report.CreateTime == null || report.CreateTime.Value.Date < now.Date))
            //{
            //    return false;
            //}
            return true;
        }

        /// <summary>
        /// 搜索未填安监日报的项目列表
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<UnReportProjectResponseDto>>> SearchUnReportSafeProjectsAsync(UnReportProjectsRequestDto model)
        {
            model.ResetModelProperty();
            var result = new ResponseAjaxResult<List<UnReportProjectResponseDto>>();
            var startDay = ((DateTime)model.StartTime).ToDateDay();
            var endDay = ((DateTime)model.EndTime).ToDateDay();
            //获取当前用户授权数据
            var userAuthForData = await GetCurrentUserAuthForDataAsync();
            //解析属性标签
            ResolveTagNames(model.TagName, out List<int> categoryList, out List<int> tagList, out List<int> tag2List);
            RefAsync<int> total = 0;
            var list = new List<UnReportProjectResponseDto>();
            var query = _dbProject.AsQueryable()
                .InnerJoin(_dbCrossDateDay.AsQueryable(), (p, c) => c.DateDay >= startDay && c.DateDay <= endDay)
                .LeftJoin(_dbSafeSupervisionDayReport.AsQueryable(), (p, c, d) => p.Id == d.ProjectId && c.DateDay == d.DateDay && d.DateDay >= startDay && d.DateDay <= endDay && d.IsDelete == 1)
                .LeftJoin<ProjectChangeRecord>((p, c, d, r) => p.Id == r.ProjectId && c.DateDayTime >= r.StartTime && c.DateDayTime < r.EndTime)
                .Where((p, c, d) => p.IsDelete == 1 && CommonData.ConstructionStatus.Contains(p.StatusId))
                .WhereIF(!userAuthForData.IsAdmin, (p, c, d) => userAuthForData.CompanyIds.Contains(p.ProjectDept))
                .WhereIF(tagList.Any(), (p, c, d) => tagList.Contains(p.Tag))
                .WhereIF(tag2List.Any(), (p, c, d) => tag2List.Contains(p.Tag2))
                .WhereIF(categoryList.Any(), (p, c, d) => categoryList.Contains(p.Category))
                .WhereIF(model.CompanyId != null, (p, c, d) => p.CompanyId == model.CompanyId)
                .WhereIF(model.ProjectDept != null, (p, c, d) => p.ProjectDept == model.ProjectDept)
                .WhereIF(model.ProjectStatusId != null && model.ProjectStatusId.Any(), (p, c, d) => model.ProjectStatusId.Contains(p.StatusId))
                .WhereIF(model.ProjectRegionId != null, (p, c, d) => p.RegionId == model.ProjectRegionId)
                .WhereIF(model.ProjectTypeId != null, (p, c, d) => p.TypeId == model.ProjectTypeId)
                .WhereIF(!string.IsNullOrWhiteSpace(model.ProjectName), (p, c, d) => SqlFunc.Contains(p.Name, model.ProjectName))
                .Select((p, c, d, r) => new UnReportProjectResponseDto
                {
                    ProjectId = p.Id,
                    ProjectName = p.Name,
                    ProjectTypeId = p.TypeId == null ? Guid.Empty : p.TypeId,
                    Category = p.Category,
                    CompanyId = p.CompanyId,
                    StatusId = p.StatusId,
                    DateDay = c.DateDay,
                    CreateTime = p.CreateTime,
                    DayReportId = d.Id,
                    ChangeStateId = r.Id
                });
            query = _dbContext.Queryable(query).Where(t => t.DayReportId == null && t.ProjectTypeId != CommonData.NoConstrutionProjectType && t.ChangeStateId == null).OrderBy(t => new { t.ProjectId, t.DateDay });
            //list = model.IsFullExport ? await query.ToListAsync() : await query.ToPageListAsync(model.PageIndex, model.PageSize, total);
            //获取停工记录
            var resultList = await query.ToListAsync();
            var unList = new List<UnReportProjectResponseDto>();
            total = resultList.Where(x => x.CreateTime != null && x.DateDay >= x.CreateTime.Value.ToDateDay()).ToList().Count;
            total += resultList.Where(x => x.CreateTime == null).ToList().Count();
            var valueList = resultList.Where(x => x.CreateTime != null && x.DateDay >= x.CreateTime.Value.ToDateDay()).ToList();
            unList.AddRange(valueList);
            var createTimeNullList = resultList.Where(x => x.CreateTime == null).ToList();
            unList.AddRange(createTimeNullList);

            unList = model.IsFullExport ? unList : unList.Skip((model.PageIndex - 1) * model.PageSize).Take(model.PageSize).ToList();
            var companyIds = unList.Select(t => t.CompanyId).Distinct().ToArray();
            var companys = await GetInstitutionsAsync(companyIds);
            var projectTypes = await GetProjectTypesAsync();
            var projectStatusList = await GetProjectStatusListAsync();
            unList.ForEach(item =>
            {
                item.CompanyName = companys.FirstOrDefault(t => t.PomId == item.CompanyId)?.Name;
                item.ProjectTypeName = projectTypes.FirstOrDefault(t => t.PomId == item.ProjectTypeId)?.Name;
                item.StatusName = projectStatusList.FirstOrDefault(t => t.StatusId == item.StatusId)?.Name;
            });
            return result.SuccessResult(unList, total);
        }

        /// <summary>
        /// 获取一个安监日报
        /// </summary>
        /// <param name="projectId">项目Id</param>
        /// <param name="dateDay">日期</param>
        /// <returns></returns>
        private async Task<SafeSupervisionDayReport?> GetSafeSupervisionDayReportAsync(Guid projectId, int dateDay)
        {
            return await _dbSafeSupervisionDayReport.GetFirstAsync(t => t.ProjectId == projectId && t.DateDay == dateDay && t.IsDelete == 1);
        }

        #endregion

        #region 船舶日报(包含项目-船舶日报，无项目-船舶动态日报)

        /// <summary>
        /// 新增或修改一个船舶日报
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> SaveShipDayReportAsync(SaveShipDayReportRequestDto model)
        {
            model.ResetModelProperty();
            var result = new ResponseAjaxResult<bool>();
            if (!ConvertHelper.TryConvertDateTimeFromDateDay(model.DateDay, out DateTime dayTime))
            {
                return result.FailResult(HttpStatusCode.SaveFail, "日期时间格式不正确");
            }
            var ship = await GetOwnerShipAsync(model.ShipId);
            if (ship == null)
            {
                return result.FailResult(HttpStatusCode.DataNotEXIST, ResponseMessage.DATA_NOTEXIST_OWNERSHIP);
            }
            var shipDayReport = await GetShipDayReportAsync(model.ShipId, model.DateDay);
            if (!VerifyOfShipDayReport(model.DateDay, shipDayReport))
            {
                return result.FailResult(HttpStatusCode.NotAllowChange, ResponseMessage.NOTALLOW_CHANGE_SHIPDAYREPORT);
            }
            var isAddedShipDayReport = false;
            var oldProjectId = shipDayReport?.ProjectId;
            if (shipDayReport == null)
            {
                if (model.ShipDayReportType == ShipDayReportType.ProjectShip)
                {
                    var project = await GetProjectPartAsync((Guid)model.ProjectId);
                    if (project == null)
                    {
                        return result.FailResult(HttpStatusCode.DataNotEXIST, ResponseMessage.DATA_NOTEXIST_PROJECT);
                    }
                }
                shipDayReport = new ShipDayReport()
                {
                    Id = GuidUtil.Next(),
                    CreateId = _currentUser.Id
                };
                isAddedShipDayReport = true;
            }
            else
            {
                shipDayReport.UpdateId = _currentUser.Id;
            }
            _mapper.Map(model, shipDayReport);
            if (isAddedShipDayReport)
            {
                await _dbShipDayReport.AsInsertable(shipDayReport).EnableDiffLogEvent(NewLogInfo(EntityType.ShipDayReport, shipDayReport.Id, ModelState.Add)).ExecuteCommandAsync();
            }
            else
            {
                await _dbShipDayReport.AsUpdateable(shipDayReport).EnableDiffLogEvent(NewLogInfo(EntityType.ShipDayReport, shipDayReport.Id, ModelState.Update)).ExecuteCommandAsync();
            }
            // 如果当前船舶的状态不等于填报的船舶状态则更新
            if (ship.ShipState != shipDayReport.ShipState)
            {
                ship.ShipState = shipDayReport.ShipState;
                await _dbOwnerShip.AsUpdateable(ship).UpdateColumns(t => new { t.ShipState }).ExecuteCommandAsync();
            }

            // 联动变更自有船舶月报运转时间
            if (shipDayReport.ShipDayReportType == ShipDayReportType.ProjectShip)
            {
                await ChangeOwnerShipMonthReportByShipDayReportAsync(shipDayReport, dayTime, oldProjectId);
            }
            //船舶变更记录
            await _entityChangeService.RecordEntitysChangeAsync(EntityType.ShipDayReport, shipDayReport.Id);
            return result.SuccessResult(true, ResponseMessage.OPERATION_SAVE_SUCCESS);
        }

        /// <summary>
        /// 联动变更船舶月报 运转时间
        /// </summary>
        /// <param name="shipDayReport">船舶日报</param>
        /// <param name="dayTime">填报日期</param>
        /// <param name="oldProjectId">存在的项目Id（如果船舶日报对应的项目变更，对应的项目自有船舶月报都需要重新计算运转时间）</param>
        /// <returns></returns>
        private async Task ChangeOwnerShipMonthReportByShipDayReportAsync(ShipDayReport shipDayReport, DateTime dayTime, Guid? oldProjectId)
        {
            var monthTime = GetReportMonthTime(dayTime);
            var lastDateMonthTime = monthTime.AddMonths(-1);
            var startDateDay = new DateTime(lastDateMonthTime.Year, lastDateMonthTime.Month, 26).ToDateDay();
            var endDateDay = new DateTime(monthTime.Year, monthTime.Month, 25).ToDateDay();
            var monthReport = await GetOwnerShipMonthReportAsync(shipDayReport.ProjectId, shipDayReport.ShipId, monthTime.ToDateMonth());
            //项目信息
            var projectList = await _dbContext.Queryable<Project>().Where(x => x.IsDelete == 1).ToListAsync();
            var sipMovementList = await _dbContext.Queryable<ShipMovement>().Where(x => x.IsDelete == 1).ToListAsync();
            if (monthReport != null)
            {
                var sumShipDayReport = await SumShipDayReportAsync(shipDayReport.ProjectId, shipDayReport.ShipId, startDateDay, endDateDay, projectList, sipMovementList);
                if (monthReport.WorkingHours != sumShipDayReport.WorkingHours)
                {
                    monthReport.WorkingHours = sumShipDayReport.WorkingHours;
                    await _dbOwnerShipMonthReport.AsUpdateable(monthReport).UpdateColumns(t => new { t.WorkingHours }).ExecuteCommandAsync();
                    await _entityChangeService.RecordEntitysChangeAsync(EntityType.OwnerShipMonthReport, monthReport.Id);
                }

            }
            // 如果项目Id变更，则原有的项目自有船舶月报运转时间也重新计算
            if (oldProjectId != null && oldProjectId != shipDayReport.ProjectId)
            {
                var oldMonthReport = await GetOwnerShipMonthReportAsync((Guid)oldProjectId, shipDayReport.ShipId, monthTime.ToDateMonth());
                if (oldMonthReport != null)
                {
                    var sumShipDayReport = await SumShipDayReportAsync((Guid)oldProjectId, shipDayReport.ShipId, startDateDay, endDateDay, projectList, sipMovementList);
                    if (oldMonthReport.WorkingHours != sumShipDayReport.WorkingHours)
                    {
                        oldMonthReport.WorkingHours = sumShipDayReport.WorkingHours;
                        await _dbOwnerShipMonthReport.AsUpdateable(oldMonthReport).UpdateColumns(t => new { t.WorkingHours }).ExecuteCommandAsync();
                        await _entityChangeService.RecordEntitysChangeAsync(EntityType.OwnerShipMonthReport, oldMonthReport.Id);
                    }
                }
            }
        }

        /// <summary>
        /// 搜索一个船舶日报
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<ShipDayReportResponseDto>> SearchShipDayReportAsync(ShipDayReportRequestDto model)
        {
            model.ResetModelProperty();
            var result = new ResponseAjaxResult<ShipDayReportResponseDto>();
            var ship = await GetShipPartAsync(model.ShipId, ShipType.OwnerShip);
            if (ship == null)
            {
                return result.FailResult(HttpStatusCode.DataNotEXIST, ResponseMessage.DATA_NOTEXIST_OWNERSHIP);
            }
            int dateDay = model.DateDay ?? DateTime.Now.AddDays(-1).ToDateDay();
            //上一天日期
            int lastDateDay = 0;
            if (ConvertHelper.TryConvertDateTimeFromDateDay(dateDay, out DateTime time))
            {
                lastDateDay = time.AddDays(-1).ToDateDay();
            }
            var resShipDayReport = new ShipDayReportResponseDto();
            var shipDayReport = await GetShipDayReportAsync(model.ShipId, dateDay);
            // 如日报对象不存在 返回默认空日报对象
            if (shipDayReport == null)
            {
                //是否取前一天数据
                if (model.IsTakeBeforeDayData)
                {
                    var lastShipDayReport = await GetShipDayReportAsync(model.ShipId, lastDateDay);
                    if (lastShipDayReport != null)
                    {
                        _mapper.Map(lastShipDayReport, resShipDayReport);
                    }
                }
            }
            else
            {
                _mapper.Map(shipDayReport, resShipDayReport);
                if (shipDayReport.ProjectId != model.ProjectId)
                {
                    var differentProjectName = shipDayReport.ShipDayReportType == ShipDayReportType.DynamicShip ? "非关联项目" : (await GetProjectPartAsync(shipDayReport.ProjectId))?.Name;
                    resShipDayReport.HasWaning = true;
                    resShipDayReport.WaningMessage = $"{ship.Name}船舶日报({time.ToString("yyyy年MM月dd日")})已在{differentProjectName}上填报，保存后将被覆盖！";
                }
            }

            resShipDayReport.ProjectId = model.ProjectId;
            resShipDayReport.DateDay = dateDay;
            resShipDayReport.ShipId = model.ShipId;
            resShipDayReport.ShipDayReportType = model.ShipDayReportType;
            resShipDayReport.ShipName = ship.Name;
            resShipDayReport.ShipKindTypeId = ship.ShipKindTypeId;
            resShipDayReport.CabinCapacityOrDouRong = shipDayReport == null ? null : shipDayReport.CabinCapacityOrDouRong;
            resShipDayReport.ShipKindTypeName = resShipDayReport.ShipKindTypeId == null ? null : (await GetShipPingTypeAsync((Guid)resShipDayReport.ShipKindTypeId))?.Name;
            resShipDayReport.PortName = resShipDayReport.PortId == null ? null : (await GetPortDatasPartAsync(resShipDayReport.PortId.ToString()))?.Name;
            resShipDayReport.IsCanSubmit = VerifyOfShipDayReport(dateDay, shipDayReport);
            return result.SuccessResult(resShipDayReport);
        }

        /// <summary>
        /// 验证船舶填报日期
        /// </summary>
        /// <returns></returns>
        private bool VerifyOfShipDayReport(int dateDay, ShipDayReport? report)
        {
            var now = DateTime.Now;
            // 填报日期大于下个月时间日期节点则验证失败
            if (dateDay > now.AddMonths(1).ToDateDay())
            {
                return false;
            }
            // 如果存在日报 与 日报填报时间小于今天 与 创建时间小于今天则验证时验证失败
            //if (report != null && dateDay < now.ToDateDay() && (report.CreateTime == null || report.CreateTime.Value.Date < now.Date))
            //{
            //    return false;
            //}
            return true;
        }

        /// <summary>
        /// 搜索未填船舶日报的船舶列表
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<UnReportShipResponseDto>>> SearchUnReportShipsAsync(UnReportShipsRequestDto model)
        {
            model.ResetModelProperty();
            var result = new ResponseAjaxResult<List<UnReportShipResponseDto>>();
            var startDay = ((DateTime)model.StartTime).ToDateDay();
            var endDay = ((DateTime)model.EndTime).ToDateDay();
            //获取当前用户授权数据
            var userAuthForData = await GetCurrentUserAuthForDataAsync();
            //解析属性标签
            ResolveTagNames(model.TagName, out List<int> categoryList, out List<int> tagList, out List<int> tag2List);
            RefAsync<int> total = 0;
            var list = new List<UnReportShipResponseDto>();
            var query = _dbOwnerShip.AsQueryable()
                .LeftJoin(_dbShipMovement.AsQueryable(), (s, m) => s.PomId == m.ShipId && m.ShipType == ShipType.OwnerShip && (m.Status == ShipMovementStatus.Enter || m.Status == ShipMovementStatus.Quit) && m.IsDelete == 1)
                .InnerJoin(_dbCrossDateDay.AsQueryable(), (s, m, c) => c.DateDay >= startDay && c.DateDay <= endDay)
                .Where((s, m, c) => s.IsDelete == 1)
                .Select((s, m, c) =>
                new UnReportShipResponseDto
                {
                    ShipId = s.PomId,
                    ShipName = s.Name,
                    ShipKindTypeId = s.TypeId,
                    DateDay = c.DateDay,
                    NoDateTime = c.DateDayTime,
                    EnterTime = ((m.Status == ShipMovementStatus.Enter && m.EnterTime <= c.DateDayTime) || (m.Status == ShipMovementStatus.Quit && m.EnterTime <= c.DateDayTime && m.QuitTime >= c.DateDayTime)) ? m.EnterTime : null,
                    ProjectId = ((m.Status == ShipMovementStatus.Enter && m.EnterTime <= c.DateDayTime) || (m.Status == ShipMovementStatus.Quit && m.EnterTime <= c.DateDayTime && m.QuitTime >= c.DateDayTime)) ? m.ProjectId : Guid.Empty
                }).Distinct();
            query = _dbContext.Queryable(query)
               .LeftJoin(_dbProject.AsQueryable(), (q, p) => q.ProjectId == p.Id && p.IsDelete == 1)
               .LeftJoin(_dbShipDayReport.AsQueryable(), (q, p, d) => q.ShipId == d.ShipId && q.ProjectId == d.ProjectId && q.DateDay == d.DateDay && d.DateDay >= startDay && d.DateDay <= endDay && d.IsDelete == 1)
               .WhereIF(!userAuthForData.IsAdmin, (q, p, d) => userAuthForData.CompanyIds.Contains(p.ProjectDept))
               .WhereIF(tagList.Any(), (q, p, d) => tagList.Contains(p.Tag))
               .WhereIF(tag2List.Any(), (q, p, d) => tag2List.Contains(p.Tag2))
               .WhereIF(model.ShipTypeId != null, (q, p, d) => q.ShipKindTypeId == model.ShipTypeId)
               .WhereIF(categoryList.Any(), (q, p, d) => categoryList.Contains(p.Category))
               .WhereIF(model.CompanyId != null, (q, p, d) => p.CompanyId == model.CompanyId)
               .WhereIF(model.ProjectDept != null, (q, p, d) => p.ProjectDept == model.ProjectDept)
               .WhereIF(model.ProjectStatusId != null && model.ProjectStatusId.Any(), (q, p, d) => model.ProjectStatusId.Contains(p.StatusId))
               .WhereIF(model.ProjectRegionId != null, (q, p, d) => p.RegionId == model.ProjectRegionId)
               .WhereIF(model.ProjectTypeId != null, (q, p, d) => p.TypeId == model.ProjectTypeId)
               .WhereIF(model.ShipPingId != null, (q, p, d) => q.ShipId == model.ShipPingId)
               .WhereIF(!string.IsNullOrWhiteSpace(model.ProjectName), (q, p, d) => SqlFunc.Contains(p.Name, model.ProjectName))
               .Select((q, p, d) => new UnReportShipResponseDto
               {
                   ShipId = q.ShipId,
                   ShipName = q.ShipName,
                   ShipKindTypeId = q.ShipKindTypeId,
                   EnterTime = q.EnterTime,
                   ProjectId = q.ProjectId,
                   DateDay = q.DateDay,
                   NoDateTime = q.NoDateTime,
                   ProjectTypeId = p.TypeId == null ? Guid.Empty : p.TypeId,
                   ProjectName = p.Name,
                   DayReportId = d.Id,
                   CreateTime = p.CreateTime
               });
            query = _dbContext.Queryable(query).LeftJoin<ProjectChangeRecord>((t, e) => t.ProjectId == e.ProjectId && t.NoDateTime >= e.StartTime && t.NoDateTime < e.EndTime).
                Where((t, e) => (t.DayReportId == null && e.Id == null) && t.ProjectTypeId != CommonData.NoConstrutionProjectType).OrderBy(t => new { t.ShipId, DateDay = SqlFunc.Desc(t.DateDay) });
            //获取停工记录
            var resultList = await query.ToListAsync();
            var unList = new List<UnReportShipResponseDto>();
            total = resultList.Where(x => x.CreateTime != null && x.NoDateTime >= x.CreateTime).ToList().Count;
            total += resultList.Where(x => x.CreateTime == null).ToList().Count();
            var valueList = resultList.Where(x => x.CreateTime != null && x.NoDateTime >= x.CreateTime).ToList();
            unList.AddRange(valueList);
            var createTimeNullList = resultList.Where(x => x.CreateTime == null).ToList();
            unList.AddRange(createTimeNullList);
            var emptyGuidList = unList.Where(x => x.ProjectId == Guid.Empty).ToList();
            var removeList = new List<UnReportShipResponseDto>();
            foreach (var item in emptyGuidList)
            {
                if (unList.Any(t => t.ShipId == item.ShipId && t.DateDay == item.DateDay && t.ProjectId != Guid.Empty))
                {
                    removeList.Add(item);
                }
            }
            unList = unList.Where(t => !removeList.Contains(t)).ToList();
            total = unList.Count;
            unList = model.IsFullExport ? unList : unList.Skip((model.PageIndex - 1) * model.PageSize).Take(model.PageSize).ToList();
            var shipKindTypeIds = unList.Select(t => t.ShipKindTypeId).Distinct().ToArray();
            var shipKindTypes = await GetShipPingTypesAsync(shipKindTypeIds);
            unList.ForEach(item =>
            {
                item.ShipKindTypeName = shipKindTypes.FirstOrDefault(t => t.PomId == item.ShipKindTypeId)?.Name;
                item.ShipDayReportType = item.ProjectId == Guid.Empty || item.ProjectId == null ? ShipDayReportType.DynamicShip : ShipDayReportType.ProjectShip;
            });
            return result.SuccessResult(unList, total);
        }

        /// <summary>
        /// 获取一个船舶日报
        /// </summary>
        /// <returns></returns>
        private async Task<ShipDayReport?> GetShipDayReportAsync(Guid shipId, int dateDay)
        {
            return await _dbShipDayReport.GetFirstAsync(t => t.ShipId == shipId && t.DateDay == dateDay && t.IsDelete == 1);
        }

        /// <summary>
        /// 获取船舶日报集合
        /// </summary>
        /// <param name="projectId">项目Id</param>
        /// <param name="shipId">船舶Id</param>
        /// <param name="startDateDay">起始日期</param>
        /// <param name="endDateDay">结束日期</param>
        /// <returns></returns>
        private async Task<List<ShipDayReport>> GetShipDayReportsAsync(Guid projectId, Guid shipId, int startDateDay, int endDateDay)
        {
            return await _dbShipDayReport.GetListAsync(t => t.ProjectId == projectId && t.ShipId == shipId && t.DateDay >= startDateDay && t.DateDay <= endDateDay && t.IsDelete == 1);
        }

        /// <summary>
        /// 项目-自有船舶-统计
        /// </summary>
        /// <returns></returns>
        private async Task<SumShipReportDto> SumShipDayReportAsync(Guid projectId, Guid shipId, int startDateDay, int endDateDay, List<Project> projects, List<ShipMovement> shipMovements)
        {
            var num = 0M;
            var sumReport = await _dbShipDayReport.AsQueryable().Where(t => t.ProjectId == projectId && t.ShipId == shipId && t.DateDay >= startDateDay && t.DateDay <= endDateDay && t.IsDelete == 1)
                .GroupBy(t => new { t.ProjectId, t.ShipId }).Select(t => new SumShipReportDto()
                {
                    // 生产运转
                    WorkingHours = SqlFunc.AggregateSum(t.Dredge ?? 0) + SqlFunc.AggregateSum(t.Sail ?? 0) + SqlFunc.AggregateSum(t.BlowingWater ?? 0) + SqlFunc.AggregateSum(t.SedimentDisposal ?? 0) + SqlFunc.AggregateSum(t.BlowShore ?? 0)
                }).FirstAsync();


            #region 新逻辑
            if (projects != null && projects.Any() && shipMovements != null && shipMovements.Any())
            {
                //新逻辑
                var list = await _dbShipDayReport.AsQueryable().Where(t => t.ProjectId == Guid.Empty && t.ShipId == shipId && t.DateDay >= startDateDay && t.DateDay <= endDateDay && t.IsDelete == 1).ToListAsync();
                var primaryIds = list.Where(t => t.ProjectId == Guid.Empty).Select(t => new { id = t.Id, shipId = t.ShipId, DateDay = t.DateDay, ProjectId = t.ProjectId }).ToList();
                foreach (var item in primaryIds)
                {
                    var isExistProject = shipMovements.Where(x => x.IsDelete == 1 && x.ShipId == item.shipId && x.EnterTime.HasValue == true && (x.EnterTime.Value.ToDateDay() <= item.DateDay || x.QuitTime.HasValue == true && x.QuitTime.Value.ToDateDay() >= item.DateDay)).OrderByDescending(x => x.EnterTime).ToList();
                    foreach (var project in isExistProject)
                    {

                        if (project.QuitTime.HasValue && project.QuitTime.Value.ToDateDay() >= item.DateDay)
                        {

                            var oldValue = list.Where(t => t.ShipId == project.ShipId && t.DateDay == item.DateDay).FirstOrDefault();
                            if (oldValue != null)
                            {
                                oldValue.ProjectId = project.ProjectId;

                            }
                        }
                        if (project.EnterTime.HasValue && project.QuitTime.HasValue == false && project.EnterTime.Value.ToDateDay() <= item.DateDay)
                        {

                            var oldValue = list.Where(t => t.ShipId == project.ShipId && t.DateDay == item.DateDay).FirstOrDefault();
                            if (oldValue != null)
                            {
                                oldValue.ProjectId = project.ProjectId;

                            }
                        }
                        if (project.EnterTime.HasValue && project.QuitTime.HasValue && project.QuitTime.Value.ToDateDay() >= item.DateDay)
                        {

                            var oldValue = list.Where(t => t.ShipId == project.ShipId && t.DateDay == item.DateDay).FirstOrDefault();
                            if (oldValue != null)
                            {
                                oldValue.ProjectId = project.ProjectId;

                            }
                        }
                    }

                }

                foreach (var item in list)
                {
                    num += ((item.Dredge ?? 0) + (item.Sail ?? 0) + (item.BlowingWater ?? 0) + (item.SedimentDisposal ?? 0) + (item.BlowingWater ?? 0));
                }
            }
            if (sumReport != null)
            {
                sumReport.WorkingHours += num;
            }

            #endregion


            sumReport = sumReport ?? new SumShipReportDto();
            sumReport.ProjectId = projectId;
            sumReport.ShipId = shipId;
            sumReport.ShipType = ShipType.OwnerShip;
            return sumReport;
        }

        #endregion


        #region 项目月报

        #region 修改推送项目月报特殊字段

        /// <summary>
        /// 修改项目月报特殊字段推送
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> UpdatePushMonthReportSpeailFiledAsync(ModifyPushMonthReportSpecialFieldsRequestDto requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<bool>();
            //数据读取
            var isExistResult = await _dbContext.Queryable<MonthReport>().Where(x => x.IsDelete == 1 && x.Id == requestDto.MonthReportId).FirstAsync();
            if (isExistResult != null)
            {
                isExistResult.ProgressDeviationDescriptionPushPom = requestDto.ProgressDeviationDescriptionPushPom;
                isExistResult.ProgressDescriptionPushPom = requestDto.ProgressDescriptionPushPom;
                isExistResult.IsPushPom = isExistResult.IsPushPom + 1;
                await _dbMonthReport.AsUpdateable(isExistResult).EnableDiffLogEvent(NewLogInfo(EntityType.MonthReport, isExistResult.Id, ModelState.Update)).ExecuteCommandAsync();
                // 记录对象变更
                await _entityChangeService.RecordEntitysChangeAsync(EntityType.MonthReport, isExistResult.Id);
                _iPushPomService.PushSpecialFieldsMonthReportsAsync(isExistResult.Id);
                responseAjaxResult.SuccessResult(true);
            }
            else
            {
                responseAjaxResult.FailResult(HttpStatusCode.UpdateFail, "数据不存在，修改失败");
            }
            return responseAjaxResult;
        }

        #endregion

        #region 项目月报列表

        /// <summary>
        /// 解析标签名称集合
        /// </summary>
        private void ResolveTagNames(string[]? tagNames, out List<int> categoryList, out List<int> tagList, out List<int> tag2List)
        {
            categoryList = new List<int>();
            tagList = new List<int>();
            tag2List = new List<int>();
            if (tagNames != null)
            {
                foreach (var item in tagNames)
                {
                    switch (item)
                    {
                        case "境内":
                            categoryList.Add(0);
                            break;
                        case "境外":
                            categoryList.Add(1);
                            break;
                        case "传统":
                            tagList.Add(0);
                            break;
                        case "新兴":
                            tagList.Add(1);
                            break;
                        case "现汇":
                            tag2List.Add(0);
                            break;
                        case "投资":
                            tag2List.Add(1);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 搜索项目月报列表
        /// </summary>
        public async Task<ResponseAjaxResult<MonthtReportsResponseDto>> SearchMonthReportsAsync(MonthtReportsRequstDto model)
        {
            if (model.IsDuiWai)
            {
                _currentUser.Account = "2022002867";
                _currentUser.CurrentLoginDepartmentId = "bd840460-1e3a-45c8-abed-6e66903e6465".ToGuid();
                _currentUser.CurrentLoginInstitutionId = "bd840460-1e3a-45c8-abed-6e66903e6465".ToGuid();
                _currentUser.CurrentLoginInstitutionOid = "101162350";
                _currentUser.CurrentLoginInstitutionPoid = "101114066";
                _currentUser.CurrentLoginRoleId = "08db268c-d0d0-4e0f-8a66-39ff15bd3865".ToGuid();
                _currentUser.CurrentLoginIsAdmin = true;
                _currentUser.CurrentLoginUserType = 1;
                _currentUser.Id = "08d63666-7e36-4e20-8024-ee9c96c51623".ToGuid();
            }
            #region 时间判断
            //查询当前是第几月
            var nowYear = DateTime.Now.Year;
            var nowMonth = DateTime.Now.Month;
            var nowDay = DateTime.Now.Day;
            var monthDay = 0;
            if (nowDay > 26)
            {
                nowMonth += 1;
            }
            if (int.Parse(DateTime.Now.ToString("MMdd")) > 1226)
            {
                nowYear += 1;
            }
            if (nowMonth.ToString().Length == 1)
            {
                monthDay = int.Parse(nowYear + $"0{nowMonth}");
            }
            else
            {
                monthDay = int.Parse(nowYear + $"{nowMonth}");
            }
            #endregion

            model.ResetModelProperty();
            var result = new ResponseAjaxResult<MonthtReportsResponseDto>();
            List<string> userId = new List<string>()
            {
                "L20080287",//赵锐
				"2016042370",//杨加录
				"2020012309", //李倩
				"2017005354", //孟浩
				"2018009624", //王琦
				"2018008722",//陈河元
				"2019013759", //朱智文
				"2016146340"//陈翠
			};
            var startTime = (DateTime)model.StartTime;
            var endTime = (DateTime)model.EndTime;
            var startMonth = startTime.ToDateMonth();
            var endMonth = endTime.ToDateMonth();
            var userAuthForData = await GetCurrentUserAuthForDataAsync();
            //解析属性标签
            ResolveTagNames(model.TagName, out List<int> categoryList, out List<int> tagList, out List<int> tag2List);
            RefAsync<int> total = 0;
            var query = _dbMonthReport.AsQueryable().LeftJoin(_dbProject.AsQueryable(), (m, p) => m.ProjectId == p.Id && p.IsDelete == 1);
            if (!model.IsDuiWai)//非对外 监控中心自己使用
            {
                query = MonthReportsWheres(query, model, startMonth, endMonth, userAuthForData, categoryList, tagList, tag2List)
                        .OrderBy((m, p) => new { m.ProjectId, m.DateMonth });
            }
            else
            {
                //对外接口使用
                query = MonthReportsWheres(query, model, userAuthForData, categoryList, tagList, tag2List)
                        .OrderBy((m, p) => new { m.ProjectId, m.DateMonth });
            }

            var selQuery = query.Select((m, p) => new MonthtReportsResponseDto.MonthtReportDto()
            {
                Id = m.Id,
                ProjectId = m.ProjectId,
                DateMonth = m.DateMonth,
                AccomplishQuantities = SqlFunc.Round(m.CompletedQuantity, 2),
                AccomplishValue = model.IsConvert == true ? SqlFunc.Round(m.CompleteProductionAmount, 2) : SqlFunc.Round(m.CurrencyCompleteProductionAmount, 2),
                RecognizedValue = SqlFunc.Round(m.PartyAConfirmedProductionAmount, 2),
                PaymentAmount = m.PartyAPayAmount,
                ProgressDeviationReasonType = m.ProgressDeviationReason,
                CostDeviationReasonType = m.CostDeviationReason,
                CostDeviationDescription = m.CostDeviationDescription,
                ProjectedCost = SqlFunc.Round(m.MonthEstimateCostAmount, 2),
                Name = p.Name,
                ProjectTypeId = p.TypeId,
                ExchangeRate = p.ExchangeRate,
                ConstructionQualificationId = p.ProjectConstructionQualificationId,
                Tag = p.Tag,
                Tag2 = p.Tag2,
                Category = p.Category,
                CompanyId = p.CompanyId,
                AreaId = p.AreaId,
                RegionId = p.RegionId,
                StatusId = p.StatusId,
                ClassifyStandard = p.ClassifyStandard,
                ContractAmount = p.Amount,//合同额
                ValidContractAmount = p.ECAmount,//有效合同额
                Remarks = p.QuantityRemarks,
                BudgetInterestRate = p.BudgetInterestRate * 100,
                BudgetaryReasons = p.BudgetaryReasons,
                CompilationTime = p.CompilationTime,
                AdministrationNumber = p.Administrator,
                ConstructionNumber = p.Constructor,
                PersonReportForms = p.ReportFormer,
                PhoneReportForms = p.ReportForMertel,
                Code = p.Code, //项目编码
                ContractMeaPayProp = p.ContractMeaPayProp, //比例
                TaxRate = p.Rate * 100,
                ProgressDescription = m.ProgressDescription,//形象进度描述
                ProgressDeviationDescription = m.ProgressDeviationDescription,//进度偏差原因
                ProgressDeviationDescriptionPushPom = m.ProgressDeviationDescriptionPushPom,//进度偏差原因（pom）
                ProgressDescriptionPushPom = m.ProgressDescriptionPushPom,//形象进度描述（pom）
                PushStatus = m.IsPushPom,//推送次数
                StatusText = m.StatusText,
                OutsourcingExpensesAmount = m.OutsourcingExpensesAmount,
                UpdateTime = m.UpdateTime,
                CreateTime = m.CreateTime
            });

            var list = model.IsFullExport ? await selQuery.ToListAsync() : await selQuery.ToPageListAsync(model.PageIndex, model.PageSize, total);
            if (!model.IsDuiWai)
            {
                list = list
                .Where(x => string.IsNullOrEmpty(x.UpdateTime.ToString()) || x.UpdateTime == DateTime.MinValue ?
                  x.CreateTime >= model.StartTime && x.CreateTime <= model.EndTime
                : x.UpdateTime >= model.StartTime && x.UpdateTime <= model.EndTime)
                .ToList();
            }

            var projectIds = list.Select(t => t.ProjectId).Distinct().ToArray();
            var companyIds = list.Select(t => t.CompanyId).Distinct().ToArray();
            var regionIds = list.Select(t => t.RegionId).Distinct().ToArray();
            var areaIds = list.Select(t => t.AreaId).Distinct().ToArray();
            var companys = await GetInstitutionsAsync(companyIds);
            // 项目状态
            var projectStatusList = await GetProjectStatusListAsync();
            // 施工资质
            var constructionQualifications = await GetConstructionQualificationsAsync();
            // 项目类型
            var projectTypes = await GetProjectTypesAsync();
            // 项目所属区域
            var projectAreas = await GetProjectAreasAsync();
            // 施工地点
            var areas = await GetProvincesAsync(areaIds);
            // 省地区
            var provinceAreas = await GetProvincesByLevelAsync(1);
            // 市地区
            var districtAreas = await GetProvincesByLevelAsync(2);
            // 项目干系单位
            var projectOrgs = await GetProjectOrgsAsync(projectIds);
            var organizationIds = projectOrgs.Select(t => t.OrganizationId).Distinct().ToArray();
            // 项目干系单位-往来单位
            var organizationUnits = await GetDealingUnitPartsAsync(organizationIds);
            // 项目负责人
            var projectLeaders = await GetProjectLeadersAsync(projectIds);
            var userIds = projectLeaders.Select(t => t.AssistantManagerId).Distinct().ToArray();
            var projectLeaderUsers = await GetUserPartAsync(userIds);
            // 行业分类
            var industryClassifications = await GetIndustryClassificationsAsync();
            // 年度计划产值/产量
            var years = GetPlanYears(startTime, endTime);
            var projectAnnualPlans = await GetProjectAnnualPlansAsync(projectIds, years);
            // 累计产值/产量
            var sumMonthReports = await SumMonthReportsAsync(projectIds);
            //项目历史确认和收款产值
            var sumHistoryMonthReports = await _dbContext.Queryable<ProjectMonthReportHistory>().Where(x => x.IsDelete == 1 && projectIds.Contains(x.ProjectId.Value)).ToListAsync();

            // 年度累计产值/产量
            var sumYearMonthReports = await SumMonthReportsByYearAsync(projectIds, startTime.Year, endTime.Year);
            // 年度计划
            list.ForEach(item =>
            {
                if (item.StatusText == "一级审批中" && (nowDay >= 26 || nowDay <= 1))
                {
                    item.IsShowRecall = item.DateMonth == monthDay;
                }

                var thisMonthTime = item.DateMonthTime;
                var area = areas.FirstOrDefault(t => t.PomId == item.AreaId);
                var thispProjectOrgs = projectOrgs.Where(t => t.ProjectId == item.ProjectId).ToList();
                // 业主单位
                var ownerUnit = organizationUnits.FirstOrDefault(t => t.PomId == thispProjectOrgs.FirstOrDefault(i => i.Type == "1")?.OrganizationId);
                // 甲方单位
                var PartyAUnit = organizationUnits.FirstOrDefault(t => t.PomId == thispProjectOrgs.FirstOrDefault(i => i.Type == "2")?.OrganizationId);
                // 乙方单位
                var PartyBUnit = organizationUnits.FirstOrDefault(t => t.PomId == thispProjectOrgs.FirstOrDefault(i => i.Type == "3")?.OrganizationId);
                // 项目负责人
                var projectLeader = projectLeaderUsers.FirstOrDefault(t => t.PomId == projectLeaders.FirstOrDefault(i => i.ProjectId == item.ProjectId)?.AssistantManagerId);
                // 月度项目计划（产值/产量）
                var sumProjectMonthPlanned = SumMonthProjectPlanned(projectAnnualPlans, item.ProjectId, thisMonthTime);
                // 年度项目计划（产值/产量）
                var sumProjectYearPlanned = SumYearProjectPlannedAsync(projectAnnualPlans, item.ProjectId, thisMonthTime.Year);
                // 项目月报累计统计
                var sumMonthReport = sumMonthReports.FirstOrDefault(t => t.ProjectId == item.ProjectId);
                // 项目月报年度累计统计
                var sumYearMonthReport = sumYearMonthReports.FirstOrDefault(t => t.ProjectId == item.ProjectId && t.DateYear == thisMonthTime.Year);
                item.IsAdmin = userId.Contains(_currentUser.Account) ? true : false;
                item.State = projectStatusList.FirstOrDefault(t => t.StatusId == item.StatusId)?.Name;
                item.Construction = constructionQualifications.FirstOrDefault(t => t.PomId == item.ConstructionQualificationId)?.Name;
                item.ProjectType = projectTypes.FirstOrDefault(t => t.PomId == item.ProjectTypeId)?.Name;
                item.Area = projectAreas.FirstOrDefault(t => t.AreaId == item.RegionId)?.Name;
                item.OwnerUnitName = ownerUnit?.ZBPNAME_ZH;
                item.FirstPartyUnit = PartyAUnit?.ZBPNAME_ZH;
                item.SecondPartyName = PartyBUnit?.ZBPNAME_ZH;
                item.PersonAdministration = projectLeader?.Name;
                item.PhoneAdministration = projectLeader?.Phone;
                item.TweUnit = "中交广州航道局有限公司";
                item.ThreeUnit = companys.FirstOrDefault(t => t.PomId == item.CompanyId)?.Name;
                // 计划产值/工程量
                item.PlanQuantities = Math.Round(sumProjectMonthPlanned.PlannedQuantities, 2);
                item.PlanOutPutValue = Math.Round(sumProjectMonthPlanned.PlannedOutputValue, 2);
                item.YearPlanQuantities = Math.Round(sumProjectYearPlanned.PlannedQuantities, 2);
                item.YearPlanOutPutValue = Math.Round(sumProjectYearPlanned.PlannedOutputValue, 2);
                // 合同金额
                if (item.ContractAmount != null)
                {
                    item.ContractAmount = Math.Round((decimal)(model.IsConvert == true ? item.ContractAmount * (item.ExchangeRate ?? 1m) : item.ContractAmount), 2);
                }
                // 有效合同额
                if (item.ValidContractAmount != null)
                {
                    item.ValidContractAmount = Math.Round((decimal)item.ValidContractAmount, 2);
                }
                // 年度统计月报
                if (sumYearMonthReport != null)
                {
                    item.YearAccomplishQuantities = Math.Round(sumYearMonthReport.CompletedQuantity, 2);
                    item.YearAccomplishCost = Math.Round(sumYearMonthReport.CostAmount, 2);
                    item.YearAccomplishValue = Math.Round(model.IsConvert == true ? sumYearMonthReport.CompleteProductionAmount : sumYearMonthReport.CurrencyCompleteProductionAmount, 2);
                    item.YearPaymentAmount = Math.Round(sumYearMonthReport.PartyAPayAmount, 2);
                    item.YearRecognizedValue = Math.Round(sumYearMonthReport.PartyAConfirmedProductionAmount, 2);
                    item.YearProjectedCost = Math.Round(sumYearMonthReport.EstimateCostAmount, 2);
                    item.YearOutsourcingExpensesAmount = Math.Round(sumYearMonthReport.OutsourcingExpensesAmount, 2);
                }
                // 累计统计月报
                if (sumMonthReport != null)
                {
                    item.AccumulativeQuantities = Math.Round(sumMonthReport.CompletedQuantity, 2);
                    item.CumulativeAccomplishCost = Math.Round(sumMonthReport.CostAmount, 2);
                    item.CumulativeCompleted = Math.Round(model.IsConvert == true ? sumMonthReport.CompleteProductionAmount : sumMonthReport.CurrencyCompleteProductionAmount, 2);
                    //item.CumulativePaymentAmount = Math.Round(sumMonthReport.PartyAPayAmount, 2);
                    // item.CumulativeValue = Math.Round(sumMonthReport.PartyAConfirmedProductionAmount, 2);
                    var itemHistotyMonth = sumHistoryMonthReports.Where(x => x.ProjectId == item.ProjectId).FirstOrDefault();

                    if (itemHistotyMonth != null)
                    {
                        item.CumulativePaymentAmount = Math.Round(sumMonthReport.PartyAPayAmount, 2) + Math.Round(itemHistotyMonth.KaileiProjectPayment.Value * 10000, 2);

                        item.CumulativeValue = Math.Round(sumMonthReport.PartyAConfirmedProductionAmount, 2) +
                        Math.Round(itemHistotyMonth.KaileiOwnerConfirmation.Value * 10000, 2);
                    }
                    else
                    {
                        item.CumulativePaymentAmount = Math.Round(sumMonthReport.PartyAPayAmount, 2);
                        item.CumulativeValue = Math.Round(sumMonthReport.PartyAConfirmedProductionAmount, 2);
                    }

                    item.CumulativeOutsourcingExpensesAmount = Math.Round(sumMonthReport.OutsourcingExpensesAmount, 2);

                }
                // (国别|省）/市
                if (area != null)
                {
                    item.Nationality = area.Overseas == "1" ? area.Zaddvsname : GetProvinceArea(provinceAreas, districtAreas, area)?.Zaddvsname;
                    item.District = GetDistrictArea(districtAreas, area)?.Zaddvsname;
                }
                // 行业分类
                if (!string.IsNullOrWhiteSpace(item.ClassifyStandard))
                {
                    var classifyStandardIds = item.ClassifyStandard.Split(",").Select(t => t.ToGuid()).ToArray();
                    if (classifyStandardIds.Length > 0)
                    {
                        var classification = industryClassifications.FirstOrDefault(t => t.PomId == classifyStandardIds[0]);
                        item.OneClassify = classification?.Name;
                        item.OneCode = classification?.Code;
                    }
                    if (classifyStandardIds.Length > 1)
                    {
                        var classification = industryClassifications.FirstOrDefault(t => t.PomId == classifyStandardIds[1]);
                        item.TweClassify = classification?.Name;
                        item.TweCode = classification?.Code;
                    }
                    if (classifyStandardIds.Length > 2)
                    {
                        var classification = industryClassifications.FirstOrDefault(t => t.PomId == classifyStandardIds[2]);
                        item.ThreeClassify = classification?.Name;
                        item.ThreeCode = classification?.Code;
                    }
                }
                // 标后预算预计成本
                item.PostmarkProjectedCost = Math.Round(item.CumulativeCompleted * ((100 - item.BudgetInterestRate ?? 0) / 100), 2);
                // 支付滞后比例(支付滞后比例 = 合同约定的计量支付比例-实际支付比例,   实际支付比例=甲方付款金额/甲方确认产值)
                if (item.RecognizedValue != 0)
                {
                    item.PaymentLagRatio = Math.Round(item.ContractMeaPayProp - ((item.PaymentAmount / item.RecognizedValue) * 100), 2);
                }
                // 实际毛利率
                if (item.CumulativeCompleted > 0)
                {
                    item.EffectiveGrossMargin = Math.Round(((item.CumulativeCompleted - item.CumulativeAccomplishCost) / item.CumulativeCompleted) * 100, 2);
                }
                // 剩余合同金额
                item.ResidueContractAmount = Math.Round((item.ContractAmount ?? 0) - item.CumulativeCompleted, 2);
                // 偏差计算
                //   本月进度偏差  
                item.DeviationQuantities = Math.Round(item.PlanQuantities - item.AccomplishQuantities, 2); ;
                //   年度进度偏差
                item.YearDeviation = Math.Round(item.YearPlanQuantities - item.YearAccomplishQuantities, 2);
                //   本月产值进度偏差 
                item.ScheduleOutPutValue = Math.Round(item.PlanOutPutValue - item.AccomplishValue, 2);
                //   年度产值偏差 
                item.YearAnnualOutputValue = Math.Round(item.YearPlanOutPutValue - item.YearAccomplishValue, 2);
                //   本月产值进度偏差
                item.CostDeviation = Math.Round(item.ProjectedCost - item.AccomplishCost, 2); ;
                //   年度成本偏差
                item.YearCostDeviation = Math.Round(item.YearProjectedCost - item.YearAccomplishCost, 2);
                //   标后预算成本偏差
                item.PostmarkCostDeviation = Math.Round(item.PostmarkProjectedCost - item.CumulativeAccomplishCost, 2);
                //   毛利率偏差
                if (item.BudgetInterestRate != null && item.BudgetInterestRate != 0)
                {
                    item.GrossMarginDeviation = Math.Round(((item.EffectiveGrossMargin - (decimal)item.BudgetInterestRate) / (decimal)item.BudgetInterestRate) * 100, 2);
                }
            });

            //  合计
            var totalQuery = _dbMonthReport.AsQueryable().LeftJoin(_dbProject.AsQueryable(), (m, p) => m.ProjectId == p.Id && p.IsDelete == 1);
            totalQuery = MonthReportsWheres(totalQuery, model, startMonth, endMonth, userAuthForData, categoryList, tagList, tag2List);
            var totalMonthtReport = await totalQuery.Select((m, p) => new MonthtReportsResponseDto.TotalMonthtReportsDto
            {
                AccomplishQuantities = SqlFunc.Round(SqlFunc.AggregateSum(m.CompletedQuantity), 2),
                AccomplishValue = SqlFunc.Round(SqlFunc.AggregateSum(m.CompleteProductionAmount), 2),
                RecognizedValue = SqlFunc.Round(SqlFunc.AggregateSum(m.PartyAConfirmedProductionAmount), 2),
                PaymentAmount = SqlFunc.Round(SqlFunc.AggregateSum(m.PartyAPayAmount), 2),
                OutsourcingExpensesAmount = SqlFunc.Round(SqlFunc.AggregateSum(m.OutsourcingExpensesAmount), 2)

            }).FirstAsync();
            totalMonthtReport = totalMonthtReport ?? new MonthtReportsResponseDto.TotalMonthtReportsDto();
            return result.SuccessResult(new MonthtReportsResponseDto() { Reports = list, Total = totalMonthtReport }, total);
        }

        /// <summary>
        /// 月报列表筛选条件
        /// </summary>
        /// <returns></returns>
        private ISugarQueryable<MonthReport, Project> MonthReportsWheres(ISugarQueryable<MonthReport, Project> query, MonthtReportsRequstDto model, int startMonth, int endMonth, UserAuthForDataDto userAuthForData, List<int> categoryList, List<int> tagList, List<int> tag2List)
        {
            return query.Where((m, p) => m.DateMonth >= startMonth && m.DateMonth <= endMonth)
                  .WhereIF(!userAuthForData.IsAdmin, (m, p) => userAuthForData.CompanyIds.Contains(p.ProjectDept))
                  .WhereIF(model.CompanyId != null, (m, p) => p.CompanyId == model.CompanyId)
                  .WhereIF(model.ProjectDept != null, (m, p) => p.ProjectDept == model.ProjectDept)
                  .WhereIF(model.ProjectStatusId != null && model.ProjectStatusId.Any(), (m, p) => model.ProjectStatusId.Contains(p.StatusId))
                  .WhereIF(model.ProjectRegionId != null, (m, p) => p.RegionId == model.ProjectRegionId)
                  .WhereIF(model.ProjectTypeId != null, (m, p) => p.TypeId == model.ProjectTypeId)
                  .WhereIF(!string.IsNullOrWhiteSpace(model.ProjectName), (m, p) => SqlFunc.Contains(p.Name, model.ProjectName))
                  .WhereIF(model.ProjectAreaId! != null, (m, p) => p.AreaId == model.ProjectAreaId)
                  .WhereIF(tagList.Any(), (m, p) => tagList.Contains(p.Tag))
                  .WhereIF(tag2List.Any(), (m, p) => tag2List.Contains(p.Tag2))
                  .WhereIF(categoryList.Any(), (m, p) => categoryList.Contains(p.Category))
                  .WhereIF(model.IsMajor == true, (m, p) => p.IsMajor == 1)
                  .WhereIF(model.Status != null, (m, p) => m.Status == model.Status);
        }

        /// <summary>
        /// 月报列表筛选条件（对外接口调用时使用）
        /// </summary>
        /// <returns></returns>
        private ISugarQueryable<MonthReport, Project> MonthReportsWheres(ISugarQueryable<MonthReport, Project> query, MonthtReportsRequstDto model, UserAuthForDataDto userAuthForData, List<int> categoryList, List<int> tagList, List<int> tag2List)
        {
            return query
              //.Where((m, p) => m.DateMonth >= startMonth && m.DateMonth <= endMonth)
              .WhereIF(!userAuthForData.IsAdmin, (m, p) => userAuthForData.CompanyIds.Contains(p.ProjectDept))
              .WhereIF(model.CompanyId != null, (m, p) => p.CompanyId == model.CompanyId)
              .WhereIF(model.ProjectDept != null, (m, p) => p.ProjectDept == model.ProjectDept)
              .WhereIF(model.ProjectStatusId != null && model.ProjectStatusId.Any(), (m, p) => model.ProjectStatusId.Contains(p.StatusId))
              .WhereIF(model.ProjectRegionId != null, (m, p) => p.RegionId == model.ProjectRegionId)
              .WhereIF(model.ProjectTypeId != null, (m, p) => p.TypeId == model.ProjectTypeId)
              .WhereIF(!string.IsNullOrWhiteSpace(model.ProjectName), (m, p) => SqlFunc.Contains(p.Name, model.ProjectName))
              .WhereIF(model.ProjectAreaId! != null, (m, p) => p.AreaId == model.ProjectAreaId)
              .WhereIF(tagList.Any(), (m, p) => tagList.Contains(p.Tag))
              .WhereIF(tag2List.Any(), (m, p) => tag2List.Contains(p.Tag2))
              .WhereIF(categoryList.Any(), (m, p) => categoryList.Contains(p.Category))
              .WhereIF(model.IsMajor == true, (m, p) => p.IsMajor == 1)
              .WhereIF(model.Status != null, (m, p) => m.Status == model.Status);
        }


        /// <summary>
        /// 获取年度计划集合
        /// </summary>
        /// <returns></returns>
        private async Task<SumProjectPlannedDto> SumMonthProjectPlannedAsync(Guid projectId, DateTime dayTime)
        {
            var monthTime = dayTime.Day < 26 ? new DateTime(dayTime.Year, dayTime.Month, 1) : new DateTime(dayTime.AddMonths(1).Year, dayTime.AddMonths(1).Month, 1);
            var years = GetPlanYears(monthTime, monthTime);
            var plans = await GetProjectAnnualPlansAsync(new Guid[] { projectId }, years);
            return SumMonthProjectPlanned(plans, projectId, monthTime);
        }

        /// <summary>
        /// 根据时间范围获取计划年份集合
        /// </summary>
        /// <returns></returns>
        private int[] GetPlanYears(DateTime startTime, DateTime endTime)
        {
            var years = new List<int>();
            for (var i = startTime.Year; i <= endTime.Year; i++)
            {
                years.Add(i);
            }
            //判断结束时间是否跨年
            if (endTime.Month == 12 && endTime.Day > 25)
            {
                years.Add(endTime.AddMonths(1).Year);
            }
            return years.ToArray();
        }

        /// <summary>
        /// 获取年度计划集合
        /// </summary>
        /// <param name="projectIds">项目id集合</param>
        /// <param name="years">年份集合</param>
        /// <returns></returns>
        private async Task<List<ProjectPlanProduction>> GetProjectAnnualPlansAsync(Guid[] projectIds, int[] years)
        {
            return await _dbProjectAnnualPlan.GetListAsync(t => t.IsDelete == 1 && projectIds.Contains(t.ProjectId) && years.Contains(t.Year));
        }

        /// <summary>
        /// 统计项目年计划
        /// </summary>
        /// <returns></returns>
        private SumProjectPlannedDto SumYearProjectPlannedAsync(List<ProjectPlanProduction> projectAnnualPlans, Guid projectId, int year)
        {
            var monthPlanneds = new List<SumProjectPlannedDto>();
            for (int i = 1; i <= 12; i++)
            {
                var monthTime = new DateTime(year, i, 1);
                monthPlanneds.Add(SumMonthProjectPlanned(projectAnnualPlans, projectId, monthTime));
            }
            return new SumProjectPlannedDto() { PlannedOutputValue = monthPlanneds.Sum(t => t.PlannedOutputValue), PlannedQuantities = monthPlanneds.Sum(t => t.PlannedQuantities) };
        }

        /// <summary>
        /// 统计项目月计划
        /// </summary>
        /// <returns></returns>
        private SumProjectPlannedDto SumMonthProjectPlanned(List<ProjectPlanProduction> projectAnnualPlans, Guid projectId, DateTime monthTime)
        {
            var startYearTime = new DateTime(monthTime.Year, 1, 1);
            var endYearTime = startYearTime.AddYears(1);
            // 本年内的最新提交的一条项目年度计划
            var projectAnnualPlan = projectAnnualPlans.FirstOrDefault(t => t.ProjectId == projectId && t.Year == monthTime.Year);
            if (projectAnnualPlan == null)
            {
                return new SumProjectPlannedDto();
            }
            decimal? planOutValue = null;
            decimal? plannedQuantities = null;
            switch (monthTime.Month)
            {
                case 1:
                    planOutValue = projectAnnualPlan.OnePlanProductionValue;
                    plannedQuantities = projectAnnualPlan.OnePlannedQuantities;
                    break;
                case 2:
                    planOutValue = projectAnnualPlan.TwoPlanProductionValue;
                    plannedQuantities = projectAnnualPlan.TwoPlannedQuantities;
                    break;
                case 3:
                    planOutValue = projectAnnualPlan.ThreePlanProductionValue;
                    plannedQuantities = projectAnnualPlan.ThreePlannedQuantities;
                    break;
                case 4:
                    planOutValue = projectAnnualPlan.FourPlanProductionValue;
                    plannedQuantities = projectAnnualPlan.FourPlannedQuantities;
                    break;
                case 5:
                    planOutValue = projectAnnualPlan.FivePlanProductionValue;
                    plannedQuantities = projectAnnualPlan.FivPlannedQuantities;
                    break;
                case 6:
                    planOutValue = projectAnnualPlan.SixPlanProductionValue;
                    plannedQuantities = projectAnnualPlan.SixPlannedQuantities;
                    break;
                case 7:
                    planOutValue = projectAnnualPlan.SevenPlanProductionValue;
                    plannedQuantities = projectAnnualPlan.SevPlannedQuantities;
                    break;
                case 8:
                    planOutValue = projectAnnualPlan.EightPlanProductionValue;
                    plannedQuantities = projectAnnualPlan.EigPlannedQuantities;
                    break;
                case 9:
                    planOutValue = projectAnnualPlan.NinePlanProductionValue;
                    plannedQuantities = projectAnnualPlan.NinPlannedQuantities;
                    break;
                case 10:
                    planOutValue = projectAnnualPlan.TenPlanProductionValue;
                    plannedQuantities = projectAnnualPlan.TenPlannedQuantities;
                    break;
                case 11:
                    planOutValue = projectAnnualPlan.ElevenPlanProductionValue;
                    plannedQuantities = projectAnnualPlan.ElePlannedQuantities;
                    break;
                case 12:
                    planOutValue = projectAnnualPlan.TwelvePlanProductionValue;
                    plannedQuantities = projectAnnualPlan.TwePlannedQuantities;
                    break;
            }
            return new SumProjectPlannedDto() { PlannedOutputValue = planOutValue ?? 0, PlannedQuantities = plannedQuantities ?? 0 };
        }

        /// <summary>
        /// 获取省
        /// </summary>
        /// <param name="provinceAreas">省集合</param>
        /// <param name="districtAreas">市集合</param>
        /// <param name="area">地点</param>
        /// <returns></returns>
        private Province? GetProvinceArea(List<Province> provinceAreas, List<Province> districtAreas, Province area)
        {
            if (area.Zaddvslevel == 1)
            {
                return area;
            }
            else if (area.Zaddvslevel == 2)
            {
                return provinceAreas.FirstOrDefault(t => t.Zaddvscode == area.Zaddvsup);
            }
            else if (area.Zaddvslevel == 3)
            {
                var districtArea = districtAreas.FirstOrDefault(t => t.Zaddvscode == area.Zaddvsup);
                if (districtArea != null)
                {
                    return provinceAreas.FirstOrDefault(t => t.Zaddvscode == districtArea.Zaddvsup);
                }
            }
            return null;
        }

        /// <summary>
        /// 获取市
        /// </summary>
        /// <param name="districtAreas">市集合</param>
        /// <param name="area">地点</param>
        /// <returns></returns>
        private Province? GetDistrictArea(List<Province> districtAreas, Province area)
        {
            if (area.Zaddvslevel == 2)
            {
                return area;
            }
            else if (area.Zaddvslevel == 3)
            {
                return districtAreas.FirstOrDefault(t => t.Zaddvscode == area.Zaddvsup);
            }
            return null;
        }
        #endregion


        /// <summary>
        /// 项目月报状态是否已完成
        /// </summary>
        /// <returns></returns>
        public async Task<bool> IsFinishMonthReport(Guid projectId, int dateMonth)
        {
            var monthProject = await GetMonthReportAsync(projectId, dateMonth);
            if (monthProject != null && monthProject.Status == MonthReportStatus.Finish)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        ///保存一条项目月报
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> SaveProjectMonthReportAsync(SaveProjectMonthReportRequestDto model)
        {
            model.ResetModelProperty();
            var result = new ResponseAjaxResult<bool>();
            if (model.DateMonth == 202306)
            {
                return result.FailResult(HttpStatusCode.SaveFail, "由于历史数据的缘故，系统不允许修改6月份月报数据");
            }
            //if (!(now.Day >= 26 || (now.Day >= 1 && now.Day <= 5)))
            //{
            //    return result.FailResult(HttpStatusCode.SaveFail, "填报时间已过，时间截至在当月26号 - 次月5号");
            //}
            if (!ConvertHelper.TryParseFromDateMonth(model.DateMonth, out DateTime monthTime))
            {
                return result.FailResult(HttpStatusCode.SaveFail, "月份时间格式不正确");
            }
            //var allowSaveDateMonth = GetDefaultReportDateMonth();
            //if (model.DateMonth != allowSaveDateMonth)
            //{
            //    return result.FailResult(HttpStatusCode.SaveFail, $"{monthTime.ToString("yyyy年MM月")}项目月报已超过允许的填报时间");
            //}
            var project = await GetProjectPartAsync(model.ProjectId);
            if (project == null)
            {
                return result.FailResult(HttpStatusCode.DataNotEXIST, ResponseMessage.DATA_NOTEXIST_PROJECT);
            }
            var monthReport = await GetMonthReportAsync(model.ProjectId, model.DateMonth);
            // 是否是非施工项目
            var isNonConstruction = project.TypeId == CommonData.NoConstrutionProjectType;
            // 币种/汇率
            var currencyId = model.CurrencyId;
            var currencyExchangeRate = model.CurrencyExchangeRate;
            var reqDetails = model.ResolveDetails();
            reqDetails.ForEach(item =>
            {
                // 计算月报明细产值(元)
                item.CompleteProductionAmount = item.CompletedQuantity * item.UnitPrice * currencyExchangeRate;
            });
            var addDetails = new List<MonthReportDetail>();
            var updateDetails = new List<MonthReportDetail>();
            var removeDetails = new List<MonthReportDetail>();
            bool isAddMonthReport = false;
            if (monthReport == null)
            {
                monthReport = new MonthReport() { Id = GuidUtil.Next(), CreateId = _currentUser.Id, DateYear = monthTime.ToDateYear() };
                var lastDateMonth = monthTime.AddMonths(-1).ToDateMonth();
                var lastMonthReport = await GetMonthReportAsync(model.ProjectId, lastDateMonth);
                // 上个项目月报的（下月预估成本）带到本月存储
                if (lastMonthReport != null)
                {
                    monthReport.MonthEstimateCostAmount = lastMonthReport.NextMonthEstimateCostAmount;
                }
                addDetails = _mapper.Map(reqDetails, addDetails);
                isAddMonthReport = true;
            }
            else
            {
                var oldDetails = await GetMonthReportDetailsAsync(monthReport.Id);
                var exsitDetailIds = oldDetails.Where(t => reqDetails.Any(r => r.DetailId == t.Id)).Select(t => (Guid?)t.Id).ToArray();
                var addReqDetails = reqDetails.Where(t => !exsitDetailIds.Contains(t.DetailId)).ToList();
                addDetails = _mapper.Map(addReqDetails, addDetails);
                removeDetails = oldDetails.Where(t => !exsitDetailIds.Contains(t.Id)).ToList();
                updateDetails = oldDetails.Where(t => exsitDetailIds.Contains(t.Id)).ToList();
                monthReport.UpdateId = _currentUser.Id;
            }
            monthReport = _mapper.Map(model, monthReport);
            /*
             * 计算项目月报外包支出/产量/产值
             * 1、非施工类项目 外包支出/产值/产量以客户直接填写为准
             * 2、不是非施工分类项目 外包支出/产值/产量以统计月报明细为准
            */
            monthReport.CompletedQuantity = isNonConstruction ? monthReport.CompletedQuantity : reqDetails.Sum(t => (decimal)t.CompletedQuantity);
            monthReport.CompleteProductionAmount = isNonConstruction ? monthReport.CompleteProductionAmount : reqDetails.Sum(t => (decimal)t.CompleteProductionAmount);
            monthReport.CurrencyCompleteProductionAmount = isNonConstruction ? monthReport.CompleteProductionAmount : reqDetails.Sum(t => (decimal)t.UnitPrice * (decimal)t.CompletedQuantity);
            monthReport.OutsourcingExpensesAmount = isNonConstruction ? monthReport.OutsourcingExpensesAmount : reqDetails.Sum(t => (decimal)t.OutsourcingExpensesAmount) * currencyExchangeRate;
            monthReport.CurrencyId = currencyId;
            monthReport.CurrencyExchangeRate = currencyExchangeRate;
            monthReport.Status = isNonConstruction ? MonthReportStatus.Finish : model.Status;
            monthReport.StatusText = isNonConstruction ? "已完成" : model.StatusText;
            // 数据填充
            addDetails.ForEach(item =>
            {
                item.Id = GuidUtil.Next();
                item.CreateId = _currentUser.Id;
                item.MonthReportId = monthReport.Id;
                item.ProjectId = monthReport.ProjectId;
                item.DateMonth = monthReport.DateMonth;
                item.DateYear = monthReport.DateYear;
            });
            updateDetails.ForEach(item =>
            {
                var updateReqItem = reqDetails.Single(t => t.DetailId == item.Id);
                _mapper.Map(updateReqItem, item);
                item.UpdateId = _currentUser.Id;
            });
            removeDetails.ForEach(item =>
            {
                item.DeleteId = _currentUser.Id;
                item.IsDelete = 0;
            });
            var modelState = isAddMonthReport ? ModelState.Add : ModelState.Update;
            var newReportDetails = new List<MonthReportDetail>();
            newReportDetails.AddRange(addDetails);
            newReportDetails.AddRange(updateDetails);
            if (isAddMonthReport)
            {
                await _dbMonthReport.AsInsertable(monthReport).EnableDiffLogEvent(NewLogInfo(EntityType.MonthReport, monthReport.Id, modelState)).ExecuteCommandAsync();
            }
            else
            {
                await _dbMonthReport.AsUpdateable(monthReport).EnableDiffLogEvent(NewLogInfo(EntityType.MonthReport, monthReport.Id, modelState)).ExecuteCommandAsync();
            }
            if (removeDetails.Any())
            {
                await _dbMonthReportDetail.AsUpdateable(removeDetails).UpdateColumns(t => new { t.IsDelete, t.DeleteId, t.DeleteTime }).EnableDiffLogEvent(NewLogInfo(EntityType.MonthReport, monthReport.Id, modelState)).ExecuteCommandAsync();
            }
            if (updateDetails.Any())
            {
                await _dbMonthReportDetail.AsUpdateable(updateDetails).EnableDiffLogEvent(NewLogInfo(EntityType.MonthReport, monthReport.Id, modelState)).ExecuteCommandAsync();
            }
            if (addDetails.Any())
            {
                await _dbMonthReportDetail.AsInsertable(addDetails).EnableDiffLogEvent(NewLogInfo(EntityType.MonthReport, monthReport.Id, modelState)).ExecuteCommandAsync();
            }
            //失效暂存数据
            await CeaseEffectStagingMonthReportAsync(model.ProjectId, model.DateMonth);
            /*
             * 1、 联动变更对应的船舶月报产量/产值
             * 2、 保持船舶月报产值/产量永远于项目月报明细保持一致
             */
            await ChangeOwnerShipMonthReportByMonthReport(monthReport, newReportDetails);
            // 记录对象变更
            await _entityChangeService.RecordEntitysChangeAsync(EntityType.MonthReport, monthReport.Id);
            return result.SuccessResult(true);
        }

        /// <summary>
        /// 联动变更船舶月报产量/产值
        /// </summary>
        /// <param name="monthReport">项目月报</param>
        /// <param name="newDetails">新的月报明细</param>
        /// <returns></returns>
        private async Task ChangeOwnerShipMonthReportByMonthReport(MonthReport monthReport, List<MonthReportDetail> newDetails)
        {
            // 获取项目下所有的自有船舶月报
            var ownerShipMonthReports = await GetOwnerShipMonthReportsAsync(monthReport.ProjectId, monthReport.DateMonth);
            var changeReports = new List<OwnerShipMonthReport>();
            foreach (var ownerShipMonthReport in ownerShipMonthReports)
            {
                var thisShipDetails = newDetails.Where(t => t.OutPutType == ConstructionOutPutType.Self && t.ShipId == ownerShipMonthReport.ShipId);
                var production = thisShipDetails.Sum(t => t.CompletedQuantity);
                var productionAmount = thisShipDetails.Sum(t => t.CompleteProductionAmount);
                if (ownerShipMonthReport.Production != production || ownerShipMonthReport.ProductionAmount != productionAmount)
                {
                    ownerShipMonthReport.Production = production;
                    ownerShipMonthReport.ProductionAmount = productionAmount;
                    changeReports.Add(ownerShipMonthReport);
                }
            }
            if (changeReports.Any())
            {
                await _dbOwnerShipMonthReport.AsUpdateable(changeReports).UpdateColumns(t => new { t.Production, t.ProductionAmount }).ExecuteCommandAsync();
                await _entityChangeService.RecordEntitysChangeAsync(EntityType.OwnerShipMonthReport, changeReports.Select(t => t.Id).ToArray());
            }
        }


        /// <summary>
        ///暂存项目月报
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> StagingMonthReportAsync(StagingMonthReportRequestDto model)
        {
            var result = new ResponseAjaxResult<bool>();
            var stagingData = await GetMonthReportStagingDataAsync(model.ProjectId, model.DateMonth);
            var isAdd = false;
            if (stagingData == null)
            {
                stagingData = new StagingData()
                {
                    Id = GuidUtil.Next(),
                    CreateId = _currentUser.Id,
                    ProjectId = model.ProjectId,
                    DateMonth = model.DateMonth,
                    BizType = StagingBizType.SaveMonthReport

                };
                isAdd = true;
            }
            else
            {
                stagingData.UpdateId = _currentUser.Id;
            }
            stagingData.BizData = JsonConvert.SerializeObject(model);
            stagingData.IsEffectStaging = true;
            #region 日志信息
            var logDto = new LogInfo()
            {
                Id = GuidUtil.Increment(),
                DataId = stagingData.Id,
                BusinessModule = "/首页/项目月报/暂存",
                BusinessRemark = "/首页/项目月报/暂存",
                OperationId = _currentUser.Id,
                OperationName = _currentUser.Name,
            };
            #endregion
            if (isAdd)
            {
                await _dbStagingData.AsInsertable(stagingData).EnableDiffLogEvent(logDto).ExecuteCommandAsync();
            }
            else
            {
                await _dbStagingData.AsUpdateable(stagingData).EnableDiffLogEvent(logDto).ExecuteCommandAsync();
            }
            return result.SuccessResult(true);
        }

        /// <summary>
        ///失效项目月报暂存数据
        /// </summary>
        /// <returns></returns>
        public async Task CeaseEffectStagingMonthReportAsync(Guid projectId, int dateMonth)
        {
            var stagingData = await GetMonthReportStagingDataAsync(projectId, dateMonth);
            if (stagingData != null && stagingData.IsEffectStaging)
            {
                stagingData.IsEffectStaging = false;
                await _dbStagingData.AsUpdateable(stagingData).UpdateColumns(t => new { t.IsEffectStaging }).ExecuteCommandAsync();
            }
        }

        /// <summary>
        /// 获取项目月报未填报列表
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<List<UnReportProjectRepResponseDto>>> GetSearchNotFillProjectMonthRepAsync(ProjectMonthRepRequestDto model)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<UnReportProjectRepResponseDto>>();
            List<Guid> projectStatus = new List<Guid>()
            {
                "2c800da1-184f-408a-b5a4-fd915e8d6b6a".ToGuid(),
                "19050a4b-fe95-47cf-aafe-531d5894c88b".ToGuid(),
                "cd3c6e83-1b7c-40c2-a415-5a44f13584cc".ToGuid(),
                "75089b9a-b18b-442c-bfc8-fde4024d737f".ToGuid()
            };
            var userAuthForData = await GetCurrentUserAuthForDataAsync();
            if (model.StartTime == null && model.EndTime == null)
            {
                if (DateTime.Now.Day > 25)
                {
                    model.StartTime = DateTime.Now;
                    model.EndTime = DateTime.Now;
                }
                else
                {
                    model.StartTime = DateTime.Now.AddMonths(-1);
                    model.EndTime = DateTime.Now.AddMonths(-1);
                }
            }
            var startTime = ((DateTime)model.StartTime).ToDateMonth();
            var endTime = ((DateTime)model.EndTime).ToDateMonth();
            RefAsync<int> total = 0;
            var list = new List<UnReportProjectRepResponseDto>();
            ResolveTagNames(model.TagName, out List<int> categoryList, out List<int> tagList, out List<int> tag2List);
            var institution = await _dbContext.Queryable<Institution>().Where(x => x.Status == "4").ToListAsync();
            var ids = institution.Select(x => x.PomId).ToList();
            var query = _dbProject.AsQueryable()
                .InnerJoin(_dbcrossMonth.AsQueryable(), (x, y) => y.Month >= startTime && y.Month <= endTime)
                .LeftJoin(_dbMonthReport.AsQueryable(), (x, y, t) => x.Id == t.ProjectId && y.Month == t.DateMonth && t.DateMonth >= startTime && t.DateMonth <= endTime && t.IsDelete == 1)
                .WhereIF(!userAuthForData.IsAdmin, (x, y, t) => userAuthForData.CompanyIds.Contains(x.ProjectDept))
                .WhereIF(true, (x, y, t) => x.IsDelete == 1 && projectStatus.Contains(x.StatusId.Value) && !ids.Contains(x.ProjectDept))
                .WhereIF(model.CompanyId != null, (x, y, t) => x.CompanyId == model.CompanyId)
                .WhereIF(model.ProjectDept != null, (x, y, t) => x.ProjectDept == model.ProjectDept)
                .WhereIF(model.ProjectStatusId != null && model.ProjectStatusId.Any(), (x, y, t) => model.ProjectStatusId.Contains(x.StatusId.Value.ToString()))
                .WhereIF(model.ProjectRegionId != null, (x, y, t) => x.RegionId == model.ProjectRegionId)
                .WhereIF(model.ProjectTypeId != null, (x, y, t) => x.TypeId == model.ProjectTypeId)
                .WhereIF(!string.IsNullOrWhiteSpace(model.ProjectName), (x, y, t) => SqlFunc.Contains(x.Name, model.ProjectName))
                .WhereIF(model.ProjectAreaId != null, (x, y, t) => x.AreaId == model.ProjectAreaId)
                .WhereIF(categoryList != null && categoryList.Any(), (x, y, t) => categoryList.Contains(x.Category))
                .WhereIF(tagList != null && tagList.Any(), (x, y, t) => tagList.Contains(x.Tag))
                .WhereIF(tag2List != null && tag2List.Any(), (x, y, t) => tag2List.Contains(x.Tag2))
                .Select((x, y, t) => new UnReportProjectRepResponseDto
                {
                    ProjectId = x.Id,
                    ProjectName = x.Name,
                    ProjectTypeId = x.TypeId == null ? Guid.Empty : x.TypeId,
                    Category = x.Category,
                    CompanyId = x.CompanyId,
                    StatusId = x.StatusId,
                    DateDay = y.Month,
                    ResponseMonthId = t.Id
                });
            query = _dbContext.Queryable(query).Where(x => x.ResponseMonthId == null).OrderBy(x => new { x.ProjectId, x.DateDay });
            list = model.IsFullExport ? await query.ToListAsync() : await query.ToPageListAsync(model.PageIndex, model.PageSize, total);
            var companyIds = list.Select(t => t.CompanyId).Distinct().ToArray();
            var companys = await GetInstitutionsAsync(companyIds);
            var projectTypes = await GetProjectTypesAsync();
            var projectStatusList = await GetProjectStatusListAsync();
            list.ForEach(item =>
            {
                item.CompanyName = companys.FirstOrDefault(t => t.PomId == item.CompanyId)?.Name;
                item.ProjectTypeName = projectTypes.FirstOrDefault(t => t.PomId == item.ProjectTypeId)?.Name;
                item.StatusName = projectStatusList.FirstOrDefault(t => t.StatusId == item.StatusId)?.Name;
            });
            return responseAjaxResult.SuccessResult(list, total);
        }

        /// <summary>
        /// 获取一条项目月报暂存数据
        /// </summary>
        /// <returns></returns>
        private async Task<StagingData?> GetMonthReportStagingDataAsync(Guid projectId, int dateMonth)
        {
            return await _dbStagingData.GetFirstAsync(t => t.BizType == StagingBizType.SaveMonthReport && t.ProjectId == projectId && t.DateMonth == dateMonth && t.IsDelete == 1);
        }


        /// <summary>
        /// 从暂存里获取获取月报详情列表
        /// </summary>
        /// <returns></returns>
        private async Task<StagingMonthReportRequestDto?> GetStagingMonthReportAsync(Guid projectId, int dateMonth)
        {
            var stagingData = await GetMonthReportStagingDataAsync(projectId, dateMonth);
            // 判断是否有暂存数据
            if (stagingData == null || (!stagingData.IsEffectStaging) || stagingData.BizData == null)
            {
                return null;
            }
            return JsonConvert.DeserializeObject<StagingMonthReportRequestDto>(stagingData.BizData);
        }

        /// <summary>
        /// 搜索一条项目月报
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<ProjectMonthReportResponseDto>> SearchProjectMonthReportAsync(ProjectMonthReportRequestDto model)
        {
            model.ResetModelProperty();
            var result = new ResponseAjaxResult<ProjectMonthReportResponseDto>();
            var resMonthReport = new ProjectMonthReportResponseDto();
            var project = await GetProjectPartAsync(model.ProjectId);
            var projectTotalMonthProdcutionValue = await _dbContext.Queryable<MonthReport>().Where(x => x.IsDelete == 1 && x.ProjectId == model.ProjectId)
                .SumAsync(x => x.CompleteProductionAmount);
            if (project == null)
            {
                return result.FailResult(HttpStatusCode.DataNotEXIST, ResponseMessage.DATA_NOTEXIST_PROJECT);
            }
            //当前时间月份
            var nowDateMonth = GetDefaultReportDateMonth();
            var dateMonth = model.DateMonth ?? nowDateMonth;
            var projectWBSList = await GetProjectWBSListAsync(model.ProjectId, model.DateMonth);
            var resWBSList = projectWBSList.Select(t => new ProjectMonthReportResponseDto.ResTreeProjectWBSDetailDto()
            {
                ProjectWBSId = t.Id,
                KeyId = t.KeyId,
                Pid = t.Pid,
                ProjectWBSName = t.Name,
                UnitPrice = t.UnitPrice,
                ContractQuantity = t.EngQuantity,
                ContractAmount = t.UnitPrice * t.EngQuantity
            }).ToList();
            ConvertHelper.TryParseFromDateMonth(dateMonth, out DateTime monthTime);
            var yearStartMonth = new DateTime(monthTime.Year, 1, 1).ToDateMonth();
            var yearEndMonth = new DateTime(monthTime.Year, 12, 1).ToDateMonth();
            // 年份统计
            var sumYearDetails = await SumMonthReportDetailsAsync(model.ProjectId, yearStartMonth, yearEndMonth);
            // 全累计
            var sumDetails = await SumMonthReportDetailsAsync(model.ProjectId, DateTime.MinValue.ToDateMonth(), DateTime.MaxValue.ToDateMonth());
            // 项目月报
            var monthReport = await GetMonthReportAsync(model.ProjectId, dateMonth);
            // 船舶Id
            var subShipIds = new Guid[0];
            var ownerShipIds = new Guid[0];
            var resDetails = new List<ProjectMonthReportResponseDto.ResMonthReportDetail>();


            // 是否是非施工项目
            var isNonConstruction = project.TypeId == CommonData.NoConstrutionProjectType;

            // 是否可暂存
            var isCanStaging = false;
            // 是否从暂存中取的业务数据
            var isFromStaging = false;
            // 是否来源于日报
            var isFromDayReport = false;
            // 月报状态
            var status = monthReport?.Status ?? MonthReportStatus.None;
            // 月报状态文本
            var statusText = string.Empty;
            // 是否可提交
            var isCanSubmit = await IsCanSubmitAsync(monthReport, dateMonth, nowDateMonth);
            if (monthReport != null && monthReport.Status != MonthReportStatus.Finish)
            {
                statusText = monthReport.Status == MonthReportStatus.ApproveReject ? monthReport.StatusText + ",原因：" + monthReport.RejectReason : monthReport.StatusText;
            }
            // 判断业务来源暂存
            if (isCanSubmit && dateMonth == nowDateMonth)
            {
                try
                {
                    var stagingMonthReport = await GetStagingMonthReportAsync(model.ProjectId, dateMonth);
                    if (stagingMonthReport != null)
                    {
                        resMonthReport = _mapper.Map(stagingMonthReport, resMonthReport);
                        resDetails = _mapper.Map(stagingMonthReport.ResolveDetails(), resDetails);
                        isFromStaging = true;
                        statusText = "暂存中";
                    }
                }
                catch
                {
                }
                isCanStaging = true;
            }
            if (!isFromStaging)
            {
                if (monthReport == null)
                {
                    //判断月份天数范围
                    ConvertHelper.TryParseFromDateMonth(dateMonth, out DateTime dateMonthTime);
                    var lastDateMonthTime = dateMonthTime.AddMonths(-1);
                    var startMonthDateDay = new DateTime(lastDateMonthTime.Year, lastDateMonthTime.Month, 26).ToDateDay();
                    var endMonthDateDay = new DateTime(dateMonthTime.Year, dateMonthTime.Month, 25).ToDateDay();
                    var sumMonthDayReports = await SumDayReportBySourceAsync(model.ProjectId, startMonthDateDay, endMonthDateDay);
                    sumMonthDayReports.ForEach(item =>
                    {
                        var resDetail = new ProjectMonthReportResponseDto.ResMonthReportDetail()
                        {
                            ProjectWBSId = item.ProjectWBSId,
                            UnitPrice = item.UnitPrice,
                            OutsourcingExpensesAmount = item.OutsourcingExpensesAmount,
                            CompletedQuantity = item.Production,
                            CompleteProductionAmount = item.UnitPrice * item.Production,
                            OutPutType = item.OutPutType,
                            ShipId = item.ShipId,
                            ConstructionNature = item.ConstructionNature
                        };
                        resDetails.Add(resDetail);
                    });
                    isFromDayReport = true;
                }
                else
                {
                    var details = await GetMonthReportDetailsAsync(monthReport.Id);
                    resDetails = _mapper.Map<List<MonthReportDetail>, List<ProjectMonthReportResponseDto.ResMonthReportDetail>>(details);
                    _mapper.Map(monthReport, resMonthReport);
                }
            }
            subShipIds = resDetails.Where(t => t.OutPutType == ConstructionOutPutType.SubPackage && t.ShipId != null).Select(t => (Guid)t.ShipId).ToArray();
            ownerShipIds = resDetails.Where(t => t.OutPutType != ConstructionOutPutType.SubPackage && t.ShipId != null).Select(t => (Guid)t.ShipId).ToArray();
            // 自有资源
            var ownerShips = await GetShipResourcesAsync(ownerShipIds, ConstructionOutPutType.Self);
            // 分包资源
            var subShips = await GetShipResourcesAsync(subShipIds, ConstructionOutPutType.SubPackage);
            // 施工性质
            var constructionNatures = await GetDictionarysAsync(DictionaryTypeNo.ConstructionNature);
            resDetails.ForEach(item =>
            {
                var thisSumYearDetail = sumYearDetails.FirstOrDefault(t => t.ProjectWBSId == item.ProjectWBSId && t.OutPutType == item.OutPutType && t.ShipId == item.ShipId && t.UnitPrice == item.UnitPrice && t.ConstructionNature == t.ConstructionNature);
                var thisSumDetail = sumDetails.FirstOrDefault(t => t.ProjectWBSId == item.ProjectWBSId && t.OutPutType == item.OutPutType && t.ShipId == item.ShipId && t.UnitPrice == item.UnitPrice && t.ConstructionNature == t.ConstructionNature);

                if (thisSumYearDetail != null)
                {
                    item.YearCompletedQuantity = Math.Round(thisSumYearDetail.CompletedQuantity, 2);
                    item.YearCompleteProductionAmount = thisSumYearDetail.CurrencyCompleteProductionAmount;
                    //item.YearOutsourcingExpensesAmount = Math.Round(thisSumYearDetail.OutsourcingExpensesAmount, 2);
                    item.YearOutsourcingExpensesAmount = thisSumYearDetail.OutsourcingExpensesAmount;
                }
                if (thisSumDetail != null)
                {
                    item.TotalCompletedQuantity = Math.Round(thisSumDetail.CompletedQuantity, 2);
                    //累计完成值
                    // item.TotalCompleteProductionAmount = thisSumDetail.CurrencyCompleteProductionAmount;
                    //目前是月度完成产值 暂时不取项目结构子项相加
                    //item.TotalCompleteProductionAmount = Math.Round(projectTotalMonthProdcutionValue,2);
                    item.TotalOutsourcingExpensesAmount = Math.Round(thisSumDetail.OutsourcingExpensesAmount, 2);
                }
                if (item.OutPutType == ConstructionOutPutType.SubPackage)
                {
                    item.ShipName = subShips.FirstOrDefault(t => t.ShipId == item.ShipId)?.ShipName;
                }
                else
                {
                    item.ShipName = ownerShips.FirstOrDefault(t => t.ShipId == item.ShipId)?.ShipName;
                }
                item.ConstructionNatureName = constructionNatures.FirstOrDefault(t => t.Type == item.ConstructionNature)?.Name;
                item.CompleteProductionAmount = (item.UnitPrice ?? 0) * (item.CompletedQuantity ?? 0);
                var resWBS = resWBSList.FirstOrDefault(t => t.ProjectWBSId == item.ProjectWBSId);
                if (resWBS == null)
                {
                    resWBS = new ProjectMonthReportResponseDto.ResTreeProjectWBSDetailDto()
                    {
                        ProjectWBSId = item.ProjectWBSId,
                        Pid = "0",
                        KeyId = GuidUtil.Next().ToString()
                    };
                    resWBSList.Add(resWBS);
                }
                //item.ContractQuantity = resWBS.ContractQuantity;
                //item.ContractAmount = resWBS.ContractAmount;
                resWBS.ReportDetails.Add(item);
            });
            var level1NodeList = resWBSList.Where(t => t.Pid == "0").ToList();
            level1NodeList.ForEach(node =>
            {
                MapTreeResNode(isFromDayReport, node, resWBSList);
            });
            // 未填报 或驳回状态币种/汇率依赖于项目变更
            if (status == MonthReportStatus.None || status == MonthReportStatus.ApproveReject)
            {
                // 币种/汇率
                var currencyId = project.CurrencyId ?? CommonData.RMBCurrencyId;
                var currencyExchangeRate = 1m;
                if (currencyId != CommonData.RMBCurrencyId)
                {
                    currencyExchangeRate = await GetCurrencyRateAsync(currencyId);
                }
                resMonthReport.CurrencyId = currencyId;
                resMonthReport.CurrencyExchangeRate = currencyExchangeRate;
            }

            #region 增加四个字段
            if (resMonthReport != null)
            {
                #region 参数问题
                var startTime = string.Empty;
                if (model.DateMonth == null)
                {
                    model.DateMonth = DateTime.Now.AddMonths(-1).Month;
                }
                //else if (model.DateMonth.HasValue)
                //{
                //    if (DateTime.Now.Day >= 26 || DateTime.Now.Day <= 1)
                //    {
                //        model.DateMonth = DateTime.Now.Month;
                //    }
                //    else {
                //        model.DateMonth = DateTime.Now.AddMonths(-1).Month;
                //    }
                //}
                if (model.DateMonth.Value.ToString().Length == 1)
                {
                    model.DateMonth = int.Parse(DateTime.Now.Year + $"0{model.DateMonth}");
                }
                #endregion
                var resultData = await GetProjectProductionValue(model.ProjectId, model.DateMonth.Value);
                resMonthReport.CurrentYearOffirmProductionValue = resultData.Item1;
                resMonthReport.TotalYearKaileaOffirmProductionValue = resultData.Item2;
                resMonthReport.CurrenYearCollection = resultData.Item3;
                resMonthReport.TotalYearCollection = resultData.Item4;
            }


            #endregion

            #region 修改汇率
            if (resMonthReport.CurrencyId != Guid.Empty && resMonthReport.CurrencyId != CommonData.RMBCurrencyId)
            {
                resMonthReport.CurrencyExchangeRate = await GetCurrencyRateAsync(resMonthReport.CurrencyId);
            }

            #endregion
            resMonthReport.TotalProductionAmount = Math.Round(projectTotalMonthProdcutionValue, 2);
            resMonthReport.Status = status;
            resMonthReport.StatusText = statusText;
            resMonthReport.DateMonth = dateMonth;
            resMonthReport.ProjectId = model.ProjectId;
            resMonthReport.TreeDetails = level1NodeList.ToArray();
            resMonthReport.ProjectCategory = project.Category;
            resMonthReport.IsFromStaging = isFromStaging;
            resMonthReport.IsCanSubmit = isCanSubmit;
            resMonthReport.IsCanStaging = isCanStaging;
            resMonthReport.IsNonConstruction = isNonConstruction;
            resMonthReport.ProjectName = project.Name;
            resMonthReport.JobId = monthReport?.JobId;
            /*
             * 1、非施工类项目 外包支出/产值/产量以客户直接填写为准
             * 2、不是非施工分类项目 外包支出/产值/产量以统计月报明细为准
             */
            resMonthReport.OutsourcingExpensesAmount = isNonConstruction ? resMonthReport.OutsourcingExpensesAmount : level1NodeList.Sum(t => t.OutsourcingExpensesAmount);
            resMonthReport.CompletedQuantity = isNonConstruction ? resMonthReport.CompletedQuantity : level1NodeList.Sum(t => t.CompletedQuantity);
            resMonthReport.CompleteProductionAmount = isNonConstruction ? resMonthReport.CompleteProductionAmount : level1NodeList.Sum(t => t.CompleteProductionAmount);
            resMonthReport.JobSubmitType = monthReport != null && monthReport.Status == MonthReportStatus.ApproveReject ? JobSubmitType.ResetJob : JobSubmitType.AddJob;
            resMonthReport.ModelState = monthReport == null ? ModelState.Add : ModelState.Update;
            return result.SuccessResult(resMonthReport);
        }

        /// <summary>
        ///是否允许提交
        /// </summary>
        /// <returns></returns>
        private async Task<bool> IsCanSubmitAsync(MonthReport? monthReport, int dateMonth, int nowDateMonth)
        {
            if (dateMonth > nowDateMonth)
            {
                return false;
            }
            if (monthReport != null && monthReport.Status == MonthReportStatus.Approveling)
            {
                return false;
            }
            if (monthReport != null && monthReport.Status == MonthReportStatus.Finish)
            {
                var unFinishIob = await GetUnFinishMonthReportJobPartAsync(monthReport.ProjectId, monthReport.DateMonth);
                if (unFinishIob != null)
                {
                    return false;
                }
            }
            if (dateMonth == nowDateMonth && monthReport != null && monthReport.Status == MonthReportStatus.Finish)
            {
                return await _bizAuthorizeService.IsAuthorizedAsync(_currentUser.Id, BizModule.MonthReport);
            }
            if (dateMonth < nowDateMonth && (monthReport == null || monthReport.Status == MonthReportStatus.Finish))
            {
                return await _bizAuthorizeService.IsAuthorizedAsync(_currentUser.Id, BizModule.MonthReport);
            }
            return true;
        }


        /// <summary>
        /// 适配树形节点
        /// </summary>
        /// <param name="isFilterParentDetails">是否过滤掉父级的填报明细</param>
        /// <param name="node">节点</param>
        /// <param name="list">项目结构集合</param>
        private void MapTreeResNode(bool isFilterParentDetails, ProjectMonthReportResponseDto.ResTreeProjectWBSDetailDto node, List<ProjectMonthReportResponseDto.ResTreeProjectWBSDetailDto> list)
        {
            if (string.IsNullOrWhiteSpace(node.KeyId) || string.IsNullOrWhiteSpace(node.Pid) || node.KeyId == node.Pid)
            {
                return;
            }
            var childrenNodes = list.Where(t => t.Pid == node.KeyId).ToArray();
            foreach (var childrenNode in childrenNodes)
            {
                MapTreeResNode(isFilterParentDetails, childrenNode, list);
            }
            node.Children = childrenNodes;
            if (isFilterParentDetails && node.Children.Any())
            {
                node.ReportDetails = new List<ProjectMonthReportResponseDto.ResMonthReportDetail>();
            }
        }

        /// <summary>
        /// 获取未完成项目月报任务
        /// </summary>
        /// <returns></returns>
        private async Task<Model.Job?> GetUnFinishMonthReportJobPartAsync(Guid projectId, int datemonth)
        {
            var query = _dbJob.AsQueryable().Where(t => t.IsDelete == 1 && t.ProjectId == projectId && t.DateMonth == datemonth && t.BizModule == BizModule.MonthReport && t.IsFinish == false);
            return await query.Select(t => new Model.Job { Id = t.Id, ApproveStatus = t.ApproveStatus, IsFinish = t.IsFinish }).FirstAsync();
        }

        /// <summary>
        /// 获取一个项目月报明细集合
        /// </summary>
        /// <returns></returns>
        private async Task<List<MonthReportDetail>> GetMonthReportDetailsAsync(Guid monthReportId)
        {
            return await _dbMonthReportDetail.GetListAsync(t => t.MonthReportId == monthReportId && t.IsDelete == 1);
        }

        /// <summary>
        /// 获取一个项目月报
        /// </summary>
        /// <param name="projectId">项目Id</param>
        /// <param name="dateMonth">月份</param>
        /// <returns></returns>
        private async Task<MonthReport?> GetMonthReportAsync(Guid projectId, int dateMonth)
        {
            return await _dbMonthReport.GetFirstAsync(t => t.ProjectId == projectId && t.DateMonth == dateMonth && t.IsDelete == 1);
        }

        /// <summary>
        /// 项目-年份-统计 
        /// </summary>
        /// <returns></returns>
        private async Task<List<SumMonthReportDto>> SumMonthReportsByYearAsync(Guid[] projectIds, int startDateYear, int endDateYear)
        {
            if (!projectIds.Any())
            {
                return new List<SumMonthReportDto>();
            }
            return await _dbMonthReport.AsQueryable().Where(t => projectIds.Contains(t.ProjectId) && t.DateYear >= startDateYear && t.DateYear <= endDateYear && t.IsDelete == 1)
                 .GroupBy(t => new { t.ProjectId, t.DateYear })
                 .Select(t => new SumMonthReportDto
                 {
                     ProjectId = t.ProjectId,
                     DateYear = t.DateYear,
                     CompletedQuantity = SqlFunc.AggregateSum(t.CompletedQuantity),
                     CompleteProductionAmount = SqlFunc.AggregateSum(t.CompleteProductionAmount),
                     PartyAConfirmedProductionAmount = SqlFunc.AggregateSum(t.PartyAConfirmedProductionAmount),
                     PartyAPayAmount = SqlFunc.AggregateSum(t.PartyAPayAmount),
                     CostAmount = SqlFunc.AggregateSum(t.CostAmount),
                     EstimateCostAmount = SqlFunc.AggregateSum(t.MonthEstimateCostAmount),
                     CurrencyCompleteProductionAmount = SqlFunc.AggregateSum(t.CurrencyCompleteProductionAmount),
                     OutsourcingExpensesAmount = SqlFunc.AggregateSum(t.OutsourcingExpensesAmount)
                 }).ToListAsync();
        }

        /// <summary>
        /// 项目月报-统计 
        /// </summary>
        /// <returns></returns>
        private async Task<List<SumMonthReportDto>> SumMonthReportsAsync(Guid[] projectIds)
        {
            if (!projectIds.Any())
            {
                return new List<SumMonthReportDto>();
            }
            return await _dbMonthReport.AsQueryable().Where(t => projectIds.Contains(t.ProjectId) && t.IsDelete == 1)
                 .GroupBy(t => t.ProjectId)
                 .Select(t => new SumMonthReportDto
                 {
                     ProjectId = t.ProjectId,
                     CompletedQuantity = SqlFunc.AggregateSum(t.CompletedQuantity),
                     CompleteProductionAmount = SqlFunc.AggregateSum(t.CompleteProductionAmount),
                     PartyAConfirmedProductionAmount = SqlFunc.AggregateSum(t.PartyAConfirmedProductionAmount),
                     PartyAPayAmount = SqlFunc.AggregateSum(t.PartyAPayAmount),
                     CostAmount = SqlFunc.AggregateSum(t.CostAmount),
                     EstimateCostAmount = SqlFunc.AggregateSum(t.MonthEstimateCostAmount),
                     CurrencyCompleteProductionAmount = SqlFunc.AggregateSum(t.CurrencyCompleteProductionAmount),
                     OutsourcingExpensesAmount = SqlFunc.AggregateSum(t.OutsourcingExpensesAmount)
                 }).ToListAsync();
        }

        /// <summary>
        /// 项目月报-明细-统计
        /// </summary>
        /// <returns></returns>
        private async Task<List<SumMonthReportDetailDto>> SumMonthReportDetailsAsync(Guid projectId, int startMonth, int endMonth)
        {
            return await _dbMonthReportDetail.AsQueryable().Where(t => projectId == t.ProjectId && t.DateMonth >= startMonth && t.DateMonth <= endMonth && t.IsDelete == 1)
                 .GroupBy(t => new { t.ProjectId, t.ProjectWBSId, t.OutPutType, t.ShipId, t.UnitPrice, t.ConstructionNature })
                 .Select(t => new SumMonthReportDetailDto
                 {
                     ProjectId = t.ProjectId,
                     ProjectWBSId = t.ProjectWBSId,
                     OutPutType = t.OutPutType,
                     ShipId = t.ShipId,
                     UnitPrice = t.UnitPrice,
                     ConstructionNature = t.ConstructionNature,
                     CompletedQuantity = SqlFunc.AggregateSum(t.CompletedQuantity),
                     CompleteProductionAmount = SqlFunc.AggregateSum(t.CompleteProductionAmount),
                     CurrencyCompleteProductionAmount = SqlFunc.AggregateSum(t.UnitPrice * t.CompletedQuantity),
                     OutsourcingExpensesAmount = SqlFunc.AggregateSum(t.OutsourcingExpensesAmount)
                 }).ToListAsync();
        }

        /// <summary>
        /// 项目月报-船舶-统计
        /// </summary>
        /// <returns></returns>
        private async Task<List<SumMonthReportDetailDto>> SumMonthReportDetailAsync(Guid[] projectIds, Guid[] shipIds, ConstructionOutPutType outPutType, int startMonth, int endMonth)
        {
            return await _dbMonthReportDetail.AsQueryable()
                  .Where(t => t.IsDelete == 1 && projectIds.Contains(t.ProjectId) && t.DateMonth >= startMonth && t.DateMonth <= endMonth && shipIds.Contains(t.ShipId) && t.OutPutType == outPutType)
                   .GroupBy(t => new { t.ProjectId, t.ShipId, t.OutPutType, t.DateMonth })
                   .Select(t => new SumMonthReportDetailDto
                   {
                       ProjectId = t.ProjectId,
                       OutPutType = t.OutPutType,
                       DateMonth = t.DateMonth,
                       CompletedQuantity = SqlFunc.AggregateSum(t.CompletedQuantity),
                       CompleteProductionAmount = SqlFunc.AggregateSum(t.CompleteProductionAmount),
                       CurrencyCompleteProductionAmount = SqlFunc.AggregateSum(t.UnitPrice * t.CompletedQuantity),
                   }).ToListAsync();
        }

        /// <summary>
        /// 项目月报-船舶-月份-统计
        /// </summary>
        /// <returns></returns>
        private async Task<SumMonthReportDetailDto> SumMonthReportDetailAsync(Guid projectId, Guid shipId, ConstructionOutPutType outPutType, int dateMonth)
        {
            var sumReport = await _dbMonthReportDetail.AsQueryable()
                .Where(t => t.ProjectId == projectId && t.DateMonth == dateMonth && t.IsDelete == 1 && t.ShipId == shipId && t.OutPutType == outPutType)
                 .GroupBy(t => new { t.ProjectId, t.ShipId, t.OutPutType, t.DateMonth })
                 .Select(t => new SumMonthReportDetailDto
                 {
                     CompletedQuantity = SqlFunc.AggregateSum(t.CompletedQuantity),
                     CompleteProductionAmount = SqlFunc.AggregateSum(t.CompleteProductionAmount),
                     CurrencyCompleteProductionAmount = SqlFunc.AggregateSum(t.UnitPrice * t.CompletedQuantity),
                 }).FirstAsync();

            sumReport = sumReport ?? new SumMonthReportDetailDto();
            sumReport.ProjectId = projectId;
            sumReport.DateMonth = dateMonth;
            sumReport.ShipId = shipId;
            sumReport.OutPutType = outPutType;
            return sumReport;
        }

        #endregion

        #region 船舶月报

        #region 船舶动态月报(暂未启用)

        /// <summary>
        ///保存一条船舶动态月报
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> SaveShipDynamicMonthReportAsync(SaveShipDynamicMonthReportRequestDto model)
        {
            var result = new ResponseAjaxResult<bool>();
            var now = DateTime.Now;
            if (!(now.Day >= 26 || (now.Day >= 1 && now.Day <= 5)))
            {
                return result.FailResult(HttpStatusCode.SaveFail, "填报时间已过，时间截至在当月26-次月五号");
            }
            var allowSaveDateMonth = now.ToDateMonth();
            if (now.Day < 26)
            {
                allowSaveDateMonth = now.AddMonths(-1).ToDateMonth();
            }
            if (model.DateMonth != allowSaveDateMonth)
            {
                return result.FailResult(HttpStatusCode.SaveFail, "船舶动态月报不可更改");
            }
            var ship = await GetShipPartAsync(model.ShipId, ShipType.OwnerShip);
            if (ship == null)
            {
                return result.FailResult(HttpStatusCode.DataNotEXIST, ResponseMessage.DATA_NOTEXIST_OWNERSHIP);
            }
            var monthReport = await GetShipDynamicMonthReportAsync(model.ShipId, model.DateMonth);
            var monthReportDetails = new List<ShipDynamicMonthReportDetail>();
            bool isAddMonthReport = false;
            if (monthReport == null)
            {
                monthReport = new ShipDynamicMonthReport() { Id = GuidUtil.Next(), CreateId = _currentUser.Id };
                isAddMonthReport = true;
            }
            else
            {
                monthReport.UpdateId = _currentUser.Id;
                monthReportDetails = await GetShipDynamicMonthReportDetailsAsync(monthReport.Id);
            }
            monthReport = _mapper.Map(model, monthReport);
            var oldDetails = new List<ShipDynamicMonthReportDetail>();
            var addReqDetails = new List<SaveShipDynamicMonthReportRequestDto.ReqShipDynamicMonthReportDetail>();
            if (isAddMonthReport)
            {
                addReqDetails = model.Details.ToList();
            }
            else
            {
                oldDetails = await GetShipDynamicMonthReportDetailsAsync(monthReport.Id);
                addReqDetails = model.Details.Where(t => t.DetailId == null).ToList();
            }
            var exsitDetailIds = model.Details.Where(t => t.DetailId != null).Select(t => t.DetailId).ToArray();
            // 新增
            var addDetails = _mapper.Map<List<SaveShipDynamicMonthReportRequestDto.ReqShipDynamicMonthReportDetail>, List<ShipDynamicMonthReportDetail>>(addReqDetails);
            // 移除
            var removeDetails = oldDetails.Where(t => !exsitDetailIds.Contains(t.Id)).ToList();
            // 更新
            var updateDetails = oldDetails.Where(t => exsitDetailIds.Contains(t.Id)).ToList();
            // 数据填充
            addDetails.ForEach(detail =>
            {
                detail.Id = GuidUtil.Next();
                detail.CreateId = _currentUser.Id;
                detail.ShipDynamicMonthReportId = monthReport.Id;
                detail.ShipId = monthReport.ShipId;
                detail.DateMonth = monthReport.DateMonth;
            });
            updateDetails.ForEach(detail =>
            {
                var updateReqDetail = model.Details.Single(t => t.DetailId == detail.Id);
                _mapper.Map(updateReqDetail, detail);
                detail.UpdateId = _currentUser.Id;
            });
            removeDetails.ForEach(detail =>
            {
                detail.DeleteId = _currentUser.Id;
                detail.IsDelete = 0;
            });
            if (isAddMonthReport)
            {
                await _dbShipDynamicMonthReport.AsInsertable(monthReport).ExecuteCommandAsync();
            }
            else
            {
                await _dbShipDynamicMonthReport.AsUpdateable(monthReport).ExecuteCommandAsync();
            }
            if (removeDetails.Any())
            {
                await _dbShipDynamicMonthReportDetail.AsUpdateable(removeDetails).UpdateColumns(t => new { t.IsDelete, t.DeleteId, t.DeleteTime }).ExecuteCommandAsync();
            }
            if (updateDetails.Any())
            {
                await _dbShipDynamicMonthReportDetail.AsUpdateable(updateDetails).ExecuteCommandAsync();
            }
            if (addDetails.Any())
            {
                await _dbShipDynamicMonthReportDetail.AsInsertable(addDetails).ExecuteCommandAsync();
            }
            //todo推送到Pom
            return result.SuccessResult(true);
        }

        /// <summary>
        /// 搜索一条船舶动态月报
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<ShipDynamicMonthReportResponseDto>> SearchShipDynamicMonthReportAsync(ShipDynamicMonthReportRequestDto model)
        {
            var result = new ResponseAjaxResult<ShipDynamicMonthReportResponseDto>();
            var now = DateTime.Now;
            var findDateMonth = now.ToDateMonth();
            // 26号为月节点的起始时间
            if (now.Day < 26)
            {
                findDateMonth = now.AddMonths(-1).ToDateMonth();
            }
            var dateMonth = model.DateMonth ?? findDateMonth;
            var monthReport = await GetShipDynamicMonthReportAsync(model.ShipId, dateMonth);
            var resMonthReport = new ShipDynamicMonthReportResponseDto();
            var resDetails = new List<ShipDynamicMonthReportResponseDto.ResShipDynamicMonthReportDetail>();
            if (monthReport == null)
            {
                resMonthReport.DateMonth = dateMonth;
                resMonthReport.ShipId = model.ShipId;
                if (dateMonth == findDateMonth)
                {
                    //判断月份天数范围
                    ConvertHelper.TryParseFromDateMonth(dateMonth, out DateTime dateMonthTime);
                    var lastDateMonthTime = dateMonthTime.AddMonths(-1);
                    var startDateDay = new DateTime(lastDateMonthTime.Year, lastDateMonthTime.Month, 26).ToDateDay();
                    var endDateDay = new DateTime(dateMonthTime.Year, dateMonthTime.Month, 25).ToDateDay();
                    var dayReports = (await GetShipDayReportsAsync(Guid.Empty, model.ShipId, startDateDay, endDateDay));
                    var shipStates = dayReports.Select(t => t.ShipState).Distinct().ToArray();
                    var stateDayReports = new List<Tuple<ProjectShipState, List<ShipDayReport>>>();
                    foreach (var shipState in shipStates)
                    {
                        var thisStateDayReports = new Tuple<ProjectShipState, List<ShipDayReport>>(shipState, new List<ShipDayReport>());
                        var thisDayReports = dayReports.Where(t => t.ShipState == shipState).OrderBy(t => t.DateDay).ToList();
                        DateTime? lastTime = null;
                        thisDayReports.ForEach(item =>
                        {
                            ConvertHelper.TryConvertDateTimeFromDateDay(item.DateDay, out DateTime dayTime);
                            if (lastTime == null)
                            {
                                thisStateDayReports.Item2.Add(item);
                            }
                            else
                            {
                                if (lastTime.Value.AddDays(1) == dayTime)
                                {
                                    thisStateDayReports.Item2.Add(item);
                                }
                                else
                                {
                                    stateDayReports.Add(thisStateDayReports);
                                    thisStateDayReports = new Tuple<ProjectShipState, List<ShipDayReport>>(shipState, new List<ShipDayReport>());
                                    thisStateDayReports.Item2.Add(item);
                                }
                            }
                            lastTime = dayTime;
                        });
                        stateDayReports.Add(thisStateDayReports);
                    }
                    stateDayReports.ForEach(item =>
                    {
                        if (item.Item2.Any())
                        {
                            var resDetail = new ShipDynamicMonthReportResponseDto.ResShipDynamicMonthReportDetail();
                            ConvertHelper.TryConvertDateTimeFromDateDay(item.Item2[0].DateDay, out DateTime startTime);
                            ConvertHelper.TryConvertDateTimeFromDateDay(item.Item2.Last().DateDay, out DateTime endTime);
                            resDetail.StartTime = startTime;
                            resDetail.EndTime = endTime;
                            resDetail.PortId = item.Item2[0].PortId;
                            resDetails.Add(resDetail);
                        }
                    });
                }
            }
            else
            {
                var monthReportDetails = await GetShipDynamicMonthReportDetailsAsync(monthReport.Id);
                _mapper.Map(monthReport, resMonthReport);
                resDetails = _mapper.Map<List<ShipDynamicMonthReportDetail>, List<ShipDynamicMonthReportResponseDto.ResShipDynamicMonthReportDetail>>(monthReportDetails);
            }
            resMonthReport.Details = resDetails.ToArray();
            return result.SuccessResult(resMonthReport);
        }

        /// <summary>
        /// 获取一个船舶月报
        /// </summary>
        /// <param name="shipId">船舶Id</param>
        /// <param name="dateMonth">月份</param>
        /// <returns></returns>
        private async Task<ShipDynamicMonthReport?> GetShipDynamicMonthReportAsync(Guid shipId, int dateMonth)
        {
            return await _dbShipDynamicMonthReport.GetFirstAsync(t => t.ShipId == shipId && t.DateMonth == dateMonth && t.IsDelete == 1);
        }

        /// <summary>
        /// 获取船舶月报明细集合
        /// </summary>
        /// <param name="shipDynamicMonthReportId">船舶动态月报Id</param>
        /// <returns></returns>
        private async Task<List<ShipDynamicMonthReportDetail>> GetShipDynamicMonthReportDetailsAsync(Guid shipDynamicMonthReportId)
        {
            return await _dbShipDynamicMonthReportDetail.GetListAsync(t => t.ShipDynamicMonthReportId == shipDynamicMonthReportId && t.IsDelete == 1);
        }

        #endregion

        #region 分包船舶月报

        /// <summary>
        /// 保存一条分包船舶月报
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> SaveSubShipMonthReportAsync(SaveSubShipMonthReportRequestDto model)
        {
            var result = new ResponseAjaxResult<bool>();
            var now = DateTime.Now;
            //if (!(now.Day >= 26 || (now.Day >= 1 && now.Day <= 5)))
            //{
            //    return result.FailResult(HttpStatusCode.SaveFail, "填报时间已过，时间截至在当月26号 - 次月5号");
            //}
            if (!ConvertHelper.TryParseFromDateMonth(model.DateMonth, out DateTime dateMonthTime))
            {
                return result.FailResult(HttpStatusCode.SaveFail, "月份时间格式不正确");
            }
            if (!VerifyDateMonthForSave(model.DateMonth))
            {
                return result.FailResult(HttpStatusCode.SaveFail, "分包船舶月报不可更改");
            }
            var project = await GetProjectPartAsync(model.ProjectId);
            if (project == null)
            {
                return result.FailResult(HttpStatusCode.DataNotEXIST, ResponseMessage.DATA_NOTEXIST_PROJECT);
            }
            var ship = await GetShipPartAsync(model.ShipId, ShipType.SubShip);
            if (ship == null)
            {
                return result.FailResult(HttpStatusCode.DataNotEXIST, "船舶不存在");
            }
            var monthReport = await GetSubShipMonthReportAsync(model.ProjectId, model.ShipId, model.DateMonth);
            bool isAddMonthReport = false;
            if (monthReport == null)
            {
                monthReport = new SubShipMonthReport() { Id = GuidUtil.Next(), CreateId = _currentUser.Id, DateYear = dateMonthTime.ToDateYear() };
                var lastDateMonth = dateMonthTime.AddMonths(-1).ToDateMonth();
                var lastMonthReport = await GetSubShipMonthReportAsync(model.ProjectId, model.ShipId, lastDateMonth);
                // 上个项目月报的（下月预估成本）带到本月存储
                if (lastMonthReport != null)
                {
                    monthReport.MonthPlanProduction = lastMonthReport.NextMonthPlanProduction;
                    monthReport.MonthPlanProductionAmount = lastMonthReport.NextMonthPlanProductionAmount;
                }
                isAddMonthReport = true;
            }
            else
            {
                monthReport.UpdateId = _currentUser.Id;
            }
            monthReport = _mapper.Map(model, monthReport);
            if (isAddMonthReport)
            {
                await _dbSubShipMonthReport.AsInsertable(monthReport).EnableDiffLogEvent(NewLogInfo(EntityType.SubShipMonthReport, monthReport.Id, ModelState.Add)).ExecuteCommandAsync();
            }
            else
            {
                await _dbSubShipMonthReport.AsUpdateable(monthReport).EnableDiffLogEvent(NewLogInfo(EntityType.SubShipMonthReport, monthReport.Id, ModelState.Update)).ExecuteCommandAsync();
            }
            // 记录数据变更
            await _entityChangeService.RecordEntitysChangeAsync(EntityType.SubShipMonthReport, monthReport.Id);
            return result.SuccessResult(true);
        }

        /// <summary>
        /// 搜索一条分包船舶月报
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<SubShipMonthReportResponseDto>> SearchSubShipMonthReportAsync(SubShipMonthReportRequestDto model)
        {
            model.ResetModelProperty();
            var result = new ResponseAjaxResult<SubShipMonthReportResponseDto>();
            var project = await GetProjectPartAsync(model.ProjectId);
            if (project == null)
            {
                return result.FailResult(HttpStatusCode.DataNotEXIST, ResponseMessage.DATA_NOTEXIST_PROJECT);
            }
            var dateMonth = model.DateMonth ?? GetDefaultReportDateMonth();
            var monthReport = await GetSubShipMonthReportAsync(model.ProjectId, model.ShipId, dateMonth);
            var sumReport = await SumMonthReportDetailAsync(model.ProjectId, model.ShipId, ConstructionOutPutType.SubPackage, dateMonth);
            var resMonthReport = new SubShipMonthReportResponseDto();
            if (monthReport == null)
            {
                //船舶进出场
                var shipMovement = await GetShipMovementAsync(model.ProjectId, model.ShipId, ShipType.SubShip);
                resMonthReport.DateMonth = dateMonth;
                resMonthReport.ProjectId = model.ProjectId;
                resMonthReport.ShipId = model.ShipId;
                resMonthReport.EnterTime = shipMovement?.EnterTime;
                resMonthReport.QuitTime = shipMovement?.QuitTime;
                //产值合计（元）
                resMonthReport.ProductionAmount = Math.Round(sumReport.CompleteProductionAmount, 2);
                //产量合计（ m³）
                resMonthReport.Production = Math.Round(sumReport.CompletedQuantity, 2);
            }
            else
            {
                _mapper.Map(monthReport, resMonthReport);
            }

            return result.SuccessResult(resMonthReport);
        }

        /// <summary>
        /// 获取一个分包船舶月报
        /// </summary>
        /// <returns></returns>
        private async Task<SubShipMonthReport?> GetSubShipMonthReportAsync(Guid projectId, Guid shipId, int dateMonth)
        {
            return await _dbSubShipMonthReport.GetFirstAsync(t => t.ProjectId == projectId && t.ShipId == shipId && t.DateMonth == dateMonth && t.IsDelete == 1);
        }

        /// <summary>
        /// 获取分包船舶月报集合
        /// </summary>
        /// <returns></returns>
        private async Task<List<SubShipMonthReport>> GetSubShipMonthReportsAsync(Guid projectId, int dateMonth)
        {
            return await _dbSubShipMonthReport.GetListAsync(t => t.ProjectId == projectId && t.DateMonth == dateMonth && t.IsDelete == 1);

        }

        /// <summary>
        /// 获取分包船舶月报列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <param name="import"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<SearchSubShipMonthRepResponseDto>>> GetSearchSubShipMonthRepAsync(MonthRepRequestDto requestDto, int import)
        {
            var responseDto = new ResponseAjaxResult<List<SearchSubShipMonthRepResponseDto>>();
            if (requestDto.IsDuiWai)
            {
                _currentUser.Account = "2022002867";
                _currentUser.CurrentLoginDepartmentId = "bd840460-1e3a-45c8-abed-6e66903e6465".ToGuid();
                _currentUser.CurrentLoginInstitutionId = "bd840460-1e3a-45c8-abed-6e66903e6465".ToGuid();
                _currentUser.CurrentLoginInstitutionOid = "101162350";
                _currentUser.CurrentLoginInstitutionPoid = "101114066";
                _currentUser.CurrentLoginRoleId = "08db268c-d0d0-4e0f-8a66-39ff15bd3865".ToGuid();
                _currentUser.CurrentLoginIsAdmin = true;
                _currentUser.CurrentLoginUserType = 1;
                _currentUser.Id = "08d63666-7e36-4e20-8024-ee9c96c51623".ToGuid();
            }

            #region 日期控制
            //开始日期结束日期都是空  默认按当天日期匹配
            if ((requestDto.InStartDate == DateTime.MinValue || string.IsNullOrWhiteSpace(requestDto.InStartDate.ToString())) && (requestDto.InEndDate == DateTime.MinValue || string.IsNullOrWhiteSpace(requestDto.InEndDate.ToString())))
            {
                if (DateTime.Now.Day >= 26)
                {
                    requestDto.InStartDate = DateTime.Now;
                    requestDto.InEndDate = DateTime.Now;
                }
                else
                {
                    requestDto.InStartDate = DateTime.Now.AddMonths(-1);
                    requestDto.InEndDate = DateTime.Now.AddMonths(-1);
                }
            }
            else
            {
                if (requestDto.InStartDate > requestDto.InEndDate)
                {
                    responseDto.Data = null;
                    responseDto.FailResult(HttpStatusCode.ParameterError, "传入开始日期大于传入结束日期");
                    return responseDto;
                }
            }
            #endregion
            #region 初始化筛选
            //属性标签
            var categoryList = new List<int>();
            var tagList = new List<int>();
            var tag2List = new List<int>();
            if (requestDto.TagName != null && requestDto.TagName.Any())
            {
                foreach (var item in requestDto.TagName)
                {
                    switch (item)
                    {
                        case "境内":
                            categoryList.Add(0);
                            break;
                        case "境外":
                            categoryList.Add(1);
                            break;
                        case "传统":
                            tagList.Add(0);
                            break;
                        case "新兴":
                            tagList.Add(1);
                            break;
                        case "现汇":
                            tag2List.Add(0);
                            break;
                        case "投资":
                            tag2List.Add(1);
                            break;
                        default:
                            break;
                    }
                }
            }
            List<Guid> departmentIds = new List<Guid>();
            //获取所有机构
            var institution = await _dbContext.Queryable<Institution>().ToListAsync();
            if (_currentUser.CurrentLoginIsAdmin)
            {
                departmentIds = institution.Select(x => x.PomId.Value).ToList();
                departmentIds.Add(_currentUser.CurrentLoginInstitutionId);
            }
            else
            {
                //获取当前登陆人是否属于公司登陆系统 
                var curLogin = institution.Where(x => x.Oid == _currentUser.CurrentLoginInstitutionOid).FirstOrDefault();
                if (curLogin != null)
                {
                    if (curLogin.Ocode.Contains("0000A") || curLogin.Ocode.Contains("0000B") || curLogin.Ocode.Contains("0000C") || curLogin.Ocode.Contains("0000E"))
                    {
                        var reverse = curLogin.Grule.Split('-').Reverse().ToArray()[1];
                        departmentIds = institution.Where(x => x.Grule.Contains(reverse)).Select(x => x.PomId.Value).ToList();
                    }
                    else
                    {
                        departmentIds.Add(_currentUser.CurrentLoginDepartmentId.Value);
                    }
                    departmentIds.Add(curLogin.PomId.Value);
                }
            }
            #endregion
            //当月
            int startMonth = ConvertHelper.ToDateMonth(Convert.ToDateTime(requestDto.InStartDate));
            int endMonth = ConvertHelper.ToDateMonth(Convert.ToDateTime(requestDto.InEndDate));
            #region 根据日期筛选数据
            var subShipMonthRepData = new List<SubShipMonthReport>();
            RefAsync<int> total = 0;
            if (import == 1)
            {
                var ssubShipMonthRepData = _dbContext.Queryable<SubShipMonthReport>()
                                 .LeftJoin<Project>((smr, p) => smr.ProjectId == p.Id)
                                 .LeftJoin<SubShip>((smr, p, sub) => smr.ShipId == sub.PomId)
                                 .WhereIF(requestDto.CompanyId != null, (smr, p) => p.CompanyId == requestDto.CompanyId)
                                 .WhereIF(requestDto.ProjectDept != null, (smr, p) => p.ProjectDept == requestDto.ProjectDept)
                                 .WhereIF(requestDto.ProjectStatusId != null && requestDto.ProjectStatusId.Any(), (smr, p) => requestDto.ProjectStatusId.Contains(p.StatusId.Value.ToString()))
                                 .WhereIF(requestDto.ProjectRegionId != null, (smr, p) => p.RegionId == requestDto.ProjectRegionId)
                                 .WhereIF(requestDto.ProjectTypeId != null, (smr, p) => p.TypeId == requestDto.ProjectTypeId)
                                 .WhereIF(!string.IsNullOrWhiteSpace(requestDto.ProjectName), (smr, p) => SqlFunc.Contains(p.Name, requestDto.ProjectName))
                                 .WhereIF(requestDto.ProjectAreaId! != null, (smr, p) => p.AreaId == requestDto.ProjectAreaId)
                                 .WhereIF(categoryList != null && categoryList.Any(), (smr, p) => categoryList.Contains(p.Category))
                                 .WhereIF(tagList != null && tagList.Any(), (smr, p) => tagList.Contains(p.Tag))
                                 .WhereIF(tag2List != null && tag2List.Any(), (smr, p) => tag2List.Contains(p.Tag2))
                                 .WhereIF(!string.IsNullOrWhiteSpace(requestDto.ShipName), (smr, p, sub) => sub.Name.Contains(requestDto.ShipName))
                                 .WhereIF(requestDto.ShipTypeId != Guid.Empty && !string.IsNullOrWhiteSpace(requestDto.ShipTypeId.ToString()), (smr, p, sub) => requestDto.ShipTypeId == sub.TypeId)
                                 .OrderByDescending((smr, p) => new { p.Id, smr.DateMonth });
                if (!requestDto.IsDuiWai)
                {
                    subShipMonthRepData = await ssubShipMonthRepData
                                .Where((smr, p) => smr.IsDelete == 1 && smr.DateMonth >= startMonth && smr.DateMonth <= endMonth)
                                .ToPageListAsync(requestDto.PageIndex, requestDto.PageSize, total);
                }
                else
                {
                    var res = await ssubShipMonthRepData.ToListAsync();
                    subShipMonthRepData = res
                        .Where(x => string.IsNullOrEmpty(x.UpdateTime.ToString()) || x.UpdateTime == DateTime.MinValue ?
                        x.CreateTime >= requestDto.InStartDate && x.CreateTime <= requestDto.InEndDate
                        : x.UpdateTime >= requestDto.InStartDate && x.UpdateTime <= requestDto.InEndDate)
                        .ToList();
                }
            }
            else
            {
                subShipMonthRepData = await _dbContext.Queryable<SubShipMonthReport>()
                                .LeftJoin<Project>((smr, p) => smr.ProjectId == p.Id)
                                .LeftJoin<SubShip>((smr, p, sub) => smr.ShipId == sub.PomId)
                                .WhereIF(requestDto.CompanyId != null, (smr, p) => p.CompanyId == requestDto.CompanyId)
                                .WhereIF(requestDto.ProjectDept != null, (smr, p) => p.ProjectDept == requestDto.ProjectDept)
                                .WhereIF(requestDto.ProjectStatusId != null && requestDto.ProjectStatusId.Any(), (smr, p) => requestDto.ProjectStatusId.Contains(p.StatusId.Value.ToString()))
                                .WhereIF(requestDto.ProjectRegionId != null, (smr, p) => p.RegionId == requestDto.ProjectRegionId)
                                .WhereIF(requestDto.ProjectTypeId != null, (smr, p) => p.TypeId == requestDto.ProjectTypeId)
                                .WhereIF(!string.IsNullOrWhiteSpace(requestDto.ProjectName), (smr, p) => SqlFunc.Contains(p.Name, requestDto.ProjectName))
                                .WhereIF(requestDto.ProjectAreaId! != null, (smr, p) => p.AreaId == requestDto.ProjectAreaId)
                                .WhereIF(categoryList != null && categoryList.Any(), (smr, p) => categoryList.Contains(p.Category))
                                .WhereIF(tagList != null && tagList.Any(), (smr, p) => tagList.Contains(p.Tag))
                                .WhereIF(tag2List != null && tag2List.Any(), (smr, p) => tag2List.Contains(p.Tag2))
                                .WhereIF(!string.IsNullOrWhiteSpace(requestDto.ShipName), (smr, p, sub) => sub.Name.Contains(requestDto.ShipName))
                                .WhereIF(requestDto.ShipTypeId != Guid.Empty && !string.IsNullOrWhiteSpace(requestDto.ShipTypeId.ToString()), (smr, p, sub) => requestDto.ShipTypeId == sub.TypeId)
                                .OrderByDescending((smr, p) => smr.DateMonth)
                                .ToListAsync();
                if (!requestDto.IsDuiWai)
                {
                    subShipMonthRepData = subShipMonthRepData
                                .Where((smr, p) => smr.IsDelete == 1 && smr.DateMonth >= startMonth && smr.DateMonth <= endMonth)
                                .ToList();
                }
                else
                {
                    subShipMonthRepData = subShipMonthRepData
                        .Where(x => string.IsNullOrEmpty(x.UpdateTime.ToString()) || x.UpdateTime == DateTime.MinValue ?
                        x.CreateTime >= requestDto.InStartDate && x.CreateTime <= requestDto.InEndDate
                        : x.UpdateTime >= requestDto.InStartDate && x.UpdateTime <= requestDto.InEndDate)
                        .ToList();
                }
            }
            //年度产值 、 产量、运转时间、施工天数
            //取最新年份的数据
            int year = startMonth > endMonth ? Convert.ToInt32(startMonth.ToString().Substring(0, 4)) : Convert.ToInt32(endMonth.ToString().Substring(0, 4));
            var subShipYearData = await _dbContext.Queryable<SubShipMonthReport>().Where(x => x.IsDelete == 1 && x.DateYear == year).GroupBy(x => x.ShipId).Select(x => new { x.ShipId, YearWorkHours = SqlFunc.AggregateSum(x.WorkingHours), YearWorkDays = SqlFunc.AggregateSum(x.ConstructionDays), YearQuantity = SqlFunc.AggregateSum(x.Production), YearOutputVal = SqlFunc.AggregateSum(x.ProductionAmount) }).ToListAsync();
            //累计完成产值、工程量
            var subAccumulateData = await _dbContext.Queryable<SubShipMonthReport>().Where(x => x.IsDelete == 1).GroupBy(x => x.ShipId).Select(x => new { x.ShipId, AccumulateQuantity = SqlFunc.AggregateSum(x.Production), AccumulateOutputVal = SqlFunc.AggregateSum(x.ProductionAmount) }).ToListAsync();
            //获取项目ids、所属公司
            var proIds = subShipMonthRepData.Select(x => x.ProjectId).ToList();
            var proData = await _dbContext.Queryable<Project>().Where(x => x.IsDelete == 1 && proIds.Contains(x.Id)).Select(x => new { x.Id, x.Name, x.CompanyId, x.ReportForMertel, x.RegionId, x.AreaId }).ToListAsync();
            //获取船舶ids
            var subShipIds = subShipMonthRepData.Select(x => x.ShipId).ToList();
            //获取船舶名称
            var subShipData = await _dbContext.Queryable<SubShip>().Where(x => subShipIds.Contains(x.PomId)).Select(x => new { x.PomId, x.Name, x.TypeId, x.CompanyId }).ToListAsync();
            //获取船舶类型
            var shipTypeIds = subShipData.Select(x => x.TypeId).ToList();
            var shipTypeData = await _dbContext.Queryable<SubShip>()
                .LeftJoin<ShipPingType>((os, st) => os.TypeId == st.PomId)
                .Where((os, st) => os.IsDelete == 1 && st.IsDelete == 1 && subShipIds.Contains(os.PomId) && shipTypeIds.Contains(os.TypeId)).Select((os, st) => new { ShipId = os.PomId, ShipTypeId = st.PomId, st.Name }).ToListAsync();
            //获取机构
            var instinIds = proData.Select(x => x.CompanyId).ToList();
            var instinData = await _dbContext.Queryable<Project>()
                .LeftJoin<Institution>((p, i) => p.CompanyId == i.PomId)
                .Where((p, i) => instinIds.Contains(p.CompanyId) && proIds.Contains(p.Id) && p.IsDelete == 1 && i.IsDelete == 1)
                .Select((p, i) => new { i.PomId, i.Name, p.Id }).ToListAsync();
            //单位机构
            var unitIds = subShipData.Select(x => x.CompanyId.ToString()).ToList();
            var unitData = await _dbContext.Queryable<SubShipMonthReport>()
                .LeftJoin<SubShip>((rep, sub) => rep.ShipId == sub.PomId)
                .LeftJoin<DealingUnit>((rep, sub, dea) => sub.CompanyId == dea.PomId.ToString())
                .Where((rep, sub, dea) => unitIds.Contains(dea.PomId.ToString()) && proIds.Contains(rep.ProjectId) && rep.IsDelete == 1 && rep.IsDelete == 1)
                .Select((rep, sub, dea) => new { dea.ZBPNAME_ZH, rep.ProjectId }).ToListAsync();
            //合同清单类型
            var contractData = await _dbContext.Queryable<DictionaryTable>().Where(x => x.TypeNo == 9 && x.IsDelete == 1).Select(s => new { s.Type, s.Name }).ToListAsync();
            //动态概述
            var dynamicData = await _dbContext.Queryable<DictionaryTable>().Where(x => x.TypeNo == 10 && x.IsDelete == 1).Select(s => new { s.Type, s.Name }).ToListAsync();
            //区域
            var regionIds = proData.Select(x => x.RegionId).ToList();
            var regionData = await _dbContext.Queryable<Project>()
                .LeftJoin<ProjectArea>((p, pa) => p.RegionId == pa.AreaId)
                .Where((p, pa) => p.IsDelete == 1 && pa.IsDelete == 1 && regionIds.Contains(p.RegionId)).Select((p, pa) => new { p.AreaId, pa.Name, p.Id }).ToListAsync();
            //地点
            var areaIds = proData.Select(x => x.AreaId).ToList();
            var areaData = await _dbContext.Queryable<Project>()
                .LeftJoin<Province>((p, pro) => p.AreaId == pro.PomId)
                .Where((p, pro) => p.IsDelete == 1 && pro.IsDelete == 1 && areaIds.Contains(p.AreaId)).Select((p, pro) => new { pro.PomId, pro.Zaddvsname, p.Id }).ToListAsync();
            var result = new List<SearchSubShipMonthRepResponseDto>();
            subShipMonthRepData.ForEach(x => result.Add(new SearchSubShipMonthRepResponseDto
            {
                Id = x.Id,
                SubShipName = subShipData.FirstOrDefault(y => y.PomId == x.ShipId)?.Name,
                ShipTypeName = shipTypeData.FirstOrDefault(y => y.ShipId == x.ShipId)?.Name,
                UnitName = unitData.FirstOrDefault(y => y.ProjectId == x.ProjectId)?.ZBPNAME_ZH,
                EnterTime = x.EnterTime.ToString("yyyy-MM-dd"),
                QuitTime = x.QuitTime.ToString("yyyy-MM-dd"),
                ProjectName = proData.FirstOrDefault(y => y.Id == x.ProjectId)?.Name,
                RegionName = regionData.FirstOrDefault(y => y.Id == x.ProjectId)?.Name,
                CountryName = areaData.FirstOrDefault(y => y.Id == x.ProjectId)?.Zaddvsname,
                MonthWorkHours = x.WorkingHours,
                MonthWorkDays = x.ConstructionDays,
                MonthQuantity = x.Production,
                MonthOutputVal = x.ProductionAmount,
                MonthPlanOutputVal = x.MonthPlanProductionAmount,
                MonthPlanQuantity = x.MonthPlanProduction,
                AccumulateOutputVal = subAccumulateData.FirstOrDefault(y => y.ShipId == x.ShipId).AccumulateOutputVal,
                AccumulateQuantity = subAccumulateData.FirstOrDefault(y => y.ShipId == x.ShipId).AccumulateQuantity,
                YearWorkHours = subShipYearData.Any() ? subShipYearData.FirstOrDefault(y => y.ShipId == x.ShipId).YearWorkHours : 0,
                YearWorkDays = subShipYearData.Any() ? subShipYearData.FirstOrDefault(y => y.ShipId == x.ShipId).YearWorkDays : 0,
                YearQuantity = subShipYearData.Any() ? subShipYearData.FirstOrDefault(y => y.ShipId == x.ShipId).YearQuantity : 0,
                YearOutputVal = subShipYearData.Any() ? subShipYearData.FirstOrDefault(y => y.ShipId == x.ShipId).YearOutputVal : 0,
                SecUnitName = "广航局",
                ThiUnitName = instinData.FirstOrDefault(y => y.Id == x.ProjectId)?.Name,
                DynamicContent = dynamicData.FirstOrDefault(y => y.Type == x.DynamicDescriptionType)?.Name,
                ShipDynamic = x.DynamicDescriptionType,
                ContractTypeName = contractData.FirstOrDefault(y => y.Type == x.ContractDetailType)?.Name,
                IsExamine = 0,
                SubmitDate = MonthDate(x.DateMonth),
                ProjectId = x.ProjectId,
                SubShipId = x.ShipId,
                UpdateTime = x.UpdateTime
            }));
            #endregion
            responseDto.Count = total;
            responseDto.Data = result;
            responseDto.Success();
            return responseDto;
        }
        /// <summary>
        /// 日期转换
        /// </summary>
        /// <param name="dateMonth"></param>
        /// <returns></returns>
        public DateTime MonthDate(int dateMonth)
        {
            ConvertHelper.TryParseFromDateMonth(dateMonth, out DateTime monthDate);
            return monthDate;
        }
        /// <summary>
        /// 分包船舶月报导出
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<ExcelImportResponseDto>> SubShipMonthRepExcel(MonthRepRequestDto requestDto)
        {
            var responseDto = new ResponseAjaxResult<ExcelImportResponseDto>();
            var excelImpDto = new ExcelImportResponseDto();
            var responseData = await GetSearchSubShipMonthRepAsync(requestDto, 2);
            var result = new List<SubShipMonthRepExcelDto>();
            responseData.Data.ForEach(x => result.Add(new SubShipMonthRepExcelDto
            {
                SubShipName = x.SubShipName,
                ContractTypeName = x.ContractTypeName,
                EnterTime = x.EnterTime,
                IsExamine = x.IsExamine == 0 ? "否" : "是",
                MonthOutputVal = x.MonthOutputVal,
                MonthQuantity = x.MonthQuantity,
                MonthWorkDays = x.MonthWorkDays,
                MonthWorkHours = x.MonthWorkHours,
                ProjectName = x.ProjectName,
                QuitTime = x.QuitTime,
                SecUnitName = x.SecUnitName,
                ShipTypeName = x.ShipTypeName,
                ThiUnitName = x.ThiUnitName,
                YearOutputVal = x.YearOutputVal,
                YearQuantity = x.YearQuantity,
                YearWorkDays = x.YearWorkDays,
                YearWorkHours = x.YearWorkHours,
                SubmitDate = x.SubmitDate.ToString("yyyy-MM-dd"),
                AccumulateOutputVal = x.AccumulateOutputVal,
                AccumulateQuantity = x.AccumulateQuantity,
                CountryName = x.CountryName,
                DynamicContent = x.DynamicContent,
                MonthPlanOutputVal = x.MonthPlanOutputVal,
                MonthPlanQuantity = x.MonthPlanQuantity,
                RegionName = x.RegionName,
                UnitName = x.UnitName
            }));
            excelImpDto.SubShipMonthRepExcelDtos = result;
            responseDto.Data = excelImpDto;
            responseDto.Success();
            return responseDto;
        }
        #endregion

        #region 自有船舶月报

        /// <summary>
        /// 保存一条自有船舶月报
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> SaveOwnerShipMonthReportAsync(SaveOwnerShipMonthReportRequestDto model)
        {
            var result = new ResponseAjaxResult<bool>();
            var now = DateTime.Now;
            //if (!(now.Day >= 26 || (now.Day >= 1 && now.Day <= 5)))
            //{
            //    return result.FailResult(HttpStatusCode.SaveFail, "填报时间已过，时间截至在当月26号 - 次月5号");
            //}
            if (!ConvertHelper.TryParseFromDateMonth(model.DateMonth, out DateTime dateMonthTime))
            {
                return result.FailResult(HttpStatusCode.SaveFail, "月份时间格式不正确");
            }
            if (!VerifyDateMonthForSave(model.DateMonth))
            {
                return result.FailResult(HttpStatusCode.SaveFail, "自有船舶月报不可更改");
            }
            var project = await GetProjectPartAsync(model.ProjectId);
            if (project == null)
            {
                return result.FailResult(HttpStatusCode.DataNotEXIST, ResponseMessage.DATA_NOTEXIST_PROJECT);
            }
            var ship = await GetShipPartAsync(model.ShipId, ShipType.OwnerShip);
            if (ship == null)
            {
                return result.FailResult(HttpStatusCode.DataNotEXIST, ResponseMessage.DATA_NOTEXIST_OWNERSHIP);
            }
            var contractDetailType = await GetDictionaryTableObject(DictionaryTypeNo.ContractDetail, (int)model.ContractDetailType);
            if (contractDetailType == null)
            {
                return result.FailResult(HttpStatusCode.DataNotEXIST, "合同清单类型不存在");
            }
            var lastDateMonthTime = dateMonthTime.AddMonths(-1);
            var startDateDay = new DateTime(lastDateMonthTime.Year, lastDateMonthTime.Month, 26).ToDateDay();
            var endDateDay = new DateTime(dateMonthTime.Year, dateMonthTime.Month, 25).ToDateDay();
            var monthReport = await GetOwnerShipMonthReportAsync(model.ProjectId, model.ShipId, model.DateMonth);
            var sumMonthReport = await SumMonthReportDetailAsync(model.ProjectId, model.ShipId, ConstructionOutPutType.Self, model.DateMonth);
            //项目信息
            var projectList = await _dbContext.Queryable<Project>().Where(x => x.IsDelete == 1).ToListAsync();
            var sipMovementList = await _dbContext.Queryable<ShipMovement>().Where(x => x.IsDelete == 1).ToListAsync();
            var sumShipDayReport = await SumShipDayReportAsync(model.ProjectId, model.ShipId, startDateDay, endDateDay, projectList, sipMovementList);
            var addSoils = new List<OwnerShipMonthReportSoil>();
            var updateSoils = new List<OwnerShipMonthReportSoil>();
            var removeSoils = new List<OwnerShipMonthReportSoil>();
            bool isAddMonthReport = false;
            if (monthReport == null)
            {
                monthReport = new OwnerShipMonthReport() { Id = GuidUtil.Next(), CreateId = _currentUser.Id, DateYear = dateMonthTime.ToDateYear() };
                addSoils = _mapper.Map(model.Soils, addSoils);
                isAddMonthReport = true;
            }
            else
            {
                var oldSoils = await GetOwnerShipMonthReportSoilsAsync(monthReport.Id);
                var exsitSoilIds = oldSoils.Where(t => model.Soils.Any(r => r.MonthReportSoilId == t.Id)).Select(t => (Guid?)t.Id).ToArray();
                var addReqSoils = model.Soils.Where(t => !exsitSoilIds.Contains(t.MonthReportSoilId)).ToList();
                monthReport.UpdateId = _currentUser.Id;
                addSoils = _mapper.Map(addReqSoils, addSoils);
                removeSoils = oldSoils.Where(t => !exsitSoilIds.Contains(t.Id)).ToList();
                updateSoils = oldSoils.Where(t => exsitSoilIds.Contains(t.Id)).ToList();
            }
            monthReport = _mapper.Map(model, monthReport);
            // 运转时间
            monthReport.WorkingHours = sumShipDayReport.WorkingHours;
            // 产值/产量
            monthReport.ProductionAmount = sumMonthReport.CompleteProductionAmount;
            monthReport.Production = sumMonthReport.CompletedQuantity;
            // 数据填充
            addSoils.ForEach(soil =>
            {
                soil.Id = GuidUtil.Next();
                soil.CreateId = _currentUser.Id;
                soil.OwnerShipMonthReportId = monthReport.Id;
                soil.ProjectId = monthReport.ProjectId;
                soil.ShipId = monthReport.ShipId;
                soil.DateMonth = monthReport.DateMonth;
                soil.DateYear = monthReport.DateYear;
            });
            updateSoils.ForEach(soil =>
            {
                var updateReqSoil = model.Soils.Single(t => t.MonthReportSoilId == soil.Id);
                _mapper.Map(updateReqSoil, soil);
                soil.UpdateId = _currentUser.Id;
            });
            removeSoils.ForEach(soil =>
            {
                soil.DeleteId = _currentUser.Id;
                soil.IsDelete = 0;
            });
            var modelState = isAddMonthReport ? ModelState.Add : ModelState.Update;
            if (isAddMonthReport)
            {
                await _dbOwnerShipMonthReport.AsInsertable(monthReport).EnableDiffLogEvent(NewLogInfo(EntityType.OwnerShipMonthReport, monthReport.Id, modelState)).ExecuteCommandAsync();
            }
            else
            {
                await _dbOwnerShipMonthReport.AsUpdateable(monthReport).EnableDiffLogEvent(NewLogInfo(EntityType.OwnerShipMonthReport, monthReport.Id, modelState)).ExecuteCommandAsync();
            }
            if (removeSoils.Any())
            {
                await _dbOwnerShipMonthReportSoil.AsUpdateable(removeSoils).UpdateColumns(t => new { t.IsDelete, t.DeleteId, t.DeleteTime }).EnableDiffLogEvent(NewLogInfo(EntityType.OwnerShipMonthReport, monthReport.Id, modelState)).ExecuteCommandAsync();
            }
            if (updateSoils.Any())
            {
                await _dbOwnerShipMonthReportSoil.AsUpdateable(updateSoils).EnableDiffLogEvent(NewLogInfo(EntityType.OwnerShipMonthReport, monthReport.Id, modelState)).ExecuteCommandAsync();
            }
            if (addSoils.Any())
            {
                await _dbOwnerShipMonthReportSoil.AsInsertable(addSoils).EnableDiffLogEvent(NewLogInfo(EntityType.OwnerShipMonthReport, monthReport.Id, modelState)).ExecuteCommandAsync();
            }
            // 记录数据变更
            await _entityChangeService.RecordEntitysChangeAsync(EntityType.OwnerShipMonthReport, monthReport.Id);
            return result.SuccessResult(true);
        }

        /// <summary>
        /// 搜索一条自有船舶月报
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<OwnerShipMonthReportResponseDto>> SearchOwnerShipMonthReportAsync(OwnerShipMonthReportRequestDto model)
        {
            model.ResetModelProperty();
            var result = new ResponseAjaxResult<OwnerShipMonthReportResponseDto>();
            var project = await GetProjectPartAsync(model.ProjectId);
            if (project == null)
            {
                return result.FailResult(HttpStatusCode.DataNotEXIST, ResponseMessage.DATA_NOTEXIST_PROJECT);
            }
            var dateMonth = model.DateMonth ?? GetDefaultReportDateMonth();
            var monthReport = await GetOwnerShipMonthReportAsync(model.ProjectId, model.ShipId, dateMonth);
            //判断月份天数范围
            ConvertHelper.TryParseFromDateMonth(dateMonth, out DateTime dateMonthTime);
            var resMonthReport = new OwnerShipMonthReportResponseDto();
            var projectList = await _dbContext.Queryable<Project>().Where(x => x.IsDelete == 1).ToListAsync();
            var sipMovementList = await _dbContext.Queryable<ShipMovement>().Where(x => x.IsDelete == 1).ToListAsync();
            if (monthReport == null)
            {
                var lastDateMonthTime = dateMonthTime.AddMonths(-1);
                var startDateDay = new DateTime(lastDateMonthTime.Year, lastDateMonthTime.Month, 26).ToDateDay();
                var endDateDay = new DateTime(dateMonthTime.Year, dateMonthTime.Month, 25).ToDateDay();
                var sumMonthReport = await SumMonthReportDetailAsync(model.ProjectId, model.ShipId, ConstructionOutPutType.Self, dateMonth);
                var sumShipDayReport = await SumShipDayReportAsync(model.ProjectId, model.ShipId, startDateDay, endDateDay, projectList, sipMovementList);
                //获取船舶进出场
                var shipMovement = await GetShipMovementAsync(model.ProjectId, model.ShipId, ShipType.OwnerShip);
                resMonthReport.DateMonth = dateMonth;
                resMonthReport.ProjectId = model.ProjectId;
                resMonthReport.ShipId = model.ShipId;
                resMonthReport.EnterTime = shipMovement?.EnterTime;
                resMonthReport.QuitTime = shipMovement?.QuitTime;
                // 运转时间
                resMonthReport.WorkingHours = sumShipDayReport.WorkingHours;
                // 产值合计（元）
                resMonthReport.ProductionAmount = sumMonthReport.CompleteProductionAmount;
                // 产量合计（ m³）
                resMonthReport.Production = sumMonthReport.CompletedQuantity;
            }
            else
            {
                var monthReportSoils = await GetOwnerShipMonthReportSoilsAsync(monthReport.Id);
                _mapper.Map(monthReport, resMonthReport);
                resMonthReport.Soils = _mapper.Map<OwnerShipMonthReportResponseDto.ResOwnerShipMonthReportSoil[]>(monthReportSoils);
            }
            resMonthReport.ProductionAmount = Math.Round(resMonthReport.ProductionAmount, 2);
            resMonthReport.Production = Math.Round(resMonthReport.Production, 2);
            return result.SuccessResult(resMonthReport);
        }

        /// <summary>
        /// 获取一条自有船舶月报
        /// </summary>
        /// <returns></returns>
        private async Task<OwnerShipMonthReport?> GetOwnerShipMonthReportAsync(Guid projectId, Guid shipId, int dateMonth
            )
        {
            return await _dbOwnerShipMonthReport.GetFirstAsync(t => t.ProjectId == projectId && t.ShipId == shipId && t.DateMonth == dateMonth && t.IsDelete == 1);

        }

        /// <summary>
        /// 获取一条自有船舶月报-施工土质集合
        /// </summary>
        /// <returns></returns>
        private async Task<List<OwnerShipMonthReportSoil>> GetOwnerShipMonthReportSoilsAsync(Guid ownerShipMonthReportId)
        {
            return await _dbOwnerShipMonthReportSoil.GetListAsync(t => t.OwnerShipMonthReportId == ownerShipMonthReportId && t.IsDelete == 1);
        }

        /// <summary>
        /// 获取自有船舶月报集合
        /// </summary>
        /// <returns></returns>
        private async Task<List<OwnerShipMonthReport>> GetOwnerShipMonthReportsAsync(Guid projectId, int dateMonth)
        {
            return await _dbOwnerShipMonthReport.GetListAsync(t => t.ProjectId == projectId && t.DateMonth == dateMonth && t.IsDelete == 1);

        }

        /// <summary>
        /// 统计船舶月报
        /// </summary>
        /// <returns></returns>
        private async Task<List<SumOwnerShipMonthReportDto>> SumOwnerShipMonthReportsAsync(Guid[] projectIds, Guid[] shipIds, int startMonth, int endMonth)
        {
            return await _dbOwnerShipMonthReport.AsQueryable()
                  .Where(t => t.IsDelete == 1 && projectIds.Contains(t.ProjectId) && t.DateMonth >= startMonth && t.DateMonth <= endMonth && shipIds.Contains(t.ShipId))
                   .GroupBy(t => new { t.ProjectId, t.ShipId })
                   .Select(t => new SumOwnerShipMonthReportDto
                   {
                       ProjectId = t.ProjectId,
                       ShipId = t.ShipId,
                       Production = SqlFunc.AggregateSum(t.Production),
                       ProductionAmount = SqlFunc.AggregateSum(t.ProductionAmount)
                   }).ToListAsync();
        }

        /// <summary>
        /// 项目月报-船舶-历史数据差值统计
        /// </summary>
        /// <returns></returns>
        private async Task<List<SumHistoryOwnerShipReport>> SumHistoryPojectShipReportAsync(int dateYear)
        {
            var sumList = new List<SumHistoryOwnerShipReport>();
            if (dateYear != 2023)
            {
                return sumList;
            }
            int startMonth = 202301;
            int endMonth = 202310;
            var historyPojectShipReports = await _dbContext.Queryable<HistoryOwnerShipReport>().Where(t => t.IsDelete == 1).ToListAsync();
            var historyPojectIds = historyPojectShipReports.Select(t => t.ProjectId).Distinct().ToArray();
            var historyOwnerShipIds = historyPojectShipReports.Select(t => t.ShipId).Distinct().ToArray();
            var sumShipReports = await SumOwnerShipMonthReportsAsync(historyPojectIds, historyOwnerShipIds, startMonth, endMonth);
            historyPojectShipReports.ForEach(item =>
            {
                var sumItem = new SumHistoryOwnerShipReport() { ProjectId = item.ProjectId, ShipId = item.ShipId };
                var sumShipReport = sumShipReports.FirstOrDefault(t => t.ProjectId == item.ProjectId && t.ShipId == item.ShipId);
                var sumProduction = (sumShipReport?.Production) ?? 0;
                var sumProductionAmount = (sumShipReport?.ProductionAmount) ?? 0;
                var historyDiffProduction = item.HistoryProduction - sumProduction;
                var historyDiffProductionAmount = item.HistoryProductionAmount - sumProductionAmount;
                sumItem.HistoryDiffProduction = historyDiffProduction < 0 ? 0 : historyDiffProduction;
                sumItem.HistoryDiffProductionAmount = historyDiffProductionAmount < 0 ? 0 : historyDiffProductionAmount;
                sumList.Add(sumItem);
            });
            return sumList;
        }

        #endregion

        /// <summary>
        /// 获取月报船舶列表
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<ShipsForReportResponseDto>> SearchShipsForMonthReportAsync(ShipsForReportRequestDto model)
        {
            model.ResetModelProperty();
            var result = new ResponseAjaxResult<ShipsForReportResponseDto>();
            var resShipsForReport = new ShipsForReportResponseDto();
            var dateMonth = model.DateMonth ?? GetDefaultReportDateMonth();
            var project = await GetProjectPartAsync(model.ProjectId);
            RefAsync<int> total = 0;
            //分页筛选(进场或出场)船舶集合
            var resShips = await _dbShipMovement.AsQueryable().Where(t => t.IsDelete == 1 && t.ProjectId == model.ProjectId && (t.Status == ShipMovementStatus.Enter || t.Status == ShipMovementStatus.Quit))
                .OrderByDescending(t => t.CreateTime)
                .Select(t => new ShipsForReportResponseDto.ResShipForReport()
                {
                    ShipId = t.ShipId,
                    ProjectId = t.ProjectId,
                    ShipType = t.ShipType,
                    Remarks = t.Remarks,
                }).ToPageListAsync(model.PageIndex, model.PageSize, total);
            var ownerShipIds = resShips.Where(t => t.ShipType == ShipType.OwnerShip).Select(t => t.ShipId).ToArray();
            var subShipIds = resShips.Where(t => t.ShipType == ShipType.SubShip).Select(t => t.ShipId).ToArray();
            var ships = await GetShipPartsAsync(ownerShipIds, ShipType.OwnerShip);
            ships.AddRange(await GetShipPartsAsync(subShipIds, ShipType.SubShip));
            var shipKindTypeIds = ships.Where(t => t.ShipKindTypeId != null).Select(t => t.ShipKindTypeId).Distinct().ToArray();
            // 船舶类型
            var shipKindTypes = await GetShipPingTypesAsync(shipKindTypeIds);
            // 自有船舶月报
            var ownerShipMonthReports = await GetOwnerShipMonthReportsAsync(model.ProjectId, dateMonth);
            // 分包船舶月报
            var subShipMonthReports = await GetSubShipMonthReportsAsync(model.ProjectId, dateMonth);
            // 数据填充
            resShips.ForEach(resShip =>
            {
                var ship = ships.FirstOrDefault(t => t.PomId == resShip.ShipId && t.ShipType == resShip.ShipType);
                resShip.DateMonth = dateMonth;
                resShip.ProjectName = project?.Name;
                resShip.ProjectId = project == null ? Guid.Empty : resShip.ProjectId == Guid.Empty ? project.Id : resShip.ProjectId;
                resShip.ShipName = ship?.Name;
                resShip.ShipKindTypeName = shipKindTypes.FirstOrDefault(t => t.PomId == ship?.ShipKindTypeId)?.Name;
                if (resShip.ShipType == ShipType.OwnerShip)
                {
                    var shipMonthReport = ownerShipMonthReports.FirstOrDefault(t => t.ShipId == resShip.ShipId);
                    resShip.DateMonth = shipMonthReport?.DateMonth;
                    resShip.FillReportStatus = shipMonthReport == null ? FillReportStatus.UnReport : FillReportStatus.Reported;
                }
                else if (resShip.ShipType == ShipType.SubShip)
                {
                    var shipMonthReport = subShipMonthReports.FirstOrDefault(t => t.ShipId == resShip.ShipId);
                    resShip.DateMonth = shipMonthReport?.DateMonth;
                    resShip.FillReportStatus = shipMonthReport == null ? FillReportStatus.UnReport : FillReportStatus.Reported;
                }
            });
            resShipsForReport.Ships = resShips.ToArray();
            resShipsForReport.DateMonth = dateMonth;
            return result.SuccessResult(resShipsForReport, total);
        }

        /// <summary>
        /// 自有船舶月报列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <param name="import"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<SearchOwnShipMonthRepResponseDto>> GetSearchOwnShipMonthRepAsync(MonthRepRequestDto requestDto, int import)
        {
            var responseDto = new ResponseAjaxResult<SearchOwnShipMonthRepResponseDto>();
            var list = new SearchOwnShipMonthRepResponseDto();
            var sumInfo = new SumOwnShipMonth();
            if (requestDto.IsDuiWai)
            {
                _currentUser.Account = "2022002867";
                _currentUser.CurrentLoginDepartmentId = "bd840460-1e3a-45c8-abed-6e66903e6465".ToGuid();
                _currentUser.CurrentLoginInstitutionId = "bd840460-1e3a-45c8-abed-6e66903e6465".ToGuid();
                _currentUser.CurrentLoginInstitutionOid = "101162350";
                _currentUser.CurrentLoginInstitutionPoid = "101114066";
                _currentUser.CurrentLoginRoleId = "08db268c-d0d0-4e0f-8a66-39ff15bd3865".ToGuid();
                _currentUser.CurrentLoginIsAdmin = true;
                _currentUser.CurrentLoginUserType = 1;
                _currentUser.Id = "08d63666-7e36-4e20-8024-ee9c96c51623".ToGuid();
            }
            #region 日期控制
            //开始日期结束日期都是空  默认按当天日期匹配
            if ((requestDto.InStartDate == DateTime.MinValue || string.IsNullOrWhiteSpace(requestDto.InStartDate.ToString())) && (requestDto.InEndDate == DateTime.MinValue || string.IsNullOrWhiteSpace(requestDto.InEndDate.ToString())))
            {
                if (DateTime.Now.Day >= 26)
                {
                    requestDto.InStartDate = DateTime.Now;
                    requestDto.InEndDate = DateTime.Now;
                }
                else
                {
                    requestDto.InStartDate = DateTime.Now.AddMonths(-1);
                    requestDto.InEndDate = DateTime.Now.AddMonths(-1);
                }
            }
            else
            {
                if (requestDto.InStartDate > requestDto.InEndDate)
                {
                    responseDto.Data = null;
                    responseDto.FailResult(HttpStatusCode.ParameterError, "传入开始日期大于结束日期");
                    return responseDto;
                }
            }
            #endregion
            //当月
            int startMonth = ConvertHelper.ToDateMonth(Convert.ToDateTime(requestDto.InStartDate));
            int endMonth = ConvertHelper.ToDateMonth(Convert.ToDateTime(requestDto.InEndDate));
            #region 初始化筛选
            //属性标签
            var categoryList = new List<int>();
            var tagList = new List<int>();
            var tag2List = new List<int>();
            if (requestDto.TagName != null && requestDto.TagName.Any())
            {
                foreach (var item in requestDto.TagName)
                {
                    switch (item)
                    {
                        case "境内":
                            categoryList.Add(0);
                            break;
                        case "境外":
                            categoryList.Add(1);
                            break;
                        case "传统":
                            tagList.Add(0);
                            break;
                        case "新兴":
                            tagList.Add(1);
                            break;
                        case "现汇":
                            tag2List.Add(0);
                            break;
                        case "投资":
                            tag2List.Add(1);
                            break;
                        default:
                            break;
                    }
                }
            }
            List<Guid> departmentIds = new List<Guid>();
            //获取所有机构
            var institution = await _dbContext.Queryable<Institution>().ToListAsync();
            if (_currentUser.CurrentLoginIsAdmin)
            {
                departmentIds = institution.Select(x => x.PomId.Value).ToList();
                departmentIds.Add(_currentUser.CurrentLoginInstitutionId);
            }
            else
            {
                //获取当前登陆人是否属于公司登陆系统 
                var curLogin = institution.Where(x => x.Oid == _currentUser.CurrentLoginInstitutionOid).FirstOrDefault();
                if (curLogin != null)
                {
                    if (curLogin.Ocode.Contains("0000A") || curLogin.Ocode.Contains("0000B") || curLogin.Ocode.Contains("0000C") || curLogin.Ocode.Contains("0000E"))
                    {
                        var reverse = curLogin.Grule.Split('-').Reverse().ToArray()[1];
                        departmentIds = institution.Where(x => x.Grule.Contains(reverse)).Select(x => x.PomId.Value).ToList();
                    }
                    else
                    {
                        departmentIds.Add(_currentUser.CurrentLoginDepartmentId.Value);
                    }
                    departmentIds.Add(curLogin.PomId.Value);
                }
            }
            #endregion

            #region 根据日期筛选数据
            var ownShipMonthRepData = new List<OwnerShipMonthReport>();
            RefAsync<int> total = 0;
            var shipId = new Guid[] { };
            if (import == 1)//列表
            {
                var data = _dbContext.Queryable<OwnerShipMonthReport>()
                                    .LeftJoin<Project>((osm, p) => osm.ProjectId == p.Id)
                                    .LeftJoin<OwnerShip>((osm, p, own) => osm.ShipId == own.PomId)
                                    //.Where((osm, p) => osm.IsDelete == 1 && osm.DateMonth >= startMonth && osm.DateMonth <= endMonth)
                                    .WhereIF(requestDto.CompanyId != null, (osm, p) => p.CompanyId == requestDto.CompanyId)
                                    .WhereIF(requestDto.ProjectDept != null, (osm, p) => p.ProjectDept == requestDto.ProjectDept)
                                    .WhereIF(requestDto.ProjectStatusId != null && requestDto.ProjectStatusId.Any(), (osm, p) => requestDto.ProjectStatusId.Contains(p.StatusId.Value.ToString()))
                                    .WhereIF(requestDto.ProjectRegionId != null, (osm, p) => p.RegionId == requestDto.ProjectRegionId)
                                    .WhereIF(requestDto.ProjectTypeId != null, (osm, p) => p.TypeId == requestDto.ProjectTypeId)
                                    .WhereIF(!string.IsNullOrWhiteSpace(requestDto.ProjectName), (osm, p) => SqlFunc.Contains(p.Name, requestDto.ProjectName))
                                    .WhereIF(requestDto.ProjectAreaId! != null, (osm, p) => p.AreaId == requestDto.ProjectAreaId)
                                    .WhereIF(categoryList != null && categoryList.Any(), (osm, p) => categoryList.Contains(p.Category))
                                    .WhereIF(tagList != null && tagList.Any(), (osm, p) => tagList.Contains(p.Tag))
                                    .WhereIF(tag2List != null && tag2List.Any(), (osm, p) => tag2List.Contains(p.Tag2))
                                    .WhereIF(!string.IsNullOrWhiteSpace(requestDto.ShipName), (osm, p, own) => own.PomId.ToString() == requestDto.ShipName)
                                    .WhereIF(requestDto.ShipTypeId != Guid.Empty && !string.IsNullOrWhiteSpace(requestDto.ShipTypeId.ToString()), (osm, p, own) => requestDto.ShipTypeId == own.TypeId)
                                    .OrderByDescending((osm, p) => new { p.Id, osm.DateMonth });
                if (!requestDto.IsDuiWai)//监控中心自己用 非对外接口
                {
                    data = data.Where((osm, p) => osm.IsDelete == 1 && osm.DateMonth >= startMonth && osm.DateMonth <= endMonth);
                    shipId = data.Select(osm => osm.ShipId).ToList().Distinct().ToArray();
                    sumInfo.SumMonthOutputVal = Math.Round(data.Sum(osm => osm.ProductionAmount), 2);
                    sumInfo.SumMonthQuantity = Math.Round(data.Sum(osm => osm.Production), 2);
                    ownShipMonthRepData = await data.ToPageListAsync(requestDto.PageIndex, requestDto.PageSize, total);
                }
                else
                {
                    ownShipMonthRepData = data.ToList();
                    ownShipMonthRepData = ownShipMonthRepData
                        .Where(x => string.IsNullOrEmpty(x.UpdateTime.ToString()) || x.UpdateTime == DateTime.MinValue ?
                         x.CreateTime >= requestDto.InStartDate && x.CreateTime <= requestDto.InEndDate
                        : x.UpdateTime >= requestDto.InStartDate && x.UpdateTime <= requestDto.InEndDate)
                        .ToList();
                    shipId = ownShipMonthRepData.Select(osm => osm.ShipId).ToList().Distinct().ToArray();
                    sumInfo.SumMonthOutputVal = Math.Round(ownShipMonthRepData.Sum(osm => osm.ProductionAmount), 2);
                    sumInfo.SumMonthQuantity = Math.Round(ownShipMonthRepData.Sum(osm => osm.Production), 2);
                }
            }
            else
            {
                ownShipMonthRepData = await _dbContext.Queryable<OwnerShipMonthReport>()
                                    .LeftJoin<Project>((osm, p) => osm.ProjectId == p.Id)
                                    .LeftJoin<OwnerShip>((osm, p, own) => osm.ShipId == own.PomId)
                                    .WhereIF(requestDto.CompanyId != null, (osm, p) => p.CompanyId == requestDto.CompanyId)
                                    .WhereIF(requestDto.ProjectDept != null, (osm, p) => p.ProjectDept == requestDto.ProjectDept)
                                    .WhereIF(requestDto.ProjectStatusId != null && requestDto.ProjectStatusId.Any(), (osm, p) => requestDto.ProjectStatusId.Contains(p.StatusId.Value.ToString()))
                                    .WhereIF(requestDto.ProjectRegionId != null, (osm, p) => p.RegionId == requestDto.ProjectRegionId)
                                    .WhereIF(requestDto.ProjectTypeId != null, (osm, p) => p.TypeId == requestDto.ProjectTypeId)
                                    .WhereIF(!string.IsNullOrWhiteSpace(requestDto.ProjectName), (osm, p) => SqlFunc.Contains(p.Name, requestDto.ProjectName))
                                    .WhereIF(requestDto.ProjectAreaId! != null, (osm, p) => p.AreaId == requestDto.ProjectAreaId)
                                    .WhereIF(categoryList != null && categoryList.Any(), (osm, p) => categoryList.Contains(p.Category))
                                    .WhereIF(tagList != null && tagList.Any(), (osm, p) => tagList.Contains(p.Tag))
                                    .WhereIF(tag2List != null && tag2List.Any(), (osm, p) => tag2List.Contains(p.Tag2))
                                    .WhereIF(!string.IsNullOrWhiteSpace(requestDto.ShipName), (osm, p, own) => own.PomId.ToString() == requestDto.ShipName)
                                    .WhereIF(requestDto.ShipTypeId != Guid.Empty && !string.IsNullOrWhiteSpace(requestDto.ShipTypeId.ToString()), (osm, p, own) => requestDto.ShipTypeId == own.TypeId)
                                    .OrderByDescending((osm, p) => osm.DateMonth).ToListAsync();

                if (!requestDto.IsDuiWai)//监控中心自己用 非对外接口
                {
                    ownShipMonthRepData = ownShipMonthRepData
                                    .Where((osm, p) => osm.IsDelete == 1 && osm.DateMonth >= startMonth && osm.DateMonth <= endMonth)
                                    .ToList();
                }
                else
                {
                    ownShipMonthRepData = ownShipMonthRepData
                        .Where(x => string.IsNullOrEmpty(x.UpdateTime.ToString()) || x.UpdateTime == DateTime.MinValue ?
                         x.CreateTime >= requestDto.InStartDate && x.CreateTime <= requestDto.InEndDate
                        : x.UpdateTime >= requestDto.InStartDate && x.UpdateTime <= requestDto.InEndDate)
                        .ToList();
                }
            }
            //年度产值 、 产量、运转时间、施工天数
            //取最新年份的数据
            int year = startMonth > endMonth ? Convert.ToInt32(startMonth.ToString().Substring(0, 4)) : Convert.ToInt32(endMonth.ToString().Substring(0, 4));
            var ownShipYearData = await _dbContext.Queryable<OwnerShipMonthReport>().Where(x => x.IsDelete == 1 && x.DateYear == year).GroupBy(x => new { x.ShipId, x.ProjectId }).Select(x => new { x.ShipId, x.ProjectId, YearWorkHours = SqlFunc.AggregateSum(x.WorkingHours), YearWorkDays = SqlFunc.AggregateSum(x.ConstructionDays), YearQuantity = SqlFunc.AggregateSum(x.Production), YearOutputVal = SqlFunc.AggregateSum(x.ProductionAmount) }).ToListAsync();
            sumInfo.SumYearOutputVal = Math.Round(ownShipYearData.Where(t => shipId.Contains(t.ShipId)).Sum(t => t.YearOutputVal), 2);
            sumInfo.SumYearQuantity = Math.Round(ownShipYearData.Where(t => shipId.Contains(t.ShipId)).Sum(t => t.YearQuantity), 2);

            //获取项目ids、所属公司
            var proIds = ownShipMonthRepData.Select(x => x.ProjectId).ToList();
            var proData = await _dbContext.Queryable<Project>().Where(x => x.IsDelete == 1 && proIds.Contains(x.Id)).Select(x => new { x.Id, x.Name, x.CompanyId, x.ReportForMertel, x.CurrencyId }).ToListAsync();
            //获取船舶ids
            var ownShipIds = ownShipMonthRepData.Select(x => x.ShipId).ToList();
            //获取船舶名称
            var ownShipData = await _dbContext.Queryable<OwnerShip>().Where(x => ownShipIds.Contains(x.PomId)).Select(x => new { x.PomId, x.Name, x.TypeId }).ToListAsync();
            //获取船舶类型
            var shipTypeIds = ownShipData.Select(x => x.TypeId).ToList();
            var shipTypeData = await _dbContext.Queryable<OwnerShip>()
                .LeftJoin<ShipPingType>((os, st) => os.TypeId == st.PomId)
                .Where((os, st) => os.IsDelete == 1 && st.IsDelete == 1 && ownShipIds.Contains(os.PomId) && shipTypeIds.Contains(os.TypeId)).Select((os, st) => new { ShipId = os.PomId, ShipTypeId = st.PomId, st.Name }).ToListAsync();
            //获取机构
            var instinIds = proData.Select(x => x.CompanyId).ToList();
            var instinData = await _dbContext.Queryable<Project>()
                .LeftJoin<Institution>((p, i) => p.CompanyId == i.PomId)
                .Where((p, i) => instinIds.Contains(p.CompanyId) && proIds.Contains(p.Id) && p.IsDelete == 1 && i.IsDelete == 1)
                .Select((p, i) => new { i.PomId, i.Name, p.Id }).ToListAsync();
            //获取项目负责人
            var usersData = await _dbContext.Queryable<Project>()
                .LeftJoin<ProjectLeader>((p, pl) => p.Id == pl.ProjectId)
                .LeftJoin<Model.User>((p, pl, u) => u.PomId == pl.AssistantManagerId)
                .Where((p, pl, u) => !string.IsNullOrWhiteSpace(p.ReportForMertel) && pl.IsPresent == true && pl.IsDelete == 1 && pl.Type == 1 && proIds.Contains(p.Id))
                .Select((p, pl, u) => new { p.Id, u.Name, u.Phone }).ToListAsync();
            //报表负责人
            var repUsersData = await _dbContext.Queryable<Project>()
                .LeftJoin<Model.User>((p, u) => p.ReportForMertel == u.Phone)
                .Where((p, u) => !string.IsNullOrWhiteSpace(p.ReportForMertel) && p.IsDelete == 1 && u.IsDelete == 1 && proIds.Contains(p.Id))
                .Select((p, u) => new { p.Id, u.Name, u.Phone }).ToListAsync();
            //合同清单类型
            var contractData = await _dbContext.Queryable<DictionaryTable>().Where(x => x.TypeNo == 9 && x.IsDelete == 1).Select(s => new { s.Type, s.Name }).ToListAsync();

            var result = new List<SearchOwnShipMonthRep>();
            //项目月报表明细
            //var projectMonthShipDetails = ownShipMonthRepData.Select(x => x.ShipId).ToList();
            //var monthReportDetailList = await _dbContext.Queryable<MonthReportDetail>()
            //	.Where(x => x.IsDelete == 1 && projectMonthShipDetails.Contains(x.ShipId) && x.DateMonth >= startMonth && x.DateMonth <= endMonth)
            //	.Select(s => new { s.ShipId,s.ProjectId,s.CompletedQuantity, s.CompleteProductionAmount }).ToListAsync();
            //查询汇率表
            //var currencyConverter = await _dbContext.Queryable<CurrencyConverter>()
            //	.Where(x => x.Year == DateTime.Now.Year && x.IsDelete == 1)
            //	.ToListAsync();

            ownShipMonthRepData.ForEach(x =>
            result.Add(new SearchOwnShipMonthRep
            {
                Id = x.Id,
                ProjectName = proData.FirstOrDefault(y => y.Id == x.ProjectId)?.Name,
                OwnShipName = ownShipData.FirstOrDefault(y => y.PomId == x.ShipId)?.Name,
                ShipTypeName = shipTypeData.FirstOrDefault(y => y.ShipId == x.ShipId)?.Name,
                EnterTime = x.EnterTime.ToString("yyyy-MM-dd"),
                QuitTime = x.QuitTime.ToString("yyyy-MM-dd"),
                MonthWorkHours = Math.Round(x.WorkingHours, 2),
                MonthWorkDays = x.ConstructionDays,
                MonthQuantity = Math.Round(x.Production, 2),
                MonthOutputVal = Math.Round(x.ProductionAmount, 2),
                YearOutputVal = ownShipYearData.Any() ? Math.Round(ownShipYearData.FirstOrDefault(y => y.ShipId == x.ShipId && y.ProjectId == x.ProjectId).YearOutputVal, 2) : 0,
                YearQuantity = ownShipYearData.Any() ? Math.Round(ownShipYearData.FirstOrDefault(y => y.ShipId == x.ShipId && y.ProjectId == x.ProjectId).YearQuantity, 2) : 0,
                YearWorkDays = ownShipYearData.Any() ? ownShipYearData.FirstOrDefault(y => y.ShipId == x.ShipId && y.ProjectId == x.ProjectId).YearWorkDays : 0,
                YearWorkHours = ownShipYearData.Any() ? Math.Round(ownShipYearData.FirstOrDefault(y => y.ShipId == x.ShipId && y.ProjectId == x.ProjectId).YearWorkHours, 2) : 0,
                SecUnitName = "广航局",
                ThiUnitName = instinData.FirstOrDefault(y => y.Id == x.ProjectId)?.Name,
                HeadUserName = usersData.FirstOrDefault(y => y.Id == x.ProjectId)?.Name,
                HeadUserTel = usersData.FirstOrDefault(y => y.Id == x.ProjectId)?.Phone,
                RepUserName = repUsersData.FirstOrDefault(y => y.Id == x.ProjectId)?.Name,
                RepUserTel = repUsersData.FirstOrDefault(y => y.Id == x.ProjectId)?.Phone,
                IsExamine = 0,
                SubmitDate = MonthDate(x.DateMonth),
                ContractTypeName = contractData.FirstOrDefault(y => y.Type == x.ContractDetailType)?.Name,
                ProjectId = x.ProjectId,
                OwnShipId = x.ShipId,
                GYFSId = x.WorkModeId,
                SJCTId = x.WorkTypeId,
                GKJBId = x.ConditionGradeId,
                QDLXId = x.ContractDetailType,
                UpdateTime = x.UpdateTime,
                CreateTime = x.CreateTime,
                DigDeep = x.DigDeep,
                BlowingDistance = x.BlowingDistance,
                HaulDistance = x.HaulDistance
            }));
            //foreach (var item in result)
            //{
            //	var currencyId = proData.FirstOrDefault(y => y.Id == item.ProjectId.Value)?.CurrencyId.ToString();
            //	var exchangeRate = currencyConverter.Where(z => z.CurrencyId.ToString() == currencyId).Select(z => z.ExchangeRate).First().Value;
            //	item.MonthOutputVal = Math.Round(monthReportDetailList.Where(y => y.ShipId == item.OwnShipId && y.ProjectId == item.ProjectId).Select(y => y.CompleteProductionAmount).Sum(), 2);
            //}
            #endregion

            #region 新逻辑
            if (result.Any())
            {
                var startTime = 0;
                var endTime = 0;
                if (requestDto.InEndDate.HasValue)
                {
                    startTime = int.Parse(requestDto.InEndDate.Value.AddMonths(-1).ToString("yyyyMM26"));
                    endTime = int.Parse(requestDto.InEndDate.Value.ToString("yyyyMM25"));
                }

                //项目信息
                var projectList = await _dbContext.Queryable<Project>().Where(x => x.IsDelete == 1).ToListAsync();
                var sipMovementList = await _dbContext.Queryable<ShipMovement>().Where(x => x.IsDelete == 1).ToListAsync();
                foreach (var res in result)
                {
                    #region 新逻辑
                    var num = 0M;
                    if (projectList != null && projectList.Any() && sipMovementList != null && sipMovementList.Any())
                    {

                        //新逻辑
                        var list1 = await _dbShipDayReport.AsQueryable().Where(t => t.ProjectId == Guid.Empty && t.ShipId == res.OwnShipId && t.DateDay >= startTime && t.DateDay <= endTime && t.IsDelete == 1).ToListAsync();
                        var primaryIds = list1.Where(t => t.ProjectId == Guid.Empty).Select(t => new { id = t.Id, shipId = t.ShipId, DateDay = t.DateDay, ProjectId = t.ProjectId }).ToList();
                        foreach (var item in primaryIds)
                        {
                            var isExistProject = sipMovementList.Where(x => x.IsDelete == 1 && x.ShipId == item.shipId && x.EnterTime.HasValue == true && (x.EnterTime.Value.ToDateDay() <= item.DateDay || x.QuitTime.HasValue == true && x.QuitTime.Value.ToDateDay() >= item.DateDay)).OrderByDescending(x => x.EnterTime).ToList();
                            foreach (var project in isExistProject)
                            {

                                if (project.QuitTime.HasValue && project.QuitTime.Value.ToDateDay() >= item.DateDay)
                                {

                                    var oldValue = list1.Where(t => t.ShipId == project.ShipId && t.DateDay == item.DateDay).FirstOrDefault();
                                    if (oldValue != null)
                                    {
                                        oldValue.ProjectId = project.ProjectId;

                                    }
                                }
                                if (project.EnterTime.HasValue && project.QuitTime.HasValue == false && project.EnterTime.Value.ToDateDay() <= item.DateDay)
                                {

                                    var oldValue = list1.Where(t => t.ShipId == project.ShipId && t.DateDay == item.DateDay).FirstOrDefault();
                                    if (oldValue != null)
                                    {
                                        oldValue.ProjectId = project.ProjectId;

                                    }
                                }
                                if (project.EnterTime.HasValue && project.QuitTime.HasValue && project.QuitTime.Value.ToDateDay() >= item.DateDay)
                                {

                                    var oldValue = list1.Where(t => t.ShipId == project.ShipId && t.DateDay == item.DateDay).FirstOrDefault();
                                    if (oldValue != null)
                                    {
                                        oldValue.ProjectId = project.ProjectId;

                                    }
                                }
                            }

                        }

                        foreach (var item in list1)
                        {
                            num += ((item.Dredge ?? 0) + (item.Sail ?? 0) + (item.BlowingWater ?? 0) + (item.SedimentDisposal ?? 0) + (item.BlowingWater ?? 0));
                        }
                    }
                    res.MonthWorkHours += num;
                    res.YearWorkHours += num;
                    #endregion
                }

            }
            #endregion
            list.searchOwnShipMonthReps = result;
            list.ownShipMonth = sumInfo;
            responseDto.Count = total;
            responseDto.Data = list;
            responseDto.Success();
            return responseDto;
        }
        /// <summary>
        /// 自有船舶未填报列表
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<NotFillOwnShipSearchResponseDto>>> GetSearchNotFillOwnShipSearchResponseDto(NotFillOwnShipRequestDto requestDto)
        {
            var responseDto = new ResponseAjaxResult<List<NotFillOwnShipSearchResponseDto>>();
            #region 日期控制
            //开始日期结束日期都是空  默认按当天日期匹配
            if ((requestDto.InStartDate == DateTime.MinValue || string.IsNullOrWhiteSpace(requestDto.InStartDate.ToString())) && (requestDto.InEndDate == DateTime.MinValue || string.IsNullOrWhiteSpace(requestDto.InEndDate.ToString())))
            {
                if (DateTime.Now.Day >= 26)
                {
                    requestDto.InStartDate = DateTime.Now;
                    requestDto.InEndDate = DateTime.Now;
                }
                else
                {
                    requestDto.InStartDate = DateTime.Now.AddMonths(-1);
                    requestDto.InEndDate = DateTime.Now.AddMonths(-1);
                }
            }
            else
            {
                if (requestDto.InStartDate > requestDto.InEndDate)
                {
                    responseDto.Data = null;
                    responseDto.FailResult(HttpStatusCode.ParameterError, "传入开始日期大于结束日期");
                    return responseDto;
                }
            }
            #endregion

            #region 初始化筛选
            //属性标签
            var categoryList = new List<int>();
            var tagList = new List<int>();
            var tag2List = new List<int>();
            if (requestDto.TagName != null && requestDto.TagName.Any())
            {
                foreach (var item in requestDto.TagName)
                {
                    switch (item)
                    {
                        case "境内":
                            categoryList.Add(0);
                            break;
                        case "境外":
                            categoryList.Add(1);
                            break;
                        case "传统":
                            tagList.Add(0);
                            break;
                        case "新兴":
                            tagList.Add(1);
                            break;
                        case "现汇":
                            tag2List.Add(0);
                            break;
                        case "投资":
                            tag2List.Add(1);
                            break;
                        default:
                            break;
                    }
                }
            }
            #endregion

            //月份
            int startMonth = requestDto.InStartDate.ToDateMonth();
            int endMonth = requestDto.InEndDate.ToDateMonth();
            //判断选择的日期范围差
            int monthsDiff = 0;
            List<int> dateMonth = new List<int>();
            if (!((requestDto.InStartDate == DateTime.MinValue || string.IsNullOrWhiteSpace(requestDto.InStartDate.ToString())) && (requestDto.InEndDate == DateTime.MinValue || string.IsNullOrWhiteSpace(requestDto.InEndDate.ToString()))))
            {
                monthsDiff = Utils.CalculateMonthDifference(requestDto.InStartDate, requestDto.InEndDate);
            }
            var date = requestDto.InStartDate;
            for (int i = 0; i <= monthsDiff; i++)
            {
                if (i == 0)
                {
                    int month = date.ToDateMonth();
                    dateMonth.Add(month);
                }
                else
                {
                    date = date.AddMonths(1);
                    int month = date.ToDateMonth();
                    dateMonth.Add(month);
                }

            }
            var InstartTime = new DateTime();
            if (requestDto.InStartDate.Month == 1)
            {
                InstartTime = new DateTime(requestDto.InStartDate.AddYears(-1).Year, requestDto.InStartDate.AddMonths(-1).Month, 26);

            }
            else
            {
                InstartTime = new DateTime(requestDto.InStartDate.Year, requestDto.InStartDate.AddMonths(-1).Month, 26);
            }
            var InendTime = new DateTime(requestDto.InEndDate.Year, requestDto.InEndDate.Month, 25);

            //船舶类型  耙吸 绞吸 抓斗
            var shipTypeIds = CommonData.ShipType.Split(',').ToList();
            var shipTypeData = await _dbContext.Queryable<ShipPingType>().Where(x => shipTypeIds.Contains(x.PomId.ToString())).Select(x => new { x.PomId, x.Name }).ToListAsync();
            //获取所有自有船舶
            var ownShips = await _dbContext.Queryable<OwnerShip>().Where(x => shipTypeIds.Contains(x.TypeId.ToString())).Select(x => new { x.PomId, x.Name, x.CompanyId, x.TypeId }).ToListAsync();
            //船所属机构
            var instinIds = ownShips.Select(x => x.CompanyId).ToList();
            var instinData = await _dbContext.Queryable<Institution>().Where(x => x.IsDelete == 1 && instinIds.Contains(x.PomId.Value)).Select(x => new { x.PomId, x.Name }).ToListAsync();
            //获取自有船舶ids
            var ownShipIds = ownShips.Select(x => x.PomId).ToList();
            //获取已经填报的自有船舶月报
            var fillOwnShips = await _dbContext.Queryable<OwnerShipMonthReport>()
                .Where(osm => ownShipIds.Contains(osm.ShipId) && osm.IsDelete == 1 && osm.DateMonth == endMonth)
                .Where(osm => osm.IsDelete == 1 && osm.DateMonth >= startMonth && osm.DateMonth <= endMonth)
                .ToListAsync();
            var result = new List<NotFillOwnShipSearchResponseDto>();
            foreach (var item in ownShips)
            {
                for (int i = 0; i <= monthsDiff; i++)
                {
                    var isExistFirst = fillOwnShips.Where(x => x.ShipId == item.PomId && x.DateMonth == dateMonth[i]).FirstOrDefault();
                    if (isExistFirst == null)
                    {
                        result.Add(new NotFillOwnShipSearchResponseDto
                        {
                            NotFillDate = string.Format("{0}年{1}月", dateMonth[i].ToString().Substring(0, 4), dateMonth[i].ToString().Substring(dateMonth[i].ToString().Length - 2, 2)),
                            NotDate = dateMonth[i],
                            OwnShipId = item.PomId,
                            OwnShipName = item.Name,
                            ShipTypeName = shipTypeData.FirstOrDefault(x => item.TypeId == x.PomId)?.Name,
                            CompanyId = item.CompanyId,
                            ShipCompanyName = instinData.FirstOrDefault(x => item.CompanyId == x.PomId)?.Name,
                            ShipTypeId = item.TypeId
                        });
                    }
                }
            }

            //获取已进场的船舶
            var MobilizationShip = await _dbContext.Queryable<ShipMovement>().Where(x => x.IsDelete == 1 && x.ShipType == ShipType.OwnerShip && x.EnterTime >= InstartTime).ToListAsync();
            //在判断这些进场的船舶是否在结束日期内处于未退场
            var notQuitMobilizationShip = MobilizationShip.Where(x => x.QuitTime == null || x.QuitTime <= InendTime).ToList();
            foreach (var item in result)
            {
                var shipFirst = notQuitMobilizationShip.Where(x => x.ShipId == item.OwnShipId).FirstOrDefault();
                if (shipFirst != null)
                {
                    item.ProjectId = shipFirst.ProjectId;
                    item.OwnShipId = shipFirst.ShipId;
                    //进场时间为空时 直接赋值
                    if (shipFirst.EnterTime == null)
                    {
                        item.enterTime = shipFirst.EnterTime;
                    }
                    //进场时间有值 但是退场时间为空 
                    else if (shipFirst.QuitTime == null)
                    {
                        if (item.NotDate >= shipFirst.EnterTime.Value.ToDateMonth())
                        {
                            item.enterTime = shipFirst.EnterTime;
                        }
                    }
                    //未填报日期在进场日期和退场日期范围内
                    else if (item.NotDate >= shipFirst.EnterTime.Value.ToDateMonth() && item.NotDate <= shipFirst.QuitTime.Value.ToDateMonth())
                    {
                        item.enterTime = shipFirst.EnterTime;
                    }
                    //未填报日期小于进场日期 或者 未填报日期 大于退场日期 则属于未进场状态
                    else if (item.NotDate < shipFirst.EnterTime.Value.ToDateMonth() || item.NotDate > shipFirst.QuitTime.Value.ToDateMonth())
                    {
                        item.enterTime = null;
                    }
                    if (shipFirst.QuitTime == null)
                    {
                        item.quitTime = shipFirst.QuitTime;
                    }
                    else if (item.NotDate != shipFirst.QuitTime.Value.ToDateMonth())
                    {
                        item.quitTime = null;
                    }
                    else
                    {
                        item.quitTime = shipFirst.QuitTime;
                    }
                }
                //判断进退场状态
                if (item.enterTime == null && item.quitTime == null)
                {
                    item.InOutStatus = ShipMovementStatus.None.ToDescription();
                }
                else if (item.enterTime != null && item.quitTime == null)
                {
                    item.InOutStatus = ShipMovementStatus.Enter.ToDescription();
                }
                else if (item.quitTime != null)
                {
                    item.InOutStatus = ShipMovementStatus.Quit.ToDescription();
                }
            }
            var projectList = await _dbContext.Queryable<Project>().Where(x => x.IsDelete == 1).ToListAsync();

            foreach (var item in result)
            {
                var projectFirst = projectList.Where(x => x.Id == item.ProjectId).FirstOrDefault();
                item.ProjectName = projectFirst == null ? "" : projectFirst.Name;
            }

            if (!string.IsNullOrWhiteSpace(requestDto.CompanyId.ToString())) result = result.Where(x => x.CompanyId == requestDto.CompanyId).ToList();
            if (!string.IsNullOrWhiteSpace(requestDto.ProjectName)) result = result.Where(x => x.ProjectName.Contains(requestDto.ProjectName)).ToList();
            if (!string.IsNullOrWhiteSpace(requestDto.ShipName)) result = result.Where(x => x.OwnShipId.ToString() == requestDto.ShipName).ToList();
            if (requestDto.ShipTypeId != Guid.Empty && !string.IsNullOrWhiteSpace(requestDto.ShipTypeId.ToString())) result = result.Where(x => x.ShipTypeId == requestDto.ShipTypeId).ToList();

            int skipCount = (requestDto.PageIndex - 1) * requestDto.PageSize;
            var data = result.OrderByDescending(x => x.NotFillDate).Skip(skipCount).Take(requestDto.PageSize).ToList();
            responseDto.Data = data;
            responseDto.Count = result.Count();
            responseDto.Success();
            return responseDto;
        }
        /// <summary>
        /// 自有船舶月报导出
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<ExcelImportResponseDto>> OwnShipMonthRepExcel(MonthRepRequestDto requestDto)
        {
            var responseDto = new ResponseAjaxResult<ExcelImportResponseDto>();
            var excelImpDto = new ExcelImportResponseDto();
            var responseData = await GetSearchOwnShipMonthRepAsync(requestDto, 2);
            var result = new List<OwnShipMonthRepExcelDto>();
            responseData.Data.searchOwnShipMonthReps.ForEach(x => result.Add(new OwnShipMonthRepExcelDto
            {
                OwnShipName = x.OwnShipName,
                ContractTypeName = x.ContractTypeName,
                EnterTime = x.EnterTime,
                HeadUserName = string.IsNullOrWhiteSpace(x.HeadUserName) ? null : x.HeadUserName,
                HeadUserTel = string.IsNullOrWhiteSpace(x.HeadUserTel) ? null : x.HeadUserTel,
                IsExamine = x.IsExamine == 0 ? "否" : "是",
                MonthOutputVal = x.MonthOutputVal,
                MonthQuantity = x.MonthQuantity,
                MonthWorkDays = x.MonthWorkDays,
                MonthWorkHours = x.MonthWorkHours,
                ProjectName = x.ProjectName,
                QuitTime = x.QuitTime,
                RepUserName = string.IsNullOrWhiteSpace(x.RepUserName) ? null : x.RepUserName,
                RepUserTel = string.IsNullOrWhiteSpace(x.RepUserTel) ? null : x.RepUserTel,
                SecUnitName = x.SecUnitName,
                ShipTypeName = x.ShipTypeName,
                ThiUnitName = x.ThiUnitName,
                YearOutputVal = x.YearOutputVal,
                YearQuantity = x.YearQuantity,
                YearWorkDays = x.YearWorkDays,
                YearWorkHours = x.YearWorkHours,
                SubmitDate = x.SubmitDate.ToString("yyyy-MM-dd")
            }));
            excelImpDto.OwnShipMonthRepExcelDtos = result;
            responseDto.Data = excelImpDto;
            responseDto.Success();
            return responseDto;
        }

        #endregion

        #region 公用业务方法

        #region 数据权限范围

        /// <summary>
        /// 获取当前用户授权数据
        /// </summary>
        /// <returns></returns>
        public async Task<UserAuthForDataDto> GetCurrentUserAuthForDataAsync()
        {
            var userAuthData = new UserAuthForDataDto() { IsAdmin = _currentUser.CurrentLoginIsAdmin };
            if (userAuthData.IsAdmin)
            {
                return userAuthData;
            }
            //非超级管理员 授权逻辑
            var institution = await _dbInstitution.GetSingleAsync(t => t.Oid == _currentUser.CurrentLoginInstitutionOid && t.IsDelete == 1);
            if (institution.PomId != null)
            {
                var result = await _baseService.SearchCompanySubPullDownAsync(institution.PomId.Value, false, true);
                if (result.Code == HttpStatusCode.Success && result.Data != null)
                {
                    userAuthData.CompanyIds = result.Data.Where(t => t.Id != null).Select(t => t.Id).ToArray();
                }
            }
            return userAuthData;
        }


        #endregion

        /// <summary>
        /// 获取默认允许填报的月份
        /// </summary>
        /// <returns></returns>
        private int GetDefaultReportDateMonth()
        {
            DateTime time = DateTime.Now;
            return time.Day < 26 ? time.AddMonths(-1).ToDateMonth() : time.ToDateMonth();
        }

        /// <summary>
        /// 获取填报月份时间
        /// </summary>
        /// <returns></returns>
        private DateTime GetReportMonthTime(DateTime time)
        {
            if (time.Day < 26)
            {
                return new DateTime(time.Year, time.Month, 1);
            }
            else
            {
                var nextMonthTime = time.AddMonths(1);
                return new DateTime(nextMonthTime.Year, nextMonthTime.Month, 1);
            }
        }

        /// <summary>
        /// 获取月份的起始时间
        /// </summary>
        private DateTime GetMonthStartDayTime(DateTime dayTime)
        {
            if (dayTime.Day < 26)
            {
                var lastMonthDayTime = dayTime.AddMonths(-1);
                return new DateTime(lastMonthDayTime.Year, lastMonthDayTime.Month, 26);
            }
            else
            {
                return new DateTime(dayTime.Year, dayTime.Month, 26);
            }
        }

        /// <summary>
        /// 获取年份的起始时间
        /// </summary>
        private DateTime GetYearStartDayTime(DateTime dayTime)
        {
            if (dayTime.Month == 12 && dayTime.Day >= 26)
            {
                return new DateTime(dayTime.Year, 12, 26);
            }
            else
            {
                var lastYearDayTime = dayTime.AddYears(-1);
                return new DateTime(lastYearDayTime.Year, 12, 26);
            }
        }

        /// <summary>
        /// 验证填报月份
        /// </summary>
        /// <returns></returns>
        private bool VerifyDateMonthForSave(int dateMonth)
        {
            var nowMonth = GetDefaultReportDateMonth();
            // 填报月份判断
            if (dateMonth > nowMonth)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 获取文件全路径
        /// </summary>
        /// <returns></returns>
        private string GetFullFileUrl(Files? file)
        {
            if (file == null)
            {
                return string.Empty;
            }
            var rootPath = AppsettingsHelper.GetValue("RootFilePath");
            return rootPath + file.Name;
        }

        /// <summary>
        /// 获取最新年份的一条税率转换
        /// </summary>
        /// <returns></returns>
        private async Task<decimal> GetCurrencyRateAsync(Guid currencyId)
        {
            var currencyConverter = await _dbCurrencyConverter.AsQueryable().Where(t => t.CurrencyId == currencyId.ToString() && t.Year == DateTime.Now.Year && t.IsDelete == 1).SingleAsync();
            if (currencyConverter == null || currencyConverter.ExchangeRate == null)
            {
                throw new InvalidDataException("汇率数据不存在");
            }
            return (decimal)currencyConverter.ExchangeRate;
        }


        /// <summary>
        /// 获取项目领导班子集合
        /// </summary>
        /// <returns></returns>
        private async Task<List<ProjectLeader>> GetProjectLeadersAsync(Guid[] projectIds)
        {
            if (projectIds == null || !projectIds.Any())
            {
                return new List<ProjectLeader>();
            }
            var xprojectIds = projectIds.Select(t => (Guid?)t);
            return await _dbProjectLeader.GetListAsync(t => xprojectIds.Contains(t.ProjectId) && t.IsDelete == 1);
        }

        /// <summary>
        /// 获取项目干系单位信息集合
        /// </summary>
        /// <returns></returns>
        private async Task<List<ProjectOrg>> GetProjectOrgsAsync(Guid[] projectIds)
        {
            if (projectIds == null || !projectIds.Any())
            {
                return new List<ProjectOrg>();
            }
            var xprojectIds = projectIds.Select(t => (Guid?)t);
            return await _dbProjectOrg.GetListAsync(t => xprojectIds.Contains(t.ProjectId) && t.IsDelete == 1);
        }


        /// <summary>
        /// 获取（国/省/市/区）集合
        /// </summary>
        /// <returns></returns>
        private async Task<List<Province>> GetProvincesAsync(Guid?[] provinceIds)
        {
            if (provinceIds == null || !provinceIds.Any())
            {
                return new List<Province>();
            }
            return await _dbProvince.GetListAsync(t => provinceIds.Contains(t.PomId) && t.IsDelete == 1);
        }

        /// <summary>
        /// 获取（国/省/市/区）集合 By Level
        /// </summary>
        /// <returns></returns>
        private async Task<List<Province>> GetProvincesByLevelAsync(int level)
        {
            return await _dbProvince.GetListAsync(t => t.Zaddvslevel == level && t.IsDelete == 1);
        }

        /// <summary>
        /// 获取项目行业分类集合
        /// </summary>
        /// <returns></returns>
        private async Task<List<IndustryClassification>> GetIndustryClassificationsAsync()
        {
            return await _dbIndustryClassification.GetListAsync(t => t.IsDelete == 1);
        }

        /// <summary>
        /// 获取项目施工资质集合
        /// </summary>
        /// <returns></returns>
        private async Task<List<ConstructionQualification>> GetConstructionQualificationsAsync()
        {
            return await _dbConstructionQualification.GetListAsync(t => t.IsDelete == 1);
        }

        /// <summary>
        /// 获取项目区域集合
        /// </summary>
        /// <returns></returns>
        private async Task<List<ProjectArea>> GetProjectAreasAsync()
        {
            return await _dbProjectArea.GetListAsync(t => t.IsDelete == 1);
        }

        /// <summary>
        /// 获取机构集合
        /// </summary>
        /// <returns></returns>
        private async Task<List<Institution>> GetInstitutionsAsync(Guid?[] institutionIds)
        {
            if (institutionIds == null || !institutionIds.Any())
            {
                return new List<Institution>();
            }
            return await _dbInstitution.GetListAsync(t => institutionIds.Contains(t.PomId) && t.IsDelete == 1);
        }

        /// <summary>
        /// 获取项目类型集合
        /// </summary>
        /// <returns></returns>
        private async Task<List<ProjectType>> GetProjectTypesAsync()
        {
            return await _dbProjectType.GetListAsync(t => t.IsDelete == 1);
        }

        /// <summary>
        /// 获取项目状态集合(部分字段(StatusId,Name))
        /// </summary>
        /// <returns></returns>
        private async Task<List<ProjectStatus>> GetProjectStatusListAsync()
        {
            return await _dbProjectStatus.GetListAsync(t => t.IsDelete == 1);
        }

        /// <summary>
        /// 获取船舶类型
        /// </summary>
        /// <returns></returns>
        private async Task<ShipPingType?> GetShipPingTypeAsync(Guid shipTypeId)
        {
            return (await GetShipPingTypesAsync(new Guid?[] { shipTypeId })).FirstOrDefault();
        }

        /// <summary>
        /// 获取船舶类型集合
        /// </summary>
        /// <returns></returns>
        private async Task<List<ShipPingType>> GetShipPingTypesAsync(Guid?[] shipTypeIds)
        {
            if (shipTypeIds == null || !shipTypeIds.Any())
            {
                return new List<ShipPingType>();
            }
            return await _dbShipPingType.GetListAsync(t => t.IsDelete == 1 && shipTypeIds.Contains(t.PomId));
        }

        /// <summary>
        /// 获取一条船舶进出场
        /// </summary>
        /// <returns></returns>
        private async Task<ShipMovement?> GetShipMovementAsync(Guid projectId, Guid shipId, ShipType shipType)
        {
            return await _dbShipMovement.GetFirstAsync(t => t.ProjectId == projectId && t.ShipId == shipId && t.ShipType == shipType && t.IsDelete == 1);
        }

        /// <summary>
        /// 获取一个港口（部分字段（PomId,Name））
        /// </summary>
        /// <returns></returns>
        private async Task<PortData?> GetPortDatasPartAsync(string? portId)
        {
            return await _dbPortData.AsQueryable().Where(t => t.IsDelete == 1 && t.PomId == portId).Select(t => new PortData() { PomId = t.PomId, Name = t.Name }).FirstAsync();
        }

        /// <summary>
        /// 获取港口集合（部分字段（PomId,Name））
        /// </summary>
        /// <param name="portIds">港口PomId集合</param>
        /// <returns></returns>
        private async Task<List<PortData>> GetPortDatasPartAsync(string?[] portIds)
        {
            var findPortIds = portIds.Where(t => !string.IsNullOrWhiteSpace(t)).ToArray();
            if (!findPortIds.Any())
            {
                return new List<PortData>();
            }
            return await _dbPortData.AsQueryable().Where(t => t.IsDelete == 1 && findPortIds.Contains(t.PomId)).Select(t => new Domain.Models.PortData() { PomId = t.PomId, Name = t.Name }).ToListAsync();
        }

        /// <summary>
        /// 获取一个项目(部分字段（Id,Name,PomId,Category，CurrencyId，TypeId）)
        /// </summary>
        /// <param name="projectId">项目id</param>
        /// <returns></returns>
        private async Task<Project?> GetProjectPartAsync(Guid projectId)
        {
            return await _dbProject.AsQueryable().Where(t => t.Id == projectId && t.IsDelete == 1).Select(t => new Project() { Id = t.Id, PomId = t.PomId, Name = t.Name, Category = t.Category, CurrencyId = t.CurrencyId, TypeId = t.TypeId }).FirstAsync();
        }

        /// <summary>
        /// 获取一个文件
        /// </summary>
        /// <returns></returns>
        private async Task<Files?> GetFileAsync(Guid fileId)
        {
            return await _dbFile.GetFirstAsync(t => t.Id == fileId && t.IsDelete == 1);
        }

        /// <summary>
        /// 获取文件集合
        /// </summary>
        /// <returns></returns>
        private async Task<List<Files>> GetFilesAsync(Guid[] fileIds)
        {
            if (!fileIds.Any())
            {
                return new List<Files>();
            }
            return await _dbFile.GetListAsync(t => fileIds.Contains(t.Id) && t.IsDelete == 1);
        }

        /// <summary>
        /// 获取一个字典对象
        /// </summary>
        /// <returns></returns>
        private async Task<DictionaryTable?> GetDictionaryTableObject(DictionaryTypeNo typeNo, int type)
        {
            return (await GetDictionarysAsync(typeNo)).FirstOrDefault(t => t.Type == type);
        }

        /// <summary>
        /// 获取字典集合
        /// </summary>
        /// <returns></returns>
        private async Task<List<DictionaryTable>> GetDictionarysAsync(DictionaryTypeNo typeNo)
        {
            return await _dbDictionaryTable.GetListAsync(t => t.TypeNo == (int)typeNo && t.IsDelete == 1);
        }

        /// <summary>
        /// 获取一条自有船舶
        /// </summary>
        /// <returns></returns>
        private async Task<OwnerShip?> GetOwnerShipAsync(Guid shipId)
        {
            return await _dbOwnerShip.GetFirstAsync(t => t.IsDelete == 1 && t.PomId == shipId);
        }

        /// <summary>
        /// 获取一个船舶（部分字段（PomId, Name,ShipKindTypeId））
        /// </summary>
        /// <returns></returns>
        private async Task<ShipPartDto?> GetShipPartAsync(Guid shipId, ShipType shipType)
        {
            return (await GetShipPartsAsync(new Guid[] { shipId }, shipType)).FirstOrDefault();
        }

        /// <summary>
        /// 获取船舶集合（部分字段（PomId, Name,ShipKindTypeId））
        /// </summary>
        /// <returns></returns>
        private async Task<List<ShipPartDto>> GetShipPartsAsync(Guid[] shipIds, ShipType shipType)
        {
            if (shipType == ShipType.OwnerShip)
            {
                return await _dbOwnerShip.AsQueryable().Where(t => shipIds.Contains(t.PomId) && t.IsDelete == 1).Select(t => new ShipPartDto { PomId = t.PomId, ShipType = shipType, Name = t.Name, ShipKindTypeId = t.TypeId }).ToListAsync();
            }
            else if (shipType == ShipType.SubShip)
            {
                return await _dbSubShip.AsQueryable().Where(t => shipIds.Contains(t.PomId) && t.IsDelete == 1).Select(t => new ShipPartDto { PomId = t.PomId, ShipType = shipType, Name = t.Name, ShipKindTypeId = t.TypeId }).ToListAsync();
            }
            else if (shipType == ShipType.SubBusinessUnit)
            {
                var dealingUnitIds = shipIds.Select(t => (Guid?)t).ToArray();
                return await _dbDealingUnit.AsQueryable().Where(t => dealingUnitIds.Contains(t.PomId) && t.IsDelete == 1).Select(t => new ShipPartDto { PomId = t.PomId, ShipType = shipType, Name = t.ZBPNAME_ZH }).ToListAsync();
            }
            return new List<ShipPartDto>();
        }


        /// <summary>
        /// 获取一个资源
        /// </summary>
        /// <returns></returns>
        private async Task<ShipResourceDto?> GetShipResourceAsync(Guid shipId, ConstructionOutPutType outPutType)
        {
            return (await GetShipResourcesAsync(new Guid[] { shipId }, outPutType)).FirstOrDefault();
        }

        /// <summary>
        /// 获取资源集合
        /// </summary>
        /// <returns></returns>
        private async Task<List<ShipResourceDto>> GetShipResourcesAsync(Guid[] shipIds, ConstructionOutPutType outPutType)
        {
            var list = new List<ShipResourceDto>();
            List<ShipPartDto>? shipList = null;
            List<DictionaryTable>? customShips = null;
            if (outPutType == ConstructionOutPutType.Self || outPutType == ConstructionOutPutType.SubOwner)
            {
                shipList = await GetShipPartsAsync(shipIds, ShipType.OwnerShip);
                customShips = await GetDictionarysAsync(DictionaryTypeNo.CustomShip);
            }
            else if (outPutType == ConstructionOutPutType.SubPackage)
            {
                shipList = await GetShipPartsAsync(shipIds, ShipType.SubShip);
                shipList.AddRange(await GetShipPartsAsync(shipIds, ShipType.SubBusinessUnit));
                customShips = await GetDictionarysAsync(DictionaryTypeNo.CustomShip);
            }
            if (shipList != null)
            {
                list = shipList.Where(t => t.PomId != null).Select(t => new ShipResourceDto() { OutPutType = outPutType, ShipId = (Guid)t.PomId, ShipName = t.Name }).ToList();
            }
            if (customShips != null)
            {
                list.AddRange(customShips.Select(t => new ShipResourceDto() { OutPutType = outPutType, ShipId = t.Id, ShipName = t.Name }));
            }
            return list;
        }

        /// <summary>
        /// 获取往来单位集合（部分字段（PomId,ZBPNAME_ZH））
        /// </summary>
        /// <returns></returns>
        private async Task<List<DealingUnit>> GetDealingUnitPartsAsync(Guid?[] dealingUnitIds)
        {
            if (dealingUnitIds == null || !dealingUnitIds.Any())
            {
                return new List<DealingUnit>();
            }
            return await _dbDealingUnit.AsQueryable().Where(t => dealingUnitIds.Contains(t.PomId) && t.IsDelete == 1).Select(t => new DealingUnit { PomId = t.PomId, ZBPNAME_ZH = t.ZBPNAME_ZH }).ToListAsync();
        }

        /// <summary>
        /// 获取施工分类集合
        /// </summary>
        /// <returns></returns>
        private async Task<List<ProjectWBS>> GetProjectWBSListAsync(Guid projectId, int? DateMonth = 0)
        {
            List<ProjectWBS> projectWBs = new List<ProjectWBS>();
            #region 参数问题
            var month = "0";
            if (DateTime.Now.Day >= 26 && DateTime.Now.Day <= 1)
            {
                month = DateTime.Now.Month.ToString();
            }
            else
            {
                month = DateTime.Now.AddMonths(-1).Month.ToString();
            }
            if (month.Length == 1)
            {
                month = "0" + month;
            }
            var yearMonth = int.Parse((DateTime.Now.Year + month));
            #endregion
            if (DateMonth == null || yearMonth == DateMonth)
            {
                //查询最新的wbs
                return await _dbProjectWBS.AsQueryable().Where(t => t.ProjectId == projectId.ToString() && t.IsDelete == 1).ToListAsync();
            }
            else
            {
                //查询历史数据 
                var result = await _dbContext.Queryable<ProjectWbsHistoryMonth>().Where(t => t.DateMonth == DateMonth && t.ProjectId == projectId.ToString() && t.IsDelete == 1).ToListAsync();
                if (result == null || !result.Any())
                {
                    //查询最新的wbs
                    return await _dbProjectWBS.AsQueryable().Where(t => t.ProjectId == projectId.ToString() && t.IsDelete == 1).ToListAsync();
                }
                foreach (var item in result)
                {
                    projectWBs.Add(new ProjectWBS()
                    {
                        ContractAmount = item.ContractAmount,
                        CreateId = item.CreateId,
                        CreateTime = item.CreateTime,
                        Def = item.Def,
                        DeleteId = item.DeleteId,
                        DeleteTime = item.DeleteTime,
                        DownOne = item.DownOne,
                        EngQuantity = item.EngQuantity,
                        IsDelete = item.IsDelete,
                        ItemNum = item.ItemNum,
                        KeyId = item.KeyId,
                        Name = item.Name,
                        Id = item.Id,
                        Pid = item.Pid,
                        ProjectId = item.ProjectId,
                        ProjectNum = item.ProjectNum,
                        Prev = item.Prev,
                        ProjectWBSId = item.ProjectWBSId,
                        UnitPrice = item.UnitPrice,
                        UpdateId = item.UpdateId,
                        UpdateTime = item.UpdateTime,
                    });
                }
                return projectWBs;
            }

        }

        /// <summary>
        /// 递归获取施工分类一级层级名称
        /// </summary>
        /// <returns></returns>
        private string? GetProjectWBSLevel1Name(List<ProjectWBS> projectWBSList, ProjectWBS projectWBS)
        {
            if (string.IsNullOrWhiteSpace(projectWBS.Pid) || projectWBS.Pid.TrimAll() == "0")
            {
                return projectWBS.Name;
            }
            var partentProjectWBS = projectWBSList.FirstOrDefault(t => t.KeyId == projectWBS.Pid);
            if (partentProjectWBS == null)
            {
                return projectWBS.Name;
            }
            if (partentProjectWBS.KeyId == partentProjectWBS.Pid)
            {
                return partentProjectWBS.Name;
            }
            return GetProjectWBSLevel1Name(projectWBSList, partentProjectWBS);
        }

        /// <summary>
        /// 递归获取施工分类层级名称
        /// </summary>
        /// <returns></returns>
        private string? GetProjectWBSLevelName(List<ProjectWBS> projectWBSList, string? levelName, string? pid)
        {
            if (string.IsNullOrWhiteSpace(pid) || pid.TrimAll() == "0")
            {
                return levelName;
            }
            var partentProjectWBS = projectWBSList.FirstOrDefault(t => t.KeyId == pid);
            if (partentProjectWBS == null || partentProjectWBS.KeyId == partentProjectWBS.Pid)
            {
                return levelName;
            }
            return GetProjectWBSLevelName(projectWBSList, partentProjectWBS.Name + "/" + levelName, partentProjectWBS.Pid);
        }

        /// <summary>
        /// 获取用户部分信息（Id,PomId,Name）
        /// </summary>
        /// <returns></returns>
        private async Task<Domain.Models.User?> GetUserPartAsync(Guid userId)
        {
            return await _dbUser.AsQueryable().Where(t => t.IsDelete == 1 && t.Id == userId).Select(t => new Domain.Models.User() { Id = t.Id, PomId = t.PomId, Name = t.Name }).FirstAsync();
        }

        /// <summary>
        /// 获取用户部分信息（Id,PomId,Name,Phone）
        /// </summary>
        /// <returns></returns>
        private async Task<List<Domain.Models.User>> GetUserPartAsync(Guid[] userPomIds)
        {
            return await _dbUser.AsQueryable().Where(t => t.IsDelete == 1 && userPomIds.Contains(t.PomId)).Select(t => new Domain.Models.User() { Id = t.Id, PomId = t.PomId, Name = t.Name, Phone = t.Phone }).ToListAsync();
        }

        /// <summary>
        /// 批量存储文件
        /// </summary>
        /// <returns></returns>
        private async Task SaveFilesAsync(FileInfoRequestDto[] reqFiles)
        {
            var files = new List<Files>();
            foreach (var reqFile in reqFiles)
            {
                files.Add(new Files() { Id = (Guid)reqFile.FileId, Name = reqFile.Name, OriginName = reqFile.OriginName, SuffixName = reqFile.SuffixName });
            }
            await _dbFile.InsertRangeAsync(files);
        }

        #endregion


        #region 未填报交建通消息通知
        /// <summary>
        /// 未填报通知
        /// </summary>
        /// <param name="type">1是产值日报未填报通知   2是船舶日报  3是安监日报</param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> UnReportJjtNotifAsync(int type)
        {
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            List<UnpRrojectReportJjtResponseDto> unpRrojectReportJjtResponseDto = new List<UnpRrojectReportJjtResponseDto>();
            UnReportProjectsRequestDto unReportProjectsRequestDto = new UnReportProjectsRequestDto() { PageSize = 10000 };
            UnReportShipsRequestDto unReportShipsRequestDto = new UnReportShipsRequestDto() { PageSize = 10000 };
            //发消息
            List<SingleMessageTemplateRequestDto> singleMessageTemplateRequestDtos = new List<SingleMessageTemplateRequestDto>();
            if (type == 1)
            {
                var projectReportResult = await SearchUnReportProjectsAsync(unReportProjectsRequestDto);
                if (projectReportResult.Data != null && projectReportResult.Data.Any())
                {

                    foreach (var item in projectReportResult.Data)
                    {
                        unpRrojectReportJjtResponseDto.Add(new UnpRrojectReportJjtResponseDto()
                        {
                            DateDay = item.DateDay,
                            Id = item.ProjectId,
                            Name = item.ProjectName
                        });

                    }
                }
            }

            if (type == 2)
            {
                var shipReportResult = await SearchUnReportShipsAsync(unReportShipsRequestDto);
                if (shipReportResult.Data != null && shipReportResult.Data.Any())
                {
                    shipReportResult.Data.Select(x => new UnpRrojectReportJjtResponseDto
                    {
                        DateDay = x.DateDay,
                        Id = x.ProjectId.Value,
                        DayReportId = x.DayReportId.Value,
                        Name = x.ProjectName
                    });
                }
            }

            if (type == 3)
            {
                var safeReportResult = await SearchUnReportSafeProjectsAsync(unReportProjectsRequestDto);
                if (safeReportResult.Data != null && safeReportResult.Data.Any())
                {
                    safeReportResult.Data.Select(x => new UnpRrojectReportJjtResponseDto
                    {
                        DateDay = x.DateDay,
                        Id = x.ProjectId,
                        DayReportId = x.DayReportId.Value,
                        Name = x.ProjectName
                    });
                }
            }

            if (unpRrojectReportJjtResponseDto.Any())
            {
                var projectIds = unpRrojectReportJjtResponseDto.Select(x => x.Id).ToList();

                var projectList = await _dbProject.AsQueryable().Where(x => x.IsDelete == 1 && projectIds.Contains(x.Id)).Select(x => new Project() { Id = x.Id, ReportForMertel = x.ReportForMertel, CompanyId = x.CompanyId })
                      .ToListAsync();

                if (projectList.Any())
                {
                    var allReportformerPhone = projectList.Select(x => x.ReportForMertel).ToList();
                    //获取用户的人资编码
                    var jjtSendOfUser = await _dbUser.AsQueryable().Where(x => x.IsDelete == 1 && allReportformerPhone.Contains(x.Phone)).ToListAsync();
                    foreach (var item in unpRrojectReportJjtResponseDto)
                    {
                        var singleProject = projectList.SingleOrDefault(x => x.Id == item.Id);
                        var allUserAccount = jjtSendOfUser.SingleOrDefault(x => x.Phone == singleProject.ReportForMertel);
                        item.Reportformer = singleProject?.ReportForMertel;
                        item.CompanyId = singleProject?.CompanyId.Value;
                        item.UserAccount = allUserAccount?.LoginAccount;
                    }
                    var userGroup = unpRrojectReportJjtResponseDto.GroupBy(x => x.UserAccount);
                    //项目填报负责人发消息
                    foreach (var item in userGroup)
                    {

                        if (!string.IsNullOrWhiteSpace(item.Key))
                        {
                            var user = new List<string>() { item.Key };
                            var msgResponse = new SingleMessageTemplateRequestDto
                            {
                                IsAll = false,
                                UserIds = user,
                                MessageType = JjtMessageType.TEXT,
                            };
                            var keyValue = item.Select(x => x).ToList();
                            string projectName = string.Empty;
                            foreach (var value in keyValue)
                            {
                                projectName += value.Name + ",";
                            }
                            projectName = projectName.Substring(0, projectName.Length - 1);
                            msgResponse.TextContent = "您好，您所负责的（" + projectName + "），当前还没有填报。请您尽快填报！！！";
                            singleMessageTemplateRequestDtos.Add(msgResponse);
                        }

                    }
                    //项目所在公司的领导发消息
                    var companyProject = unpRrojectReportJjtResponseDto.GroupBy(x => x.CompanyId);
                    foreach (var item in companyProject)
                    {

                        //陈翠
                        var msgResponse = new SingleMessageTemplateRequestDto
                        {
                            IsAll = false,
                            MessageType = JjtMessageType.TEXT,
                        };
                        if (item.Key.Value == "bd840460-1e3a-45c8-abed-6e66903eb465".ToGuid())
                        {
                            msgResponse.UserIds = new List<string>() { "2016146340" };

                        }
                        if (item.Key.Value == "3c5b138b-601a-442a-9519-2508ec1c1eb2".ToGuid())
                        {
                            //朱智文
                            msgResponse.UserIds = new List<string>() { "2017028792" };

                        }
                        if (item.Key.Value == "a8db9bb0-4667-4320-b03d-b0b7f8728b61".ToGuid())
                        {
                            //陈河元
                            msgResponse.UserIds = new List<string>() { "2018008722" };

                        }
                        if (item.Key.Value == "11c9c978-9ef3-411d-ba70-0d0eed93e048".ToGuid())
                        {
                            //王琦
                            msgResponse.UserIds = new List<string>() { "2018009624" };

                        }
                        if (item.Key.Value == "c0d51e81-03dd-4ef8-bd83-6eb1355879e1".ToGuid())
                        {
                            //孟浩
                            msgResponse.UserIds = new List<string>() { "2017005354" };

                        }
                        if (item.Key.Value == "65052a94-6ea7-44ba-96b4-cf648de0d28a".ToGuid())
                        {
                            //李倩
                            msgResponse.UserIds = new List<string>() { "2020012309" };

                        }
                        if (item.Key.Value == "5a8f39de-8515-456b-8620-c0d01c012e04".ToGuid())
                        {
                            //杨加录
                            msgResponse.UserIds = new List<string>() { "2016042370" };

                        }
                        if (item.Key.Value == "ef7bdb95-e802-4bf5-a7ae-9ef5205cd624".ToGuid())
                        {
                            //赵锐
                            msgResponse.UserIds = new List<string>() { "L20080287" };

                        }
                        var keyValue = item.Select(x => x).ToList();
                        string projectName = string.Empty;
                        foreach (var value in keyValue)
                        {
                            projectName += value.Name + ",";
                        }
                        projectName = projectName.Substring(0, projectName.Length - 1);
                        msgResponse.TextContent = "您好，您所负责的（" + projectName + "），当前还没有填报。请您尽快填报！！！";
                        singleMessageTemplateRequestDtos.Add(msgResponse);
                    }
                    //发消息
                    foreach (var item in singleMessageTemplateRequestDtos)
                    {
                        JjtUtils.SinglePushMessage(item);
                    }



                }
            }
            return responseAjaxResult;

        }
        #endregion


        #region 操作日志

        /// <summary>
        /// New 一个操作日志
        /// </summary>
        /// <returns></returns>
        private LogInfo NewLogInfo(EntityType moduleType, Guid? dataId, ModelState state, Guid? operatorId = null, string? operatorName = null)
        {
            string moduleName = string.Empty;
            switch (moduleType)
            {
                case EntityType.DayReport:
                    moduleName = "/首页/项目日报/" + (state == ModelState.Add ? "填报" : "编辑");
                    break;
                case EntityType.SafeDayReport:
                    moduleName = "/首页/安监日报/" + (state == ModelState.Add ? "填报" : "编辑");
                    break;
                case EntityType.ShipDayReport:
                    moduleName = "/首页/船舶日报/" + (state == ModelState.Add ? "填报" : "编辑");
                    break;
                case EntityType.MonthReport:
                    moduleName = "/首页/项目月报/" + (state == ModelState.Add ? "填报" : "编辑");
                    break;
                case EntityType.SubShipMonthReport:
                    moduleName = "/首页/分包船舶月报/" + (state == ModelState.Add ? "填报" : "编辑");
                    break;
                case EntityType.OwnerShipMonthReport:
                    moduleName = "/首页/自有船舶月报/" + (state == ModelState.Add ? "填报" : "编辑");
                    break;
                case EntityType.MonthReportPushSpecialFields:
                    moduleName = "/进度与成本管控/项目月报/" + (state == ModelState.Update ? "推送字段编辑" : "");
                    break;

            }
            return new LogInfo()
            {
                Id = GuidUtil.Increment(),
                BusinessModule = moduleName,
                BusinessRemark = moduleName,
                OperationId = operatorId ?? _currentUser.Id,
                DataId = dataId,
                OperationName = operatorName ?? _currentUser.Name,
            };
        }


        #endregion

        /// <summary>
        /// 项目月度产报excel数据导出
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<ProjectMonthExcelResponseDto>> ProjectMonthExcelAsync(ProjectMonthReportRequestDto model)
        {
            ResponseAjaxResult<ProjectMonthExcelResponseDto> responseAjaxResult = new ResponseAjaxResult<ProjectMonthExcelResponseDto>();
            ProjectMonthExcelResponseDto projectMonthExcelResponseDto = new ProjectMonthExcelResponseDto();
            var wbsList = new List<ProjectWbsData>();
            //  在任务中导出
            ResponseAjaxResult<ProjectMonthReportResponseDto> data;
            //获取项目月报数据
            //  在任务中导出
            if (model.JobId != null && model.JobId != Guid.Empty)
            {
                var job = await _dbContext.Queryable<Domain.Models.Job>().SingleAsync(x => x.IsDelete == 1 && x.Id == model.JobId);
                data = new ResponseAjaxResult<ProjectMonthReportResponseDto>().SuccessResult(JsonConvert.DeserializeObject<ProjectMonthReportResponseDto>(job.BizData));
            }
            else
            {
                data = await SearchProjectMonthReportAsync(model);
            }
            if (data.Data != null)
            {
                projectMonthExcelResponseDto.TimeValue = data.Data.DateMonthTime;
                var projectName = await _dbContext.Queryable<Project>().FirstAsync(x => x.IsDelete == 1 && x.Id == data.Data.ProjectId);
                var name = projectName.Name;
                projectMonthExcelResponseDto.projectOverall =
                    new List<ProjectOverall>()
                    {
                    new ProjectOverall()
                    {
                        ProjectClassificationName = name,
                        ContractQuantity = data.Data.ContractQuantity,
                        ContractAmount = data.Data.ContractAmount,
                        CompletedQuantity = data.Data.CompletedQuantity,
                        CompleteProductionAmount= data.Data.CompleteProductionAmount,
                        OutsourcingExpensesAmount = data.Data.OutsourcingExpensesAmount,
                        YearCompletedQuantity=data.Data.YearCompletedQuantity,
                        YearCompleteProductionAmount= data.Data.YearCompleteProductionAmount,
                        YearOutsourcingExpensesAmount=data.Data.YearOutsourcingExpensesAmount,
                        TotalCompletedQuantity=data.Data.TotalCompletedQuantity,
                        TotalCompleteProductionAmount=data.Data.TotalCompleteProductionAmount,
                        TotalOutsourcingExpensesAmount = data.Data.TotalOutsourcingExpensesAmount
                    }
                    };
                wbsList = MapTreeResNode2(data.Data.TreeDetails, wbsList, "0");
                projectMonthExcelResponseDto.projectWbsDatas = wbsList;
            }
            responseAjaxResult.Data = projectMonthExcelResponseDto;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }

        /// <summary>
        /// 递归取出树结构中的每一条数据
        /// </summary>
        /// <param name="list"></param>
        /// <param name="wbsList"></param>
        /// <returns></returns>
        private List<ProjectWbsData> MapTreeResNode2(ProjectMonthReportResponseDto.ResTreeProjectWBSDetailDto[] list, List<ProjectWbsData> wbsList, string level)
        {
            for (int i = 0; i < list.Length; i++)
            {
                var model = new ProjectWbsData();
                _mapper.Map(list[i], model);
                var childLevel = string.Empty;
                if (list[i].Pid == "0")
                {
                    childLevel = (i + 1).ToString();
                }
                else
                {
                    childLevel = $"{level}.{i + 1}";
                }
                model.Level = childLevel;
                wbsList.Add(model);
                if (list[i].ReportDetails.Any())
                {
                    for (int j = 0; j < list[i].ReportDetails.Count; j++)
                    {
                        var report = new ProjectWbsData();
                        _mapper.Map(list[i].ReportDetails[j], report);
                        report.Level = $"{model.Level}.{j + 1}";
                        wbsList.Add(report);
                    }
                }
                MapTreeResNode2(list[i].Children, wbsList, childLevel);
            }
            return wbsList;
        }


        /// <summary>
        /// 产值产量汇总excel数据导出
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<ProjectOutPutValueExcelResponseDto>> SearchProjectOutPutExcelAsync(ProjectOutPutExcelRequestDto putExcelRequestDto)
        {
            ResponseAjaxResult<ProjectOutPutValueExcelResponseDto> responseAjaxResult = new ResponseAjaxResult<ProjectOutPutValueExcelResponseDto>();
            ProjectOutPutValueExcelResponseDto putValueExcelResponseDto = new ProjectOutPutValueExcelResponseDto();
            var now = DateTime.Now;
            //获取月份时间段
            Utils.GetDatePeriod(putExcelRequestDto.SearchTime, Convert.ToInt32(TimeType.lastMonth), out DateTime startTime, out DateTime endTime);
            //项目历史数据
            var projectHistoryData = await _dbContext.Queryable<ProjectHistoryData>().ToListAsync();
            // 排除掉公司（广航局总体）
            var excludeNames = new string?[] { "广航局总体" };
            // 排除掉的项目（斯里兰卡科伦坡港口城发展项目）
            var excludeProjectIds = new Guid[] { "08db3b35-fb38-4bb9-8213-fe97a3275400".ToGuid() };
            // 统计的公司
            var totalCompanys = await _dbContext.Queryable<ProductionMonitoringOperationDayReport>().Where(t => t.IsDelete == 1 && t.Type == 1).ToListAsync();
            totalCompanys = totalCompanys.Where(t => !excludeNames.Contains(t.Name)).ToList();
            var totalCompanyIds = totalCompanys.Where(t => !excludeNames.Contains(t.Name)).Select(t => t.ItemId).ToArray();
            // 统计的项目
            var projects = await _dbContext.Queryable<Project>().Where(t => t.IsDelete == 1 && totalCompanyIds.Contains(t.CompanyId)).ToListAsync();
            projects = projects.Where(t => !excludeProjectIds.Contains(t.Id)).ToList();
            var projectDeptIds = projects.Select(t => t.ProjectDept).Distinct().ToArray();
            var institutions = _dbContext.Queryable<Institution>().Where(t => t.IsDelete == 1 && projectDeptIds.Contains(t.PomId)).ToArray();
            var company = new List<baseProjectData>();
            projects.ForEach(item =>
            {
                var sortCommany = totalCompanys.FirstOrDefault(t => t.ItemId == item.CompanyId);
                var ba = new baseProjectData() { ProjectId = item.Id, ProjectName = item.Name };
                ba.CompanyName = sortCommany?.Name;
                ba.DeptName = institutions.FirstOrDefault(t => t.PomId == item.ProjectDept)?.Name;
                ba.Sort = sortCommany?.Sort;
                ba.CompanyId = sortCommany?.ItemId;
                company.Add(ba);
            });
            //////获取公司基础信息
            //var company = await _dbContext.Queryable<ProductionMonitoringOperationDayReport>()
            //    .LeftJoin<Project>((pm, p) => pm.ItemId == p.CompanyId)
            //    .LeftJoin<Institution>((pm, p, i) => p.ProjectDept == i.PomId)
            //    .Where((pm, p, i) => pm.IsDelete == 1 && p.IsDelete == 1 && i.IsDelete == 1 && pm.Type == 1 && pm.Name != "广航局总体(个)")
            //    .Select((pm, p, i) =>
            //    new baseProjectData
            //    {
            //        CompanyId = pm.ItemId.Value,
            //        CompanyName = pm.Name,
            //        Sort = pm.Sort,
            //        ProjectId = p.Id,
            //        ProjectName = p.Name,
            //        DeptName = i.Shortname

            //    }).ToListAsync();
            ////获取自有船舶月报信息
            var ownShipList = await _dbOwnerShipMonthReport.AsQueryable().Where(x => x.IsDelete == 1).ToListAsync();
            //获取分包船舶月报信息
            var subShipList = await _dbSubShipMonthReport.AsQueryable().Where(x => x.IsDelete == 1).ToListAsync();
            //获取月报信息
            var monthReportList = await _dbMonthReportDetail.AsQueryable().Where(x => x.IsDelete == 1).ToListAsync();
            //获取本月的月报数据 
            var monthList = await _dbMonthReport.AsQueryable().Where(t => t.IsDelete == 1 && t.DateMonth == endTime.ToDateMonth())
                .Select(t => new
                {
                    DateMonth = t.DateMonth,
                    ProjectId = t.ProjectId,
                    Status = t.Status
                }).ToListAsync();
            //获取项目信息
            var projectList = await _dbProject.AsQueryable().Where(x => x.IsDelete == 1).ToListAsync();
            //获取自有船舶信息
            var ownList = await _dbOwnerShip.AsQueryable().Where(x => x.IsDelete == 1).ToListAsync();
            var keyNoteList = await SearchKeynoteShipExcelAsync(putExcelRequestDto, ownShipList, projectList, ownList);

            List<OutPutInfo> outPutInfos = new List<OutPutInfo>();
            #region Excel使用
            var projectIds = projectList.Select(x => x.Id).ToArray();
            // 累计产值/产量
            var sumMonthReports = await SumMonthReportsAsync(projectIds);
            #endregion
            foreach (var item in company)
            {
                #region 
                OutPutInfo outPutInfo = new OutPutInfo();
                outPutInfo.CompanySort = item.Sort;
                outPutInfo.CompanyId = item.CompanyId;
                if (item.CompanyId.ToString() == "ef7bdb95-e802-4bf5-a7ae-9ef5205cd624")
                {
                    outPutInfo.CompanyName = "设计院";
                }
                else
                {
                    outPutInfo.CompanyName = item.CompanyName;
                }
                if (monthList.Where(t => t.ProjectId == item.ProjectId && t.Status != MonthReportStatus.Finish).Any())
                {
                    outPutInfo.State = "审批中";
                }
                outPutInfo.DeptName = string.IsNullOrWhiteSpace(item.DeptName) == true ? "" : Regex.Replace(item.DeptName, @"\（.*\）", string.Empty);
                outPutInfo.DeptName = string.IsNullOrWhiteSpace(outPutInfo.DeptName) == true ? "" : Regex.Replace(outPutInfo.DeptName, @"\( .*\)", string.Empty);
                outPutInfo.DeptName = outPutInfo.DeptName.Replace("中交广航局", "").Replace("广航局", "");
                outPutInfo.ProjectName = item.ProjectName;
                //月度
                outPutInfo.OwnProduction = monthReportList.Where(x => x.ProjectId == item.ProjectId && x.OutPutType == ConstructionOutPutType.Self && x.DateMonth == endTime.ToDateMonth()).Sum(x => x.CompletedQuantity) / 10000;
                outPutInfo.SubProduction = monthReportList.Where(x => x.ProjectId == item.ProjectId && x.OutPutType == ConstructionOutPutType.SubPackage && x.DateMonth == endTime.ToDateMonth()).Sum(x => x.CompletedQuantity) / 10000;
                outPutInfo.SumProduction = outPutInfo.OwnProduction + outPutInfo.SubProduction;
                outPutInfo.OwnOutPutValue = monthReportList.Where(x => x.ProjectId == item.ProjectId && x.OutPutType == ConstructionOutPutType.Self && x.DateMonth == endTime.ToDateMonth()).Sum(x => x.CompleteProductionAmount) / 10000;
                outPutInfo.SubOutPutValue = monthReportList.Where(x => x.ProjectId == item.ProjectId && x.OutPutType == ConstructionOutPutType.SubPackage && x.DateMonth == endTime.ToDateMonth()).Sum(x => x.CompleteProductionAmount) / 10000;
                outPutInfo.SumOutPutValue = outPutInfo.OwnOutPutValue + outPutInfo.SubOutPutValue;
                //判断这个项目当月是否填报
                if (monthList.Where(t => t.ProjectId == item.ProjectId).Any())
                {
                    outPutInfo.SubExpenditure = monthReportList.Where(x => x.ProjectId == item.ProjectId && x.OutPutType == ConstructionOutPutType.SubPackage && x.DateMonth == endTime.ToDateMonth()).Sum(x => x.OutsourcingExpensesAmount) / 10000;
                    outPutInfo.SubDiffValue = outPutInfo.SumOutPutValue - outPutInfo.OwnOutPutValue - outPutInfo.SubExpenditure;
                }
                //年度
                if (endTime.Year == 2023)
                {
                    outPutInfo.YearOwnOutPutValue = monthReportList.Where(x => x.ProjectId == item.ProjectId && x.OutPutType == ConstructionOutPutType.Self && x.DateYear == endTime.Year && x.DateMonth <= endTime.ToDateMonth() && x.DateMonth > 202306).Sum(x => x.CompleteProductionAmount) / 10000;
                    outPutInfo.YearSubOutPutValue = monthReportList.Where(x => x.ProjectId == item.ProjectId && x.OutPutType == ConstructionOutPutType.SubPackage && x.DateYear == endTime.Year && x.DateMonth <= endTime.ToDateMonth() && x.DateMonth > 202306).Sum(x => x.CompleteProductionAmount) / 10000;
                    outPutInfo.YearSubProduction = monthReportList.Where(x => x.ProjectId == item.ProjectId && x.OutPutType == ConstructionOutPutType.SubPackage && x.DateYear == endTime.Year && x.DateMonth <= endTime.ToDateMonth() && x.DateMonth > 202306).Sum(x => x.CompletedQuantity) / 10000;
                    outPutInfo.YearOwnProduction = monthReportList.Where(x => x.ProjectId == item.ProjectId && x.OutPutType == ConstructionOutPutType.Self && x.DateYear == endTime.Year && x.DateMonth <= endTime.ToDateMonth() && x.DateMonth > 202306).Sum(x => x.CompletedQuantity) / 10000;
                    outPutInfo.YearSumProduction = outPutInfo.YearOwnProduction + outPutInfo.YearSubProduction + (Convert.ToDecimal(projectHistoryData.FirstOrDefault(x => x.ProjectId == item.ProjectId)?.Production) / 10000);
                    outPutInfo.YearSumOutPutValue = outPutInfo.YearOwnOutPutValue + outPutInfo.YearSubOutPutValue + (Convert.ToDecimal(projectHistoryData.FirstOrDefault(x => x.ProjectId == item.ProjectId)?.OutputValue) / 10000);
                    outPutInfo.YearSubExpenditure = monthReportList.Where(x => x.ProjectId == item.ProjectId && x.OutPutType == ConstructionOutPutType.SubPackage && x.DateMonth > 202306 && x.DateMonth <= endTime.ToDateMonth()).Sum(x => x.OutsourcingExpensesAmount) / 10000 + (Convert.ToDecimal(projectHistoryData.FirstOrDefault(x => x.ProjectId == item.ProjectId)?.OutsourcingExpenses) / 10000); ;
                }
                else
                {
                    outPutInfo.YearOwnOutPutValue = monthReportList.Where(x => x.ProjectId == item.ProjectId && x.OutPutType == ConstructionOutPutType.Self && x.DateYear == endTime.Year && x.DateMonth <= endTime.ToDateMonth()).Sum(x => x.CompleteProductionAmount) / 10000;
                    outPutInfo.YearSubOutPutValue = monthReportList.Where(x => x.ProjectId == item.ProjectId && x.OutPutType == ConstructionOutPutType.SubPackage && x.DateYear == endTime.Year && x.DateMonth <= endTime.ToDateMonth()).Sum(x => x.CompleteProductionAmount) / 10000;
                    outPutInfo.YearSubProduction = monthReportList.Where(x => x.ProjectId == item.ProjectId && x.OutPutType == ConstructionOutPutType.SubPackage && x.DateYear == endTime.Year && x.DateMonth <= endTime.ToDateMonth()).Sum(x => x.CompletedQuantity) / 10000;
                    outPutInfo.YearOwnProduction = monthReportList.Where(x => x.ProjectId == item.ProjectId && x.OutPutType == ConstructionOutPutType.Self && x.DateYear == endTime.Year && x.DateMonth <= endTime.ToDateMonth()).Sum(x => x.CompletedQuantity) / 10000;
                    outPutInfo.YearSubExpenditure = monthReportList.Where(x => x.ProjectId == item.ProjectId && x.OutPutType == ConstructionOutPutType.SubPackage && x.DateYear == endTime.Year && x.DateMonth <= endTime.ToDateMonth()).Sum(x => x.OutsourcingExpensesAmount) / 10000;
                    outPutInfo.YearSumProduction = outPutInfo.YearOwnProduction + outPutInfo.YearSubProduction;
                    outPutInfo.YearSumOutPutValue = outPutInfo.YearOwnOutPutValue + outPutInfo.YearSubOutPutValue;
                }
                outPutInfo.YearSubDiffValue = outPutInfo.YearSumOutPutValue - outPutInfo.YearOwnOutPutValue - outPutInfo.YearSubExpenditure;
                //若本年自行产量和本年分包产量和本年外包支出都为空 则跳过不展示
                if (outPutInfo.YearOwnOutPutValue == 0 && outPutInfo.YearSubDiffValue == 0 && outPutInfo.YearSubExpenditure == 0)
                {
                    continue;
                }

                #endregion
                //开累值计算
                #region 原来的
                //var ownValue = monthReportList.Where(x => x.ProjectId == item.ProjectId && x.OutPutType == ConstructionOutPutType.Self && x.DateMonth <= endTime.ToDateMonth() && x.DateMonth > 202306).Sum(x => x.CompleteProductionAmount) / 10000;
                //var subValue = monthReportList.Where(x => x.ProjectId == item.ProjectId && x.OutPutType == ConstructionOutPutType.SubPackage && x.DateMonth <= endTime.ToDateMonth() && x.DateMonth > 202306).Sum(x => x.CompleteProductionAmount) / 10000;
                //outPutInfo.TotalOutPutValue = ownValue + subValue + (Convert.ToDecimal(projectHistoryData.FirstOrDefault(x => x.ProjectId == item.ProjectId)?.AccumulatedOutputValue) / 10000);
                #endregion
                //开累值计算
                // 项目月报累计统计
                var sumMonthReport = sumMonthReports.FirstOrDefault(t => t.ProjectId == item.ProjectId);
                if (sumMonthReport != null)
                {
                    outPutInfo.TotalOutPutValue = Math.Round(sumMonthReport.CompleteProductionAmount / 10000, 2);
                }
                outPutInfos.Add(outPutInfo);
            }

            //计算所有公司汇总值
            List<SumOutPutInfo> sumOutPutInfos = new List<SumOutPutInfo>()
            {
                 new  SumOutPutInfo()
                 {
                        OwnProduction = outPutInfos.Sum(x=>x.OwnProduction),
                        SubProduction = outPutInfos.Sum(x=>x.SubProduction),
                        SumProduction = outPutInfos.Sum(x=>x.SumProduction),
                        OwnOutPutValue = outPutInfos.Sum(x=>x.OwnOutPutValue),
                        SubDiffValue = outPutInfos.Sum(x=>x.SubDiffValue),
                        SubExpenditure = outPutInfos.Sum(x=>x.SubExpenditure),
                        SumOutPutValue = outPutInfos.Sum(x=>x.SumOutPutValue),
                        YearOwnProduction = outPutInfos.Sum(x=>x.YearOwnProduction),
                        YearSubProduction = outPutInfos.Sum(x=>x.YearSubProduction),
                        YearSumProduction = outPutInfos.Sum(x=>x.YearSumProduction),
                        YearOwnOutPutValue = outPutInfos.Sum(x=>x.YearOwnOutPutValue),
                        YearSubDiffValue = outPutInfos.Sum(x=>x.YearSubDiffValue),
                        YearSubExpenditure = outPutInfos.Sum(x=>x.YearSubExpenditure),
                        YearSumOutPutValue = outPutInfos.Sum(x=>x.YearSumOutPutValue),
                        TotalSumOutPutValue = outPutInfos.Sum(x=>x.TotalOutPutValue)
                 }
            };
            // outPutInfos = outPutInfos.OrderBy(x => x.CompanyName).ToList();
            List<OutPutInfo> SumList = new List<OutPutInfo>();
            var SumCompanyGroup = outPutInfos.GroupBy(x => new { x.CompanyId, x.CompanyName, x.CompanySort });
            foreach (var item in SumCompanyGroup)
            {
                OutPutInfo model = new OutPutInfo();
                model.CompanyName = item.Key.CompanyName;
                model.DeptName = item.Key.CompanyName + " 汇总";
                //月度
                model.OwnProduction = item.Sum(x => x.OwnProduction);
                model.SubProduction = item.Sum(x => x.SubProduction);
                model.SumProduction = item.Sum(x => x.SumProduction);
                model.OwnOutPutValue = item.Sum(x => x.OwnOutPutValue);
                model.SubDiffValue = item.Sum(x => x.SubDiffValue);
                model.SubExpenditure = item.Sum(x => x.SubExpenditure);
                model.SumOutPutValue = item.Sum(x => x.SumOutPutValue);
                //年度
                model.YearOwnProduction = item.Sum(x => x.YearOwnProduction);
                model.YearSubProduction = item.Sum(x => x.YearSubProduction);
                model.YearSumProduction = item.Sum(x => x.YearSumProduction);
                model.YearOwnOutPutValue = item.Sum(x => x.YearOwnOutPutValue);
                model.YearSubDiffValue = item.Sum(x => x.YearSubDiffValue);
                model.YearSubExpenditure = item.Sum(x => x.YearSubExpenditure);
                model.YearSumOutPutValue = item.Sum(x => x.YearSumOutPutValue);
                model.TotalOutPutValue = item.Sum(x => x.TotalOutPutValue);
                model.CompanyId = item.Key.CompanyId;
                model.CompanySort = item.Key.CompanySort;
                SumList.Add(model);
            }
            SumList = SumList.OrderBy(t => t.CompanySort).ToList();
            List<OutPutInfo> AllList = new List<OutPutInfo>();
            foreach (var item in SumList)
            {
                //按公司插入汇总的数据
                AllList.Add(item);
                //然后按顺序插入公司对应的数据
                var ComList = outPutInfos.Where(x => item.CompanyId == x.CompanyId).ToList();
                AllList.AddRange(ComList);
            }

            putValueExcelResponseDto.TimeValue = endTime.Year + "年" + endTime.Month + "月";
            putValueExcelResponseDto.sumOutPutInfos = sumOutPutInfos;
            putValueExcelResponseDto.outPutInfos = AllList;
            putValueExcelResponseDto.keynoteShipInfos = keyNoteList;
            responseAjaxResult.Data = putValueExcelResponseDto;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }


        /// <summary>
        /// 产值产量汇总excel 数据导出 Npoi
        /// </summary>
        /// <returns></returns>
        public async Task<byte[]> SearchProjectOutPutNpoiExcelAsync(ProjectOutPutExcelRequestDto putExcelRequestDto)
        {
            //获取产值产量汇总
            var data = SearchProjectOutPutExcelAsync(putExcelRequestDto);

            //模板位置
            //var templatePath = @"D:\GHJCode\wom.api\GHMonitoringCenterApi.Domain.Shared\Template\Excel\ProjectMonthOutPutTemplate.xlsx";
            //var templatePath = "D:\\projectconllection\\dotnet\\szgh\\GHMonitoringCenterApi.Domain.Shared\\Template\\Excel\\ProjectMonthOutPutTemplate.xlsx";
            var templatePath = $"Template/Excel/ProjectMonthOutPutTemplate.xlsx";
            XSSFWorkbook workbook = null;
            using (var fs = new FileStream(templatePath, FileMode.Open, FileAccess.Read))
            {
                //创建工作簿
                workbook = new XSSFWorkbook(fs);
                #region 产值产量汇总
                //获取工作表
                ISheet sheet = workbook.GetSheetAt(0);
                ICellStyle cellStyle = workbook.CreateCellStyle();
                // 水平对齐
                cellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                //垂直对齐
                cellStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                cellStyle.DataFormat = workbook.CreateDataFormat().GetFormat("[$-10804]#,##0.0;-#,##0.0;\"\"");
                //设置字体
                IFont font = workbook.CreateFont();
                font.FontHeightInPoints = 11;
                font.FontName = "微软雅黑";
                cellStyle.SetFont(font);

                ICellStyle cellStyle1 = workbook.CreateCellStyle();
                // 水平对齐
                cellStyle1.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Left;
                //垂直对齐
                cellStyle1.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                //设置字体
                IFont font1 = workbook.CreateFont();
                font1.FontHeightInPoints = 12;
                font1.FontName = "微软雅黑";
                font1.IsBold = true;
                cellStyle1.SetFont(font1);

                ICellStyle cellStyle2 = workbook.CreateCellStyle();
                // 水平对齐
                cellStyle2.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                //垂直对齐
                cellStyle2.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                //设置字体
                IFont font2 = workbook.CreateFont();
                font2.FontHeightInPoints = 14;
                font2.FontName = "微软雅黑";
                font2.IsBold = true;
                cellStyle2.SetFont(font1);

                ICellStyle cellStyle4 = workbook.CreateCellStyle();
                // 水平对齐
                cellStyle4.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Left;
                //垂直对齐
                cellStyle4.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                //设置字体
                IFont font4 = workbook.CreateFont();
                font4.FontHeightInPoints = 11;
                font4.FontName = "微软雅黑";

                cellStyle4.SetFont(font);

                ICellStyle cellStyleBG1 = workbook.CreateCellStyle();
                // 水平对齐
                cellStyleBG1.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                //垂直对齐
                cellStyleBG1.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                cellStyleBG1.DataFormat = workbook.CreateDataFormat().GetFormat("[$-10804]#,##0.0;-#,##0.0;\"\"");
                //设置字体
                IFont fontSheetBG1 = workbook.CreateFont();
                fontSheetBG1.FontHeightInPoints = 11;
                fontSheetBG1.FontName = "微软雅黑";
                fontSheetBG1.Color = IndexedColors.Red.Index;
                cellStyleBG1.SetFont(fontSheetBG1);

                ICellStyle cellStyleBG2 = workbook.CreateCellStyle();
                // 水平对齐
                cellStyleBG2.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Left;
                //垂直对齐
                cellStyleBG2.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                //设置字体
                IFont fontSheetBG2 = workbook.CreateFont();
                fontSheetBG2.FontHeightInPoints = 11;
                fontSheetBG2.FontName = "微软雅黑";
                fontSheetBG2.Color = IndexedColors.Red.Index;
                cellStyleBG2.SetFont(fontSheetBG2);


                //标题
                var titleRow = sheet.GetRow(0);
                titleRow.CreateCell(0).SetCellValue(data.Result.Data.TimeValue + "产值产量汇总表");
                titleRow.GetCell(0).CellStyle = cellStyle2;

                //汇总值
                var row = sheet.GetRow(4);
                foreach (var item in data.Result.Data.sumOutPutInfos)
                {
                    row.CreateCell(3).SetCellValue(Convert.ToDouble(item.OwnProduction));
                    row.CreateCell(4).SetCellValue(Convert.ToDouble(item.SubProduction));
                    row.CreateCell(5).SetCellValue(Convert.ToDouble(item.SumProduction));
                    row.CreateCell(6).SetCellValue(Convert.ToDouble(item.OwnOutPutValue));
                    row.CreateCell(7).SetCellValue(Convert.ToDouble(item.SubDiffValue));
                    row.CreateCell(8).SetCellValue(Convert.ToDouble(item.SubExpenditure));
                    row.CreateCell(9).SetCellValue(Convert.ToDouble(item.SumOutPutValue));
                    row.CreateCell(10).SetCellValue(Convert.ToDouble(item.YearOwnProduction));
                    row.CreateCell(11).SetCellValue(Convert.ToDouble(item.YearSubOutPutValue));
                    row.CreateCell(12).SetCellValue(Convert.ToDouble(item.YearSumProduction));
                    row.CreateCell(13).SetCellValue(Convert.ToDouble(item.YearOwnOutPutValue));
                    row.CreateCell(14).SetCellValue(Convert.ToDouble(item.YearSubDiffValue));
                    row.CreateCell(15).SetCellValue(Convert.ToDouble(item.YearSubExpenditure));
                    row.CreateCell(16).SetCellValue(Convert.ToDouble(item.YearSumOutPutValue));
                    row.CreateCell(17).SetCellValue(Convert.ToDouble(item.TotalSumOutPutValue));
                }
                for (int i = 3; i < 18; i++)
                {
                    row.GetCell(i).CellStyle = cellStyle;
                }

                var rowIndex = 4;
                foreach (var item in data.Result.Data.outPutInfos)
                {
                    rowIndex++;
                    var datarow = sheet.CreateRow(rowIndex);
                    datarow.CreateCell(0).SetCellValue(item.CompanyName);
                    datarow.CreateCell(1).SetCellValue(item.DeptName);
                    datarow.CreateCell(2).SetCellValue(item.ProjectName);
                    datarow.CreateCell(3).SetCellValue(Convert.ToDouble(item.OwnProduction));
                    datarow.CreateCell(4).SetCellValue(Convert.ToDouble(item.SubProduction));
                    datarow.CreateCell(5).SetCellValue(Convert.ToDouble(item.SumProduction));
                    datarow.CreateCell(6).SetCellValue(Convert.ToDouble(item.OwnOutPutValue));
                    datarow.CreateCell(7).SetCellValue(Convert.ToDouble(item.SubDiffValue));
                    datarow.CreateCell(8).SetCellValue(Convert.ToDouble(item.SubExpenditure));
                    datarow.CreateCell(9).SetCellValue(Convert.ToDouble(item.SumOutPutValue));
                    datarow.CreateCell(10).SetCellValue(Convert.ToDouble(item.YearOwnProduction));
                    datarow.CreateCell(11).SetCellValue(Convert.ToDouble(item.YearSubProduction));
                    datarow.CreateCell(12).SetCellValue(Convert.ToDouble(item.YearSumProduction));
                    datarow.CreateCell(13).SetCellValue(Convert.ToDouble(item.YearOwnOutPutValue));
                    datarow.CreateCell(14).SetCellValue(Convert.ToDouble(item.YearSubDiffValue));
                    datarow.CreateCell(15).SetCellValue(Convert.ToDouble(item.YearSubExpenditure));
                    datarow.CreateCell(16).SetCellValue(Convert.ToDouble(item.YearSumOutPutValue));
                    datarow.CreateCell(17).SetCellValue(Convert.ToDouble(item.TotalOutPutValue));

                    datarow.GetCell(0).CellStyle = cellStyle;
                    datarow.GetCell(1).CellStyle = cellStyle;
                    if (item.State == "审批中")
                    {
                        datarow.GetCell(2).CellStyle = cellStyleBG2;
                        datarow.GetCell(3).CellStyle = cellStyleBG1;
                        datarow.GetCell(4).CellStyle = cellStyleBG1;
                        datarow.GetCell(5).CellStyle = cellStyleBG1;
                        datarow.GetCell(6).CellStyle = cellStyleBG1;
                        datarow.GetCell(7).CellStyle = cellStyleBG1;
                        datarow.GetCell(8).CellStyle = cellStyleBG1;
                        datarow.GetCell(9).CellStyle = cellStyleBG1;
                        datarow.GetCell(10).CellStyle = cellStyleBG1;
                        datarow.GetCell(11).CellStyle = cellStyleBG1;
                        datarow.GetCell(12).CellStyle = cellStyleBG1;
                        datarow.GetCell(13).CellStyle = cellStyleBG1;
                        datarow.GetCell(14).CellStyle = cellStyleBG1;
                        datarow.GetCell(15).CellStyle = cellStyleBG1;
                        datarow.GetCell(16).CellStyle = cellStyleBG1;
                        datarow.GetCell(17).CellStyle = cellStyleBG1;
                    }
                    else
                    {
                        datarow.GetCell(2).CellStyle = cellStyle4;
                        datarow.GetCell(3).CellStyle = cellStyle;
                        datarow.GetCell(4).CellStyle = cellStyle;
                        datarow.GetCell(5).CellStyle = cellStyle;
                        datarow.GetCell(6).CellStyle = cellStyle;
                        datarow.GetCell(7).CellStyle = cellStyle;
                        datarow.GetCell(8).CellStyle = cellStyle;
                        datarow.GetCell(9).CellStyle = cellStyle;
                        datarow.GetCell(10).CellStyle = cellStyle;
                        datarow.GetCell(11).CellStyle = cellStyle;
                        datarow.GetCell(12).CellStyle = cellStyle;
                        datarow.GetCell(13).CellStyle = cellStyle;
                        datarow.GetCell(14).CellStyle = cellStyle;
                        datarow.GetCell(15).CellStyle = cellStyle;
                        datarow.GetCell(16).CellStyle = cellStyle;
                        datarow.GetCell(17).CellStyle = cellStyle;
                    }
                    if (item.DeptName.Contains("汇总"))
                    {
                        sheet.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex, 1, 2));
                        datarow.GetCell(1).CellStyle = cellStyle1;
                    }
                }

                var firstCellZero = 0;
                var lastCellZero = 0;
                var firstCellOne = 0;
                var lastCellOne = 0;
                // 合并相同单元格的列
                for (var rowIndexMer = 6; rowIndexMer <= sheet.LastRowNum; rowIndexMer++)
                {
                    //获取当前行
                    var currentRow = sheet.GetRow(rowIndexMer);
                    //获取当前上一行
                    var previousRow = sheet.GetRow(rowIndexMer - 1);
                    //合并第一列的单元格
                    MergedReginCells(sheet, currentRow, previousRow, rowIndexMer, 0, ref firstCellZero, ref lastCellZero);
                    //合并第二列的单元格
                    MergedReginCells(sheet, currentRow, previousRow, rowIndexMer, 1, ref firstCellOne, ref lastCellOne);
                }
                #endregion

                #region 重点船舶


                // 获取工作表
                ISheet sheet2 = workbook.GetSheetAt(2);
                ICellStyle sheetTwoCellStyle = workbook.CreateCellStyle();
                // 水平对齐
                sheetTwoCellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                //垂直对齐
                sheetTwoCellStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                //设置字体
                IFont fontSheetTwo = workbook.CreateFont();
                fontSheetTwo.FontHeightInPoints = 11;
                fontSheetTwo.FontName = "微软雅黑";
                sheetTwoCellStyle.SetFont(fontSheetTwo);

                ICellStyle sheetTwoCellStyle2 = workbook.CreateCellStyle();
                // 水平对齐
                sheetTwoCellStyle2.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                //垂直对齐
                sheetTwoCellStyle2.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                sheetTwoCellStyle2.FillForegroundColor = 0;
                sheetTwoCellStyle2.FillPattern = FillPattern.SolidForeground;
                ((XSSFColor)sheetTwoCellStyle2.FillForegroundColorColor).SetRgb(new byte[] { 183, 222, 232 });
                //设置字体
                IFont fontSheetTwo2 = workbook.CreateFont();
                fontSheetTwo2.FontHeightInPoints = 11;
                fontSheetTwo2.FontName = "微软雅黑";
                fontSheetTwo2.IsBold = true;
                sheetTwoCellStyle2.SetFont(fontSheetTwo2);

                ICellStyle sheetTwoCellStyle3 = workbook.CreateCellStyle();
                // 水平对齐
                sheetTwoCellStyle3.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                //垂直对齐
                sheetTwoCellStyle3.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                //设置字体
                IFont fontSheetTwo3 = workbook.CreateFont();
                fontSheetTwo3.FontHeightInPoints = 11;
                fontSheetTwo3.FontName = "微软雅黑";
                sheetTwoCellStyle3.SetFont(fontSheetTwo3);

                ICellStyle sheetTwoCellStyle5 = workbook.CreateCellStyle();
                // 水平对齐
                sheetTwoCellStyle5.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Left;
                //垂直对齐
                sheetTwoCellStyle5.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                sheetTwoCellStyle5.SetFont(fontSheetTwo);

                ICellStyle sheetTwoCellStyleBG1 = workbook.CreateCellStyle();
                // 水平对齐
                sheetTwoCellStyleBG1.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                //垂直对齐
                sheetTwoCellStyleBG1.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                //设置字体
                IFont fontSheetTwoBG1 = workbook.CreateFont();
                fontSheetTwoBG1.FontHeightInPoints = 11;
                fontSheetTwoBG1.FontName = "微软雅黑";
                fontSheetTwoBG1.Color = IndexedColors.Red.Index;
                sheetTwoCellStyleBG1.SetFont(fontSheetTwoBG1);

                ICellStyle sheetTwoCellStyleBG3 = workbook.CreateCellStyle();
                // 水平对齐
                sheetTwoCellStyleBG3.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Left;
                //垂直对齐
                sheetTwoCellStyleBG3.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                //设置字体
                IFont fontSheetTwoBG3 = workbook.CreateFont();
                fontSheetTwoBG3.FontHeightInPoints = 11;
                fontSheetTwoBG3.FontName = "微软雅黑";
                fontSheetTwoBG3.Color = IndexedColors.Red.Index;
                sheetTwoCellStyleBG3.SetFont(fontSheetTwoBG3);


                var rowIndex2 = 0;
                foreach (var item in data.Result.Data.keynoteShipInfos)
                {
                    rowIndex2++;
                    var datarow = sheet2.CreateRow(rowIndex2);
                    datarow.CreateCell(0).SetCellValue(item.ShipType);
                    if (!item.ShipName.Contains("汇总"))
                    {
                        datarow.CreateCell(1).SetCellValue(item.ShipName);
                    }
                    else
                    {
                        datarow.CreateCell(1).SetCellValue("");
                    }
                    datarow.CreateCell(2).SetCellValue(item.ProjectName);
                    datarow.CreateCell(3).SetCellValue(Math.Round(Convert.ToDouble(item.ProductionValue), 2));
                    datarow.CreateCell(4).SetCellValue(Math.Round(Convert.ToDouble(item.OutputValue), 2));
                    datarow.CreateCell(5).SetCellValue(Math.Round(Convert.ToDouble(item.OnSiteDays), 0));
                    datarow.CreateCell(6).SetCellValue(Math.Round(Convert.ToDouble(item.WorkingHours), 2));
                    datarow.CreateCell(7).SetCellValue(Convert.ToDouble(item.MyPropertyRate1));
                    datarow.CreateCell(8).SetCellValue(Math.Round(Convert.ToDouble(item.YearProductionValue), 2));
                    datarow.CreateCell(9).SetCellValue(Math.Round(Convert.ToDouble(item.YearOutputValue), 2));
                    datarow.CreateCell(10).SetCellValue(Math.Round(Convert.ToDouble(item.YearOnSiteDays), 0));
                    datarow.CreateCell(11).SetCellValue(Math.Round(Convert.ToDouble(item.YearWorkingHours), 2));
                    datarow.CreateCell(12).SetCellValue(Convert.ToDouble(item.MyPropertyRate2));
                    if (item.ShipName.Contains("汇总"))
                    {
                        datarow.GetCell(0).CellStyle = sheetTwoCellStyle2;
                        datarow.GetCell(1).CellStyle = sheetTwoCellStyle2;
                        datarow.GetCell(2).CellStyle = sheetTwoCellStyle2;
                        datarow.GetCell(3).CellStyle = sheetTwoCellStyle2;
                        datarow.GetCell(4).CellStyle = sheetTwoCellStyle2;
                        datarow.GetCell(5).CellStyle = sheetTwoCellStyle2;
                        datarow.GetCell(6).CellStyle = sheetTwoCellStyle2;
                        datarow.GetCell(7).CellStyle = sheetTwoCellStyle2;
                        datarow.GetCell(8).CellStyle = sheetTwoCellStyle2;
                        datarow.GetCell(9).CellStyle = sheetTwoCellStyle2;
                        datarow.GetCell(10).CellStyle = sheetTwoCellStyle2;
                        datarow.GetCell(11).CellStyle = sheetTwoCellStyle2;
                        datarow.GetCell(12).CellStyle = sheetTwoCellStyle2;
                    }
                    else
                    {
                        if (item.State == "审批中")
                        {
                            datarow.GetCell(0).CellStyle = sheetTwoCellStyleBG1;
                            datarow.GetCell(1).CellStyle = sheetTwoCellStyleBG1;
                            datarow.GetCell(2).CellStyle = sheetTwoCellStyleBG3;
                            datarow.GetCell(3).CellStyle = sheetTwoCellStyleBG1;
                            datarow.GetCell(4).CellStyle = sheetTwoCellStyleBG1;
                            datarow.GetCell(5).CellStyle = sheetTwoCellStyleBG1;
                            datarow.GetCell(6).CellStyle = sheetTwoCellStyleBG1;
                            datarow.GetCell(7).CellStyle = sheetTwoCellStyleBG1;
                            datarow.GetCell(8).CellStyle = sheetTwoCellStyleBG1;
                            datarow.GetCell(9).CellStyle = sheetTwoCellStyleBG1;
                            datarow.GetCell(10).CellStyle = sheetTwoCellStyleBG1;
                            datarow.GetCell(11).CellStyle = sheetTwoCellStyleBG1;
                            datarow.GetCell(12).CellStyle = sheetTwoCellStyleBG1;
                        }
                        else
                        {
                            datarow.GetCell(0).CellStyle = sheetTwoCellStyle;
                            datarow.GetCell(1).CellStyle = sheetTwoCellStyle;
                            datarow.GetCell(2).CellStyle = sheetTwoCellStyle5;
                            datarow.GetCell(3).CellStyle = sheetTwoCellStyle;
                            datarow.GetCell(4).CellStyle = sheetTwoCellStyle;
                            datarow.GetCell(5).CellStyle = sheetTwoCellStyle3;
                            datarow.GetCell(6).CellStyle = sheetTwoCellStyle3;
                            datarow.GetCell(7).CellStyle = sheetTwoCellStyle3;
                            datarow.GetCell(8).CellStyle = sheetTwoCellStyle;
                            datarow.GetCell(9).CellStyle = sheetTwoCellStyle;
                            datarow.GetCell(10).CellStyle = sheetTwoCellStyle3;
                            datarow.GetCell(11).CellStyle = sheetTwoCellStyle3;
                            datarow.GetCell(12).CellStyle = sheetTwoCellStyle3;
                        }
                    }
                }
                #endregion

                //内存流对象
                using var memory = new MemoryStream();
                workbook.Write(memory);
                return memory.ToArray();
            }
        }

        /// <summary>
        /// 合并单元格
        /// </summary>
        private void MergedReginCells(ISheet sheet, IRow currentRow, IRow previousRow, int rowIndexMer, int column, ref int firstCell, ref int lastCell)
        {
            //判断当前行是否与上一行相同  第一列
            if (currentRow.GetCell(column).ToString() == previousRow.GetCell(column).ToString())
            {
                //这里是出现多条相同记录时  记录第一行的行号 与 最后一行的行号
                if (firstCell == 0)
                {
                    firstCell = previousRow.RowNum;
                }
                lastCell = currentRow.RowNum;
                //当前行数等于最后一行时进行合并
                if (rowIndexMer == sheet.LastRowNum)
                {
                    // 合并当前单元格与上一行相同列的单元格
                    sheet.AddMergedRegion(new CellRangeAddress(firstCell, lastCell, column, column));
                }
            }
            else
            {
                //若值都不为空进行合并 然后清空重新记录
                if (firstCell > 0 && lastCell > 0)
                {
                    // 合并当前单元格与上一行相同列的单元格
                    sheet.AddMergedRegion(new CellRangeAddress(firstCell, lastCell, column, column));
                    firstCell = 0;
                    lastCell = 0;
                }
            }
        }

        #region 暂时不用
        /// <summary>
        /// 合并单元格
        /// </summary>
        //private void MergedReginCells(ISheet sheet, IRow currentRow, IRow previousRow, int rowIndexMer, int column, ref int firstCell, ref int lastCell)
        //{
        //    //判断当前行是否与上一行相同  第一列
        //    if (currentRow.GetCell(column).ToString() == previousRow.GetCell(column).ToString())
        //    {
        //        //这里是出现多条相同记录时  记录第一行的行号 与 最后一行的行号
        //        if (firstCell == 0)
        //        {
        //            firstCell = previousRow.RowNum;
        //        }
        //        lastCell = currentRow.RowNum;
        //        //当前行数等于最后一行时进行合并
        //        if (rowIndexMer == sheet.LastRowNum)
        //        {
        //            // 合并当前单元格与上一行相同列的单元格
        //            sheet.AddMergedRegion(new CellRangeAddress(firstCell, lastCell, column, column));
        //        }
        //    }
        //    else
        //    {
        //        //若值都不为空进行合并 然后清空重新记录
        //        if (firstCell > 0 && lastCell > 0)
        //        {
        //            // 合并当前单元格与上一行相同列的单元格
        //            sheet.AddMergedRegion(new CellRangeAddress(firstCell, lastCell, column, column));
        //            firstCell = 0;
        //            lastCell = 0;
        //        }
        //    }
        //}
        #endregion

        /// <summary>
        /// 重点船舶数据查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<KeynoteShipInfo>> SearchKeynoteShipExcelAsync(ProjectOutPutExcelRequestDto putExcelRequestDto, List<OwnerShipMonthReport> ownerShips, List<Project> projects, List<OwnerShip> ships)
        {
            List<KeynoteShipInfo> keynoteList = new List<KeynoteShipInfo>();
            //获取月份时间段
            Utils.GetDatePeriod(putExcelRequestDto.SearchTime, Convert.ToInt32(TimeType.lastMonth), out DateTime startTime, out DateTime endTime);
            //获取所有自有船舶月报
            var ownerShipMonthReportList = await _dbContext.Queryable<OwnerShipMonthReport>().ToListAsync();
            var ownShipDay = await _dbContext.Queryable<ShipDayReport>().Where(x => x.DateDay >= startTime.ToDateDay() && x.DateDay <= endTime.ToDateDay()).ToListAsync();
            var shipMovementList = await _dbContext.Queryable<ShipMovement>().ToListAsync();

            #region 新逻辑
            //新逻辑
            var primaryIds = ownShipDay.Where(t => t.ProjectId == Guid.Empty).Select(t => new { id = t.Id, shipId = t.ShipId, DateDay = t.DateDay, ProjectId = t.ProjectId }).ToList();
            foreach (var item in primaryIds)
            {
                var isExistProject = shipMovementList.Where(x => x.IsDelete == 1 && x.ShipId == item.shipId && x.EnterTime.HasValue == true && (x.EnterTime.Value.ToDateDay() <= item.DateDay || x.QuitTime.HasValue == true && x.QuitTime.Value.ToDateDay() >= item.DateDay)).OrderByDescending(x => x.EnterTime).ToList();
                foreach (var project in isExistProject)
                {

                    if (project.QuitTime.HasValue && project.QuitTime.Value.ToDateDay() >= item.DateDay)
                    {

                        var oldValue = ownShipDay.Where(t => t.ShipId == project.ShipId && t.DateDay == item.DateDay).FirstOrDefault();
                        if (oldValue != null)
                        {
                            oldValue.ProjectId = project.ProjectId;
                        }
                    }
                    if (project.EnterTime.HasValue && project.QuitTime.HasValue == false && project.EnterTime.Value.ToDateDay() <= item.DateDay)
                    {
                        var oldValue = ownShipDay.Where(t => t.ShipId == project.ShipId && t.DateDay == item.DateDay).FirstOrDefault();
                        if (oldValue != null)
                        {
                            oldValue.ProjectId = project.ProjectId;
                        }
                    }
                    if (project.EnterTime.HasValue && project.QuitTime.HasValue && project.QuitTime.Value.ToDateDay() >= item.DateDay)
                    {

                        var oldValue = ownShipDay.Where(t => t.ShipId == project.ShipId && t.DateDay == item.DateDay).FirstOrDefault();
                        if (oldValue != null)
                        {
                            oldValue.ProjectId = project.ProjectId;
                        }
                    }
                }

            }

            #endregion
            var ownerMonthList = ownerShips.Where(x => x.DateMonth == endTime.ToDateMonth()).OrderBy(x => x.ShipId).ToList();
            var projectIds = ownerMonthList.Select(x => x.ProjectId).ToList();
            var shipIds = ownerMonthList.Select(x => x.ShipId).ToList();

            // 获取排序配置
            var selTypes = new int?[] { 2, 3 };
            var sortOptions = await _dbContext.Queryable<ProductionMonitoringOperationDayReport>().Where(t => selTypes.Contains(t.Type) && t.IsDelete == 1).ToListAsync();
            // 历史船舶数据
            var sumHistoryShipReports = await SumHistoryPojectShipReportAsync(endTime.Year);
            //获取项目信息
            var projectList = projects.Where(x => x.IsDelete == 1 && projectIds.Contains(x.Id)).ToList();
            //获取船舶信息
            var shipList = ships.Where(x => x.IsDelete == 1 && shipIds.Contains(x.PomId)).ToList();
            //获取本月的月报数据 
            var monthList = await _dbMonthReport.AsQueryable().Where(t => t.IsDelete == 1 && t.DateMonth == endTime.ToDateMonth())
                .Select(t => new
                {
                    DateMonth = t.DateMonth,
                    ProjectId = t.ProjectId,
                    Status = t.Status
                }).ToListAsync();
            var JuneShipList = new List<JuneKeyNote>();
            //获取重点船舶23年6月份的数据
            JuneShipList = await _dbContext.Queryable<JuneKeyNote>().Where(t => t.IsDelete == 1).ToListAsync();
            foreach (var item in ownerMonthList)
            {
                KeynoteShipInfo keynote = new KeynoteShipInfo();
                keynote.ShipType = shipList.Where(x => x.PomId == item.ShipId).FirstOrDefault() == null ? "" : shipList.Where(x => x.PomId == item.ShipId).FirstOrDefault().TypeClass;
                keynote.ShipId = item.ShipId;
                keynote.ShipName = shipList.Where(x => x.PomId == item.ShipId).FirstOrDefault() == null ? "" : shipList.Where(x => x.PomId == item.ShipId).FirstOrDefault().Name;
                keynote.ProjectId = item.ProjectId;
                keynote.ProjectName = projectList.Where(x => x.Id == item.ProjectId).FirstOrDefault() == null ? "" : projectList.Where(x => x.Id == item.ProjectId).FirstOrDefault().ShortName;
                keynote.ProductionValue = item.Production / 10000;
                keynote.OutputValue = item.ProductionAmount / 10000;
                var shipMovement = ownerShips.Where(x => x.ShipId == item.ShipId && x.ProjectId == item.ProjectId && x.DateMonth == item.DateMonth).FirstOrDefault();
                keynote.OnSiteDays = ownShipDay.Where(x => x.ProjectId == item.ProjectId && x.ShipId == item.ShipId).ToList().Count();
                keynote.WorkingHours = item.WorkingHours;
                keynote.MyPropertyRate1 = item.WorkingHours == 0 ? 0 : Convert.ToInt32(Math.Round(keynote.ProductionValue.Value, 1) * 10000 / Math.Round(item.WorkingHours, 2));
                keynote.YearProductionValue = ownerShips.Where(x => x.ShipId == item.ShipId && x.ProjectId == item.ProjectId && x.DateYear == endTime.Year && x.DateMonth <= endTime.ToDateMonth()).Sum(x => x.Production) / 10000;
                keynote.YearOutputValue = ownerShips.Where(x => x.ShipId == item.ShipId && x.ProjectId == item.ProjectId && x.DateYear == endTime.Year && x.DateMonth <= endTime.ToDateMonth()).Sum(x => x.ProductionAmount) / 10000;
                keynote.YearWorkingHours = ownerShips.Where(x => x.ShipId == item.ShipId && x.ProjectId == item.ProjectId && x.DateYear == endTime.Year && x.DateMonth <= endTime.ToDateMonth()).Sum(x => x.WorkingHours);
                keynote.MyPropertyRate2 = keynote.YearWorkingHours == 0 ? 0 : Convert.ToInt32(Math.Round(keynote.YearProductionValue.Value, 1) * 10000 / Math.Round(keynote.YearWorkingHours.Value, 2));
                keynote.YearOnSiteDays = Convert.ToInt32(ownerShipMonthReportList.Where(x => x.ProjectId == item.ProjectId && x.ShipId == item.ShipId).Sum(t => t.ConstructionDays));
                if (JuneShipList.Where(t => t.ProjectId == item.ProjectId.ToString() && t.ShipId == item.ShipId).Any())
                {
                    var value = JuneShipList.Where(t => t.ProjectId == item.ProjectId.ToString() && t.ShipId == item.ShipId).Single().YearOnSiteDays;
                    keynote.YearOnSiteDays = keynote.YearOnSiteDays + value;
                    var value2 = JuneShipList.Where(t => t.ProjectId == item.ProjectId.ToString() && t.ShipId == item.ShipId).Single().YearWorkingHours;
                    keynote.YearWorkingHours = keynote.YearWorkingHours + value2;
                }
                if (monthList.Where(t => t.ProjectId == item.ProjectId && t.Status != MonthReportStatus.Finish).Any())
                {
                    keynote.State = "审批中";
                }
                // 加入排序
                var thisOwnerShip = shipList.FirstOrDefault(t => t.PomId == item.ShipId);
                if (thisOwnerShip != null)
                {
                    var shipTypeSortOption = sortOptions.FirstOrDefault(t => t.Type == 2 && t.ItemId == thisOwnerShip.TypeId);
                    var shipSortOption = sortOptions.FirstOrDefault(t => t.Type == 3 && t.ItemId == thisOwnerShip.PomId);
                    keynote.ShipTypeSort = shipTypeSortOption?.Sort;
                    keynote.ShipSort = shipSortOption?.Sort;
                }
                // 加入历史差值
                var thisSumHistoryShipReport = sumHistoryShipReports.FirstOrDefault(t => t.ProjectId == item.ProjectId && t.ShipId == item.ShipId);
                if (thisSumHistoryShipReport != null)
                {
                    keynote.YearProductionValue += (thisSumHistoryShipReport.HistoryDiffProduction / 10000);
                    keynote.YearOutputValue += (thisSumHistoryShipReport.HistoryDiffProductionAmount / 10000);
                }
                keynoteList.Add(keynote);
            }

            //foreach (var item in keynoteList)
            //{
            //    var onDays = 0;
            //    //获取船舶一年的数据
            //    var projectShipList = ownerShips.Where(x => x.ProjectId == item.ProjectId && x.ShipId == item.ShipId && x.DateYear == endTime.Year && x.DateMonth <= endTime.ToDateMonth()).ToList();
            //    foreach (var item2 in projectShipList)
            //    {
            //        onDays += item2.QuitTime.Subtract(item2.EnterTime).Days + 1;
            //        item.YearOnSiteDays = onDays;
            //    }
            //}
            List<KeynoteShipInfo> sumShipInfos = new List<KeynoteShipInfo>();
            var groupList = keynoteList.GroupBy(x => new { x.ShipId, x.ShipName, x.ShipTypeSort, x.ShipSort }).ToList();
            foreach (var item in groupList)
            {

                //计算汇总值
                KeynoteShipInfo sumKeynoteShipInfo = new KeynoteShipInfo();
                sumKeynoteShipInfo.ShipName = item.Key.ShipName + " 汇总";
                sumKeynoteShipInfo.ProductionValue = item.Sum(x => x.ProductionValue);
                sumKeynoteShipInfo.OutputValue = item.Sum(x => x.OutputValue);
                sumKeynoteShipInfo.OnSiteDays = item.Sum(x => x.OnSiteDays);
                sumKeynoteShipInfo.WorkingHours = item.Sum(x => x.WorkingHours);
                sumKeynoteShipInfo.MyPropertyRate1 = item.Sum(x => x.WorkingHours) == 0 ? 0 : Convert.ToInt32(Math.Round(item.Sum(x => x.ProductionValue.Value), 1) * 10000 / Math.Round(item.Sum(x => x.WorkingHours.Value), 2));
                sumKeynoteShipInfo.YearProductionValue = item.Sum(x => x.YearProductionValue);
                sumKeynoteShipInfo.YearOutputValue = item.Sum(x => x.YearOutputValue);
                sumKeynoteShipInfo.YearWorkingHours = item.Sum(x => x.YearWorkingHours);
                sumKeynoteShipInfo.MyPropertyRate2 = item.Sum(x => x.YearWorkingHours) == 0 ? 0 : Convert.ToInt32(Math.Round(item.Sum(x => x.YearProductionValue.Value), 1) * 10000 / Math.Round(item.Sum(x => x.YearWorkingHours.Value), 2));
                sumKeynoteShipInfo.ShipId = item.Key.ShipId;
                sumKeynoteShipInfo.ShipTypeSort = item.Key.ShipTypeSort;
                sumKeynoteShipInfo.ShipSort = item.Key.ShipSort;
                sumShipInfos.Add(sumKeynoteShipInfo);
            }
            sumShipInfos = sumShipInfos.OrderBy(t => t.ShipTypeSort).ThenBy(t => t.ShipSort).ToList();
            List<KeynoteShipInfo> allKeynoteList = new List<KeynoteShipInfo>();
            foreach (var item in sumShipInfos)
            {
                var model = keynoteList.Where(x => item.ShipId == x.ShipId).ToList();
                allKeynoteList.AddRange(model);
                if (model.Count > 1)
                {
                    allKeynoteList.Add(item);
                }
            }
            return allKeynoteList;
        }

        /// <summary>
        /// 获取月报导出Word数据
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<List<MonthReportProjectWordResponseDto>>> SearchMonthReportProjectWordAsync(MonthtReportsRequstDto model)
        {
            var projectTypeId = "048120ae-1e9f-46d8-a38f-5d5e9e49ecba".ToGuid();
            //项目状态Id
            var projectStatusId = await _dbContext.Queryable<ProjectStatus>().Where(x => x.Sequence == 1 || x.Sequence == 2 || x.Sequence == 3).Select(x => x.StatusId).ToListAsync();
            //公司排序Id
            List<Guid> company = new List<Guid>()
            {
                new Guid("bd840460-1e3a-45c8-abed-6e66903eb465"),
                new Guid("3c5b138b-601a-442a-9519-2508ec1c1eb2"),
                new Guid("a8db9bb0-4667-4320-b03d-b0b7f8728b61"),
                new Guid("11c9c978-9ef3-411d-ba70-0d0eed93e048"),
                new Guid("5a8f39de-8515-456b-8620-c0d01c012e04"),
                new Guid("c0d51e81-03dd-4ef8-bd83-6eb1355879e1"),
                new Guid("65052a94-6ea7-44ba-96b4-cf648de0d28a"),
                new Guid("ef7bdb95-e802-4bf5-a7ae-9ef5205cd624")
            };
            var responseAjaxResult = new ResponseAjaxResult<List<MonthReportProjectWordResponseDto>>();
            var data = await SearchMonthReportsAsync(model);
            var projectMonthReportHistory = await _dbContext.Queryable<ProjectMonthReportHistory>().ToListAsync();
            var projectHistoryData = await _dbContext.Queryable<ProjectHistoryData>().ToListAsync();
            var time = model.StartTime.Value.Year;
            var ids = data.Data.Reports.Select(x => x.Id).ToList();
            var userAuthForData = await GetCurrentUserAuthForDataAsync();
            var monthReport = await _dbContext.Queryable<MonthReport>().ToListAsync();
            var currencyConverter = await _dbContext.Queryable<CurrencyConverter>().ToListAsync();
            var projectBaseInfo = await _dbContext.Queryable<MonthReport>()
                .LeftJoin<Project>((x, y) => x.ProjectId == y.Id)
                .LeftJoin<Institution>((x, y, z) => y.CompanyId == z.PomId)
                .LeftJoin<ProjectType>((x, y, z, s) => y.TypeId == s.PomId)
                .LeftJoin<ProjectScale>((x, y, z, s, q) => y.GradeId == q.PomId)
                .LeftJoin<ProjectStatus>((x, y, z, s, q, t) => y.StatusId == t.StatusId)
                .OrderBy((x, y) => y.Category)
                .OrderByDescending((x, y) => y.Latitude)
                .Where((x, y) => x.IsDelete == 1 && ids.Contains(x.Id) && company.Contains(y.CompanyId.Value) && projectStatusId.Contains(y.StatusId.Value) && y.TypeId != projectTypeId)
                .Select((x, y, z, s, q, t) => new MonthReportProjectWordResponseDto
                {
                    Id = x.Id,
                    CompanyId = y.CompanyId,
                    ProjectId = x.ProjectId,
                    MonthTime = x.DateMonth,
                    ProjectBelonging = z.Shortname,
                    ProjectName = y.Name,
                    ProjectType = s.Name,
                    ProjectGrade = q.Name,
                    ProjectLocation = y.ProjectLocation,
                    ProjectState = t.Name,
                    CompanyName = z.Shortname,
                    ProjectContent = y.QuantityRemarks,
                    CurrencyConverterId = y.CurrencyId,
                    OriginalContractAmount = y.Amount,
                    CommencementTime = string.IsNullOrWhiteSpace(y.CommencementTime.ToString()) ? null : "开工日期：" + y.CommencementTime.Value.ToString("yyyy年MM月dd日"),
                    //ChangeAmount = SqlFunc.Round((y.ECAmount - y.Amount) / 10000, 2),
                    ActualContractAmount = y.ECAmount,
                    //ContractChangeInformation = 
                    DurationInformation = y.DurationInformation,
                    ContractChangeInformation = y.ContractChangeInfo,
                    // MonthProjectDebriefing =
                    //工程产值
                    EngineeringMonthOutputValue = SqlFunc.Round(x.CompleteProductionAmount / 10000, 2),
                    //业主确认
                    OwnerMonthOutputValue = SqlFunc.Round(x.PartyAConfirmedProductionAmount / 10000, 2),
                    //工程收款
                    ProjectMonthOutputValue = SqlFunc.Round(x.PartyAPayAmount / 10000, 2),
                    MonthProjectDebriefing = x.ProgressDescription,
                    ExistingProblems = x.ProblemDescription,
                    TakeSteps = x.SolveProblemDescription,
                    CoordinationMatters = x.CoordinationMatters
                }).ToListAsync();
            var projectBaseInfo1 = projectBaseInfo.Where(x => company.Contains(x.CompanyId.Value)).OrderBy(x => company.IndexOf(x.CompanyId.Value)).ToList();
            var projectBaseInfo2 = projectBaseInfo.Where(x => !company.Contains(x.CompanyId.Value)).ToList();
            projectBaseInfo = projectBaseInfo1.Concat(projectBaseInfo2).ToList();
            foreach (var item in projectBaseInfo)
            {
                var exchangeRate = currencyConverter.FirstOrDefault(x => x.CurrencyId == item.CurrencyConverterId.ToString())?.ExchangeRate;
                item.OriginalContractAmount = Math.Round(Convert.ToDecimal((item.OriginalContractAmount) * exchangeRate / 10000), 2);
                item.ActualContractAmount = Math.Round(Convert.ToDecimal((item.ActualContractAmount) * exchangeRate / 10000), 2);
                item.ChangeAmount = Math.Round(Convert.ToDecimal(item.ActualContractAmount - item.OriginalContractAmount), 2);
                if (item.MonthTime.ToString().Contains("2023"))
                {
                    item.EngineeringYeahOutputValue = Math.Round((monthReport.Where(h => h.IsDelete == 1 && h.DateMonth.ToString().Contains(time.ToString()) && h.DateMonth > 202306 && h.ProjectId == item.ProjectId && h.DateMonth <= item.MonthTime).Sum(h => h.CompleteProductionAmount) + Convert.ToDecimal(projectHistoryData.FirstOrDefault(x => x.ProjectId == item.ProjectId)?.OutputValue)) / 10000, 2);
                    item.OwnerYeahOutputValue = Math.Round(monthReport.Where(h => h.IsDelete == 1 && h.DateMonth > 202308 && h.DateMonth.ToString().Contains(time.ToString()) && h.DateMonth >= 202308 && h.DateMonth <= item.MonthTime && h.ProjectId == item.ProjectId).Sum(h => h.PartyAConfirmedProductionAmount) / 10000, 2) + Convert.ToDecimal(projectMonthReportHistory.FirstOrDefault(x => x.ProjectId == item.ProjectId)?.YearOwnerConfirmation);
                    item.ProjectYeahOutputValue = Math.Round(monthReport.Where(h => h.IsDelete == 1 && h.DateMonth > 202308 && h.DateMonth.ToString().Contains(time.ToString()) && h.ProjectId == item.ProjectId && h.DateMonth <= item.MonthTime).Sum(h => h.PartyAPayAmount) / 10000, 2) + Convert.ToDecimal(projectMonthReportHistory.FirstOrDefault(x => x.ProjectId == item.ProjectId)?.YearProjectPayment); ;
                }
                else
                {
                    item.EngineeringYeahOutputValue = Math.Round(monthReport.Where(h => h.IsDelete == 1 && h.DateMonth <= item.MonthTime && h.DateMonth.ToString().Contains(time.ToString()) && h.ProjectId == item.ProjectId).Sum(h => h.CompleteProductionAmount) / 10000, 2);
                    item.OwnerYeahOutputValue = Math.Round(monthReport.Where(h => h.IsDelete == 1 && h.DateMonth <= item.MonthTime && h.DateMonth.ToString().Contains(time.ToString()) && h.ProjectId == item.ProjectId).Sum(h => h.PartyAConfirmedProductionAmount) / 10000, 2);
                    item.ProjectYeahOutputValue = Math.Round(monthReport.Where(h => h.IsDelete == 1 && h.DateMonth <= item.MonthTime && h.DateMonth.ToString().Contains(time.ToString()) && h.ProjectId == item.ProjectId).Sum(h => h.PartyAPayAmount) / 10000, 2);

                }
                //item.EngineeringAccumulatedEngineering = Math.Round((monthReport.Where(h => h.IsDelete == 1 && h.DateMonth >=202306 && h.ProjectId == item.ProjectId && h.DateMonth <= item.MonthTime).Sum(h => h.CompleteProductionAmount) + Convert.ToDecimal(projectHistoryData.FirstOrDefault(x => x.ProjectId == item.ProjectId)?.AccumulatedOutputValue)) / 10000, 2);
                item.EngineeringAccumulatedEngineering = Math.Round((monthReport.Where(h => h.IsDelete == 1 && h.DateMonth >= 202306 && h.ProjectId == item.ProjectId && h.DateMonth <= item.MonthTime).Sum(h => h.CompleteProductionAmount)) / 10000, 2);
                if (item.ActualContractAmount != 0 && item.ActualContractAmount != null && item.EngineeringAccumulatedEngineering != null)
                {
                    item.EngineeringProportion = Math.Round(item.EngineeringAccumulatedEngineering.Value / item.ActualContractAmount.Value, 4);
                }
                else
                {
                    item.EngineeringProportion = 0;

                }
                item.OwnerAccumulatedEngineering = Math.Round(monthReport.Where(h => h.IsDelete == 1 && h.DateMonth > 202308 && h.ProjectId == item.ProjectId && h.DateMonth <= item.MonthTime).Sum(h => h.PartyAConfirmedProductionAmount) / 10000, 2) + Convert.ToDecimal(projectMonthReportHistory.FirstOrDefault(x => x.ProjectId == item.ProjectId)?.KaileiOwnerConfirmation);
                if (item.EngineeringAccumulatedEngineering != 0 && item.EngineeringAccumulatedEngineering != null && item.OwnerAccumulatedEngineering != null)
                {
                    item.OwnerProportion = Math.Round(item.OwnerAccumulatedEngineering.Value / item.EngineeringAccumulatedEngineering.Value, 4);
                }
                else
                {
                    item.OwnerProportion = 0;

                }
                item.ProjectAccumulatedEngineering = Math.Round(monthReport.Where(h => h.IsDelete == 1 && h.DateMonth > 202308 && h.ProjectId == item.ProjectId && h.DateMonth <= item.MonthTime).Sum(h => h.PartyAPayAmount) / 10000, 2) + Convert.ToDecimal(projectMonthReportHistory.FirstOrDefault(x => x.ProjectId == item.ProjectId)?.KaileiProjectPayment); ;
                if (item.OwnerAccumulatedEngineering != 0 && item.OwnerAccumulatedEngineering != null && item.ProjectAccumulatedEngineering != null)
                {
                    item.ProjectProportion = Math.Round(item.ProjectAccumulatedEngineering.Value / item.OwnerAccumulatedEngineering.Value, 4);
                }
                else
                {
                    item.ProjectProportion = 0;

                }
            }
            responseAjaxResult.Success();
            responseAjaxResult.Data = projectBaseInfo;
            responseAjaxResult.Count = projectBaseInfo.Count;
            return responseAjaxResult;

        }

        /// <summary>
        /// 获取项目带班生产动态
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<ProjectShiftProductionResponseDto>> GetProjectShiftProductionAsync()
        {
            ResponseAjaxResult<ProjectShiftProductionResponseDto> responseAjaxResult = new ResponseAjaxResult<ProjectShiftProductionResponseDto>();
            ProjectShiftProductionResponseDto responseDto = new ProjectShiftProductionResponseDto();
            ProjectShiftProductionSumInfo productionSumInfo = new ProjectShiftProductionSumInfo();

            var holidayConfig = await _dbContext.Queryable<HolidayConfig>().Where(x => x.IsDelete == 1).FirstAsync();
            if (holidayConfig != null)
            {
                responseDto.Title = holidayConfig.Title;
            }
            //获取当前用户授权数据
            //var userAuthForData = await GetCurrentUserAuthForDataAsync();
            var time = DateTime.Now.AddDays(-1).ToDateDay();
            var statusList = CommonData.PConstruc.Split(',').Select(x => x.ToGuid()).ToList();
            //var list = await _dbProject.AsQueryable().LeftJoin<DayReport>((p, d) => p.Id == d.ProjectId && d.DateDay == time)
            //    .LeftJoin<Institution>((p, d, c) => p.CompanyId == c.PomId)
            //    .Where((p, d, c) => statusList.Contains((Guid)p.StatusId) && p.TypeId != CommonData.NoConstrutionProjectType)
            //    //.WhereIF(!userAuthForData.IsAdmin, (p, d, c) => userAuthForData.CompanyIds.Contains(p.ProjectDept))
            //    .Select((p, d, c) => new ProjectShiftProductionInfo
            //    {
            //        DayReportId = d.Id,
            //        CompanyId = c.PomId,
            //        CompanyName = c.Name,
            //        ProjectName = p.ShortName,
            //        ShiftLeader = d.ShiftLeader,
            //        ShiftPhone = d.ShiftLeaderPhone,
            //        SiteManagementPersonNum = d.SiteManagementPersonNum,
            //        SiteConstructionPersonNum = d.SiteConstructionPersonNum,
            //        ConstructionDeviceNum = d.ConstructionDeviceNum,
            //        FewLandWorkplace = d.FewLandWorkplace,
            //        LandWorkplace = d.LandWorkplace,
            //        SiteShipNum = d.SiteShipNum,
            //        OnShipPersonNum = d.OnShipPersonNum,
            //        HazardousConstructionNum = d.HazardousConstructionNum,
            //        HazardousConstructionDescription = d.HazardousConstructionDescription
            //    }).ToListAsync();
            var list = await _dbProject.AsQueryable()
                .LeftJoin<DayReport>((p, d) => p.Id == d.ProjectId && d.DateDay == time)
                .LeftJoin<Institution>((p, d, c) => p.CompanyId == c.PomId)
                .Where((p, d, c) => statusList.Contains(p.StatusId.Value) && p.TypeId != CommonData.NoConstrutionProjectType)
                //.WhereIF(!userAuthForData.IsAdmin, (p, d, c) => userAuthForData.CompanyIds.Contains(p.ProjectDept))
                .Select((p, d, c) => new ProjectShiftProductionInfo
                {
                    DayReportId = d.Id,
                    CompanyId = c.PomId,
                    CompanyName = c.Name,
                    ProjectName = p.ShortName,
                    ShiftLeader = d.ShiftLeader,
                    ShiftPhone = d.ShiftLeaderPhone,
                    SiteManagementPersonNum = d.SiteManagementPersonNum,
                    SiteConstructionPersonNum = d.SiteConstructionPersonNum,
                    ConstructionDeviceNum = d.ConstructionDeviceNum,
                    FewLandWorkplace = d.FewLandWorkplace,
                    LandWorkplace = d.LandWorkplace,
                    SiteShipNum = d.SiteShipNum,
                    OnShipPersonNum = d.OnShipPersonNum,
                    HazardousConstructionNum = d.HazardousConstructionNum,
                    HazardousConstructionDescription = d.HazardousConstructionDescription
                }).ToListAsync();

            //已填报项目
            var completedList = list.Where(t => t.DayReportId != null).ToList();
            //获取相关公司
            var companyList = await _dbContext.Queryable<ProductionMonitoringOperationDayReport>().Where(t => t.IsDelete == 1 && t.Type == 1 && t.Name != "" && t.Name != "广航局总体(个)").ToListAsync();
            //未填报项目
            var unCompleteList = list.Where(t => t.DayReportId == null).ToList();
            foreach (var item in unCompleteList)
            {
                UnProjectShitInfo unProjectShitInfo = new UnProjectShitInfo();
                unProjectShitInfo.CompanyName = companyList.Where(t => t.ItemId == item.CompanyId).FirstOrDefault() == null ? item.CompanyName : companyList.Where(t => t.ItemId == item.CompanyId).FirstOrDefault().Name;
                unProjectShitInfo.ProjectName = item.ProjectName;
                responseDto.unProjectShitInfos.Add(unProjectShitInfo);
            }

            productionSumInfo.SumSiteManagementPersonNum = completedList.Sum(t => t.SiteManagementPersonNum);
            productionSumInfo.SumSiteConstructionPersonNum = completedList.Sum(t => t.SiteConstructionPersonNum);
            productionSumInfo.SumConstructionDeviceNum = completedList.Sum(t => t.ConstructionDeviceNum);
            productionSumInfo.SumFewLandWorkplace = completedList.Sum(t => t.FewLandWorkplace);
            productionSumInfo.SumLandWorkplace = completedList.Sum(t => t.LandWorkplace);
            productionSumInfo.SumSiteShipNum = completedList.Sum(t => t.SiteShipNum);
            productionSumInfo.SumOnShipPersonNum = completedList.Sum(t => t.OnShipPersonNum);
            productionSumInfo.SumHazardousConstructionNum = completedList.Sum(t => t.HazardousConstructionNum);

            responseDto.TimeValue = DateTime.Now.Date;
            responseDto.sumInfo = productionSumInfo;
            //排序
            completedList = completedList.OrderByDescending(x => x.HazardousConstructionNum).ToList();

            responseDto.projectShiftProductionInfos = completedList;
            responseAjaxResult.Data = responseDto;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }




        public async Task<ResponseAjaxResult<ProjectShiftProductionResponseDto>> GetProjectShiftProductionAsync(DateTime date)
        {
            ResponseAjaxResult<ProjectShiftProductionResponseDto> responseAjaxResult = new ResponseAjaxResult<ProjectShiftProductionResponseDto>();
            ProjectShiftProductionResponseDto responseDto = new ProjectShiftProductionResponseDto();
            ProjectShiftProductionSumInfo productionSumInfo = new ProjectShiftProductionSumInfo();
            //获取当前用户授权数据
            //var userAuthForData = await GetCurrentUserAuthForDataAsync();
            var time = date.AddDays(-1).ToDateDay();
            var statusList = CommonData.PConstruc.Split(',').Select(x => x.ToGuid()).ToList();
            //var list = await _dbProject.AsQueryable().LeftJoin<DayReport>((p, d) => p.Id == d.ProjectId && d.DateDay == time)
            //    .LeftJoin<Institution>((p, d, c) => p.CompanyId == c.PomId)
            //    .Where((p, d, c) => statusList.Contains((Guid)p.StatusId) && p.TypeId != CommonData.NoConstrutionProjectType)
            //    //.WhereIF(!userAuthForData.IsAdmin, (p, d, c) => userAuthForData.CompanyIds.Contains(p.ProjectDept))
            //    .Select((p, d, c) => new ProjectShiftProductionInfo
            //    {
            //        DayReportId = d.Id,
            //        CompanyId = c.PomId,
            //        CompanyName = c.Name,
            //        ProjectName = p.ShortName,
            //        ShiftLeader = d.ShiftLeader,
            //        ShiftPhone = d.ShiftLeaderPhone,
            //        SiteManagementPersonNum = d.SiteManagementPersonNum,
            //        SiteConstructionPersonNum = d.SiteConstructionPersonNum,
            //        ConstructionDeviceNum = d.ConstructionDeviceNum,
            //        FewLandWorkplace = d.FewLandWorkplace,
            //        LandWorkplace = d.LandWorkplace,
            //        SiteShipNum = d.SiteShipNum,
            //        OnShipPersonNum = d.OnShipPersonNum,
            //        HazardousConstructionNum = d.HazardousConstructionNum,
            //        HazardousConstructionDescription = d.HazardousConstructionDescription
            //    }).ToListAsync();
            var list = await _dbProject.AsQueryable()
                .LeftJoin<DayReport>((p, d) => p.Id == d.ProjectId && d.DateDay == time)
                .LeftJoin<Institution>((p, d, c) => p.CompanyId == c.PomId)
                .Where((p, d, c) => statusList.Contains(p.StatusId.Value) && p.TypeId != CommonData.NoConstrutionProjectType)
                //.WhereIF(!userAuthForData.IsAdmin, (p, d, c) => userAuthForData.CompanyIds.Contains(p.ProjectDept))
                .Select((p, d, c) => new ProjectShiftProductionInfo
                {
                    DayReportId = d.Id,
                    CompanyId = c.PomId,
                    CompanyName = c.Name,
                    ProjectName = p.ShortName,
                    ShiftLeader = d.ShiftLeader,
                    ShiftPhone = d.ShiftLeaderPhone,
                    SiteManagementPersonNum = d.SiteManagementPersonNum,
                    SiteConstructionPersonNum = d.SiteConstructionPersonNum,
                    ConstructionDeviceNum = d.ConstructionDeviceNum,
                    FewLandWorkplace = d.FewLandWorkplace,
                    LandWorkplace = d.LandWorkplace,
                    SiteShipNum = d.SiteShipNum,
                    OnShipPersonNum = d.OnShipPersonNum,
                    HazardousConstructionNum = d.HazardousConstructionNum,
                    HazardousConstructionDescription = d.HazardousConstructionDescription
                }).ToListAsync();

            //已填报项目
            var completedList = list.Where(t => t.DayReportId != null).ToList();
            //获取相关公司
            var companyList = await _dbContext.Queryable<ProductionMonitoringOperationDayReport>().Where(t => t.IsDelete == 1 && t.Type == 1 && t.Name != "" && t.Name != "广航局总体(个)").ToListAsync();
            //未填报项目
            var unCompleteList = list.Where(t => t.DayReportId == null).ToList();
            foreach (var item in unCompleteList)
            {
                UnProjectShitInfo unProjectShitInfo = new UnProjectShitInfo();
                unProjectShitInfo.CompanyName = companyList.Where(t => t.ItemId == item.CompanyId).FirstOrDefault() == null ? item.CompanyName : companyList.Where(t => t.ItemId == item.CompanyId).FirstOrDefault().Name;
                unProjectShitInfo.ProjectName = item.ProjectName;
                responseDto.unProjectShitInfos.Add(unProjectShitInfo);
            }

            productionSumInfo.SumSiteManagementPersonNum = completedList.Sum(t => t.SiteManagementPersonNum);
            productionSumInfo.SumSiteConstructionPersonNum = completedList.Sum(t => t.SiteConstructionPersonNum);
            productionSumInfo.SumConstructionDeviceNum = completedList.Sum(t => t.ConstructionDeviceNum);
            productionSumInfo.SumFewLandWorkplace = completedList.Sum(t => t.FewLandWorkplace);
            productionSumInfo.SumLandWorkplace = completedList.Sum(t => t.LandWorkplace);
            productionSumInfo.SumSiteShipNum = completedList.Sum(t => t.SiteShipNum);
            productionSumInfo.SumOnShipPersonNum = completedList.Sum(t => t.OnShipPersonNum);
            productionSumInfo.SumHazardousConstructionNum = completedList.Sum(t => t.HazardousConstructionNum);

            responseDto.TimeValue = date.Date;
            responseDto.sumInfo = productionSumInfo;
            //排序
            completedList = completedList.OrderByDescending(x => x.HazardousConstructionNum).ToList();

            responseDto.projectShiftProductionInfos = completedList;
            responseAjaxResult.Data = responseDto;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }





        /// <summary>
        /// 获取完工项目信息
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<ClosingProjectResponseDto>>> GetProjectFinishedAsync(DateTime time)
        {
            //项目状态Id
            List<Guid> ProjectStatus = new List<Guid>()
            {
                new Guid ("62986752-9b40-4d02-8887-b014a6ee7a9d"),
                new Guid ("28bc58dc-41ed-4135-8628-a4e6a571032b"),
                new Guid ("2a30d69d-ad3a-4a51-a1d7-7b363150437d"),
            };
            var responseAjaxResult = new ResponseAjaxResult<List<ClosingProjectResponseDto>>();
            var timeTear = await _dbContext.Queryable<Project>()
                .LeftJoin<Institution>((x, y) => x.CompanyId == y.PomId)
                .Where(x => ProjectStatus.Contains(x.StatusId.Value))
                .Select((x, y) => new ClosingProjectResponseDto { ProjectName = x.Name, CompanyName = y.Shortname, ClosingDateTime = x.CompletionTime })
                .ToListAsync();
            if (timeTear.Any())
            {
                responseAjaxResult.Data = timeTear;
                responseAjaxResult.Count = timeTear.Count;
                responseAjaxResult.Success();
            }
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取当月船舶进场天数
        /// </summary>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="Month"></param>
        /// <returns></returns>
        public int SearchDaysAsync(DateTime StartTime, DateTime? EndTime, int Month)
        {
            int Days = 0;
            ConvertHelper.TryParseFromDateMonth(Month, out DateTime datetime);
            DateTime endLastDayOfMonth = new DateTime(datetime.Year, datetime.Month, 25);

            DateTime startLastDayOfMonth = new DateTime(datetime.AddMonths(-1).Year, datetime.AddMonths(-1).Month, 26);
            TimeSpan? span = new TimeSpan();
            if (EndTime != null)
            {
                if (startLastDayOfMonth < StartTime && endLastDayOfMonth < EndTime)
                {
                    span = endLastDayOfMonth - StartTime;
                }
                if (startLastDayOfMonth < StartTime && endLastDayOfMonth > EndTime)
                {
                    span = EndTime.Value - StartTime;
                }
                if (startLastDayOfMonth > StartTime && endLastDayOfMonth > EndTime)
                {
                    span = EndTime.Value - startLastDayOfMonth;
                }
                if (startLastDayOfMonth > StartTime && endLastDayOfMonth < EndTime)
                {
                    span = endLastDayOfMonth - startLastDayOfMonth;
                }
                if (startLastDayOfMonth == StartTime && endLastDayOfMonth == EndTime)
                {
                    span = endLastDayOfMonth - startLastDayOfMonth;
                }
            }
            else
            {
                if (startLastDayOfMonth < StartTime)
                {
                    span = EndTime - StartTime;
                }
                if (startLastDayOfMonth > StartTime)
                {
                    span = EndTime - startLastDayOfMonth;
                }
            }
            if (span != null)
            {
                Days = span.Value.Days + 1;
            }
            if (Days < 0)
            {
                Days = 0;
            }
            return Days;
        }
        /// <summary>
        /// 获取本年天数
        /// </summary>
        /// <returns></returns>
        public int SearchDaysAsync(List<ShipMovement> shipMovement, int DateTimeMonth)
        {
            ConvertHelper.TryParseFromDateMonth(DateTimeMonth, out DateTime datetime);
            DateTime startDate = new DateTime(datetime.AddYears(-1).Year, 12, 26); // 开始日期
            DateTime targetDate = new DateTime(datetime.Year, datetime.Month, 25); // 结束日期
            int days = 0;
            TimeSpan? span = new TimeSpan();
            var shipMovementList = shipMovement.Where(x => (x.EnterTime >= startDate && x.EnterTime <= targetDate) || (x.QuitTime >= startDate && x.QuitTime <= targetDate) || x.QuitTime == null).ToList();
            foreach (var item in shipMovementList)
            {
                if (item.QuitTime != null)
                {
                    if (item.EnterTime < startDate && item.QuitTime < targetDate)
                    {
                        span = item.QuitTime - startDate;
                    }
                    if (item.EnterTime < startDate && item.QuitTime > targetDate)
                    {
                        span = targetDate - startDate;
                    }
                    if (item.EnterTime > startDate && item.QuitTime > targetDate)
                    {
                        span = targetDate - item.EnterTime;
                    }
                    if (item.EnterTime > startDate && item.QuitTime < targetDate)
                    {
                        span = item.QuitTime - item.EnterTime;
                    }
                    if (item.EnterTime == startDate && item.QuitTime == targetDate)
                    {
                        span = item.QuitTime - item.EnterTime;
                    }
                }
                else
                {
                    if (item.EnterTime < startDate)
                    {
                        span = targetDate - startDate;
                    }
                    if (item.EnterTime > startDate)
                    {
                        span = targetDate - item.EnterTime;
                    }
                }
                if (span != null && span.Value.Days > 0)
                {
                    days += span.Value.Days + 1;
                }
            }
            return days;
        }


        /// <summary>
        /// 获取已填报月份集合
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<ReportedMonthResponseDto>> SearchReportedMonthsAsync(ReportedMonthRequestDto model)
        {
            var maxMonthTime = GetDefaultReportDateMonthTime();
            var result = new ResponseAjaxResult<ReportedMonthResponseDto>();
            var reportedMonths = new List<string>();
            var dateMonths = await _dbMonthReport.AsQueryable().Where(t => t.IsDelete == 1 && t.ProjectId == model.ProjectId).OrderByDescending(t => t.DateMonth).Select(t => t.DateMonth).ToListAsync();
            dateMonths.ForEach(item =>
            {
                ConvertHelper.TryParseFromDateMonth(item, out DateTime monthTime);
                reportedMonths.Add(monthTime.ToString("yyyy-MM"));
            });
            return result.SuccessResult(new ReportedMonthResponseDto() { ReportedMonth = reportedMonths, MaxReportMonth = maxMonthTime.ToString("yyyy-MM") });
        }

        /// <summary>
        /// 获取默认允许填报的月份
        /// </summary>
        /// <returns></returns>
        private DateTime GetDefaultReportDateMonthTime()
        {
            DateTime time = DateTime.Now;
            return time.Day < 26 ? time.AddMonths(-1) : time;
        }

        /// <summary>
        /// 第一个 本年甲方确认产值
        /// 第二个 开累甲方确认产值
        /// 第三个 本年甲方付款金额
        /// 第四个 开累甲方付款金额
        /// </summary>
        /// <param name="projectId">项目ID</param>
        /// <param name="dateMonth"></param>
        /// <returns></returns>
        public async Task<Tuple<decimal, decimal, decimal, decimal>> GetProjectProductionValue(Guid projectId, int dateMonth)
        {
            var currentYearOffirmProductionValue = 0M;
            var totalYearKaileaOffirmProductionValue = 0M;
            var currenYearCollection = 0M;
            var totalYearCollection = 0M;
            try
            {
                var currentYear = DateTime.Now.Year;
                var projectMonthReportHistory = await _dbContext.Queryable<ProjectMonthReportHistory>()
                   .Where(x => x.IsDelete == 1 && x.ProjectId == projectId).FirstAsync();

                var currentTotalYearOffirmProductionValue = await _dbContext.Queryable<MonthReport>()
                    .Where(x => x.IsDelete == 1 && x.ProjectId == projectId && x.DateMonth <= dateMonth).ToListAsync();
                //本年甲方确认产值(当年)
                var initMonth = new DateTime(DateTime.Now.Year, 1, 1).ToDateMonth();
                currentYearOffirmProductionValue = currentTotalYearOffirmProductionValue.Where(x => x.DateMonth >= initMonth && x.DateMonth <= dateMonth)
                   //原来的// x.DateYear==currentYear)
                   .Sum(x => x.PartyAConfirmedProductionAmount);

                //开累甲方确认产值(历史数据+2023-7至12月的数据+2024年的数据）
                if (projectMonthReportHistory != null && currentTotalYearOffirmProductionValue.Any())
                {
                    totalYearKaileaOffirmProductionValue = projectMonthReportHistory.KaileiOwnerConfirmation.Value * 10000
                       + currentTotalYearOffirmProductionValue.Sum(x => x.PartyAConfirmedProductionAmount);
                }
                else
                {
                    totalYearKaileaOffirmProductionValue =
                            currentTotalYearOffirmProductionValue.Sum(x => x.PartyAConfirmedProductionAmount);
                }

                var currenTotalYearCollection = await _dbContext.Queryable<MonthReport>()
                    .Where(x => x.IsDelete == 1 && x.ProjectId == projectId && x.DateYear <= currentYear).ToListAsync();

                //本年甲方付款金额
                currenYearCollection = currenTotalYearCollection.Where(x => x.DateYear == currentYear).Sum(x => x.PartyAPayAmount);
                if (projectMonthReportHistory != null && currenTotalYearCollection.Any())
                {
                    //开累甲方付款金额
                    totalYearCollection = projectMonthReportHistory.KaileiProjectPayment.Value * 10000
                    + currenTotalYearCollection.Sum(x => x.PartyAPayAmount);
                }
                else
                {
                    //开累甲方付款金额
                    totalYearCollection = currenTotalYearCollection.Sum(x => x.PartyAPayAmount);
                }

            }
            catch (Exception ex)
            {


            }
            return Tuple.Create(currentYearOffirmProductionValue, totalYearKaileaOffirmProductionValue, currenYearCollection, totalYearCollection);
        }
    }
}
