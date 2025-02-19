using NPOI.SS.Formula.Functions;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 日报审批表
    /// </summary>
    [SugarTable("t_daypushapprove", IsDisabledDelete = true)]
    public class DayPushApprove:BaseEntity<Guid>
    {

        [SugarColumn(ColumnDataType = "int")]
        public int  DayTime { get; set; }
        [SugarColumn(Length = 36)]
        public Guid ApproveId { get; set; }
        [SugarColumn(Length = 36)]
        public string ApproveName { get; set; }
        [SugarColumn(Length = 36)]
        public string Status { get; set; }
    }
}
