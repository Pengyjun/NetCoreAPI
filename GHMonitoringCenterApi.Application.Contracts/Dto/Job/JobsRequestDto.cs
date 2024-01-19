using GHMonitoringCenterApi.Application.Contracts.Dto.Enums;
using GHMonitoringCenterApi.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Job
{
    /// <summary>
    /// 获取任务集合请求
    /// </summary>
    public  class JobsRequestDto:BaseRequestDto
    {

        /// <summary>
        /// 任务状态 1:代办，2：已办
        /// </summary>
        public JobStatus JobStatus { get; set; }

        /// <summary>
        /// 业务模块
        /// </summary>
        public  BizModule? BizModule { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string? ProjectName { get; set; }

        /// <summary>
        /// 任务列表查找类型
        /// </summary>
        public JobsFindType? FindType { get; set; }


        /// <summary>
        /// 提交起始时间
        /// </summary>
        public DateTime? StartSubmitTime { get; set; }

        /// <summary>
        /// 提交结束时间
        /// </summary>
        public DateTime? EndSubmitTime { get; set; }
    }
}
