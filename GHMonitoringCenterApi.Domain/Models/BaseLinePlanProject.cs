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
        /// 计划版本呢
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
    }

    
}
