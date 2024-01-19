using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 项目月报历史数据
    /// </summary>
    [SugarTable("t_projectmonthreporthistory", IsDisabledDelete = true)]
    public class ProjectMonthReportHistory : BaseEntity<Guid>
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        [SugarColumn(Length = 255)]
        public string? ProjectName { get; set; }
        /// <summary>
        /// 项目Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? ProjectId { get; set; }
        /// <summary>
        /// 本年业主确认(万元)
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)", DefaultValue = "0")]
        public decimal? YearOwnerConfirmation { get; set; }
        /// <summary>
        /// 开累业主确认(万元)
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)", DefaultValue = "0")]
        public decimal? KaileiOwnerConfirmation { get; set; }
        /// <summary>
        /// 本年工程收款(万元)
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)", DefaultValue = "0")]
        public decimal? YearProjectPayment { get; set; }
        /// <summary>
        /// 开累工程收款(万元)
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)", DefaultValue = "0")]
        public decimal? KaileiProjectPayment { get; set; }


    }
}
