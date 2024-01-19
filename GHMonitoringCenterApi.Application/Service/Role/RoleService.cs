using GHMonitoringCenterApi.Application.Contracts.Dto.Role;
using GHMonitoringCenterApi.Application.Contracts.IService.Role;
using GHMonitoringCenterApi.Domain.IRepository;
using GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Domain.Shared;
using Model = GHMonitoringCenterApi.Domain.Models;
using SqlSugar;
using GHMonitoringCenterApi.Domain.Shared.Const;
using GHMonitoringCenterApi.Domain.Shared.Enums;
using GHMonitoringCenterApi.Domain.Shared.Util;
using UtilsSharp;
using GHMonitoringCenterApi.Application.Contracts.IService;

namespace GHMonitoringCenterApi.Application.Service.Role
{
    /// <summary>
    /// 角色服务实现层
    /// </summary>
    public class RoleService : IRoleService
    {


        #region 依赖注入
        public IBaseRepository<Institution> baseInstitutionRepository { get; set; }
        public IBaseRepository<InstitutionRole> baseInstitutionRoleRepository { get; set; }
        public IBaseRepository<Model.User> baseUserRepository { get; set; }
        public IBaseRepository<Model.Role> baseRoleRepository { get; set; }
        public IBaseRepository<Model.RoleMenu> baseRoleMenuRepository { get; set; }
        public IBaseRepository<Model.UserInstitution> baseUserInstitutionRepository { get; set; }
        public IBaseRepository<Model.Menu> baseMenuRepository { get; set; }

        public IBaseService baseService { get; set; }
        public ISqlSugarClient dbContext { get; set; }
        private readonly GlobalObject _globalObject;
        private CurrentUser _currentUser { get { return _globalObject.CurrentUser; } }
        public RoleService(IBaseRepository<Institution> baseInstitutionRepository, IBaseRepository<InstitutionRole> baseInstitutionRoleRepository, IBaseRepository<Model.User> baseUserRepository, IBaseRepository<Model.Role> baseRoleRepository, IBaseRepository<RoleMenu> baseRoleMenuRepository,
            IBaseRepository<UserInstitution> baseUserInstitutionRepository, IBaseRepository<Model.Menu> baseMenuRepository,
            IBaseService baseService, ISqlSugarClient dbContext, GlobalObject globalObject)
        {
            this.baseInstitutionRepository = baseInstitutionRepository;
            this.baseInstitutionRoleRepository = baseInstitutionRoleRepository;
            this.baseUserRepository = baseUserRepository;
            this.baseRoleRepository = baseRoleRepository;
            this.baseRoleMenuRepository = baseRoleMenuRepository;
            this.baseUserInstitutionRepository = baseUserInstitutionRepository;
            this.baseMenuRepository = baseMenuRepository;
            this.baseService = baseService;
            this.dbContext = dbContext;
            this._globalObject = globalObject;
        }

        #endregion

        #region 新增角色
        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="addOrUpdateRoleRequestDto"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> AddAsync(AddOrUpdateRoleRequestDto addOrUpdateRoleRequestDto, Guid userId)
        {
            #region 针对广航局总部的权限特殊处理
            //如果是广航局总部的话 能看到所有的机构信息 非广航局总部的角色只能看到自己下的机构信息
            //判断当前机构角色是不是广航局总部的角色
            var institutionInfo = await baseService.IsGHJInstitution(addOrUpdateRoleRequestDto.InstitutionId);
            addOrUpdateRoleRequestDto.InstitutionId = institutionInfo == Guid.Empty ? addOrUpdateRoleRequestDto.InstitutionId : institutionInfo;
            #endregion
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            var roleId = GuidUtil.Next();
            Model.Role role = new Model.Role()
            {
                Id = roleId,
                Name = addOrUpdateRoleRequestDto.Name,
                Type = addOrUpdateRoleRequestDto.Type,
                IsAdmin = false,
                Remark = addOrUpdateRoleRequestDto.Remark,
                IsApprove = addOrUpdateRoleRequestDto.IsApprove,
                CreateId = _currentUser.Id,
                OperationType = addOrUpdateRoleRequestDto.OperationType,
            };
            Model.InstitutionRole institutionRole = new InstitutionRole()
            {
                Id = GuidUtil.Next(),
                RoleId = roleId,
                InstitutionId = addOrUpdateRoleRequestDto.InstitutionId
            };
            #region 日志信息
            var logDto = new LogInfo()
            {
                Id = GuidUtil.Increment(),
                BusinessModule = "/系统管理/角色管理/新增",
                BusinessRemark = "/系统管理/角色管理/新增",
                OperationId = _currentUser.Id,
                OperationName = _currentUser.Name,
            };
            #endregion
            await baseInstitutionRoleRepository.AsInsertable(institutionRole).EnableDiffLogEvent(logDto).ExecuteCommandAsync();
            var insertSuccess = await baseRoleRepository.AsInsertable(role).EnableDiffLogEvent(logDto).ExecuteCommandAsync();
            if (insertSuccess > 0)
            {
                responseAjaxResult.Data = true;
                responseAjaxResult.Success(ResponseMessage.OPERATION_INSERT_SUCCESS);
            }
            else
                responseAjaxResult.Fail(ResponseMessage.OPERATION_INSERT_FAIL);
            return responseAjaxResult;
        }
        #endregion

