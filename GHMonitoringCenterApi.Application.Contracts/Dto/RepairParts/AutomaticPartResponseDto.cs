using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.RepairParts
{
    /// <summary>
    /// 自动统计返回对象
    /// </summary>
    public class AutomaticPartResponseDto
    {
        /// <summary>
        /// 项目编号（入账年份）
        /// </summary>
        public string? ProjectNo { get; set; }


        /// <summary>
        /// 船舶名称
        /// </summary>
        public string? ShipName { get; set; }

        /// <summary>
        /// 已完未结金额
        /// </summary>
        public decimal? UnsettledAmount { get; set; }

        /// <summary>
        ///  已结未报金额
        /// </summary>
        public decimal? SettleAmount { get; set; }

        /// <summary>
        ///  报账金额
        /// </summary>
        public decimal? ReportedAmount { get; set; }

        /// <summary>
        /// 出库金额
        /// </summary>
        public decimal? OutboundAmount { get; set; }

        /// <summary>
        /// 运输费
        /// </summary>
        public decimal? TransportationAmount { get; set; }

    }
}
