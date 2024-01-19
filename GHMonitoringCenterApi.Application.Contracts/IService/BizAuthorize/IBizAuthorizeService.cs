using GHMonitoringCenterApi.Application.Contracts.Dto.BizAuthorize;
using GHMonitoringCenterApi.Domain.Enums;
using GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Domain.Shared.Util;
using GHMonitoringCenterApi.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.IService.BizAuthorize
{
    /// <summary>
    /// 业务授权
    /// </summary>
    public interface IBizAuthorizeService
    {
        /// <summary>
        /// 保存授权用户
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> SaveAuthorizedUserAsync(SaveAuthorizeRequestDto model);


        /// <summary>
        /// 是否业务授权
        /// </summary>
        /// <returns></returns>
        Task<bool> IsAuthorizedAsync(Guid userId, BizModule bizModule);

        /// <summary>
        /// 搜索用户列表 For 授权
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<UserAuthorizeResponseDto>>> SearchUsersForAuthorizeAsync(SearchAuthorizeRequestDto model);


        /// <summary>
        /// 移除授权用户
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> RemoveAuthorizedUserAsync(RemoveAuthorizeRequestDto model);
        }
}
