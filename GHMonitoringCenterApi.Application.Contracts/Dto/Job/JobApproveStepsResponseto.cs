
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GHMonitoringCenterApi.Domain.Enums;
using UtilsSharp;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Job
{
    /// <summary>
    /// 任务审批流程返回结果
    /// </summary>
    public class JobApproveStepsResponseto
    {
        /// <summary>
        /// 当前进入的层级
        /// </summary>
        public ApproveLevel CurrentApproveLevel { get; set; }

        /// <summary>
        /// 是否结束
        /// </summary>
        public bool IsFinish { get; set; }

        /// <summary>
        /// 审批流程步骤集合
        /// </summary>
        public ResJobApproveStep[]? Steps { get; set; }

        /// <summary>
        /// 审批流程步骤
        /// </summary>
        public class ResJobApproveStep
        {
            /// <summary>
            /// 审批状态(0:未审核，1：审核驳回，2：审核通过)
            /// </summary>
            public JobApproveStatus ApproveStatus { get; set; }

            /// <summary>
            /// 审批层级（1：一级审批）
            /// </summary>
            public ApproveLevel ApproveLevel { get; set; }

            /// <summary>
            /// 审批层级名称
            /// </summary>
            public string? ApproveLevelName { get { return EnumExtension.GetEnumDescription(this.ApproveLevel); } }
    }
    }
}
