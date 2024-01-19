using GHMonitoringCenterApi.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.User
{

    /// <summary>
    /// 用户信息响应DTO
    /// </summary>
    public class UserResponseDto
    {

        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid? Id { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 当前登录的角色ID
        /// </summary>
        public Guid CurrentLoginRoleId { get; set; }
        /// <summary>
        /// 当前登录机构ID
        /// </summary>
        public Guid CurrentLoginInstitutionId { get; set; }
        /// <summary>
        /// 当前登录机构名称
        /// </summary>
        public string CurrentLoginInstitutionName { get; set; }
        /// <summary>
        /// 当前登录机构Oid
        /// </summary>
        public string CurrentLoginInstitutionOid { get; set; }
        /// <summary>
        /// 当前登录机构父POID
        /// </summary>
        public string CurrentLoginInstitutionPoid { get; set; }
        /// <summary>
        /// 当前登录用户的部门ID
        /// </summary>
        public Guid? CurrentLoginDepartmentId { get; set; }
        /// <summary>
        /// 获取当前登录的机构分组信息
        /// </summary>
        public string CurrentLoginInstitutionGrule { get; set; }
        /// <summary>
        /// 当前用户登录用户角色类型
        /// </summary>
        public int? CurrentLoginUserType { get; set; }
        /// <summary>
        /// 角色信息
        /// </summary>
        public List<RoleInfo> RoleInfos { get; set; }

       
    }

}
