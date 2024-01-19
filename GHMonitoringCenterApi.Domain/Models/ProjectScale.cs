using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 项目规模
    /// </summary>
    [SugarTable("t_projectscale", IsDisabledDelete = true)]
    public class ProjectScale: BaseEntity<Guid>
    {
        /// <summary>
        /// Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid PomId { get; set; }
        /// <summary>
        /// parentId
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? ParentId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [SugarColumn(Length = 255)]
        public string? Name { get; set; }
        /// <summary>
        /// 简称
        /// </summary>
        [SugarColumn(Length = 255)]
        public string? ShortName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [SugarColumn(Length = 255)]
        public string? Code { get; set; }
        /// <summary>
        /// 序列
        /// </summary>
        [SugarColumn(Length = 255)]
        public string? Sequence { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(Length = 255)]
        public string? Remarks { get; set; }
        /// <summary>
        /// 完整ID
        /// </summary>
        [SugarColumn(Length = 1000)]
        public string? FullId { get; set; }
    }
}