        #region 删除角色
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<bool>> RemoveAsync(Guid id)
        {
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            var roleEntity = await baseRoleRepository.AsQueryable().SingleAsync(x => x.IsDelete == 1 && x.Id == id);
            if (roleEntity != null)
            {
                var userList = await baseInstitutionRoleRepository.AsQueryable().Where(x => x.IsDelete == 1 && x.RoleId == id)
                     .Select(x => new InstitutionRole() { Id = x.Id, UserId = x.UserId, InstitutionId = x.InstitutionId }).ToListAsync();
                if (userList.Any())
                {
                    var isSuccess = await baseInstitutionRoleRepository.DeleteAsync(userList);
                    if (isSuccess)
                    {
                        var userInstitutionList = await baseUserInstitutionRepository.AsQueryable()
                           .Where(x => x.IsDelete == 1 && userList.Select(x => x.UserId).Contains(x.UserId) && x.InstitutionId == userList[0].InstitutionId)
                           .ToListAsync();
                        if (userInstitutionList.Any())
                        {
                            isSuccess = await baseUserInstitutionRepository.DeleteAsync(userInstitutionList);
                            if (isSuccess)
                            {
                                isSuccess = await baseRoleRepository.DeleteAsync(roleEntity);
                                if (isSuccess)
                                {
                                    //清空当前角色下面的redis缓存
                                    var usids=userInstitutionList.Select(x => x.UserId).ToList();
                                    var userAll=await baseUserRepository.AsQueryable().Where(x => x.IsDelete == 1 && usids.Contains(x.Id)).ToListAsync();
                                    if (userList.Any())
                                    {
                                       string[] accountArray=userAll.Select(x => x.LoginAccount).ToArray();
                                        if (accountArray.Any())
                                        { 
                                           await RedisUtil.Instance.DelAsync(accountArray);
                                        }
                                    }

                                    responseAjaxResult.Success(ResponseMessage.OPERATION_UPDATE_SUCCESS);
                                    return responseAjaxResult;
                                }
                            }
                            else
                            {
                                responseAjaxResult.Success(ResponseMessage.OPERATION_UPDATE_SUCCESS);
                                return responseAjaxResult;

                            }
                        }
                        else
                        {
                            isSuccess = await baseRoleRepository.DeleteAsync(roleEntity);
                            if (isSuccess)
                            {
                                responseAjaxResult.Success(ResponseMessage.OPERATION_UPDATE_SUCCESS);
                                return responseAjaxResult;
                            }
                            responseAjaxResult.Success(ResponseMessage.OPERATION_UPDATE_SUCCESS);
                            return responseAjaxResult;

                        }
                    }
                    else
                    {
                        responseAjaxResult.Fail(ResponseMessage.OPERATION_UPDATE_FAIL, HttpStatusCode.UpdateFail);
                        return responseAjaxResult;

                    }
                }
            }
            else
            {
                responseAjaxResult.Fail(ResponseMessage.OPERATION_DATA_NOTEXIST, HttpStatusCode.DataNotEXIST);
            }
            return responseAjaxResult;

        }
        #endregion

