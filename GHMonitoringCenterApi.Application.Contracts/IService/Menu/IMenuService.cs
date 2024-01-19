using GHMonitoringCenterApi.Application.Contracts.Dto;
using GHMonitoringCenterApi.Application.Contracts.Dto.Menu;
using GHMonitoringCenterApi.Application.Contracts.Dto.MenuColumn;
using GHMonitoringCenterApi.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.IService.Menu
{

    /// <summary>
    /// 菜单接口层
    /// </summary>
    public interface IMenuService
    {
        /// <summary>
        /// 根据用户ID 查询当前用户菜单
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<MenuResponseDto>>> SearchMenuByUserId(Guid userId, Guid roleId);


        /// <summary>
        /// 添加或修改菜单
        /// </summary>
        /// <param name="addOrUpdateMenuRequestDto"></param>
        /// <param name="IsAdmin">只要系统管理员才有权限增加菜单</param>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> SaveAsync(AddOrUpdateMenuRequestDto addOrUpdateMenuRequestDto,Guid roleId, bool IsAdmin,CurrentUser currentUser);

        /// <summary>
        /// 获取所有根菜单  默认查询第一节菜单
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<RootMenuResponseDto>>> SearchRootMenuAsync(int parentId = 0);

        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <param name="menuListRequestDto"></param>
        /// <param name="isPage">是否分页显示</param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<MenuListResponseDto>>> SearchMenuColumnAsync(MenuListRequestDto  menuListRequestDto);
        /// <summary>
        /// 删除菜单列表
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> DeleteMenuAsync(Guid Id,CurrentUser currentUser);
        /// <summary>
        /// 增加首页菜单权限用户列表
        /// </summary>
        /// <param name="basePrimaryRequestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> AddHomeMenuPermissionUser(AddHomeUserRequestDto addHomeUserRequestDto);
        /// <summary>
        /// 删除首页菜单权限用户列表
        /// </summary>
        /// <param name="basePrimaryRequestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> DeleteableHomeMenuPermissionUser(BasePrimaryRequestDto basePrimaryRequestDto);
        /// <summary>
        /// 定时删除首页菜单权限用户列表
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> TimeDeleteableHomeMenuPermissionUserAsync();
    }
}
