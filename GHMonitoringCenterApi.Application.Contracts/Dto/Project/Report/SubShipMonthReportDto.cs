using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report
{
    /// <summary>
    /// 分包船舶月报Dto
    /// </summary>
    public abstract  class SubShipMonthReportDto
    {
        /// <summary>
        /// 项目id
        /// </summary>
        public Guid ProjectId { get; set; }

        /// <summary>
        /// 分包船舶Id
        /// </summary>
        public Guid ShipId { get; set; }

        /// <summary>
        /// 进场时间
        /// </summary>
        public DateTime? EnterTime { get; set; }
       
        /// <summary>
        /// 退场时间
        /// </summary>
        public DateTime? QuitTime { get; set; }

        /// <summary>
        /// 填报月份（例：202304，注解：上月的26-本月的25，例：2023.3.26-2023.04.25）
        /// </summary>
        public int DateMonth { get; set; }

        /// <summary>
        /// 产量（方）
        /// </summary>
        public decimal? Production { get; set; }

        /// <summary>
        /// 产值（元）
        /// </summary>
        public decimal? ProductionAmount { get; set; }

        /// <summary>
        /// 运转（h）
        /// </summary>
        public decimal? WorkingHours { get; set; }

        /// <summary>
        /// 施工天数
        /// </summary>
        public int? ConstructionDays { get; set; }

        /// <summary>
        /// 合同清单类型(字典表typeno=9)
        /// </summary>
        public int? ContractDetailType { get; set; }

        /// <summary>
        /// 当前动态描述(字典表typeno=10)
        /// </summary>
        public int? DynamicDescriptionType { get; set; }

        /// <summary>
        /// 下月计划工程量（方）
        /// </summary>
        public decimal? NextMonthPlanProduction { get; set; }

        /// <summary>
        /// 下月计划产值（元）
        /// </summary>
        public decimal? NextMonthPlanProductionAmount { get; set; }
    }
}
