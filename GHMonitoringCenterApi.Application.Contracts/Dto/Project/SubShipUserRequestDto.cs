using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project
{
    /// <summary>
    /// 分包船舶列表请求
    /// </summary>
    public class SubShipUserRequestDto : BaseRequestDto
    {
        /// <summary>
        /// 船舶名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public Guid? TypeId { get; set; }
    }
}
