using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Institution
{


    /// <summary>
    /// 机构组织树请求DTO
    /// </summary>
    public class InstitutionTreeRequestDto
    {
        /// <summary>
        /// 机构组织吗
        /// </summary>
        public string? Poid { get; set; }
    }
}
