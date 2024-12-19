
using SqlSugar;

using System.Data;

using AutoMapper;
using System.Net.Sockets;
using Newtonsoft.Json;
using UtilsSharp;
using System.Text;
using HNKC.CrewManagePlatform.Services.Interface.CurrentUser;
using HNKC.CrewManagePlatform.Models.Dtos.Menus;
using HNKC.CrewManagePlatform.SqlSugars.Models;
using HNKC.CrewManagePlatform.Util;
using System.IO;
using HNKC.CrewManagePlatform.Utils;
using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Enums;
using HNKC.CrewManagePlatform.Models.Dtos;

namespace HNKC.CrewManagePlatform.Services.Menus
{


    /// <summary>
    /// 菜单接口 实现层
    /// </summary>
    public class MenuService : CurrentUserService, IMenuService
    {

        #region 依赖注入
        private readonly ISqlSugarClient _dbContext;
        private readonly IMapper _mapper;

        /// <summary>
        /// 依赖注入
        /// </summary>
        public MenuService(ISqlSugarClient dbContext, IMapper mapper)
        {
            this._mapper = mapper;
            this._dbContext = dbContext;
        }

      
        #endregion



        /// <summary>
        /// 用户菜单
        /// </summary>
        /// <param name="roltId"></param>
        /// <returns></returns>
        public async Task<List<UserMenuResponseTree>> SearchUserMenusAsync()
        {
            var userInfo = GlobalCurrentUser;
            var roleMenu = await _dbContext.Queryable<RoleMenu>().Where(x => x.IsDelete == 1 && x.RoleBusinessId == userInfo.RoleBusinessId).ToListAsync();
            if (roleMenu.Count > 0)
            {
                var menuId = roleMenu.Select(x => x.MenuBusinessId.Value).ToList();
                var menuList = await _dbContext.Queryable<Menu>().Where(x => x.IsDelete == 1 && menuId.Contains(x.BusinessId))
                     .Select(x => new UserMenuResponseTree()
                     {
                         BId=x.BusinessId,
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


        /// <summary>
        /// 添加菜单
        /// </summary>
        /// <param name="userMenuRequest"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> AddMenusAsync(UserMenuRequest userMenuRequest)
        {
            var menuList=await _dbContext.Queryable<Menu>().Where(x => x.IsDelete == 1).ToListAsync();
            var maxMid = menuList.Max(x => x.MId);
            var menuBuinessId = GuidUtil.Next();
            Menu menu = new Menu()
            {
                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                BusinessId = menuBuinessId,
                MId = (maxMid + 1),
                ComponentUrl = userMenuRequest.ComponentUrl,
                Icon = userMenuRequest.Icon,
                Name = userMenuRequest.Name,
                ParentId = userMenuRequest.ParentId,
                Sort = userMenuRequest.Sort,
                Url = userMenuRequest.Url,
                Remark = userMenuRequest.Remark,
            };
            RoleMenu roleMenu = new RoleMenu()
            {
                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                BusinessId = Guid.Empty,
                MenuBusinessId = menuBuinessId,
                RoleBusinessId = GlobalCurrentUser.RoleBusinessId,

            };
            await  _dbContext.Insertable<Menu>(menu).ExecuteCommandAsync();
            await  _dbContext.Insertable<RoleMenu>(roleMenu).ExecuteCommandAsync();
            return true;
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="userMenuRequest"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Result> RemoveMenusAsync(BaseRequest  baseRequest)
        {
            var menu =await _dbContext.Queryable<Menu>().Where(x => x.IsDelete == 1 && x.BusinessId == baseRequest.BId).FirstAsync();
            if (menu == null)
            {
              return  Result.Fail("数据不存在", (int)ResponseHttpCode.DataNoExist);
            }
             menu.IsDelete = 0;
            _dbContext.Updateable<Menu>(menu);
            return Result.Success("删除成功");
        }

        public async Task<Result> ModifyMenusAsync(UserMenuRequest userMenuRequest)
        {
            var menuInfo = await _dbContext.Queryable<Menu>().Where(x => x.IsDelete == 1 && x.BusinessId == userMenuRequest.BId).FirstAsync();
            if (menuInfo == null)
            {
                return Result.Fail("数据不存在", (int)ResponseHttpCode.DataNoExist);
            }
            menuInfo.ComponentUrl = userMenuRequest.ComponentUrl;
            menuInfo.Icon = userMenuRequest.Icon;
            menuInfo.Name = userMenuRequest.Name;
            menuInfo.Sort = userMenuRequest.Sort;
            menuInfo.Url = userMenuRequest.Url;
            menuInfo.Remark = userMenuRequest.Remark;
            await _dbContext.Insertable<Menu>(menuInfo).ExecuteCommandAsync();
            return Result.Success("修改成功");

        }
    }
}
