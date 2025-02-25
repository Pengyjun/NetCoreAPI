using SqlSugar;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 项目年初计划产值 产量
    /// </summary>
    [SugarTable("t_projectannualproduction", IsDisabledDelete = true)]
    public class ProjectAnnualProduction : BaseEntity<Guid>
    {
        /// <summary>
        /// 1月产量
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal JanuaryProductionQuantity { get; set; }

        /// <summary>
        /// 1月产值
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal JanuaryProductionValue { get; set; }

        /// <summary>
        /// 2月产量
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal FebruaryProductionQuantity { get; set; }

        /// <summary>
        /// 2月产值
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal FebruaryProductionValue { get; set; }

        /// <summary>
        /// 3月产量
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal MarchProductionQuantity { get; set; }

        /// <summary>
        /// 3月产值
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal MarchProductionValue { get; set; }

        /// <summary>
        /// 4月产量
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal AprilProductionQuantity { get; set; }

        /// <summary>
        /// 4月产值
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal AprilProductionValue { get; set; }

        /// <summary>
        /// 5月产量
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal MayProductionQuantity { get; set; }

        /// <summary>
        /// 5月产值
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal MayProductionValue { get; set; }

        /// <summary>
        /// 6月产量
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal JuneProductionQuantity { get; set; }

        /// <summary>
        /// 6月产值
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal JuneProductionValue { get; set; }

        /// <summary>
        /// 7月产量
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal JulyProductionQuantity { get; set; }

        /// <summary>
        /// 7月产值
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal JulyProductionValue { get; set; }

        /// <summary>
        /// 8月产量
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal AugustProductionQuantity { get; set; }

        /// <summary>
        /// 8月产值
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal AugustProductionValue { get; set; }

        /// <summary>
        /// 9月产量
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal SeptemberProductionQuantity { get; set; }

        /// <summary>
        /// 9月产值
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal SeptemberProductionValue { get; set; }

        /// <summary>
        /// 10月产量
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal OctoberProductionQuantity { get; set; }

        /// <summary>
        /// 10月产值
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal OctoberProductionValue { get; set; }

        /// <summary>
        /// 11月产量
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal NovemberProductionQuantity { get; set; }

        /// <summary>
        /// 11月产值
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal NovemberProductionValue { get; set; }

        /// <summary>
        /// 12月产量
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal DecemberProductionQuantity { get; set; }

        /// <summary>
        /// 12月产值
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal DecemberProductionValue { get; set; }

        /// <summary>
        /// 年份
        /// </summary>
        [SugarColumn(ColumnDataType = "int", DefaultValue = "0")]
        public int Year { get; set; }

        ///// <summary>
        ///// 平均日产值 =当月产值/月天数  先不用
        ///// </summary>
        //[SugarColumn(ColumnDataType = "decimal(20,8)")]
        //public decimal DayProductionValue { get; set; }

        ///// <summary>
        ///// 当月多少天 先不用
        ///// </summary>
        //[SugarColumn(ColumnDataType = "int", DefaultValue = "0")]
        //public int Days { get; set; }

        /// <summary>
        /// 资源id(船舶)
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? ShipId { get; set; }

        /// <summary>
        /// 项目id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? ProjectId { get; set; }

        /// <summary>
        /// 所属公司
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid CompanyId { get; set; }

        ///// <summary>
        ///// 排序
        ///// </summary>
        //[SugarColumn(ColumnDataType = "int", DefaultValue = "0")]
        //public int Sequence { get; set; }
    }
}
