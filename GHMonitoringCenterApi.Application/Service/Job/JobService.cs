using AutoMapper;
using CDC.MDM.Core.Common.Util;
using GHMonitoringCenterApi.Application.Contracts.Dto.Enums;
using GHMonitoringCenterApi.Application.Contracts.Dto.Job;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project.MonthReportForProject;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report;
using GHMonitoringCenterApi.Application.Contracts.IService;
using GHMonitoringCenterApi.Application.Contracts.IService.Job;
using GHMonitoringCenterApi.Application.Contracts.IService.Project;
using GHMonitoringCenterApi.Domain.Enums;
using GHMonitoringCenterApi.Domain.IRepository;
using GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Domain.Shared;
using GHMonitoringCenterApi.Domain.Shared.Const;
using GHMonitoringCenterApi.Domain.Shared.Enums;
using GHMonitoringCenterApi.Domain.Shared.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SqlSugar;
using SqlSugar.Extensions;
using System.Linq.Expressions;
using UtilsSharp;

namespace GHMonitoringCenterApi.Application.Service.Job
{
    /// <summary>
    /// 任务中心业务层
    /// </summary>
    public class JobService : IJobService
    {
        /// <summary>
        /// 项目
        /// </summary>
        private readonly IBaseRepository<Project> _dbProject;

        /// <summary>
        /// 项目主数据
        /// </summary>
        private readonly IBaseRepository<Domain.Models.ProjectMasterData> _dbProjectMasterData;

        /// <summary>
        /// 任务
        /// </summary>
        private readonly IBaseRepository<Domain.Models.Job> _dbJob;

        /// <summary>
        /// 任务审批人
        /// </summary>
        private readonly IBaseRepository<JobApprover> _dbJobApprover;

        /// <summary>
        /// 任务记录
        /// </summary>
        private readonly IBaseRepository<JobRecord> _dbJobRecord;

        /// <summary>
        /// 地区
        /// </summary>
        private readonly IBaseRepository<Province> _dbProvince;

        /// <summary>
        /// 项目类型
        /// </summary>
        private readonly IBaseRepository<ProjectType> _dbProjectType;

        /// <summary>
        /// 项目干系人
        /// </summary>
        private readonly IBaseRepository<ProjectLeader> _dbProjectLeader;

        /// <summary>
        /// 机构信息
        /// </summary>
        private readonly IBaseRepository<Institution> _dbInstitution;

        /// <summary>
        /// 机构角色
        /// </summary>
        private readonly IBaseRepository<InstitutionRole> _dbInstitutionRole;

        /// <summary>
        /// 行业标准分类
        /// </summary>
        private readonly IBaseRepository<IndustryClassification> _dbIndustryClassification;

        /// <summary>
        /// 用户
        /// </summary>
        private readonly IBaseRepository<Domain.Models.User> _dbUser;

        /// <summary>
        /// 角色
        /// </summary>
        private readonly IBaseRepository<Domain.Models.Role> _dbRole;

        /// <summary>
        /// 币种
        /// </summary>
        private readonly IBaseRepository<Currency> _dbCurrency;

        /// <summary>
        /// 项目业务层
        /// </summary>
        private readonly IProjectService _projectService;

        /// <summary>
        /// 项目填报业务层
        /// </summary>
        private readonly IProjectReportService _projectReportService;

        /// <summary>
        /// 项目审批人
        /// </summary>
        private readonly IBaseRepository<ProjectApprover> _dbProjectApprover;

        /// <summary>
        /// 匹配
        /// </summary>
        private readonly IMapper _mapper;
        /// <summary>
        /// 新版项目月报wbs列表
        /// </summary>
        private readonly IMonthReportForProjectService _mPService;

        /// <summary>
        /// 基准计划信息业务层
        /// </summary>
        //private readonly IBaseLinePlanProjectService _baseLinePlanProjectService;

        /// <summary>
        /// 全局对象
        /// </summary>
        private readonly GlobalObject _globalObject;

        /// <summary>
        /// 通用方法业务层
        /// </summary>

        private readonly IBaseService _baseService;
        private readonly ISqlSugarClient _dbContext;
        /// <summary>
        /// 当前登录用户
        /// </summary>
        private CurrentUser _currentUser { get { return _globalObject.CurrentUser; } }


        /// <summary>
        /// 项目日报构造
        /// </summary>
        public JobService(IBaseRepository<Project> dbProject
            , IBaseRepository<Domain.Models.ProjectMasterData> dbProjectMasterData
            , IBaseRepository<Domain.Models.Job> dbJob
            , IBaseRepository<JobApprover> dbJobApprover
            , IBaseRepository<JobRecord> dbJobRecord
            , IBaseRepository<Province> dbProvince
            , IBaseRepository<ProjectType> dbProjectType
            , IBaseRepository<Institution> dbInstitution
            , IBaseRepository<InstitutionRole> dbInstitutionRole
            , IBaseRepository<IndustryClassification> dbIndustryClassification
            , IBaseRepository<ProjectLeader> dbProjectLeader
            , IBaseRepository<Domain.Models.User> dbUser
            , IBaseRepository<Domain.Models.Role> dbRole
            , IBaseRepository<Currency> dbCurrency
            , IBaseRepository<ProjectApprover> dbProjectApprover
            , IProjectService projectService
             , IProjectReportService projectReportService
            , IBaseService baseService
            , IMapper mapper
            , GlobalObject globalObject
            , ISqlSugarClient dbContext
            , IMonthReportForProjectService mPService)
        {
            _baseService = baseService;
            _dbProject = dbProject;
            _dbProjectMasterData = dbProjectMasterData;
            _dbJob = dbJob;
            _dbJobApprover = dbJobApprover;
            _dbJobRecord = dbJobRecord;
            _dbProvince = dbProvince;
            _dbProjectType = dbProjectType;
            _dbInstitution = dbInstitution;
            _dbInstitutionRole = dbInstitutionRole;
            _dbIndustryClassification = dbIndustryClassification;
            _dbProjectLeader = dbProjectLeader;
            _dbUser = dbUser;
            _dbRole = dbRole;
            _dbCurrency = dbCurrency;
            _dbProjectApprover = dbProjectApprover;
            _projectService = projectService;
            _projectReportService = projectReportService;
            _mapper = mapper;
            _globalObject = globalObject;
            _dbContext = dbContext;
            _mPService = mPService;
            //_baseLinePlanProjectService = baseLinePlanProjectService;
        }
        #region 提交任务

