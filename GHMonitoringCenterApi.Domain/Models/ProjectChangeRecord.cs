using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 项目变更记录 日报未填报使用
    /// </summary>
    [SugarTable("t_projectchangerecord", IsDisabledDelete = true)]
    public class ProjectChangeRecord
    {
        [SugarColumn(Length = 36)]
        public Guid Id { get; set; }
        [SugarColumn(Length = 36)]
        public Guid ProjectId { get; set; }
        [SugarColumn(Length =5)]
        public int Status { get; set; }
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime StartTime { get; set; }
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime EndTime { get; set; }
    }
}
