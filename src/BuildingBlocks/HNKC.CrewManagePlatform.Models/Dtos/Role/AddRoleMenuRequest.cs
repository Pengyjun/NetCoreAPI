using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.CrewManagePlatform.Models.Dtos.Role
{
    public class AddRoleMenuRequest
    {
        /// <summary>
        /// 角色业务ID
        /// </summary>
        [Required(ErrorMessage = "角色业务ID不能为空")]
        public Guid? RoleBusinessId { get; set; }
        /// <summary>
        /// 菜单项
        /// </summary>
        public List<RoleMenuOptions> RoleMenuOptions { get; set; }
    }


    public class RoleMenuOptions
    {
        /// <summary>
        /// 菜单业务ID
        /// </summary>
        [Required(ErrorMessage = "菜单业务ID不能为空")]
        public Guid? MenuBusinessId { get; set; }
    }
}
