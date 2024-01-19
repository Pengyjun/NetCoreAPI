using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Timing
{
    /// <summary>
    /// 项目干系单位
    /// </summary>
    public class PomProjectOrgResponseDto
    {
        /// <summary>
        /// pomId
        /// </summary>
        public Guid? Id { get; set; }
        /// <summary>
        /// 项目Id
        /// </summary>
        public Guid ProjectId { get; set; }
        /// <summary>
        /// 单位Id
        /// </summary>
        public Guid? OrganizationId { get; set; }
        /// <summary>
        /// 单位类型
        /// </summary>
        public string? Type { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string? Remarks { get; set; }
    }
}
