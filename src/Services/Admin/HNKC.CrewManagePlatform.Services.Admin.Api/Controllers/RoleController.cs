using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Dtos;
using HNKC.CrewManagePlatform.Models.Dtos.CrewArchives;
using HNKC.CrewManagePlatform.Models.Dtos.Role;
using HNKC.CrewManagePlatform.Services.Interface.CrewArchives;
using HNKC.CrewManagePlatform.Services.Role;
using HNKC.CrewManagePlatform.SqlSugars.UnitOfTransaction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HNKC.CrewManagePlatform.Services.Admin.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : BaseController
    {

        private IRoleService  roleService;
        /// <summary>
        /// 服务注入
        /// </summary>
        /// <param name="service"></param>
        public RoleController(IRoleService roleService)
        {
            this.roleService = roleService;
        }
        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="addRoleRequest"></param>
        /// <returns></returns>
        [HttpPost("addRole")]
        [Transactional]
        public async Task<IActionResult> addRoleAsync([FromBody] AddRoleRequest  addRoleRequest)
        {
            var data = await roleService.AddAsync(addRoleRequest);
            return Ok(data);    
        }


        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="addRoleRequest"></param>
        /// <returns></returns>
        [HttpPost("ModifyRole")]
        [Transactional]
        public async Task<IActionResult> ModifyAsync([FromBody] AddRoleRequest addRoleRequest)
        {
            var data = await roleService.ModifyAsync(addRoleRequest);
            return Ok(data);
        }
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="addRoleRequest"></param>
        /// <returns></returns>
        [HttpPost("RemoveRole")]
        [Transactional]
        public async Task<IActionResult> RemoveAsync([FromBody] BaseRequest  baseRequest)
        {
            var data = await roleService.RemoveAsync(baseRequest);
            return Ok(data);
        }
        /// <summary>
        /// 查询当前机构下面的角色
        /// </summary>
        /// <param name="addRoleRequest"></param>
        /// <returns></returns>
        [HttpGet("SearchCurrentInstitutionRole")]
        public async Task<IActionResult> SearchCurrentInstitutionRoleAsync([FromQuery] string   oid)
        {
            var data = await roleService.SearchCurrentInstitutionRoleAsync(oid);
            return Ok(data);
        }

        /// <summary>
        /// 为用户分配角色
        /// </summary>
        /// <param name="addRoleRequest"></param>
        /// <returns></returns>
        [HttpPost("AddUserRole")]
        public async Task<Result> AddUserRoleAsync([FromBody] UserRoleRequest  userRoleRequest)
        {
            return await roleService.AddUserRoleAsync(userRoleRequest);
        }

        /// <summary>
        /// 查看已分配 未分配用户信息
        /// </summary>
        /// <param name="addUserRoleRequest"></param>
        /// <returns></returns>
        [HttpGet("SearchRoleUser")]
        public async Task<IActionResult> SearchRoleUserAsync([FromQuery]AddUserRoleRequest addUserRoleRequest)
        {
            var data = await roleService.SearchRoleUserAsync(addUserRoleRequest);
            return Ok(data);
        }


        /// <summary>
        /// 角色分配菜单
        /// </summary>
        /// <param name="addRoleMenuRequest"></param>
        /// <returns></returns>
        [HttpPost("RoleMenu")]
        public async Task<Result> AddRoleMenuAsync([FromBody] AddRoleMenuRequest  addRoleMenuRequest)
        {
            return await roleService.AddRoleMenuAsync(addRoleMenuRequest);
        }


        /// <summary>
        /// 角色查询菜单
        /// </summary>
        /// <param name="addRoleMenuRequest"></param>
        /// <returns></returns>
        [HttpGet("SearchRoleMenu")]
        public async Task<IActionResult> SearchRoleMenuAsync([FromQuery] BaseRequest baseRequest)
        {
            var data= await roleService.SearchRoleMenuAsync(baseRequest);
            return Ok(data);
        }

        /// <summary>
        /// 删除角色用户
        /// </summary>
        /// <param name="addRoleMenuRequest"></param>
        /// <returns></returns>
        [HttpPost("RemoveRoleUser")]
        public async Task<Result> RemoveRoleUserAsync([FromBody] RemoveRoleUserRequest removeRoleUserRequest)
        {
            return await roleService.RemoveRoleUserAsync(removeRoleUserRequest);
        }
    }
}
