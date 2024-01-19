using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.ProjectPlanProduction
{
    /// <summary>
    /// 项目年度计划详情
    /// </summary>
    public class ProjectPlanDetailResponseDto
    {
        /// <summary>
        /// 项目id
        /// </summary>
        public Guid ProjectId { get; set; }
        /// <summary>
        /// 年度
        /// </summary>
        public List<ProjectYearInfo> projectYearInfos { get; set; } = new List<ProjectYearInfo>();
        /// <summary>
        /// 月度
        /// </summary>
        public ProjectMonthInfo projectMonthInfos { get; set; } = new ProjectMonthInfo();
    }
    /// <summary>
    /// 年度数据
    /// </summary>
    public class ProjectYearInfo
    {
        /// <summary>
        /// 年度
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// 年度计划产值
        /// </summary>
        public decimal? YearPlanActual { get; set; }
    }
    /// <summary>
    /// 月度数据
    /// </summary>
    public class ProjectMonthInfo
    {
        /// <summary>
        /// 年度
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// 一月
        /// </summary>
        public decimal? OnePlanProductionValue { get; set; }
        /// <summary>
        /// 二月
        /// </summary>
        public decimal? TwoPlanProductionValue { get; set; }
        /// <summary>
        /// 三月
        /// </summary>
        public decimal? ThreePlanProductionValue { get; set; }
        /// <summary>
        /// 四月
        /// </summary>
        public decimal? FourPlanProductionValue { get; set; }
        /// <summary>
        /// 五月
        /// </summary>
        public decimal? FivePlanProductionValue { get; set; }
        /// <summary>
        /// 六月
        /// </summary>
        public decimal? SixPlanProductionValue { get; set; }
        /// <summary>
        /// 七月
        /// </summary>
        public decimal? SevenPlanProductionValue { get; set; }
        /// <summary>
        /// 八月
        /// </summary>
        public decimal? EightPlanProductionValue { get; set; }
        /// <summary>
        /// 九月
        /// </summary>
        public decimal? NinePlanProductionValue { get; set; }
        /// <summary>
        /// 十月
        /// </summary>
        public decimal? TenPlanProductionValue { get; set; }
        /// <summary>
        /// 十一月
        /// </summary>
        public decimal? ElevenPlanProductionValue { get; set; }
        /// <summary>
        /// 十二月
        /// </summary>
        public decimal? TwelvePlanProductionValue { get; set; }
    }


}