        /// <summary>
        /// 提交任务
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> SubmitJobAsync<TBiz>(SubmitJobRequestDto<TBiz> model) where TBiz : class
        {
            model.ResetModelProperty();
            var result = new ResponseAjaxResult<bool>();
            if (!VerifySubmitModel(model, out string msg))
            {
                return result.FailResult(HttpStatusCode.VerifyFail, msg);
            }
            Domain.Models.Job? job;

            // 新增任务
            if (model.SubmitType == JobSubmitType.AddJob)
            {
                job = new Domain.Models.Job()
                {
                    Id = GuidUtil.Next(),
                    CreateId = _currentUser.Id,
                    CreateTime = DateTime.Now,
                    SubmitTime = DateTime.Now,
                    ProjectId = (Guid)model.ProjectId,
                    BizModule = model.BizModule,
                    BizData = model.GetJsonBizData(),
                    ApproveLevel = ApproveLevel.None,
                    FinishApproveLevel = model.FinishApproveLevel,
                    DateDay = model.DateDay,
                    DateMonth = model.DateMonth,
                };
                var jobApprovers = new List<JobApprover>();
                foreach (var approver in model.Approvers)
                {
                    if (jobApprovers.Any(t => t.ApproverId == approver.ApproverId))
                    {
                        continue;
                    }
                    jobApprovers.Add(new JobApprover()
                    {
                        Id = GuidUtil.Next(),
                        CreateId = _currentUser.Id,
                        ApproverId = approver.ApproverId,
                        ApproveLevel = ApproveLevel.Level1,
                        JobId = job.Id,
                        ApproverJobStatus = JobStatus.UnHandle
                    });
                }
                await _dbJob.AsInsertable(job).ExecuteCommandAsync();
                await _dbJobApprover.AsInsertable(jobApprovers).ExecuteCommandAsync();
            }
            // 重置任务
            else if (model.SubmitType == JobSubmitType.ResetJob)
            {
                job = await GetJobAsync((Guid)model.JobId);
                if (job == null)
                {
                    return result.FailResult(HttpStatusCode.DataNotEXIST, ResponseMessage.DATA_NOTEXIST_JOB);
                }
                if (job.IsFinish || job.ApproveStatus == JobApproveStatus.Pass)
                {
                    return result.FailResult(HttpStatusCode.NotAllowChange, ResponseMessage.NOTALLOW_CHANGE_JOB);
                }
                var jobApprovers = await GetJobApproversAsync(job.Id);
                job.ApproveStatus = JobApproveStatus.None;
                job.ApproveLevel = ApproveLevel.None;
                job.BizData = model.GetJsonBizData();
                job.UpdateId = _currentUser.Id;
                job.ApproveTime = null;
                job.RejectReason = null;
                job.SubmitTime = DateTime.Now;
                foreach (var jobApprover in jobApprovers)
                {
                    // 如果审批层级大于一级则移除
                    if (jobApprover.ApproveLevel > ApproveLevel.Level1)
                    {
                        jobApprover.IsDelete = 0;
                        jobApprover.DeleteId = _currentUser.Id;
                    }
                    else
                    {
                        jobApprover.UpdateId = _currentUser.Id;
                        jobApprover.ApproveStatus = JobApproveStatus.None;
                        jobApprover.ApproverJobStatus = JobStatus.UnHandle;
                    }
                }
                await _dbJob.AsUpdateable(job).ExecuteCommandAsync();
                await _dbJobApprover.AsUpdateable(jobApprovers).ExecuteCommandAsync();
            }
            else
            {
                return result.FailResult(HttpStatusCode.DataNotMatch, ResponseMessage.DATA_NOTMATCH_BIZ);
            }
            // 任务记录
            var jobRecord = new JobRecord()
            {
                JobId = job.Id,
                OperatorId = _currentUser.Id,
                OperateTime = DateTime.Now,
                OperatorName = _currentUser.Name,
                OperateContent = model.SubmitType == JobSubmitType.AddJob ? "创建项目" : "修改项目",
                ApproveStatus = job.ApproveStatus,
                RejectReason = job.RejectReason
            };

            if (model.BizModule == BizModule.BaseLinePlan)
            {
                jobRecord.OperateContent = model.SubmitType == JobSubmitType.AddJob ? "新建基准计划" : "修改基准计划";
            }

            // 业务处理
            var bizHandleResult = await HandleBizAsync(job);
            if (bizHandleResult.Code != HttpStatusCode.Success)
            {
                return result.FailResult(bizHandleResult.Code, bizHandleResult.Message);
            }
            await _dbJobRecord.AsInsertable(jobRecord).ExecuteCommandAsync();
            var level1ApproverIds = model.Approvers.Where(t => t.ApproveLevel == ApproveLevel.Level1).Select(t => t.ApproverId).ToArray();
            await SendJitToApproverAsync(level1ApproverIds);
            await SendJitOfBizAsync(model);
            return result.SuccessResult(true);
        }

        /// <summary>
        /// 验证提交的Model
        /// </summary>
        /// <returns></returns>
        private bool VerifySubmitModel<TBiz>(SubmitJobRequestDto<TBiz> model, out string msg) where TBiz : class
        {
            msg = string.Empty;
            if (model.BizModule == BizModule.MonthReport)
            {
                if (model.SubmitType == JobSubmitType.AddJob && IsExistsUnFinishMonthReportJob((Guid)model.ProjectId, model.DateMonth))
                {
                    msg = $"该项目已存在未完成的审批流程中，请勿重复提交";
                    return false;
                }
            }
            return true;
        }



        /// <summary>
        /// 发送给审批人的通知
        /// </summary>
        /// <returns></returns>
        private async Task SendJitToApproverAsync(Guid[] userIds)
        {
            if (userIds == null || !userIds.Any())
            {
                return;
            }
            var userGroupCodes = await _dbUser.AsQueryable().Where(x => userIds.Contains(x.Id)).Select(x => x.GroupCode).ToListAsync();
            var noticeUserIds = userGroupCodes.Where(t => !string.IsNullOrWhiteSpace(t)).Select(t => t.ObjToString()).ToArray();
            var content = "您好，您有一条来自于" + _currentUser.Name + "的提交的待审批信息，请登录数字广航-智慧运营监控中心系统（任务中心->我的任务）进行审核或交建通工作台（广航应用->广航项目运营中心）进行审核（外网需登录VPN）。";
            SendJit(noticeUserIds, content);
        }


        /// <summary>
        /// 根据业务发送相关详细
        /// </summary>
        /// <returns></returns>
        private async Task SendJitOfBizAsync<TBiz>(SubmitJobRequestDto<TBiz> model) where TBiz : class
        {
            if (model.BizModule == BizModule.MonthReport)
            {
                // 如果项目月报变更加入审批流 通知陈翠
                if (model.SubmitType == JobSubmitType.AddJob && model.BizData is SaveProjectMonthReportRequestDto bizModel && bizModel.ModelState == ModelState.Update)
                {
                    var noticeUserIds = new string[] { CommonData.ChenCuiGroupCode };
                    ConvertHelper.TryParseFromDateMonth(bizModel.DateMonth, out DateTime monthTime);
                    var content = $"您好，您有一条来自于{_currentUser.Name}的提交的项目月报“{bizModel.ProjectName}（{monthTime.ToString("yyyy年MM月")}）”变更信息。";
                    SendJit(noticeUserIds, content);
                }
            }
            await Task.CompletedTask;
        }

        /// <summary>
        /// 发送消息到交建通
        /// </summary>
        /// <returns></returns>
        private void SendJit(string[] noticeUserIds, string content)
        {
            if (noticeUserIds == null || !noticeUserIds.Any())
            {
                return;
            }
            var singleMessageTemplateRequestDto = new SingleMessageTemplateRequestDto()
            {
                IsAll = false,
                MessageType = "text",
                UserIds = noticeUserIds.ToList(),
                TextContent = content
            };
            JjtUtils.SinglePushMessage(singleMessageTemplateRequestDto);
        }
        #endregion

        #region 移除任务

        /// <summary>
        /// 移除任务
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> ReomveJobAsync(ReomveJobRequestDto model)
        {
            var result = new ResponseAjaxResult<bool>();
            var job = await GetJobAsync(model.JobId);
            if (job == null)
            {
                return result.FailResult(HttpStatusCode.DataNotEXIST, ResponseMessage.DATA_NOTEXIST_JOB);
            }
            if (job.CreateId != _currentUser.Id)
            {
                return result.FailResult(HttpStatusCode.NotAllowChange, ResponseMessage.NOTALLOW_CHANGE_JOB);
            }
            job.IsDelete = 0;
            job.DeleteId = _currentUser.Id;
            await _dbJob.AsUpdateable(job).UpdateColumns(t => new { t.DeleteId, t.DeleteTime, t.IsDelete }).ExecuteCommandAsync();
            return result.SuccessResult(true);
        }

        #endregion

        #region 搜索任务集合

        /// <summary>
        /// 搜索 （待办/已办）任务列表
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<JobResponseDto>>> SearchJobsAsync(JobsRequestDto model)
        {
            if (model.FindType == JobsFindType.AllJobs)
            {
                return await SearchAllJobsAsync(model);
            }
            else
            {
                return await SearchApproverJobsAsync(model);
            }
        }

