using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.CompanyProductionValueInfos
{
    /// <summary>
    /// 公司产值信息请求Dto
    /// </summary>
    public class CompanyProductionValueInfoRequsetDto : BaseRequestDto
    {
        /// <summary>
        /// 年份查询
        /// </summary>
        public int? Dateday { get; set; }
    }
}
