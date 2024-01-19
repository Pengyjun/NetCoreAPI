using GHMonitoringCenterApi.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.ResourceManagement
{
    /// <summary>
    /// 获取船舶列表
    /// </summary>
    public class SearchShipTabulationResponseDto: BaseRequestDto
    {
        /// <summary>
        /// 类型（1：自有船舶，2：分包船舶 4：分包-船舶）
        /// </summary>
        public ConstructionOutPutType Type { get; set; }
    }
}
