using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 系统日志表
    /// </summary>
    [SugarTable("t_systemupdatlog", IsDisabledDelete = true)]
    public class SystemUpdatLogs: BaseEntity<Guid>
    {      
        /// <summary>
        /// 日志内容
        /// </summary>
        [SugarColumn(Length = 1000)]
        public string? LogContent { get; set; }
        /// <summary>
        /// 日期天数
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int DateDay { get; set; }
     }
}
