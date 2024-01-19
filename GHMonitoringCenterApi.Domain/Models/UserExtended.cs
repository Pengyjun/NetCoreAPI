using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 用户扩展表
    /// </summary>
    [SugarTable("t_userextended", IsDisabledDelete = true)]
    public class UserExtended : BaseEntity<Guid>
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? UserId { get; set; }
        /// <summary>
        /// 部门Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? DepartmentId { get; set; }
        /// <summary>
        /// 公司Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? CompanyId { get; set; }
    }
}
