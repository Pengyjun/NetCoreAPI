using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Job
{
    /// <summary>
    /// 任务消息返回结果
    /// </summary>
    public  class JobNoticeResponseDto
    {
        /// <summary>
        /// 未处理的任务条数(审核人未审核的任务条数)
        /// </summary>
        public int UnHandleJobCount { get; set; }
    }
}
