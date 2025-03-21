using AutoMapper;
using GHMonitoringCenterApi.Application.Contracts.Dto;
using GHMonitoringCenterApi.Application.Contracts.Dto.CompanyProductionValueInfos;
using GHMonitoringCenterApi.Application.Contracts.Dto.Enums;
using GHMonitoringCenterApi.Application.Contracts.Dto.EquipmentManagement;
using GHMonitoringCenterApi.Application.Contracts.Dto.Job;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project;
using GHMonitoringCenterApi.Application.Contracts.IService;
using GHMonitoringCenterApi.Application.Contracts.IService.Job;
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
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System.ComponentModel.Design;
using Wangkanai.Extensions;


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

        public IBaseRepository<ProjectStatus> projectStatusRepository { get; set; }


        public IJobService _jobService { get; set; }
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
        public BaseLinePlanProjectService(IBaseRepository<ProjectWbsHistoryMonth> baseProjectWbsHistory, IBaseRepository<ProjectWBS> baseProjectWBSRepository, ISqlSugarClient dbContext, IMapper mapper, IBaseRepository<Files> baseFilesRepository, IBaseRepository<ProjectOrg> baseProjectOrgRepository, IBaseRepository<ProjectLeader> baseProjectLeaderRepository, IPushPomService pushPomService, IBaseService baseService, ILogService logService, IEntityChangeService entityChangeService, GlobalObject globalObject, IBaseRepository<Project> baseProjects, IBaseRepository<BaseLinePlanAncomparison> baseLinePlanAncomparisonRepository, IBaseRepository<ProjectStatus> projectStatusRepository, IJobService jobService)
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
            this.projectStatusRepository = projectStatusRepository;
            this._jobService = jobService;
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
                .Where(t => t.IsDelete == 1)
                .ToListAsync();

            var pIds = pPlanProduction.Select(x => x.ProjectId).ToList();

            var baseplanproject = await dbContext.Queryable<BaseLinePlanProject>()
               .Where(t => t.IsDelete == 1 && t.Id == requestBody.ProjectId).FirstAsync();

            //项目基本信息
            var projects = await dbContext.Queryable<Project>().Where(t => t.IsDelete == 1 && pIds.Contains(t.Id)).Select(x => new { x.Id, x.CurrencyId, x.ShortName, x.ECAmount, x.ContractStipulationEndDate }).ToListAsync();


            var projectStatus = await dbContext.Queryable<ProjectStatus>().Where(p => p.IsDelete == 1).ToListAsync();

            var ships = await dbContext.Queryable<OwnerShip>().Where(t => t.IsDelete == 1).ToListAsync();

            var sums = await dbContext.Queryable<BaseLinePlanProjectAnnualPlanProduction>()
                .Where(t => t.IsDelete == 1 && t.ProjectId == requestBody.ProjectId).Select(p => new BaseLinePlanProjectAnnualPlanProduction
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
                if (baseplanproject != null)
                {
                    if (!string.IsNullOrWhiteSpace(baseplanproject.Association))
                    {
                        //关联项目 MasterCode为导入时关联项目 
                        var pp = await dbContext.Queryable<Project>().FirstAsync(x => x.MasterCode == baseplanproject.Association && x.IsDelete == 1);
                        if (pp == null)
                        {
                            //关联项目 Association为界面关联项目 
                            pp = await dbContext.Queryable<Project>().FirstAsync(x => x.Association == baseplanproject.Id.ToString() && x.IsDelete == 1);
                        }
                        if (pp != null)
                        {
                            rr.AssociationName = pp.ShortName;
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

                            rr.EffectiveAmount = System.Convert.ToDecimal(pp.ECAmount);
                            rr.ContractStipulationEndDate = pp.ContractStipulationEndDate?.ToString("yyyy-MM-dd");
                            rr.RemainingAmount = System.Convert.ToDecimal(pp.ECAmount) - completeAmount;
                            //rr.AccumulatedOutputValue = accumulatedOutputValue;
                            //rr.AccumulatedOutputValue = sum;
                        }
                    }
                }
                else if (baseplanproject != null)
                {
                    rr.EffectiveAmount = baseplanproject.EffectiveAmount;
                    rr.RemainingAmount = baseplanproject.RemainingAmount;
                }

                rr.AccumulatedOutputValue = Setnumericalconversion(sum);
                if (baseplanproject != null)
                {
                    //rr.PlanVersion = baseplanproject.PlanVersion;
                    rr.PlanType = baseplanproject.PlanType;
                    rr.StartStatus = baseplanproject.StartStatus;
                    if (!string.IsNullOrWhiteSpace(rr.StartStatus))
                    {
                        var status = projectStatus.Where(p => p.StatusId == rr.StartStatus.ToGuid()).FirstOrDefault();
                        if (status != null)
                        {
                            rr.StartStatusName = status.Name;
                        }
                    }
                    rr.Association = baseplanproject.Association;
                    rr.CompletionTime = baseplanproject.CompletionTime;
                    rr.IsSubPackage = baseplanproject.IsSubPackage;
                    rr.PlanStatus = baseplanproject.PlanStatus;
                    rr.PlanStatusText = baseplanproject.PlanStatus == 0 ? "已保存" : baseplanproject.PlanStatus == 1 ? "驳回" : baseplanproject.PlanStatus == 2 ? "审核通过" : baseplanproject.PlanStatus == 4 ? "待项目审核" : baseplanproject.PlanStatus == 5 ? "待公司审核" : "撤回";
                    rr.ProjectName = baseplanproject.ShortName;
                    rr.SubmitStatus = baseplanproject.SubmitStatus.GetValueOrDefault();
                    rr.RejectReason = baseplanproject.RejectReason;

                    if (baseplanproject.EffectiveAmount > 0)
                    {
                        rr.EffectiveAmount = baseplanproject.EffectiveAmount;
                    }

                    if (baseplanproject.RemainingAmount > 0)
                    {
                        rr.EffectiveAmount = baseplanproject.EffectiveAmount;
                    }

                    if (baseplanproject.CompletionTime != null)
                    {
                        rr.CompletionTime = baseplanproject.CompletionTime;
                    }

                    var job = await dbContext.Queryable<Domain.Models.Job>().Where(p => p.ProjectId == baseplanproject.Id && p.IsFinish == false).FirstAsync();
                    if (job != null)
                    {
                        rr.JobId = job.Id;
                        var approver = await dbContext.Queryable<JobApprover>().Where(p => p.JobId == job.Id && p.ApproverJobStatus == JobStatus.UnHandle).FirstAsync();
                        if (approver != null)
                        {
                            rr.ApproverId = approver.ApproverId;
                            var user = await dbContext.Queryable<Domain.Models.User>().Where(p => p.Id == approver.ApproverId).FirstAsync();
                            rr.ApproverName = user?.Name;

                            //rr.ApproveLevel = approver.ApproveLevel;
                            if (approver.ApproverId == _currentUser.Id)
                            {
                                rr.HasReview = true;
                                if (approver.ApproveLevel < ApproveLevel.Level2)
                                {
                                    rr.NextApprovers = dbContext.Queryable<BaseLinePlanApprover>().Where(p => p.CompanyId == _currentUser.CurrentLoginInstitutionId && p.ApproveLevel == ApproveLevel.Level2).Select(p => new SaveJobApproverRequestDto()
                                    {
                                        ApproveLevel = p.ApproveLevel,
                                        ApproverId = p.ApproverId
                                    }).ToArray();
                                }
                            }
                        }
                    }
                }
            }

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
                .Where(t => t.IsDelete == 1)
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
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> SaveBaseLinePlanProjectAnnualProductionAsync(SaveBaseLinePlanProjectDto? input)
        {
            input.SubmitStatus = 0;
            //input.PlanStatus = 0;
            return await SaveBaseLinePlanProjectAsync(input);
        }


        #region 保存数据
        private async Task<ResponseAjaxResult<bool>> SaveBaseLinePlanProjectAsync(SaveBaseLinePlanProjectDto? input)
        {
            ResponseAjaxResult<bool> rt = new();
            List<BaseLinePlanProjectAnnualPlanProduction> addTables = new();
            List<BaseLinePlanProjectAnnualPlanProduction> upTables = new();
            List<BaseLinePlanAnnualProductionShips> addShipTables = new();
            List<Guid> Ids = new();

            if (input != null)
            {
                var tables = await dbContext.Queryable<BaseLinePlanProjectAnnualPlanProduction>().Where(t => t.IsDelete == 1 && input.AnnualProductions.Select(x => x.Id).Contains(t.Id)).ToListAsync();
                var baseid = GuidUtil.Next();
                BaseLinePlanProject baseLinePlanProject = null;
                var baselinefirbaseLinePlanProject = await dbContext.Queryable<BaseLinePlanProject>().Where(p => p.Id == input.ProjectId).FirstAsync();

                //是否基准计划填报期间
                var isbaseline = await dbContext.Queryable<DictionaryTable>().Where(p => p.Type == 12).FirstAsync();
                if (baselinefirbaseLinePlanProject != null)
                {
                    if (isbaseline != null && isbaseline.Name == "1")
                    {
                        baselinefirbaseLinePlanProject.PlanType = "1";
                    }
                    baselinefirbaseLinePlanProject.ShortName = input.ShortName;
                    //baselinefirbaseLinePlanProject.CompanyId = _currentUser.CurrentLoginInstitutionId;
                    baselinefirbaseLinePlanProject.Year = input.Year;
                    baselinefirbaseLinePlanProject.StartStatus = input.StartStatus;
                    baselinefirbaseLinePlanProject.ProjectId = input.ProjectId;
                    baselinefirbaseLinePlanProject.Association = input.Association;
                    if (input.CompletionTime != null)
                    {
                        baselinefirbaseLinePlanProject.CompletionTime = input.CompletionTime;
                    }
                    baselinefirbaseLinePlanProject.EffectiveAmount = input.EffectiveAmount;
                    baselinefirbaseLinePlanProject.IsSubPackage = input.IsSubPackage;
                    baselinefirbaseLinePlanProject.RemainingAmount = input.RemainingAmount;
                    baselinefirbaseLinePlanProject.SubmitStatus = input.SubmitStatus;
                    baselinefirbaseLinePlanProject.PlanStatus = input.PlanStatus.GetValueOrDefault();
                    input.ProjectId = baselinefirbaseLinePlanProject.Id;
                    await GetPlanVersion(baselinefirbaseLinePlanProject);
                    await dbContext.Updateable(baselinefirbaseLinePlanProject).ExecuteCommandAsync();
                }
                else
                {
                    baseLinePlanProject = new BaseLinePlanProject();
                    baseLinePlanProject.Id = baseid;
                    baseLinePlanProject.CompanyId = _currentUser.CurrentLoginInstitutionId;
                    baseLinePlanProject.Year = input.Year;
                    baseLinePlanProject.StartStatus = input.StartStatus;
                    baseLinePlanProject.ProjectId = input.ProjectId;
                    baseLinePlanProject.Association = input.Association;
                    baseLinePlanProject.CompletionTime = input.CompletionTime;
                    baseLinePlanProject.EffectiveAmount = input.EffectiveAmount;
                    baseLinePlanProject.IsSubPackage = input.IsSubPackage;
                    baseLinePlanProject.RemainingAmount = input.RemainingAmount;
                    baseLinePlanProject.ShortName = input.ShortName;
                    baseLinePlanProject.SubmitStatus = input.SubmitStatus;
                    baseLinePlanProject.PlanStatus = input.PlanStatus.GetValueOrDefault();
                    if (isbaseline != null && isbaseline.Name == "1")
                    {
                        baseLinePlanProject.PlanType = "1";
                    }
                    await GetPlanVersion(baseLinePlanProject, 1);
                    await dbContext.Insertable(baseLinePlanProject).ExecuteCommandAsync();
                    input.ProjectId = baseid;
                }

                foreach (var item in input.AnnualProductions)
                {

                    var existids = dbContext.Queryable<BaseLinePlanProjectAnnualPlanProduction>().Where(p => p.ProjectId == input.ProjectId).Select(p => p.Id).ToList();
                    if (existids.Count > 0)
                    {
                        await dbContext.Deleteable<BaseLinePlanAnnualProductionShips>().Where(p => existids.Contains(p.ProjectAnnualProductionId)).ExecuteCommandAsync();

                        await dbContext.Deleteable<BaseLinePlanProjectAnnualPlanProduction>().Where(p => existids.Contains(p.Id)).ExecuteCommandAsync();
                    }

                    //if (!string.IsNullOrWhiteSpace(item.Id.ToString()) && item.Id != Guid.Empty)
                    //{
                    //    var first = tables.FirstOrDefault(x => x.Id == item.Id);
                    //    if (first != null)
                    //    {
                    //        Ids.Add(item.Id.Value);

                    //        first.ProjectId = baseid;
                    //        first.CompanyId = _currentUser.CurrentLoginInstitutionId;
                    //        first.Year = DateTime.Now.Year;
                    //        first.Id = GuidUtil.Next();
                    //        first.JanuaryProductionQuantity = Setnumericalconversiontwo(item.JanuaryProductionQuantity);
                    //        first.JanuaryProductionValue = Setnumericalconversiontwo(item.JanuaryProductionValue);
                    //        first.FebruaryProductionQuantity = Setnumericalconversiontwo(item.FebruaryProductionQuantity);
                    //        first.FebruaryProductionValue = Setnumericalconversiontwo(item.FebruaryProductionValue);
                    //        first.MarchProductionQuantity = Setnumericalconversiontwo(item.MarchProductionQuantity);
                    //        first.MarchProductionValue = Setnumericalconversiontwo(item.MarchProductionValue);
                    //        first.AprilProductionQuantity = Setnumericalconversiontwo(item.AprilProductionQuantity);
                    //        first.AprilProductionValue = Setnumericalconversiontwo(item.AprilProductionValue);
                    //        first.MayProductionQuantity = Setnumericalconversiontwo(item.MayProductionQuantity);
                    //        first.MayProductionValue = Setnumericalconversiontwo(item.MayProductionValue);
                    //        first.JuneProductionQuantity = Setnumericalconversiontwo(item.JuneProductionQuantity);
                    //        first.JuneProductionValue = Setnumericalconversiontwo(item.JuneProductionValue);
                    //        first.JulyProductionQuantity = Setnumericalconversiontwo(item.JulyProductionQuantity);
                    //        first.JulyProductionValue = Setnumericalconversiontwo(item.JulyProductionValue);
                    //        first.AugustProductionValue = Setnumericalconversiontwo(item.AugustProductionValue);
                    //        first.AugustProductionQuantity = Setnumericalconversiontwo(item.AugustProductionQuantity);
                    //        first.SeptemberProductionQuantity = Setnumericalconversiontwo(item.SeptemberProductionQuantity);
                    //        first.SeptemberProductionValue = Setnumericalconversiontwo(item.SeptemberProductionValue);
                    //        first.OctoberProductionQuantity = Setnumericalconversiontwo(item.OctoberProductionQuantity);
                    //        first.OctoberProductionValue = Setnumericalconversiontwo(item.OctoberProductionValue);
                    //        first.NovemberProductionQuantity = Setnumericalconversiontwo(item.NovemberProductionQuantity);
                    //        first.NovemberProductionValue = Setnumericalconversiontwo(item.NovemberProductionValue);
                    //        first.DecemberProductionQuantity = Setnumericalconversiontwo(item.DecemberProductionQuantity);
                    //        first.DecemberProductionValue = Setnumericalconversiontwo(item.DecemberProductionValue);
                    //        addTables.Add(first);

                    //        foreach (var ship in item.AnnualProductionShips)
                    //        {
                    //            addShipTables.Add(new BaseLinePlanAnnualProductionShips
                    //            {
                    //                Id = GuidUtil.Next(),
                    //                ShipType = ship.ShipType,
                    //                ShipId = ship.ShipId,
                    //                ShipName = ship.ShipName,
                    //                ProjectAnnualProductionId = first.Id
                    //            });
                    //        }
                    //    }
                    //}
                    //else
                    //{
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
                        ProjectId = input.ProjectId,
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
                    //    }
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
        #endregion

        /// <summary>
        /// 提交基准计划
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> SubmitBaseLinePlanProjectAnnualProductionAsync(SubmitBaseLinePlanProjectDto? input)
        {
            input.SubmitStatus = 1;
            input.PlanStatus = 4;
            var response = await SaveBaseLinePlanProjectAsync(input);
            if (response.Data == true)
            {
                var Approvers = await dbContext.Queryable<BaseLinePlanApprover>().Where(x => x.CompanyId == _currentUser.CurrentLoginInstitutionId && x.BizModule == BizModule.BaseLinePlan && x.ApproveLevel == ApproveLevel.Level2).Select(p => new BaseLinePlanApprover
                {
                    CompanyId = p.CompanyId,
                    ApproveLevel = p.ApproveLevel,
                    ApproverId = p.ApproverId,
                    BizModule = p.BizModule
                }).FirstAsync();
                if (Approvers != null)
                {
                    SaveJobApproverRequestDto dto = new SaveJobApproverRequestDto();
                    dto.ApproverId = Approvers.ApproverId;
                    dto.ApproveLevel = Approvers.ApproveLevel;
                    if (input.submitJob.Approvers.Length == 0)
                    {
                        input.submitJob.Approvers = new SaveJobApproverRequestDto[] { dto };
                    }
                }
                //var baseline = await dbContext.Queryable<BaseLinePlanProject>().Where(p => p.Id == input.ProjectId).FirstAsync();
                //input.submitJob.BizData = baseline;
                input.submitJob.JobId = GuidUtil.Next();
                input.ProjectId = input.ProjectId;
                input.submitJob.SubmitType = JobSubmitType.AddJob;
                await _jobService.SubmitJobAsync(input.submitJob);
            }
            return response;
        }

        /// <summary>
        /// 生成计划版本
        /// </summary>
        /// <param name="baseLinePlanProject"></param>
        /// <returns></returns>
        private async Task GetPlanVersion(BaseLinePlanProject baseLinePlanProject, int isadd = 0)
        {
            if (!string.IsNullOrWhiteSpace(baseLinePlanProject.PlanVersion) && !string.IsNullOrWhiteSpace(baseLinePlanProject.PlanVersion.Replace("-基准计划", "")) && isadd != 0)
            {
                return;
            }

            var baseLinestatus = await dbContext.Queryable<DictionaryTable>().Where(x => x.TypeNo == 12 && x.IsDelete == 0).FirstAsync();
            string planname = "基准计划";
            if (baseLinestatus != null && baseLinestatus.Name == "1")
            {
                planname = "新建计划";
            }

            if (!string.IsNullOrWhiteSpace(baseLinePlanProject.Association))
            {
                var project = await dbContext.Queryable<Project>().Where(p => p.MasterCode == baseLinePlanProject.Association).FirstAsync();
                if (project != null)
                {
                    var baseLinePlan = await dbContext.Queryable<BaseLinePlanProject>().Where(x => x.Year == baseLinePlanProject.Year && x.PlanVersion.StartsWith(project.ShortName)).CountAsync();
                    baseLinePlanProject.PlanVersion = project.ShortName.Replace("-基准计划", "") + "-" + planname + "V" + (baseLinePlan + 1);
                }
                else
                {
                    var baseLinePlan = await dbContext.Queryable<BaseLinePlanProject>().Where(x => x.Year == baseLinePlanProject.Year && x.ShortName == baseLinePlanProject.ShortName).CountAsync();
                    if (!string.IsNullOrWhiteSpace(baseLinePlanProject.ShortName))
                    {
                        baseLinePlanProject.PlanVersion = baseLinePlanProject.ShortName.Replace("-基准计划", "") + "-" + planname + "V" + (baseLinePlan + 1);
                    }
                }
            }
            else
            {
                var baseLinePlan = await dbContext.Queryable<BaseLinePlanProject>().Where(x => x.Year == baseLinePlanProject.Year && x.ShortName == baseLinePlanProject.ShortName).CountAsync();
                if (!string.IsNullOrWhiteSpace(baseLinePlanProject.ShortName))
                {
                    baseLinePlanProject.PlanVersion = baseLinePlanProject.ShortName.Replace("-基准计划", "") + "-" + planname + "V" + (baseLinePlan + 1);
                }
            }
        }

        /// <summary>
        /// 基础信息 自有船舶
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

            var typeids = new List<Guid>()
            {
                "06b7a5ce-e105-46c8-8b1d-24c8ba7f9dbf".ToGuid(), //耙吸船
                "1f2e628a-9d95-40e7-acc6-68b84e992d47".ToGuid(),// 环保绞吸船
                "f1718922-c213-4409-a59f-fdaf3d6c5e23".ToGuid(),//绞吸船
                "6959792d-27a4-4f2b-8fa4-a44222f08cb2".ToGuid() //抓斗船
            };

            var owns = await dbContext.Queryable<OwnerShip>().Where(x => x.IsDelete == 1 && typeids.Contains(x.TypeId.Value))
                .Select(x => new BaseLinePlanShipsForAnnualProduction
                {
                    Id = x.PomId,
                    Name = x.Name,
                    SubOrOwn = 1
                }).ToListAsync();

            //var subs = await dbContext.Queryable<SubShip>().Where(x => x.IsDelete == 1)
            //    .Select(x => new BaseLinePlanShipsForAnnualProduction
            //    {
            //        Id = x.PomId,
            //        Name = x.Name,
            //        SubOrOwn = 2
            //    }).ToListAsync();
            ownShipsForAnnualProductions.AddRange(owns);

            ownShipsForAnnualProductions.Add(new BaseLinePlanShipsForAnnualProduction()
            {
                Id = "f86a0717-86f2-4695-a472-86204dd6adac".ToGuid(),
                Name = "其他",
                SubOrOwn = 0
            });

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

            var userInfo = _currentUser;
            //公司ID
            var companyId = await dbContext.Queryable<Institution>().Where(p => p.IsDelete == 1 && p.Oid == userInfo.CurrentLoginInstitutionOid).Select(x => x.PomId.Value).FirstAsync();

            List<Guid> userIds = new List<Guid>();
            userIds.Add("08db4e0d-a531-4619-8fae-d97e57eeb375".ToGuid());


            var projects = await dbContext.Queryable<BaseLinePlanProject>()
                .Where(p => p.IsDelete == 1 && guids.Contains(p.CompanyId))
                //.WhereIF(userIds[0] != userInfo.Id, x => x.CompanyId == companyId)
                .WhereIF(requestBody.Year != null, p => p.Year == requestBody.Year)
                .WhereIF(!string.IsNullOrWhiteSpace(requestBody.StartStatus), p => p.StartStatus == requestBody.StartStatus)
                .WhereIF(requestBody.CompanyId != null, p => p.CompanyId == requestBody.CompanyId)
                .Select(p => p.Id).ToListAsync();


            if (requestBody.Year == null)
            {
                requestBody.Year = DateTime.Now.Year;
            }
            var intitutionList = dbContext.Queryable<Institution>().Where(x => x.IsDelete == 1);
            //var list= dbContext.Queryable<BaseLinePlanAncomparison>().LeftJoin<Project>((b,p)=>b.Code==p.MasterCode).
            RefAsync<int> total = 0;
            var annualProductionShips = await dbContext.Queryable<BaseLinePlanAnnualProductionShips>()
                .Where(t => t.IsDelete == 1).ToListAsync();
            var mainIds = annualProductionShips.Select(x => x.ProjectAnnualProductionId).ToList();

            //var projectList = await dbContext.Queryable<Project>().Where(x => x.IsDelete == 1).Select(x => new Project { Id = x.Id, Name = x.Name, CompanyId = x.CompanyId }).ToListAsync();

            var pPlanProduction = await dbContext.Queryable<BaseLinePlanProjectAnnualPlanProduction>()
                 .Where(t => t.IsDelete == 1 && projects.Contains(t.ProjectId.Value) && t.Year == requestBody.Year)
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
                 .ToPageListAsync(requestBody.PageIndex, requestBody.PageSize, total);

            foreach (var item in pPlanProduction)
            {
                Converttowanyuan(item);

                //item.ProjectName = baseplanproject.Where(p => p.Id == item.ProjectId).FirstOrDefault()?.ShortName;
                var baseline = baseplanproject.Where(p => p.Id == item.ProjectId).FirstOrDefault();
                if (baseline != null)
                {
                    item.CompanyId = baseline.CompanyId;
                    item.CompanyName = intitutionList.SingleAsync(x => x.PomId == item.CompanyId).Result.Name;
                    item.ProjectName = baseline.PlanVersion;
                    item.Id = baseline.Id;
                    item.PlanStatus = baseline.PlanStatus;
                    item.PlanStatusStr = baseline.PlanStatus == 0 ? "已保存" : baseline.PlanStatus == 1 ? "驳回" : baseline.PlanStatus == 2 ? "审核通过" : baseline.PlanStatus == 4 ? "待项目审核" : baseline.PlanStatus == 5 ? "待公司审核" : "撤回";
                }
            }
            rt.Count = total;
            rt.Data = pPlanProduction;
            rt.Success();
            return rt;
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

        #region  废弃 项目汇总
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
        #endregion


        /// <summary>
        /// 基准计划项目界面 局工程部界面 
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<SearchSubsidiaryCompaniesProjectProductionDto>>> SearchBureauProjectProductionAsync(SearchBaseLinePlanProjectAnnualProductionRequest requestBody)
        {
            List<SearchSubsidiaryCompaniesProjectProductionDto> resultlist = new List<SearchSubsidiaryCompaniesProjectProductionDto>();
            ResponseAjaxResult<List<SearchSubsidiaryCompaniesProjectProductionDto>> rt = new();


            var userInfo = _currentUser;
            //公司ID
            var companyId = await dbContext.Queryable<Institution>().Where(p => p.IsDelete == 1 && p.Oid == userInfo.CurrentLoginInstitutionOid).Select(x => x.PomId.Value).FirstAsync();

            List<Guid> userIds = new List<Guid>();
            userIds.Add("08db4e0d-a531-4619-8fae-d97e57eeb375".ToGuid());
            userIds.Add("08db3bbb-7e36-4e20-8024-ee9c9bc516e3".ToGuid());



            var baseplanproject = await dbContext.Queryable<BaseLinePlanProject>()
           .Where(t => t.IsDelete == 1)
            //.WhereIF(!userIds.Contains(userInfo.Id), x => x.CompanyId == companyId).
            .WhereIF(requestBody.CompanyId != null, x => x.CompanyId == requestBody.CompanyId)
           .WhereIF(!string.IsNullOrWhiteSpace(requestBody.StartStatus), t => t.StartStatus == requestBody.StartStatus).
           WhereIF(requestBody.Year != null, t => t.Year == requestBody.Year).ToListAsync();

            List<Guid> baseplanprojectIds = new List<Guid>();
            baseplanprojectIds = baseplanproject
                .Select(t => t.Id).ToList();

            var annualProductionShips = await dbContext.Queryable<BaseLinePlanAnnualProductionShips>()
                .Where(t => t.IsDelete == 1).ToListAsync();
            var intitutionList = dbContext.Queryable<Institution>().Where(x => x.IsDelete == 1);
            var mainIds = annualProductionShips.Select(x => x.ProjectAnnualProductionId).ToList();

            if (requestBody.Year == null)
            {
                requestBody.Year = DateTime.Now.Year;
            }
            RefAsync<int> total = 0;
            var projectList = await dbContext.Queryable<Project>()
                .Where(x => x.IsDelete == 1)
                .Select(x => new Project { Id = x.Id, Name = x.Name, CompanyId = x.CompanyId }).ToListAsync();

            var pPlanProduction = await dbContext.Queryable<BaseLinePlanProjectAnnualPlanProduction>()
                 .Where(t => t.IsDelete == 1 && t.Year == requestBody.Year && baseplanprojectIds.Contains(t.ProjectId.Value))
                 .WhereIF(requestBody.CompanyId != null, p => p.CompanyId == requestBody.CompanyId)
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
                     DecemberProductionValue = SqlFunc.AggregateSum(it.DecemberProductionValue),
                     LatestDate = SqlFunc.AggregateMax(it.CreateTime)
                 }).OrderByDescending(p => p.LatestDate)
                .ToPageListAsync(requestBody.PageIndex, requestBody.PageSize, total);

            var jobs = await dbContext.Queryable<Domain.Models.Job>().Where(p => baseplanprojectIds.Contains(p.ProjectId) && p.IsFinish == false).ToListAsync();
            var approvers = await dbContext.Queryable<JobApprover>().Where(p => jobs.Select(p => p.Id).ToList().Contains(p.JobId)).ToListAsync();
            foreach (var item in pPlanProduction)
            {
                Converttowanyuan(item);
                var baseline = baseplanproject.Where(p => p.Id == item.ProjectId).FirstOrDefault();
                if (baseline != null)
                {

                    if (item.CompanyId != Guid.Empty)
                    {
                        item.CompanyName = intitutionList.SingleAsync(x => x.PomId == item.CompanyId).Result.Name;
                    }
                    item.Association = baseline.Association;
                    item.ProjectName = baseline.ShortName;
                    item.PlanVersion = baseline.PlanVersion;
                    item.Id = baseline.Id;
                    item.PlanStatus = baseline.PlanStatus;
                    //item.PlanStatusStr = item.PlanStatusStr = baseline.PlanStatus == 0 ? "已保存" : baseline.PlanStatus == 1 ? "驳回" : baseline.PlanStatus == 2 ? "审核通过" : baseline.PlanStatus == 4 ? "待项目审核" : baseline.PlanStatus == 5 ? "待公司审核" : "撤回";
                    item.CompanyId = baseline.CompanyId;
                    item.SubmitStatus = baseline.SubmitStatus;
                    //item.HasEdit = baseline.CreateId == _currentUser.Id ? true : false;
                    item.RejectReason = baseline.RejectReason;

                    item.PlanStatusStr = baseline.PlanStatus == 0 ? "已保存" : baseline.PlanStatus == 1 ? "驳回" : baseline.PlanStatus == 2 ? "审核通过" : baseline.PlanStatus == 4 ? "待项目审核" : baseline.PlanStatus == 5 ? "待公司审核" : "撤回";
                    item.HasEdit = baseline.CreateId == _currentUser.Id ? true : false;
                    //item.CompanyName = intitutionList.SingleAsync(x => x.PomId == item.CompanyId).Result.Name;
                    item.ProjectName = !string.IsNullOrEmpty(baseline.Association) ? baseline.PlanVersion : !string.IsNullOrWhiteSpace(baseline.PlanVersion) ? baseline.PlanVersion : baseline.ShortName;
                }

                item.IsAssociationbe = !string.IsNullOrWhiteSpace(item.Association) ? true : false;


                var job = jobs.Where(p => p.ProjectId == item.ProjectId && p.IsFinish == false).FirstOrDefault();
                if (job != null)
                {
                    item.JobId = job.Id;
                    var approver = approvers.Where(p => p.JobId == job.Id && p.ApproverJobStatus == JobStatus.UnHandle).FirstOrDefault();
                    if (approver != null)
                    {
                        item.ApproverId = approver.ApproverId;
                        var user = await dbContext.Queryable<Domain.Models.User>().Where(p => p.Id == approver.ApproverId).FirstAsync();
                        item.ApproverName = user?.Name;
                        //item.ApproveLevel = approver.ApproveLevel;
                        if (approver.ApproverId == _currentUser.Id)
                        {
                            item.HasReview = true;
                            if (approver.ApproveLevel < ApproveLevel.Level2)
                            {
                                item.NextApprovers = dbContext.Queryable<BaseLinePlanApprover>().Where(p => p.CompanyId == _currentUser.CurrentLoginInstitutionId && p.ApproveLevel == ApproveLevel.Level2).Select(p => new SaveJobApproverRequestDto()
                                {
                                    ApproveLevel = p.ApproveLevel,
                                    ApproverId = p.ApproverId
                                }).ToArray();
                            }
                        }
                    }
                }
            }
            rt.Count = total;
            rt.Data = pPlanProduction;
            rt.Success();
            return rt;
        }



        #region  局工程部界面 项目合计 废弃
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
        #endregion

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

            //用户信息
            var userInfo = _currentUser;
            //公司ID
            var companyId = await dbContext.Queryable<Institution>().Where(p => p.IsDelete == 1 && p.Oid == userInfo.CurrentLoginInstitutionOid).Select(x => x.PomId.Value).FirstAsync();

            List<Guid> userIds = new List<Guid>();
            userIds.Add("08db4e0d-a531-4619-8fae-d97e57eeb375".ToGuid());
            userIds.Add("08db3bbb-7e36-4e20-8024-ee9c9bc516e3".ToGuid());


            var baseplanproject = await dbContext.Queryable<BaseLinePlanProject>().Where(t => t.IsDelete == 1)
                 .WhereIF(!string.IsNullOrWhiteSpace(requestBody.PlanType), p => p.PlanType == requestBody.PlanType)
                 .WhereIF(!userIds.Contains(userInfo.Id), x => x.CompanyId == companyId)
                 .WhereIF(!string.IsNullOrWhiteSpace(requestBody.StartStatus), p => p.StartStatus == requestBody.StartStatus)
         .ToListAsync();
            RefAsync<int> total = 0;
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
                 .Where(t => t.IsDelete == 1 && baseplanprojectIds.Contains(t.ProjectId.Value) && t.Year == requestBody.Year)
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
                 .ToPageListAsync(requestBody.PageIndex, requestBody.PageSize, total);


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
            rt.Count = total;
            rt.Data = pPlanProduction;
            rt.Success();
            return rt;
        }



        #region  废弃
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
        #endregion

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
            ResponseAjaxResult<List<SearchSubsidiaryCompaniesProjectProductionDto>> rt = await GetBaseLinePlanList(requestBody);
            return rt;
        }

        private async Task<ResponseAjaxResult<List<SearchSubsidiaryCompaniesProjectProductionDto>>> GetBaseLinePlanList(BaseLinePlanAncomparisonRequsetDto requestBody)
        {
            List<SearchSubsidiaryCompaniesProjectProductionDto> resultlist = new List<SearchSubsidiaryCompaniesProjectProductionDto>();
            ResponseAjaxResult<List<SearchSubsidiaryCompaniesProjectProductionDto>> rt = new();
            //用户信息
            var userInfo = _currentUser;
            //公司ID
            var companyId = await dbContext.Queryable<Institution>().Where(p => p.IsDelete == 1 && p.Oid == userInfo.CurrentLoginInstitutionOid).Select(x => x.PomId.Value).FirstAsync();

            var baseplanproject = await dbContext.Queryable<BaseLinePlanProject>()
             .Where(t => t.IsDelete == 1).ToListAsync();
            List<Guid> userIds = new List<Guid>();
            userIds.Add("08db4e0d-a531-4619-8fae-d97e57eeb375".ToGuid());
            userIds.Add("08db3bbb-7e36-4e20-8024-ee9c9bc516e3".ToGuid());

            RefAsync<int> total = 0;

            var projects = await dbContext.Queryable<BaseLinePlanProject>().Where(p => p.IsDelete == 1)
                .WhereIF(requestBody.Year != null, p => p.Year == requestBody.Year)
                //.WhereIF(!userIds.Contains(userInfo.Id), x => x.CompanyId == companyId)
                .WhereIF(!string.IsNullOrWhiteSpace(requestBody.Name), p => p.ShortName.Contains(requestBody.Name))
                //.WhereIF(requestBody.PlanStatus != null, p => p.PlanStatus == requestBody.PlanStatus)
                .WhereIF(requestBody.IsSubPackage != null, p => p.IsSubPackage == requestBody.IsSubPackage)
                .ToListAsync();


            List<Guid> baseplanprojectIds = new List<Guid>();
            baseplanprojectIds = projects
                .Select(t => t.Id).ToList();

            if (requestBody.Year == null)
            {
                requestBody.Year = DateTime.Now.Year;
            }

            //var list= dbContext.Queryable<BaseLinePlanAncomparison>().LeftJoin<Project>((b,p)=>b.Code==p.MasterCode).
            var annualProductionShips = await dbContext.Queryable<BaseLinePlanAnnualProductionShips>()
                .Where(t => t.IsDelete == 1).ToListAsync();
            var mainIds = annualProductionShips.Select(x => x.ProjectAnnualProductionId).ToList();
            var intitutionList = dbContext.Queryable<Institution>().Where(x => x.IsDelete == 1);
            //var projectList = await dbContext.Queryable<Project>().Where(x => x.IsDelete == 1).Select(x => new Project { Id = x.Id, Name = x.Name, CompanyId = x.CompanyId }).ToListAsync();

            var pPlanProduction = dbContext.Queryable<BaseLinePlanProjectAnnualPlanProduction>()
                 .Where(t => t.IsDelete == 1 && baseplanprojectIds.Contains(t.ProjectId.Value) && t.Year == requestBody.Year)
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
                     DecemberProductionValue = SqlFunc.AggregateSum(it.DecemberProductionValue),
                     LatestDate = SqlFunc.AggregateMax(it.CreateTime)
                 }).MergeTable().OrderByDescending(p => p.LatestDate);

            var pPlanProductionList = new List<SearchSubsidiaryCompaniesProjectProductionDto>();
            if (!requestBody.IsFullExport)
            {
                pPlanProductionList = await pPlanProduction.ToPageListAsync(requestBody.PageIndex, requestBody.PageSize, total);
            }
            else
            {
                pPlanProductionList = await pPlanProduction.ToListAsync();
            }
            if (pPlanProductionList.Count == 0 && _currentUser.CurrentLoginUserType == 3)
            {
                if (!requestBody.IsFullExport)
                {
                    pPlanProductionList = await pPlanProduction.ToPageListAsync(requestBody.PageIndex, requestBody.PageSize, total);
                }
                else
                {
                    pPlanProductionList = await pPlanProduction.ToListAsync();
                }
            }


            var jobs = await dbContext.Queryable<Domain.Models.Job>().Where(p => pPlanProductionList.Select(p => p.ProjectId).ToList().Contains(p.ProjectId) && p.IsFinish == false).ToListAsync();
            var approvers = await dbContext.Queryable<JobApprover>().Where(p => jobs.Select(p => p.Id).ToList().Contains(p.JobId)).ToListAsync();
            foreach (var item in pPlanProductionList)
            {
                Converttowanyuan(item);
                var baseline = baseplanproject.Where(p => p.Id == item.ProjectId).FirstOrDefault();
                if (baseline != null)
                {
                    item.CompanyId = baseline.CompanyId;
                    if (item.CompanyId != Guid.Empty)
                    {
                        item.CompanyName = intitutionList.SingleAsync(x => x.PomId == item.CompanyId).Result.Name;
                    }
                    item.Association = baseline.Association;
                    item.ProjectName = !string.IsNullOrEmpty(baseline.Association) ? baseline.PlanVersion : !string.IsNullOrWhiteSpace(baseline.PlanVersion) ? baseline.PlanVersion : baseline.ShortName;
                    item.PlanVersion = baseline.PlanVersion;
                    item.Id = baseline.Id;
                    item.PlanStatus = baseline.PlanStatus;
                    item.PlanStatusStr = item.PlanStatusStr = baseline.PlanStatus == 0 ? "已保存" : baseline.PlanStatus == 1 ? "驳回" : baseline.PlanStatus == 2 ? "审核通过" : baseline.PlanStatus == 4 ? "待项目审核" : baseline.PlanStatus == 5 ? "待公司审核" : "撤回";
                    item.SubmitStatus = baseline.SubmitStatus;
                    item.HasEdit = baseline.CreateId == _currentUser.Id ? true : _currentUser.CurrentLoginUserType == 2 && item.CompanyId == companyId ? true : false;
                    item.RejectReason = baseline.RejectReason;
                    item.IsAssociationbe = !string.IsNullOrWhiteSpace(item.Association) ? true : false;
                    item.Association = baseline.Association;
                }

                var job = jobs.Where(p => p.ProjectId == item.ProjectId && p.IsFinish == false).FirstOrDefault();
                if (job != null)
                {
                    item.JobId = job.Id;
                    var approver = approvers.Where(p => p.JobId == job.Id && p.ApproverJobStatus == JobStatus.UnHandle).FirstOrDefault();
                    if (approver != null)
                    {
                        item.ApproverId = approver.ApproverId;
                        var user = await dbContext.Queryable<Domain.Models.User>().Where(p => p.Id == approver.ApproverId).FirstAsync();
                        item.ApproverName = user?.Name;
                        //item.ApproveLevel = approver.ApproveLevel;
                        if (approver.ApproverId == _currentUser.Id)
                        {
                            item.HasReview = true;
                            if (approver.ApproveLevel < ApproveLevel.Level2)
                            {
                                item.NextApprovers = dbContext.Queryable<BaseLinePlanApprover>().Where(p => p.CompanyId == _currentUser.CurrentLoginInstitutionId && p.ApproveLevel == ApproveLevel.Level2).Select(p => new SaveJobApproverRequestDto()
                                {
                                    ApproveLevel = p.ApproveLevel,
                                    ApproverId = p.ApproverId
                                }).ToArray();
                            }
                        }
                    }

                }

            }
            rt.Success();
            rt.Count = total;

            rt.Data = pPlanProductionList;
            return rt;
        }


        private ISugarQueryable<SearchSubsidiaryCompaniesProjectProductionDto> GetBaseLineplannew(BaseLinePlanAncomparisonRequsetDto requestBody, RefAsync<int> total, Guid? companyId)
        {
            var query = dbContext.Queryable<BaseLinePlanProjectAnnualPlanProduction>()
                                          .RightJoin<BaseLinePlanProject>((it, p) => it.ProjectId == p.Id)
                             .Where((it, p) => it.IsDelete == 1)
                             .WhereIF(_currentUser.CurrentLoginUserType == 3 && companyId != null, (it, p) => p.CompanyId == companyId && p.Association != null)
                             .WhereIF(requestBody.Year != null, (it, p) => p.Year == requestBody.Year)
                             .WhereIF(!string.IsNullOrWhiteSpace(requestBody.Name), (it, p) => p.ShortName.Contains(requestBody.Name) || p.PlanVersion.Contains(requestBody.Name))
                             .Select((it, p) => new SearchSubsidiaryCompaniesProjectProductionDto()
                             {
                                 ProjectId = it.ProjectId,
                                 JanuaryProductionValue = it.JanuaryProductionValue,
                                 FebruaryProductionValue = it.FebruaryProductionValue,
                                 MarchProductionValue = it.MarchProductionValue,
                                 AprilProductionValue = it.AprilProductionValue,
                                 MayProductionValue = it.MayProductionValue,
                                 JuneProductionValue = it.JuneProductionValue,
                                 JulyProductionValue = it.JulyProductionValue,
                                 AugustProductionValue = it.AugustProductionValue,
                                 SeptemberProductionValue = it.SeptemberProductionValue,
                                 OctoberProductionValue = it.OctoberProductionValue,
                                 NovemberProductionValue = it.NovemberProductionValue,
                                 DecemberProductionValue = it.DecemberProductionValue,
                                 LatestDate = it.CreateTime,
                                 CompanyId = p.CompanyId,
                                 ProjectName = p.ShortName,
                                 Id = p.Id,
                                 PlanStatus = p.PlanStatus,
                                 SubmitStatus = p.SubmitStatus,
                                 RejectReason = p.RejectReason,
                                 Association = p.Association,
                                 PlanVersion = p.PlanVersion
                             });
            return query;
        }



        /// <summary>
        /// 基准计划审核
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> BaseLinePlanProjectApproveAsync(SearchSubsidiaryCompaniesProjectProductionDto input)
        {
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            if (input.Id != null)
            {
                var job = await dbContext.Queryable<Domain.Models.Job>().Where(x => x.IsDelete == 1 && x.Id == input.JobId).FirstAsync();
                var baseLinePlan = await dbContext.Queryable<BaseLinePlanProject>().Where(p => p.Id == input.Id).FirstAsync();
                baseLinePlan.PlanStatus = (int)job.ApproveStatus;
                baseLinePlan.RejectReason = job.RejectReason;
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


        /// <summary>
        /// 提交审核
        /// </summary>
        /// <param name="baseLinePlan"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> BaseLinePlanProjectApproveAsync(SubmitJobOfBaseLinePlanRequestDto model)
        {
            var Approvers = await dbContext.Queryable<BaseLinePlanApprover>().Where(x => x.CompanyId == _currentUser.CurrentLoginInstitutionId && x.BizModule == BizModule.BaseLinePlan && x.ApproveLevel == ApproveLevel.Level1).Select(p => new BaseLinePlanApprover
            {
                CompanyId = p.CompanyId,
                ApproveLevel = p.ApproveLevel,
                ApproverId = p.ApproverId,
                BizModule = p.BizModule
            }).FirstAsync();
            //if (Approvers != null)
            //{
            //    SaveJobApproverRequestDto dto = new SaveJobApproverRequestDto();
            //    dto.ApproverId = Approvers.ApproverId;
            //    dto.ApproveLevel = Approvers.ApproveLevel;
            //    model.Approvers = new SaveJobApproverRequestDto[] { dto };
            //}
            //model.BizData = baseLinePlan;
            //model.JobId = GuidUtil.Next();
            //model.ProjectId = baseLinePlan.Id;
            //model.SubmitType = JobSubmitType.AddJob;



            //ApproveJobRequestDto approve = new ApproveJobRequestDto();
            //approve.JobId = "08dd6531-442f-4944-8d7e-bd5c0fa85877".ToGuid();
            //approve.ApproveStatus = JobApproveStatus.Pass;
            //var Approverstwo = await dbContext.Queryable<BaseLinePlanApprover>().Where(x => x.CompanyId == _currentUser.CurrentLoginInstitutionId && x.BizModule == BizModule.BaseLinePlan && x.ApproveLevel == ApproveLevel.Level2).Select(p => new BaseLinePlanApprover
            //{
            //    CompanyId = p.CompanyId,
            //    ApproveLevel = p.ApproveLevel,
            //    ApproverId = p.ApproverId,
            //    BizModule = p.BizModule
            //}).FirstAsync();
            //if (Approverstwo != null)
            //{
            //    SaveJobApproverRequestDto dto = new SaveJobApproverRequestDto();
            //    dto.ApproverId = Approverstwo.ApproverId;
            //    dto.ApproveLevel = Approverstwo.ApproveLevel;
            //    approve.NextApprovers = new SaveJobApproverRequestDto[] { dto };
            //}

            //await _jobService.ApproveJobAsync(approve);
            return await _jobService.SubmitJobAsync(model);

        }


        /// <summary>
        /// 基准计划审核
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> BaseLinePlanProjectApprove(SearchSubsidiaryCompaniesProjectProductionDto input)
        {
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            var baseLinePlan = await dbContext.Queryable<BaseLinePlanProject>().Where(x => x.Id == input.Id).FirstAsync();
            baseLinePlan.PlanStatus = input.PlanStatus;
            baseLinePlan.RejectReason = input.RejectReason;
            bool result = await dbContext.Updateable<BaseLinePlanProject>(baseLinePlan).ExecuteCommandAsync() > 0;
            responseAjaxResult.Data = result;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }


        /// <summary>
        /// 导出基准计划
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<BaseLinePlanExcelOutputDto>>> BaseLinePlanExcelOutPutAsync(BaseLinePlanAncomparisonRequsetDto requestBody)
        {
            var list = await GetBaseLinePlanList(requestBody);
            var responseAjaxResult = new ResponseAjaxResult<List<BaseLinePlanExcelOutputDto>>();
            responseAjaxResult.Data = list.Data.Select(it => new BaseLinePlanExcelOutputDto()
            {
                JanuaryProductionValue = it.JanuaryProductionValue,
                FebruaryProductionValue = it.FebruaryProductionValue,
                MarchProductionValue = it.MarchProductionValue,
                AprilProductionValue = it.AprilProductionValue,
                MayProductionValue = it.MayProductionValue,
                JuneProductionValue = it.JuneProductionValue,
                JulyProductionValue = it.JulyProductionValue,
                AugustProductionValue = it.AugustProductionValue,
                SeptemberProductionValue = it.SeptemberProductionValue,
                OctoberProductionValue = it.OctoberProductionValue,
                NovemberProductionValue = it.NovemberProductionValue,
                DecemberProductionValue = it.DecemberProductionValue,
                CompanyName = it.CompanyName,
                ProjectName = it.ProjectName
            }).ToList();
            return responseAjaxResult;
        }


        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="imports"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<BaseLineImportOutput>> BaseLinePlanProjectAnnualProductionImport(List<BaseLinePlanProjectAnnualProductionImport> imports, BaseLinePlanprojectImportDto input)
        {
            var responseAjaxResult = new ResponseAjaxResult<BaseLineImportOutput>();
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

                foreach (var item in imports)
                {
                    //var years = item.Year;
                    //if (string.IsNullOrWhiteSpace(years))
                    //{
                    //    responseAjaxResult.Fail("年份不能为空");
                    //}
                    //int year;
                    //bool result = int.TryParse(years, out year);
                    //if (result == false)
                    //{
                    //    responseAjaxResult.Fail("年份格式异常");
                    //}
                    var BaseId = GuidUtil.Next();
                    var add = new BaseLinePlanProject()
                    {
                        ShortName = item.ShortName,
                        Id = BaseId,
                        CompanyId = _currentUser.CurrentLoginInstitutionId,
                        Year = DateTime.Now.Year,
                        SubmitStatus = 0,
                        CreateTime = DateTime.Now,
                    };

                    //if (input.Association > 0)
                    //{
                    add.Association = item.Association;
                    //}

                    if (!string.IsNullOrWhiteSpace(add.Association))
                    {
                        var co = await dbContext.Queryable<Project>().Where(p => p.MasterCode == add.Association).FirstAsync();
                        if (co != null)
                        {
                            add.CompanyId = co.CompanyId.Value;
                        }
                        else
                        {
                            add.CompanyId = "bd840460-1e3a-45c8-abed-6e66903eb465".ToGuid();
                        }
                    }
                    else
                    {
                        add.CompanyId = "bd840460-1e3a-45c8-abed-6e66903eb465".ToGuid();
                    }
                    await GetPlanVersion(add, 1);
                    addbaseLinePlanProjects.Add(add);
                    var b = new BaseLinePlanProjectAnnualPlanProduction
                    {
                        JanuaryProductionValue = Setnumericalconversiontwo(item.JanuaryProductionValue),
                        FebruaryProductionValue = Setnumericalconversiontwo(item.FebruaryProductionValue),
                        MarchProductionValue = Setnumericalconversiontwo(item.MarchProductionValue),
                        AprilProductionValue = Setnumericalconversiontwo(item.AprilProductionValue),
                        MayProductionValue = Setnumericalconversiontwo(item.MayProductionValue),
                        JuneProductionValue = Setnumericalconversiontwo(item.JuneProductionValue),
                        JulyProductionValue = Setnumericalconversiontwo(item.JulyProductionValue),
                        AugustProductionValue = Setnumericalconversiontwo(item.AugustProductionValue),
                        SeptemberProductionValue = Setnumericalconversiontwo(item.SeptemberProductionValue),
                        OctoberProductionValue = Setnumericalconversiontwo(item.OctoberProductionValue),
                        NovemberProductionValue = Setnumericalconversiontwo(item.NovemberProductionValue),
                        DecemberProductionValue = Setnumericalconversiontwo(item.DecemberProductionValue),
                        ProjectId = BaseId,
                        CompanyId = _currentUser.CurrentLoginInstitutionId,
                        Year = DateTime.Now.Year
                    };
                    b.Id = Guid.NewGuid();
                    addTables.Add(b);
                }

                foreach (var item in addTables)
                {
                    item.Id = GuidUtil.Next();
                }
                await dbContext.Insertable(addTables).ExecuteCommandAsync();
                await dbContext.Insertable(addbaseLinePlanProjects).ExecuteCommandAsync();
                //var baseline = await dbContext.Queryable<BaseLinePlanProject>().Where(x => x.Id == BaseId).FirstAsync();
                //responseAjaxResult.Data = new BaseLineImportOutput() { Id = BaseId };
                responseAjaxResult.Success();
                return responseAjaxResult;
            }
            catch
            {
                responseAjaxResult.Success(ResponseMessage.OPERATION_UPLOAD_FAIL, HttpStatusCode.UploadFail);
                return responseAjaxResult;
            }
        }

        #region 通用
        /// <summary>
        /// 获取计划基准下拉框
        /// </summary>
        /// <param name="requsetDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<BaseLinePlanSelectOptiong>>> SearchBaseLinePlanOptionsAsync(BaseLinePlanAncomparisonRequsetDto requsetDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<BaseLinePlanSelectOptiong>>();
            RefAsync<int> total = 0;
            var List = await dbContext.Queryable<BaseLinePlanProject>()
            .Where(x => x.IsDelete == 1 && x.PlanVersion != null)
            .Select(x => new BaseLinePlanSelectOptiong()
            {
                Id = x.Id,
                ShortName = x.ShortName,
                PlanVersion = x.PlanVersion
            }).ToPageListAsync(requsetDto.PageIndex, requsetDto.PageSize, total);
            responseAjaxResult.Success();
            responseAjaxResult.Count = total;
            responseAjaxResult.Data = List;
            return responseAjaxResult;
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

        /// <summary>
        /// 获取基准计划审批列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<ApproveUsersResponseDto>>> SearchBaseLinePlanApproveUsersAsync(ApproveUsersRequsetDto input)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<ApproveUsersResponseDto>>();
            RefAsync<int> total = 0;
            var List = await dbContext.Queryable<Domain.Models.User>()
                .LeftJoin<Domain.Models.Company>((u, c) => u.CompanyId == c.PomId)
            .Where(u => u.IsDelete == 1)
            .WhereIF(!string.IsNullOrWhiteSpace(input.Name), u => u.Name.Contains(input.Name))
            .Select((u, c) => new ApproveUsersResponseDto()
            {
                Companyid = u.CompanyId,
                Id = u.Id,
                Name = u.Name,
                Pomid = u.PomId,
                CompanyName = c.Name
            }).ToPageListAsync(input.PageIndex, input.PageSize, total);
            responseAjaxResult.Success();
            responseAjaxResult.Count = total;
            responseAjaxResult.Data = List;
            return responseAjaxResult;
        }


        public async Task<ResponseAjaxResult<List<BaseLinePlanProjectResponseDto>>> SearchBaseLinePlanProjectAsync(SearchBaseLinePlanProjectRequsetDto input)
        {
            List<Guid> departmentIds = new List<Guid>();
            var oids = await dbContext.Queryable<Institution>().Where(x => x.IsDelete == 1 && x.Oid == _currentUser.CurrentLoginInstitutionOid).SingleAsync();
            var InstitutionId = await baseService.SearchCompanySubPullDownAsync(oids.PomId.Value, false, true);
            departmentIds = InstitutionId.Data.Select(x => x.Id.Value).ToList();
            var responseAjaxResult = new ResponseAjaxResult<List<BaseLinePlanProjectResponseDto>>();
            RefAsync<int> total = 0;
            List<Guid?> StatusIds = new List<Guid?>()
            {
                "cd3c6e83-1b7c-40c2-a415-5a44f13584cc".ToGuid(),
                "2c800da1-184f-408a-b5a4-fd915e8d6b6a".ToGuid(),
                "19050a4b-fe95-47cf-aafe-531d5894c88b".ToGuid()
            };
            var List = await dbContext.Queryable<Domain.Models.Project>()
            .Where(x => x.IsDelete == 1 && StatusIds.Contains(x.StatusId) && x.MasterCode != null)
            .WhereIF(!string.IsNullOrWhiteSpace(input.Name), x => x.Name.Contains(input.Name))
            .Where(x => departmentIds.Contains(x.ProjectDept.Value))
            .Select(x => new BaseLinePlanProjectResponseDto()
            {
                Id = x.Id,
                Name = x.Name,
                MasterCode = x.MasterCode,
                ShortName = x.ShortName
            }).ToListAsync();
            //.ToPageListAsync(input.PageIndex, input.PageSize, total);
            responseAjaxResult.Success();
            responseAjaxResult.Count = total;
            responseAjaxResult.Data = List;
            return responseAjaxResult;
        }


        /// <summary>
        /// 编辑是否基准计划填报期
        /// </summary>
        /// <param name="input">0,基准  1:新建</param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> ToggleBaseLinePlanPeriodAsync(string input)
        {
            var responseAjaxResult = new ResponseAjaxResult<bool>();
            var model = await dbContext.Queryable<DictionaryTable>().Where(p => p.TypeNo == 12).FirstAsync();
            model.Name = input;
            bool result = await dbContext.Updateable(model).ExecuteCommandAsync() > 0;
            responseAjaxResult.Data = result;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }


        /// <summary>
        /// 获取是否基准计划填报期
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<string>> GetBaseLinePlanPeriodStatusAsync()
        {
            var responseAjaxResult = new ResponseAjaxResult<string>();
            var model = await dbContext.Queryable<DictionaryTable>().Where(p => p.TypeNo == 12 && p.IsDelete == 1).Select(p => new DictionaryTable() { TypeNo = p.TypeNo, Name = p.Name, Id = p.Id, Remark = p.Remark }).FirstAsync();
            responseAjaxResult.Data = model.Name;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }


        #endregion
    }
}
