using AutoMapper;
using GHMonitoringCenterApi.Application.Contracts.Dto;
using GHMonitoringCenterApi.Application.Contracts.Dto.CompanyProductionValueInfos;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project;
using GHMonitoringCenterApi.Application.Contracts.IService;
using GHMonitoringCenterApi.Application.Contracts.IService.OperationLog;
using GHMonitoringCenterApi.Application.Contracts.IService.Project;
using GHMonitoringCenterApi.Application.Contracts.IService.Push;
using GHMonitoringCenterApi.Domain.Enums;
using GHMonitoringCenterApi.Domain.IRepository;
using GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Domain.Shared;
using GHMonitoringCenterApi.Domain.Shared.Enums;
using GHMonitoringCenterApi.Domain.Shared.Util;
using Microsoft.AspNetCore.Mvc.Formatters.Internal;
using NPOI.SS.Formula.Functions;
using SqlSugar;
using UtilsSharp;

namespace GHMonitoringCenterApi.Application.Service.Projects
{
    /// <summary>
    /// 项目业务接口层的实现层
    /// </summary>
    public class BaseLinePlanProjectService : IBaseLinePlanProjectService
    {

        #region 依赖注入
        public IBaseRepository<ProjectWBS> baseProjectWBSRepository { get; set; }
        public IBaseRepository<Project> baseProjects { get; set; }
        public IBaseRepository<ProjectWbsHistoryMonth> baseProjectWbsHistory { get; set; }

        public ISqlSugarClient dbContext { get; set; }
        public IMapper mapper { get; set; }
        public IBaseRepository<Files> baseFilesRepository { get; set; }
        public IBaseRepository<ProjectOrg> baseProjectOrgRepository { get; set; }
        public IBaseRepository<ProjectLeader> baseProjectLeaderRepository { get; set; }

        public IBaseRepository<BaseLinePlanAncomparison> baseLinePlanAncomparisonRepository { get; set; }
        public IPushPomService _pushPomService { get; set; }
        public IBaseService baseService { get; set; }
        public ILogService logService { get; set; }
        public IEntityChangeService entityChangeService { get; set; }

