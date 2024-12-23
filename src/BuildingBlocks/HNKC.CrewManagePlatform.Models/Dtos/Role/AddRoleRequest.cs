using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.CrewManagePlatform.Models.Dtos.Role
{
    public class AddRoleRequest:BaseRequest
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        [Required(ErrorMessage ="角色名称不能为空")]
        public string? Name { get; set; }
        /// <summary>
        /// 角色类型 1是超级管理员 2是船员 3是船长 
        /// </summary>
        [Required(ErrorMessage = "角色类型不能为空")]
        [Range(1,3,ErrorMessage ="角色类型不合法")]
        public int? Type { get; set; }

        /// <summary>
        /// 是否审批   1是审批  0是 无审批
        /// </summary>
        [Required(ErrorMessage = "是否审批不能为空")]
        [Range(0, 1, ErrorMessage = "是否审批不合法")]
        public int? IsApprove { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(256, ErrorMessage = "超度超出限制")]
        public string? Remark { get; set; }

        /// <summary>
        /// 机构OID
        /// </summary>
        [Required(ErrorMessage ="机构OId不能为空")]
        public string? Oid { get; set; }
    }
}
