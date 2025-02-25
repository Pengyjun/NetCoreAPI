using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 日报偏差表
    /// </summary>
    [SugarTable("t_dailydeviation", IsDisabledDelete = true)]
    public class DailyDeviation : BaseEntity<Guid>
    {
        /// <summary>
        /// 项目id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid ProjectId { get; set; }
        [SugarColumn(Length = 36)]
        public Guid CompanyId { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        [SugarColumn(Length = 300)]
        public string? ProjectName { get; set; }
        /// <summary>
        /// 状态id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid StatusId { get; set; }
        /// <summary>
        /// 当日实际产值（元）
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal DayActualProductionAmount { get; set; }
        /// <summary>
        /// 日报偏差值
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal DayActualProductionAmountDeviation { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int Dateday { get; set; }

    }
}
