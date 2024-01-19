using GHMonitoringCenterApi.Application.Contracts.Dto.BizAuthorize;
using GHMonitoringCenterApi.Application.Contracts.IService.BizAuthorize;
using GHMonitoringCenterApi.Application.Contracts.IService.Job;
using GHMonitoringCenterApi.Application.Service.Authorize;
using GHMonitoringCenterApi.Domain.Enums;
using GHMonitoringCenterApi.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GHMonitoringCenterApi.Controllers.BizAuthorize
{
    /// <summary>
    /// 业务授权
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BizAuthorizeController : BaseController
    {

        #region 依赖注入

        /// <summary>
        /// 授权业务
        /// </summary>
        private readonly IBizAuthorizeService _bizAuthorizeService;

        #endregion

        public BizAuthorizeController(IBizAuthorizeService bizAuthorizeService)
        {
            _bizAuthorizeService = bizAuthorizeService;
        }

        /// <summary>
        /// 保存授权用户
        /// </summary>
        /// <returns></returns>
        [HttpPost("SaveAuthorizedUser")]
        public async Task<ResponseAjaxResult<bool>> SaveAuthorizedUserAsync([FromBody] SaveAuthorizeRequestDto model)
        {
            return await _bizAuthorizeService.SaveAuthorizedUserAsync(model);
        }

        /// <summary>
        /// 搜索用户列表 For 授权
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchUsersForAuthorize")]
        public async Task<ResponseAjaxResult<List<UserAuthorizeResponseDto>>> SearchUsersForAuthorizeAsync([FromQuery] SearchAuthorizeRequestDto model)
        {
            return await _bizAuthorizeService.SearchUsersForAuthorizeAsync(model);
        }


        /// <summary>
        /// 移除授权用户
        /// </summary>
        /// <returns></returns>
        [HttpPost("RemoveAuthorizedUser")]
        public async Task<ResponseAjaxResult<bool>> RemoveAuthorizedUserAsync([FromBody] RemoveAuthorizeRequestDto model)
        {
            return await _bizAuthorizeService.RemoveAuthorizedUserAsync(model);
        }

    }
}
