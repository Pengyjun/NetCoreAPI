using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 公司产值信息
    /// </summary>
    [SugarTable("t_companyproductionvalueinfo", IsDisabledDelete = true)]
    public class CompanyProductionValueInfo:BaseEntity<Guid>
    {
        /// <summary>
        /// 公司ID
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid CompanyId { get; set; }
        [SugarColumn(ColumnDataType = "int")]
        public int? DateDay  { get; set; }
        /// <summary>
        /// 第一月完成产值
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? OneCompleteProductionValue { get; set; }
        /// <summary>
        /// 第二月完成产值
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? TwoCompleteProductionValue { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? ThreeCompleteProductionValue { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? FourCompleteProductionValue { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? FiveCompleteProductionValue { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? SixCompleteProductionValue { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? SevenCompleteProductionValue { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? EightCompleteProductionValue { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? NineCompleteProductionValue { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? TenCompleteProductionValue { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? ElevenCompleteProductionValue { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? TwelveCompleteProductionValue { get; set; }

        /// <summary>
        /// 第一月计划产值
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? OnePlanProductionValue { get; set; }
        /// <summary>
        /// 第二月计划产值
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? TwoPlanProductionValue { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? ThreePlaProductionValue { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? FourPlaProductionValue { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? FivePlaProductionValue { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? SixPlaProductionValue { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? SevenPlaProductionValue { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? EightPlaProductionValue { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? NinePlaProductionValue { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? TenPlaProductionValue { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? ElevenPlaProductionValue { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? TwelvePlaProductionValue { get; set; }
        [SugarColumn(ColumnDataType = "int")]
        public int? Sort { get; set; }
        /// <summary>
        /// 每年指标
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,6)")]
        public decimal? YearIndex { get; set; }
    }
}
