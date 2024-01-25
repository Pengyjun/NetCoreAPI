using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.RepairParts
{
    /// <summary>
    /// 自动统计列表返回对象
    /// </summary>
    public class AutomaticPartsResponseDto
    {
        /// <summary>
        /// 明细列表
        /// </summary>
        public List<AutomaticPartResponseDto> List { get; set; } = default!;


        /// <summary>
        /// 运输费
        /// </summary>
        public decimal? TransportationAmount { get; set; }
    }
}
