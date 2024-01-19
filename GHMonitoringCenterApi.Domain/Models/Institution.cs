using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{

    /// <summary>
    /// 机构数据表 
    /// </summary>
    [SugarTable("t_institution", IsDisabledDelete = true)]
    public class Institution:BaseEntity<Guid>
    {
        [SugarColumn(Length = 36)]
        public Guid? PomId { get; set; }
        [SugarColumn(Length = 30)]
        public string? Oid { get; set; }
        [SugarColumn(Length = 30)]
        public string? Poid { get; set; }
        [SugarColumn(Length = 200)]
        public string? Gpoid { get; set; }
        [SugarColumn(Length = 100)]
        public string? Ocode { get; set; }
        [SugarColumn(Length = 200)]
        public string? Name { get; set; }
        [SugarColumn(Length = 200)]
        public string? Shortname { get; set; }
        [SugarColumn(Length = 20)]
        public string? Status { get; set; }
        [SugarColumn(Length = 20)]
        public string? Sno { get; set; }
        [SugarColumn(Length = 1000)]
        public string? Orule { get; set; }
        [SugarColumn(Length = 1000)]
        public string? Grule { get; set; }
        [SugarColumn(Length = 100)]
        public string? Grade { get; set; }
    }
}
