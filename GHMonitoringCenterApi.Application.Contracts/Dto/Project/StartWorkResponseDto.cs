using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project
{


    /// <summary>
    /// 开停工时间
    /// </summary>
    public class StartWorkResponseDto
    {
        /// <summary>
        /// 项目ID
        /// </summary>
        public Guid? ProjectId { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string? Name { get; set; }
        /// 开工时间
        /// </summary>
        public DateTime? StartWorkTime { get; set; }
        /// <summary>
        /// 停工时间
        /// </summary>
        public DateTime? EndWorkTime { get; set; }
    }
}
