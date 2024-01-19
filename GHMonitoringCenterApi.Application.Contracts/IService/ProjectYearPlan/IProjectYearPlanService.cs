using GHMonitoringCenterApi.Application.Contracts.Dto.ProjectYearPlan;
using GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.IService.ProjectYearPlan
{

    /// <summary>
    /// 项目年初计划接口层
    /// </summary>
    public interface IProjectYearPlanService
    {
        /// <summary>
        /// 获取项目年初计划列表
        /// </summary>
        /// <param name="projectYearPlanRequestDto">请求参数封装</param>
        /// <returns></returns>
        Task<ResponseAjaxResult<ProjectYearPlanResponseDto>> SearchProjectPlanAsync(ProjectYearPlanRequestDto projectYearPlanRequestDto);

        #region 新增拟建项目
        /// <summary>
        /// 新增拟建项目
        /// </summary>
        /// <param name="insertProjectYearPlanRequestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> InsertPlanBuildProject(InsertProjectYearPlanRequestDto insertProjectYearPlanRequestDto);
        #endregion


        Task<ResponseAjaxResult<ProjectPlanWbsResponseDto>> GetProjectPlanWbs(ProjectPlanWbsRequestDto projectPlanWbsRequestDto  );

        /// <summary>
        /// 保存wbs
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> SaveProjectPlanWbsAsync(ProjectYearPlanTreeRequestDto projectYearPlanTreeRequestDto);

    }
}
