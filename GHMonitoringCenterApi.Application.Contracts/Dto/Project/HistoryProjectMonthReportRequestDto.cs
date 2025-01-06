namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project
{
    /// <summary>
    /// 历史月报产值请求dto
    /// </summary>
    public class HistoryProjectMonthReportRequestDto : BaseRequestDto
    {
        /// <summary>
        /// 历史填报月份
        /// </summary>
        public DateTime? DateMonth { get; set; }
    }
}
