using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.ProjectDepartMent
{
    /// <summary>
    /// 获取产值请求
    /// </summary>
    public class OutputValueRequestDto
    {
        /// <summary>
        /// 项目Id
        /// </summary>
        public Guid? ProjectId { get; set; }
    }
}
