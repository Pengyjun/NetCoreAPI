using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Timing
{


    /// <summary>
    /// 项目区域响应Dto
    /// </summary>
    public class PomProjectAreaResponseDto
    {
        /// <summary>
        /// 区域Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public string Sequence { get; set; }
    }
}
