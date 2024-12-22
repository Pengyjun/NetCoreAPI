using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.CrewManagePlatform.Models.Dtos.Role
{


    /// <summary>
    /// 角色响应DTO
    /// </summary>
    public class RoleResponse
    {
        /// <summary>
        /// 所属机构ID
        /// </summary>
        public string? Oid { get; set; }

        /// <summary>
        /// 机构业务ID
        /// </summary>
        public Guid? InstitutionBusinessId { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 角色业务ID
        /// </summary>
        public Guid? RoleBusinessId { get; set; }

        /// <summary>
        /// 是否审批  1可以审批  0不可以
        /// </summary>
        public bool? IsApprove { get; set; }

        /// <summary>
        /// 是否是超级管理员  1是  0不是
        /// </summary>
        public bool? IsAdmin { get; set; }
    }
}
