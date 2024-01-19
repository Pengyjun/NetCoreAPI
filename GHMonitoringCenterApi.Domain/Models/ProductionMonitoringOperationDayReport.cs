using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{

    /// <summary>
    /// 生产监控运营日报表基础数据
    /// </summary>
    [SugarTable("t_productionmonitoringoperationdayreport", IsDisabledDelete = true)]
    public class ProductionMonitoringOperationDayReport:BaseEntity<Guid>
    {

        /// <summary>
        /// 类型 1是 项目相关  2是船舶相关
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int? Type { get; set; }
        /// <summary>
        /// 操作项的ID
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid?  ItemId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [SugarColumn(Length = 100)]
        public string?  Name { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int? DateDay { get; set; }
        /// <summary>
        /// 产值  亿元
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,6)", DefaultValue = "0")]
        public decimal YearProductionValue { get; set; }

        /// <summary>
        /// 运转小时
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)", DefaultValue = "0")]
        public decimal TurnHours { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int? Sort { get; set; }

        /// <summary>
        /// 是否是汇总项  1 是 0 不是  
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int? Collect { get; set; }
        
        /// <summary>
        /// 在场天数量基础
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)", DefaultValue = "0")]
        public decimal OnDayCount { get; set; }
    }
}
