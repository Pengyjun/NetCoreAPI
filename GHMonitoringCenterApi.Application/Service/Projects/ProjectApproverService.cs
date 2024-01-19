using AutoMapper;
using GHMonitoringCenterApi.Application.Contracts.Dto;
using GHMonitoringCenterApi.Application.Contracts.Dto.Enums;
using GHMonitoringCenterApi.Application.Contracts.Dto.Job;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project.Approver;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report;
using GHMonitoringCenterApi.Application.Contracts.IService.Project;
using GHMonitoringCenterApi.Domain.Enums;
using GHMonitoringCenterApi.Domain.IRepository;
using GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Domain.Shared;
using GHMonitoringCenterApi.Domain.Shared.Const;
using GHMonitoringCenterApi.Domain.Shared.Enums;
using GHMonitoringCenterApi.Domain.Shared.Util;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;
using Org.BouncyCastle.Asn1.Cmp;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Service.Projects
{

    /// <summary>
    /// 项目审批人业务层
    /// </summary>
    public class ProjectApproverService : IProjectApproverService
    {

        #region 注入实例

        /// <summary>
        /// 项目
        /// </summary>
        private readonly IBaseRepository<Project> _dbProject;

        /// <summary>
        /// 项目审批人
        /// </summary>
        private readonly IBaseRepository<ProjectApprover> _dbProjectApprover;

        /// <summary>
        /// 用户表
        /// </summary>
        private readonly IBaseRepository<Domain.Models.User> _dbUser;

        /// <summary>
        /// 项目干系人
        /// </summary>
        private readonly IBaseRepository<ProjectLeader> _dbProjectLeader;

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

        /// <summary>
        /// 当前登录用户
        /// </summary>
        private CurrentUser _currentUser { get { return _globalObject.CurrentUser; } }

        #endregion 
        /// <summary>
        /// 构造
        /// </summary>
        public ProjectApproverService(GlobalObject globalObject, IMapper mapper, ISqlSugarClient dbContext, IBaseRepository<Project> dbProject, IBaseRepository<ProjectApprover> dbProjectApprover, IBaseRepository<Domain.Models.User> dbUser, IBaseRepository<ProjectLeader> dbProjectLeader)
        {
            _globalObject = globalObject;
            _dbContext = dbContext;
            _dbProject = dbProject;
            _dbProjectApprover = dbProjectApprover;
            _dbUser = dbUser;
            _dbProjectLeader = dbProjectLeader;
            _mapper = mapper;
        }

        /// <summary>
        /// 保存项目审批人
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> SaveProjectApproverAsync(SaveProjectApproverRequestDto model)
        {
            var result = new ResponseAjaxResult<bool>();
            ProjectApprover? approver = null;
            bool isAdd = false;
            var project = await GetProjectPartAsync(model.ProjectId);
            if (project == null)
            {
                return result.FailResult(HttpStatusCode.DataNotEXIST, ResponseMessage.DATA_NOTEXIST_PROJECT);
            }
            if (model.ProjectApproverId == null)
            {
                approver = new ProjectApprover() { Id = GuidUtil.Next(), CreateId = _currentUser.Id };
                isAdd = true;
            }
            else
            {
                approver = await GetProjectApproverAsync((Guid)model.ProjectApproverId);
                if (approver == null)
                {
                    return result.FailResult(HttpStatusCode.DataNotEXIST, "未找到项目审批人数据");
                }
            }
            _mapper.Map(model, approver);
            if (isAdd)
            {
                await _dbProjectApprover.AsInsertable(approver).EnableDiffLogEvent(NewLogInfo(approver.Id)).ExecuteCommandAsync();
            }
            else
            {
                await _dbProjectApprover.AsUpdateable(approver).EnableDiffLogEvent(NewLogInfo(approver.Id)).ExecuteCommandAsync();
            }
            return result.SuccessResult(true);
        }

        /// <summary>
        /// 移除项目审批人
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> RemoveProjectApproverAsync(RemoveProjectApproverRequestDto model)
        {
            var result = new ResponseAjaxResult<bool>();
            var approver = new ProjectApprover() { Id = model.ProjectApproverId };
            approver.IsDelete = 0;
            approver.DeleteId = _currentUser.Id;
            approver.DeleteTime = DateTime.Now;
            await _dbProjectApprover.AsUpdateable(approver).UpdateColumns(t => new { t.DeleteId, t.DeleteTime, t.IsDelete }).EnableDiffLogEvent(NewLogInfo(approver.Id)).ExecuteCommandAsync();
            return result.SuccessResult(true);
        }

        /// <summary>
        /// 获取审批人列表
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<ProjectApproverResponseDto>>> GetProjectApproversAsync(ProjectApproverRequestDto model)
        {
            var result = new ResponseAjaxResult<List<ProjectApproverResponseDto>>();
            var project = await GetProjectPartAsync(model.ProjectId);
            if (project == null)
            {
                return result.FailResult(HttpStatusCode.DataNotEXIST, ResponseMessage.DATA_NOTEXIST_PROJECT);
            }
            var list = new List<ProjectApproverResponseDto>();
           
            // 默认一级审批用户
            var defaultLevel1Users = await GetDeafultLevel1ApproveUsersAsync(model.ProjectId);
            // 默认二级审批用户
            var defaultLevel2Users = await GetDeafultLevel2ApproveUsersAsync(model.ProjectId);
            var defaultLevel1IApprovers = defaultLevel1Users.Select(t => new ProjectApproverResponseDto() { ApproveLevel = ApproveLevel.Level1, ApproverId = t.Id, IsDefaultApprover = true });
            var defaultLevel2IApprovers = defaultLevel2Users.Select(t => new ProjectApproverResponseDto() { ApproveLevel = ApproveLevel.Level2, ApproverId = t.Id, IsDefaultApprover = true });
            var projectApprovers = await _dbProjectApprover.AsQueryable().Where(t => t.IsDelete == 1 && t.ProjectId == model.ProjectId && t.BizModule == model.BizModule).Select(t => new ProjectApproverResponseDto()
            {
                ProjectApproverId = t.Id,
                ApproveLevel = t.ApproveLevel,
                ApproverId = t.ApproverId
            }).ToListAsync();
            var users = await GetUsersAsync(projectApprovers.Select(t => t.ApproverId).Distinct().ToArray());
            users.AddRange(defaultLevel1Users);
            users.AddRange(defaultLevel2Users);
            list.AddRange(defaultLevel1IApprovers);
            list.AddRange(defaultLevel2IApprovers);
            list.AddRange(projectApprovers);
            list.ForEach(item =>
            {
                var user = users.FirstOrDefault(t => t.Id == item.ApproverId);
                item.BizModule = model.BizModule;
                item.ProjectId = model.ProjectId;
                item.ProjectName = project.Name;
                item.ApproverName = user?.Name;
                item.ApproverPhone = user?.Phone;
            });
            list = list.OrderByDescending(t => t.IsDefaultApprover).OrderBy(t => t.ApproveLevel).ToList();
            return result.SuccessResult(list);
        }

        /// <summary>
        ///  获取默认一级级审批用户
        /// </summary>
        private async Task<List<Domain.Models.User>> GetDeafultLevel1ApproveUsersAsync(Guid ProjectId)
        {
            var users = new List<Domain.Models.User>();
            var project = await GetProjectPartAsync(ProjectId);
            if (project != null)
            {
                //获取项目干系人员
                var projectLeaders = await _dbProjectLeader.AsQueryable().Where(x => x.IsDelete == 1 && (x.ProjectId == ProjectId || x.ProjectId == project.MasterProjectId) && x.IsPresent == true && x.Type == 1).ToListAsync();
                var leaderIds = projectLeaders.Select(x => x.AssistantManagerId).Distinct().ToList();
                if (leaderIds.Any())
                {
                    //获取用户信息
                    users = await _dbUser.AsQueryable().Where(x => x.IsDelete == 1 && leaderIds.Contains(x.PomId)).ToListAsync();
                }
            }
            return users;
        }

        /// <summary>
        /// 获取默认二级审批用户
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        private async Task<List<Domain.Models.User>> GetDeafultLevel2ApproveUsersAsync(Guid ProjectId)
        {
            var project = await GetProjectPartAsync(ProjectId);
            var users = new List<Domain.Models.User>();
            var loginAccounts = new List<string>();
            if (project != null)
            {
                //陈翠
                if (project.CompanyId == "bd840460-1e3a-45c8-abed-6e66903eb465".ToGuid())
                {
                    loginAccounts = new List<string>() { "2016146340" };
                }
                if (project.CompanyId == "3c5b138b-601a-442a-9519-2508ec1c1eb2".ToGuid())
                {
                    //朱智文
                    loginAccounts = new List<string>() { "2017028792" };
                }
                if (project.CompanyId == "a8db9bb0-4667-4320-b03d-b0b7f8728b61".ToGuid())
                {
                    //陈河元
                    loginAccounts = new List<string>() { "2018008722" };
                }
                if (project.CompanyId == "11c9c978-9ef3-411d-ba70-0d0eed93e048".ToGuid())
                {
                    //王琦
                    loginAccounts = new List<string>() { "2018009624" };
                }
                if (project.CompanyId == "c0d51e81-03dd-4ef8-bd83-6eb1355879e1".ToGuid())
                {
                    //孟浩
                    loginAccounts = new List<string>() { "2017005354" };
                }
                if (project.CompanyId == "65052a94-6ea7-44ba-96b4-cf648de0d28a".ToGuid())
                {
                    //李倩
                    loginAccounts = new List<string>() { "2020012309" };
                }
                if (project.CompanyId == "5a8f39de-8515-456b-8620-c0d01c012e04".ToGuid())
                {
                    //杨加录
                    loginAccounts = new List<string>() { "2016042370" };
                }
                if (project.CompanyId == "ef7bdb95-e802-4bf5-a7ae-9ef5205cd624".ToGuid())
                {
                    //赵锐
                    loginAccounts = new List<string>() { "L20080287" };
                }
            }
            if (loginAccounts.Any())
            {
                users = await _dbUser.GetListAsync(t => t.IsDelete == 1 && loginAccounts.Contains(t.LoginAccount));
            }
            return users;
        }

        /// <summary>
        /// 获取一个项目审批人
        /// </summary>
        /// <returns></returns>
        private async Task<ProjectApprover> GetProjectApproverAsync(Guid projectApproverId)
        {
            return await _dbProjectApprover.GetFirstAsync(t => t.IsDelete == 1 && t.Id == projectApproverId);
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

        #region 操作日志

        /// <summary>
        /// New 一个操作日志
        /// </summary>
        /// <returns></returns>
        private LogInfo NewLogInfo(Guid? dataId, Guid? operatorId = null, string? operatorName = null)
        {
            string moduleName = "/项目业务数据/项目与审核审批人";
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
    }
}
