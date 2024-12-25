using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.CrewManagePlatform.Models.Dtos.Role
{
    public class RemoveRoleUserRequest
    {
        [Required(ErrorMessage ="机构业务Id不能为空")]
        public Guid? InstitutionBusinessId { get; set; }
        [Required(ErrorMessage = "角色业务Id不能为空")]
        public Guid? RoleBusinessId { get; set; }

        public List<RoleUserOptions> RoleUserOptions { get; set; }
    }

    public class RoleUserOptions 
    {
        /// <summary>
        /// 用户业务ID
        /// </summary>
        [Required(ErrorMessage ="用户业务ID不能为空")]
        public Guid? UserBusinsessId { get; set; }
    }
}
