using GHMonitoringCenterApi.Application.Contracts.Dto.Enums;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project;
using GHMonitoringCenterApi.Domain.Enums;
using GHMonitoringCenterApi.Domain.Shared.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Job
{
    /// <summary>
    /// 提交审批任务请求
    ///<para>业务页面-提交审核：必传 SubmitType=1,ProjectId，Approvers,BizData</para> 
    ///<para>草稿箱-提交审核：必传 SubmitType=2,JobId,BizData</para>
    /// </summary>
    public class SubmitJobOfProjectWBSRequestDto : SubmitJobRequestDto<SaveProjectWBSRequestDto>
    {

        /// <summary>
        /// 业务类型
        /// </summary>
        public override BizModule BizModule { get { return BizModule.ProjectWBS; } }
    }
}
