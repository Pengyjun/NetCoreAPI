using GHMonitoringCenterApi.Application.Contracts.Dto.Menu;
using GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Domain.Shared;
using SqlSugar;
using GHMonitoringCenterApi.Application.Contracts.IService.Menu;
using GHMonitoringCenterApi.Domain.Shared.Util;
using GHMonitoringCenterApi.Domain.IRepository;
using Model = GHMonitoringCenterApi.Domain.Models;
using System.Data;
using GHMonitoringCenterApi.Domain.Shared.Const;
using GHMonitoringCenterApi.Domain.Shared.Enums;
using AutoMapper;
using GHMonitoringCenterApi.Application.Contracts.Dto.MenuColumn;
using GHMonitoringCenterApi.Application.Contracts.Dto;
using System.Net.Sockets;
using GHMonitoringCenterApi.Application.Service.OperationLog;
using Newtonsoft.Json;
using UtilsSharp;
using GHMonitoringCenterApi.Application.Contracts.IService.OperationLog;
using System.Text;

namespace GHMonitoringCenterApi.Application.Service.Menu
{


    /// <summary>
    /// 菜单接口 实现层
    /// </summary>
    public class MenuService : IMenuService
    {

        #region 依赖注入
        public IBaseRepository<Model.Menu> baseMenuRepository { get; set; }
        public IBaseRepository<Model.RoleMenu> baseRoleMenuRepository { get; set; }

        public ISqlSugarClient dbContent { get; set; }

        public IMapper mapper { get; set; }
        public ILogService logService { get; set; }
        private readonly GlobalObject _globalObject;
        private CurrentUser _currentUser { get { return _globalObject.CurrentUser; } }

        public MenuService(IBaseRepository<Model.Menu> baseMenuRepository, IBaseRepository<RoleMenu> baseRoleMenuRepository, ISqlSugarClient dbContent, IMapper mapper, ILogService logService, GlobalObject globalObject)
        {
            this.baseMenuRepository = baseMenuRepository;
            this.baseRoleMenuRepository = baseRoleMenuRepository;
            this.dbContent = dbContent;
            this.mapper = mapper;
            this.logService = logService;
            _globalObject = globalObject;
        }
        #endregion

        #region 添加菜单
        /// <summary>
        /// 添加或修改菜单
        /// </summary>
        /// <param name="addOrUpdateMenuRequestDto"></param>
        /// <param name="IsAdmin">只要系统管理员才有权限增加菜单</param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> SaveAsync(AddOrUpdateMenuRequestDto addOrUpdateMenuRequestDto, Guid roleId, bool isAdmin, CurrentUser currentUser)
        {
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();

            if (!isAdmin)
            {
                responseAjaxResult.Fail(ResponseMessage.OPERATION_NOPERMISSION_FAIL, HttpStatusCode.NoPermission);
                return responseAjaxResult;
            }

            var menuEntity = mapper.Map<AddOrUpdateMenuRequestDto, Model.Menu>(addOrUpdateMenuRequestDto);

            if (addOrUpdateMenuRequestDto.RequestType)
            {
                var menuId = GuidUtil.Next();
                var maxMid = await baseMenuRepository.AsQueryable().MaxAsync(x => x.MId);
                var currnetNodeMenuSort = await baseMenuRepository.AsQueryable().Where(x => x.IsDelete == 1 && x.ParentId == addOrUpdateMenuRequestDto.ParentId).MaxAsync(x => x.Sort);
                if (!currnetNodeMenuSort.HasValue)
                {
                    menuEntity.Sort = 1;
                }
                else
                {
                    menuEntity.Sort = currnetNodeMenuSort.Value + 1;
                }
                menuEntity.MId = maxMid.HasValue == false ? 1 : (maxMid + 1);
                menuEntity.Id = menuId;
                RoleMenu roleMenu = new RoleMenu()
                {
                    MenuId = menuId,
                    RoleId = roleId,
                    Id = GuidUtil.Next()
                };
                #region 日志信息
                var logDto = new LogInfo()
                {
                    Id = GuidUtil.Increment(),
                    BusinessModule = "/系统管理/菜单管理/新增",
                    BusinessRemark = "/系统管理/菜单管理/新增",
                    OperationId = _currentUser.Id,
                    OperationName = _currentUser.Name,
                };
                #endregion
                await baseRoleMenuRepository.AsInsertable(roleMenu).EnableDiffLogEvent(logDto).ExecuteCommandAsync();
                await baseMenuRepository.AsInsertable(menuEntity).EnableDiffLogEvent(logDto).ExecuteCommandAsync();

                responseAjaxResult.Data = true;
                responseAjaxResult.Success(ResponseMessage.OPERATION_INSERT_SUCCESS);
            }
            else
            {
                //修改
                var menuSingle = await baseMenuRepository.AsQueryable().SingleAsync(x => x.IsDelete == 1 && x.Id == addOrUpdateMenuRequestDto.Id);
                var oldData = menuSingle;
                if (menuSingle == null)
                {
                    responseAjaxResult.Fail(ResponseMessage.OPERATION_DATA_NOTEXIST, HttpStatusCode.DataNotEXIST);
                    return responseAjaxResult;
                }
                menuSingle.Name = addOrUpdateMenuRequestDto.Name;
                menuSingle.Url = addOrUpdateMenuRequestDto.Url;
                menuSingle.Icon = addOrUpdateMenuRequestDto.Icon;

                #region 日志信息
                var logDto = new LogInfo()
                {
                    Id = GuidUtil.Increment(),
                    BusinessModule = "/系统管理/菜单管理/编辑",
                    BusinessRemark = "/系统管理/菜单管理/编辑",
                    OperationId = _currentUser.Id,
                    OperationName = _currentUser.Name,
                };
                #endregion
                await baseMenuRepository.AsUpdateable(menuEntity).EnableDiffLogEvent(logDto).ExecuteCommandAsync();
                responseAjaxResult.Data = true;
                responseAjaxResult.Success(ResponseMessage.OPERATION_UPDATE_SUCCESS);
            }
            return responseAjaxResult;
        }
        #endregion

