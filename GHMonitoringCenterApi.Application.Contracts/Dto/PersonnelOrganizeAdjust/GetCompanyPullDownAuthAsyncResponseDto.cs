using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.PersonnelOrganizeAdjust
{
    /// <summary>
    /// 人员组织调整获取所属公司下拉接响应Dto
    /// </summary>
    public class GetCompanyPullDownAuthAsyncResponseDto
    {
        /// <summary>
        /// 部门Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
       public string? Name { get; set; }
    }
}
