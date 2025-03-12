﻿using AutoMapper;
using GHMonitoringCenterApi.Application.Contracts.Dto;
using GHMonitoringCenterApi.Application.Contracts.Dto.CompanyProductionValueInfos;
using GHMonitoringCenterApi.Application.Contracts.Dto.EquipmentManagement;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project;
using GHMonitoringCenterApi.Application.Contracts.IService;
using GHMonitoringCenterApi.Application.Contracts.IService.OperationLog;
using GHMonitoringCenterApi.Application.Contracts.IService.Project;
using GHMonitoringCenterApi.Application.Contracts.IService.Push;
using GHMonitoringCenterApi.Application.Contracts.IService.User;
using GHMonitoringCenterApi.Domain.Enums;
using GHMonitoringCenterApi.Domain.IRepository;
using GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Domain.Shared;
using GHMonitoringCenterApi.Domain.Shared.Const;
using GHMonitoringCenterApi.Domain.Shared.Enums;
using GHMonitoringCenterApi.Domain.Shared.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NPOI.HSSF.Extractor;
using SqlSugar;
using SqlSugar.Extensions;
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

            if (requestBody.Year == null)
            {
                requestBody.Year = DateTime.Now.Year;
            }

            //初始化
            List<BaseLinePlanProjectAnnualPlanProduction> pPlanProduction = new();

            var annualProductionShips = await dbContext.Queryable<BaseLinePlanAnnualProductionShips>()
                .Where(t => t.IsDelete == 1).ToListAsync();

            var mainIds = annualProductionShips.Select(x => x.ProjectAnnualProductionId).ToList();

            pPlanProduction = await dbContext.Queryable<BaseLinePlanProjectAnnualPlanProduction>()
                .WhereIF(!string.IsNullOrWhiteSpace(requestBody.ProjectId.ToString()), t => t.ProjectId == requestBody.ProjectId)
                .Where(t => t.IsDelete == 1 && mainIds.Contains(t.Id) && t.Year == requestBody.Year)
                .ToListAsync();

            var pIds = pPlanProduction.Select(x => x.ProjectId).ToList();

            var baseplanproject = await dbContext.Queryable<BaseLinePlanProject>()
               .Where(t => t.IsDelete == 1 && t.Id == requestBody.ProjectId).FirstAsync();

            //项目基本信息
            var projects = await dbContext.Queryable<Project>().Where(t => t.IsDelete == 1 && pIds.Contains(t.Id)).Select(x => new { x.Id, x.CurrencyId, x.ShortName, x.ECAmount, x.ContractStipulationEndDate }).ToListAsync();

            var ships = await dbContext.Queryable<OwnerShip>().Where(t => t.IsDelete == 1).ToListAsync();

            var sums = await dbContext.Queryable<BaseLinePlanProjectAnnualPlanProduction>()
                .Where(t => t.IsDelete == 1 && mainIds.Contains(t.Id) && t.ProjectId == requestBody.ProjectId).Select(p => new BaseLinePlanProjectAnnualPlanProduction
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

            //项目开累产值   24年调整最终版本+至今为止月报数据
            var monthReport = await dbContext.Queryable<MonthReport>().Where(t => t.IsDelete == 1).ToListAsync();
            var monthReport24 = await dbContext.Queryable<MonthReportDetailHistory>().ToListAsync();

            if (!string.IsNullOrWhiteSpace(requestBody.ProjectId.ToString()))
            {
                decimal completeAmount = 0M;//完成产值
                decimal accumulatedOutputValue = 0M;//开累产值


                if (baseplanproject != null && !string.IsNullOrWhiteSpace(baseplanproject.Association))
                {
                    var pp = await dbContext.Queryable<Project>().FirstAsync(x => x.MasterCode == baseplanproject.Association && x.IsDelete == 1);
                    if (pp != null)
                    {
                        var dateMonth = DateTime.Now.ToDateMonth();
                        var yearMonth = System.Convert.ToInt32(DateTime.Now.Year + "01");
                        if (pp.CurrencyId == "2a0e99b4-f989-4967-b5f1-5519091d4280".ToGuid())//国内
                        {

                            accumulatedOutputValue = monthReport24.Where(x => x.ProjectId == pp.Id).Sum(x => System.Convert.ToDecimal(x.ActualCompAmount));//开累
                                                                                                                                                           //202306历史产值追加
                            accumulatedOutputValue += monthReport.Where(x => x.ProjectId == pp.Id && x.DateMonth == 202306).Sum(x => System.Convert.ToDecimal(x.ActualCompAmount));
                        }
                        else
                        {
                            //获取汇率
                            var currencyConvert = await dbContext.Queryable<CurrencyConverter>().Where(t => t.IsDelete == 1 && t.CurrencyId == pp.CurrencyId.ToString()).OrderByDescending(x => x.Year).FirstAsync();
                            pp.ECAmount = pp.ECAmount * currencyConvert.ExchangeRate;
                            accumulatedOutputValue = monthReport24.Where(x => x.ProjectId == pp.Id).Sum(x => System.Convert.ToDecimal(x.RMBHValue));//开累
                                                                                                                                                    //202306历史产值追加
                            accumulatedOutputValue += monthReport.Where(x => x.ProjectId == pp.Id && x.DateMonth == 202306).Sum(x => System.Convert.ToDecimal(x.RMBHValue));
                        }
                        //截止到当月的完成产值+历史调整开累产值=开累产值
                        accumulatedOutputValue += monthReport.Where(x => x.ProjectId == pp.Id && x.DateMonth > 202412 && x.DateMonth <= dateMonth).Sum(x => x.CompleteProductionAmount);
                        //2024年调整的开累数+截止到当年一月的开累数（年初开累产值）
                        completeAmount = accumulatedOutputValue + monthReport.Where(x => x.ProjectId == pp.Id && x.DateMonth > 202412 && x.DateMonth < yearMonth).Sum(x => x.CompleteProductionAmount);

                        rr.ProjectName = pp.ShortName;
                        rr.EffectiveAmount = System.Convert.ToDecimal(pp.ECAmount);
                        rr.ContractStipulationEndDate = pp.ContractStipulationEndDate?.ToString("yyyy-MM-dd");
                        rr.RemainingAmount = System.Convert.ToDecimal(pp.ECAmount) - completeAmount;
                        rr.AccumulatedOutputValue = accumulatedOutputValue;
                        rr.AccumulatedOutputValue = sum;

                    }
                }
                else if (baseplanproject != null)
                {
                    rr.EffectiveAmount = baseplanproject.EffectiveAmount;
                    rr.RemainingAmount = baseplanproject.RemainingAmount;
                }
                if (baseplanproject != null)
                {
                    //rr.PlanVersion = baseplanproject.PlanVersion;
                    rr.PlanType = baseplanproject.PlanType;
                    rr.StartStatus = baseplanproject.StartStatus;
                    rr.Association = baseplanproject.Association;
                    rr.CompletionTime = baseplanproject.CompletionTime;
                    rr.IsSubPackage = baseplanproject.IsSubPackage;
                    rr.PlanStatus = baseplanproject.PlanStatus;
                    rr.PlanStatusText = baseplanproject.PlanStatus == 1 ? "已审批" : baseplanproject.PlanStatus == 0 ? "待审批" : "驳回";
                    rr.ProjectName = baseplanproject.ShortName;
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

            if (requestBody.Year == null)
            {
                requestBody.Year = DateTime.Now.Year;
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
                .Where(t => t.Year == requestBody.Year)
                .Where(t => t.IsDelete == 1 && mainIds.Contains(t.Id))
                .ToListAsync();

            foreach (var item in pPlanProduction)
            {

                SearchBaseLinePlanProjectAnnualProductionDto ppChild = new();
                ppChild.Id = item.Id;
                ppChild.JanuaryProductionQuantity = Setnumericalconversion(item.JanuaryProductionQuantity);
                ppChild.JanuaryProductionValue = Setnumericalconversion(item.JanuaryProductionValue);
                ppChild.FebruaryProductionQuantity = Setnumericalconversion(item.FebruaryProductionQuantity);
                ppChild.FebruaryProductionValue = Setnumericalconversion(item.FebruaryProductionValue);
                ppChild.MarchProductionQuantity = Setnumericalconversion(item.MarchProductionQuantity);
                ppChild.MarchProductionValue = Setnumericalconversion(item.MarchProductionValue);
                ppChild.AprilProductionQuantity = Setnumericalconversion(item.AprilProductionQuantity);
                ppChild.AprilProductionValue = Setnumericalconversion(item.AprilProductionValue);
                ppChild.MayProductionQuantity = Setnumericalconversion(item.MayProductionQuantity);
                ppChild.MayProductionValue = Setnumericalconversion(item.MayProductionValue);
                ppChild.JuneProductionQuantity = Setnumericalconversion(item.JuneProductionQuantity);
                ppChild.JuneProductionValue = Setnumericalconversion(item.JuneProductionValue);
                ppChild.JulyProductionQuantity = Setnumericalconversion(item.JulyProductionQuantity);
                ppChild.JulyProductionValue = Setnumericalconversion(item.JulyProductionValue);
                ppChild.AugustProductionValue = Setnumericalconversion(item.AugustProductionValue);
                ppChild.AugustProductionQuantity = Setnumericalconversion(item.AugustProductionQuantity);
                ppChild.SeptemberProductionQuantity = Setnumericalconversion(item.SeptemberProductionQuantity);
                ppChild.SeptemberProductionValue = Setnumericalconversion(item.SeptemberProductionValue);
                ppChild.OctoberProductionQuantity = Setnumericalconversion(item.OctoberProductionQuantity);
                ppChild.OctoberProductionValue = Setnumericalconversion(item.OctoberProductionValue);
                ppChild.NovemberProductionQuantity = Setnumericalconversion(item.NovemberProductionQuantity);
                ppChild.NovemberProductionValue = Setnumericalconversion(item.NovemberProductionValue);
                ppChild.DecemberProductionQuantity = Setnumericalconversion(item.DecemberProductionQuantity);
                ppChild.DecemberProductionValue = Setnumericalconversion(item.DecemberProductionValue);
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
                    var baseid = GuidUtil.Next();
                    BaseLinePlanProject baseLinePlanProject = null;
                    var baselineplan = item.baseLinePlanproject;
                    var baselinefirbaseLinePlanProject = await dbContext.Queryable<BaseLinePlanProject>().Where(p => p.Id == baselineplan.ProjectId).FirstAsync();
                    if (baselinefirbaseLinePlanProject != null)
                    {
                        baselinefirbaseLinePlanProject.PlanType = baselineplan.PlanType;
                        baselinefirbaseLinePlanProject.StartStatus = baselineplan.StartStatus;
                        baselinefirbaseLinePlanProject.CompanyId = _currentUser.CurrentLoginInstitutionId;
                        baselinefirbaseLinePlanProject.Year = baselineplan.Year;
                        baselinefirbaseLinePlanProject.PlanType = baselineplan.PlanType;
                        baselinefirbaseLinePlanProject.StartStatus = baselineplan.StartStatus;
                        baselinefirbaseLinePlanProject.ProjectId = baselineplan.ProjectId;
                        baselinefirbaseLinePlanProject.Association = baselineplan.Association;
                        baselinefirbaseLinePlanProject.CompletionTime = baselineplan.CompletionTime;
                        baselinefirbaseLinePlanProject.EffectiveAmount = baselineplan.EffectiveAmount;
                        baselinefirbaseLinePlanProject.IsSubPackage = baselineplan.IsSubPackage;
                        baselinefirbaseLinePlanProject.RemainingAmount = baselineplan.RemainingAmount;
                        await dbContext.Updateable(baselinefirbaseLinePlanProject).ExecuteCommandAsync();
                    }
                    else
                    {
                        baseLinePlanProject = new BaseLinePlanProject();
                        baseLinePlanProject.Id = baseid;
                        baseLinePlanProject.CompanyId = _currentUser.CurrentLoginInstitutionId;
                        baseLinePlanProject.Year = baselineplan.Year;
                        baseLinePlanProject.PlanType = baselineplan.PlanType;
                        baseLinePlanProject.StartStatus = baselineplan.StartStatus;
                        baseLinePlanProject.ProjectId = baselineplan.ProjectId;
                        baseLinePlanProject.Association = baselineplan.Association;
                        baseLinePlanProject.CompletionTime = baselineplan.CompletionTime;
                        baseLinePlanProject.EffectiveAmount = baselineplan.EffectiveAmount;
                        baseLinePlanProject.IsSubPackage = baselineplan.IsSubPackage;
                        baseLinePlanProject.RemainingAmount = baselineplan.RemainingAmount;
                        baseLinePlanProject.ShortName = baselineplan.ShortName;
                        await dbContext.Insertable(baseLinePlanProject).ExecuteCommandAsync();
                    }

                    if (!string.IsNullOrWhiteSpace(item.Id.ToString()) && item.Id != Guid.Empty)
                    {
                        var first = tables.FirstOrDefault(x => x.Id == item.Id);
                        if (first != null)
                        {
                            first.JanuaryProductionQuantity = Setnumericalconversiontwo(item.JanuaryProductionQuantity);
                            first.JanuaryProductionValue = Setnumericalconversiontwo(item.JanuaryProductionValue);
                            first.FebruaryProductionQuantity = Setnumericalconversiontwo(item.FebruaryProductionQuantity);
                            first.FebruaryProductionValue = Setnumericalconversiontwo(item.FebruaryProductionValue);
                            first.MarchProductionQuantity = Setnumericalconversiontwo(item.MarchProductionQuantity);
                            first.MarchProductionValue = Setnumericalconversiontwo(item.MarchProductionValue);
                            first.AprilProductionQuantity = Setnumericalconversiontwo(item.AprilProductionQuantity);
                            first.AprilProductionValue = Setnumericalconversiontwo(item.AprilProductionValue);
                            first.MayProductionQuantity = Setnumericalconversiontwo(item.MayProductionQuantity);
                            first.MayProductionValue = Setnumericalconversiontwo(item.MayProductionValue);
                            first.JuneProductionQuantity = Setnumericalconversiontwo(item.JuneProductionQuantity);
                            first.JuneProductionValue = Setnumericalconversiontwo(item.JuneProductionValue);
                            first.JulyProductionQuantity = Setnumericalconversiontwo(item.JulyProductionQuantity);
                            first.JulyProductionValue = Setnumericalconversiontwo(item.JulyProductionValue);
                            first.AugustProductionValue = Setnumericalconversiontwo(item.AugustProductionValue);
                            first.AugustProductionQuantity = Setnumericalconversiontwo(item.AugustProductionQuantity);
                            first.SeptemberProductionQuantity = Setnumericalconversiontwo(item.SeptemberProductionQuantity);
                            first.SeptemberProductionValue = Setnumericalconversiontwo(item.SeptemberProductionValue);
                            first.OctoberProductionQuantity = Setnumericalconversiontwo(item.OctoberProductionQuantity);
                            first.OctoberProductionValue = Setnumericalconversiontwo(item.OctoberProductionValue);
                            first.NovemberProductionQuantity = Setnumericalconversiontwo(item.NovemberProductionQuantity);
                            first.NovemberProductionValue = Setnumericalconversiontwo(item.NovemberProductionValue);
                            first.DecemberProductionQuantity = Setnumericalconversiontwo(item.DecemberProductionQuantity);
                            first.DecemberProductionValue = Setnumericalconversiontwo(item.DecemberProductionValue);
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
                            JanuaryProductionQuantity = Setnumericalconversiontwo(item.JanuaryProductionQuantity),
                            JanuaryProductionValue = Setnumericalconversiontwo(item.JanuaryProductionValue),
                            FebruaryProductionQuantity = Setnumericalconversiontwo(item.FebruaryProductionQuantity),
                            FebruaryProductionValue = Setnumericalconversiontwo(item.FebruaryProductionValue),
                            MarchProductionQuantity = Setnumericalconversiontwo(item.MarchProductionQuantity),
                            MarchProductionValue = Setnumericalconversiontwo(item.MarchProductionValue),
                            AprilProductionQuantity = Setnumericalconversiontwo(item.AprilProductionQuantity),
                            AprilProductionValue = Setnumericalconversiontwo(item.AprilProductionValue),
                            MayProductionQuantity = Setnumericalconversiontwo(item.MayProductionQuantity),
                            MayProductionValue = Setnumericalconversiontwo(item.MayProductionValue),
                            JuneProductionQuantity = Setnumericalconversiontwo(item.JuneProductionQuantity),
                            JuneProductionValue = Setnumericalconversiontwo(item.JuneProductionValue),
                            JulyProductionQuantity = Setnumericalconversiontwo(item.JulyProductionQuantity),
                            JulyProductionValue = Setnumericalconversiontwo(item.JulyProductionValue),
                            AugustProductionValue = Setnumericalconversiontwo(item.AugustProductionValue),
                            AugustProductionQuantity = Setnumericalconversiontwo(item.AugustProductionQuantity),
                            SeptemberProductionQuantity = Setnumericalconversiontwo(item.SeptemberProductionQuantity),
                            SeptemberProductionValue = Setnumericalconversiontwo(item.SeptemberProductionValue),
                            OctoberProductionQuantity = Setnumericalconversiontwo(item.OctoberProductionQuantity),
                            OctoberProductionValue = Setnumericalconversiontwo(item.OctoberProductionValue),
                            NovemberProductionQuantity = Setnumericalconversiontwo(item.NovemberProductionQuantity),
                            NovemberProductionValue = Setnumericalconversiontwo(item.NovemberProductionValue),
                            DecemberProductionQuantity = Setnumericalconversiontwo(item.DecemberProductionQuantity),
                            DecemberProductionValue = Setnumericalconversiontwo(item.DecemberProductionValue),
                            ProjectId = baseid,
                            CompanyId = _currentUser.CurrentLoginInstitutionId,
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
            //var projects = await dbContext.Queryable<Project>().Where(p => p.IsDelete == 1 && guids.Contains(p.ProjectDept)).Select(p => p.Id).ToListAsync();
            var baseplanproject = await dbContext.Queryable<BaseLinePlanProject>()
             .Where(t => t.IsDelete == 1).ToListAsync();

            var projects = await dbContext.Queryable<BaseLinePlanProject>().Where(p => p.IsDelete == 1).WhereIF(requestBody.Year != null, p => p.Year == requestBody.Year).WhereIF(!string.IsNullOrWhiteSpace(requestBody.StartStatus), p => p.StartStatus == requestBody.StartStatus).Select(p => p.Id).ToListAsync();

            if (requestBody.Year == null)
            {
                requestBody.Year = DateTime.Now.Year;
            }

            //var list= dbContext.Queryable<BaseLinePlanAncomparison>().LeftJoin<Project>((b,p)=>b.Code==p.MasterCode).

            var annualProductionShips = await dbContext.Queryable<BaseLinePlanAnnualProductionShips>()
                .Where(t => t.IsDelete == 1).ToListAsync();
            var mainIds = annualProductionShips.Select(x => x.ProjectAnnualProductionId).ToList();

            //var projectList = await dbContext.Queryable<Project>().Where(x => x.IsDelete == 1).Select(x => new Project { Id = x.Id, Name = x.Name, CompanyId = x.CompanyId }).ToListAsync();

            var pPlanProduction = await dbContext.Queryable<BaseLinePlanProjectAnnualPlanProduction>()
                 .Where(t => t.IsDelete == 1 && mainIds.Contains(t.Id) && projects.Contains(t.ProjectId.Value) && t.Year == requestBody.Year)
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
                Converttowanyuan(item);

                item.ProjectName = baseplanproject.Where(p => p.Id == item.ProjectId).FirstOrDefault()?.ShortName;
                //var plantype = baseplanproject.Where(p => p.ProjectId == item.ProjectId).FirstOrDefault()?.PlanType;
                //if (plantype == "新建")
                //{
                //    item.NewPlanName = item.ProjectName + "-新建";
                //}
                //else
                //{
                //    item.BasePlanName = item.ProjectName + "-基准";
                //}
            }
            rt.Count = pPlanProduction.Count;
            return rt.SuccessResult(pPlanProduction);
        }

        private void Converttowanyuan(SearchSubsidiaryCompaniesProjectProductionDto item)
        {
            item.JanuaryProductionValue = Setnumericalconversion(item.JanuaryProductionValue);
            item.FebruaryProductionValue = Setnumericalconversion(item.FebruaryProductionValue);
            item.MarchProductionValue = Setnumericalconversion(item.MarchProductionValue);
            item.AprilProductionValue = Setnumericalconversion(item.AprilProductionValue);
            item.MayProductionValue = Setnumericalconversion(item.MayProductionValue);
            item.JuneProductionValue = Setnumericalconversion(item.JuneProductionValue);
            item.JulyProductionValue = Setnumericalconversion(item.JulyProductionValue);
            item.AugustProductionValue = Setnumericalconversion(item.AugustProductionValue);
            item.SeptemberProductionValue = Setnumericalconversion(item.SeptemberProductionValue);
            item.OctoberProductionValue = Setnumericalconversion(item.OctoberProductionValue);
            item.NovemberProductionValue = Setnumericalconversion(item.NovemberProductionValue);
            item.DecemberProductionValue = Setnumericalconversion(item.DecemberProductionValue);
        }

        public async Task<ResponseAjaxResult<SearchBaseLinePlanProjectAnnualProduction>> SearchBaseLinePlanProjectAnnualProductionSumAsync(SearchBaseLinePlanProjectAnnualProductionRequest requestBody)
        {

            ResponseAjaxResult<SearchBaseLinePlanProjectAnnualProduction> rt = new();
            SearchBaseLinePlanProjectAnnualProduction rr = new();
            List<BaseLinePlanProjectAnnualPlanProduction> pPlanProduction = new();

            var pomids = await dbContext.Queryable<Institution>().Where(p => p.Grule.Contains(_currentUser.CurrentLoginInstitutionOid)).ToListAsync();
            List<Guid?> guids = pomids.Select(p => p.PomId).ToList();
            //var projects = await dbContext.Queryable<Project>().Where(p => p.IsDelete == 1 && guids.Contains(p.ProjectDept)).Select(p => p.Id).ToListAsync();
            var baseplanproject = await dbContext.Queryable<BaseLinePlanProject>()
             .Where(t => t.IsDelete == 1).ToListAsync();

            var projects = await dbContext.Queryable<BaseLinePlanProject>()
             .Where(t => t.IsDelete == 1).Select(p => p.Id).ToListAsync();

            var annualProductionShips = await dbContext.Queryable<BaseLinePlanAnnualProductionShips>()
                .Where(t => t.IsDelete == 1).ToListAsync();

            var mainIds = annualProductionShips.Select(x => x.ProjectAnnualProductionId).ToList();

            if (requestBody.Year == null)
            {
                requestBody.Year = DateTime.Now.Year;
            }

            var sums = await dbContext.Queryable<BaseLinePlanProjectAnnualPlanProduction>()
                .Where(t => t.IsDelete == 1 && mainIds.Contains(t.Id) && projects.Contains(t.ProjectId.Value) && t.Year == requestBody.Year).Select(p => new BaseLinePlanProjectAnnualPlanProduction
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

            var pIds = pPlanProduction.Select(x => x.Id).ToList();

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
                var baseline = baseplanproject.Where(t => t.Id == item).First();
                if (baseline.Association != null)
                {
                    var pp = await dbContext.Queryable<Project>().FirstAsync(x => x.Id == item && x.IsDelete == 1);
                    if (pp != null)
                    {
                        var dateMonth = DateTime.Now.ToDateMonth();
                        var yearMonth = System.Convert.ToInt32(DateTime.Now.Year + "01");
                        if (pp.CurrencyId == "2a0e99b4-f989-4967-b5f1-5519091d4280".ToGuid())//国内
                        {

                            accumulatedOutputValue = monthReport24.Where(x => x.ProjectId == pp.Id).Sum(x => System.Convert.ToDecimal(x.ActualCompAmount));//开累
                                                                                                                                                           //202306历史产值追加
                            accumulatedOutputValue += monthReport.Where(x => x.ProjectId == pp.Id && x.DateMonth == 202306).Sum(x => System.Convert.ToDecimal(x.ActualCompAmount));
                        }
                        else
                        {
                            //获取汇率
                            var currencyConvert = await dbContext.Queryable<CurrencyConverter>().Where(t => t.IsDelete == 1 && t.CurrencyId == pp.CurrencyId.ToString()).OrderByDescending(x => x.Year).FirstAsync();
                            pp.ECAmount = pp.ECAmount * currencyConvert.ExchangeRate;
                            accumulatedOutputValue = monthReport24.Where(x => x.ProjectId == pp.Id).Sum(x => System.Convert.ToDecimal(x.RMBHValue));//开累
                                                                                                                                                    //202306历史产值追加
                            accumulatedOutputValue += monthReport.Where(x => x.ProjectId == pp.Id && x.DateMonth == 202306).Sum(x => System.Convert.ToDecimal(x.RMBHValue));
                        }
                        //截止到当月的完成产值+历史调整开累产值=开累产值
                        accumulatedOutputValue += monthReport.Where(x => x.ProjectId == pp.Id && x.DateMonth > 202412 && x.DateMonth <= dateMonth).Sum(x => x.CompleteProductionAmount);
                        //2024年调整的开累数+截止到当年一月的开累数（年初开累产值）
                        completeAmount = accumulatedOutputValue + monthReport.Where(x => x.ProjectId == pp.Id && x.DateMonth > 202412 && x.DateMonth < yearMonth).Sum(x => x.CompleteProductionAmount);

                        rr.ProjectName = baseplanproject.Where(p => p.Id == item).First()?.ShortName;
                        EffectiveAmount += System.Convert.ToDecimal(pp.ECAmount);
                        rr.ContractStipulationEndDate = pp.ContractStipulationEndDate?.ToString("yyyy-MM-dd");
                        RemainingAmount += System.Convert.ToDecimal(pp.ECAmount) - completeAmount;
                        rr.AccumulatedOutputValue = accumulatedOutputValue;
                    }
                }
                else
                {
                    EffectiveAmount += System.Convert.ToDecimal(baseline.EffectiveAmount);
                    RemainingAmount += System.Convert.ToDecimal(baseline.RemainingAmount);
                }


            }

            rr.EffectiveAmount = EffectiveAmount;
            rr.RemainingAmount = RemainingAmount;
            rr.AccumulatedOutputValue = Setnumericalconversion(sum);
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
           .Where(t => t.IsDelete == 1).WhereIF(!string.IsNullOrWhiteSpace(requestBody.StartStatus), t => t.StartStatus == requestBody.StartStatus).WhereIF(requestBody.Year != null, t => t.Year == requestBody.Year).ToListAsync();

            List<Guid> baseplanprojectIds = new List<Guid>();
            baseplanprojectIds = baseplanproject
                .Select(t => t.Id).ToList();

            var annualProductionShips = await dbContext.Queryable<BaseLinePlanAnnualProductionShips>()
                .Where(t => t.IsDelete == 1).ToListAsync();

            var mainIds = annualProductionShips.Select(x => x.ProjectAnnualProductionId).ToList();

            if (requestBody.Year == null)
            {
                requestBody.Year = DateTime.Now.Year;
            }

            var projectList = await dbContext.Queryable<Project>().Where(x => x.IsDelete == 1).Select(x => new Project { Id = x.Id, Name = x.Name, CompanyId = x.CompanyId }).ToListAsync();

            var pPlanProduction = await dbContext.Queryable<BaseLinePlanProjectAnnualPlanProduction>()
                 .Where(t => t.IsDelete == 1 && mainIds.Contains(t.Id) && t.Year == requestBody.Year)
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

                Converttowanyuan(item);


                item.ProjectName = baseplanproject.Where(p => p.Id == item.ProjectId).FirstOrDefault()?.ShortName;
                var plantype = baseplanproject.Where(p => p.ProjectId == item.ProjectId).FirstOrDefault()?.PlanType;
                //if (plantype == "新建")
                //{
                //    item.NewPlanName = item.ProjectName + "-新建";
                //}
                //else
                //{
                //    item.BasePlanName = item.ProjectName + "-基准";
                //}

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

            if (requestBody.Year == null)
            {
                requestBody.Year = DateTime.Now.Year;
            }

            var annualProductionShips = await dbContext.Queryable<BaseLinePlanAnnualProductionShips>()
                .Where(t => t.IsDelete == 1).ToListAsync();

            var mainIds = annualProductionShips.Select(x => x.ProjectAnnualProductionId).ToList();

            var sums = await dbContext.Queryable<BaseLinePlanProjectAnnualPlanProduction>()
                .Where(t => t.IsDelete == 1 && mainIds.Contains(t.Id) && projects.Contains(t.ProjectId.Value) && t.Year == requestBody.Year).Select(p => new BaseLinePlanProjectAnnualPlanProduction
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
                    var yearMonth = System.Convert.ToInt32(DateTime.Now.Year + "01");
                    if (pp.CurrencyId == "2a0e99b4-f989-4967-b5f1-5519091d4280".ToGuid())//国内
                    {

                        accumulatedOutputValue = monthReport24.Where(x => x.ProjectId == pp.Id).Sum(x => System.Convert.ToDecimal(x.ActualCompAmount));//开累
                                                                                                                                                       //202306历史产值追加
                        accumulatedOutputValue += monthReport.Where(x => x.ProjectId == pp.Id && x.DateMonth == 202306).Sum(x => System.Convert.ToDecimal(x.ActualCompAmount));
                    }
                    else
                    {
                        //获取汇率
                        var currencyConvert = await dbContext.Queryable<CurrencyConverter>().Where(t => t.IsDelete == 1 && t.CurrencyId == pp.CurrencyId.ToString()).OrderByDescending(x => x.Year).FirstAsync();
                        pp.ECAmount = pp.ECAmount * currencyConvert.ExchangeRate;
                        accumulatedOutputValue = monthReport24.Where(x => x.ProjectId == pp.Id).Sum(x => System.Convert.ToDecimal(x.RMBHValue));//开累
                                                                                                                                                //202306历史产值追加
                        accumulatedOutputValue += monthReport.Where(x => x.ProjectId == pp.Id && x.DateMonth == 202306).Sum(x => System.Convert.ToDecimal(x.RMBHValue));
                    }
                    //截止到当月的完成产值+历史调整开累产值=开累产值
                    accumulatedOutputValue += monthReport.Where(x => x.ProjectId == pp.Id && x.DateMonth > 202412 && x.DateMonth <= dateMonth).Sum(x => x.CompleteProductionAmount);
                    //2024年调整的开累数+截止到当年一月的开累数（年初开累产值）
                    completeAmount = accumulatedOutputValue + monthReport.Where(x => x.ProjectId == pp.Id && x.DateMonth > 202412 && x.DateMonth < yearMonth).Sum(x => x.CompleteProductionAmount);

                    rr.ProjectName = pp.ShortName;
                    EffectiveAmount += System.Convert.ToDecimal(pp.ECAmount);
                    rr.ContractStipulationEndDate = pp.ContractStipulationEndDate?.ToString("yyyy-MM-dd");
                    RemainingAmount += System.Convert.ToDecimal(pp.ECAmount) - completeAmount;
                    rr.AccumulatedOutputValue = accumulatedOutputValue;
                }
            }

            rr.EffectiveAmount = EffectiveAmount;
            rr.RemainingAmount = RemainingAmount;
            rr.AccumulatedOutputValue = Setnumericalconversion(sum);
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

            List<Guid> baseplanprojectIds = new List<Guid>();
            baseplanprojectIds = baseplanproject
                .Select(t => t.Id).ToList();

            if (requestBody.Year == null)
            {
                requestBody.Year = DateTime.Now.Year;
            }

            List<SearchCompaniesProjectProductionDto> resultlist = new List<SearchCompaniesProjectProductionDto>();
            ResponseAjaxResult<List<SearchCompaniesProjectProductionDto>> rt = new();

            var annualProductionShips = await dbContext.Queryable<BaseLinePlanAnnualProductionShips>()
                .Where(t => t.IsDelete == 1).ToListAsync();

            var mainIds = annualProductionShips.Select(x => x.ProjectAnnualProductionId).ToList();

            var projectList = await dbContext.Queryable<Project>().Where(x => x.IsDelete == 1).Select(x => new Project { Id = x.Id, Name = x.Name, CompanyId = x.CompanyId }).ToListAsync();

            var pPlanProduction = await dbContext.Queryable<BaseLinePlanProjectAnnualPlanProduction>()
                 .Where(t => t.IsDelete == 1 && mainIds.Contains(t.Id) && baseplanprojectIds.Contains(t.ProjectId.Value) && t.Year == requestBody.Year)
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

                item.JanuaryProductionValue = Setnumericalconversion(item.JanuaryProductionValue);
                item.FebruaryProductionValue = Setnumericalconversion(item.FebruaryProductionValue);
                item.MarchProductionValue = Setnumericalconversion(item.MarchProductionValue);
                item.AprilProductionValue = Setnumericalconversion(item.AprilProductionValue);
                item.MayProductionValue = Setnumericalconversion(item.MayProductionValue);
                item.JuneProductionValue = Setnumericalconversion(item.JuneProductionValue);
                item.JulyProductionValue = Setnumericalconversion(item.JulyProductionValue);
                item.AugustProductionValue = Setnumericalconversion(item.AugustProductionValue);
                item.SeptemberProductionValue = Setnumericalconversion(item.SeptemberProductionValue);
                item.OctoberProductionValue = Setnumericalconversion(item.OctoberProductionValue);
                item.NovemberProductionValue = Setnumericalconversion(item.NovemberProductionValue);
                item.DecemberProductionValue = Setnumericalconversion(item.DecemberProductionValue);
                item.CompanyName = List.Where(p => p.CompanyId == item.CompanyId).FirstOrDefault()?.CompanyName;
                //item.AnnualTotal = item.JanuaryProductionValue + item.FebruaryProductionValue + item.MarchProductionValue + item.AprilProductionValue
                //    + item.MayProductionValue + item.JuneProductionValue + item.JulyProductionValue + item.AugustProductionValue + item.SeptemberProductionValue
                //    + item.OctoberProductionValue + item.NovemberProductionValue + item.DecemberProductionValue;
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
            foreach (var x in List)
            {
                //x.JanuaryProductionValue = Setnumericalconversion(x.JanuaryProductionValue);
                //x.FebruaryProductionValue = Setnumericalconversion(x.FebruaryProductionValue);
                //x.MarchProductionValue = Setnumericalconversion(x.MarchProductionValue);
                //x.AprilProductionValue = Setnumericalconversion(x.AprilProductionValue);
                //x.MayProductionValue = Setnumericalconversion(x.MayProductionValue);
                //x.JuneProductionValue = Setnumericalconversion(x.JuneProductionValue);
                //x.JulyProductionValue = Setnumericalconversion(x.JulyProductionValue);
                //x.AugustProductionValue = Setnumericalconversion(x.AugustProductionValue);
                //x.SeptemberProductionValue = Setnumericalconversion(x.SeptemberProductionValue);
                //x.OctoberProductionValue = Setnumericalconversion(x.OctoberProductionValue);
                //x.NovemberProductionValue = Setnumericalconversion(x.NovemberProductionValue);
                //x.DecemberProductionValue = Setnumericalconversion(x.DecemberProductionValue);

                x.JanuaryProductionValue = (x.JanuaryProductionValue);
                x.FebruaryProductionValue = (x.FebruaryProductionValue);
                x.MarchProductionValue = (x.MarchProductionValue);
                x.AprilProductionValue = (x.AprilProductionValue);
                x.MayProductionValue = (x.MayProductionValue);
                x.JuneProductionValue = (x.JuneProductionValue);
                x.JulyProductionValue = (x.JulyProductionValue);
                x.AugustProductionValue = (x.AugustProductionValue);
                x.SeptemberProductionValue = (x.SeptemberProductionValue);
                x.OctoberProductionValue = (x.OctoberProductionValue);
                x.NovemberProductionValue = (x.NovemberProductionValue);
                x.DecemberProductionValue = (x.DecemberProductionValue);
            }

            responseAjaxResult.Success();
            responseAjaxResult.Count = total;
            responseAjaxResult.Data = List;
            return responseAjaxResult;
        }


        /// <summary>
        /// 计划基准
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
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
                var basemodel = await dbContext.Queryable<BaseLinePlanAncomparison>().Where(p => p.ProjectName == porjectName.Name).FirstAsync();
                if (basemodel == null && !string.IsNullOrWhiteSpace(porjectName.MasterCode))
                {
                    basemodel = await dbContext.Queryable<BaseLinePlanAncomparison>().Where(p => p.Code == porjectName.MasterCode).FirstAsync();
                }
                if (basemodel != null)
                {
                    rt.Data = new BaseLinePlanprojectComparisonRequestDto()
                    {
                        JanuaryProductionValue = (basemodel.JanuaryProductionValue),
                        FebruaryProductionValue = (basemodel.FebruaryProductionValue),
                        MarchProductionValue = (basemodel.MarchProductionValue),
                        AprilProductionValue = (basemodel.AprilProductionValue),
                        MayProductionValue = (basemodel.MayProductionValue),
                        JuneProductionValue = (basemodel.JuneProductionValue),
                        JulyProductionValue = (basemodel.JulyProductionValue),
                        AugustProductionValue = (basemodel.AugustProductionValue),
                        SeptemberProductionValue = (basemodel.SeptemberProductionValue),
                        OctoberProductionValue = (basemodel.OctoberProductionValue),
                        NovemberProductionValue = (basemodel.NovemberProductionValue),
                        DecemberProductionValue = (basemodel.DecemberProductionValue)
                    };
                }
            }
            rt.Success();
            return rt;
        }


        /// <summary>                    
        /// 计划基准新
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<SearchSubsidiaryCompaniesProjectProductionDto>>> SearchBaseLinePlanAncomparisonNewAsync(BaseLinePlanAncomparisonRequsetDto requestBody)
        {
            List<SearchSubsidiaryCompaniesProjectProductionDto> resultlist = new List<SearchSubsidiaryCompaniesProjectProductionDto>();
            ResponseAjaxResult<List<SearchSubsidiaryCompaniesProjectProductionDto>> rt = new();
            var pomids = await dbContext.Queryable<Institution>().Where(p => p.Grule.Contains(_currentUser.CurrentLoginInstitutionOid)).ToListAsync();
            List<Guid?> guids = pomids.Select(p => p.PomId).ToList();
            //var projects = await dbContext.Queryable<Project>().Where(p => p.IsDelete == 1 && guids.Contains(p.ProjectDept)).Select(p => p.Id).ToListAsync();
            var baseplanproject = await dbContext.Queryable<BaseLinePlanProject>()
             .Where(t => t.IsDelete == 1).ToListAsync();

            var projects = await dbContext.Queryable<BaseLinePlanProject>().Where(p => p.IsDelete == 1).WhereIF(requestBody.Year != null, p => p.Year == requestBody.Year).WhereIF(!string.IsNullOrWhiteSpace(requestBody.StartStatus), p => p.StartStatus == requestBody.StartStatus).Select(p => p.Id).ToListAsync();

            if (requestBody.Year == null)
            {
                requestBody.Year = DateTime.Now.Year;
            }

            //var list= dbContext.Queryable<BaseLinePlanAncomparison>().LeftJoin<Project>((b,p)=>b.Code==p.MasterCode).
            var annualProductionShips = await dbContext.Queryable<BaseLinePlanAnnualProductionShips>()
                .Where(t => t.IsDelete == 1).ToListAsync();
            var mainIds = annualProductionShips.Select(x => x.ProjectAnnualProductionId).ToList();

            //var projectList = await dbContext.Queryable<Project>().Where(x => x.IsDelete == 1).Select(x => new Project { Id = x.Id, Name = x.Name, CompanyId = x.CompanyId }).ToListAsync();

            var pPlanProduction = await dbContext.Queryable<BaseLinePlanProjectAnnualPlanProduction>()
                 .Where(t => t.IsDelete == 1 && mainIds.Contains(t.Id) && projects.Contains(t.ProjectId.Value) && t.Year == requestBody.Year)
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
                Converttowanyuan(item);
                item.ProjectName = baseplanproject.Where(p => p.Id == item.ProjectId).FirstOrDefault()?.ShortName;
            }
            rt.Count = pPlanProduction.Count;
            return rt.SuccessResult(pPlanProduction);
        }


        /// <summary>
        /// 去除小数点及转换成万元
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public decimal Setnumericalconversion(decimal? value)
        {
            if (value.GetValueOrDefault() == 0)
            {
                return 0;
            }
            else
            {
                var val = Math.Floor((value.GetValueOrDefault() / 10000) * 100) / 100;

                return System.Convert.ToDecimal(val.ToString("0"));
            }
        }

        /// <summary>
        /// 去除小数点及转换成元
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public decimal Setnumericalconversiontwo(decimal? value)
        {
            if (value.GetValueOrDefault() == 0)
            {
                return 0;
            }
            else
            {
                var val = value.GetValueOrDefault() * 10000;
                return val;
            }
        }

        public async Task<ResponseAjaxResult<bool>> BaseLinePlanProjectAnnualProductionImport(List<BaseLinePlanProjectAnnualProductionImport> imports)
        {
            var responseAjaxResult = new ResponseAjaxResult<bool>();
            try
            {
                List<BaseLinePlanProjectAnnualPlanProduction> addTables = new();
                List<BaseLinePlanAnnualProductionShips> addShipTables = new();
                List<BaseLinePlanProject> addbaseLinePlanProjects = new();

                var project = await dbContext.Queryable<Project>().Where(x => x.IsDelete == 1).ToListAsync();
                var ships = await dbContext.Queryable<OwnerShip>().Where(t => t.IsDelete == 1).ToListAsync();
                var institution = await dbContext.Queryable<Institution>().Where(x => x.IsDelete == 1).ToListAsync();
                var baseLinePlans = await dbContext.Queryable<BaseLinePlanProject>().Where(x => x.IsDelete == 1).ToListAsync();
                List<MarineEquipment> marineEquipment = new List<MarineEquipment>();
                foreach (var group in imports.GroupBy(it => it.ShortName))
                {
                    var baseLine = baseLinePlans.FirstOrDefault(x => x.ShortName == group.Key && x.CompanyId == _currentUser.CurrentLoginInstitutionId);
                    if (baseLine != null)
                    {
                        responseAjaxResult.Data = false;
                        responseAjaxResult.Success(ResponseMessage.OPERATION_COMPANY_IDENTICAL);
                        return responseAjaxResult;
                    }
                    var BaseId = GuidUtil.Next();
                    addbaseLinePlanProjects.Add(new BaseLinePlanProject()
                    {
                        ShortName = group.Key,
                        Id = BaseId,
                        CompanyId = _currentUser.CurrentLoginInstitutionId,
                        Year = group.FirstOrDefault().Year,
                        CreateTime = DateTime.Now,
                    });
                    foreach (var item in group)
                    {
                        if (item.Year == 0)
                        {
                            responseAjaxResult.Data = false;
                            responseAjaxResult.Success("年份不能为空");
                        }

                        if (string.IsNullOrWhiteSpace(item.ShipName))
                        {
                            responseAjaxResult.Data = false;
                            responseAjaxResult.Success("船舶名称不能为空");
                            return responseAjaxResult;
                        }

                        var Ship = ships.FirstOrDefault(x => x.Name == item.ShipName);
                        if (Ship == null)
                        {
                            responseAjaxResult.Data = false;
                            responseAjaxResult.Success(item.ShipName + "船舶名称不存在");
                            return responseAjaxResult;
                        }

                        var id = GuidUtil.Next();
                        addTables.Add(new BaseLinePlanProjectAnnualPlanProduction
                        {
                            JanuaryProductionQuantity = Setnumericalconversiontwo(item.JanuaryProductionQuantity),
                            JanuaryProductionValue = Setnumericalconversiontwo(item.JanuaryProductionValue),
                            FebruaryProductionQuantity = Setnumericalconversiontwo(item.FebruaryProductionQuantity),
                            FebruaryProductionValue = Setnumericalconversiontwo(item.FebruaryProductionValue),
                            MarchProductionQuantity = Setnumericalconversiontwo(item.MarchProductionQuantity),
                            MarchProductionValue = Setnumericalconversiontwo(item.MarchProductionValue),
                            AprilProductionQuantity = Setnumericalconversiontwo(item.AprilProductionQuantity),
                            AprilProductionValue = Setnumericalconversiontwo(item.AprilProductionValue),
                            MayProductionQuantity = Setnumericalconversiontwo(item.MayProductionQuantity),
                            MayProductionValue = Setnumericalconversiontwo(item.MayProductionValue),
                            JuneProductionQuantity = Setnumericalconversiontwo(item.JuneProductionQuantity),
                            JuneProductionValue = Setnumericalconversiontwo(item.JuneProductionValue),
                            JulyProductionQuantity = Setnumericalconversiontwo(item.JulyProductionQuantity),
                            JulyProductionValue = Setnumericalconversiontwo(item.JulyProductionValue),
                            AugustProductionValue = Setnumericalconversiontwo(item.AugustProductionValue),
                            AugustProductionQuantity = Setnumericalconversiontwo(item.AugustProductionQuantity),
                            SeptemberProductionQuantity = Setnumericalconversiontwo(item.SeptemberProductionQuantity),
                            SeptemberProductionValue = Setnumericalconversiontwo(item.SeptemberProductionValue),
                            OctoberProductionQuantity = Setnumericalconversiontwo(item.OctoberProductionQuantity),
                            OctoberProductionValue = Setnumericalconversiontwo(item.OctoberProductionValue),
                            NovemberProductionQuantity = Setnumericalconversiontwo(item.NovemberProductionQuantity),
                            NovemberProductionValue = Setnumericalconversiontwo(item.NovemberProductionValue),
                            DecemberProductionQuantity = Setnumericalconversiontwo(item.DecemberProductionQuantity),
                            DecemberProductionValue = Setnumericalconversiontwo(item.DecemberProductionValue),
                            ProjectId = BaseId,
                            CompanyId = _currentUser.CurrentLoginInstitutionId,
                            Year = item.Year,
                            Id = id
                        });

                        addShipTables.Add(new BaseLinePlanAnnualProductionShips
                        {
                            Id = GuidUtil.Next(),
                            ShipType = Ship != null ? 1 : 2,
                            ShipId = Ship.Id,
                            ShipName = Ship.Name,
                            ProjectAnnualProductionId = id
                        });
                    }
                }
                await dbContext.Insertable(addTables).ExecuteCommandAsync();
                await dbContext.Insertable(addbaseLinePlanProjects).ExecuteCommandAsync();
                await dbContext.Insertable(addShipTables).ExecuteCommandAsync();
                responseAjaxResult.Data = true;
                responseAjaxResult.Success();
                return responseAjaxResult;
            }
            catch
            {
                responseAjaxResult.Data = true;
                responseAjaxResult.Success(ResponseMessage.OPERATION_UPLOAD_FAIL, HttpStatusCode.UploadFail);
                return responseAjaxResult;
            }
        }

        /// <summary>
        /// 基准计划审核
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> BaseLinePlanProjectApproveAsync(int isApprove, string? id)
        {
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            if (!string.IsNullOrWhiteSpace(id))
            {
                var obj = new BaseLinePlanProjectApprove()
                {
                    Id = Guid.NewGuid(),
                    ApproveId = _currentUser.Id,
                    ApproveName = _currentUser.Name,
                    Status = isApprove == 1 ? "已审批" : isApprove == 0 ? "未审批" : "驳回",
                    BaseLinePlanid = id,
                };
                await dbContext.Insertable<BaseLinePlanProjectApprove>(obj).ExecuteCommandAsync();
                var baseLinePlan = await dbContext.Queryable<BaseLinePlanProject>().Where(x => x.Id == id.ToGuid()).FirstAsync();
                var baseLinePlanCount = await dbContext.Queryable<BaseLinePlanProject>().Where(x => x.ShortName.Contains(baseLinePlan.ShortName)).CountAsync();
                baseLinePlan.PlanVersion = baseLinePlan.ShortName + "-" + baseLinePlan.Year + "年基准计划V" + baseLinePlanCount + 1;
                baseLinePlan.PlanStatus = isApprove;
                await dbContext.Updateable<BaseLinePlanProject>(baseLinePlan).ExecuteCommandAsync();
                responseAjaxResult.Success();
            }
            else
            {
                responseAjaxResult.Data = false;
                responseAjaxResult.Success("您没有权限审核", HttpStatusCode.VerifyFail);
                return responseAjaxResult;
            }
            return responseAjaxResult;
        }
    }
}