        #region 获取当前公司角色信息
        /// <summary>
        /// 获取当前公司角色信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="grule">当前登录者的机构层级关系</param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<RoleResponseDto>>> SearchRoleAsync(Guid id, string grule)
        {
            ResponseAjaxResult<List<RoleResponseDto>> responseAjaxResult = new ResponseAjaxResult<List<RoleResponseDto>>();
            List<RoleResponseDto> roleResponseDtos = new List<RoleResponseDto>();
            var institutionRoleList = await baseInstitutionRoleRepository.AsQueryable().Where(x => x.IsDelete == 1 && x.InstitutionId == id).ToListAsync();
            if (institutionRoleList.Any())
            {
                #region 业务判断
                var institutionInfoawait = await baseInstitutionRepository.AsQueryable().SingleAsync(x => x.IsDelete == 1 && x.PomId == id);
                if (institutionInfoawait != null)
                {
                    if (institutionInfoawait.Grule.Length < grule.Length)
                    {
                        //无权限
                       // responseAjaxResult.Fail(ResponseMessage.OPERATION_NOPERMISSION_FAIL, HttpStatusCode.NoPermission);
                        responseAjaxResult.Success();
                        return responseAjaxResult;
                    }
                }
                #endregion

                var roleIds = institutionRoleList.Select(x => x.RoleId);
                if (roleIds.Any())
                {
                    var roleList = await baseRoleRepository.AsQueryable().Where(x => x.IsDelete == 1 && roleIds.Contains(x.Id)).ToListAsync();
                    if (roleList.Any())
                    {
                        foreach (var item in roleList)
                        {
                            RoleResponseDto roleResponseDto = new RoleResponseDto()
                            {
                                Id = item.Id,
                                Name = item.Name,
                                Type = item.Type.Value,
                                CreateTime = item.CreateTime,
                                IsApprove = item.IsApprove,
                                Remark = item.Remark,
                                OperationType= item.OperationType,
                            };
                            roleResponseDtos.Add(roleResponseDto);
                        }
                    }
                }
            }
            responseAjaxResult.Data = roleResponseDtos;
            responseAjaxResult.Count = roleResponseDtos.Count;
            responseAjaxResult.Success();
            return responseAjaxResult;

        }
        #endregion

        #region 更新角色
        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="addOrUpdateRoleRequestDto"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> UpdateAsync(AddOrUpdateRoleRequestDto addOrUpdateRoleRequestDto, CurrentUser currentUser)
        {
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            var isExist = await baseRoleRepository.AsQueryable().SingleAsync(x => x.IsDelete == 1 && x.Id == addOrUpdateRoleRequestDto.Id);
            if (isExist == null)
            {
                responseAjaxResult.Fail(ResponseMessage.OPERATION_ROLE_NOTEXIST, HttpStatusCode.RoleNotEXIST);
                return responseAjaxResult;
            }
            Model.Role role = new Model.Role()
            {
                Id = addOrUpdateRoleRequestDto.Id,
                Name = addOrUpdateRoleRequestDto.Name,
                Type = addOrUpdateRoleRequestDto.Type,
                IsAdmin = isExist.IsAdmin,
                Remark = addOrUpdateRoleRequestDto.Remark,
                IsApprove = addOrUpdateRoleRequestDto.IsApprove,
                OperationType=addOrUpdateRoleRequestDto.OperationType,
            };
            #region 日志信息
            var logDto = new LogInfo()
            {
                Id = GuidUtil.Increment(),
                BusinessModule = "/系统管理/角色管理/编辑",
                BusinessRemark = "/系统管理/角色管理/编辑",
                OperationId = currentUser.Id,
                OperationName = currentUser.Name,
            };
            #endregion

            var isUpdateSuccess = await baseRoleRepository.AsUpdateable(role)
                .EnableDiffLogEvent(logDto)
                .ExecuteCommandAsync();
            if (isUpdateSuccess > 0)
            {
                responseAjaxResult.Data = true;
                responseAjaxResult.Success(ResponseMessage.OPERATION_UPDATE_SUCCESS);
            }
            else
                responseAjaxResult.Fail(ResponseMessage.OPERATION_UPDATE_FAIL);
            return responseAjaxResult;
        }


        #endregion