        /// <summary>
        /// 搜索 审批人（待办/已办）任务列表
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<JobResponseDto>>> SearchApproverJobsAsync(JobsRequestDto model)
        {
            var result = new ResponseAjaxResult<List<JobResponseDto>>();
            var selectExpr = GetSelectExpr(model.BizModule);
            RefAsync<int> total = 0;
            var query = _dbJob.AsQueryable()
                  .InnerJoin(_dbJobApprover.AsQueryable(), (j, a) => j.Id == a.JobId && a.IsDelete == 1)
                  .LeftJoin(_dbProject.AsQueryable(), (j, a, p) => j.ProjectId == p.Id && p.IsDelete == 1)
                  .Where((j, a, p) => a.ApproverId == _currentUser.Id && a.ApproverJobStatus == model.JobStatus && j.IsDelete == 1)
                  .Where((j, a, p) => j.BizModule != BizModule.BaseLinePlan)
                  .WhereIF(model.BizModule != null, (j, a, p) => j.BizModule == model.BizModule)
                  .WhereIF(model.JobStatus == JobStatus.Handled, (j, a, p) => j.ApproveLevel == a.ApproveLevel)
                  .WhereIF(model.JobStatus == JobStatus.UnHandle, (j, a, p) => j.ApproveLevel == a.ApproveLevel - 1 && (j.ApproveStatus == JobApproveStatus.None || j.ApproveStatus == JobApproveStatus.Pass))
                  .WhereIF(!string.IsNullOrEmpty(model.ProjectName), (j, a, p) => SqlFunc.Contains(p.Name, model.ProjectName))
                  .WhereIF(model.StartSubmitTime != null, (j, a, p) => j.SubmitTime >= model.StartSubmitTime)
                  .WhereIF(model.EndSubmitTime != null, (j, a, p) => j.SubmitTime < ((DateTime)model.EndSubmitTime).AddDays(1));
            if (model.JobStatus == JobStatus.Handled)
            {
                query = query.Distinct().OrderByDescending((j, a, p) => new { j.ApproveTime, j.SubmitTime });
            }
            else
            {
                query = query.Distinct().OrderByDescending((j, a, p) => j.SubmitTime);
            }
            var resJobList = await query.Select(selectExpr).ToPageListAsync(model.PageIndex, model.PageSize, total);
            // 业务关联相关库，进行任务数据填充
            await FillSearchJobsAsync(resJobList, model.BizModule);
            // 其他任务数据填充
            foreach (var resJob in resJobList)
            {
                resJob.ApproveStatusText = GetApproveStatusText(resJob.IsFinish, resJob.ApproveLevel, resJob.ApproveStatus);
            }
            return result.SuccessResult(resJobList, total);
        }

        /// <summary>
        /// 筛选字段（todo 根据不同得任务模块可能存在不同得返回字段）
        /// </summary>
        /// <param name="jobBizModule">任务业务模块</param>
        /// <returns></returns>
        private Expression<Func<Domain.Models.Job, JobApprover, Project, JobResponseDto>> GetSelectExpr(BizModule? jobBizModule)
        {
            #region 留存筛选字段一
            //Expression<Func<Domain.Models.Job, Project, JobApprover, Domain.Models.ProjectMasterData, JobResponseDto>> selectExpr = (a, b, c, d) => new JobResponseDto()
            //{
            //    JobId = a.Id,
            //    CreateTime = a.CreateTime,
            //    JobStatus = c.ApproverJobStatus,
            //    ApproveStatus = a.ApproveStatus,
            //    ApproverId = a.ApproverId,
            //    ApproveLevel = a.ApproveLevel,
            //    ApproveTime = a.ApproveTime,
            //    BizModule = a.BizModule,
            //    BizHandleMessage = a.BizHandleMessage,
            //    ProjectId = a.ProjectId,
            //    IsFinish = a.IsFinish,
            //    ProjectName = b.Name,
            //    ProjectAreaId = b.AreaId,
            //    ProjectClassifyStandard = b.ClassifyStandard,
            //    ProjectCode = b.Code,
            //    ProjectTypeId = b.TypeId,
            //    ProjectBeforeName = d.BeforeName,
            //    ProjectForeign = d.Foreign,
            //    ProjectCity = d.ProjectCity
            //};
            #endregion

            Expression<Func<Domain.Models.Job, JobApprover, Project, JobResponseDto>> selectExpr = (j, a, p) => new JobResponseDto()
            {
                JobId = j.Id,
                SubmitTime = j.SubmitTime,
                SubmiterId = j.CreateId,
                JobStatus = a.ApproverJobStatus,
                ApproveStatus = j.ApproveStatus,
                ApproverId = j.ApproverId,
                ApproveLevel = j.ApproveLevel,
                ApproveTime = j.ApproveTime,
                BizModule = j.BizModule,
                BizHandleMessage = j.BizHandleMessage,
                ProjectId = j.ProjectId,
                IsFinish = j.IsFinish,
                ProjectAmount = p.Amount,
                ProjectName = p.Name,
                ProjectCategory = p.Category,
                ProjectCompanyId = p.CompanyId,
                ProjectTypeId = p.TypeId,
                ProjectRate = p.Rate,
                ProjectCurrencyId = p.CurrencyId
            };
            return selectExpr;
        }

        /// <summary>
        /// 搜索所有（待办/已办）任务列表
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<JobResponseDto>>> SearchAllJobsAsync(JobsRequestDto model)
        {
            var result = new ResponseAjaxResult<List<JobResponseDto>>();
            RefAsync<int> total = 0;
            var query = _dbJob.AsQueryable()
                  .LeftJoin(_dbProject.AsQueryable(), (j, p) => j.ProjectId == p.Id && p.IsDelete == 1)
                  .Where((j, p) => j.IsDelete == 1)
                  .Where((j, p) => j.BizModule != BizModule.BaseLinePlan)
                  .WhereIF(model.BizModule != null, (j, p) => j.BizModule == model.BizModule)
                  .WhereIF(model.JobStatus == JobStatus.Handled, (j, p) => j.IsFinish == true)
                  .WhereIF(model.JobStatus == JobStatus.UnHandle, (j, p) => j.IsFinish == false)
                  .WhereIF(!string.IsNullOrEmpty(model.ProjectName), (j, p) => SqlFunc.Contains(p.Name, model.ProjectName))
                  .WhereIF(model.StartSubmitTime != null, (j, p) => j.SubmitTime >= model.StartSubmitTime)
                  .WhereIF(model.EndSubmitTime != null, (j, p) => j.SubmitTime < ((DateTime)model.EndSubmitTime).AddDays(1));
            if (model.JobStatus == JobStatus.Handled)
            {
                query = query.OrderByDescending((j, p) => new { j.ApproveTime });
            }
            else
            {
                query = query.OrderByDescending((j, p) => j.SubmitTime);
            }
            var resJobList = await query.Select((j, p) => new JobResponseDto()
            {
                JobId = j.Id,
                SubmitTime = j.SubmitTime,
                SubmiterId = j.CreateId,
                ApproveStatus = j.ApproveStatus,
                ApproverId = j.ApproverId,
                ApproveLevel = j.ApproveLevel,
                ApproveTime = j.ApproveTime,
                BizModule = j.BizModule,
                BizHandleMessage = j.BizHandleMessage,
                ProjectId = j.ProjectId,
                IsFinish = j.IsFinish,
                ProjectAmount = p.Amount,
                ProjectName = p.Name,
                ProjectCategory = p.Category,
                ProjectCompanyId = p.CompanyId,
                ProjectTypeId = p.TypeId,
                ProjectRate = p.Rate * 100,
                ProjectCurrencyId = p.CurrencyId
            }).ToPageListAsync(model.PageIndex, model.PageSize, total);
            // 业务关联相关库，进行任务数据填充
            await FillSearchJobsAsync(resJobList, model.BizModule);
            // 其他任务数据填充
            foreach (var resJob in resJobList)
            {
                resJob.ApproveStatusText = GetApproveStatusText(resJob.IsFinish, resJob.ApproveLevel, resJob.ApproveStatus);
            }
            return result.SuccessResult(resJobList, total);
        }

