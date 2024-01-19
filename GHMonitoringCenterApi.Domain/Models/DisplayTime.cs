using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 显示日期
    /// </summary>
    [SugarTable("t_displaytime", IsDisabledDelete = true)]
    public class DisplayTime : BaseEntity<Guid>
    {
        /// <summary>
        /// 首页菜单Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid MenuId { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int? StartTime { get; set; }
        /// <summary>
        /// 截止时间
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int? EndTime { get; set; }
    }
}
