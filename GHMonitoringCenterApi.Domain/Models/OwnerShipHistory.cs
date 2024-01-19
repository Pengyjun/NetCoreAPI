using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{

    /// <summary>
    /// 自有船舶历史数据
    /// </summary>
    [SugarTable("t_ownershiphistory", IsDisabledDelete = true)]

    public class OwnerShipHistory:BaseEntity<Guid>
    {
        [SugarColumn(Length =30)]
        public string? Name { get; set; }
        [SugarColumn(ColumnDataType = "int")]
        public int? Year { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal? YearShipHistory { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal? OnDay { get; set; }
        [SugarColumn(ColumnDataType = "int")]
        public int? Sort { get; set; }

        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public int? WorkingHours { get; set; }
    }
}
