using GHMonitoringCenterApi.Domain.Enums;
using GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Domain.Shared.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Job
{
    /// <summary>
    /// 审批人返回对象
    /// </summary>
    public  class JobApproverResponseDto
    {
        /// <summary>
        /// 审批层级
        /// </summary>
        public ApproveLevel ApproveLevel { get; set; }

        /// <summary>
        /// 审批层级名称
        /// </summary>
        public string ApproveLevelName { get { return ApproveLevel.ToDescription(); } }

        /// <summary>
        /// 审批人集合
        /// </summary>
        public ResJobApproverDto[] Approvers { get; set; } = new ResJobApproverDto[0];
    }

    /// <summary>
    /// 审批人
    /// </summary>
    public class ResJobApproverDto: JobApproverDto
    {
        /// <summary>
        /// 唯一Id
        /// </summary>
        public string? Key { get; set; }

        /// <summary>
        /// 审批人名称
        /// </summary>
        public string? ApproverName { get; set; }

        /// <summary>
        /// 审批人电话号码
        /// </summary>
        public string? ApproverPhone { get; set; }
    }
}
