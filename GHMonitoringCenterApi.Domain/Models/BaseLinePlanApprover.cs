using GHMonitoringCenterApi.Domain.Enums;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 基准计划审批人
    /// </summary>
    [SugarTable("t_baselineplanapprover", IsDisabledDelete = true)]
    public class BaseLinePlanApprover : BaseEntity<Guid>
    {
        /// <summary>
        /// 公司Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid CompanyId { get; set; }

        /// <summary>
        /// 审批的业务模块
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public BizModule BizModule { get; set; }

        /// <summary>
        /// 审批人Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid ApproverId { get; set; }

        /// <summary>
        /// 审批人层级
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public ApproveLevel ApproveLevel { get; set; }
    }
}