        #region 角色分配菜单
        /// <summary>
        /// 角色分配菜单
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="menuIds"></param>
        /// <param name="createId"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> AddMenuAsync(Guid roleId, List<Guid> menuIds)
        {
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            var isExist = await baseRoleRepository.AsQueryable().SingleAsync(x => x.IsDelete == 1 && x.Id == roleId);
            if (isExist == null)
            {
                responseAjaxResult.Fail(ResponseMessage.OPERATION_ROLE_NOTEXIST, HttpStatusCode.RoleNotEXIST);
                return responseAjaxResult;
            }
            var menuList = await baseRoleMenuRepository.AsQueryable().Where(x => x.IsDelete == 1 && x.RoleId == roleId).ToListAsync();
            if (menuList.Any())
            {
                await baseRoleMenuRepository.DeleteAsync(x => x.IsDelete == 1 && x.RoleId == roleId);
            }

            //所有菜单
            var addMenuList = await baseMenuRepository.AsQueryable().Where(x => x.IsDelete == 1).ToListAsync();
            List<Guid> addMenuIds = new List<Guid>();
            //查询传入菜单的根节点
            foreach (var item in menuIds)
            {
                //判断当前传入的值是否为根节点
                if (!addMenuList.Any(x => x.Id == item && x.ParentId == 0))
                {
                    //查询该子节点的父节点
                    var parentId = addMenuList.Where(x => x.Id == item).Select(x => x.ParentId).FirstOrDefault();
                    //查询该父节点的id
                    var parentMenuId = addMenuList.Where(x => x.MId == parentId).Select(x => x.Id).FirstOrDefault();
                    if (!menuIds.Any(x => x == parentMenuId))
                    {
                        addMenuIds.Add(parentMenuId);
                    }
                }
            }
            menuIds.AddRange(addMenuIds);

            List<RoleMenu> roleMenus = new List<RoleMenu>();
            foreach (var item in menuIds)
            {
                roleMenus.Add(new RoleMenu()
                {
                    RoleId = roleId,
                    MenuId = item,
                    Id = GuidUtil.Next(),
                });
            }
            #region 日志信息
            var logDto = new LogInfo()
            {
                Id = GuidUtil.Increment(),
                BusinessModule = "/系统管理/菜单权限/分配菜单权限",
                BusinessRemark = "/系统管理/菜单权限/分配菜单权限",
                OperationId = _currentUser.Id,
                OperationName = _currentUser.Name,
            };
            #endregion
            var insertFlag = await baseRoleMenuRepository.AsInsertable(roleMenus).EnableDiffLogEvent(logDto).ExecuteCommandAsync();
            if (insertFlag > 0)
            {
                responseAjaxResult.Data = true;
                responseAjaxResult.Success(ResponseMessage.OPERATION_SAVE_SUCCESS);
            }
            else
            {
                responseAjaxResult.Success(ResponseMessage.OPERATION_SAVE_FAIL);
            }
            return responseAjaxResult;
        }


        #endregion

        #region 获取当前角色下面的用户
        /// <summary>
        /// 获取当前角色下面的用户
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<RoleUserResponseDto>>> SearchUserCurrentRoleAsync(string KeyWords, int pageIndex, int pageSize, Guid roleId)
        {
            ResponseAjaxResult<List<RoleUserResponseDto>> responseAjaxResult = new ResponseAjaxResult<List<RoleUserResponseDto>>();
            var institutionRoleList = await baseInstitutionRoleRepository.AsQueryable().Where(x => x.IsDelete == 1 && x.RoleId == roleId).ToListAsync();
            if (institutionRoleList.Any())
            {
                RefAsync<int> total = 0;
                var userIds = institutionRoleList.Where(x => !string.IsNullOrWhiteSpace(x.UserId.ToString())).Select(x => x.UserId.Value);
                var roleIds = institutionRoleList.Where(x => !string.IsNullOrWhiteSpace(x.UserId.ToString())).Select(x => x.RoleId.Value);
                if (userIds.Any())
                {
                    var userList = await baseUserRepository.AsQueryable()
                          .Where(x => x.IsDelete == 1 && userIds.Contains(x.Id))
                          .WhereIF(!string.IsNullOrWhiteSpace(KeyWords), x => SqlFunc.Contains(x.Name, KeyWords) || SqlFunc.Contains(x.Phone, KeyWords))
                          .Select(x => new RoleUserResponseDto() { Id = x.Id, PomId = x.PomId, Name = x.Name, Phone = x.Phone, LoginAccount = x.LoginAccount }).ToPageListAsync(pageIndex, pageSize, total);

                    if (userList.Any())
                    {
                        var roleSingle = await baseRoleRepository.AsQueryable().SingleAsync(x => x.Id == roleId);
                        foreach (var user in userList)
                        {
                            user.RoleName = roleSingle?.Name;
                        }
                    }
                    responseAjaxResult.Data = userList;
                    responseAjaxResult.Count = total;
                }
            }
            responseAjaxResult.Success();
            return responseAjaxResult;
        }


