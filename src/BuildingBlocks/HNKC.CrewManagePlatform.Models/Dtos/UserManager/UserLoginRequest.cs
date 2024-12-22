using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.CrewManagePlatform.Models.Dtos.UserManager
{
    public class UserLoginRequest
    {
        /// <summary>
        /// 手机号
        /// </summary>
        [Required(ErrorMessage ="账号不能为空")]
        public string? Phone { get; set; }

        /// <summary>
        /// 职工号
        /// </summary>
        [Required(ErrorMessage = "密码不能为空")]
        public string? Password { get; set; }
        /// <summary>
        /// 角色业务ID
        /// </summary>
        public Guid? RoleBusinessId { get; set; }
    }
}
