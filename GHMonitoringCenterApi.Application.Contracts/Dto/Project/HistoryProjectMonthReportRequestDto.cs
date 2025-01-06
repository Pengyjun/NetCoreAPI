namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project
{
    /// <summary>
    /// 历史月报产值请求dto
    /// </summary>
    public class HistoryProjectMonthReportRequestDto : BaseRequestDto
    {
        /// <summary>
        /// 历史填报年份
        /// </summary>
        public int DateYear { get; set; }
    }
    /// <summary>
    /// 历史月报产值入参
    /// </summary>
    public class HistoryProjectMonthReportRequestParam
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid CompleteId { get; set; }
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
}
