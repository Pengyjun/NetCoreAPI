using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.PersonnelOrganizeAdjust
{
    /// <summary>
    /// 获取已添加人员请求Dto
    /// </summary>
    public class SearchAddedPersonnelRequestDto: BaseRequestDto
    {
        /// <summary>
        /// 下级单位
        /// </summary>
        public Guid? SecondaryUnit { get; set; }
    }
}







