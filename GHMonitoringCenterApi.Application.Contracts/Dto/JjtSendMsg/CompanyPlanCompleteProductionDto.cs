using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.JjtSendMsg
{


    /// <summary>
    /// 各个公司的计划和完成产值
    /// </summary>
    public class CompanyPlanCompleteProductionDto
    {
        /// <summary>
        /// 公司ID
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 计划产值
        /// </summary>
        public decimal PlanProductionValue { get; set; }
        /// <summary>
        /// 完成产值
        /// </summary>
        public decimal CompleteProductionValue { get; set; }
    }
}
