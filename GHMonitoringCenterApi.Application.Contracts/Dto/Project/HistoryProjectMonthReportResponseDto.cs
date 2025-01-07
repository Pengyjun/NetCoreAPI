namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project
{
    /// <summary>
    /// 历史产报列表
    /// </summary>
    public class HistoryProjectMonthReportResponseDto
    {
        /// <summary>
        /// 完成产值主键
        /// </summary>
        public Guid CompleteId { get; set; }
        /// <summary>
        /// 项目id
        /// </summary>
        public Guid ProjectId { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string? ProjectName { get; set; }

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
        /// <summary>
        /// 所属年份
        /// </summary>
        public int Year { get; set; }

        #region 合计
        /// <summary>
        /// 年度累计完成产值
        /// </summary>
        public decimal? TotalCompleteValue { get; set; }
        /// <summary>
        /// 开累
        /// </summary>
        public decimal? AccumulateuCompleteValue { get; set; }
        #endregion
    }
}
