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
    public class StartWorkRecord : BaseEntity<Guid>
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
        /// 开工时间
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime? StartWorkTime { get; set; }
        /// <summary>
        /// 停工时间
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime? EndWorkTime { get; set; }
        /// <summary>
        /// 停工原因
        /// </summary>
        [SugarColumn(Length = 1000)]
        public string? StopWorkReson { get; set; }
        /// <summary>
        /// 修改前状态
        /// </summary>
        [SugarColumn(Length = 200)]
        public string? BeforeStatus { get; set; }
        /// <summary>
        /// 修改后状态
        /// </summary>
        [SugarColumn(Length = 200)]
        public string? AfterStatus { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        [SugarColumn(Length = 200)]
        public string? UpdateUser { get; set; }


    }
}
