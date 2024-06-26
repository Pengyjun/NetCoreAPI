namespace GHMonitoringCenterApi.Application.Contracts.Dto.Ship
{
    /// <summary>
    /// 分包船舶月报列表
    /// </summary>
    public class SearchSubShipMonthRepResponseDto
    {
        /// <summary>
        /// 月报主键id
        /// </summary>
        public Guid? Id { get; set; }
        /// <summary>
        /// 项目id
        /// </summary>
        public Guid? ProjectId { get; set; }
        /// <summary>
        /// 分包船舶id
        /// </summary>
        public Guid? SubShipId { get; set; }
        /// <summary>
        /// 分包船名称
        /// </summary>
        public string? SubShipName { get; set; }
        /// <summary>
        /// 船舶类型名称
        /// </summary>
        public string? ShipTypeName { get; set; }
        /// <summary>
        /// 单位名称
        /// </summary>
        public string? UnitName { get; set; }
        /// <summary>
        /// 进场时间
        /// </summary>
        public string? EnterTime { get; set; }
        /// <summary>
        /// 退场时间
        /// </summary>
        public string? QuitTime { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string? ProjectName { get; set; }
        /// <summary>
        /// 合同清单类型
        /// </summary>
        public string? ContractTypeName { get; set; }
        /// <summary>
        /// 区域名称
        /// </summary>
        public string? RegionName { get; set; }
        /// <summary>
        /// 省域国别（地点 不精确到省）
        /// </summary>
        public string? CountryName { get; set; }
        /// <summary>
        /// 本月运转时间
        /// </summary>
        public decimal MonthWorkHours { get; set; }
        /// <summary>
        /// 本月施工天数
        /// </summary>
        public decimal MonthWorkDays { get; set; }
        /// <summary>
        /// 本月完成工程量
        /// </summary>
        public decimal MonthQuantity { get; set; }
        /// <summary>
        /// 本月施工产值
        /// </summary>
        public decimal MonthOutputVal { get; set; }
        /// <summary>
        /// 本月计划产值
        /// </summary>
        public decimal MonthPlanOutputVal { get; set; }
        /// <summary>
        /// 本月计划工程量
        /// </summary>
        public decimal MonthPlanQuantity { get; set; }
        /// <summary>
        /// 累计完成产值
        /// </summary>
        public decimal AccumulateOutputVal { get; set; }
        /// <summary>
        /// 累计完成工程量
        /// </summary>
        public decimal AccumulateQuantity { get; set; }
        /// <summary>
        /// 年度运转时间
        /// </summary>
        public decimal YearWorkHours { get; set; }
        /// <summary>
        /// 年度运转天数
        /// </summary>
        public decimal YearWorkDays { get; set; }
        /// <summary>
        /// 年度完成工程量
        /// </summary>
        public decimal YearQuantity { get; set; }
        /// <summary>
        /// 年度完成产值
        /// </summary>
        public decimal YearOutputVal { get; set; }
        /// <summary>
        /// 二级单位名称
        /// </summary>
        public string? SecUnitName { get; set; }
        /// <summary>
        /// 项目三级单位名称
        /// </summary>
        public string? ThiUnitName { get; set; }
        /// <summary>
        /// 动态概述
        /// </summary>
        public string? DynamicContent { get; set; }
        /// <summary>
        /// 船舶动态
        /// </summary>
        public int ShipDynamic { get; set; }
        /// <summary>
        /// 是否审核
        /// </summary>
        public int IsExamine { get; set; }
        /// <summary>
        /// 提交日期
        /// </summary>
        public DateTime SubmitDate { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }
    }
}