        #endregion

        #region 移除当前角色下面的用户
        /// <summary>
        /// 移除当前角色下面的用户
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<bool>> RemoveRoleUserAsync(Guid roleId, Guid institutionId, Guid userId)
        {
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            var roleSingle = await baseInstitutionRoleRepository.AsQueryable().SingleAsync(x => x.IsDelete == 1 && x.UserId == userId && x.RoleId == roleId && x.InstitutionId == institutionId);
            var roleUserCount = await baseInstitutionRoleRepository.AsQueryable().CountAsync(x => x.IsDelete == 1 && x.RoleId == roleId && x.InstitutionId == institutionId);
            if (roleSingle == null)
            {
                responseAjaxResult.Fail(ResponseMessage.OPERATION_DATA_NOTEXIST, HttpStatusCode.DataNotEXIST);
                return responseAjaxResult;
            }
            var currentRoleUserSingle = await baseUserInstitutionRepository.AsQueryable().SingleAsync(x => x.IsDelete == 1 && x.InstitutionId == roleSingle.InstitutionId && x.UserId == userId);
            if (currentRoleUserSingle != null)
            {
                currentRoleUserSingle.IsDelete = 0;
                roleSingle.UserId = null;
                var isDeleteSuccess = await baseUserInstitutionRepository.DeleteAsync(currentRoleUserSingle);
                if (roleUserCount > 1)
                {
                    isDeleteSuccess = await baseInstitutionRoleRepository.DeleteAsync(roleSingle);
                }
                else
                {
                    isDeleteSuccess = await baseInstitutionRoleRepository.UpdateAsync(roleSingle);
                }
                responseAjaxResult.Data = isDeleteSuccess;
                if (isDeleteSuccess)
                {
                    responseAjaxResult.Success(ResponseMessage.OPERATION_DELETE_SUCCESS);
                }
                else
                {
                    responseAjaxResult.Fail(ResponseMessage.OPERATION_DELETE_FAIL);
                }
            }
            else
            {
                responseAjaxResult.Fail(ResponseMessage.OPERATION_DATA_NOTEXIST);
            }
            //var token = HttpContentAccessFactory.Current.Request.Headers["Authorization"].ToString();
            //var md5 = token.TrimAll().ToMd5();
            //var account=_currentUser.Account;
            //if (!string.IsNullOrWhiteSpace(account))
            //{
            //    //删除 redis缓存
            //    var isExist= await RedisUtil.Instance.ExistsAsync(account);
            //    if(isExist)
            //    {
            //        await RedisUtil.Instance.DelAsync(account);
            //    }
            //}
            #region 清除redis缓存
            if (responseAjaxResult.Data)
            {
                var redis = RedisUtil.Instance;
                var userLoginName = await baseUserRepository.AsQueryable()
                    .Where(x => x.IsDelete == 1 && x.Id == userId)
                    .Select(x => x.LoginAccount).SingleAsync();
                redis.Del(userLoginName);
            }
            #endregion
            return responseAjaxResult;
        }
        #endregion

