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
        public List<string> XAxis { get; set; } = new List<string>();
        /// <summary>
        /// y轴完成产值
        /// </summary>
        public List<decimal> YAxisComplete { get; set; } = new List<decimal>();
        /// <summary>
        /// Y轴计划产值
        /// </summary>
        public List<decimal> YAxisPlan { get; set; } = new List<decimal>();
    }

    public class ShipEachPlanCompleteResponseDto
    {

        /// <summary>
        /// x轴
        /// </summary>
        public List<string> XAxis { get; set; }=new List<string>();
        /// <summary>
        /// y轴完成产值
        /// </summary>
        public List<decimal> YAxisPxComplete { get; set; } = new List<decimal>();
        /// <summary>
        /// Y轴计划产值
        /// </summary>
        public List<decimal> YAxisPxPlan { get; set; } = new List<decimal>();




        /// <summary>
        /// y轴完成产值
        /// </summary>
        public List<decimal> YAxisJxComplete { get; set; } = new List<decimal>();
        /// <summary>
        /// Y轴计划产值
        /// </summary>
        public List<decimal> YAxisJxPlan { get; set; } = new List<decimal>();



        /// <summary>
        /// y轴完成产值
        /// </summary>
        public List<decimal> YAxisZdComplete { get; set; } = new List<decimal>();
        /// <summary>
        /// Y轴计划产值
        /// </summary>
        public List<decimal> YAxisZdPlan { get; set; } = new List<decimal>();


        /// <summary>
        /// y轴完成产值
        /// </summary>
        public List<decimal> YAxisComplete { get; set; } = new List<decimal>();
        /// <summary>
        /// Y轴计划产值
        /// </summary>
        public List<decimal> YAxisPlan { get; set; } = new List<decimal>();

    }
}
