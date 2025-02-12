using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 项目管理类型
    /// </summary>
    public class ProjectMangerType:BaseEntity<Guid>
    {
        /// <summary>
        /// 编码Code值
        /// </summary>
        [SugarColumn(Length = 256)]
        public string? Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [SugarColumn(Length = 256)]
        public string? Name { get; set; }

        /// <summary>
        /// 业务说明
        /// </summary>
        [SugarColumn(Length = 256)]
        public string? BusinessRemark { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(Length = 512)]
        public string? Remarks { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int? Sort { get; set; }
    
    }

}
