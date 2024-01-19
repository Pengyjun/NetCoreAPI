using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Shared
{
    /// <summary>
    /// 实体字段备注
    /// </summary>
    public class EntityFieldRemark
    {
        /// <summary>
        /// 字段
        /// </summary>
        public string Field { get; set; }
        /// <summary>
        /// 字段值
        /// </summary>
        public string Name { get; set; }
    }
}