        #region 获取所有根菜单
        /// <summary>
        /// 获取所有根菜单  默认查询第一节菜单
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<List<RootMenuResponseDto>>> SearchRootMenuAsync(int parentId = 0)
        {
            ResponseAjaxResult<List<RootMenuResponseDto>> responseAjaxResult = new ResponseAjaxResult<List<RootMenuResponseDto>>();
            responseAjaxResult.Data = await baseMenuRepository.AsQueryable().Where(x => x.IsDelete == 1 && x.ParentId == parentId)
                .Select(x => new RootMenuResponseDto() { Name = x.Name, Mid = x.MId.Value, ParentId = x.ParentId.Value }).ToListAsync();
            responseAjaxResult.Count = responseAjaxResult.Data.Count;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        #endregion

        #region  根据用户ID 查询当前用户菜单
        /// <summary>
        /// 根据用户ID 查询当前用户菜单
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<MenuResponseDto>>> SearchMenuByUserId(Guid userId, Guid roleId)
        {
            ResponseAjaxResult<List<MenuResponseDto>> responseAjaxResult = new ResponseAjaxResult<List<MenuResponseDto>>();
            List<MenuResponseDto> menuResponseDtos = new List<MenuResponseDto>();
            //把选中的节点数据统一放在集合里面
            //List<int> selectNodes = new List<int>();
            var userInstitutionList = await dbContent.Queryable<UserInstitution>().Where(x => x.IsDelete == 1 && x.UserId == userId).ToListAsync();
            if (userInstitutionList.Any())
            {
                var institutionIdList = userInstitutionList.Select(x => x.InstitutionId).ToList();
                if (institutionIdList.Any())
                {
                    var institutionRoleSingle = await dbContent.Queryable<InstitutionRole>()
                        .SingleAsync(x => institutionIdList.Contains(x.InstitutionId) && x.RoleId == roleId && x.UserId == userId);
                    if (institutionRoleSingle != null)
                    {
                        var roleMenuList = await dbContent.Queryable<RoleMenu>().Where(x => x.IsDelete == 1 && x.RoleId == institutionRoleSingle.RoleId).ToListAsync();
                        if (roleMenuList.Any())
                        {
                            var menuIdList = roleMenuList.Select(x => x.MenuId).ToList();
                            if (menuIdList.Any())
                            {
                                var menuList = await dbContent.Queryable<Model.Menu>().Where(x => x.IsDelete == 1 && menuIdList.Contains(x.Id)).OrderBy(x => x.Sort).ToListAsync();
                                //若登录人不是陈翠 则隐藏项目审核审批人菜单
                                List<string> userAccounts = new List<string> { "2016146340", "2022002687" };
                                if (!userAccounts.Contains(_currentUser.Account))
                                {
                                    menuList = menuList.Where(x => x.Name != "项目审核审批人" && x.Name !="全部任务").ToList();
                                }
                                //登录人不是刘国银和大东  隐藏
                                List<string> userAccountss = new List<string> { "2018015149", "2022002687" };
                                if (!userAccountss.Contains(_currentUser.Account))
                                {
                                    menuList = menuList.Where(x => x.Name != "图文推送导出").ToList();
                                }
                                #region 注释代码
                                //if (_currentUser.Account == "2016147624"
                                //    || _currentUser.Account == "2019012604"
                                //    || _currentUser.Account == "2019012610"
                                //    || _currentUser.Account == "2016147113"
                                //    || _currentUser.Account == "2016146569"
                                //    || _currentUser.Account == "2016147295"
                                //    || _currentUser.Account == "2018010691"
                                //    || _currentUser.Account == "2016146977"
                                //    || _currentUser.Account == "L20085885"
                                //    || _currentUser.Account == "2016147728"
                                //    || _currentUser.Account == "L20096392"
                                //    || _currentUser.Account == "2023011142"
                                //    || _currentUser.Account == "2022013570")
                                //{
                                //    menuList = menuList.Where(x =>
                                //    x.Name == "装备管理" ||
                                //    x.Name == "自有船舶月报数据" ||
                                //    x.Name == "分包船舶月报数据" ||
                                //    x.Name == "自有船舶日报数据" ||
                                //    x.Name == "船舶进退场" ||
                                //    x.Name == "分包船舶管理" ||
                                //    x.Name == "船舶滚动修理计划与执行" ||
                                //    x.Name == "非自有设备管理" ||
                                //    x.Name == "船舶成本管理" ||
                                //    x.Name == "项目业务数据" ||
                                //    x.Name == "项目信息清单" ||
                                //    x.Name == "项目与报表负责人"
                                //    ).ToList();

                                //}
                                #endregion

                                if (menuList.Any())
                                {

                                    foreach (var item in menuList)
                                    {
                                        MenuResponseDto menuResponseDto = new MenuResponseDto()
                                        {
                                            Id = item.Id,
                                            KeyId = item.MId.Value.ToString(),
                                            Pid = item.ParentId.Value.ToString(),
                                            Name = item.Name,
                                            Url = item.Url,
                                            Icon = item.Icon,
                                            Sort = item.Sort.Value,
                                            SelectNodes = new List<int>(),
                                        };
                                        menuResponseDtos.Add(menuResponseDto);
                                    }
                                    if (menuResponseDtos.Any())
                                    {

                                        menuResponseDtos = ListToTreeUtil.GetTree("0", menuResponseDtos);
                                        menuResponseDtos = menuResponseDtos.OrderBy(x => x.Sort).ToList();

                                        #region 是否选中字段赋值  前端反选使用
                                        //foreach (var currentMenus in menuResponseDtos)
                                        //{
                                        //    var currentMenuItem = menuList.SingleOrDefault(x => x.IsDelete == 1 && x.Id == currentMenus.Id);
                                        //    currentMenus.IsSelect = currentMenuItem != null ? true : false;
                                        //    selectNodes.Add(currentMenuItem.MId.Value);
                                        //    if (currentMenus.SelectNodes.Any())
                                        //    {
                                        //        foreach (var twoNode in currentMenus.Node)
                                        //        {
                                        //            currentMenuItem = menuList.SingleOrDefault(x => x.IsDelete == 1 && x.Id == twoNode.Id);
                                        //            twoNode.IsSelect = currentMenuItem != null ? true : false;
                                        //            selectNodes.Add(currentMenuItem.MId.Value);
                                        //            if (twoNode.Node.Any())
                                        //            {
                                        //                foreach (var threeNode in twoNode.Node)
                                        //                {
                                        //                    currentMenuItem = menuList.SingleOrDefault(x => x.IsDelete == 1 && x.Id == threeNode.Id);
                                        //                    threeNode.IsSelect = currentMenuItem != null ? true : false;
                                        //                    selectNodes.Add(currentMenuItem.MId.Value);
                                        //                }
                                        //            }
                                        //        }
                                        //    }
                                        //}
                                        #endregion
                                    }
                                }
                            }
                        }
                    }
                }
            }
            //if (menuResponseDtos.Any())
            //{
            //    menuResponseDtos[0].SelectNodes = selectNodes;
            //}
            responseAjaxResult.Data = menuResponseDtos;
            responseAjaxResult.Count = menuResponseDtos.Count;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }


        #endregion

        #region 获取菜单列表
        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<List<MenuListResponseDto>>> SearchMenuColumnAsync(MenuListRequestDto menuListRequestDto)
        {
            RefAsync<int> total = 0;
            ResponseAjaxResult<List<MenuListResponseDto>> responseAjaxResult = new ResponseAjaxResult<List<MenuListResponseDto>>();
            List<MenuListResponseDto> menuColumnResponseDtos = null;
            //把选中的节点数据统一放在集合里面
            List<int> selectNodes = new List<int>();
            var linq = baseMenuRepository.AsQueryable()
                .Where(x => x.IsDelete == 1)
                .WhereIF(!string.IsNullOrWhiteSpace(menuListRequestDto.KeyWords), x => SqlFunc.Contains(x.Name, menuListRequestDto.KeyWords) || SqlFunc.Contains(x.Url, menuListRequestDto.KeyWords)).OrderBy(x => x.Sort)
                .Select(x => new MenuListResponseDto { SelectNodes = new List<int>(), Value = x.Id, Label = x.Name, KeyId = x.MId.ToString(), Pid = x.ParentId.ToString(), Url = x.Url, Icon = x.Icon, Sort = x.Sort, Remark = x.Remark, Createtime = x.CreateTime, Updateid = x.UpdateId, Isdelete = x.IsDelete });
            if (!menuListRequestDto.IsPage)
            {
                //若登录人不是陈翠 则隐藏项目审核审批人菜单
                List<string> userAccounts = new List<string> { "2016146340", "2022002687" };
                if (!userAccounts.Contains(_currentUser.Account))
                {
                    menuColumnResponseDtos = await linq.Where(x => x.Name != "项目审核审批人").ToListAsync();
                }
                else
                {
                    menuColumnResponseDtos = await linq.ToListAsync();
                }
            }
            else
            {
                //若登录人不是陈翠 则隐藏项目审核审批人菜单
                List<string> userAccounts = new List<string> { "2016146340", "2022002687" };
                if (!userAccounts.Contains(_currentUser.Account))
                {
                    menuColumnResponseDtos = await linq.Where(x => x.Name != "项目审核审批人").ToPageListAsync(menuListRequestDto.PageIndex, menuListRequestDto.PageSize, total);
                }
                else
                {
                    menuColumnResponseDtos = await linq.ToPageListAsync(menuListRequestDto.PageIndex, menuListRequestDto.PageSize, total);
                }
            }
            //首页菜单权限菜单显示与隐藏功能
            var dictionaryTableList = await dbContent.Queryable<DictionaryTable>().Where(x => x.TypeNo == 11 && x.Type == 1).Select(x => x.Name).ToListAsync();
            if (menuColumnResponseDtos.Any())
            {
                //若登录人没有永久首页菜单权限 则隐藏月报填报入口管理菜单
                if (!dictionaryTableList.Contains(_currentUser.Account))
                {
                    menuColumnResponseDtos = menuColumnResponseDtos.Where(x => x.Label != "月报填报入口管理").ToList();
                }
            }

            if (menuColumnResponseDtos.Any())
            {
                menuColumnResponseDtos = ListToTreeUtil.GetTree("0", menuColumnResponseDtos);
            }

            if (menuColumnResponseDtos.Any())
            {
                var menu = dbContent.Queryable<Model.RoleMenu>().Where(x => x.IsDelete == 1 && x.RoleId == menuListRequestDto.RoleId);
                var menuIdList = await menu.Select(x => x.MenuId.Value).ToListAsync();
                if (menuIdList.Any())
                {
                    var menuList = await dbContent.Queryable<Model.Menu>().Where(x => x.IsDelete == 1 && menuIdList.Contains(x.Id)).ToListAsync();
                    #region 是否选中字段赋值  前端反选使用
                    foreach (var currentMenus in menuColumnResponseDtos)
                    {
                        var currentMenuItem = menuList.SingleOrDefault(x => x.IsDelete == 1 && x.Id == currentMenus.Value);
                        if (currentMenuItem != null)
                        {
                            currentMenus.IsSelect = currentMenuItem != null ? true : false;
                            selectNodes.Add(currentMenuItem.MId.Value);
                            if (currentMenus.Node != null && currentMenus.Node.Any())
                            {
                                foreach (var twoNode in currentMenus.Node)
                                {
                                    currentMenuItem = menuList.SingleOrDefault(x => x.IsDelete == 1 && x.Id == twoNode.Value);
                                    twoNode.IsSelect = currentMenuItem != null ? true : false;
                                    if (currentMenuItem != null)
                                    {
                                        selectNodes.Add(currentMenuItem.MId.Value);
                                    }
                                    if (currentMenus.SelectNodes != null && twoNode.Node.Any())
                                    {
                                        foreach (var threeNode in twoNode.Node)
                                        {
                                            currentMenuItem = menuList.SingleOrDefault(x => x.IsDelete == 1 && x.Id == threeNode.Value);
                                            threeNode.IsSelect = currentMenuItem != null ? true : false;
                                            selectNodes.Add(currentMenuItem.MId.Value);
                                        }
                                    }
                                }
                            }
                        }

                    }
                    #endregion
                }
            }
            if (menuColumnResponseDtos.Any())
            {
                menuColumnResponseDtos[0].SelectNodes = selectNodes;
            }

            responseAjaxResult.Count = total;
            responseAjaxResult.Data = menuColumnResponseDtos.OrderBy(x => x.Sort).ToList();
            responseAjaxResult.Success();
            return responseAjaxResult;
        }


        #endregion

        #region 删除菜单列表
        /// <summary>
        /// 删除菜单列表
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> DeleteMenuAsync(Guid Id, CurrentUser currentUser)
        {
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            var MenusList = await baseMenuRepository.AsQueryable().SingleAsync(x => x.Id == Id);

            if (MenusList != null)
            {
                #region 日志信息
                var logDto = new LogInfo()
                {
                    Id = GuidUtil.Increment(),
                    BusinessModule = "/系统管理/菜单管理/删除",
                    BusinessRemark = "/系统管理/菜单管理/删除",
                    OperationId = _currentUser.Id,
                    OperationName = _currentUser.Name,
                };
                #endregion
                MenusList.IsDelete = 0;
                //await baseMenuRepository.UpdateAsync(MenusList);               // baseMenuRepository.Updateable(MenusList)
                var b = await baseMenuRepository.AsUpdateable(MenusList).EnableDiffLogEvent(logDto).ExecuteCommandAsync();
                responseAjaxResult.Data = true;
                responseAjaxResult.Success();
            }
            else
            {
                responseAjaxResult.Data = false;
                responseAjaxResult.Success(ResponseMessage.OPERATION_FAIL);
            }
            return responseAjaxResult;
        }


        #endregion

        #region 增加首页菜单权限用户列表
        /// <summary>
        /// 增加首页菜单权限用户列表
        /// </summary>
        /// <param name="basePrimaryRequestDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<bool>> AddHomeMenuPermissionUser(AddHomeUserRequestDto addHomeUserRequestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<bool>();
            if (_currentUser.Id  != "08db4e0d-a531-4691-8dfc-24c2766074ce".ToGuid() && _currentUser.Id != "08db3bbb-7e36-4e20-8024-ee9c9bc516e3".ToGuid())
            {
                responseAjaxResult.Data = false;
                responseAjaxResult.Success(ResponseMessage.OPERATION_NOPERMISSION_FAIL);
                return responseAjaxResult;
            }

            var user = await dbContent.Queryable<Model.User>().Where(x => x.Id == addHomeUserRequestDto.UserId).SingleAsync();
            var userDictionaryTable = new DictionaryTable()
            {
                Id = GuidUtil.Next(),
                Type = addHomeUserRequestDto.Type,
                TypeNo = 11,
                Name = user.GroupCode,
                Remark = user.Name
            };
            //var userGroupCode = await dbContent.Queryable<DictionaryTable>().Where(x => x.TypeNo == 11 && x.Name == user).FirstAsync();
            if (user != null && await dbContent.Queryable<DictionaryTable>().Where(x => x.TypeNo == 11 && x.Name == userDictionaryTable.Name).FirstAsync() == null)
            {
                await dbContent.Insertable(userDictionaryTable).ExecuteCommandAsync();
                responseAjaxResult.Data = true;
                responseAjaxResult.Success(ResponseMessage.OPERATION_SAVE_SUCCESS);
            }
            else
            {
                responseAjaxResult.Data = false;
                responseAjaxResult.Success(ResponseMessage.OPERATION_SAVE_FAIL);
            }
            return responseAjaxResult;
        }
        #endregion

