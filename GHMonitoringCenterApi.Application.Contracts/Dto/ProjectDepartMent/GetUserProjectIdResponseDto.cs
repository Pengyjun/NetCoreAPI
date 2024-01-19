using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.ProjectDepartMent
{
    /// <summary>
    /// 根据用户获取项目Id
    /// </summary>
    public class GetUserProjectIdResponseDto
    {
        /// <summary>
        /// 项目Id
        /// </summary>
        public Guid ProjectId { get; set; }
    }
}
