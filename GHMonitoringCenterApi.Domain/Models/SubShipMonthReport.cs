using GHMonitoringCenterApi.Domain.Enums;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 分包船舶月报
    /// </summary>
    [SugarTable("t_subshipmonthreport", IsDisabledDelete = true)]
    [SugarIndex("INDEX_SSMR_UQ_KEY", nameof(ProjectId), OrderByType.Asc, nameof(ShipId), OrderByType.Asc, nameof(DateMonth), OrderByType.Asc, true)]
    public  class SubShipMonthReport : BaseEntity<Guid>
    {
        /// <summary>
        /// 项目Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid ProjectId { get; set; }

        /// <summary>
        /// 分包船舶Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid ShipId { get; set; }

        /// <summary>
        /// 进场时间
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime EnterTime { get; set; }

        /// <summary>
        /// 退场时间
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime QuitTime { get; set; }

        /// <summary>
        /// 填报月份（例：202304，注解：上月的26-本月的25，例：2023.3.26-2023.04.25）
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int DateMonth { get; set; }

        /// <summary>
        /// 填报年份
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int DateYear { get; set; }

        /// <summary>
        /// 运转（h）
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal WorkingHours { get; set; }

        /// <summary>
        /// 施工天数
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int ConstructionDays { get; set; }

        /// <summary>
        /// 产量（方）
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal Production { get; set; }

        /// <summary>
        /// 产值（元）
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal ProductionAmount { get; set; }

        /// <summary>
        /// 合同清单类型(字典表typeno=9)
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int ContractDetailType { get; set; }

        /// <summary>
        /// 当前动态描述(字典表typeno=10)
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int DynamicDescriptionType { get; set; }

        /// <summary>
        /// 下月计划工程量（方）
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal NextMonthPlanProduction { get; set; }

        /// <summary>
        /// 下月计划产值（元）
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal NextMonthPlanProductionAmount { get; set; }

        /// <summary>
        /// 本月计划工程量（方）
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal MonthPlanProduction { get; set; }

        /// <summary>
        /// 本月计划产值（元）
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal MonthPlanProductionAmount { get; set; }
    }
}
