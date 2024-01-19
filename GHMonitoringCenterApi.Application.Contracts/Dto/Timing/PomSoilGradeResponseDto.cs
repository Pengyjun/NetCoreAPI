using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Timing
{
    /// <summary>
    /// 疏浚土分级响应土分级
    /// </summary>
    public class PomSoilGradeResponseDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// PomId
        /// </summary>
        public Guid? PomId { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        public string? Grade { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string? StatusDescription { get; set; }

        /// <summary>
        /// 顺序
        /// </summary>
        public int? Sequence { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remarks { get; set; }
    }
}
