using GHMonitoringCenterApi.Domain.Shared.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.ProjectAdjustMonthReport
{

    /// <summary>
    /// 调整开累数响应DTO
    /// </summary>
    public class ProjectAdjustResponseDto
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 月份
        /// </summary>
        public int DateMonth { get; set; }
        /// <summary>
        /// 币种
        /// </summary>
        public string Current { get; set; }

        public decimal ExchangeRate { get; set; }
        //public decimal? TaxRate { get; set; }

        public List<ProjectAdjustProductionValueResponseDto> projectAdjustProductionValueResponseDtos { get; set; }

    }
}