        /// <summary>
        /// 全局对象
        /// </summary>
        private readonly GlobalObject _globalObject;
        /// <summary>
        /// 当前用户
        /// </summary>
        private CurrentUser _currentUser { get { return _globalObject.CurrentUser; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseProjectWbsHistory"></param>
        /// <param name="baseProjectWBSRepository"></param>
        /// <param name="dbContext"></param>
        /// <param name="mapper"></param>
        /// <param name="baseFilesRepository"></param>
        /// <param name="baseProjectOrgRepository"></param>
        /// <param name="baseProjectLeaderRepository"></param>
        /// <param name="pushPomService"></param>
        /// <param name="baseService"></param>
        /// <param name="logService"></param>
        /// <param name="entityChangeService"></param>
        /// <param name="globalObject"></param>
        /// <param name="baseProjects"></param>
        public BaseLinePlanProjectService(IBaseRepository<ProjectWbsHistoryMonth> baseProjectWbsHistory, IBaseRepository<ProjectWBS> baseProjectWBSRepository, ISqlSugarClient dbContext, IMapper mapper, IBaseRepository<Files> baseFilesRepository, IBaseRepository<ProjectOrg> baseProjectOrgRepository, IBaseRepository<ProjectLeader> baseProjectLeaderRepository, IPushPomService pushPomService, IBaseService baseService, ILogService logService, IEntityChangeService entityChangeService, GlobalObject globalObject, IBaseRepository<Project> baseProjects, IBaseRepository<BaseLinePlanAncomparison> baseLinePlanAncomparisonRepository)
        {
            this.baseProjectWBSRepository = baseProjectWBSRepository;
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.baseFilesRepository = baseFilesRepository;
            this.baseProjectOrgRepository = baseProjectOrgRepository;
            this.baseProjectLeaderRepository = baseProjectLeaderRepository;
            this._pushPomService = pushPomService;
            this.baseService = baseService;
            this.logService = logService;
            this.entityChangeService = entityChangeService;
            this._globalObject = globalObject;
            this.baseProjectWbsHistory = baseProjectWbsHistory;
            this.baseProjects = baseProjects;
            this.baseLinePlanAncomparisonRepository = baseLinePlanAncomparisonRepository;
        }
        #endregion


        #region 基准计划
        /// <summary>
        /// 导入数据
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> ExcelProjectAnnualProductionAsync(ProjectAnnualProductionDto requestBody)
        {
            ResponseAjaxResult<bool> rt = new();
            List<ProjectAnnualPlanProduction> add = new();

            var rr = requestBody.ExcelImport.ToList();

            if (rr != null && rr.Any())
            {
                var tables = await dbContext.Queryable<ProjectAnnualPlanProduction>()
                    .Where(t => t.IsDelete == 1 && t.Year == DateTime.Now.Year)
                    .ToListAsync();
                tables.ForEach(x => x.IsDelete = 0);
                await dbContext.Updateable(tables).UpdateColumns(x => x.IsDelete).ExecuteCommandAsync();

                var projects = await dbContext.Queryable<Project>()
                    .Select(x => new { x.Id, x.CompanyId, x.Name })
                    .ToListAsync();

                var unmatchedNames = new List<string>();

                foreach (var item in rr)
                {
                    var name = item.ProjectName.Length > 10 ? item.ProjectName.Substring(0, 6) : item.ProjectName;
                    var pp = projects.FirstOrDefault(x => x.Name.Contains(name));

                    if (pp == null)
                    {
                        unmatchedNames.Add(item.ProjectName);
                        continue;
                    }

                    add.Add(new ProjectAnnualPlanProduction
                    {
                        JanuaryProductionValue = item.JanuaryProductionValue,
                        FebruaryProductionValue = item.FebruaryProductionValue,
                        MarchProductionValue = item.MarchProductionValue,
                        AprilProductionValue = item.AprilProductionValue,
                        MayProductionValue = item.MayProductionValue,
                        JuneProductionValue = item.JuneProductionValue,
                        JulyProductionValue = item.JulyProductionValue,
                        AugustProductionValue = item.AugustProductionValue,
                        SeptemberProductionValue = item.SeptemberProductionValue,
                        OctoberProductionValue = item.OctoberProductionValue,
                        NovemberProductionValue = item.NovemberProductionValue,
                        DecemberProductionValue = item.DecemberProductionValue,
                        ProjectId = pp.Id,
                        CompanyId = pp.CompanyId.Value,
                        Year = DateTime.Now.Year,
                        Id = Guid.NewGuid()
                    });
                }

                if (add.Any())
                {
                    await dbContext.Insertable(add).ExecuteCommandAsync();
                }

                rt.Data = true;
                rt.Success();

            }
            else
            {
                rt.Data = false;
            }

            return rt;
        }

        /// <summary>
        /// 公司初始化
        /// </summary>
        /// <returns></returns>
        private Dictionary<Guid, (string Name, int Sequence)> KeyValuePairs()
        {
            return new Dictionary<Guid, (string, int)>
            {
                { "c0d51e81-03dd-4ef8-bd83-6eb1355879e1".ToGuid(), ("五公司", 5) },
                { "bd840460-1e3a-45c8-abed-6e66903eb465".ToGuid(), ("直营项目", 9) },
                { "5a8f39de-8515-456b-8620-c0d01c012e04".ToGuid(), ("四公司", 4) },
                { "3c5b138b-601a-442a-9519-2508ec1c1eb2".ToGuid(), ("疏浚公司", 1) },
                { "65052a94-6ea7-44ba-96b4-cf648de0d28a".ToGuid(), ("福建公司", 6) },
                { "a8db9bb0-4667-4320-b03d-b0b7f8728b61".ToGuid(), ("交建公司", 2) },
                { "11c9c978-9ef3-411d-ba70-0d0eed93e048".ToGuid(), ("三公司", 3) },
                { "01ff7a0e-e827-4b46-9032-0a540ce1fba3".ToGuid(), ("菲律宾公司", 7) },
                { "9930ca6d-7131-4a2a-8b75-ced9c0579b8c".ToGuid(), ("广航水利", 8) }
            };
        }
        #endregion

        /// <summary>
        /// 获取基准计划详情
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<SearchBaseLinePlanProjectAnnualProduction>> SearchBaseLinePlanProjectAnnualProductionDetailsAsync(SearchBaseLinePlanProjectAnnualProductionRequest requestBody)
        {
            ResponseAjaxResult<SearchBaseLinePlanProjectAnnualProduction> rt = new();
            SearchBaseLinePlanProjectAnnualProduction rr = new();
            List<SearchBaseLinePlanProjectAnnualProductionDto> rsList = new();

            if (requestBody.ProjectId == Guid.Empty || string.IsNullOrWhiteSpace(requestBody.ProjectId.ToString()))
            {
                return rt.SuccessResult(rr);
            }
            //初始化
            List<BaseLinePlanProjectAnnualPlanProduction> pPlanProduction = new();

            var annualProductionShips = await dbContext.Queryable<BaseLinePlanAnnualProductionShips>()
                .Where(t => t.IsDelete == 1).ToListAsync();

            var mainIds = annualProductionShips.Select(x => x.ProjectAnnualProductionId).ToList();

            pPlanProduction = await dbContext.Queryable<BaseLinePlanProjectAnnualPlanProduction>()
                .WhereIF(!string.IsNullOrWhiteSpace(requestBody.ProjectId.ToString()), t => t.ProjectId == requestBody.ProjectId)
                .Where(t => t.IsDelete == 1 && mainIds.Contains(t.Id))
                .ToListAsync();

            var pIds = pPlanProduction.Select(x => x.ProjectId).ToList();

            var baseplanproject = await dbContext.Queryable<BaseLinePlanProject>()
               .Where(t => t.IsDelete == 1 && t.ProjectId == requestBody.ProjectId).FirstAsync();

            //项目基本信息
            var projects = await dbContext.Queryable<Project>().Where(t => t.IsDelete == 1 && pIds.Contains(t.Id)).Select(x => new { x.Id, x.CurrencyId, x.ShortName, x.ECAmount, x.ContractStipulationEndDate }).ToListAsync();

            var ships = await dbContext.Queryable<OwnerShip>().Where(t => t.IsDelete == 1).ToListAsync();

            //项目开累产值   24年调整最终版本+至今为止月报数据
            var monthReport = await dbContext.Queryable<MonthReport>().Where(t => t.IsDelete == 1).ToListAsync();
            var monthReport24 = await dbContext.Queryable<MonthReportDetailHistory>().ToListAsync();

            if (!string.IsNullOrWhiteSpace(requestBody.ProjectId.ToString()))
            {
                decimal completeAmount = 0M;//完成产值
                decimal accumulatedOutputValue = 0M;//开累产值
                var pp = await dbContext.Queryable<Project>().FirstAsync(x => x.Id == requestBody.ProjectId && x.IsDelete == 1);
                if (pp != null)
                {
                    var dateMonth = DateTime.Now.ToDateMonth();
                    var yearMonth = Convert.ToInt32(DateTime.Now.Year + "01");
                    if (pp.CurrencyId == "2a0e99b4-f989-4967-b5f1-5519091d4280".ToGuid())//国内
                    {

                        accumulatedOutputValue = monthReport24.Where(x => x.ProjectId == pp.Id).Sum(x => Convert.ToDecimal(x.ActualCompAmount));//开累
                        //202306历史产值追加
                        accumulatedOutputValue += monthReport.Where(x => x.ProjectId == pp.Id && x.DateMonth == 202306).Sum(x => Convert.ToDecimal(x.ActualCompAmount));
                    }
                    else
                    {
                        //获取汇率
                        var currencyConvert = await dbContext.Queryable<CurrencyConverter>().Where(t => t.IsDelete == 1 && t.CurrencyId == pp.CurrencyId.ToString()).OrderByDescending(x => x.Year).FirstAsync();
                        pp.ECAmount = pp.ECAmount * currencyConvert.ExchangeRate;
                        accumulatedOutputValue = monthReport24.Where(x => x.ProjectId == pp.Id).Sum(x => Convert.ToDecimal(x.RMBHValue));//开累
                        //202306历史产值追加
                        accumulatedOutputValue += monthReport.Where(x => x.ProjectId == pp.Id && x.DateMonth == 202306).Sum(x => Convert.ToDecimal(x.RMBHValue));
                    }
                    //截止到当月的完成产值+历史调整开累产值=开累产值
                    accumulatedOutputValue += monthReport.Where(x => x.ProjectId == pp.Id && x.DateMonth > 202412 && x.DateMonth <= dateMonth).Sum(x => x.CompleteProductionAmount);
                    //2024年调整的开累数+截止到当年一月的开累数（年初开累产值）
                    completeAmount = accumulatedOutputValue + monthReport.Where(x => x.ProjectId == pp.Id && x.DateMonth > 202412 && x.DateMonth < yearMonth).Sum(x => x.CompleteProductionAmount);

                    rr.ProjectName = pp.ShortName;
                    rr.EffectiveAmount = Convert.ToDecimal(pp.ECAmount);
                    rr.ContractStipulationEndDate = pp.ContractStipulationEndDate?.ToString("yyyy-MM-dd");
                    rr.RemainingAmount = Convert.ToDecimal(pp.ECAmount) - completeAmount;
                    rr.AccumulatedOutputValue = accumulatedOutputValue;
                    if (baseplanproject != null)
                    {
                        //rr.PlanVersion = baseplanproject.PlanVersion;
                        rr.PlanType = baseplanproject.PlanType;
                        rr.StartStatus = baseplanproject.StartStatus;
                    }

                }
            }

            foreach (var item in pPlanProduction)
            {
                SearchBaseLinePlanProjectAnnualProductionDto ppChild = new();
                ppChild.Id = item.Id;
                ppChild.JanuaryProductionQuantity = item.JanuaryProductionQuantity;
                ppChild.JanuaryProductionValue = item.JanuaryProductionValue;
                ppChild.FebruaryProductionQuantity = item.FebruaryProductionQuantity;
                ppChild.FebruaryProductionValue = item.FebruaryProductionValue;
                ppChild.MarchProductionQuantity = item.MarchProductionQuantity;
                ppChild.MarchProductionValue = item.MarchProductionValue;
                ppChild.AprilProductionQuantity = item.AprilProductionQuantity;
                ppChild.AprilProductionValue = item.AprilProductionValue;
                ppChild.MayProductionQuantity = item.MayProductionQuantity;
                ppChild.MayProductionValue = item.MayProductionValue;
                ppChild.JuneProductionQuantity = item.JuneProductionQuantity;
                ppChild.JuneProductionValue = item.JuneProductionValue;
                ppChild.JulyProductionQuantity = item.JulyProductionQuantity;
                ppChild.JulyProductionValue = item.JulyProductionValue;
                ppChild.AugustProductionValue = item.AugustProductionValue;
                ppChild.AugustProductionQuantity = item.AugustProductionQuantity;
                ppChild.SeptemberProductionQuantity = item.SeptemberProductionQuantity;
                ppChild.SeptemberProductionValue = item.SeptemberProductionValue;
                ppChild.OctoberProductionQuantity = item.OctoberProductionQuantity;
                ppChild.OctoberProductionValue = item.OctoberProductionValue;
                ppChild.NovemberProductionQuantity = item.NovemberProductionQuantity;
                ppChild.NovemberProductionValue = item.NovemberProductionValue;
                ppChild.DecemberProductionQuantity = item.DecemberProductionQuantity;
                ppChild.DecemberProductionValue = item.DecemberProductionValue;
                ppChild.AnnualProductionShips = annualProductionShips
                    .Where(x => x.ProjectAnnualProductionId == item.Id)
                    .Select(x => new BaseLinePlanAnnualPlanProductionShips { ShipId = x.ShipId, ShipType = x.ShipType, ShipName = x.ShipName })
                    .ToList();
                ppChild.ProjectId = item.ProjectId;
                ppChild.CompanyId = item.CompanyId;
                rsList.Add(ppChild);
            }

            rt.Count = rsList.Count;
            rr.SearchProjectAnnualProductionDto = rsList;
            return rt.SuccessResult(rr);
        }

        /// <summary>
        /// 获取基准计划列表
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<SearchBaseLinePlanProjectAnnualProductionDto>>> SearchBaseLinePlanProjectAnnualProductionAsync(SearchBaseLinePlanProjectAnnualProductionRequest requestBody)
        {
            ResponseAjaxResult<List<SearchBaseLinePlanProjectAnnualProductionDto>> rt = new();
            List<SearchBaseLinePlanProjectAnnualProductionDto> rr = new();


            if (requestBody.ProjectId == Guid.Empty || string.IsNullOrWhiteSpace(requestBody.ProjectId.ToString()))
            {
                return rt.SuccessResult(rr);
            }
            //初始化
            List<BaseLinePlanProjectAnnualPlanProduction> pPlanProduction = new();

            var baseplanproject = await dbContext.Queryable<BaseLinePlanProject>()
                .Where(t => t.IsDelete == 1).ToListAsync();

            var annualProductionShips = await dbContext.Queryable<BaseLinePlanAnnualProductionShips>()
                .Where(t => t.IsDelete == 1).ToListAsync();

            var mainIds = annualProductionShips.Select(x => x.ProjectAnnualProductionId).ToList();

            pPlanProduction = await dbContext.Queryable<BaseLinePlanProjectAnnualPlanProduction>()
                .WhereIF(!string.IsNullOrWhiteSpace(requestBody.ProjectId.ToString()), t => t.ProjectId == requestBody.ProjectId)
                .Where(t => t.IsDelete == 1 && mainIds.Contains(t.Id))
                .ToListAsync();

            foreach (var item in pPlanProduction)
            {
                SearchBaseLinePlanProjectAnnualProductionDto ppChild = new();
                ppChild.Id = item.Id;
                ppChild.JanuaryProductionQuantity = item.JanuaryProductionQuantity;
                ppChild.JanuaryProductionValue = item.JanuaryProductionValue;
                ppChild.FebruaryProductionQuantity = item.FebruaryProductionQuantity;
                ppChild.FebruaryProductionValue = item.FebruaryProductionValue;
                ppChild.MarchProductionQuantity = item.MarchProductionQuantity;
                ppChild.MarchProductionValue = item.MarchProductionValue;
                ppChild.AprilProductionQuantity = item.AprilProductionQuantity;
                ppChild.AprilProductionValue = item.AprilProductionValue;
                ppChild.MayProductionQuantity = item.MayProductionQuantity;
                ppChild.MayProductionValue = item.MayProductionValue;
                ppChild.JuneProductionQuantity = item.JuneProductionQuantity;
                ppChild.JuneProductionValue = item.JuneProductionValue;
                ppChild.JulyProductionQuantity = item.JulyProductionQuantity;
                ppChild.JulyProductionValue = item.JulyProductionValue;
                ppChild.AugustProductionValue = item.AugustProductionValue;
                ppChild.AugustProductionQuantity = item.AugustProductionQuantity;
                ppChild.SeptemberProductionQuantity = item.SeptemberProductionQuantity;
                ppChild.SeptemberProductionValue = item.SeptemberProductionValue;
                ppChild.OctoberProductionQuantity = item.OctoberProductionQuantity;
                ppChild.OctoberProductionValue = item.OctoberProductionValue;
                ppChild.NovemberProductionQuantity = item.NovemberProductionQuantity;
                ppChild.NovemberProductionValue = item.NovemberProductionValue;
                ppChild.DecemberProductionQuantity = item.DecemberProductionQuantity;
                ppChild.DecemberProductionValue = item.DecemberProductionValue;
                ppChild.AnnualProductionShips = annualProductionShips
                    .Where(x => x.ProjectAnnualProductionId == item.Id)
                    .Select(x => new BaseLinePlanAnnualPlanProductionShips { ShipId = x.ShipId, ShipType = x.ShipType, ShipName = x.ShipName })
                    .ToList();

                ppChild.ProjectId = item.ProjectId;
                ppChild.CompanyId = item.CompanyId;
                rr.Add(ppChild);
            }

            var projectList = await dbContext.Queryable<Project>().Where(x => x.IsDelete == 1).Select(x => new Project { Id = x.Id, Name = x.Name, CompanyId = x.CompanyId }).ToListAsync();
            var porjectName = projectList.Where(p => p.Id == requestBody.ProjectId).FirstOrDefault();
            rt.Count = rr.Count;
            return rt.SuccessResult(rr);
        }

        /// <summary>
        /// 保存基准计划
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> SaveBaseLinePlanProjectAnnualProductionAsync(List<SearchBaseLinePlanProjectAnnualProductionDto>? requestBody)
        {
            ResponseAjaxResult<bool> rt = new();

            List<BaseLinePlanProjectAnnualPlanProduction> addTables = new();
            List<BaseLinePlanProjectAnnualPlanProduction> upTables = new();
            List<BaseLinePlanAnnualProductionShips> addShipTables = new();
            List<Guid> Ids = new();

            if (requestBody != null && requestBody.Any())
            {
                var tables = await dbContext.Queryable<BaseLinePlanProjectAnnualPlanProduction>().Where(t => t.IsDelete == 1 && requestBody.Select(x => x.Id).Contains(t.Id)).ToListAsync();
                foreach (var item in requestBody)
                {
                    BaseLinePlanProject baseLinePlanProject = null;
                    var baselineplan = item.baseLinePlanproject;
                    var baselinefirbaseLinePlanProject = await dbContext.Queryable<BaseLinePlanProject>().Where(p => p.ProjectId == baselineplan.ProjectId).FirstAsync();
                    if (baselinefirbaseLinePlanProject != null)
                    {
                        baselinefirbaseLinePlanProject.PlanType = baselineplan.PlanType;
                        baselinefirbaseLinePlanProject.StartStatus = baselineplan.StartStatus;
                        await dbContext.Updateable(baselinefirbaseLinePlanProject).ExecuteCommandAsync();
                    }
                    else
                    {
                        baseLinePlanProject = new BaseLinePlanProject();
                        baseLinePlanProject.Id = GuidUtil.Next();
                        baseLinePlanProject.CompanyId = item.CompanyId;
                        baseLinePlanProject.Year = DateTime.Now.Year;
                        baseLinePlanProject.PlanType = baselineplan.PlanType;
                        baseLinePlanProject.StartStatus = baselineplan.StartStatus;
                        baseLinePlanProject.ProjectId = baselineplan.ProjectId;
                        await dbContext.Insertable(baseLinePlanProject).ExecuteCommandAsync();
                    }

                    if (!string.IsNullOrWhiteSpace(item.Id.ToString()) && item.Id != Guid.Empty)
                    {
                        var first = tables.FirstOrDefault(x => x.Id == item.Id);
                        if (first != null)
                        {
                            first.JanuaryProductionQuantity = item.JanuaryProductionQuantity;
                            first.JanuaryProductionValue = item.JanuaryProductionValue;
                            first.FebruaryProductionQuantity = item.FebruaryProductionQuantity;
                            first.FebruaryProductionValue = item.FebruaryProductionValue;
                            first.MarchProductionQuantity = item.MarchProductionQuantity;
                            first.MarchProductionValue = item.MarchProductionValue;
                            first.AprilProductionQuantity = item.AprilProductionQuantity;
                            first.AprilProductionValue = item.AprilProductionValue;
                            first.MayProductionQuantity = item.MayProductionQuantity;
                            first.MayProductionValue = item.MayProductionValue;
                            first.JuneProductionQuantity = item.JuneProductionQuantity;
                            first.JuneProductionValue = item.JuneProductionValue;
                            first.JulyProductionQuantity = item.JulyProductionQuantity;
                            first.JulyProductionValue = item.JulyProductionValue;
                            first.AugustProductionValue = item.AugustProductionValue;
                            first.AugustProductionQuantity = item.AugustProductionQuantity;
                            first.SeptemberProductionQuantity = item.SeptemberProductionQuantity;
                            first.SeptemberProductionValue = item.SeptemberProductionValue;
                            first.OctoberProductionQuantity = item.OctoberProductionQuantity;
                            first.OctoberProductionValue = item.OctoberProductionValue;
                            first.NovemberProductionQuantity = item.NovemberProductionQuantity;
                            first.NovemberProductionValue = item.NovemberProductionValue;
                            first.DecemberProductionQuantity = item.DecemberProductionQuantity;
                            first.DecemberProductionValue = item.DecemberProductionValue;
                            upTables.Add(first);
                            Ids.Add(first.Id);

                            foreach (var ship in item.AnnualProductionShips)
                            {
                                addShipTables.Add(new BaseLinePlanAnnualProductionShips
                                {
                                    Id = GuidUtil.Next(),
                                    ShipType = ship.ShipType,
                                    ShipId = ship.ShipId,
                                    ShipName = ship.ShipName,
                                    ProjectAnnualProductionId = first.Id
                                });
                            }
                        }
                    }
                    else
                    {
                        var id = GuidUtil.Next();
                        addTables.Add(new BaseLinePlanProjectAnnualPlanProduction
                        {
                            JanuaryProductionQuantity = item.JanuaryProductionQuantity,
                            JanuaryProductionValue = item.JanuaryProductionValue,
                            FebruaryProductionQuantity = item.FebruaryProductionQuantity,
                            FebruaryProductionValue = item.FebruaryProductionValue,
                            MarchProductionQuantity = item.MarchProductionQuantity,
                            MarchProductionValue = item.MarchProductionValue,
                            AprilProductionQuantity = item.AprilProductionQuantity,
                            AprilProductionValue = item.AprilProductionValue,
                            MayProductionQuantity = item.MayProductionQuantity,
                            MayProductionValue = item.MayProductionValue,
                            JuneProductionQuantity = item.JuneProductionQuantity,
                            JuneProductionValue = item.JuneProductionValue,
                            JulyProductionQuantity = item.JulyProductionQuantity,
                            JulyProductionValue = item.JulyProductionValue,
                            AugustProductionValue = item.AugustProductionValue,
                            AugustProductionQuantity = item.AugustProductionQuantity,
                            SeptemberProductionQuantity = item.SeptemberProductionQuantity,
                            SeptemberProductionValue = item.SeptemberProductionValue,
                            OctoberProductionQuantity = item.OctoberProductionQuantity,
                            OctoberProductionValue = item.OctoberProductionValue,
                            NovemberProductionQuantity = item.NovemberProductionQuantity,
                            NovemberProductionValue = item.NovemberProductionValue,
                            DecemberProductionQuantity = item.DecemberProductionQuantity,
                            DecemberProductionValue = item.DecemberProductionValue,
                            ProjectId = item.ProjectId,
                            CompanyId = item.CompanyId,
                            Year = DateTime.Now.Year,
                            Id = id
                        });

                        foreach (var ship in item.AnnualProductionShips)
                        {
                            addShipTables.Add(new BaseLinePlanAnnualProductionShips
                            {
                                Id = GuidUtil.Next(),
                                ShipType = ship.ShipType,
                                ShipId = ship.ShipId,
                                ShipName = ship.ShipName,
                                ProjectAnnualProductionId = id
                            });
                        }
                    }
                }
                if (Ids.Any())
                {
                    var delete = await dbContext.Queryable<BaseLinePlanAnnualProductionShips>().Where(x => Ids.Contains(x.ProjectAnnualProductionId)).ToListAsync();
                    await dbContext.Deleteable(delete).ExecuteCommandAsync();
                }
                if (upTables.Any())
                {
                    await dbContext.Updateable(upTables).ExecuteCommandAsync();
                }
                if (addTables.Any())
                {
                    await dbContext.Insertable(addTables).ExecuteCommandAsync();
                }
                if (addShipTables.Any())
                {
                    await dbContext.Insertable(addShipTables).ExecuteCommandAsync();
                }
                rt.SuccessResult(true, "保存成功");
            }
            else rt.FailResult(HttpStatusCode.SaveFail, "保存失败");

            return rt;
        }

        /// <summary>
        /// 基础信息
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<BaseLinePlanAnnualProduction>> BaseLinePlanAnnualProductionAsync()
        {
            ResponseAjaxResult<BaseLinePlanAnnualProduction> rt = new();
            BaseLinePlanAnnualProduction rr = new();

            var bProjects = await baseService.SearchProjectInformationAsync(new BaseRequestDto());
            var ppIds = bProjects.Data?.Select(x => x.Id).ToList();

            List<BaseLinePlanProjectInfosForAnnualProduction> projectInfosForAnnualProductions = new();
            List<BaseLinePlanShipsForAnnualProduction> ownShipsForAnnualProductions = new();

            projectInfosForAnnualProductions = await dbContext.Queryable<Project>().Where(t => t.IsDelete == 1 && ppIds.Contains(t.Id))
                .Select(x => new BaseLinePlanProjectInfosForAnnualProduction
                {
                    CompanyId = x.CompanyId,
                    ProjectId = x.Id,
                    ProjectName = x.Name
                }).ToListAsync();

            var owns = await dbContext.Queryable<OwnerShip>().Where(x => x.IsDelete == 1)
                .Select(x => new BaseLinePlanShipsForAnnualProduction
                {
                    Id = x.PomId,
                    Name = x.Name,
                    SubOrOwn = 1
                }).ToListAsync();
            var subs = await dbContext.Queryable<SubShip>().Where(x => x.IsDelete == 1)
                .Select(x => new BaseLinePlanShipsForAnnualProduction
                {
                    Id = x.PomId,
                    Name = x.Name,
                    SubOrOwn = 2
                }).ToListAsync();
            ownShipsForAnnualProductions.AddRange(owns);
            ownShipsForAnnualProductions.AddRange(subs);

            rr.ProjectInfosForAnnualProductions = projectInfosForAnnualProductions;
            rr.OwnShipsForAnnualProductions = ownShipsForAnnualProductions;
            return rt.SuccessResult(rr);
        }



        /// <summary>
        /// 基准计划项目界面 分子公司界面
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<SearchSubsidiaryCompaniesProjectProductionDto>>> SearchSubsidiaryCompaniesProjectProductionAsync(SearchBaseLinePlanProjectAnnualProductionRequest requestBody)
        {
            List<SearchSubsidiaryCompaniesProjectProductionDto> resultlist = new List<SearchSubsidiaryCompaniesProjectProductionDto>();
            ResponseAjaxResult<List<SearchSubsidiaryCompaniesProjectProductionDto>> rt = new();
            var pomids = await dbContext.Queryable<Institution>().Where(p => p.Grule.Contains(_currentUser.CurrentLoginInstitutionOid)).ToListAsync();
            List<Guid?> guids = pomids.Select(p => p.PomId).ToList();
            var projects = await dbContext.Queryable<Project>().Where(p => p.IsDelete == 1 && guids.Contains(p.ProjectDept)).Select(p => p.Id).ToListAsync();
            var baseplanproject = await dbContext.Queryable<BaseLinePlanProject>()
             .Where(t => t.IsDelete == 1).ToListAsync();

            //var list= dbContext.Queryable<BaseLinePlanAncomparison>().LeftJoin<Project>((b,p)=>b.Code==p.MasterCode).

            var annualProductionShips = await dbContext.Queryable<BaseLinePlanAnnualProductionShips>()
                .Where(t => t.IsDelete == 1).ToListAsync();
            var mainIds = annualProductionShips.Select(x => x.ProjectAnnualProductionId).ToList();
            var projectList = await dbContext.Queryable<Project>().Where(x => x.IsDelete == 1).Select(x => new Project { Id = x.Id, Name = x.Name, CompanyId = x.CompanyId }).ToListAsync();
            var pPlanProduction = await dbContext.Queryable<BaseLinePlanProjectAnnualPlanProduction>()
                 .Where(t => t.IsDelete == 1 && mainIds.Contains(t.Id) && projects.Contains(t.ProjectId.Value))
                 .GroupBy(p => p.ProjectId)
                 .Select(it => new SearchSubsidiaryCompaniesProjectProductionDto()
                 {
                     ProjectId = it.ProjectId,
                     JanuaryProductionValue = SqlFunc.AggregateSum(it.JanuaryProductionValue),
                     FebruaryProductionValue = SqlFunc.AggregateSum(it.FebruaryProductionValue),
                     MarchProductionValue = SqlFunc.AggregateSum(it.MarchProductionValue),
                     AprilProductionValue = SqlFunc.AggregateSum(it.AprilProductionValue),
                     MayProductionValue = SqlFunc.AggregateSum(it.MayProductionValue),
                     JuneProductionValue = SqlFunc.AggregateSum(it.JuneProductionValue),
                     JulyProductionValue = SqlFunc.AggregateSum(it.JulyProductionValue),
                     AugustProductionValue = SqlFunc.AggregateSum(it.AugustProductionValue),
                     SeptemberProductionValue = SqlFunc.AggregateSum(it.SeptemberProductionValue),
                     OctoberProductionValue = SqlFunc.AggregateSum(it.OctoberProductionValue),
                     NovemberProductionValue = SqlFunc.AggregateSum(it.NovemberProductionValue),
                     DecemberProductionValue = SqlFunc.AggregateSum(it.DecemberProductionValue)
                 })
                 .ToListAsync();

            foreach (var item in pPlanProduction)
            {
                item.ProjectName = projectList.Where(p => p.Id == item.ProjectId).FirstOrDefault()?.Name;
                var plantype = baseplanproject.Where(p => p.ProjectId == item.ProjectId).FirstOrDefault()?.PlanType;
                if (plantype == "新建")
                {
                    item.NewPlanName = item.ProjectName + "-新建";
                }
                else
                {
                    item.BasePlanName = item.ProjectName + "-基准";
                }
            }
            rt.Count = pPlanProduction.Count;
            return rt.SuccessResult(pPlanProduction);
        }

        public async Task<ResponseAjaxResult<SearchBaseLinePlanProjectAnnualProduction>> SearchBaseLinePlanProjectAnnualProductionSumAsync(SearchBaseLinePlanProjectAnnualProductionRequest requestBody)
        {

            ResponseAjaxResult<SearchBaseLinePlanProjectAnnualProduction> rt = new();
            SearchBaseLinePlanProjectAnnualProduction rr = new();
            List<BaseLinePlanProjectAnnualPlanProduction> pPlanProduction = new();

            var pomids = await dbContext.Queryable<Institution>().Where(p => p.Grule.Contains(_currentUser.CurrentLoginInstitutionOid)).ToListAsync();
            List<Guid?> guids = pomids.Select(p => p.PomId).ToList();
            var projects = await dbContext.Queryable<Project>().Where(p => p.IsDelete == 1 && guids.Contains(p.ProjectDept)).Select(p => p.Id).ToListAsync();
            var baseplanproject = await dbContext.Queryable<BaseLinePlanProject>()
             .Where(t => t.IsDelete == 1).ToListAsync();

            var annualProductionShips = await dbContext.Queryable<BaseLinePlanAnnualProductionShips>()
                .Where(t => t.IsDelete == 1).ToListAsync();

            var mainIds = annualProductionShips.Select(x => x.ProjectAnnualProductionId).ToList();

            var sums = await dbContext.Queryable<BaseLinePlanProjectAnnualPlanProduction>()
                .Where(t => t.IsDelete == 1 && mainIds.Contains(t.Id) && projects.Contains(t.ProjectId.Value)).Select(p => new BaseLinePlanProjectAnnualPlanProduction
                {
                    JanuaryProductionValue = SqlFunc.AggregateSum(p.JanuaryProductionValue),
                    FebruaryProductionValue = SqlFunc.AggregateSum(p.FebruaryProductionValue),
                    MarchProductionValue = SqlFunc.AggregateSum(p.MarchProductionValue),
                    AprilProductionValue = SqlFunc.AggregateSum(p.AprilProductionValue),
                    MayProductionValue = SqlFunc.AggregateSum(p.MayProductionValue),
                    JuneProductionValue = SqlFunc.AggregateSum(p.JuneProductionValue),
                    JulyProductionValue = SqlFunc.AggregateSum(p.JulyProductionValue),
                    AugustProductionValue = SqlFunc.AggregateSum(p.AugustProductionValue),
                    SeptemberProductionValue = SqlFunc.AggregateSum(p.SeptemberProductionValue),
                    OctoberProductionValue = SqlFunc.AggregateSum(p.OctoberProductionValue),
                    NovemberProductionValue = SqlFunc.AggregateSum(p.NovemberProductionValue),
                    DecemberProductionValue = SqlFunc.AggregateSum(p.DecemberProductionValue)
                }).FirstAsync();

            var sumall = sums;

            var sum = 0M;
            if (sumall != null)
            {
                sum = sumall.JanuaryProductionValue + sumall.FebruaryProductionValue +
                    sumall.MarchProductionValue + sumall.AprilProductionValue +
                    sumall.MayProductionValue + sumall.JuneProductionValue +
                    sumall.JulyProductionValue + sumall.AugustProductionValue +
                    sumall.SeptemberProductionValue + sumall.OctoberProductionValue +
                    sumall.NovemberProductionValue + sumall.DecemberProductionValue;
            }

            pPlanProduction = await dbContext.Queryable<BaseLinePlanProjectAnnualPlanProduction>().Where(it => it.IsDelete == 1).ToListAsync();

            var pIds = pPlanProduction.Select(x => x.ProjectId).ToList();

            //var baseplanproject = await dbContext.Queryable<BaseLinePlanProject>()
            //   .Where(t => t.IsDelete == 1 && t.ProjectId == requestBody.ProjectId).FirstAsync();

            ////项目基本信息
            //var projects = await dbContext.Queryable<Project>().Where(t => t.IsDelete == 1 && pIds.Contains(t.Id)).Select(x => new { x.Id, x.CurrencyId, x.ShortName, x.ECAmount, x.ContractStipulationEndDate }).ToListAsync();

            var ships = await dbContext.Queryable<OwnerShip>().Where(t => t.IsDelete == 1).ToListAsync();

            //项目开累产值   24年调整最终版本+至今为止月报数据
            var monthReport = await dbContext.Queryable<MonthReport>().Where(t => t.IsDelete == 1).ToListAsync();
            var monthReport24 = await dbContext.Queryable<MonthReportDetailHistory>().ToListAsync();
            decimal completeAmount = 0M;//完成产值
            decimal accumulatedOutputValue = 0M;//开累产值
            decimal EffectiveAmount = 0M;
            decimal RemainingAmount = 0M;
            foreach (var item in pIds)
            {
                var pp = await dbContext.Queryable<Project>().FirstAsync(x => x.Id == item && x.IsDelete == 1);
                if (pp != null)
                {
                    var dateMonth = DateTime.Now.ToDateMonth();
                    var yearMonth = Convert.ToInt32(DateTime.Now.Year + "01");
                    if (pp.CurrencyId == "2a0e99b4-f989-4967-b5f1-5519091d4280".ToGuid())//国内
                    {

                        accumulatedOutputValue = monthReport24.Where(x => x.ProjectId == pp.Id).Sum(x => Convert.ToDecimal(x.ActualCompAmount));//开累
                                                                                                                                                //202306历史产值追加
                        accumulatedOutputValue += monthReport.Where(x => x.ProjectId == pp.Id && x.DateMonth == 202306).Sum(x => Convert.ToDecimal(x.ActualCompAmount));
                    }
                    else
                    {
                        //获取汇率
                        var currencyConvert = await dbContext.Queryable<CurrencyConverter>().Where(t => t.IsDelete == 1 && t.CurrencyId == pp.CurrencyId.ToString()).OrderByDescending(x => x.Year).FirstAsync();
                        pp.ECAmount = pp.ECAmount * currencyConvert.ExchangeRate;
                        accumulatedOutputValue = monthReport24.Where(x => x.ProjectId == pp.Id).Sum(x => Convert.ToDecimal(x.RMBHValue));//开累
                                                                                                                                         //202306历史产值追加
                        accumulatedOutputValue += monthReport.Where(x => x.ProjectId == pp.Id && x.DateMonth == 202306).Sum(x => Convert.ToDecimal(x.RMBHValue));
                    }
                    //截止到当月的完成产值+历史调整开累产值=开累产值
                    accumulatedOutputValue += monthReport.Where(x => x.ProjectId == pp.Id && x.DateMonth > 202412 && x.DateMonth <= dateMonth).Sum(x => x.CompleteProductionAmount);
                    //2024年调整的开累数+截止到当年一月的开累数（年初开累产值）
                    completeAmount = accumulatedOutputValue + monthReport.Where(x => x.ProjectId == pp.Id && x.DateMonth > 202412 && x.DateMonth < yearMonth).Sum(x => x.CompleteProductionAmount);

                    rr.ProjectName = pp.ShortName;
                    EffectiveAmount += Convert.ToDecimal(pp.ECAmount);
                    rr.ContractStipulationEndDate = pp.ContractStipulationEndDate?.ToString("yyyy-MM-dd");
                    RemainingAmount += Convert.ToDecimal(pp.ECAmount) - completeAmount;
                    rr.AccumulatedOutputValue = accumulatedOutputValue;
                }
            }

            rr.EffectiveAmount = EffectiveAmount;
            rr.RemainingAmount = RemainingAmount;
            rr.AccumulatedOutputValue = sum;
            rt.Data = rr;
            rt.Success();
            return rt;
        }




        /// <summary>
        /// 基准计划项目界面 局工程部界面 
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<SearchSubsidiaryCompaniesProjectProductionDto>>> SearchBureauProjectProductionAsync(SearchBaseLinePlanProjectAnnualProductionRequest requestBody)
        {
            List<SearchSubsidiaryCompaniesProjectProductionDto> resultlist = new List<SearchSubsidiaryCompaniesProjectProductionDto>();
            ResponseAjaxResult<List<SearchSubsidiaryCompaniesProjectProductionDto>> rt = new();



            var baseplanproject = await dbContext.Queryable<BaseLinePlanProject>()
           .Where(t => t.IsDelete == 1).ToListAsync();

            List<Guid?> baseplanprojectIds = new List<Guid?>();
            baseplanprojectIds = baseplanproject
                .Select(t => t.ProjectId).ToList();

            var annualProductionShips = await dbContext.Queryable<BaseLinePlanAnnualProductionShips>()
                .Where(t => t.IsDelete == 1).ToListAsync();

            var mainIds = annualProductionShips.Select(x => x.ProjectAnnualProductionId).ToList();


            var projectList = await dbContext.Queryable<Project>().Where(x => x.IsDelete == 1).Select(x => new Project { Id = x.Id, Name = x.Name, CompanyId = x.CompanyId }).ToListAsync();

            var pPlanProduction = await dbContext.Queryable<BaseLinePlanProjectAnnualPlanProduction>()
                 .Where(t => t.IsDelete == 1 && mainIds.Contains(t.Id))
                 .GroupBy(p => p.ProjectId)
                 .Select(it => new SearchSubsidiaryCompaniesProjectProductionDto()
                 {
                     ProjectId = it.ProjectId,
                     JanuaryProductionValue = SqlFunc.AggregateSum(it.JanuaryProductionValue),
                     FebruaryProductionValue = SqlFunc.AggregateSum(it.FebruaryProductionValue),
                     MarchProductionValue = SqlFunc.AggregateSum(it.MarchProductionValue),
                     AprilProductionValue = SqlFunc.AggregateSum(it.AprilProductionValue),
                     MayProductionValue = SqlFunc.AggregateSum(it.MayProductionValue),
                     JuneProductionValue = SqlFunc.AggregateSum(it.JuneProductionValue),
                     JulyProductionValue = SqlFunc.AggregateSum(it.JulyProductionValue),
                     AugustProductionValue = SqlFunc.AggregateSum(it.AugustProductionValue),
                     SeptemberProductionValue = SqlFunc.AggregateSum(it.SeptemberProductionValue),
                     OctoberProductionValue = SqlFunc.AggregateSum(it.OctoberProductionValue),
                     NovemberProductionValue = SqlFunc.AggregateSum(it.NovemberProductionValue),
                     DecemberProductionValue = SqlFunc.AggregateSum(it.DecemberProductionValue)
                 })
                 .ToListAsync();

            foreach (var item in pPlanProduction)
            {
                item.ProjectName = projectList.Where(p => p.Id == item.ProjectId).FirstOrDefault()?.Name;
                var plantype = baseplanproject.Where(p => p.ProjectId == item.ProjectId).FirstOrDefault()?.PlanType;
                if (plantype == "新建")
                {
                    item.NewPlanName = item.ProjectName + "-新建";
                }
                else
                {
                    item.BasePlanName = item.ProjectName + "-基准";
                }

            }
            rt.Count = pPlanProduction.Count;
            return rt.SuccessResult(pPlanProduction);
        }

        /// <summary>
        /// 局工程部界面 项目合计
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<SearchBaseLinePlanProjectAnnualProduction>> SearchBureauProjectProductionAsyncSumAsync(SearchBaseLinePlanProjectAnnualProductionRequest requestBody)
        {

            ResponseAjaxResult<SearchBaseLinePlanProjectAnnualProduction> rt = new();
            SearchBaseLinePlanProjectAnnualProduction rr = new();
            List<BaseLinePlanProjectAnnualPlanProduction> pPlanProduction = new();

            var pomids = await dbContext.Queryable<Institution>().Where(p => p.Grule.Contains(_currentUser.CurrentLoginInstitutionOid)).ToListAsync();
            List<Guid?> guids = pomids.Select(p => p.PomId).ToList();
            var projects = await dbContext.Queryable<Project>().Where(p => p.IsDelete == 1 && guids.Contains(p.ProjectDept)).Select(p => p.Id).ToListAsync();
            var baseplanproject = await dbContext.Queryable<BaseLinePlanProject>()
             .Where(t => t.IsDelete == 1).ToListAsync();

            var annualProductionShips = await dbContext.Queryable<BaseLinePlanAnnualProductionShips>()
                .Where(t => t.IsDelete == 1).ToListAsync();

            var mainIds = annualProductionShips.Select(x => x.ProjectAnnualProductionId).ToList();

            var sums = await dbContext.Queryable<BaseLinePlanProjectAnnualPlanProduction>()
                .Where(t => t.IsDelete == 1 && mainIds.Contains(t.Id) && projects.Contains(t.ProjectId.Value)).Select(p => new BaseLinePlanProjectAnnualPlanProduction
                {
                    JanuaryProductionValue = SqlFunc.AggregateSum(p.JanuaryProductionValue),
                    FebruaryProductionValue = SqlFunc.AggregateSum(p.FebruaryProductionValue),
                    MarchProductionValue = SqlFunc.AggregateSum(p.MarchProductionValue),
                    AprilProductionValue = SqlFunc.AggregateSum(p.AprilProductionValue),
                    MayProductionValue = SqlFunc.AggregateSum(p.MayProductionValue),
                    JuneProductionValue = SqlFunc.AggregateSum(p.JuneProductionValue),
                    JulyProductionValue = SqlFunc.AggregateSum(p.JulyProductionValue),
                    AugustProductionValue = SqlFunc.AggregateSum(p.AugustProductionValue),
                    SeptemberProductionValue = SqlFunc.AggregateSum(p.SeptemberProductionValue),
                    OctoberProductionValue = SqlFunc.AggregateSum(p.OctoberProductionValue),
                    NovemberProductionValue = SqlFunc.AggregateSum(p.NovemberProductionValue),
                    DecemberProductionValue = SqlFunc.AggregateSum(p.DecemberProductionValue)
                }).FirstAsync();

            var sumall = sums;

            var sum = 0M;
            if (sumall != null)
            {
                sum = sumall.JanuaryProductionValue + sumall.FebruaryProductionValue +
                    sumall.MarchProductionValue + sumall.AprilProductionValue +
                    sumall.MayProductionValue + sumall.JuneProductionValue +
                    sumall.JulyProductionValue + sumall.AugustProductionValue +
                    sumall.SeptemberProductionValue + sumall.OctoberProductionValue +
                    sumall.NovemberProductionValue + sumall.DecemberProductionValue;
            }

            pPlanProduction = await dbContext.Queryable<BaseLinePlanProjectAnnualPlanProduction>().Where(it => it.IsDelete == 1).ToListAsync();

            var pIds = pPlanProduction.Select(x => x.ProjectId).ToList();

            //var baseplanproject = await dbContext.Queryable<BaseLinePlanProject>()
            //   .Where(t => t.IsDelete == 1 && t.ProjectId == requestBody.ProjectId).FirstAsync();

            ////项目基本信息
            //var projects = await dbContext.Queryable<Project>().Where(t => t.IsDelete == 1 && pIds.Contains(t.Id)).Select(x => new { x.Id, x.CurrencyId, x.ShortName, x.ECAmount, x.ContractStipulationEndDate }).ToListAsync();

            var ships = await dbContext.Queryable<OwnerShip>().Where(t => t.IsDelete == 1).ToListAsync();

            //项目开累产值   24年调整最终版本+至今为止月报数据
            var monthReport = await dbContext.Queryable<MonthReport>().Where(t => t.IsDelete == 1).ToListAsync();
            var monthReport24 = await dbContext.Queryable<MonthReportDetailHistory>().ToListAsync();
            decimal completeAmount = 0M;//完成产值
            decimal accumulatedOutputValue = 0M;//开累产值
            decimal EffectiveAmount = 0M;
            decimal RemainingAmount = 0M;
            foreach (var item in pIds)
            {
                var pp = await dbContext.Queryable<Project>().FirstAsync(x => x.Id == item && x.IsDelete == 1);
                if (pp != null)
                {
                    var dateMonth = DateTime.Now.ToDateMonth();
                    var yearMonth = Convert.ToInt32(DateTime.Now.Year + "01");
                    if (pp.CurrencyId == "2a0e99b4-f989-4967-b5f1-5519091d4280".ToGuid())//国内
                    {

                        accumulatedOutputValue = monthReport24.Where(x => x.ProjectId == pp.Id).Sum(x => Convert.ToDecimal(x.ActualCompAmount));//开累
                                                                                                                                                //202306历史产值追加
                        accumulatedOutputValue += monthReport.Where(x => x.ProjectId == pp.Id && x.DateMonth == 202306).Sum(x => Convert.ToDecimal(x.ActualCompAmount));
                    }
                    else
                    {
                        //获取汇率
                        var currencyConvert = await dbContext.Queryable<CurrencyConverter>().Where(t => t.IsDelete == 1 && t.CurrencyId == pp.CurrencyId.ToString()).OrderByDescending(x => x.Year).FirstAsync();
                        pp.ECAmount = pp.ECAmount * currencyConvert.ExchangeRate;
                        accumulatedOutputValue = monthReport24.Where(x => x.ProjectId == pp.Id).Sum(x => Convert.ToDecimal(x.RMBHValue));//开累
                                                                                                                                         //202306历史产值追加
                        accumulatedOutputValue += monthReport.Where(x => x.ProjectId == pp.Id && x.DateMonth == 202306).Sum(x => Convert.ToDecimal(x.RMBHValue));
                    }
                    //截止到当月的完成产值+历史调整开累产值=开累产值
                    accumulatedOutputValue += monthReport.Where(x => x.ProjectId == pp.Id && x.DateMonth > 202412 && x.DateMonth <= dateMonth).Sum(x => x.CompleteProductionAmount);
                    //2024年调整的开累数+截止到当年一月的开累数（年初开累产值）
                    completeAmount = accumulatedOutputValue + monthReport.Where(x => x.ProjectId == pp.Id && x.DateMonth > 202412 && x.DateMonth < yearMonth).Sum(x => x.CompleteProductionAmount);

                    rr.ProjectName = pp.ShortName;
                    EffectiveAmount += Convert.ToDecimal(pp.ECAmount);
                    rr.ContractStipulationEndDate = pp.ContractStipulationEndDate?.ToString("yyyy-MM-dd");
                    RemainingAmount += Convert.ToDecimal(pp.ECAmount) - completeAmount;
                    rr.AccumulatedOutputValue = accumulatedOutputValue;
                }
            }

            rr.EffectiveAmount = EffectiveAmount;
            rr.RemainingAmount = RemainingAmount;
            rr.AccumulatedOutputValue = sum;
            rt.Data = rr;
            rt.Success();
            return rt;
        }


        /// <summary>
        /// 基准计划公司界面
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<SearchCompaniesProjectProductionDto>>> SearchCompaniesProjectProductionAsync(SearchBaseLinePlanProjectAnnualProductionRequest requestBody)
        {
            var departmentIds = new List<Guid>();
            //var oids = await dbContext.Queryable<Institution>().Where(x => x.IsDelete == 1 && x.Oid == _currentUser.CurrentLoginInstitutionOid).SingleAsync();
            //var InstitutionId = await baseService.SearchCompanySubPullDownAsync(oids.PomId.Value, false, true);
            //departmentIds = InstitutionId.Data.Select(x => x.Id.Value).ToList();

            var baseplanproject = await dbContext.Queryable<BaseLinePlanProject>().Where(t => t.IsDelete == 1)
                 .WhereIF(!string.IsNullOrWhiteSpace(requestBody.PlanType), p => p.PlanType == requestBody.PlanType)
                 .WhereIF(!string.IsNullOrWhiteSpace(requestBody.StartStatus), p => p.StartStatus == requestBody.StartStatus)
         .ToListAsync();

            List<Guid?> baseplanprojectIds = new List<Guid?>();
            baseplanprojectIds = baseplanproject
                .Select(t => t.ProjectId).ToList();


            List<SearchCompaniesProjectProductionDto> resultlist = new List<SearchCompaniesProjectProductionDto>();
            ResponseAjaxResult<List<SearchCompaniesProjectProductionDto>> rt = new();

            var annualProductionShips = await dbContext.Queryable<BaseLinePlanAnnualProductionShips>()
                .Where(t => t.IsDelete == 1).ToListAsync();

            var mainIds = annualProductionShips.Select(x => x.ProjectAnnualProductionId).ToList();

            var projectList = await dbContext.Queryable<Project>().Where(x => x.IsDelete == 1).Select(x => new Project { Id = x.Id, Name = x.Name, CompanyId = x.CompanyId }).ToListAsync();

            var pPlanProduction = await dbContext.Queryable<BaseLinePlanProjectAnnualPlanProduction>()
                 .Where(t => t.IsDelete == 1 && mainIds.Contains(t.Id) && baseplanprojectIds.Contains(t.ProjectId))
                 .GroupBy(p => p.CompanyId)
                 .Select(it => new SearchCompaniesProjectProductionDto()
                 {
                     CompanyId = it.CompanyId,
                     JanuaryProductionValue = SqlFunc.AggregateSum(it.JanuaryProductionValue),
                     FebruaryProductionValue = SqlFunc.AggregateSum(it.FebruaryProductionValue),
                     MarchProductionValue = SqlFunc.AggregateSum(it.MarchProductionValue),
                     AprilProductionValue = SqlFunc.AggregateSum(it.AprilProductionValue),
                     MayProductionValue = SqlFunc.AggregateSum(it.MayProductionValue),
                     JuneProductionValue = SqlFunc.AggregateSum(it.JuneProductionValue),
                     JulyProductionValue = SqlFunc.AggregateSum(it.JulyProductionValue),
                     AugustProductionValue = SqlFunc.AggregateSum(it.AugustProductionValue),
                     SeptemberProductionValue = SqlFunc.AggregateSum(it.SeptemberProductionValue),
                     OctoberProductionValue = SqlFunc.AggregateSum(it.OctoberProductionValue),
                     NovemberProductionValue = SqlFunc.AggregateSum(it.NovemberProductionValue),
                     DecemberProductionValue = SqlFunc.AggregateSum(it.DecemberProductionValue)
                 })
                 .ToListAsync();


            var List = await dbContext.Queryable<ProductionMonitoringOperationDayReport>().Where(c => c.IsDelete == 1 && c.Type == 1)
                       .Where(c => c.Name != "广航局总体" && !SqlFunc.IsNullOrEmpty(c.Name))
                       .Select(c => new CompanyProductionValueInfoResponseDto
                       {
                           Id = c.Id,
                           CompanyId = c.ItemId,
                           CompanyName = c.Name
                       }).ToListAsync();


            foreach (var item in pPlanProduction)
            {
                item.CompanyName = List.Where(p => p.CompanyId == item.CompanyId).FirstOrDefault()?.CompanyName;
                item.AnnualTotal = item.JanuaryProductionValue + item.FebruaryProductionValue + item.MarchProductionValue + item.AprilProductionValue
                    + item.MayProductionValue + item.JuneProductionValue + item.JulyProductionValue + item.AugustProductionValue + item.SeptemberProductionValue
                    + item.OctoberProductionValue + item.NovemberProductionValue + item.DecemberProductionValue;
            }
            rt.Count = pPlanProduction.Count;
            return rt.SuccessResult(pPlanProduction);
        }

        /// <summary>                    
        /// 计划基准
        /// </summary>
        /// <param name="requsetDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<BaseLinePlanAncomparisonResponseDto>>> SearchBaseLinePlanAncomparisonAsync(BaseLinePlanAncomparisonRequsetDto requsetDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<BaseLinePlanAncomparisonResponseDto>>();
            RefAsync<int> total = 0;
            var List = await baseLinePlanAncomparisonRepository.AsQueryable()
            .Where(x => x.IsDelete == 1)
            .WhereIF(!string.IsNullOrWhiteSpace(requsetDto.Name), x => x.ProjectName.Contains(requsetDto.Name))
            .Select(x => new BaseLinePlanAncomparisonResponseDto()
            {
                Id = x.Id,
                Company = x.Company,
                ProjectName = x.ProjectName,
                Code = x.Code,
                JanuaryProductionValue = x.JanuaryProductionValue,
                FebruaryProductionValue = x.FebruaryProductionValue,
                MarchProductionValue = x.MarchProductionValue,
                AprilProductionValue = x.AprilProductionValue,
                MayProductionValue = x.MayProductionValue,
                JuneProductionValue = x.JuneProductionValue,
                JulyProductionValue = x.JulyProductionValue,
                AugustProductionValue = x.AugustProductionValue,
                SeptemberProductionValue = x.SeptemberProductionValue,
                OctoberProductionValue = x.OctoberProductionValue,
                NovemberProductionValue = x.NovemberProductionValue,
                DecemberProductionValue = x.DecemberProductionValue
            }).ToPageListAsync(requsetDto.PageIndex, requsetDto.PageSize, total);

            responseAjaxResult.Success();
            responseAjaxResult.Count = total;
            responseAjaxResult.Data = List;
            return responseAjaxResult;
        }

        public async Task<ResponseAjaxResult<BaseLinePlanprojectComparisonRequestDto>> SearchBaseLinePlanComparisonAsync(SearchBaseLinePlanprojectComparisonRequestDtoRequest requestBody)
        {
            ResponseAjaxResult<BaseLinePlanprojectComparisonRequestDto> rt = new();

            if (requestBody.ProjectId == Guid.Empty || string.IsNullOrWhiteSpace(requestBody.ProjectId.ToString()))
            {
                rt.Data = new BaseLinePlanprojectComparisonRequestDto();
            }
            var projectList = await dbContext.Queryable<Project>().Where(x => x.IsDelete == 1).Select(x => new Project { Id = x.Id, Name = x.Name, CompanyId = x.CompanyId }).ToListAsync();
            var porjectName = projectList.Where(p => p.Id == requestBody.ProjectId).FirstOrDefault();

            if (porjectName != null)
            {
                SearchBaseLinePlanProjectAnnualProductionDto ppChild = new();
                var basemodel = await dbContext.Queryable<BaseLinePlanAncomparison>().Where(p => p.ProjectName == porjectName.Name || p.Code == porjectName.MasterCode).FirstAsync();
                if (basemodel != null)
                {
                    rt.Data = new BaseLinePlanprojectComparisonRequestDto()
                    {
                        JanuaryProductionValue = basemodel.JanuaryProductionValue,
                        FebruaryProductionValue = basemodel.FebruaryProductionValue,
                        MarchProductionValue = basemodel.MarchProductionValue,
                        AprilProductionValue = basemodel.AprilProductionValue,
                        MayProductionValue = basemodel.MayProductionValue,
                        JuneProductionValue = basemodel.JuneProductionValue,
                        JulyProductionValue = basemodel.JulyProductionValue,
                        AugustProductionValue = basemodel.AugustProductionValue,
                        SeptemberProductionValue = basemodel.SeptemberProductionValue,
                        OctoberProductionValue = basemodel.OctoberProductionValue,
                        NovemberProductionValue = basemodel.NovemberProductionValue,
                        DecemberProductionValue = basemodel.DecemberProductionValue
                    };
                }
            }
            rt.Success();
            return rt;
        }
    }
}
