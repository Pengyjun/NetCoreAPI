
namespace GHMonitoringCenterApi.Application.Contracts.Dto.Ship
{
    /// <summary>
    /// 自有船舶月报导出模型
    /// </summary>
    public class OwnShipMonthRepExcelDto
    {
        /// <summary>
        /// 自有船舶名称
        /// </summary>
        public string? OwnShipName { get; set; }
        /// <summary>
        /// 船舶分类名称
        /// </summary>
        public string? ShipTypeName { get; set; }
        /// <summary>
        /// 进场时间
        /// </summary>
        public string? EnterTime { get; set; }
        /// <summary>
        /// 退场时间
        /// </summary>
        public string? QuitTime { get; set; }
        /// <summary>
        /// 本月运转时间
        /// </summary>
        public decimal MonthWorkHours { get; set; }
        /// <summary>
        /// 本月施工天数
        /// </summary>
        public decimal MonthWorkDays { get; set; }
        /// <summary>
        /// 合同清单类型
        /// </summary>
        public string? ContractTypeName { get; set; }
        /// <summary>
        /// 本月完成工程量
        /// </summary>
        public decimal MonthQuantity { get; set; }
        /// <summary>
        /// 本月施工产值
        /// </summary>
        public decimal MonthOutputVal { get; set; }
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
        /// 项目名称
        /// </summary>
        public string? ProjectName { get; set; }
        /// <summary>
        /// 二级单位名称
        /// </summary>
        public string? SecUnitName { get; set; }
        /// <summary>
        /// 三级单位名称
        /// </summary>
        public string? ThiUnitName { get; set; }
        /// <summary>
        /// 负责人名称
        /// </summary>
        public string? HeadUserName { get; set; }
        /// <summary>
        /// 负责人电话
        /// </summary>
        public string? HeadUserTel { get; set; }
        /// <summary>
        /// 报表负责人
        /// </summary>
        public string? RepUserName { get; set; }
        /// <summary>
        /// 报表联系人电话
        /// </summary>
        public string? RepUserTel { get; set; }
        /// <summary>
        /// 是否审核
        /// </summary>
        public string IsExamine { get; set; }
        /// <summary>
        /// 提交日期
        /// </summary>
        public string SubmitDate { get; set; }
    }
    public class SubShipMonthRepExcelDto
    {
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
        /// 是否审核
        /// </summary>
        public string IsExamine { get; set; }
        /// <summary>
        /// 提交日期
        /// </summary>
        public string SubmitDate { get; set; }
    }
    /// <summary>
    /// 自有船舶月报导出
    /// </summary>
    public class ExcelImportResponseDto
    {
        /// <summary>
        /// 自有船舶
        /// </summary>
        public List<OwnShipMonthRepExcelDto> OwnShipMonthRepExcelDtos { get; set; }
        /// <summary>
        /// 分包船舶
        /// </summary>
        public List<SubShipMonthRepExcelDto> SubShipMonthRepExcelDtos { get; set; }
    }
}
