using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.CrewManagePlatform.Models.Dtos.UserManager
{
    public class ChangRoleRequest
    {
        /// <summary>
        /// 机构ID
        /// </summary>
        [Required(ErrorMessage ="机构ID不能为空")]
        public Guid? InstitutionBusinessId { get; set; }
        /// <summary>
        /// 角色ID不
        /// </summary>
        [Required(ErrorMessage = "角色ID不能为空")]
        public Guid? RoleBusinessId { get; set; }
    }
}
