using GHMonitoringCenterApi.Domain.Enums;
using SqlSugar;

namespace GHMonitoringCenterApi.Domain.Models
{

    /// <summary>
    /// 船舶成本表  记录船舶的每月不含油成本 固定船机成本  每日燃油成本  船机总成本
    /// </summary>
    [SugarTable("t_shipmonthcost", IsDisabledDelete = true)]

    public class ShipMonthCost : BaseEntity<Guid>
    {
        /// <summary>
        /// 1 2万方耙  2万方耙   3 5000方耙   4  8527   5 7025   6 抓斗船  7 200方抓斗船   8 辅助船舶-泥驳  9 辅助船舶-拖轮   10 辅助船舶-其他
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public ShipCostType Type { get; set; }
        /// <summary>
        /// category   0 境内  1 境外
        /// </summary>
        public int Category { get; set; }

        /// <summary>
        /// 船舶id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid ShipId { get; set; }
        /// <summary>
        /// 境内 一月份不含油成本  （单位 万元/月）
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? OneNoOilCost { get; set; }
        public decimal? TwoNoOilCost { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? ThreeNoOilCost { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? FourNoOilCost { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? FiveNoOilCost { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? SixNoOilCost { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? SevenNoOilCost { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? EightNoOilCost { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? NineNoOilCost { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? TenNoOilCost { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? ElevenNoOilCost { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? TwelveNoOilCost { get; set; }


        /// <summary>
        /// 境外 一月份不含油成本  （单位 万元/月）
        /// </summary>

        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? OneNoOilCostOverseas { get; set; }
        public decimal? TwoNoOilCostOverseas { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? ThreeNoOilCostOverseas { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? FourNoOilCostOverseas { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? FiveNoOilCostOverseas { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? SixNoOilCostOverseas { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? SevenNoOilCostOverseas { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? EightNoOilCostOverseas { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? NineNoOilCostOverseas { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? TenNoOilCostOverseas { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? ElevenNoOilCostOverseas { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? TwelveNoOilCostOverseas { get; set; }


        /// <summary>
        /// 境内 每天固定船机成本（元/日）  公式 每月不含油成本*10000/30
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? DayShipCost { get; set; }



        /// <summary>
        /// 境外 每天固定船机成本（元/日）  公式 每月不含油成本*10000/30
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? DayShipCostOverseas { get; set; }


        /// <summary>
        ///境内 每日燃油成本（元/日）  （每日油耗吨×燃油单价）
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? OneDayOilCost { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? TwoDayOilCost { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? ThreeDayOilCost { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? FourDayOilCost { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? FiveDayOilCost { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? SixDayOilCost { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? SevenDayOilCost { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? EightDayOilCost { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? NineDayOilCost { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? TenDayOilCost { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? ElevenDayOilCost { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? TwelveDayOilCost { get; set; }

        /// <summary>
        /// 境外 每日燃油成本（元/日）  （每日油耗吨×燃油单价）
        /// </summary>

        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? OneDayOilCostOverseas { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? TwoDayOilCostOverseas { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? ThreeDayOilCostOverseas { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? FourDayOilCostOverseas { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? FiveDayOilCostOverseas { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? SixDayOilCostOverseas { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? SevenDayOilCostOverseas { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? EightDayOilCostOverseas { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? NineDayOilCostOverseas { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? TenDayOilCostOverseas { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? ElevenDayOilCostOverseas { get; set; }
        [SugarColumn(ColumnDataType = "decimal(18,8)")]
        public decimal? TwelveDayOilCostOverseas { get; set; }

        /// <summary>
        ///  境内 船机总成本（元/日）  每日固定船机成本（元/日）+每日燃油成本（元/日）
        /// </summary>
        //[SugarColumn(ColumnDataType = "decimal(18,8)")]
        //public decimal? ShipTotalCost { get; set; }

        /// <summary>
        /// 境外  船机总成本（元/日）  每日固定船机成本（元/日）+每日燃油成本（元/日）
        /// </summary>

        //[SugarColumn(ColumnDataType = "decimal(18,8)")]
        //public decimal? ShipTotalCostOverseas { get; set; }

        /// <summary>
        /// 年份
        /// </summary>
        public int DateYear { get; set; }
    }
}
