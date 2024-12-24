
using AutoMapper;
using HNKC.CrewManagePlatform.Models.CommonRequest;
using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Dtos;
using HNKC.CrewManagePlatform.Models.Dtos.Role;
using HNKC.CrewManagePlatform.Models.Enums;
using HNKC.CrewManagePlatform.Services.Interface.CurrentUser;
using HNKC.CrewManagePlatform.SqlSugars.Models;
using HNKC.CrewManagePlatform.Util;
using HNKC.CrewManagePlatform.Utils;
using SqlSugar;
using System.Linq;
using UtilsSharp;
using UtilsSharp.Shared.Standard;
using role = HNKC.CrewManagePlatform.SqlSugars.Models;

namespace HNKC.CrewManagePlatform.Services.Role
{
    /// <summary>
    /// 角色服务实现层
    /// </summary>
    public class RoleService : HNKC.CrewManagePlatform.Services.Interface.CurrentUser.CurrentUserService, IRoleService
    {

        #region 依赖注入
        private readonly ISqlSugarClient _dbContext;
        private readonly IMapper _mapper;

        /// <summary>
        /// 依赖注入
        /// </summary>
        public RoleService(ISqlSugarClient dbContext, IMapper mapper)
        {
            this._mapper = mapper;
            this._dbContext = dbContext;
        }


