using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 船级社
    /// </summary>
    [SugarTable("t_shipclassic", IsDisabledDelete = true)]
    public class ShipClassic : BaseEntity<Guid>
    {
        /// <summary>
        /// pomId
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid PomId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? Name { get; set; }
        /// <summary>
        /// 国别
        /// </summary>
        [SugarColumn(Length = 100)]
        public string Country { get; set; }
        public string Contact { get; set; }
        public string Tel { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }

        public string Address { get; set; }
        public string Post { get; set; }

        [SugarColumn(ColumnDataType = "int")]
        public int Sequence { get; set; }
        public string Remarks { get; set; }
    }
}
