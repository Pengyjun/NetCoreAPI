using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.ProjectPlanProduction
{
    /// <summary>
    /// 项目月度维护计划excel导入
    /// </summary>
    public class ProjectPlanExcelImportResponseDto
    {
        public List<ProjectPlanInfo> projectPlanInfos { get; set; } = new List<ProjectPlanInfo>();
    }

    public class ProjectPlanInfo
    {
        /// <summary>
        /// 项目id
        /// </summary>
        public Guid ProjectId { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// 年度
        /// </summary>
        public int Year { get; set; }
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

}
