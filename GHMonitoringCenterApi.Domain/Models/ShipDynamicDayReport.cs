using GHMonitoringCenterApi.Domain.Enums;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{

    /// <summary>
    /// 船舶动态日报（已作废）
    /// </summary>
    [SugarTable("t_shipdynamicdayreport", IsDisabledDelete = true)]
    [SugarIndex("INDEX_SDDR_UQ_KEY", nameof(ShipId), OrderByType.Asc, nameof(DateDay), OrderByType.Asc, true)]
    [Obsolete("业务已迁移到船舶日报，此表作废")]
    public class ShipDynamicDayReport : BaseEntity<Guid>
    {

        /// <summary>
        /// 填报日期（例：20230401）
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int DateDay { get; set; }

        /// <summary>
        /// 船舶Id（PomId）
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid ShipId { get; set; }

        /// <summary>
        /// 船舶状态
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public DynamicShipState ShipState { get; set; }

        /// <summary>
        /// 本月实际目的港Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? MonthDestinationPortId { get; set; }

        /// <summary>
        /// 本月实际出发港Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? MonthDeparturePortId { get; set; }

        /// <summary>
        /// 本月实际航行距离
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal? MonthNavigationalDistance { get; set; }

        /// <summary>
        /// 本月实际调遣小时
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal? MonthDispatchsHours { get; set; }

        /// <summary>
        /// 本月实际拖轮/半潜船名称
        /// </summary>
        [SugarColumn(Length = 255)]
        public string? MonthShipName { get; set; }

        /// <summary>
        /// 修理所在港口Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? RepairByPortId { get; set; }

        /// <summary>
        /// 待命所在港口Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? StandbyPortId { get; set; }

    }
}
