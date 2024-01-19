using GHMonitoringCenterApi.Application.Contracts.Dto;
using GHMonitoringCenterApi.Application.Contracts.Dto.Menu;
using GHMonitoringCenterApi.Application.Contracts.Dto.MenuColumn;
using GHMonitoringCenterApi.Application.Contracts.Dto.MenuDelete;
using GHMonitoringCenterApi.Application.Contracts.IService.Menu;
using GHMonitoringCenterApi.Domain.Shared;
using GHMonitoringCenterApi.Domain.Shared.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GHMonitoringCenterApi.Controllers.Menu
{


    /// <summary>
    /// 菜单相关控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MenuController : BaseController
    {

        #region 依赖注入
        public IMenuService menuService { get; set; }
        public MenuController(IMenuService menuService)
        {
            this.menuService = menuService;
        }
        #endregion

        /// <summary>
        /// 获取用户菜单
        /// </summary>
        /// <param name="menuRequestDto"></param>
        /// <returns></returns>
        [HttpGet("SearchMenu")]
        public async Task<ResponseAjaxResult<List<MenuResponseDto>>> SearchMenuByUserId([FromQuery] MenuRequestDto menuRequestDto)
        {
           return await menuService.SearchMenuByUserId(CurrentUser.Id, menuRequestDto.RoleId.Value);
        }

        /// <summary>
        /// 添加或修改菜单  只有系统管理员才能添加
        /// </summary>
        /// <param name="menuRequestDto"></param>
        /// <returns></returns>
        [HttpPost("SaveMenu")]
        public async Task<ResponseAjaxResult<bool>> SaveMenuAsync([FromBody] AddOrUpdateMenuRequestDto  addOrUpdateMenuRequestDto)
        {
          return await menuService.SaveAsync(addOrUpdateMenuRequestDto,  CurrentUser.RoleInfos[0].Id, CurrentUser.RoleInfos[0].IsAdmin,CurrentUser);
        }

        /// <summary>
        /// 获取所有根菜单  默认查询第一节菜单
        /// </summary>
        /// <param name="rootMenuRequestDto"></param>
        /// <returns></returns>
        [HttpGet("SearchRootMenu")]
        public async Task<ResponseAjaxResult<List<RootMenuResponseDto>>> SearchRootMenuAsync([FromQuery] RootMenuRequestDto rootMenuRequestDto)
        {
           return await menuService.SearchRootMenuAsync(rootMenuRequestDto.ParentId);
        }
        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchMenuColumn")]
        public async Task<ResponseAjaxResult<List<MenuListResponseDto>>> SearchMenuColumnAsync([FromQuery] MenuListRequestDto  menuListRequestDto)
        {
            return await menuService.SearchMenuColumnAsync(menuListRequestDto);
        }
        /// <summary>
        /// 删除菜单列表
        /// </summary>
        /// <param name="deleteMenuRequestDto"></param>
        /// <returns></returns>
        [HttpGet("MenuDelete")]
        public async Task<ResponseAjaxResult<bool>> DeleteMenuAsync([FromQuery] DeleteMenuRequestDto deleteMenuRequestDto)
        {
            return await menuService.DeleteMenuAsync(deleteMenuRequestDto.Id,CurrentUser);
        }
        /// <summary>
        /// 增加首页菜单权限用户列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("AddHomeMenuPermissionUser")]
        public async Task<ResponseAjaxResult<bool>> AddHomeMenuPermissionUserAsync([FromBody] AddHomeUserRequestDto addHomeUserRequestDto)
        {
            return await menuService.AddHomeMenuPermissionUser(addHomeUserRequestDto);
        }
        /// <summary>
        /// 删除首页菜单权限用户列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("DeleteableHomeMenuPermissionUser")]
        public async Task<ResponseAjaxResult<bool>> DeleteableHomeMenuPermissionUserAsync([FromBody] BasePrimaryRequestDto basePrimaryRequestDto)
        {
            return await menuService.DeleteableHomeMenuPermissionUser(basePrimaryRequestDto);
        }
        /// <summary>
        /// 定时删除首页菜单权限用户列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("TimeDeleteableHomeMenuPermissionUser")]
        [AllowAnonymous]

        public async Task<ResponseAjaxResult<bool>> TimeDeleteableHomeMenuPermissionUserAsync()
        {
            return await menuService.TimeDeleteableHomeMenuPermissionUserAsync();
        }
    }
}
