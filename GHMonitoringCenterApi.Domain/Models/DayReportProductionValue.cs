using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 日报调差数表
    /// </summary>
    [SugarTable("t_dayreportproductionvalue", IsDisabledDelete = true)]
    public class DayReportProductionValue:BaseEntity<Guid>
    {
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int DateDay { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)", DefaultValue = "0")]
        public decimal? ProductionValue { get; set; }
    }
}
