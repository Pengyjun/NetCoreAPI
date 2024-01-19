using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Timing
{
    /// <summary>
    /// 定时拉取港口数据响应DTO
    /// </summary>
    public class PomPortDataResponseDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public string? Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 简称
        /// </summary>
        public string? ShortName { get; set; }
        /// <summary>
        /// 港口编码
        /// </summary>
        public string? Code { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public decimal Longitude { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public decimal Latitude { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string? Remarks { get; set; }
    }
}
