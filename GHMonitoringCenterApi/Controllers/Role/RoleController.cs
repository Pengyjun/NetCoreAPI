using GHMonitoringCenterApi.Application.Contracts.Dto;
using GHMonitoringCenterApi.Application.Contracts.Dto.Role;
using GHMonitoringCenterApi.Application.Contracts.IService.Role;
using GHMonitoringCenterApi.CustomAttribute;
using GHMonitoringCenterApi.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GHMonitoringCenterApi.Controllers.Role
{


    /// <summary>
    /// 系统角色相关控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RoleController : BaseController
    {
        #region 依赖注入
        public IRoleService roleService { get; set; }
        public RoleController(IRoleService roleService)
        {
            this.roleService = roleService;
        }
        #endregion


        /// <summary>
        /// 获取当前公司角色列表
        /// </summary>
        /// <param name="basePrimaryRequestDto"></param>
        /// <returns></returns>
        [HttpGet("SearchRole")]
        public async Task<ResponseAjaxResult<List<RoleResponseDto>>> SearchRoleAsync([FromQuery]BasePrimaryRequestDto basePrimaryRequestDto)
        {
           return await roleService.SearchRoleAsync(basePrimaryRequestDto.Id,CurrentUser.CurrentLoginInstitutionGrule);
        }


        /// <summary>
        /// 角色分配菜单
        /// </summary>
        /// <param name="basePrimaryRequestDto"></param>
        /// <returns></returns>
        [HttpPost("RoleAddMenu")]
        [UnitOfWork]
        public async Task<ResponseAjaxResult<bool>> AddMenuAsync([FromBody] RoleAddMenuRequestDto  roleAddMenuRequestDto)
        {
            return await roleService.AddMenuAsync(roleAddMenuRequestDto.RoleId, roleAddMenuRequestDto.MenusIds);
        }

        /// <summary>
        /// 获取当前角色下面的用户
        /// </summary>
        /// <param name="roleUserRequestDto"></param>
        /// <returns></returns>
        [HttpGet("SearchUserCurrentRole")]
        public async Task<ResponseAjaxResult<List<RoleUserResponseDto>>> SearchUserCurrentRoleAsync([FromQuery] RoleUserRequestDto roleUserRequestDto)
        {
            return await roleService.SearchUserCurrentRoleAsync(roleUserRequestDto.KeyWords,roleUserRequestDto.PageIndex, roleUserRequestDto.PageSize, roleUserRequestDto.RoleId);
        }


        /// <summary>
        /// 删除当前角色下面的用户
        /// </summary>
        /// <param name="roleUserRequestDto"></param>
        /// <returns></returns>
        [HttpPost("RemoveRoleUser")]
        public async Task<ResponseAjaxResult<bool>> RemoveRoleUserAsync([FromBody] RemoveRoleUserRequestDto  removeRoleUserRequestDto)
        {
            return await roleService.RemoveRoleUserAsync(removeRoleUserRequestDto.RoleId, removeRoleUserRequestDto.InstitutionId,removeRoleUserRequestDto.UserId);
        }


        /// <summary>
        /// 删除当前角色
        /// </summary>
        /// <param name="roleUserRequestDto"></param>
        /// <returns></returns>
        [HttpPost("Remove")]
        [UnitOfWork]
        public async Task<ResponseAjaxResult<bool>> RemoveAsync([FromBody] BasePrimaryRequestDto basePrimaryRequestDto)
        {
            return await roleService.RemoveAsync(basePrimaryRequestDto.Id);
        }



        /// <summary>
        /// 新增角色或修改角色
        /// </summary>
        /// <param name="roleUserRequestDto"></param>
        /// <returns></returns>
        [HttpPost("Save")]
        [UnitOfWork]
        public async Task<ResponseAjaxResult<bool>> SaveAsync([FromBody] AddOrUpdateRoleRequestDto  addOrUpdateRoleRequestDto)
        {
            if (addOrUpdateRoleRequestDto.RequestType)
            return await roleService.AddAsync(addOrUpdateRoleRequestDto,CurrentUser.Id);
            else
                return await roleService.UpdateAsync(addOrUpdateRoleRequestDto,CurrentUser);

        }
        /// <summary>
        /// 添加角色用户
        /// </summary>
        /// <returns></returns>
        [HttpPost("AddRoleUser")]
        [UnitOfWork]
        public async Task<ResponseAjaxResult<bool>> AddRoleUserAsync([FromBody] AddRoleUserRequestDto  addRoleUserRequestDto)
        {
            return await roleService.AddRoleUserAsync(addRoleUserRequestDto);
        }
    }
}
