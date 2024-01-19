using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{

    /// <summary>
    /// 项目表流水记录  此表是交建通推送图片消息使用其他业务暂时没有使用
    /// </summary>
    [SugarTable("t_projectrunningwaterrecord", IsDisabledDelete = true)]
    public class ProjectRunningWaterRecord:BaseEntity<Guid>
    {
        /// <summary>
        /// 项目ID
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid ProjectId { get; set; }
        /// <summary>
        /// 改变前的项目状态ID
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? OldStatusId { get; set; }
        /// <summary>
        /// 改变后的项目状态ID
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? NewStatusId { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        [SugarColumn(Length = 300)]
        public string? Name { get; set; }
        /// <summary>
        /// 所属公司
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? CompanyId { get; set; }
    }
}
