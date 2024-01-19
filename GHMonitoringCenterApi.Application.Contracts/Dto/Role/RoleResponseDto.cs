using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Role
{


    /// <summary>
    /// 角色响应DTO
    /// </summary>
    public class RoleResponseDto
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public Guid? Id { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 角色类型
        /// </summary>
        public int? Type { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>

        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 是否存在审批的功能  系统管理员不会有审批功能
        /// </summary>
        public bool IsApprove { get; set; }

        /// <summary>
        /// 备注说明
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 操作类型  0是全部操作  1只有查询操作   
        /// </summary>
        public int OperationType { get; set; }
    }
}
