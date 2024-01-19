using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 交建通推送日报相关配置
    /// </summary>
    [SugarTable("t_dayreportjjtpushconfi", IsDisabledDelete = true)]
    public class DayReportJjtPushConfi:BaseEntity<Guid>
    {
        /// <summary>
        /// 推送账号
        /// </summary>
        [SugarColumn(Length = 50)]
        public string? PushAccount { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [SugarColumn(Length = 50)]
        public string? Name { get; set; }
        /// <summary>
        /// 推送类型 0 ：向广航局生产运营部推送   1：向项目经理、局里推送  2：向相关部门推送 3：向相关群进行推送
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int? Type { get; set; }
        /// <summary>
        /// 群号 由交建通手动调用接口创建相关群
        /// </summary>
        [SugarColumn(Length = 200)]
        public string? GroupNumber { get; set; }
        
    }
}
