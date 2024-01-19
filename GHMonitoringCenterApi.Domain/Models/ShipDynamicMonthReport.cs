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
    /// 船舶动态月报
    /// </summary>
    [SugarTable("t_shipdynamicmonthreport", IsDisabledDelete = true)]
    [SugarIndex("INDEX_SDMR_UQ_KEY", nameof(ShipId), OrderByType.Asc, nameof(DateMonth), OrderByType.Asc, true)]
    public class ShipDynamicMonthReport : BaseEntity<Guid>
    {
        /// <summary>
        /// 船舶Id（PomId）
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid ShipId { get; set; }

        /// <summary>
        /// 填报月份（例：202304，注解：上月的26-本月的25，例：2023.3.26-2023.04.25）
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int DateMonth { get; set; }

    }
}
