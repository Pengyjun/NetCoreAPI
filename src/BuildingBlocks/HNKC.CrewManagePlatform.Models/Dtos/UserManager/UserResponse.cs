using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.CrewManagePlatform.Models.Dtos.UserManager
{
    /// <summary>
    /// 用户响应DTO
    /// </summary>
    public class UserResponse:BaseResponse
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string? Phone { get; set; }

        /// <summary>
        /// 机构OID
        /// </summary>
        public string? Oid { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string? DepartmentName { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string? RoleName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
    }
}
