using GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report;
using GHMonitoringCenterApi.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Job
{
    /// <summary>
    /// 定义任务
    /// </summary>
    public interface IJobBiz
    {
        /// <summary>
        /// 业务类型
        /// </summary>
        BizModule BizModule { get; }

        /// <summary>
        /// 获取业务数据
        /// </summary>
        string GetJsonBizData();
    }
}
