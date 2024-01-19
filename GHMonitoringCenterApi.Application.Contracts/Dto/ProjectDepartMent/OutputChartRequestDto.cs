using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.ProjectDepartMent
{
    /// <summary>
    /// 项目部产值图表请求Dto
    /// </summary>
    public class OutputChartRequestDto 
    {

        /// <summary>
        /// 项目Id
        /// </summary>
        public Guid? ProjectId { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

    }
}
