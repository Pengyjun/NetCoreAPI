using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 项目干系单位信息表
    /// </summary>
    [SugarTable("t_projectorg",IsDisabledDelete =true)]
    public class ProjectOrg:BaseEntity<Guid>
    {
        /// <summary>
        /// pomId
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? PomId { get; set; }
        /// <summary>
        /// 项目Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? ProjectId { get; set; }
        /// <summary>
        /// 单位Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? OrganizationId { get; set; }
        /// <summary>
        /// 单位类型
        /// </summary>
        [SugarColumn(Length = 20)]
        public string? Type { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(Length =500)]
        public string? Remarks { get; set; }
    }
}
