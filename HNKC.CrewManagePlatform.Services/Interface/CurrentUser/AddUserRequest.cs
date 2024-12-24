using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.CrewManagePlatform.Services.Interface.CurrentUser
{
    public class AddUserRequest
    {
        [Required(ErrorMessage ="姓名不能为空")]
        public string Name { get; set; }
        [Required(ErrorMessage = "手机号不能为空")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "密码不能为空")]
        public string Pwd { get; set; }
        [Required(ErrorMessage = "机构不能为空")]
        public string Oid { get; set; }

        /// <summary>
        /// 性别ID
        /// </summary>
        [Required(ErrorMessage = "性别不能为空")]
        [Range(1,2,ErrorMessage ="性别不能为空")]
        public int? Gender { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(256)]
        public string? Remark { get; set; }
        

    }
}
