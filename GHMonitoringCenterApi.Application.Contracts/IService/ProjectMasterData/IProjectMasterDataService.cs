using GHMonitoringCenterApi.Application.Contracts.Dto;
using GHMonitoringCenterApi.Application.Contracts.Dto.ProjectMasterData;
using GHMonitoringCenterApi.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.IService.ProjectMasterData
{
    /// <summary>
    /// 项目主数据接口层
    /// </summary>
    public interface IProjectMasterDataService
    {
        /// <summary>
        /// 获取项目主数据列表
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<SearchProjectMasterDataResponseDto>>> SearchProjectMasterDataAsync(BaseRequestDto baseRequestDto);
    }
}
