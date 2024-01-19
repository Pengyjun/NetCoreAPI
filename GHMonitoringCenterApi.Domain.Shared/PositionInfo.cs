using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Shared
{

    /// <summary>
    /// 位置信息类
    /// </summary>
    public class PositionInfo
    {
        /// <summary>
        /// 省份
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 城市 市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 县区
        /// </summary>
        public string District { get; set; }
        /// <summary>
        /// 区域编码
        /// </summary>
        public string AdCode { get; set; }
    }
}
