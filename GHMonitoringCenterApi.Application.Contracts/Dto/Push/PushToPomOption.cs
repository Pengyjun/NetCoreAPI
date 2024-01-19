using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Push
{
    /// <summary>
    /// 推送到Pom的配置项
    /// </summary>
    public  class PushToPomOption
    {
        /// <summary>
        /// 推送地址
        /// </summary>
        public string? Url { get; set; }

        /// <summary>
        /// 密钥
        /// </summary>
        public string? Token { get; set; }

        /// <summary>
        /// 是否开启
        /// </summary>
        public string? IsOpen { get; set; }

        /// <summary>
        /// 单次推送数量
        /// </summary>
        public int SinglePushNum { get; set; } = 200;
    }
}
