using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.CrewManagePlatform.Models.Dtos.Role
{
    public class UserRoleRequest
    {
        /// <summary>
        /// 用户业务ID
        /// </summary>
        [Required(ErrorMessage = "用户业务ID不能为空")]
        public List<UserBusinessItem> UserBusinessItems { get; set; }
        /// <summary>
        /// 机构业务ID
        /// </summary>
        [Required(ErrorMessage = "机构业务ID不能为空")]
        public Guid? InstitutionBusinessId { get; set; }
        /// <summary>
        /// 角色业务ID
        /// </summary>
        [Required(ErrorMessage = "角色业务ID不能为空")]
        public Guid? RoleBusinessId { get; set; }
    }



    public class UserBusinessItem
    {
        [Required(ErrorMessage = "用户业务ID不能为空")]
        public Guid? UserBusinessId { get; set; }

    }
}
