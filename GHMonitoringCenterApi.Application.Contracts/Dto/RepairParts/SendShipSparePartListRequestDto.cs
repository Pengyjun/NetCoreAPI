using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.RepairParts
{
    /// <summary>
    /// 获取发船备件清单请求Dto
    /// </summary>
    public class SendShipSparePartListRequestDto : BaseRequestDto
    {
        /// <summary>
        /// 单据起始日期
        /// </summary>
        public DateTime? StartDocumentate { get; set; }
        /// <summary>
        /// 单据结束日期
        /// </summary>
        public DateTime? EndDocumentate { get; set; }
        /// <summary>
        /// 来源编号
        /// </summary>
        public string? SourceNumber { get; set; }
    }
}
