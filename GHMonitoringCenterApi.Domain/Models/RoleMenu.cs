using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{


    /// <summary>
    /// 角色菜单表
    /// </summary>
    [SugarTable("t_rolemenu", IsDisabledDelete = true)]
    public class RoleMenu:BaseEntity<Guid>
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? RoleId { get; set; }
        /// <summary>
        /// 菜单ID
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? MenuId { get; set; }
    }
}
