using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 月份补差值表  当新的一月开始的时候  需要几天的时间才能写完前一个月的月报
    /// 这个时候  程序会自动把上个月的最新值保存起来 共新的一月使用 
    /// 当前一个月报写完后  就会使用新的月报数据值 而不在使用这个表的数据
    /// 
    /// </summary>
    [SugarTable("t_monthdiffproductionvalue", IsDisabledDelete = true)]

    public class MonthDiffProductionValue:BaseEntity<Guid>
    {
        /// <summary>
        /// 公司ID
        /// </summary>
        [SugarColumn(Length = 36)]

        public Guid? CompanyId { get; set; }
        /// <summary>
        /// 产值
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]

        public decimal? ProductionValue { get; set; }
        /// <summary>
        /// 月份
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int Month { get; set; }
        /// <summary>
        /// 年份
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int Year { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int Sort { get; set; }
    }
}
