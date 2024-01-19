using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 年初项目计划产值
    /// </summary>
    [SugarTable("t_yearinitprojectplan", IsDisabledDelete = true)]
    public class YearInitProjectPlan:BaseEntity<Guid>
    {
        /// <summary>
        /// 日期
        /// </summary>
        public int DataMonth { get; set; }
        public int DataYear { get; set; }
        /// <summary>
        /// 项目ID
        /// </summary>
        public Guid? ProjectId { get; set; }
        public Guid? ProjectWbsId { get; set; }
        /// <summary>
        /// 年总工程量
        /// </summary>
        public decimal? YearTotalQuantity { get; set; }
        /// <summary>
        /// 年总产值
        /// </summary>
        public decimal? YearTotalProductionValue { get; set; }

        /// <summary>
        /// 一月份工程量
        /// </summary>
        public decimal? OneQuantity { get; set; }     
        /// <summary>
        /// 一月份产值
        /// </summary>
        public decimal? OneProductionValue { get; set; }
        /// <summary>
        /// 二月份工程量
        /// </summary>
        public decimal? TwoQuantity { get; set; }
        /// <summary>
        /// 二月份产值
        /// </summary>
        public decimal? TwoProductionValue { get; set; }


        public decimal? ThreeQuantity { get; set; }
        public decimal? ThreeProductionValue { get; set; }
        public decimal? FourQuantity { get; set; }
        public decimal? FourProductionValue { get; set; }
        public decimal? FiveQuantity { get; set; }
        public decimal? FiveProductionValue { get; set; }
        public decimal? SixQuantity { get; set; }
        public decimal? SixProductionValue { get; set; }
        public decimal? SevenQuantity { get; set; }
        public decimal? SevenProductionValue { get; set; }
        public decimal? EightQuantity { get; set; }
        public decimal? EightProductionValue { get; set; }
        public decimal? NineQuantity { get; set; }
        public decimal? NineProductionValue { get; set; }
        public decimal? TenQuantity { get; set; }
        public decimal? TenProductionValue { get; set; }
        public decimal? ElevenQuantity { get; set; }
        public decimal? ElevenProductionValue { get; set; }
        public decimal? TwelveQuantity { get; set; }
        public decimal? TwelveProductionValue { get; set; }

    }
}
