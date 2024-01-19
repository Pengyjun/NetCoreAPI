using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.PersonnelOrganizeAdjust
{
    /// <summary>
    /// 获取已添加人员响应Dto
    /// </summary>
    public class SearchAddedPersonnelReponseDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>   
        public string? Name { get; set; }
        /// <summary>
        /// 公司ID
        /// </summary>
        public Guid? CompanyId { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string? CompanyName { get; set; }
        /// <summary>
        /// 部门ID
        /// </summary>
        public Guid? DepartmentId { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string? DepartmentName { get; set; }
        /// <summary>
        /// 二级单位名称
        /// </summary>
        public string? SCompanyName { get; set; }
    }
}
