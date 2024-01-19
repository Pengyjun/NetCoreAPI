using GHMonitoringCenterApi.Application.Contracts.Dto.Project.Approver;
using GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Domain.Shared.Const;
using GHMonitoringCenterApi.Domain.Shared.Util;
using GHMonitoringCenterApi.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.IService.Project
{
    /// <summary>
    /// 项目审批人业务层
    /// </summary>
    public interface IProjectApproverService
    {
        /// <summary>
        /// 保存项目审批人
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> SaveProjectApproverAsync(SaveProjectApproverRequestDto model);

        /// <summary>
        /// 移除项目审批人
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> RemoveProjectApproverAsync(RemoveProjectApproverRequestDto model);

        /// <summary>
        /// 获取审批人列表
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<ProjectApproverResponseDto>>> GetProjectApproversAsync(ProjectApproverRequestDto model);

    }
}
