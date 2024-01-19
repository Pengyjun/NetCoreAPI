using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 属性标签
    /// </summary>
    [SugarTable("t_attributelabelc", IsDisabledDelete = true)]
    public class AttributeLabelc : BaseEntity<Guid>
    {
        /// <summary>
        /// ID
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid Id { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? Number { get; set; }
        /// <summary>
        /// 属性
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? Attribute { get; set; }
    }
}
