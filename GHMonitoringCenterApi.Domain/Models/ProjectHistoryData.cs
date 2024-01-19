using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 项目历史数据
    /// </summary>
    [SugarTable("t_projecthistorydata", IsDisabledDelete = true)]
    public class ProjectHistoryData: BaseEntity<Guid>
    {
        /// <summary>
        /// 项目Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? ProjectId { get; set; }
        /// <summary>
        /// 产值
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(30,20)")]
        public decimal? OutputValue { get; set; }
        /// <summary>
        /// 产量
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(30,20)")]
        public decimal? Production { get; set; }
        /// <summary>
        /// 外包支出
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(30,20)")]
        public decimal? OutsourcingExpenses { get; set; }
        /// <summary>
        /// 开累产值
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(30,20)")]
        public decimal? AccumulatedOutputValue { get; set; }
        /// <summary>
        /// 开累产量
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(30,20)")]
        public decimal? AccumulatedProduction { get; set; }
        /// <summary>
        /// 开累外包支出
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(30,20)")]
        public decimal? AccumulatedOutsourcingExpenses { get; set; }
    }
}
