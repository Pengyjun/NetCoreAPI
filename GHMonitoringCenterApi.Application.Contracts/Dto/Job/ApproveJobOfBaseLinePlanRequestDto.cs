using GHMonitoringCenterApi.Application.Contracts.Dto.Project;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report;
using GHMonitoringCenterApi.Domain.Enums;
using GHMonitoringCenterApi.Domain.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Job
{
    /// <summary>
    /// 审批项目月报
    /// </summary>
    public class ApproveJobOfBaseLinePlanRequestDto : ApproveJobRequestDto, IJobBiz, IValidatableObject
    {
        /// <summary>
        /// 业务类型
        /// </summary>
        public BizModule BizModule { get; } = BizModule.BaseLinePlan;

        /// <summary>
        /// 项目月报业务数据
        /// </summary>
        public BaseLinePlanprojectApprove? BizData { get; set; }

        public string GetJsonBizData()
        {
            return JsonConvert.SerializeObject(BizData);
        }
    }
}
