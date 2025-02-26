using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{

    /// <summary>
    /// 日报推送开关表
    /// </summary>
    [SugarTable("t_projectopen", IsDisabledDelete = true)]
    public class ProjectOpen:BaseEntity<Guid>
    {
        [SugarColumn(Length = 36)]
        public string Name { get; set; }
        [SugarColumn(Length = 36)]
        public Guid ProjectId { get; set; }
        [SugarColumn(ColumnDataType = "int")]
        public int DateDay { get; set; }
        [SugarColumn(ColumnDataType = "int")]
        public int IsShow { get; set; }
    }
}