        /// <summary>
        /// 业务关联相关库，进行任务数据填充
        /// </summary>
        /// <param name="resJobList">任务列表</param>
        /// <param name="jobBizModule">任务业务模块</param>
        /// <returns></returns>
        private async Task FillSearchJobsAsync(List<JobResponseDto> resJobList, BizModule? jobBizModule)
        {
            #region 留存根据筛选字段一，填充的相关库中数据

            //if (jobBizModule == null)
            //{
            //    var projectAreaIds = resJobList.Where(t => t.ProjectAreaId != null).Select(t => t.ProjectAreaId).Distinct().ToArray();
            //    var projectTypeIds = resJobList.Where(t => t.ProjectTypeId != null).Select(t => (Guid)t.ProjectTypeId).Distinct().ToArray();
            //    var projectClassifyStandards = resJobList.Select(t => t.ProjectClassifyStandard).ToArray();
            //    var projectClassifyStandardIds = SelectClassifyStandardIds(projectClassifyStandards);
            //    var projectAreas = await GetProvincePartsAsync(projectAreaIds);
            //    var projectTypes = await GetProjectTypePartsAsync(projectTypeIds);
            //    var projectIndustryClassifications = await GetIndustryClassificationPartsAsync(projectClassifyStandardIds);
            //    foreach (var jobModel in resJobList)
            //    {
            //        var projectArea = projectAreas.FirstOrDefault(t => t.PomId == jobModel.ProjectAreaId);
            //        if (projectArea != null)
            //        {
            //            jobModel.CountryRegion = projectArea.Overseas == "0" ? "中国" : projectArea.Zaddvsname;
            //            jobModel.ProjectAreaName = projectArea.Overseas == "0" ? projectArea.Zaddvsname : string.Empty;
            //        }
            //        jobModel.ProjectTypeName = projectTypes.FirstOrDefault(t => t.PomId == jobModel.ProjectTypeId)?.Name;
            //        jobModel.ProjectClassifyStandardName = SelectProjectClassifyStandardName(projectIndustryClassifications, jobModel.ProjectClassifyStandard);
            //    }
            //}

            #endregion

            var projectTypeIds = resJobList.Where(t => t.ProjectTypeId != null).Select(t => (Guid)t.ProjectTypeId).Distinct().ToArray();
            var projectCompanyIds = resJobList.Where(t => t.ProjectCompanyId != null).Select(t => t.ProjectCompanyId).Distinct().ToArray();
            var projectCurrencyIds = resJobList.Where(t => t.ProjectCurrencyId != null).Select(t => t.ProjectCurrencyId).Distinct().ToArray();
            var submiterIds = resJobList.Where(t => t.SubmiterId != null).Select(t => (Guid)t.SubmiterId).Distinct().ToArray();
            var projectTypes = await GetProjectTypesAsync(projectTypeIds);
            var projectCompanys = await GetInstitutionsAsync(projectCompanyIds);
            var projectCurrencys = await GetCurrencysAsync(projectCurrencyIds);
            var submiters = await GetUsersAsync(submiterIds);
            foreach (var resJob in resJobList)
            {
                resJob.ProjectCategoryName = resJob.ProjectCategory == 0 ? "境内" : "境外";
                resJob.ProjectTypeName = projectTypes.FirstOrDefault(t => t.PomId == resJob.ProjectTypeId)?.Name;
                resJob.ProjectCompanyName = projectCompanys.FirstOrDefault(t => t.PomId == resJob.ProjectCompanyId)?.Name;
                resJob.ProjectCurrencyName = projectCurrencys.FirstOrDefault(t => t.PomId == resJob.ProjectCurrencyId)?.Zcurrencyname;
                resJob.SubmiterName = submiters.FirstOrDefault(t => t.Id == resJob.SubmiterId)?.Name;
            }
        }



        /// <summary>
        /// 筛选项目行业分类标准名称
        /// </summary>
        /// <returns></returns>
        private string? SelectProjectClassifyStandardName(List<IndustryClassification> industryClassifications, string? projectClassifyStandardId)
        {
            string name = string.Empty;
            if (projectClassifyStandardId != null)
            {
                var projectClassifyStandardIds = projectClassifyStandardId.Split(",");
                for (int i = 0; i < projectClassifyStandardIds.Length; i++)
                {
                    var industryClassification = industryClassifications.FirstOrDefault(t => t.PomId.ToString() == projectClassifyStandardIds[i]);
                    name += (i == 0 ? industryClassification?.Name : "/" + industryClassification?.Name);
                }
            }
            return name;
        }

        /// <summary>
        /// 筛选项目行业分类标准Id集合
        /// </summary>
        /// <returns></returns>
        private Guid[] SelectClassifyStandardIds(string?[] projectClassifyStandards)
        {
            var projectClassifyStandardIds = new List<string>();
            foreach (var projectClassifyStandard in projectClassifyStandards)
            {
                if (!string.IsNullOrWhiteSpace(projectClassifyStandard))
                {
                    projectClassifyStandardIds.AddRange(projectClassifyStandard.Split(","));
                }
            }
            return projectClassifyStandardIds.Where(t => Guid.TryParse(t, out Guid guid)).Select(t => t.ToGuid()).ToArray();
        }

        #endregion

        #region 审批任务

        /// <summary>
        /// 审批任务
        /// <para>业务1：<paramref name="model.IsPassLevelApprove"/>==true,当前登录者可越级审批完成或驳回</para>
        /// <para>业务2：<paramref name="model.IsPassLevelApprove"/>==false,当前登录者必须是审批人进行审批和驳回</para>
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> ApproveJobAsync(ApproveJobRequestDto model)
        {
            model.ResetModelProperty();
            var result = new ResponseAjaxResult<bool>();
            var job = await GetJobAsync(model.JobId);
            if (job == null)
            {
                return result.FailResult(HttpStatusCode.DataNotEXIST, ResponseMessage.DATA_NOTEXIST_JOB);
            }
            var jobApprovers = new List<JobApprover>();
            var nextJobApprovers = new List<JobApprover>();
            JobApprover? jobApprover = null;
            // 是否是越级审批
            if (model.IsPassLevelApprove == true)
            {
                if (model.ApproveStatus == JobApproveStatus.Pass)
                {
                    job.IsFinish = true;
                    job.IsPassLevelApprove = true;
                    job.ApproveLevel = job.FinishApproveLevel;
                }
            }
            else
            {
                jobApprovers = await GetJobApproversAsync(model.JobId);
                // 判断当前登陆人是否为当前层级的审批人
                jobApprover = jobApprovers.FirstOrDefault(t => t.ApproveLevel == job.ApproveLevel + 1 && t.ApproverId == _currentUser.Id);
                if (jobApprover == null)
                {
                    return result.FailResult(HttpStatusCode.NotAllowChange, ResponseMessage.NOTALLOW_CHANGE_JOB);
                }
                if (model.ApproveStatus == JobApproveStatus.Pass && job.FinishApproveLevel <= jobApprover.ApproveLevel)
                {
                    job.IsFinish = true;
                }
                if (!job.IsFinish && model.ApproveStatus == JobApproveStatus.Pass)
                {
                    if (model.NextApprovers == null || !model.NextApprovers.Any())
                    {
                        return result.FailResult(HttpStatusCode.ApproveFail, "下一级审批人不能为空");
                    }
                    nextJobApprovers = model.NextApprovers.Select(t => new JobApprover()
                    {
                        Id = GuidUtil.Next(),
                        JobId = job.Id,
                        ApproveLevel = jobApprover.ApproveLevel + 1,
                        ApproverId = t.ApproverId,
                        CreateId = _currentUser.Id,
                        ApproverJobStatus = JobStatus.UnHandle
                    }).ToList();

                }
                job.IsPassLevelApprove = false;
                job.ApproveLevel = jobApprover.ApproveLevel;
            }
            if (model.ApproveStatus == JobApproveStatus.Pass && model is IJobBiz jobBiz)
            {
                job.BizData = jobBiz.GetJsonBizData();
            }
            // 任务
            var nowTime = DateTime.Now;
            job.UpdateId = _currentUser.Id;
            job.ApproverId = _currentUser.Id;
            job.ApproveStatus = model.ApproveStatus;
            job.RejectReason = model.RejectReason;
            job.ApproveTime = nowTime;
            // 审批人
            if (jobApprover != null)
            {
                jobApprover.UpdateId = _currentUser.Id;
                jobApprover.ApproveStatus = model.ApproveStatus;
                jobApprover.ApproverJobStatus = JobStatus.Handled;
                jobApprover.ApproveTime = nowTime;
            }
            // 任务记录
            var jobRecord = new JobRecord()
            {
                JobId = job.Id,
                OperatorId = _currentUser.Id,
                OperateTime = nowTime,
                OperatorName = _currentUser.Name,
                OperateContent = job.IsFinish ? "审批完成" : EnumExtension.GetEnumDescription(job.ApproveLevel),
                ApproveStatus = job.ApproveStatus,
                RejectReason = job.RejectReason,
            };

            // 业务处理
            var bizHandleResult = await HandleBizAsync(job);
            if (bizHandleResult.Code != HttpStatusCode.Success)
            {
                return result.FailResult(bizHandleResult.Code, bizHandleResult.Message);
            }
            await _dbJob.AsUpdateable(job).ExecuteCommandAsync();
            await _dbJobRecord.AsInsertable(jobRecord).ExecuteCommandAsync();
            if (jobApprover != null)
            {
                await _dbJobApprover.AsUpdateable(jobApprover).ExecuteCommandAsync();
            }
            if (nextJobApprovers.Any())
            {
                // 存储下一级审批人
                await _dbJobApprover.AsInsertable(nextJobApprovers).ExecuteCommandAsync();
            }
            // 任务还在审批中则通知下一级审批人
            if (!job.IsFinish && nextJobApprovers.Any())
            {
                var nextApproverIds = nextJobApprovers.Select(t => t.ApproverId).ToArray();
                await SendJitToApproverAsync(nextApproverIds);
            }
            return result.SuccessResult(true);
        }

