using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.CompanyProductionValueInfos
{
    /// <summary>
    /// 公司产值计划调整值
    /// </summary>
    public class AddORUpdateCompanyAdjustmentValueRequestDto
    {
        ///// <summary>
        ///// 
        ///// </summary>
        //public Guid? Id { get; set; }

        /// <summary>
        /// 计划Id
        /// </summary>
        public Guid? PlanId { get; set; }

        /// <summary>
        /// 月份
        /// </summary>
        public int? Month { get; set; }

        /// <summary>
        /// 调整值
        /// </summary>
        public decimal? Adjustmentvalue { get; set; }
    }
}
