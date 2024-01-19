using GHMonitoringCenterApi.Application.Contracts.Dto.GetProjectChanges;
using GHMonitoringCenterApi.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.IService.OperationLog
{
    /// <summary>
    /// http记录日志接口层
    /// </summary>
    public interface ILogService
    {

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="logRequestDto"></param>
        /// <returns></returns>
        Task WriteLogAsync(LogInfo logRequestDto);

        /// <summary>
        /// 日志信息
        /// </summary>
        /// <param name="getProjectChangesRequestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<SearchProjectChangesResponseDto>>> GetProjectChangesAsync(GetProjectChangesRequestDto getProjectChangesRequestDto);
    }
}