        /// <summary>
        /// 处理业务
        /// </summary>
        /// <returns></returns>
        private async Task<ResponseAjaxResult<bool>> HandleBizAsync(Domain.Models.Job job)
        {
            var result = new ResponseAjaxResult<bool>();
            if (job.BizModule == BizModule.ProjectWBS)
            {
                if (!job.IsFinish)
                {
                    return result.SuccessResult(true);
                }
                var saveModel = CastDeserializeObject<SaveProjectWBSRequestDto>(job.BizData);
                result = await _projectService.SaveProjectWBSTreeBizAsync(saveModel);
            }
            else if (job.BizModule == BizModule.MonthReport)
            {
                var saveModel = CastDeserializeObject<SaveProjectMonthReportRequestDto>(job.BizData);
                /*
                 * 验证任务提交的数据不可入库的相关逻辑（双层验证）
                 * 1、任务审批未完成，如果前端提交项目月报状态是完成状态不可入库
                 * 2、任务审批未完成，项目月报库表里的月报状态是完成状态不可入库(一般情况不会走这一层验证，除非前端提交状态与后端已存状态不一致)
                 */
                //if (!(job.IsFinish || saveModel.Status != MonthReportStatus.Finish))
                //{
                //    return result.SuccessResult(true);
                //}
                //if (!job.IsFinish)
                //{
                //    // 任务非完成情况下，再次验证已入库的项目月报是否完成
                //    if (await _projectReportService.IsFinishMonthReport(saveModel.ProjectId, saveModel.DateMonth))
                //    {
                //        return result.SuccessResult(true);
                //    }
                //}
                saveModel.JobId = job.Id;
                saveModel.RejectReason = job.RejectReason;
                saveModel.Status = job.IsFinish ? MonthReportStatus.Finish : (job.ApproveStatus == JobApproveStatus.Reject ? MonthReportStatus.ApproveReject : MonthReportStatus.Approveling);
                saveModel.StatusText = GetApproveStatusText(job.IsFinish, job.ApproveLevel, job.ApproveStatus);
                result = await _projectReportService.SaveProjectMonthReportAsync(saveModel);
            }
            else if (job.BizModule == BizModule.BaseLinePlan)
            {
                var saveModel = CastDeserializeObject<BaseLinePlanProject>(job.BizData);
                var baseline = await _dbContext.Queryable<BaseLinePlanProject>().Where(p => p.Id == saveModel.Id).FirstAsync();
                if (!job.IsFinish)
                {
                    if (job.ApproveStatus == JobApproveStatus.None)
                    {
                        baseline.PlanStatus = 4;
                    }
                    else
                    {
                        baseline.PlanStatus = (int)job.ApproveStatus;
                    }

                    if (job.ApproveStatus == JobApproveStatus.Pass && job.ApproveLevel == ApproveLevel.Level1)
                    {
                        baseline.PlanStatus = 5;
                    }
                }
                else
                {
                    baseline.PlanStatus = (int)job.ApproveStatus;
                    if (job.ApproveStatus == JobApproveStatus.Pass)
                    {
                        baseline.SubmitStatus = 2;
                    }
                }
                baseline.RejectReason = job.RejectReason;
                await _dbContext.Updateable(baseline).ExecuteCommandAsync();
                result.SuccessResult(true);
            }
            return result;
        }

        /// <summary>
        /// 强制转换
        /// </summary>
        /// <exception cref="Exception">强制转换异常</exception>
        private T CastDeserializeObject<T>(string? bizData) where T : class
        {
            if (string.IsNullOrWhiteSpace(bizData))
            {
                throw new Exception("bizData不能为空");
            }
            var model = JsonConvert.DeserializeObject<T>(bizData);
            if (model == null)
            {
                throw new Exception("bizData反序列化失败");
            }
            return model;
        }

        #endregion

        #region 审批流程

        /// <summary>
        /// 搜索审批流程
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<JobApproveStepsResponseto>> SearchJobApproveStepsAsync(JobApproveStepsRequestDto model)
        {
            var result = new ResponseAjaxResult<JobApproveStepsResponseto>();
            var resSteps = new List<JobApproveStepsResponseto.ResJobApproveStep>();
            var job = await GetJobAsync(model.JobId);
            if (job == null)
            {
                return result.FailResult(HttpStatusCode.DataNotEXIST, ResponseMessage.DATA_NOTEXIST_JOB);
            }
            for (var level = ApproveLevel.Level1; level <= job.FinishApproveLevel; level++)
            {
                var step = new JobApproveStepsResponseto.ResJobApproveStep()
                {
                    ApproveLevel = level,
                };
                if (level < job.ApproveLevel)
                {
                    step.ApproveStatus = JobApproveStatus.Pass;
                }
                else if (level == job.ApproveLevel)
                {
                    step.ApproveStatus = job.ApproveStatus;
                }
                else
                {
                    step.ApproveStatus = JobApproveStatus.None;
                }
                resSteps.Add(step);
            }
            //当前进入的层级
            var currentApproveLevel = job.ApproveStatus == JobApproveStatus.Reject ? job.ApproveLevel : job.ApproveLevel + 1;
            return result.SuccessResult(new JobApproveStepsResponseto() { Steps = resSteps.ToArray(), CurrentApproveLevel = currentApproveLevel, IsFinish = job.IsFinish });
        }

        #endregion 

        #region 任务创建审批记录

        /// <summary>
        /// 搜索任务创建/审批记录
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<JobRecordsResponseDto>> SearchJobRecordsAsync(JobRecordsRequestDto model)
        {
            var result = new ResponseAjaxResult<JobRecordsResponseDto>();
            var job = await GetJobAsync(model.JobId);
            if (job == null)
            {
                return result.FailResult(HttpStatusCode.DataNotEXIST, ResponseMessage.DATA_NOTEXIST_JOB);
            }
            var records = await GetJobRecordsAsync(model.JobId);
            var resRecords = new List<JobRecordsResponseDto.ResJobRecord>();
            records = records.OrderBy(t => t.OperateTime).ToList();
            foreach (var record in records)
            {
                var resJobRecord = new JobRecordsResponseDto.ResJobRecord();
                resJobRecord = _mapper.Map(record, resJobRecord);
                resRecords.Add(resJobRecord);
            }
            return result.SuccessResult(new JobRecordsResponseDto() { Records = resRecords.ToArray() });
        }

        #endregion

        #region 我提交的审批任务（草稿箱）

