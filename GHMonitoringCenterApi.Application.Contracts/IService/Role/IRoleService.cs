using GHMonitoringCenterApi.Application.Contracts.Dto.Role;
using GHMonitoringCenterApi.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.IService.Role
{
    /// <summary>
    /// 角色接口层
    /// </summary>
    public interface IRoleService
    {

        /// <summary>
        /// 新增角色
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> AddAsync(AddOrUpdateRoleRequestDto addOrUpdateRoleRequestDto, Guid userId);

        /// <summary>
        /// 获取当前公司的角色列表
        /// </summary>
        /// <param name="id">机构ID</param>
        /// <param name="grule">当前登录者的机构层级关系</param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<RoleResponseDto>>> SearchRoleAsync(Guid id,string grule);
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> RemoveAsync(Guid  id);

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> UpdateAsync(AddOrUpdateRoleRequestDto  addOrUpdateRoleRequestDto,CurrentUser currentUser);

        /// <summary>
        /// 角色分配菜单
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="menuIds"></param>
        /// <param name="createId"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> AddMenuAsync(Guid roleId,List<Guid> menuIds);

        /// <summary>
        /// 获取当前角色下面的用户
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name=""></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<RoleUserResponseDto>>> SearchUserCurrentRoleAsync(string KeyWords, int pageIndex,int pageSize,Guid roleId);

        /// <summary>
        /// 移除当前角色下面的用户
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="roleId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> RemoveRoleUserAsync(Guid roleId, Guid institutionId, Guid userId);
        /// <summary>
        /// 添加角色用户
        /// </summary>       
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> AddRoleUserAsync(AddRoleUserRequestDto addRoleUserRequsetDto);
    }
}
