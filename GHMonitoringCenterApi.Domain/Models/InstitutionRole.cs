using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{

    /// <summary>
    /// 机构角色关联表
    /// </summary>
    [SugarTable("t_institutionrole", IsDisabledDelete = true)]
    public class InstitutionRole:BaseEntity<Guid>
    {
        /// <summary>
        /// 机构Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? InstitutionId { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? RoleId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? UserId { get; set; }
    }
}
