using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.ConstructionLog
{
    /// <summary>
    /// 获取项目施工日志响应Dto
    /// </summary>
    public class ConstructionLogResponseDto
    {
        /// <summary>
        /// 施工日志ID
        /// </summary>
        public Guid? Id { get; set; }
        /// <summary>
        /// 项目Id
        /// </summary>
        public Guid? ProjectId { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string? ProjectName { get; set; }
        /// <summary>
        /// 公司Id
        /// </summary>
        public Guid? CompanyId { get; set; }
        /// <summary>
        /// 所属公司名称
        /// </summary>
        public string? CompanyName { get; set; }
        /// <summary>
        /// 项目状态id
        /// </summary>
        public Guid? Status { get; set; }
        /// <summary>
        /// 项目状态名称
        /// </summary>
        public string? StatusName { get; set; }
        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime? SubmitTime { get; set; }
        /// <summary>
        /// 施工填报时间
        /// </summary>
        public int? DateDay { get; set; }
    }
}
