using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 每天推送日报结果记录
    /// </summary>
    [SugarTable("t_recordpushdayreport", IsDisabledDelete = true)]
    public class RecordPushDayReport:BaseEntity<Guid>
    {

        /// <summary>
        /// 日期
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int DateDay { get; set; }
        /// <summary>
        /// json结果集
        /// </summary>
        [SugarColumn(ColumnDataType = "text")]
        public string? Json { get; set; }
    }
}
