using GHMonitoringCenterApi.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.BaseLinePlan
{
    /// <summary>
    /// 获取项目审批人集合
    /// </summary>
    public  class BaseLinePlanApproverRequestDto
    {
        /// <summary>
        /// 公司Id
        /// </summary>
        public Guid? CompanyId { get; set; }


        /// <summary>
        /// 审批的业务模块
        /// </summary>
        public BizModule BizModule { get; set; }
    }
}
