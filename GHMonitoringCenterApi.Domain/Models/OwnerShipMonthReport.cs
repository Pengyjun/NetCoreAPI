using SqlSugar;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 自有船舶月报
    /// </summary>
    [SugarTable("t_ownershipmonthreport", IsDisabledDelete = true)]
    [SugarIndex("INDEX_OSMR_UQ_KEY", nameof(ProjectId), OrderByType.Asc, nameof(ShipId), OrderByType.Asc, nameof(DateMonth), OrderByType.Asc, true)]
    public  class OwnerShipMonthReport : BaseEntity<Guid>
    {
        /// <summary>
        /// 项目Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid ProjectId { get; set; }

        /// <summary>
        /// 自有船舶Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid ShipId { get; set; }

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
        /// 运转（h）
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(8,2)")]
        public decimal WorkingHours { get; set; }

        /// <summary>
        /// 施工天数
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(8,2)")]
        public decimal ConstructionDays { get; set; }

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
        /// 工艺方式Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid  WorkModeId { get; set; }

        /// <summary>
        /// 疏浚吹填分类Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid WorkTypeId { get; set; }

        /// <summary>
        /// 工况级别Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid ConditionGradeId { get; set; }

        /// <summary>
        ///挖深（m）
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal DigDeep { get; set; }

        /// <summary>
        /// 吹距(KM)
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal BlowingDistance { get; set; }

        /// <summary>
        /// 运距(KM)
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal HaulDistance { get; set; }

    }
}
