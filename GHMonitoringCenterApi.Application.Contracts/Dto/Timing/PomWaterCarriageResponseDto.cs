using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Timing
{
    /// <summary>
    /// 水运工况级别
    /// </summary>
    public class PomWaterCarriageResponseDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 等级
        /// </summary>
        public string? Grade { get; set; }
        /// <summary>
        /// 工况
        /// </summary>
        public string? Remarks { get; set; }


    }
}
