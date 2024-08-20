namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report
{
    /// <summary>
    /// 月报产值转换dto
    /// </summary>
    public class MonthReportForProjectConvert
    {
        /// <summary>
        /// 年度完成工程量
        /// </summary>
        public decimal YearAccomplishQuantities {  get; set; }
        /// <summary>
        /// 年度完成成产值
        /// </summary>
        public decimal YearAccomplishCost {  get; set; }
        /// <summary>
        /// 年度完成成产值
        /// </summary>
        public decimal YearAccomplishValue {  get; set; }
        /// <summary>
        /// 年付款金额
        /// </summary>
        public decimal YearPaymentAmount {  get; set; }
        /// <summary>
        /// 年甲方确认产值
        /// </summary>
        public decimal YearRecognizedValue {  get; set; }
        /// <summary>
        /// 年预计成本
        /// </summary>
        public decimal YearProjectedCost {  get; set; }
        /// <summary>
        /// 年外包支出
        /// </summary>
        public decimal YearOutsourcingExpensesAmount {  get; set; }
        /// <summary>
        /// 累计完成工程量
        /// </summary>
        public decimal AccumulativeQuantities {  get; set; }
        /// <summary>
        /// 累计完成成本
        /// </summary>
        public decimal CumulativeAccomplishCost {  get; set; }
        /// <summary>
        /// 累计完成产值
        /// </summary>
        public decimal CumulativeCompleted {  get; set; }
        /// <summary>
        /// 累计付款金额
        /// </summary>
        public decimal CumulativePaymentAmount {  get; set; }
        /// <summary>
        /// 累计甲方确认产值
        /// </summary>
        public decimal CumulativeValue {  get; set; }
        /// <summary>
        /// 累计外包支出
        /// </summary>
        public decimal CumulativeOutsourcingExpensesAmount {  get; set; }
    }
}
