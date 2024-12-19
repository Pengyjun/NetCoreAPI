using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.CrewManagePlatform.SqlSugars.Models
{
    /// <summary>
    /// 用户机构关联表
    /// </summary>
    [SugarTable("t_userinstitution", IsDisabledDelete = true)]
    public class UserInstitution:BaseEntity<long>
    {
        /// <summary>
        /// 用户业务ID
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? UserBusinessId { get; set; }
        /// <summary>
        /// 机构业务ID
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? InstitutionBusinessId { get; set; }
       
    }
}
