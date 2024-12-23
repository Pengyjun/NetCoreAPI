using HNKC.CrewManagePlatform.Models.CommonRequest;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.CrewManagePlatform.Models.Dtos.Role
{
    public class AddUserRoleRequest:PageRequest
    {
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


        /// <summary>
        /// 用户授权状态  1是已授权 0是未授权
        /// </summary>
        public int? Status { get; set; }
    }
}
