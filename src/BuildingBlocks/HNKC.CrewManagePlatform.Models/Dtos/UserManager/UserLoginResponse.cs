using HNKC.CrewManagePlatform.Models.Dtos.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.CrewManagePlatform.Models.Dtos.UserManager
{
    /// <summary>
    /// 用户登录响应DTO
    /// </summary>
    public class UserLoginResponse:BaseResponse
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 工号
        /// </summary>
        public string? WorkNumber { get; set; }
        /// <summary>
        /// 响应Token
        /// </summary>
        public string? Token { get; set; }
        /// <summary>
        /// 角色信息
        /// </summary>
        public List<RoleResponse> RoleList { get; set; }
    }
}
