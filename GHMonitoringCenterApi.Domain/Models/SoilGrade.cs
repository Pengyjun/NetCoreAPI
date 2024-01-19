using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 疏浚岩土分级
    /// </summary>
    [SugarTable("t_soilgrade", IsDisabledDelete = true)]
    public class SoilGrade : BaseEntity<Guid>
    {

        /// <summary>
        /// PomId
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? PomId { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        [SugarColumn(Length = 50)]
        public string? Grade { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [SugarColumn(Length = 255)]
        public string? StatusDescription { get; set; }

        /// <summary>
        /// 顺序
        /// </summary>
        [SugarColumn(Length = 11)]
        public int? Sequence { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(Length = 500)]
        public string? Remarks { get; set; }
    }
}
