using HNKC.CrewManagePlatform.Models.CommonRequest;
using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Dtos;
using HNKC.CrewManagePlatform.Models.Dtos.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.CrewManagePlatform.Services.Role
{
    /// <summary>
    /// 角色接口层
    /// </summary>
    public interface IRoleService
    {
        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="addRoleRequest"></param>
        /// <returns></returns>
        Task<bool> AddAsync(AddRoleRequest addRoleRequest);

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="addRoleRequest"></param>
        /// <returns></returns>
        Task<Result> ModifyAsync(AddRoleRequest addRoleRequest);


        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="addRoleRequest"></param>
        /// <returns></returns>
        Task<Result> RemoveAsync(BaseRequest  baseRequest);

        /// <summary>
        /// 查询当前机构角色
        /// </summary>
        /// <param name="addRoleRequest"></param>
        /// <returns></returns>
        Task<PageResult<RoleResponse>> SearchCurrentInstitutionRoleAsync(string oid);



        /// <summary>
        /// 为用户添加角色
        /// </summary>
        /// <param name="addRoleRequest"></param>
        /// <returns></returns>
        Task<Result> AddUserRoleAsync(UserRoleRequest  userRoleRequest);

        /// <summary>
        /// 角色用户信息列表
        /// </summary>
        /// <param name="userRoleRequest"></param>
        /// <returns></returns>
        Task<PageResult<UserRoleResponse>> SearchRoleUserAsync(AddUserRoleRequest  addUserRoleRequest);


        /// <summary>
        /// 角色分配菜单
        /// </summary>
        /// <param name="userRoleRequest"></param>
        /// <returns></returns>
        Task<Result> AddRoleMenuAsync(AddRoleMenuRequest  addRoleMenuRequest);


      
    }
}
