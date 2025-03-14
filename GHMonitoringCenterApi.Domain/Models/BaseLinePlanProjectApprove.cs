using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 基准计划审批表
    /// </summary>
    [SugarTable("t_baselineplanprojectapprove", IsDisabledDelete = true)]
    public class BaseLinePlanProjectApprove: BaseEntity<Guid>
    {
        [SugarColumn(Length = 36)]
        public string BaseLinePlanid { get; set; }
        [SugarColumn(Length = 36)]
        public Guid ApproveId { get; set; }
        [SugarColumn(Length = 36)]
        public string ApproveName { get; set; }
        [SugarColumn(Length = 36)]
        public string Status { get; set; }
    }
}
