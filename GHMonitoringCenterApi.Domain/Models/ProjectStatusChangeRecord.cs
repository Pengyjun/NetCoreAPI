using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{


    /// <summary>
    /// 项目状态记录表
    /// </summary>
    [SugarTable("t_projectstatuschangerecord", IsDisabledDelete = true)]
    public class ProjectStatusChangeRecord
    {
        [SugarColumn(Length = 36)]
        public Guid Id { get; set; }
        [SugarColumn(Length = 36)]
        public Guid OldStatus { get; set; }
        [SugarColumn(Length = 36)]
        public Guid NewStatus { get; set; }
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime ChangeTime { get; set; }

        /// <summary>
        /// 是否有效   辅助字段  2023年7月份有用 后面无用  0无效 1有效
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime",DefaultValue ="1")]
        public int? IsValid { get; set; }
        /// <summary>
        /// 距离上一次暂停到在建停了多少天
        /// </summary>
        [SugarColumn(ColumnDataType = "int", DefaultValue = "0")]
        public int? StopDay { get; set; }
    }
}
