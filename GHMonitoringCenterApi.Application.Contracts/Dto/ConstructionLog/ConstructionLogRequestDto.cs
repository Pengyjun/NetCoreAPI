using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.ConstructionLog
{
    /// <summary>
    /// 获取施工日志列表请求Dto
    /// </summary>
    public class ConstructionLogRequestDto: BaseRequestDto
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        public string? ProjectName { get; set; }
        /// <summary>
        /// 所属公司Id
        /// </summary>
        public Guid? CompanyId { get; set; }
        /// <summary>
        /// 所属项目部Id
        /// </summary>
        public Guid? ProjectDepartmentId { get; set; }
        /// <summary>
        /// 项目状态
        /// </summary>
        public string[]? StatusId { get; set; }
        /// <summary>
        /// 选择日期
        /// </summary>
        public DateTime? Time { get; set; }
    }
}
