using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Timing
{
    /// <summary>
    /// 疏浚吹填分类响应Dto
    /// </summary>
    public class PomShipWorkTypeResponseDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// PomId
        /// </summary>
        public Guid? PomId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string? Name { get; set; }
    }
}
