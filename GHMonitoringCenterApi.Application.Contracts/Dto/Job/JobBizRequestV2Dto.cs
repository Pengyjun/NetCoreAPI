using GHMonitoringCenterApi.Domain.Enums;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Job
{
    /// <summary>
    /// 任务业务请求Dto
    /// </summary>
    public  class JobBizRequestV2Dto
    {

        /// <summary>
        /// 任务Id
        /// </summary>
        public Guid JobId { get; set; }

        /// <summary>
        /// 业务模块
        /// </summary>
        public BizModule BizModule { get; set; }
    }
}
