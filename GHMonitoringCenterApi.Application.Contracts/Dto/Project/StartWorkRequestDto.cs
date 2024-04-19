using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project
{
    public class StartWorkRequestDto:BaseRequestDto
    {
        /// <summary>
        /// 项目ID
        /// </summary>
        public Guid Id { get; set; }
    }
}