        #region 添加角色用户
        /// <summary>
        /// 添加角色用户
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> AddRoleUserAsync(AddRoleUserRequestDto addRoleUserRequsetDto)
        {
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            List<UserInstitution> UserInstitutions = new List<UserInstitution>();

            #region 业务判断
            if (addRoleUserRequsetDto.RoleId == AppsettingsHelper.GetValue("IsAdmin:AdminRoleId").ToGuid())
            {
                responseAjaxResult.Fail(ResponseMessage.OPERATION_SYSTEMROLE_EXIST, HttpStatusCode.SystemRole);
                return responseAjaxResult;
            }

            if (addRoleUserRequsetDto.RoleUsers.Any())
            {
                
                
                var userIds=addRoleUserRequsetDto.RoleUsers.Select(x => x.UserId).ToList();
                if (userIds.Any())
                {
                    var existRoleUser = await baseUserInstitutionRepository.AsQueryable().Where(x => x.IsDelete == 1 
                    &&x.InstitutionId==addRoleUserRequsetDto.InstitutionId&& userIds.Contains(x.UserId)).ToListAsync();
                    if (existRoleUser != null && existRoleUser.Any())
                    {
                        userIds = existRoleUser.Select(x => x.UserId).ToList();
                        if (userIds.Any())
                        {
                            var userNameList = await baseUserRepository.AsQueryable().Where(x => x.IsDelete == 1 && userIds.Contains(x.Id)).Select(x => x.Name).ToListAsync();
                            var userName = string.Join(",", userNameList.Select(x => x));
                            responseAjaxResult.Fail(string.Format(ResponseMessage.NOT_ROLEUSER_FILE, userName), HttpStatusCode.RoleUserExist);
                        }
                        return responseAjaxResult;
                    }
                }

                var isExist=await baseInstitutionRoleRepository.AsQueryable().Where(x => x.InstitutionId == addRoleUserRequsetDto.InstitutionId &&
                x.RoleId == addRoleUserRequsetDto.RoleId && userIds.Contains(x.UserId)).CountAsync();
                if (isExist > 0)
                {
                    responseAjaxResult.Fail(string.Format(ResponseMessage.NOT_ROLEUSER_FILE, "").TrimAll(), HttpStatusCode.RoleUserExist);
                    return responseAjaxResult;
                }
            }

            #endregion

            List<InstitutionRole> institutionRoles = new List<InstitutionRole>();
            var userEmptyList = await baseInstitutionRoleRepository.AsQueryable()
                .Where(x => string.IsNullOrWhiteSpace(x.UserId.Value.ToString()) && x.InstitutionId == addRoleUserRequsetDto.InstitutionId
                && x.RoleId == addRoleUserRequsetDto.RoleId).ToListAsync();
            if (userEmptyList.Any())
            {
                await baseInstitutionRoleRepository.DeleteAsync(userEmptyList);
            }
            foreach (var item in addRoleUserRequsetDto.RoleUsers)
            {
                institutionRoles.Add(new InstitutionRole()
                {
                    Id = GuidUtil.Next(),
                    InstitutionId = addRoleUserRequsetDto.InstitutionId,
                    RoleId = addRoleUserRequsetDto.RoleId,
                    UserId = item.UserId
                });
            }
            foreach (var item in addRoleUserRequsetDto.RoleUsers)
            {
                UserInstitution UserInstitutionList = new UserInstitution()
                {
                    Id = GuidUtil.Next(),
                    InstitutionId = addRoleUserRequsetDto.InstitutionId,
                    UserId = item.UserId
                };
                UserInstitutions.Add(UserInstitutionList);
            }
            if (institutionRoles.Any())
            {
                #region 日志信息
                var logDto = new LogInfo()
                {
                    Id = GuidUtil.Increment(),
                    BusinessModule = "/系统管理/角色管理/选择用户",
                    BusinessRemark = "/系统管理/角色管理/选择用户",
                    OperationId = _currentUser.Id,
                    OperationName = _currentUser.Name,
                };
                #endregion
                await baseInstitutionRoleRepository.AsInsertable(institutionRoles).EnableDiffLogEvent(logDto).ExecuteCommandAsync();
            }
            if (UserInstitutions != null)
            {

                #region 日志信息
                var logDto = new LogInfo()
                {
                    Id = GuidUtil.Increment(),
                    BusinessModule = "/系统管理/角色管理/选择用户",
                    BusinessRemark = "/系统管理/角色管理/选择用户",
                    OperationId = _currentUser.Id,
                    OperationName = _currentUser.Name,
                };
                #endregion
                await baseUserInstitutionRepository.AsInsertable(UserInstitutions).EnableDiffLogEvent(logDto).ExecuteCommandAsync();
                responseAjaxResult.Data = true;
                responseAjaxResult.Success();
            }
            else
            {
                responseAjaxResult.Data = false;
                responseAjaxResult.Fail(ResponseMessage.OPERATION_INSERT_FAIL, HttpStatusCode.InsertFail);
            }

            #region 清除redis缓存
            if (responseAjaxResult.Data)
            {
                var redis = RedisUtil.Instance;
                var userId = addRoleUserRequsetDto.RoleUsers.Select(x => x.UserId).ToList();
                var userLoginAccount = await baseUserRepository.AsQueryable()
                    .Where(x => x.IsDelete == 1 && userId.Contains(x.Id))
                    .Select(x => x.LoginAccount).ToListAsync();
                string[] userLoginNameList = userLoginAccount.ToArray();
                await redis.DelAsync(userLoginNameList);
            }
            #endregion
            return responseAjaxResult;
        }
        #endregion
    }
}
