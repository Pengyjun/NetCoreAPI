using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.ProjectYearPlan
{
    public class ProjectPlanWbsResponseDto
    {
        public string Json { get; set; }


        public Statistics  Statistics { get; set; }
    }


    public class Statistics
    {
        /// <summary>
        /// 合计合同产值
        /// </summary>
        public decimal TotalContractAmount { get; set; }

        /// <summary>
        /// 年初累计产值合计
        /// </summary>
        public decimal TotalYearInitContractAmount { get; set; }


        /// <summary>
        /// 年初剩余合同额合计
        /// </summary>
        public decimal TotalResidueContractAmount { get; set; }

        /// <summary>
        /// 计划产值合计
        /// </summary>
        public decimal TotalPlanProductionValue { get; set; }



        ///合计相关字段
        /// 一月份合计产值
        /// </summary>
        public decimal TotalOneProductionValue { get; set; }
        /// <summary>
        /// 二月份合计产值
        /// </summary>
        public decimal TotalTwoProductionValue { get; set; }

        public decimal TotalThreeProductionValue { get; set; }
        public decimal TotalFourProductionValue { get; set; }
        public decimal TotalFiveProductionValue { get; set; }
        public decimal TotalSixProductionValue { get; set; }
        public decimal TotalSevenProductionValue { get; set; }
        public decimal TotalEightProductionValue { get; set; }
        public decimal TotalNineProductionValue { get; set; }
        public decimal TotalTenProductionValue { get; set; }
        public decimal TotalElevenProductionValue { get; set; }
        public decimal TotalTwelveProductionValue { get; set; }
    }
}
