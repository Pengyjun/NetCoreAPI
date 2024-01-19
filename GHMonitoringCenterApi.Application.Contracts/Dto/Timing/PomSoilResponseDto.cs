using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Timing
{
    /// <summary>
    /// 疏浚岩土信息响应dto
    /// </summary>
    public class PomSoilResponseDto
    {
        /// <summary>
        /// 主建Id
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

        /// <summary>
        /// 编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 顺序
        /// </summary>
        public int? Sequence { get; set; }

        /// <summary>
        /// 附注
        /// </summary>
        public string? Remarks { get; set; }
    }
}
