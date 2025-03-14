using AutoMapper;
using GHMonitoringCenterApi.Application.Contracts.Dto;
using GHMonitoringCenterApi.Application.Contracts.Dto.Enums;
using GHMonitoringCenterApi.Application.Contracts.Dto.Job;
using GHMonitoringCenterApi.Application.Contracts.Dto.BaseLinePlan;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report;
using GHMonitoringCenterApi.Application.Contracts.IService.BaseLinePlan;
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

namespace GHMonitoringCenterApi.Application.BaseLinePlan
{

    /// <summary>
    /// 项目审批人业务层
    /// </summary>
    public class BaseLinePlanApproverService : IBaseLinePlanApproverService
    {

        #region 注入实例

        /// <summary>
        /// 项目
        /// </summary>
        private readonly IBaseRepository<BaseLinePlanProject> _dbbaseLinePlanProject;

        /// <summary>
        /// 项目审批人
        /// </summary>
        private readonly IBaseRepository<BaseLinePlanApprover> _dbbaseLinePlanApprover;

        /// <summary>
        /// 用户表
        /// </summary>
        private readonly IBaseRepository<Domain.Models.User> _dbUser;

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
        public BaseLinePlanApproverService(IBaseRepository<BaseLinePlanProject> dbbaseLinePlanProject, IBaseRepository<BaseLinePlanApprover> dbbaseLinePlanApprover, IBaseRepository<User> dbUser, IMapper mapper, GlobalObject globalObject, ISqlSugarClient dbContext)
        {
            _dbbaseLinePlanProject = dbbaseLinePlanProject;
            _dbbaseLinePlanApprover = dbbaseLinePlanApprover;
            _dbUser = dbUser;
            _mapper = mapper;
            _globalObject = globalObject;
            _dbContext = dbContext;
        }

        /// <summary>
        /// 保存项目审批人
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> SaveBaseLinePlanApproverAsync(SaveBaseLinePlanApproverRequestDto model)
        {
            var result = new ResponseAjaxResult<bool>();
            BaseLinePlanApprover? approver = null;
            bool isAdd = false;
            var project = await GetProjectPartAsync(model.CompanyId);
            if (project == null)
            {
                return result.FailResult(HttpStatusCode.DataNotEXIST, ResponseMessage.DATA_NOTEXIST_PROJECT);
            }
            if (model.Id == null)
            {
                approver = new BaseLinePlanApprover() { Id = GuidUtil.Next(), CreateId = _currentUser.Id };
                isAdd = true;
            }
            else
            {
                approver = await GetBaseLinePlanApproverAsync((Guid)model.ApproverId);
                if (approver == null)
                {
                    return result.FailResult(HttpStatusCode.DataNotEXIST, "未找到基准计划审批人数据");
                }
            }
            _mapper.Map(model, approver);
            if (isAdd)
            {
                await _dbbaseLinePlanApprover.AsInsertable(approver).EnableDiffLogEvent(NewLogInfo(approver.Id)).ExecuteCommandAsync();
            }
            else
            {
                await _dbbaseLinePlanApprover.AsUpdateable(approver).EnableDiffLogEvent(NewLogInfo(approver.Id)).ExecuteCommandAsync();
            }
            return result.SuccessResult(true);
        }

        /// <summary>
        /// 移除项目审批人
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> RemoveBaseLinePlanApproverAsync(RemoveBaseLinePlanApproverRequestDto model)
        {
            var result = new ResponseAjaxResult<bool>();
            var approver = new BaseLinePlanApprover() { Id = model.BaseLinePlanApproverId };
            approver.IsDelete = 0;
            approver.DeleteId = _currentUser.Id;
            approver.DeleteTime = DateTime.Now;
            await _dbbaseLinePlanApprover.AsUpdateable(approver).UpdateColumns(t => new { t.DeleteId, t.DeleteTime, t.IsDelete }).EnableDiffLogEvent(NewLogInfo(approver.Id)).ExecuteCommandAsync();
            return result.SuccessResult(true);
        }

        /// <summary>
        /// 获取审批人列表
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<BaseLinePlanApproverResponseDto>>> GetBaseLinePlanApproversAsync(BaseLinePlanApproverRequestDto model)
        {
            var result = new ResponseAjaxResult<List<BaseLinePlanApproverResponseDto>>();
            if (model.CompanyId==null)
            {
                model.CompanyId = _currentUser.CurrentLoginInstitutionId;
            }
            //var project = await GetProjectPartAsync(model.CompanyId);
            //if (project == null)
            //{
            //    return result.FailResult(HttpStatusCode.DataNotEXIST, ResponseMessage.DATA_NOTEXIST_PROJECT);
            //}
            var list = new List<BaseLinePlanApproverResponseDto>();

            // 默认一级审批用户
            var defaultLevel1Users = await GetDeafultLevel1ApproveUsersAsync(model.CompanyId);
            // 默认二级审批用户
            var defaultLevel2Users = await GetDeafultLevel2ApproveUsersAsync(model.CompanyId);
            var projectApprovers = await _dbbaseLinePlanApprover.AsQueryable().Where(t => t.IsDelete == 1 && t.CompanyId == model.CompanyId && t.BizModule == model.BizModule).Select(t => new BaseLinePlanApproverResponseDto()
            {
                Id = t.Id,
                ApproveLevel = t.ApproveLevel,
                ApproverId = t.ApproverId
            }).ToListAsync();
            var users = await GetUsersAsync(projectApprovers.Select(t => t.ApproverId).Distinct().ToArray());
            users.AddRange(defaultLevel1Users);
            users.AddRange(defaultLevel2Users);
            list.AddRange(projectApprovers);
            list.ForEach(item =>
            {
                var user = users.FirstOrDefault(t => t.Id == item.ApproverId);
                item.BizModule = model.BizModule;
                item.ApproverName = user?.Name;
                item.ApproverPhone = user?.Phone;
            });
            list = list.OrderByDescending(t => t.IsDefaultApprover).OrderBy(t => t.ApproveLevel).ToList();
            return result.SuccessResult(list);
        }

        /// <summary>
        ///  获取默认一级级审批用户
        /// </summary>
        private async Task<List<Domain.Models.User>> GetDeafultLevel1ApproveUsersAsync(Guid? CompanyID)
        {
            var users = new List<Domain.Models.User>();
            return users;
        }

        /// <summary>
        /// 获取默认二级审批用户
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
        private async Task<List<Domain.Models.User>> GetDeafultLevel2ApproveUsersAsync(Guid? CompanyID)
        {
            var project = await GetProjectPartAsync(CompanyID);
            var users = new List<Domain.Models.User>();
            return users;
        }

        /// <summary>
        /// 获取一个基准计划审批人
        /// </summary>
        /// <returns></returns>
        private async Task<BaseLinePlanApprover> GetBaseLinePlanApproverAsync(Guid Id)
        {
            return await _dbbaseLinePlanApprover.GetFirstAsync(t => t.IsDelete == 1 && t.Id == Id);
        }

        /// <summary>
        /// 获取一个基准计划
        /// </summary>
        /// <param name="Companyid">项目id</param>
        /// <returns></returns>
        private async Task<BaseLinePlanProject?> GetProjectPartAsync(Guid? Companyid)
        {
            return await _dbbaseLinePlanProject.AsQueryable().Where(t => t.CompanyId == Companyid && t.IsDelete == 1).FirstAsync();
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
