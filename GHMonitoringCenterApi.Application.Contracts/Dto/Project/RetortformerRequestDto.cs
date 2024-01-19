using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project
{
    public class RetortformerRequestDto
    {
        /// <summary>
        /// 项目Id
        /// </summary>
        public Guid ProjectId { set; get; }

        /// <summary>
        /// 报表负责人
        /// </summary>
        public string? Reportformer { get; set; }
        /// <summary>
        /// 报表负责人联系方式
        /// </summary>
        public string? ReportformerTel { get; set; }
    }
}
