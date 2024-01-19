using GHMonitoringCenterApi.Application.Contracts.Dto;
using GHMonitoringCenterApi.Application.Contracts.Dto.SystemUpdatLog;
using GHMonitoringCenterApi.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.IService.SystemUpdatLog
{
    /// <summary>
    /// 系统日志业务接口层
    /// </summary>
    public interface ISystemUpdatLogService
    {
        /// <summary>
        /// 添加系统日志
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> SaveSystemLogAsync(AddSystemLogRequsetDto addSystemLog, bool IsAdmin, CurrentUser currentUser);
        /// <summary>
        /// 查询系统日志
        /// </summary>
        /// <param name="QuerySystemLog"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<SystemUpdatLogReponseDto>>> SearchSystemLogAsync(SearchSystemUpdatLogRequsetDto QuerySystemLog);

        /// <summary>
        /// 系统日志已读标识
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<SearchLogReadSignReponseDto>> SearchLogReadSignAsync(Guid Id);
        /// <summary>
        /// 添加已读标识
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> AddLogReadSignAsync(Guid Id);
        /// <summary>
        /// 更改系统日志
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> UpdataSystemLogAsync(UpdataSystemUpdatLogRequsetDto updataSystemUpdatLogRequsetDto,bool IsAdmin, CurrentUser currentUser);
        /// <summary>
        /// 删除系统日志
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> DeleteSystemLogAsync(Guid Id,bool IsAdmin, CurrentUser currentUser);
    }
}
