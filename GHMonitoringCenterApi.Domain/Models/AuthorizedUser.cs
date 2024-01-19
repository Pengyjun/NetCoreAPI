using GHMonitoringCenterApi.Domain.Enums;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{

    /// <summary>
    /// 授权的用户
    /// </summary>
    [SugarTable("t_authorizeduser", IsDisabledDelete = true)]
    public class AuthorizedUser : BaseEntity<Guid>
    {
        /// <summary>
        /// 授权的用户Id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 业务模块
        /// </summary>
        public BizModule BizModule { get; set; }

        /// <summary>
        ///  授权模式
        /// </summary>
        public AuthorizeMode AuthorizeMode { get; set; }

        /// <summary>
        /// 授权过期时间 
        /// </summary>
        public DateTime? ExpireTime { get; set; }
    }
}
