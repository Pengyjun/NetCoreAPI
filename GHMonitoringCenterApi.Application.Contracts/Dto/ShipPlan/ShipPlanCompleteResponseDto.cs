using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.ShipPlan
{

    /// <summary>
    /// 自有船舶完成与计划对比响应DTO
    /// </summary>
    public class ShipPlanCompleteResponseDto
    {
        /// <summary>
        /// x轴
        /// </summary>
        public List<string> XAxis { get; set; }
        /// <summary>
        /// y轴完成产值
        /// </summary>
        public List<decimal> YAxisComplete { get; set; }
        /// <summary>
        /// Y轴计划产值
        /// </summary>
        public List<decimal> YAxisPlan { get; set; }
    }




    public class ShipEachPlanCompleteResponseDto
    { }
}
