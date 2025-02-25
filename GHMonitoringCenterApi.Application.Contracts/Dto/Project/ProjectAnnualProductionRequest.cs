namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project
{
    /// <summary>
    /// 
    /// </summary>
    public class ProjectAnnualProductionRequest
    {

    }
    /// <summary>
    /// 列表请求dto
    /// </summary>
    public class SearchProjectAnnualProductionRequest : BaseRequestDto
    {
        /// <summary>
        /// 项目id
        /// </summary>
        public Guid? ProjectId { get; set; }
        /// <summary>
        /// 项目生产计划查询  true
        /// </summary>

        public bool IsProjectAnnualProduction {  get; set; }
    }
}
