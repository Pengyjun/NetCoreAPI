using GHMonitoringCenterApi.Application.Contracts.Dto.ProjectDepartMent;
using GHMonitoringCenterApi.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.IService.Project
{
    /// <summary>
    /// 项目部接口层
    /// </summary>
    public interface IProjectDepartMentService
    {
        /// <summary>
        /// 项目部产值chart图
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<OutputValueChartResponseDto>>> GetOutputValueChartAsync(OutputChartRequestDto outputChartRequestDto);

        /// <summary>
        /// 获取用户的项目Id
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<GetUserProjectIdResponseDto>> GetUserProjectIdAsync();

        /// <summary>
        /// 获取项目部的相关产值-v2
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<OutputValueResponseDto>> GetOutputValueV2Async(OutputValueRequestDto model);

    }
}
