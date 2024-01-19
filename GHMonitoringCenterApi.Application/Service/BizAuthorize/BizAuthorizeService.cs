using AutoMapper;
using GHMonitoringCenterApi.Application.Contracts.Dto;
using GHMonitoringCenterApi.Application.Contracts.Dto.BizAuthorize;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project.Approver;
using GHMonitoringCenterApi.Application.Contracts.IService.BizAuthorize;
using GHMonitoringCenterApi.Domain.Enums;
using GHMonitoringCenterApi.Domain.IRepository;
using GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Domain.Shared;
using GHMonitoringCenterApi.Domain.Shared.Util;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Service.Authorize
{
    /// <summary>
    /// 业务授权
    /// </summary>
    public class BizAuthorizeService : IBizAuthorizeService
    {

        #region 注入实例


        /// <summary>
        /// 用户表
        /// </summary>
        private readonly IBaseRepository<Domain.Models.User> _dbUser;

        /// <summary>
        /// 授权业务
        /// </summary>
        private readonly IBaseRepository<AuthorizedUser> _dbAuthorizedBusiness;

        /// <summary>
        /// 机构（公司）
        /// </summary>
        private readonly IBaseRepository<Institution> _dbInstitution;

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

        #endregion 注入实例


        /// <summary>
        /// 当前登录用户
        /// </summary>
        private CurrentUser _currentUser { get { return _globalObject.CurrentUser; } }


        public BizAuthorizeService(ISqlSugarClient dbContext, IMapper mapper, GlobalObject globalObject, IBaseRepository<AuthorizedUser> dbAuthorizedBusiness, IBaseRepository<Institution> dbInstitution, IBaseRepository<Domain.Models.User> dbUser)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _globalObject = globalObject;
            _dbAuthorizedBusiness = dbAuthorizedBusiness;
            _dbUser = dbUser;
            _dbInstitution = dbInstitution;
        }

        /// <summary>
        /// 保存授权用户
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> SaveAuthorizedUserAsync(SaveAuthorizeRequestDto model)
        {
            var result = new ResponseAjaxResult<bool>();
            var authorizedUser = await GetAuthorizedUserAsync(model.UserId, model.BizModule);
            var isAdd = false;
            if (authorizedUser == null)
            {
                authorizedUser = new AuthorizedUser()
                {
                    Id = GuidUtil.Next(),
                    BizModule = model.BizModule,
                    CreateId = _currentUser.Id,
                    UserId = model.UserId,


                };
                isAdd = true;
            }
            else
            {
                authorizedUser.UpdateId = _currentUser.Id;
            }
            authorizedUser.AuthorizeMode = model.AuthorizeMode;
            authorizedUser.ExpireTime = NextExpireTime(model.AuthorizeMode);
            if (isAdd)
            {
                await _dbAuthorizedBusiness.AsInsertable(authorizedUser).ExecuteCommandAsync();
            }
            else
            {
                await _dbAuthorizedBusiness.AsUpdateable(authorizedUser).ExecuteCommandAsync();
            }
            return result.SuccessResult(true);
        }


        /// <summary>
        /// 移除授权用户
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> RemoveAuthorizedUserAsync(RemoveAuthorizeRequestDto model)
        {
            var result = new ResponseAjaxResult<bool>();
            var authorizedUser = new AuthorizedUser() { Id = model.AuthorizeId, IsDelete=0, DeleteId = _currentUser.Id, DeleteTime = DateTime.Now };
            await _dbAuthorizedBusiness.AsUpdateable(authorizedUser).UpdateColumns(t => new { t.DeleteId, t.DeleteTime, t.IsDelete }).ExecuteCommandAsync();
            return result.SuccessResult(true);
        }

        /// <summary>
        /// 是否业务授权
        /// </summary>
        /// <returns></returns>
        public async Task<bool> IsAuthorizedAsync(Guid userId, BizModule bizModule)
        {
            var authorizedUser = await GetAuthorizedUserAsync(userId, bizModule);
            if (authorizedUser != null && authorizedUser.AuthorizeMode == AuthorizeMode.AuthorizeForever)
            {
                return true;
            }
            if (authorizedUser != null && authorizedUser.AuthorizeMode == AuthorizeMode.AuthorizeOneDay && DateTime.Now <= authorizedUser.ExpireTime)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 搜索用户列表 For 授权
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<UserAuthorizeResponseDto>>> SearchUsersForAuthorizeAsync(SearchAuthorizeRequestDto model)
        {
            var result = new ResponseAjaxResult<List<UserAuthorizeResponseDto>>();
            RefAsync<int> total = 0;
            var query = _dbUser.AsQueryable()
                .LeftJoin(_dbAuthorizedBusiness.AsQueryable(), (u, a) => u.Id == a.UserId && a.BizModule == model.BizModule && a.IsDelete == 1)
                .Where((u, a) => u.IsDelete == 1)
                .WhereIF(model.AuthorizeMode!=null, (u, a) => a.AuthorizeMode== model.AuthorizeMode)
                .WhereIF(!string.IsNullOrEmpty(model.KeyWords), (u, a) =>
                   SqlFunc.Contains(u.Name, model.KeyWords)
                || SqlFunc.Contains(u.Phone, model.KeyWords))
                .Select((u, a) => new UserAuthorizeResponseDto
                {
                    UserId = u.Id,
                    UserName = u.Name,
                    UserPhone = u.Phone,
                    CompanyId=u.CompanyId,
                    DepartmentId=u.DepartmentId,
                    AuthorizeId = a.Id,
                    AuthorizeMode = a.AuthorizeMode,
                    ExpireTime = a.ExpireTime,
                });
            var list = await query.ToPageListAsync(model.PageIndex, model.PageSize, total);
            var institutionIds = new List<Guid?>();
            institutionIds.AddRange(list.Select(t => t.CompanyId).Distinct().ToArray());
            institutionIds.AddRange(list.Select(t => t.DepartmentId).Distinct().ToArray());
            var institutions = await  GetInstitutionsAsync(institutionIds.ToArray());
            list.ForEach(item =>
            {
                item.ExpireText = GetExpireText(item.AuthorizeMode, item.ExpireTime);
                item.IsExpired = item.AuthorizeMode == AuthorizeMode.AuthorizeOneDay && item.ExpireTime < DateTime.Now;
                item.UserName = $"{item.UserName}({item.UserPhone})";
                item.CompanyName = institutions.FirstOrDefault(t => t.PomId == item.CompanyId)?.Name;
                item.DepartmentName = institutions.FirstOrDefault(t => t.PomId == item.DepartmentId)?.Name;
            });
            return result.SuccessResult(list, total);
        }

        /// <summary>
        /// 下一次到期时间
        /// </summary>
        /// <returns></returns>
        private DateTime? NextExpireTime(AuthorizeMode authorizeMode)
        {
            if (authorizeMode == AuthorizeMode.AuthorizeOneDay)
            {
                return DateTime.Now.AddDays(1);
            }
            return null;
        }

        /// <summary>
        /// 获取到期时间文本内容
        /// </summary>
        /// <returns></returns>
        private string GetExpireText(AuthorizeMode? authorizeMode, DateTime? expireTime)
        {
            if (authorizeMode == AuthorizeMode.AuthorizeForever)
            {
                return "永不过期";
            }
            else if (expireTime != null)
            {
                return ((DateTime)expireTime).ToString("yyyy-MM-dd HH:mm:ss");
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取一个授权用户
        /// </summary>
        /// <returns></returns>
        private async Task<AuthorizedUser?> GetAuthorizedUserAsync(Guid userId, BizModule BizModule)
        {
            return await _dbAuthorizedBusiness.GetFirstAsync(t => t.IsDelete == 1 && t.BizModule == BizModule && t.UserId == userId);
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

    }
}