        #endregion
        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="addRoleRequest"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(AddRoleRequest addRoleRequest)
        {
            var businessId = GuidUtil.Next();
            HNKC.CrewManagePlatform.SqlSugars.Models.Role role = new role.Role()
            {
                BusinessId = businessId,
                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                IsAdmin = false,
                IsApprove = addRoleRequest.IsApprove.Value == 1 ? true : false,
                Name = addRoleRequest.Name,
                Remark = addRoleRequest.Remark,
                Type = addRoleRequest.Type,
            };
           var institutionBuinsessId=await _dbContext.Queryable<Institution>().Where(x => x.IsDelete == 1 && x.Oid == addRoleRequest.Oid).Select(x => x.BusinessId).FirstAsync();
            InstitutionRole institutionRole = new InstitutionRole()
            {
                BusinessId = businessId,
                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                InstitutionBusinessId = institutionBuinsessId,
                RoleBusinessId = businessId,
            };
            await _dbContext.Insertable<HNKC.CrewManagePlatform.SqlSugars.Models.Role>(role).ExecuteCommandAsync();
            await _dbContext.Insertable<InstitutionRole>(institutionRole).ExecuteCommandAsync();
            return true;
        }
        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="addRoleRequest"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Result> ModifyAsync(AddRoleRequest addRoleRequest)
        {
           var roleInfo=await _dbContext.Queryable<HNKC.CrewManagePlatform.SqlSugars.Models.Role>()
               .Where(x => x.IsDelete == 1 && x.BusinessId == addRoleRequest.BId).FirstAsync();
            if (roleInfo == null)
            {
                return Result.Fail("数据不存在",(int)ResponseHttpCode.DataNoExist);
            }
            roleInfo.Name = addRoleRequest.Name;
            roleInfo.Type = addRoleRequest.Type;
            roleInfo.IsApprove = addRoleRequest.IsApprove == 1 ? true : false;
            await _dbContext.Updateable<HNKC.CrewManagePlatform.SqlSugars.Models.Role>(roleInfo).ExecuteCommandAsync();
            return Result.Success();

        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="baseRequest"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Result> RemoveAsync(BaseRequest baseRequest)
        {
            var roleInfo = await _dbContext.Queryable<HNKC.CrewManagePlatform.SqlSugars.Models.Role>()
             .Where(x => x.IsDelete == 1 && x.BusinessId == baseRequest.BId).FirstAsync();
            if (roleInfo == null)
            {
                return Result.Fail("数据不存在", (int)ResponseHttpCode.DataNoExist);
            }

            roleInfo.IsDelete = 0;
           var res= await _dbContext.Queryable<InstitutionRole>()
            .Where(x => x.IsDelete == 1 && x.BusinessId == roleInfo.BusinessId).ToListAsync();
            foreach (var item in res)
            {
                item.IsDelete = 0;
            }
            await _dbContext.Updateable<HNKC.CrewManagePlatform.SqlSugars.Models.Role>(roleInfo).ExecuteCommandAsync();
            await _dbContext.Updateable<InstitutionRole>(res).ExecuteCommandAsync();
            return Result.Success();
        }


        /// <summary>
        /// 查询当前机构下的所有角色
        /// </summary>
        /// <param name="oid"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PageResult<RoleResponse>> SearchCurrentInstitutionRoleAsync(string oid)
        {
            PageResult<RoleResponse>  page = new PageResult<RoleResponse>();
            List<RoleResponse> pageResult = new List<RoleResponse>();
            var institutionBuinsessId = await _dbContext.Queryable<Institution>().Where(x => x.IsDelete == 1 && x.Oid == oid).FirstAsync();
            if (institutionBuinsessId !=null)
            {
              var roleList=  await _dbContext.Queryable<InstitutionRole>().Where(x => x.IsDelete == 1 &&x.InstitutionBusinessId== institutionBuinsessId.BusinessId).ToListAsync();
                if (roleList.Count > 0)
                {
                    var roelBusinessIds = roleList.Select(x => x.BusinessId).Distinct().ToList();
                    var roles = await _dbContext.Queryable<HNKC.CrewManagePlatform.SqlSugars.Models.Role>().Where(x => x.IsDelete == 1 && roelBusinessIds.Contains(x.BusinessId)).ToListAsync();
                    if (roles.Count > 0)
                    {

                        foreach (var item in roelBusinessIds)
                        {
                          var currentRole= roles.Where(x => x.BusinessId == item).FirstOrDefault();
                            RoleResponse roleResponse = new RoleResponse()
                            {
                                InstitutionBusinessId = institutionBuinsessId.BusinessId,
                                IsAdmin = currentRole?.IsAdmin,
                                IsApprove = currentRole?.IsApprove,
                                Name = currentRole?.Name,
                                Type = currentRole?.Type,
                                InstitutionName = institutionBuinsessId.ShortName,
                                BId= currentRole.BusinessId,
                                Remark = currentRole?.Remark,
                                Created = currentRole?.Created,
                            };
                             pageResult.Add(roleResponse);
                        }
                    }
                }
            }
            page.List = pageResult.OrderBy(x=>x.Created).ToList();
            page.TotalCount = pageResult.Count;
            return page;
        }

        /// <summary>
        /// 为用户分配角色
        /// </summary>
        /// <param name="addRoleRequest"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>

       public  async Task<Result>  AddUserRoleAsync(UserRoleRequest  userRoleRequest)
        {
          var institutionRoleInfo= await _dbContext.Queryable<InstitutionRole>().Where(x=>x.IsDelete==1&&x.BusinessId==userRoleRequest.RoleBusinessId
           &&x.InstitutionBusinessId== userRoleRequest.InstitutionBusinessId&&x.UserBusinessId.HasValue==false).ToListAsync();
            if (institutionRoleInfo.Count > 0)
            {
                foreach (var item in institutionRoleInfo)
                {
                    item.IsDelete = 0;
                }
               await _dbContext.Updateable<InstitutionRole>(institutionRoleInfo).
                    ExecuteCommandAsync();
            }
            #region 数据校验
            var institutionBusinessId = await _dbContext.Queryable<Institution>().Where(x => x.IsDelete == 1 && x.BusinessId == userRoleRequest.InstitutionBusinessId).FirstAsync();
            if (institutionBusinessId == null)
            {
                return Result.Fail("机构ID不存在", (int)ResponseHttpCode.DataNoExist);
            }
            var roleBusinessId = await _dbContext.Queryable<HNKC.CrewManagePlatform.SqlSugars.Models.Role>().Where(x => x.IsDelete == 1 && x.BusinessId == userRoleRequest.RoleBusinessId).FirstAsync();
            if (roleBusinessId == null)
            {
                return Result.Fail("角色ID不存在", (int)ResponseHttpCode.DataNoExist);
            }
            var userIds = userRoleRequest.UserBusinessItems.Select(x => x.UserBusinessId).ToList();
            var userBusinessId = await _dbContext.Queryable<User>().Where(x => x.IsDelete == 1 && userIds.Contains(x.BusinessId)).CountAsync();
            if (userBusinessId < userRoleRequest.UserBusinessItems.Count)
            {
                return Result.Fail("用户数据存在不合法", (int)ResponseHttpCode.DataNoExist);
            }
            #endregion
            List<InstitutionRole> institutionRoles=new List< InstitutionRole >();
            foreach (var item in userRoleRequest.UserBusinessItems)
            {
                institutionRoles.Add(new InstitutionRole()
                {
                    BusinessId = userRoleRequest.RoleBusinessId.Value,
                    RoleBusinessId = userRoleRequest.RoleBusinessId,
                    UserBusinessId = item.UserBusinessId

                });
            }
            await _dbContext.Insertable<InstitutionRole>(institutionRoles).
                ExecuteCommandAsync();
            return Result.Success();
        }

        /// <summary>
        /// 搜索角色用户
        /// </summary>
        /// <param name="pageRequest"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PageResult<UserRoleResponse>> SearchRoleUserAsync(AddUserRoleRequest addUserRoleRequest)
        {
            PageResult<UserRoleResponse> pageResult=new PageResult<UserRoleResponse>();
            List<UserRoleResponse> data = new List<UserRoleResponse>();
            #region 获取当前节点下的所有子节点
            var allInstitution = await _dbContext.Queryable<Institution>().Where(x => x.IsDelete == 1)
                 .Select(x => new InstitutionTree()
                 {
                     GPoid = x.GPoid,
                     Name = x.Name,
                     POid = x.Poid,
                     ShortName = x.ShortName,
                     Oid = x.Oid,
                     Sno = x.Sno,
                     BusinessId = x.BusinessId
                 })
                .ToListAsync();
            //当前机构下的所有子节点
           var currentInsititution= allInstitution.Where(x => x.BusinessId == addUserRoleRequest.InstitutionBusinessId).FirstOrDefault();
            if (currentInsititution == null)
            {
                return null;
            }
           var allNodes= new ListToTreeUtil().GetAllNodes(currentInsititution.Oid, allInstitution);
            #endregion
            RefAsync<int> total = 0;
            #region 获取当前节点下已分配的用户
            var authUserList = await _dbContext.Queryable<InstitutionRole>().Where(x => x.IsDelete == 1 && x.InstitutionBusinessId == addUserRoleRequest.InstitutionBusinessId
             && x.BusinessId == addUserRoleRequest.RoleBusinessId).Select(x => x.UserBusinessId).ToListAsync();

            #endregion
            var userList = await _dbContext.Queryable<User>()
                .Where(x => x.IsDelete == 1)
                .WhereIF(!string.IsNullOrWhiteSpace(addUserRoleRequest.KeyWords),x=>x.Name.Contains(addUserRoleRequest.KeyWords)
                || x.Phone.Contains(addUserRoleRequest.KeyWords)
                ).ToListAsync();

            foreach ( var user in userList ) 
            {
                var currentAuthUser= authUserList.Where(x => x == user.BusinessId).FirstOrDefault();
                

                if (addUserRoleRequest.Status == 1 && currentAuthUser != null)
                {
                    data.Add(new UserRoleResponse()
                    {
                        BId = user.BusinessId,
                        Created = user.Created,
                        DepartmentName = allInstitution.Where(x => x.Oid == user.Oid).FirstOrDefault()?.Name,
                        Phone = user.Phone,
                        UserName = user.Name,
                        Status = currentAuthUser.HasValue == true ? 1 : 0,
                        IsOfInstitution = allNodes.Count(x => x == user.Oid) > 0
                    });
                }
                else if (addUserRoleRequest.Status == null)
                {

                    data.Add(new UserRoleResponse()
                    {
                        BId = user.BusinessId,
                        Created = user.Created,
                        DepartmentName = allInstitution.Where(x => x.Oid == user.Oid).FirstOrDefault()?.Name,
                        Phone = user.Phone,
                        UserName = user.Name,
                        Status = currentAuthUser.HasValue == true ? 1 : 0,
                        IsOfInstitution = allNodes.Count(x => x == user.Oid) > 0
                    }) ;
                }
            }
            pageResult.List = data ;
            pageResult.TotalCount=data.Count;
            pageResult.List.Skip((addUserRoleRequest.PageIndex - 1) * addUserRoleRequest.PageSize).Take(addUserRoleRequest.PageSize).ToList();
            return pageResult ;
        }

        /// <summary>
        /// 添加角色菜单
        /// </summary>
        /// <param name="addUserRoleRequest"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Result> AddRoleMenuAsync(AddRoleMenuRequest addRoleMenuRequest)
        {
            var menuIds = addRoleMenuRequest.RoleMenuOptions.Select(x => x.MenuBusinessId).ToList();
            var roleMenuList = await _dbContext.Queryable<RoleMenu>().Where(x => x.IsDelete == 1 && x.RoleBusinessId == addRoleMenuRequest.RoleBusinessId).ToListAsync();
            if (roleMenuList.Count > 0)
            {
                foreach (var item in roleMenuList)
                {
                    item.IsDelete = 0;
                }
                await _dbContext.Updateable<RoleMenu>(roleMenuList).ExecuteCommandAsync();
            }
            //if (roleMenuList.Count == 0)
            //{ 
            //     return Result.Fail("数据不存在",(int)ResponseHttpCode.DataNoExist); 
            //}
          
            List<RoleMenu> roleMenus = new List<RoleMenu>();
            foreach (var item in addRoleMenuRequest.RoleMenuOptions)
            {
                RoleMenu roleMenu = new RoleMenu()
                {
                    Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                    MenuBusinessId = item.MenuBusinessId,
                    BusinessId = GuidUtil.Next(),
                    RoleBusinessId = addRoleMenuRequest.RoleBusinessId
                };
            }
            await _dbContext.Insertable<RoleMenu>(roleMenus).ExecuteCommandAsync();
            return Result.Success("添加成功");
        }

        /// <summary>
        /// 搜索角色菜单
        /// </summary>
        /// <param name="baseRequest"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<UserMenuResponseTree>> SearchRoleMenuAsync(BaseRequest baseRequest)
        {
            var roleMenu = await _dbContext.Queryable<RoleMenu>().Where(x => x.IsDelete == 1 && x.RoleBusinessId == baseRequest.BId).ToListAsync();
            if (roleMenu.Count > 0)
            {
                var menuId = roleMenu.Select(x => x.MenuBusinessId).ToList();
                var menuList = await _dbContext.Queryable<Menu>().Where(x => x.IsDelete == 1 && menuId.Contains(x.BusinessId))
                     .Select(x => new UserMenuResponseTree()
                     {
                         MenuCode = x.MenuCode,
                         BId = x.BusinessId,
                         Mid = x.MId.Value,
                         Sort = x.Sort,
                         Parentid = x.ParentId.Value,
                         ComponentUrl = x.ComponentUrl,
                         Icon = x.Icon,
                         Url = x.Url,
                         Name = x.Name
                     })
                    .ToListAsync();

                return ListToTreeUtil.GetTree(0, menuList);
            }
            return null;
        }
    }
}
