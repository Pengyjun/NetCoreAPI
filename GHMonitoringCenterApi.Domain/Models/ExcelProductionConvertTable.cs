using SqlSugar;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// excel历史数据转化中间表 完成
    /// </summary>
    [SugarTable("t_excelproductionconverttable", IsDisabledDelete = true)]
    public class ExcelProductionConvertTable
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? ProjectId { get; set; }
        public int Year { get; set; }
        public int DateMonth { get; set; }

        #region 完成

        /// <summary>
        /// 一月
        /// </summary>
        public decimal? OneCompleteValue { get; set; }
        /// <summary>
        /// 二月
        /// </summary>
        public decimal? TwoCompleteValue { get; set; }
        /// <summary>
        /// 三月
        /// </summary>
        public decimal? ThreeCompleteValue { get; set; }
        /// <summary>
        /// 四月
        /// </summary>
        public decimal? FourCompleteValue { get; set; }
        /// <summary>
        /// 五月
        /// </summary>
        public decimal? FiveCompleteValue { get; set; }
        /// <summary>
        /// 六月
        /// </summary>
        public decimal? SixCompleteValue { get; set; }
        /// <summary>
        /// 七月
        /// </summary>
        public decimal? SevenCompleteValue { get; set; }
        /// <summary>
        /// 八月
        /// </summary>
        public decimal? EightCompleteValue { get; set; }
        /// <summary>
        /// 九月
        /// </summary>
        public decimal? NineCompleteValue { get; set; }
        /// <summary>
        /// 十月
        /// </summary>
        public decimal? TenCompleteValue { get; set; }
        /// <summary>
        /// 十一月
        /// </summary>
        public decimal? ElevenCompleteValue { get; set; }
        /// <summary>
        /// 十二月
        /// </summary>
        public decimal? TwelveCompleteValue { get; set; }
        #endregion
    }
    /// <summary>
    /// excel历史数据转化中间表  计划
    /// </summary>
    [SugarTable("t_excelplanconverttable", IsDisabledDelete = true)]
    public class ExcelPlanConvertTable
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? ProjectId { get; set; }
        public int Year { get; set; }
        public int DateMonth { get; set; }

        #region 计划
        /// <summary>
        /// 一月
        /// </summary>
        public decimal? OnePlanProductionValue { get; set; }
        /// <summary>
        /// 二月
        /// </summary>
        public decimal? TwoPlanProductionValue { get; set; }
        /// <summary>
        /// 三月
        /// </summary>
        public decimal? ThreePlanProductionValue { get; set; }
        /// <summary>
        /// 四月
        /// </summary>
        public decimal? FourPlanProductionValue { get; set; }
        /// <summary>
        /// 五月
        /// </summary>
        public decimal? FivePlanProductionValue { get; set; }
        /// <summary>
        /// 六月
        /// </summary>
        public decimal? SixPlanProductionValue { get; set; }
        /// <summary>
        /// 七月
        /// </summary>
        public decimal? SevenPlanProductionValue { get; set; }
        /// <summary>
        /// 八月
        /// </summary>
        public decimal? EightPlanProductionValue { get; set; }
        /// <summary>
        /// 九月
        /// </summary>
        public decimal? NinePlanProductionValue { get; set; }
        /// <summary>
        /// 十月
        /// </summary>
        public decimal? TenPlanProductionValue { get; set; }
        /// <summary>
        /// 十一月
        /// </summary>
        public decimal? ElevenPlanProductionValue { get; set; }
        /// <summary>
        /// 十二月
        /// </summary>
        public decimal? TwelvePlanProductionValue { get; set; }
        #endregion
    }

    public class ConvertHelperForExcel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? ProjectId { get; set; }
        public int Year { get; set; }
        public int DateMonth { get; set; }

        #region 完成

        /// <summary>
        /// 一月
        /// </summary>
        public decimal? OneCompleteValue { get; set; }
        /// <summary>
        /// 二月
        /// </summary>
        public decimal? TwoCompleteValue { get; set; }
        /// <summary>
        /// 三月
        /// </summary>
        public decimal? ThreeCompleteValue { get; set; }
        /// <summary>
        /// 四月
        /// </summary>
        public decimal? FourCompleteValue { get; set; }
        /// <summary>
        /// 五月
        /// </summary>
        public decimal? FiveCompleteValue { get; set; }
        /// <summary>
        /// 六月
        /// </summary>
        public decimal? SixCompleteValue { get; set; }
        /// <summary>
        /// 七月
        /// </summary>
        public decimal? SevenCompleteValue { get; set; }
        /// <summary>
        /// 八月
        /// </summary>
        public decimal? EightCompleteValue { get; set; }
        /// <summary>
        /// 九月
        /// </summary>
        public decimal? NineCompleteValue { get; set; }
        /// <summary>
        /// 十月
        /// </summary>
        public decimal? TenCompleteValue { get; set; }
        /// <summary>
        /// 十一月
        /// </summary>
        public decimal? ElevenCompleteValue { get; set; }
        /// <summary>
        /// 十二月
        /// </summary>
        public decimal? TwelveCompleteValue { get; set; }
        #endregion

        #region 计划
        /// <summary>
        /// 一月
        /// </summary>
        public decimal? OnePlanProductionValue { get; set; }
        /// <summary>
        /// 二月
        /// </summary>
        public decimal? TwoPlanProductionValue { get; set; }
        /// <summary>
        /// 三月
        /// </summary>
        public decimal? ThreePlanProductionValue { get; set; }
        /// <summary>
        /// 四月
        /// </summary>
        public decimal? FourPlanProductionValue { get; set; }
        /// <summary>
        /// 五月
        /// </summary>
        public decimal? FivePlanProductionValue { get; set; }
        /// <summary>
        /// 六月
        /// </summary>
        public decimal? SixPlanProductionValue { get; set; }
        /// <summary>
        /// 七月
        /// </summary>
        public decimal? SevenPlanProductionValue { get; set; }
        /// <summary>
        /// 八月
        /// </summary>
        public decimal? EightPlanProductionValue { get; set; }
        /// <summary>
        /// 九月
        /// </summary>
        public decimal? NinePlanProductionValue { get; set; }
        /// <summary>
        /// 十月
        /// </summary>
        public decimal? TenPlanProductionValue { get; set; }
        /// <summary>
        /// 十一月
        /// </summary>
        public decimal? ElevenPlanProductionValue { get; set; }
        /// <summary>
        /// 十二月
        /// </summary>
        public decimal? TwelvePlanProductionValue { get; set; }
        #endregion
    }
}
