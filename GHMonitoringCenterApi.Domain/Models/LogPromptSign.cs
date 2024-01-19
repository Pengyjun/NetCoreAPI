using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 是否已读标识
    /// </summary>
    [SugarTable("t_logpromptsign", IsDisabledDelete = true)]
    public class LogPromptSign : BaseEntity<Guid>
    {
        /// <summary>
        /// 用户标识
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? UserId { get; set; }      
    }
}
