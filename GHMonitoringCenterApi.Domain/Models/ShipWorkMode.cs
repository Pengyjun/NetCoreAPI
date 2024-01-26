using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 工艺方式
    /// </summary>
    [SugarTable("t_shipworkmode", IsDisabledDelete = true)]
    public class ShipWorkMode : BaseEntity<Guid>
    {
        /// <summary>
        /// PomId
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid PomId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [SugarColumn(Length = 255)]
        public string Name { get; set; }
    }
}
