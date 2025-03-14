using GHMonitoringCenterApi.Domain.Enums;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.BaseLinePlan
{

    /// <summary>
    /// 项目审批人Dto
    /// </summary>
    public  abstract  class BaseLinePlanApproverDto
    {
        /// <summary>
        /// 基准计划审批人Id
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// 审批的业务模块
        /// </summary>
        public BizModule BizModule { get; set; }

        /// <summary>
        /// 公司ID
        /// </summary>
        public Guid CompanyId { get; set; }

        /// <summary>
        /// 审批人Id
        /// </summary>
        public Guid ApproverId { get; set; }

        /// <summary>
        /// 审批人层级
        /// </summary>
        public ApproveLevel ApproveLevel { get; set; }

    }
}
