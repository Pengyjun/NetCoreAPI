using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.PersonnelOrganizeAdjust
{
    /// <summary>
    /// 人员组织调整获取所属公司下拉接请求应Dto
    /// </summary>
    public class GetCompanyPullDownAuthAsyncRequestDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string? Name { get; set; }


    }
}
