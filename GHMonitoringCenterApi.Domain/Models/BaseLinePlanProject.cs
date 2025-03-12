using SqlSugar;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 项目基准计划
    /// </summary>
    [SugarTable("t_baselineplanproject", IsDisabledDelete = true)]
    public class BaseLinePlanProject : BaseEntity<Guid>
    {
        /// <summary>
        /// 年份
        /// </summary>
        [SugarColumn(ColumnDataType = "int", DefaultValue = "0")]
        public int Year { get; set; }

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

        /// <summary>
        /// 计划名称
        /// </summary>
        [SugarColumn(Length = 36)]
        public string PlanVersion { get; set; }

        /// <summary>
        /// 基准或新建计划
        /// </summary>
        [SugarColumn(Length = 36)]
        public string PlanType { get; set; }

        /// <summary>
        /// 年初状态
        /// </summary>
        [SugarColumn(Length = 36)]
        public string StartStatus { get; set; }

        /// <summary>
        /// 简称
        /// </summary>
        [SugarColumn(Length = 36)]
        public string ShortName { get; set; }

        /// <summary>
        /// 是否分包
        /// </summary>
        [SugarColumn(Length = 36)]
        public string IsSubPackage { get; set; }

        /// <summary>
        /// 总有效合同额
        /// </summary>
        public decimal EffectiveAmount { get; set; }

        /// <summary>
        /// 剩余合同额
        /// </summary>
        public decimal RemainingAmount { get; set; }

        /// <summary>
        /// 最新完工时间
        /// </summary>
        public DateTime? CompletionTime { get; set; }


        /// <summary>
        /// 计划状态
        /// </summary>
        public int PlanStatus { get; set; }

        /// <summary>
        /// 关联项目
        /// </summary>
        public string? Association { get; set; }

    }


}
