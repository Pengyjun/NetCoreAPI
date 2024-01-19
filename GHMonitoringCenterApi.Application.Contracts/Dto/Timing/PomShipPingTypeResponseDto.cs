using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Timing
{
    /// <summary>
    /// 定时拉取船舶类型响应DTO
    /// </summary>
    public class PomShipPingTypeResponseDto
    {

        /// <summary>
        /// PomId
        /// </summary>
        public Guid? Id { get; set; }
        /// <summary>
        /// 父Id
        /// </summary>
        public Guid? ParentId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        public string? Code { get; set; }
        /// <summary>
        /// 序列
        /// </summary>
        public int? Sequence { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string? Remarks { get; set; }
    }
}
