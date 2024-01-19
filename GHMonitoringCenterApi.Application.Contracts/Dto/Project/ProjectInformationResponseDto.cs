using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project
{
    /// <summary>
    /// 获取当前用户部门下项目信息相应Dto
    /// </summary>
    public class ProjectInformationResponseDto
    {
        /// <summary>
        /// 项目Id
        /// </summary>
        public Guid? Id { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 项目状态
        /// </summary>
        public string? Status { get; set; }
        /// <summary>
        /// 税率
        /// </summary>
        public decimal? Rate { get; set; }
        /// <summary>
        /// 是否必填  true 是   false  否
        /// </summary>
        public bool? Required { get; set; }


    }
}
