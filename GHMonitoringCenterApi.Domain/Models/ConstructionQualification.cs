using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 项目施工资质
    /// </summary>
    [SugarTable("t_constructionqualification", IsDisabledDelete = true)]
    public class ConstructionQualification : BaseEntity<Guid>
    {
        /// <summary>
        /// id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid PomId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [SugarColumn(Length = 255)]
        public string Name { get; set; }
    }
}