        /// <summary>
        /// 搜索我提交的审批任务
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<JobResponseDto>>> SearchSubmittedJobsAsync(SubmittedJobsRequestDto model)
        {
            var result = new ResponseAjaxResult<List<JobResponseDto>>();
            RefAsync<int> total = 0;
            var resJobList = await _dbJob.AsQueryable()
                  .LeftJoin(_dbProject.AsQueryable(), (j, p) => j.ProjectId == p.Id && p.IsDelete == 1)
                  .Where((j, p) => j.BizModule != BizModule.BaseLinePlan)
                  .Where((j, p) => j.CreateId == _currentUser.Id && j.IsDelete == 1)
                  .Select((j, p) => new JobResponseDto()
                  {
                      JobId = j.Id,
                      SubmitTime = j.SubmitTime,
                      ApproveStatus = j.ApproveStatus,
                      ApproverId = j.ApproverId,
                      ApproveLevel = j.ApproveLevel,
                      ApproveTime = j.ApproveTime,
                      BizModule = j.BizModule,
                      BizHandleMessage = j.BizHandleMessage,
                      ProjectId = j.ProjectId,
                      IsFinish = j.IsFinish,
                      ProjectAmount = p.Amount,
                      ProjectName = p.Name,
                      ProjectCategory = p.Category,
                      ProjectCompanyId = p.CompanyId,
                      ProjectTypeId = p.TypeId,
                      ProjectCurrencyId = p.CurrencyId

                  }).OrderByDescending((j) => j.SubmitTime).ToPageListAsync(model.PageIndex, model.PageSize, total);

            var projectTypeIds = resJobList.Where(t => t.ProjectTypeId != null).Select(t => (Guid)t.ProjectTypeId).Distinct().ToArray();
            var projectCompanyIds = resJobList.Where(t => t.ProjectCompanyId != null).Select(t => t.ProjectCompanyId).Distinct().ToArray();
            var projectCurrencyIds = resJobList.Where(t => t.ProjectCurrencyId != null).Select(t => t.ProjectCurrencyId).Distinct().ToArray();
            var projectTypes = await GetProjectTypesAsync(projectTypeIds);
            var companys = await GetInstitutionsAsync(projectCompanyIds);
            var currencys = await GetCurrencysAsync(projectCurrencyIds);
            foreach (var resJob in resJobList)
            {
                resJob.ProjectCategoryName = resJob.ProjectCategory == 0 ? "境内" : "境外";
                resJob.ProjectTypeName = projectTypes.FirstOrDefault(t => t.PomId == resJob.ProjectTypeId)?.Name;
                resJob.ProjectCompanyName = companys.FirstOrDefault(t => t.PomId == resJob.ProjectCompanyId)?.Name;
                resJob.ProjectCurrencyName = currencys.FirstOrDefault(t => t.PomId == resJob.ProjectCurrencyId)?.Zcurrencyname;
                resJob.ApproveStatusText = GetApproveStatusText(resJob.IsFinish, resJob.ApproveLevel, resJob.ApproveStatus);
            }
            return result.SuccessResult(resJobList, total);
        }

        #endregion

        #region 搜索任务消息

        /// <summary>
        /// 搜索任务消息(包含:审核人未审核的任务条数)
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<JobNoticeResponseDto>> SearchJobNoticeAsync()
        {
            var result = new ResponseAjaxResult<JobNoticeResponseDto>();
            var unHandleJobCount = await _dbJob.AsQueryable()
                 .InnerJoin(_dbJobApprover.AsQueryable(), (a, b) => a.Id == b.JobId)
                 .Where((a, b) => b.ApproverId == _currentUser.Id && b.ApproverJobStatus == JobStatus.UnHandle && a.ApproveLevel == b.ApproveLevel - 1 && (a.ApproveStatus == JobApproveStatus.None || a.ApproveStatus == JobApproveStatus.Pass) && a.IsDelete == 1 && b.IsDelete == 1).CountAsync();
            return result.SuccessResult(new JobNoticeResponseDto() { UnHandleJobCount = unHandleJobCount });
        }

        #endregion

        #region  任务的业务数据请求


        /// <summary>
        /// 搜索任务的业务数据
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<JobBizResponseDto<TBizResult>>> SearchJobBizAsync<TBizResult>(JobBizRequestDto model) where TBizResult : class
        {
            var result = new ResponseAjaxResult<JobBizResponseDto<TBizResult>>();
            var jobBizRes = new JobBizResponseDto<TBizResult>() { ProjectId = model.ProjectId, JobId = model.JobId, BizModule = model.BizModule };
            TBizResult? bizResult = null;
            if (model.RequestType == JobBizRequestType.FindJobBizData)
            {
                var job = await GetJobAsync((Guid)model.JobId);
                if (job == null)
                {
                    return result.FailResult(HttpStatusCode.DataNotEXIST, ResponseMessage.DATA_NOTEXIST_JOB);
                }
                if (job.BizModule != model.BizModule)
                {
                    return result.FailResult(HttpStatusCode.DataNotMatch, ResponseMessage.DATA_NOTMATCH_BIZ);
                }
                bizResult = JsonConvert.DeserializeObject<TBizResult>(job.BizData);
            }
            else if (model.RequestType == JobBizRequestType.FindDBBizData)
            {
                //项目结构业务
                if (model.BizModule == BizModule.ProjectWBS)
                {
                    var wbsResult = await _projectService.SearchProjectWBSTreeBizAsync((Guid)model.ProjectId);
                    if (wbsResult.Code != HttpStatusCode.Success)
                    {
                        return result.FailResult(wbsResult.Code, wbsResult.Message);
                    }
                    bizResult = wbsResult.Data as TBizResult;
                }
            }
            jobBizRes.BizData = bizResult;
            return result.SuccessResult(jobBizRes);
        }

        /// <summary>
        /// 搜索任务的业务数据-版本2
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<MonthReportForProjectResponseDto>> SearchMonthReportAsync(JobBizRequestV2Dto model)
        {
            var result = new ResponseAjaxResult<MonthReportForProjectResponseDto>();
            var job = await GetJobAsync(model.JobId);
            if (job == null)
            {
                return result.FailResult(HttpStatusCode.DataNotEXIST, ResponseMessage.DATA_NOTEXIST_JOB);
            }
            if (job.BizModule != model.BizModule)
            {
                return result.FailResult(HttpStatusCode.DataNotMatch, ResponseMessage.DATA_NOTMATCH_BIZ);
            }

            //读取job中的月份 // 解析 JSON 字符串
            JObject json = JObject.Parse(job.BizData);
            int dateMonth = Convert.ToInt32(json["DateMonth"]);

            //获取新的bizData
            var projectMonthReportRequestDto = new ProjectMonthReportRequestDto()
            {
                DateMonth = dateMonth,
                JobId = job.Id,
                ProjectId = job.ProjectId
            };

            result = await _mPService.SearchMonthReportForProjectAsync(projectMonthReportRequestDto);
            return result;
        }
        /// <summary>
        /// 搜索任务的业务数据-版本2
        /// </summary>
        /// <returns></returns>
        //public async Task<ResponseAjaxResult<JobBizResponseDto<TBizResult>>> SearchJobBizV2Async<TBizResult>(JobBizRequestV2Dto model) where TBizResult : class
        //{
        //    var result = new ResponseAjaxResult<JobBizResponseDto<TBizResult>>();
        //    var job = await GetJobAsync(model.JobId);
        //    if (job == null)
        //    {
        //        return result.FailResult(HttpStatusCode.DataNotEXIST, ResponseMessage.DATA_NOTEXIST_JOB);
        //    }
        //    if (job.BizModule != model.BizModule)
        //    {
        //        return result.FailResult(HttpStatusCode.DataNotMatch, ResponseMessage.DATA_NOTMATCH_BIZ);
        //    }
        //    ////获取新的bizData
        //    //var projectMonthReportRequestDto = new ProjectMonthReportRequestDto()
        //    //{ 
        //    //     DateMonth=
        //    //};
        //    //var pWbsData = await _mPService.SearchMonthReportForProjectAsync(projectMonthReportRequestDto);

        //    //读取job中的月份 // 解析 JSON 字符串
        //    JObject json = JObject.Parse(job.BizData);
        //    int dateMonth = Convert.ToInt32(json["DateMonth"]);
        //    //获取历史的
        //    var historyData = await GetProjectProductionValue(job.ProjectId, dateMonth);

        //    var currentYearOffirmProductionValue = historyData.Item1;
        //    var currenYearCollection = historyData.Item3;
        //    var totalYearKaileaOffirmProductionValue = historyData.Item2;
        //    var totalYearCollection = historyData.Item4;
        //    // 解析 JSON 字符串
        //    JObject jsonObject = JObject.Parse(job.BizData);

