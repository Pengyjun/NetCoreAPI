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
    /// 历史船舶年统计数据
    /// </summary>
    [SugarTable("t_historyownershipreport", IsDisabledDelete = true)]
    public class HistoryOwnerShipReport : BaseEntity<Guid>
    {
        /// <summary>
        /// 项目id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid ProjectId { get; set; }

        /// <summary>
        /// 自有船舶Id(PomId)
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid ShipId { get; set; }

        /// <summary>
        /// 历史累计产量（方）
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal HistoryProduction { get; set; }

        /// <summary>
        /// 历史累计完成产值（元）
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal HistoryProductionAmount { get; set; }

        /// <summary>
        /// 历史累计年份
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int HistoryDateYear { get; set; }
    }
}
