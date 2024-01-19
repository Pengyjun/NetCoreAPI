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
    /// 船舶月报明细
    /// </summary>
    [SugarTable("t_shipdynamicmonthreportdetail", IsDisabledDelete = true)]
    public class ShipDynamicMonthReportDetail : BaseEntity<Guid>
    {
        /// <summary>
        /// 船舶月报Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid ShipDynamicMonthReportId { get; set; }

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

        /// <summary>
        /// 船舶状态
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public ProjectShipState ShipState { get; set; }

        /// <summary>
        /// 所在港口
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? PortId { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime StartTime{ get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime EndTime { get; set; }
    }
}
