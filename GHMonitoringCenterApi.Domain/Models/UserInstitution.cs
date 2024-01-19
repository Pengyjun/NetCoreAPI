using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 用户机构关联表
    /// </summary>
    [SugarTable("t_userinstitution", IsDisabledDelete = true)]
    public class UserInstitution:BaseEntity<Guid>
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? UserId { get; set; }
        /// <summary>
        /// 机构Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? InstitutionId { get; set; }
       
    }
}