        //    // 添加新字段及其值
        //    jsonObject["CurrentYearOffirmProductionValue"] = currentYearOffirmProductionValue;
        //    jsonObject["currenYearCollection"] = currenYearCollection;
        //    jsonObject["totalYearKaileaOffirmProductionValue"] = totalYearKaileaOffirmProductionValue;
        //    jsonObject["totalYearCollection"] = totalYearCollection;

        //    // 转换回 JSON 字符串
        //    job.BizData = jsonObject.ToString();

        //    var bizResult = CastDeserializeObject<TBizResult>(job.BizData);
        //    var jobBizRes = new JobBizResponseDto<TBizResult>() { ProjectId = job.ProjectId, JobId = job.Id, BizModule = job.BizModule, BizData = bizResult };

        //    return result.SuccessResult(jobBizRes);
        //}


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

        /// <summary>
        /// 获取层级审批人集合
        /// </summary>
        /// <returns></returns>
        private async Task<List<ResJobApproverDto>> GetResJobApproversAsync(Guid jobId, ApproveLevel approveLevel)
        {
            var jobApprovers = await GetJobApproversAsync(jobId);
            var nextApprovers = jobApprovers.Where(t => t.ApproveLevel == approveLevel).ToArray();
            var users = await GetUsersAsync(nextApprovers.Select(t => t.ApproverId).ToArray());
            var resJobApprovers = new List<ResJobApproverDto>();
            users.ForEach(item =>
            {
                resJobApprovers.Add(new ResJobApproverDto() { ApproverId = item.Id, ApproveLevel = approveLevel, ApproverName = item.Name, ApproverPhone = item.Phone });
            });
            return resJobApprovers;
        }

        #endregion

        #region 搜索任务审批人列表

        /// <summary>
        /// 搜索任务任务审批人列表
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<JobApproverResponseDto>>> SearchJobApproversAsync(JobApproverRequestDto model)
        {
            var result = new ResponseAjaxResult<List<JobApproverResponseDto>>();
            var approvers = new List<JobApproverResponseDto>();
            // 月报审批人集合
            if (model.BizModule == BizModule.MonthReport)
            {
                approvers = await GeJobApproversOfMonthReportAsync(model.ProjectId);
            }
            else if (model.BizModule == BizModule.ProjectWBS)
            {

            }
            return result.SuccessResult(approvers);
        }

        #region 项目月报审批人列表
        /// <summary>
        /// 项目月报审批人列表
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        private async Task<List<JobApproverResponseDto>> GeJobApproversOfMonthReportAsync(Guid projectId)
        {
            //var l1 = new ResJobApprover() { Key = $"{(int)JobApproveLevel.Level1}_08db3bbb-7e36-4e20-8024-ee9c9bc516e3", ApproverId = "08db3bbb-7e36-4e20-8024-ee9c9bc516e3".ToGuid(), ApproverName = "大东", ApproveLevel = JobApproveLevel.Level1, ApproverPhone = "13703993516" };
            //var l2 = new ResJobApprover() { Key = $"{(int)JobApproveLevel.Level2}_08db3bbb-7e36-4e20-8024-ee9c9bc516e3", ApproverId = "08db3bbb-7e36-4e20-8024-ee9c9bc516e3".ToGuid(), ApproverName = "大东", ApproveLevel = JobApproveLevel.Level2, ApproverPhone = "13703993516" };
            //var l3 = new ResJobApprover() { Key = $"{(int)JobApproveLevel.Level3}_08db3bbb-7e36-4e20-8024-ee9c9bc516e3", ApproverId = "08db3bbb-7e36-4e20-8024-ee9c9bc516e3".ToGuid(), ApproverName = "大东", ApproveLevel = JobApproveLevel.Level3, ApproverPhone = "13703993516" };
            //var l1s = new JobApproverResponseDto() { ApproveLevel = JobApproveLevel.Level1, Approvers = new ResJobApprover[] { l1 } };
            //var l2s = new JobApproverResponseDto() { ApproveLevel = JobApproveLevel.Level2, Approvers = new ResJobApprover[] { l2 } };
            //var l3s = new JobApproverResponseDto() { ApproveLevel = JobApproveLevel.Level3, Approvers = new ResJobApprover[] { l3 } };
            //获取一级审批人信息
            var approverList1 = await GetDefaultLevel1ApproversAsync(projectId);
            var level1 = new JobApproverResponseDto() { ApproveLevel = ApproveLevel.Level1, Approvers = approverList1.ToArray() };
            //获取二级审批人信息
            var approverList2 = await GetDefaultLevel2ApproversAsync(projectId);
            var level2 = new JobApproverResponseDto() { ApproveLevel = ApproveLevel.Level2, Approvers = approverList2.ToArray() };
            return new List<JobApproverResponseDto>() { level1, level2 };
        }

        #endregion

        /// <summary>
        /// 获取默认一级审批人列表
        /// </summary>
        /// <returns></returns>
        private async Task<List<ResJobApproverDto>> GetDefaultLevel1ApproversAsync(Guid ProjectId)
        {
            List<ResJobApproverDto> resJobApprovers = new List<ResJobApproverDto>();
            var projectFirst = await _dbProject.AsQueryable().Where(x => x.Id == ProjectId).FirstAsync();
            if (projectFirst != null)
            {
                //获取项目干系人员
                var projectLeaders = await _dbProjectLeader.AsQueryable().Where(x => x.IsDelete == 1 && (x.ProjectId == ProjectId || x.ProjectId == projectFirst.MasterProjectId) && x.IsPresent == true && x.Type == 1).ToListAsync();
                var leaderIds = projectLeaders.Select(x => x.AssistantManagerId).Distinct().ToList();
                if (leaderIds.Any())
                {
                    //获取用户信息
                    var users = await _dbUser.AsQueryable().Where(x => x.IsDelete == 1 && leaderIds.Contains(x.PomId)).ToListAsync();
                    foreach (var item in users)
                    {
                        ResJobApproverDto resJobApprover = new ResJobApproverDto();
                        resJobApprover.Key = $"{(int)ApproveLevel.Level1}_{item.Id}";
                        resJobApprover.ApproverId = item.Id;
                        resJobApprover.ApproverName = item.Name;
                        resJobApprover.ApproverPhone = item.Phone;
                        resJobApprover.ApproveLevel = ApproveLevel.Level1;
                        resJobApprovers.Add(resJobApprover);
                    }
                }
            }
            return resJobApprovers;
        }

        /// <summary>
        ///  获取默认二级审批人列表
        /// </summary>
        /// <returns></returns>
        private async Task<List<ResJobApproverDto>> GetDefaultLevel2ApproversAsync(Guid ProjectId)
        {
            List<ResJobApproverDto> resJobApprovers = new List<ResJobApproverDto>();
            //获取项目信息
            var projectFirst = await _dbProject.AsQueryable().Where(x => x.Id == ProjectId).FirstAsync();
            List<string> userIds = new List<string>();
            if (projectFirst != null)
            {
                //陈翠
                if (projectFirst.CompanyId == "bd840460-1e3a-45c8-abed-6e66903eb465".ToGuid())
                {
                    userIds = new List<string>() { "2016146340" };
                }
                if (projectFirst.CompanyId == "3c5b138b-601a-442a-9519-2508ec1c1eb2".ToGuid())
                {
                    //朱智文
                    userIds = new List<string>() { "2019013759" };
                }
                if (projectFirst.CompanyId == "a8db9bb0-4667-4320-b03d-b0b7f8728b61".ToGuid())
                {
                    //陈河元
                    userIds = new List<string>() { "2018008722" };
                }
                if (projectFirst.CompanyId == "11c9c978-9ef3-411d-ba70-0d0eed93e048".ToGuid())
                {
                    //王琦
                    userIds = new List<string>() { "2018009624" };
                }
                if (projectFirst.CompanyId == "c0d51e81-03dd-4ef8-bd83-6eb1355879e1".ToGuid())
                {
                    //孟浩
                    // userIds = new List<string>() { "2017005354" };
                    //朱晴
                    userIds = new List<string>() { "2016027005" };
                }
                if (projectFirst.CompanyId == "65052a94-6ea7-44ba-96b4-cf648de0d28a".ToGuid())
                {
                    //李倩
                    userIds = new List<string>() { "2020012309" };
                }
                if (projectFirst.CompanyId == "5a8f39de-8515-456b-8620-c0d01c012e04".ToGuid())
                {
                    //杨加录
                    userIds = new List<string>() { "2016042370" };
                }
                if (projectFirst.CompanyId == "ef7bdb95-e802-4bf5-a7ae-9ef5205cd624".ToGuid())
                {
                    //赵锐
                    userIds = new List<string>() { "L20080287" };
                }
                if (projectFirst.CompanyId == "01ff7a0e-e827-4b46-9032-0a540ce1fba3".ToGuid())
                {
                    //冀登辉
                    userIds = new List<string>() { "2017016771" };
                }
                if (userIds.Any())
                {
                    //获取用户信息
                    var userList = await _dbUser.AsQueryable().Where(x => x.IsDelete == 1 && userIds.Contains(x.LoginAccount)).ToListAsync();
                    foreach (var item in userList)
                    {
                        ResJobApproverDto resJobApprover = new ResJobApproverDto();
                        resJobApprover.Key = $"{(int)ApproveLevel.Level2}_{item.Id}";
                        resJobApprover.ApproverId = item.Id;
                        resJobApprover.ApproverName = item.Name;
                        resJobApprover.ApproverPhone = item.Phone;
                        resJobApprover.ApproveLevel = ApproveLevel.Level2;
                        resJobApprovers.Add(resJobApprover);
                    }
                }
            }
            return resJobApprovers;
        }

