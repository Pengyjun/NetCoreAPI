using GHMonitoringCenterApi.Application.Contracts.Dto;
using GHMonitoringCenterApi.Application.Contracts.Dto.HelpCenter;
using GHMonitoringCenterApi.Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.IService.HelpCenter
{
    /// <summary>
    /// 帮助中心接口层
    /// </summary>
    public interface IHelpCenterService
    {
        /// <summary>
        /// 新增或修改帮助中心菜单
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> SaveHelpCenterMenuAsync(SaveHelpCenterRequsetDto saveHelpCenterRequsetDto);
        /// <summary>
        /// 获取帮助中心菜单
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<SearchHelpCenterMenuResponseDto>>> SearchHelpCenterMenuAsync(BaseRequestDto baseRequestDto);
        /// <summary>
        /// 获取帮助中心详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<SearchHelpCenterDetailsResponseDto>> SearchHelpCenterDetailsAsync(Guid Id);
        /// <summary>
        /// 删除帮助中心菜单
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> DeletehHelpCenterMenuAsync(DeletehHelpCenterMenuRequsetDto deletehHelpCenterMenuRequsetDto);

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <returns></returns>
        Task<IActionResult> HelpCenterDownloadFileAsync();

    }
}
