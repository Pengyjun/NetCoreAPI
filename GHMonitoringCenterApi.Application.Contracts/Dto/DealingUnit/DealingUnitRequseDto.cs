using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.DealingUnit
{
    /// <summary>
    /// 模糊查询名称
    /// </summary>
    public class DealingUnitRequseDto
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string? Name { get; set; }
    }
}
