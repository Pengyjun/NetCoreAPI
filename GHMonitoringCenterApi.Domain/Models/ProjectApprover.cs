using GHMonitoringCenterApi.Domain.Enums;
using SqlSugar;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 项目审批人
    /// </summary>

    [SugarTable("t_projectapprover", IsDisabledDelete = true)]
    public class ProjectApprover : BaseEntity<Guid>
    {
        /// <summary>
        /// 项目Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid ProjectId { get; set; }

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
