using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 项目领导班子表
    /// </summary>
    [SugarTable("t_projectleader", IsDisabledDelete = true)]
    public class ProjectLeader: BaseEntity<Guid>
    {
        /// <summary>
        /// Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? PomId { get; set; }
        /// <summary>
        /// 项目干系人员职位类型
        /// 1：项目经理； 2：执行总经理； 3：项目书记；
        /// 4：项目总工； 5：安全总监； 6：常务副经理；
        /// 7：项目副经理； 8：现场主要负责人； 9：现场技术负责人
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 项目 Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? ProjectId { get; set; }
        /// <summary>
        /// 项目 Code
        /// </summary>
        [SugarColumn(Length = 255)]
        public string? ProjectCode { get; set; }
        /// <summary>
        /// 人员 Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid AssistantManagerId { get; set; }
        /// <summary>
        /// 是否在任 true:在任 false:离任
        /// </summary>
        [SugarColumn(ColumnDataType = "bit")]
        public bool IsPresent { get; set; }
        /// <summary>
        /// 开始日期
        /// </summary>
        [SugarColumn(ColumnDataType ="datetime")]
        public DateTime? BeginDate { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(Length = 1000)]
        public string? Remarks { get; set; }
    }
}
