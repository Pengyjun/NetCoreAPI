using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 交叉月份表
    /// </summary>
    [SugarTable("t_crossmonth", IsDisabledDelete = true)]
    public class CrossMonth
    {
        /// <summary>
        /// 月份
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int Month { get; set; }
    }
}
