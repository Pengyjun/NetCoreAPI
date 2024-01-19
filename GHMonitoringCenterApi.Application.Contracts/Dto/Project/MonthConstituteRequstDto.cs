using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project
{
    /// <summary>
    /// 月报构成请求Dto
    /// </summary>
    public class MonthConstituteRequstDto
    {
        /// <summary>
        /// 项目Id
        /// </summary>
        public Guid? ProjectId { get; set; }

        ///// <summary>
        ///// 项目Id
        ///// </summary>
        //public string? ProjectId { get; set; }
    }
}
