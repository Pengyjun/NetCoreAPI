using GHMonitoringCenterApi.Application.Contracts.Dto.Enums;
using GHMonitoringCenterApi.Domain.Enums;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Job
{
    /// <summary>
    /// 任务业务数据请求
    /// </summary>
    public  abstract   class  JobBizRequestDto
    {
        /// <summary>
        /// 任务Id
        /// </summary>
        public Guid? JobId { get; set; }

        /// <summary>
        /// 项目Id
        /// </summary>
        public Guid? ProjectId { get; set; }

        /// <summary>
        /// 请求类型(1:查找任务中业务数据（草稿数据），2：查找库中业务数据)
        /// </summary>
        public JobBizRequestType RequestType { get; set; }

        /// <summary>
        /// 业务模块
        /// </summary>
        public virtual   BizModule BizModule { get; }

       
    }
}
