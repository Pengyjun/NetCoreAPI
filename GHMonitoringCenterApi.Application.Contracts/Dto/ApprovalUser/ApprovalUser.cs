using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.ApprovalUser
{
    /// <summary>
    /// 审批用户信息
    /// </summary>
    public class ApprovalUser
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid? UserId { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string? UserName { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string? Phone { get; set; }
        /// <summary>
        /// 部门Id
        /// </summary>
        public Guid? DepartmentId { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string? DepartmentName { get; set; }
        /// <summary>
        /// 角色Id
        /// </summary>
        public Guid RoleId { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }

    }
}
