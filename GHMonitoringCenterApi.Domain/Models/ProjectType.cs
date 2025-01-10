using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 项目类型
    /// </summary>
    [SugarTable("t_projecttype", IsDisabledDelete = true)]
    public class ProjectType:BaseEntity<Guid>
    {
        /// <summary>
        /// ID
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid PomId { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [SugarColumn(Length = 255)]
        public string? Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [SugarColumn(Length = 255)]
        public string? Name { get; set; }

        /// <summary>
        /// 业务说明
        /// </summary>
        [SugarColumn(Length = 255)]
        public string? BusinessRemark { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(Length = 500)]
        public string? Remarks { get; set; }
        /// <summary>
        /// 序列
        /// </summary>
        [SugarColumn(Length = 255)]
        public string? Sequence { get; set; }
        /// <summary>
        /// 单元
        /// </summary>
        [SugarColumn(Length = 255)]
        public string? Unit { get; set; }

    }
}
