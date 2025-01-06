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
        public Guid? CompleteId { get; set; }
        /// <summary>
        /// 计划产值主键
        /// </summary>
        public Guid? PlanId { get; set; }
        /// <summary>
        /// 项目id
        /// </summary>
        public Guid ProjectId { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string? ProjectName { get; set; }
        /// <summary>
        /// 月完成产值
        /// </summary>
        public decimal? CompleteValue { get; set; }
        /// <summary>
        /// 月计划产值
        /// </summary>
        public decimal? PlanValue { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime DateMonth { get; set; }
        /// <summary>
        /// 所属年份
        /// </summary>
        public int Year { get; set; }

        #region 合计行
        /// <summary>
        /// 累计完成产值
        /// </summary>
        public decimal? TotalCompleteValue { get; set; }
        #endregion
    }
}
