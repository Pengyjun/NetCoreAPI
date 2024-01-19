using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.ConstructionLog
{
    /// <summary>
    /// 获取施工日志详情请求Dto
    /// </summary>
    public class SearchConstructionLoDetailsgRequestDto
    {
        /// <summary>
        /// 项目Id
        /// </summary>
        public Guid? Id { get; set; }
        /// <summary>
        /// 填报时间
        /// </summary>
        public int? DateTime { get; set; }

    }
}
