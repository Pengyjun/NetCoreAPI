using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Timing
{
    /// <summary>
    /// 币种响应数据DTO
    /// </summary>
    public class PomCurrencyResponseDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 货币数字代码
        /// </summary>
        public string? Zcurrencycode { get; set; }
        /// <summary>
        /// 货币名称
        /// </summary>
        public string? Zcurrencyname { get; set; }
        /// <summary>
        /// 货币字母代码
        /// </summary>
        public string? Zcurrencyalphabet { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string? Zremarks { get; set; }
       
    }
}
