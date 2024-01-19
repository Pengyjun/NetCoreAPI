
namespace GHMonitoringCenterApi.Application.Contracts.Dto.Ship
{
    /// <summary>
    /// 船舶成本维护响应dto
    /// </summary>
    public class ShipCostMaintenanceResponseDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 船舶名称
        /// </summary>
        public string? OwnShipName { get; set; }

        /// <summary>
        /// 船舶Id
        /// </summary>
        public Guid OwnShipId { get; set; }

        /// <summary>
        /// 船机成本类型名称
        /// </summary>
        public string? ShipCostTypeName { get; set; }

        /// <summary>
        /// 船机成本类型
        /// </summary>
        public int ShipCostType { get; set; }

        /// <summary>
        /// 境内境外
        /// </summary>
        public string? CategoryName { get; set; }

        /// <summary>
        /// 境内境外 0  1
        /// </summary>
        public int Category { get; set; }

        /// <summary>
        /// 不含油成本
        /// </summary>
        public decimal? NoOilCost { get; set; }

        /// <summary>
        /// 固定船机成本
        /// </summary>
        public decimal? DayShipCost { get; set; }

        /// <summary>
        /// 燃油成本
        /// </summary>
        public decimal? OilCost { get; set; }

        /// <summary>
        /// 船机总成本
        /// </summary>
        public decimal? ShipTotalCost { get; set; }

        /// <summary>
        /// 当前年月
        /// </summary>
        public string? NowDate { get; set; }

        /// <summary>
        /// 用来分组的自己生成的id
        /// </summary>
        public Guid MergeCellId { get; set; }

        /// <summary>
        /// 是否禁用编辑删除按钮
        /// </summary>
        public bool IsDisabled { get; set; }
    }

    /// <summary>
    /// 不含油成本、每日固定船机成本、船机总成本、每日燃油成本
    /// </summary>
    public class OutputValue
    {

        /// <summary>
        /// 不含油成本
        /// </summary>
        public decimal? NoOilCost { get; set; }

        /// <summary>
        /// 固定船机成本
        /// </summary>
        public decimal? DayShipCost { get; set; }

        /// <summary>
        /// 燃油成本
        /// </summary>
        public decimal? OilCost { get; set; }

        /// <summary>
        /// 船机总成本
        /// </summary>
        public decimal? ShipTotalCost { get; set; }
    }

    /// <summary>
    /// 船舶 and 分类 列表返回
    /// </summary>
    public class SearchOwnShipAndShipTypeDto
    {
        /// <summary>
        /// 船舶列表
        /// </summary>
        public List<SearchOwnShip>? SearchOwnShips { get; set; }

        /// <summary>
        /// 枚举船舶类型列表
        /// </summary>
        public List<SearchShipType>? SearchShipTypes { get; set; }

    }

    /// <summary>
    /// 船舶列表
    /// </summary>
    public class SearchOwnShip
    {
        /// <summary>
        /// 船舶id
        /// </summary>
        public Guid OwnShipId { get; set; }

        /// <summary>
        /// 船舶名称
        /// </summary>
        public string? OwnShipName { get; set; }
    }

    /// <summary>
    /// 船舶类型列表
    /// </summary>
    public class SearchShipType
    {
        /// <summary>
        /// 枚举类型主键
        /// </summary>
        public int EnumKey { get; set; }

        /// <summary>
        /// 枚举船舶类型名称
        /// </summary>
        public string? EnumValueName { get; set; }
    }
}
