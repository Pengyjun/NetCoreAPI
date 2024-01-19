using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.SystemUpdatLog
{
    /// <summary>
    /// 系统日志已读标识返回Dto
    /// </summary>
    public class SearchLogReadSignReponseDto
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public Guid? Id { get; set; }
        /// <summary>
        /// 系统日志内容
        /// </summary>
        public string? LogContent { get; set; }
        /// <summary>
        /// 系统日志内容数组
        /// </summary>
        public string[]? LogContentList { get; set; }
        /// <summary>
        /// 判定是否显示系统日志   true 显示   false 不显示
        /// </summary>
        public bool? Determinar { get; set; }
    }
}
