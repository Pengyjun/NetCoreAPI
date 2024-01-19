
using GHMonitoringCenterApi.Application.Contracts.Dto.ResourceManagement;
using GHMonitoringCenterApi.Domain.Enums;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Ship
{
    /// <summary>
    /// 船舶日报图片
    /// </summary>
    public class JjtOwnShipDayRepDto
    {
        /// <summary>
        /// 船舶日报图片
        /// </summary>
        public List<JjtOwnShipDayRepResponseDto> JjtOwnShipDayRepResponse { get; set; }
        /// <summary>
        /// 船舶修理滚动计划与执行
        /// </summary>
        public List<SearchShipRepairRollingResponseDto> resourceManagementSearch { get; set; }
        /// <summary>
        /// 推送日期
        /// </summary>
        public string? PushTime { get; set; }
    }
    /// <summary>
    /// 交建通船舶日报响应列表
    /// </summary>
    public class JjtOwnShipDayRepResponseDto
    {
        /// <summary>
        /// 船舶id
        /// </summary>
        public Guid? OwnShipId { get; set; }
        /// <summary>
        /// 船舶名称
        /// </summary>
        public string? OwnShipName { get; set; }
        /// <summary>
        /// 船舶类型
        /// </summary>
        public string? OwnShipType { get; set; }
        /// <summary>
        /// 项目id
        /// </summary>
        public Guid ProjectId { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string? ProjectName { get; set; }
        /// <summary>
        /// 船舶动态枚举值
        /// </summary>
        public ProjectShipState ShipDynamic { get; set; }
        /// <summary>
        /// 船舶动态名称
        /// </summary>
        public string? ShipDynamicName { get; set; }
        /// <summary>
        /// 施工模式
        /// </summary>
        public string? ConstructionMode { get; set; }
        /// <summary>
        /// 土质
        /// </summary>
        public string? SoilQuality { get; set; }
        /// <summary>
        /// 吹距（m）
        /// </summary>
        public decimal? BlowingDistance { get; set; }
        /// <summary>
        /// 燃油单价
        /// </summary>
        public decimal? FuelUnitPrice { get; set; }
        /// <summary>
        /// 估算产值
        /// </summary>
        public decimal? EstimatedOutputAmount { get; set; }
        /// <summary>
        /// 境内境外
        /// </summary>
        public int Category { get; set; }
        /// <summary>
        /// 船舶产量
        /// </summary>
        public decimal? Production { get; set; }
        /// <summary>
        /// 施工船数
        /// </summary>
        public decimal WorkShips { get; set; }
        /// <summary>
        /// 生产率
        /// </summary>
        public decimal? WorkRate { get; set; }
        /// <summary>
        /// 运转时间（h:m）
        /// </summary>
        public decimal? WorkHours { get; set; }
        /// <summary>
        /// 时间利用率（%）
        /// </summary>
        public decimal? HoursRate { get; set; }
        /// <summary>
        /// 油耗
        /// </summary>
        public decimal OilConsumption { get; set; }
        /// <summary>
        /// 万方油耗(t)
        /// </summary>
        public decimal? WanfangOilConsumption { get; set; }
        /// <summary>
        /// 燃油补给(t)
        /// </summary>
        public decimal? FuelSupply { get; set; }
        /// <summary>
        /// 船存燃油
        /// </summary>
        public decimal? ShipboardFuel { get; set; }
        /// <summary>
        /// 当日毛利率
        /// </summary>
        public decimal? DayGrossMargin { get; set; }
        /// <summary>
        /// 船舶排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
    }
}
