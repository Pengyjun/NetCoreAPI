using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Timing
{
    /// <summary>
    /// 省市区乡
    /// </summary>
    public class PomProvinceResponseDto
    {
        /// <summary>
        /// ID
        /// </summary>
        public Guid? Id { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string? Zaddvsup { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string? Zaddvscode { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string? Zaddvsname { get; set; }
        /// <summary>
        /// 国外
        /// </summary>
        public string? Overseas { get; set; }
        /// <summary>
        /// 地区Id
        /// </summary>
        public Guid? RegionId { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int? Zaddvslevel { get; set; }
    }
}
