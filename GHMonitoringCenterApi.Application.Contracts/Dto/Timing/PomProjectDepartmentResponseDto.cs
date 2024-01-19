using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Timing
{
    /// <summary>
    /// 项目部
    /// </summary>
    public class PomProjectDepartmentResponseDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid? Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 二级公司ID
        /// </summary>
        public Guid? SCompanyId { get; set; }
        /// <summary>
        /// 二级公司名称
        /// </summary>
        public string? SCompanyName { get; set; }
        /// <summary>
        /// 三级公司ID
        /// </summary>
        public Guid? TCompanyId { get; set; }
        /// <summary>
        /// 三级公司名称
        /// </summary>
        public string? TCompanyName { get; set; }
        /// <summary>
        /// 所属公司Id
        /// </summary>
        public Guid? CompanyId { get; set; }


    }
}
