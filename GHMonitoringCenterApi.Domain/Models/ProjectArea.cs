using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{

    /// <summary>
    /// 项目区域表
    /// </summary>
    [SugarTable("t_projectarea", IsDisabledDelete = true)]
    public class ProjectArea:BaseEntity<Guid>
    {
        /// <summary>
        /// 项目状态ID
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid AreaId { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        [SugarColumn(Length = 255)]
        public string Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [SugarColumn(Length = 255)]
        public string Name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(Length = 1000)]
        public string Remarks { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [SugarColumn(Length = 20)]
        public string Sequence { get; set; }
    }
}
