using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Character
{
    /// <summary>
    /// 请求字符串Dto
    /// </summary>
    public class CharacterRequsetDto
    {
        /// <summary>
        /// 请求字符串
        /// </summary>
        public string? keyWords { get; set; }
    }
}
