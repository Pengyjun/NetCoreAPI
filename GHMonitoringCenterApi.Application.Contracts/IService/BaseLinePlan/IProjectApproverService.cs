using GHMonitoringCenterApi.Application.Contracts.Dto.BaseLinePlan;
using GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Domain.Shared.Const;
using GHMonitoringCenterApi.Domain.Shared.Util;
using GHMonitoringCenterApi.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.IService.BaseLinePlan
{
    /// <summary>
    /// 基准计划审批人业务层
    /// </summary>
    public interface IBaseLinePlanApproverService
    {
        /// <summary>
        /// 保存基准计划审批人
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> SaveBaseLinePlanApproverAsync(SaveBaseLinePlanApproverRequestDto model);

        /// <summary>
        /// 移除基准计划审批人
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> RemoveBaseLinePlanApproverAsync(RemoveBaseLinePlanApproverRequestDto model);

        /// <summary>
        /// 获取审批人列表
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<BaseLinePlanApproverResponseDto>>> GetBaseLinePlanApproversAsync(BaseLinePlanApproverRequestDto model);

    }
}
