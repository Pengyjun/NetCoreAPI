using GHMonitoringCenterApi.Domain.Shared.Util;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project
{
    /// <summary>
    /// ProjectWBS响应类
    /// </summary>
    public class ProjectWBSResponseDto:TreeNodeParentId<ProjectWBSResponseDto>
    {
        /// <summary>
        /// 项目Id
        /// </summary>
        public string? ProjectId { get; set; }
        /// <summary>
        /// 项目编号
        /// </summary>
        public string? ProjectNum { get; set; }
        /// <summary>
        /// 项目WBSId
        /// </summary>
        public string? ProjectWBSId { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public decimal? UnitPrice { get; set; }
        /// <summary>
        /// 工程量
        /// </summary>
        public decimal? EngQuantity { get; set; }
        /// <summary>
        /// 合同额
        /// </summary>
        public decimal? ContractAmount { get; set; }
    }
}
