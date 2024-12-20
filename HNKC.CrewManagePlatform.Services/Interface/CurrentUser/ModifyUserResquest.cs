using HNKC.CrewManagePlatform.Models.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.CrewManagePlatform.Services.Interface.CurrentUser
{
    public class ModifyUserResquest:BaseRequest
    {
        [Required(ErrorMessage = "姓名不能为空")]
        public string Name { get; set; }
        [Required(ErrorMessage = "手机号不能为空")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "密码不能为空")]
        public string Pwd { get; set; }
        [Required(ErrorMessage = "机构不能为空")]
        public string Oid { get; set; }
    }
}