        #region 删除首页菜单权限用户列表
        /// <summary>
        /// 删除首页菜单权限用户列表
        /// </summary>
        /// <param name="basePrimaryRequestDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<bool>> DeleteableHomeMenuPermissionUser(BasePrimaryRequestDto basePrimaryRequestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<bool>();
            if (_currentUser.Id != "08db4e0d-a531-4691-8dfc-24c2766074ce".ToGuid() && _currentUser.Id != "08db3bbb-7e36-4e20-8024-ee9c9bc516e3".ToGuid())
            {
                responseAjaxResult.Data = false;
                responseAjaxResult.Success(ResponseMessage.OPERATION_NOPERMISSION_FAIL, HttpStatusCode.NoPermission);
                return responseAjaxResult;
            }
            if (basePrimaryRequestDto.Id == "08db4e0d-a531-4691-8dfc-24c2766074ce".ToGuid() || basePrimaryRequestDto.Id == "08db3bbb-7e36-4e20-8024-ee9c9bc516e3".ToGuid())
            {
                responseAjaxResult.Data = false;
                responseAjaxResult.Success(ResponseMessage.OPERATION_DELETE_FAIL, HttpStatusCode.DeleteFail);
                return responseAjaxResult;
            }
            var user = await dbContent.Queryable<Model.User>().Where(x => x.Id == basePrimaryRequestDto.Id).Select(x => x.GroupCode).FirstAsync();
            var userGroupCode = await dbContent.Queryable<DictionaryTable>().Where(x => x.TypeNo == 11 && x.Name == user).FirstAsync();
            if (user != null && userGroupCode != null)
            {
                await dbContent.Deleteable(userGroupCode).ExecuteCommandAsync();
                responseAjaxResult.Data = true;
                responseAjaxResult.Success(ResponseMessage.OPERATION_DELETE_SUCCESS);
            }
            else
            {
                responseAjaxResult.Data = false;
                responseAjaxResult.Success(ResponseMessage.OPERATION_DELETE_FAIL);
            }
            return responseAjaxResult;
        }


        #endregion

        #region 定时删除首页菜单权限用户列表
        /// <summary>
        /// 定时删除首页菜单权限用户列表
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> TimeDeleteableHomeMenuPermissionUserAsync()
        {
            var responseAjaxResult = new ResponseAjaxResult<bool>();
            List<string> name = new List<string>()
            {
               "2016146340",
               "2022002687"
            };
            if (DateTime.Now.Year == 25)
            {
                var dictionaryTableList = await dbContent.Queryable<DictionaryTable>().Where(x=>x.TypeNo == 11 && !name.Contains(x.Name) && x.IsDelete == 1 && x.Type == 0).ToListAsync();
                if (dictionaryTableList != null)
                {
                    await dbContent.Deleteable(dictionaryTableList).ExecuteCommandAsync();
                    responseAjaxResult.Data = true;
                    responseAjaxResult.Success(ResponseMessage.OPERATION_DELETE_SUCCESS);
                }
            }
            else
            {
                responseAjaxResult.Data = false;
                responseAjaxResult.Fail(ResponseMessage.OPERATION_DELETE_FAIL);
            }
            return responseAjaxResult;
        }
        #endregion

    }
}
