using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.RepairParts
{
    /// <summary>
    /// 统计发船备件清单
    /// </summary>
    public class SumSendShipSparePartList
    {
        /// <summary>
        /// 提交财务时间年份
        /// </summary>
        public string? SubmitFinanceYear { get; set; }

        /// <summary>
        /// 领料单位
        /// </summary>
        public string? MaterialRequisitionUnit { get; set; }

        /// <summary>
        /// 出库金额
        /// </summary>
        public decimal? OutboundAmount { get; set; }
    }
}
