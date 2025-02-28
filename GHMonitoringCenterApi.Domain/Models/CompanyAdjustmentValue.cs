using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 公司产值信息调整值
    /// </summary>
    [SugarTable("t_companyadjustmentvalue", IsDisabledDelete = true)]
    public class CompanyAdjustmentValue : BaseEntity<Guid>
    {
        /// <summary>
        /// 计划Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid PlanId { get; set; }

        /// <summary>
        /// 月份
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int? Month { get; set; }

        /// <summary>
        /// 调整值
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? AdjustmentValue { get; set; }
    }
}