        #endregion

        #region 获取下一级审批人集合

        /// <summary>
        /// 获取下一级审批人集合
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<ResJobApproverDto>>> SearchJobNextApproversAsync(JobApproverRequestDto model)
        {
            var result = new ResponseAjaxResult<List<ResJobApproverDto>>();
            ApproveLevel nextLevel = ApproveLevel.Level1;
            Guid projectId = model.ProjectId;
            BizModule bizModule = model.BizModule;
            if (model.JobId != null)
            {
                var job = await GetJobAsync((Guid)model.JobId);
                if (job == null)
                {
                    return result.FailResult(HttpStatusCode.DataNotEXIST, ResponseMessage.DATA_NOTEXIST_JOB);
                }
                if (job.ApproveStatus == JobApproveStatus.Pass)
                {
                    nextLevel = job.ApproveLevel + 1;
                }
                else
                {
                    nextLevel = job.ApproveLevel + 2;
                }

                projectId = job.ProjectId;
                bizModule = job.BizModule;
            }
            var approvers = await GetProjectApproversAsync(projectId, bizModule, nextLevel);
            var users = await GetUsersAsync(approvers.Select(t => t.ApproverId).ToArray());
            var resApprovers = new List<ResJobApproverDto>();
            if (nextLevel == ApproveLevel.Level1)
            {
                resApprovers.AddRange((await GetDefaultLevel1ApproversAsync(model.ProjectId)));
            }
            else if (nextLevel == ApproveLevel.Level2)
            {
                resApprovers.AddRange((await GetDefaultLevel2ApproversAsync(model.ProjectId)));
            }
            approvers.ForEach(item =>
            {
                var user = users.FirstOrDefault(t => t.Id == item.ApproverId);
                if (user == null)
                {
                    return;
                }
                var key = $"{(int)item.ApproveLevel}_{item.ApproverId}";
                if (resApprovers.Any(t => t.Key == key))
                {
                    return;
                }
                var resApprover = new ResJobApproverDto()
                {
                    Key = key,
                    ApproveLevel = item.ApproveLevel,
                    ApproverId = item.ApproverId,
                    ApproverName = user.Name,
                    ApproverPhone = user.Phone
                };
                resApprovers.Add(resApprover);
            });
            return result.SuccessResult(resApprovers);
        }


        #endregion

        /// <summary>
        /// 获取审批状态内容
        /// </summary>
        /// <returns></returns>
        private string GetApproveStatusText(bool isFinish, ApproveLevel approveLevel, JobApproveStatus approveStatus)
        {
            if (isFinish)
            {
                return "已完成";
            }
            else if (approveStatus == JobApproveStatus.Reject)
            {
                return EnumExtension.GetEnumDescription(approveLevel) + "已驳回";
            }
            else if (approveStatus == JobApproveStatus.None || approveStatus == JobApproveStatus.Pass)
            {
                return EnumExtension.GetEnumDescription(approveLevel + 1) + "中";
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取任务审批人集合
        /// </summary>
        /// <param name="jobId">任务Id</param>
        /// <returns></returns>
        private async Task<List<JobApprover>> GetJobApproversAsync(Guid jobId)
        {
            return await _dbJobApprover.GetListAsync(t => t.JobId == jobId && t.IsDelete == 1);
        }

        /// <summary>
        /// 获取任务记录集合
        /// </summary>
        /// <param name="jobId">任务Id</param>
        /// <returns></returns>
        private async Task<List<JobRecord>> GetJobRecordsAsync(Guid jobId)
        {
            return await _dbJobRecord.GetListAsync(t => t.JobId == jobId && t.IsDelete == 1);
        }

        /// <summary>
        /// 是否存在未完成的项目月报任务
        /// </summary>
        /// <returns></returns>
        private bool IsExistsUnFinishMonthReportJob(Guid projectId, int datemonth)
        {
            return _dbJob.Count(t => t.IsDelete == 1 && t.ProjectId == projectId && t.DateMonth == datemonth && t.BizModule == BizModule.MonthReport && t.IsFinish == false) > 0;
        }

        /// <summary>
        /// 获取一个任务
        /// </summary>
        /// <param name="jobId">任务Id</param>
        /// <returns></returns>
        private async Task<Domain.Models.Job?> GetJobAsync(Guid jobId)
        {
            return await _dbJob.GetFirstAsync(t => t.Id == jobId && t.IsDelete == 1);
        }

        /// <summary>
        /// 获取项目类型集合
        /// </summary>
        /// <returns></returns>
        private async Task<List<ProjectType>> GetProjectTypesAsync(Guid[] projectTypeIds)
        {
            if (projectTypeIds == null || !projectTypeIds.Any())
            {
                return new List<ProjectType>();
            }
            return await _dbProjectType.GetListAsync(t => projectTypeIds.Contains(t.PomId) && t.IsDelete == 1);
        }

        /// <summary>
        /// 获取用户信息集合
        /// </summary>
        /// <returns></returns>
        private async Task<List<Domain.Models.User>> GetUsersAsync(Guid[] userIds)
        {
            if (userIds == null || !userIds.Any())
            {
                return new List<Domain.Models.User>();
            }
            return await _dbUser.GetListAsync(t => t.IsDelete == 1 && userIds.Contains(t.Id));
        }

        /// <summary>
        /// 获取公司（机构）信息集合
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
        /// 获取币集合
        /// </summary>
        /// <returns></returns>
        private async Task<List<Currency>> GetCurrencysAsync(Guid?[] currencyIds)
        {
            if (currencyIds == null || !currencyIds.Any())
            {
                return new List<Currency>();
            }
            return await _dbCurrency.GetListAsync(t => currencyIds.Contains(t.PomId) && t.IsDelete == 1);
        }

        /// <summary>
        /// 获取项目层级审批人集合
        /// </summary>
        /// <returns></returns>
        private async Task<List<ProjectApprover>> GetProjectApproversAsync(Guid projectId, BizModule bizModule, ApproveLevel level)
        {
            return await _dbProjectApprover.GetListAsync(t => t.IsDelete == 1 && t.ProjectId == projectId && t.BizModule == bizModule && t.ApproveLevel == level);
        }

        /// <summary>
        /// 获取一个项目(部分字段（Id,Name,PomId,Category，CompanyId,CurrencyId ）)
        /// </summary>
        /// <param name="projectId">项目id</param>
        /// <returns></returns>
        private async Task<Project?> GetProjectPartAsync(Guid projectId)
        {
            return await _dbProject.AsQueryable().Where(t => t.Id == projectId && t.IsDelete == 1).Select(t => new Project() { Id = t.Id, PomId = t.PomId, Name = t.Name, Category = t.Category, CurrencyId = t.CurrencyId, CompanyId = t.CompanyId }).FirstAsync();
        }
    }
}
