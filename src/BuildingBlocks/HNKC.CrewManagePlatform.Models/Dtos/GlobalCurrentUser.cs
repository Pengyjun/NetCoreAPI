using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.CrewManagePlatform.Models.Dtos
{
    /// <summary>
    /// 全局用户信息
    /// </summary>
    public class GlobalCurrentUser
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 是否是超级管理员 true是   其他不是
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// 用户业务ID
        /// </summary>
        public Guid? UserBusinessId { get; set; }
        /// <summary>
        /// 工号
        /// </summary>
        public string? WorkNumber { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string? Phone { get; set; }

        /// <summary>
        /// 所属公司OID
        /// </summary>
        public string? Oid { get; set; }
        /// <summary>
        /// 机构业务ID
        /// </summary>
        public Guid? InstitutionBusiessId { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>
        public long? RoleId { get; set; }
        /// <summary>
        /// 业务角色ID
        /// </summary>
        public Guid? RoleBusinessId { get; set; }

        /// <summary>
        /// 角色类型  1是超级管理员 2是船员  3是船长  4是船员部
        /// </summary>
        public int? Type { get; set; }
    }
}
