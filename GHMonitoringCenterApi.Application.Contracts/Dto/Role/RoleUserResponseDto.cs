using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Role
{


    /// <summary>
    /// 角色下面用户信息响应DTO
    /// </summary>
    public class RoleUserResponseDto
    {
        /// <summary>
        /// 用户主键ID
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 登录帐号
        /// </summary>
        public string? LoginAccount { get; set; }
        /// <summary>
        /// 用户pomId
        /// </summary>
        public Guid PomId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 用户手机号
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 用户邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }
    }
}
