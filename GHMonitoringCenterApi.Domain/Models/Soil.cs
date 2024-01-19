using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 疏浚土分类
    /// </summary>
    [SugarTable("t_soil", IsDisabledDelete = true)]
    public class Soil : BaseEntity<Guid>
    {
        /// <summary>
        /// PomId
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? PomId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [SugarColumn(Length = 50)]
        public string? Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [SugarColumn(Length = 50)]
        public string? Code { get; set; }

        /// <summary>
        /// 顺序
        /// </summary>
        [SugarColumn(Length = 11)]
        public int? Sequence { get; set; }

        /// <summary>
        /// 附注
        /// </summary>
        [SugarColumn(Length = 500)]
        public string? Remarks { get; set; }

    }
}
