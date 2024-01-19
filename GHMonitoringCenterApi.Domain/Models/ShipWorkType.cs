using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 疏浚吹填分类
    /// </summary>
    [SugarTable("t_shipWorktype", IsDisabledDelete = true)]
    public class ShipWorkType : BaseEntity<Guid>
    {
        /// <summary>
        /// PomId
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? PomId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [SugarColumn(Length = 255)]
        public string? Name { get; set; }
    }
}
