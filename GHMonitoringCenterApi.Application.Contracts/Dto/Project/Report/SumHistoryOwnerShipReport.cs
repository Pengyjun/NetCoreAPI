using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report
{
    /// <summary>
    /// 统计自有船舶月报历史数据
    /// </summary>
    public class SumHistoryOwnerShipReport
    {
        /// <summary>
        /// 项目id
        /// </summary>
        public Guid ProjectId { get; set; }

        /// <summary>
        /// 自有船舶Id(PomId)
        /// </summary>
        public Guid ShipId { get; set; }

        /// <summary>
        /// 历史完成工程量差值（方）
        /// </summary>
        public decimal HistoryDiffProduction { get; set; }

        /// <summary>
        /// 历史完成产值差值（元）
        /// </summary>
        public decimal HistoryDiffProductionAmount { get; set; }
    }
}
