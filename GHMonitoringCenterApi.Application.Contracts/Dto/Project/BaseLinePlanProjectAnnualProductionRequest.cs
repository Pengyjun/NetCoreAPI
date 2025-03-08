namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseLinePlanProjectAnnualProductionRequest
    {

    }
    /// <summary>
    /// 列表请求dto
    /// </summary>
    public class SearchBaseLinePlanProjectAnnualProductionRequest : BaseRequestDto
    {
        /// <summary>
        /// 项目id
        /// </summary>
        public Guid? ProjectId { get; set; }
        /// <summary>
        /// 项目生产计划查询  true
        /// </summary>

        public bool IsProjectAnnualProduction {  get; set; }

        /// <summary>
        /// 计划版本
        /// </summary>
        public string? PlanType { get; set; }

        /// <summary>
        /// 计划版本
        /// </summary>
        public string? StartStatus { get; set; }



        ///// <summary>
        ///// 基准计划ID
        ///// </summary>
        //public Guid? BaseLinePlanProject { get; set; }
    }
}
