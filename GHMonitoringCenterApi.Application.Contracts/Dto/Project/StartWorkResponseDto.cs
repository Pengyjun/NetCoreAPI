namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project
{


    /// <summary>
    /// 开停工时间
    /// </summary>
    public class StartWorkResponseDto
    {
        /// <summary>
        /// 项目ID
        /// </summary>
        public Guid? ProjectId { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string? CompanyName { get; set; }
        /// 开工时间
        /// </summary>
        public DateTime? StartWorkTime { get; set; }
        /// <summary>
        /// 停工时间
        /// </summary>
        public DateTime? EndWorkTime { get; set; }
        /// <summary>
        /// 停工原因
        /// </summary>
        public string? StopWorkReson { get; set; }
        /// <summary>
        /// 修改前状态
        /// </summary>
        public string? BeforeStatus { get; set; }
        /// <summary>
        /// 修改后状态
        /// </summary>
        public string? AfterStatus { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public string? UpdateUser { get; set; }
    }
}
