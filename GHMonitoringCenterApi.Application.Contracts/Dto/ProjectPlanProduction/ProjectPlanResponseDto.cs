using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.ProjectPlanProduction
{

    /// <summary>
    /// 项目计划响应DTO
    /// </summary>
    public class ProjectPlanResponseDto
    {
        public List<ProjectPlanSearchInfo> projectSearchInfos { get; set; } = new List<ProjectPlanSearchInfo>();
        public SumProjectPlanInfo sumProjectPlanInfo { get; set; }
    }

    public class ProjectPlanSearchInfo
    {
        /// <summary>
        /// 项目id
        /// </summary>
        public Guid ProjectId { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 年份
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// 公司ID
        /// </summary>
        public Guid CompanyId { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 第一个月的计划产值   后面字段以此类型
        /// </summary>
        public decimal OnePlanProductionValue { get; set; }
        public decimal TwoPlanProductionValue { get; set; }
        public decimal ThreePlanProductionValue { get; set; }
        public decimal FourPlanProductionValue { get; set; }
        public decimal FivePlanProductionValue { get; set; }
        public decimal SixPlanProductionValue { get; set; }
        public decimal SevenPlanProductionValue { get; set; }
        public decimal EightPlanProductionValue { get; set; }
        public decimal NinePlanProductionValue { get; set; }
        public decimal TenPlanProductionValue { get; set; }
        public decimal ElevenPlanProductionValue { get; set; }
        public decimal TwelvePlanProductionValue { get; set; }
    }

    public class SumProjectPlanInfo
    {
        /// <summary>
        /// 第一个月的计划产值   后面字段以此类型
        /// </summary>
        public decimal SumOnePlanProductionValue { get; set; }
        public decimal SumTwoPlanProductionValue { get; set; }
        public decimal SumThreePlanProductionValue { get; set; }
        public decimal SumFourPlanProductionValue { get; set; }
        public decimal SumFivePlanProductionValue { get; set; }
        public decimal SumSixPlanProductionValue { get; set; }
        public decimal SumSevenPlanProductionValue { get; set; }
        public decimal SumEightPlanProductionValue { get; set; }
        public decimal SumNinePlanProductionValue { get; set; }
        public decimal SumTenPlanProductionValue { get; set; }
        public decimal SumElevenPlanProductionValue { get; set; }
        public decimal SumTwelvePlanProductionValue { get; set; }
    }
}
