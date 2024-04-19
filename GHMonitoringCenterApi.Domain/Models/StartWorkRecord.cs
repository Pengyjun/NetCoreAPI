using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{

    /// <summary>
    /// 开停工记录表
    /// </summary>
    [SugarTable("t_startworkrecord", IsDisabledDelete = true)]
    public class StartWorkRecord:BaseEntity<Guid>
    {
        /// <summary>
        /// 项目ID
        /// </summary>
        [SugarColumn(Length = 50)]
        public Guid? ProjectId { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        [SugarColumn(Length = 200)]
        public string? Name { get; set; }
        /// <summary>
        /// 公司ID
        /// </summary>
        [SugarColumn(Length = 50)]
        public Guid? CompanyId { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        [SugarColumn(Length = 200)]
        public string? CompanyName { get; set; }
        /// <summary>
        /// 开工时间
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime? StartWorkTime { get; set; }
        /// <summary>
        /// 停工时间
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime? EndWorkTime { get; set; }
    }
}
