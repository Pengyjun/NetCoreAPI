using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.PersonnelOrganizeAdjust
{
    /// <summary>
    /// 获取人员授权机构响应Dto
    /// </summary>
    public class AuthorizedInstitutionsReponseDto
    {
        /// <summary>
        /// 二级单位名称
        /// </summary>
        public Guid? SCompanyId { get; set; }
        /// <summary>
        /// 二级单位名称
        /// </summary>
        public string? SCompanyName { get; set; }
        /// <summary>
        /// 部门Id
        /// </summary>
        public Guid DivisionId { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string? DivisionName { get; set; }
    }
}
