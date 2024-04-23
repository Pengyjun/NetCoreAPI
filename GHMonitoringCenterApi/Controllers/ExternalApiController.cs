using GHMonitoringCenterApi.Application.Contracts.Dto;
using GHMonitoringCenterApi.Application.Contracts.IService;
using GHMonitoringCenterApi.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GHMonitoringCenterApi.Controllers

{
    /// <summary>
    /// 对外接口控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ExternalApiController : BaseController
    {
        /// <summary>
        /// 接口注入
        /// </summary>
        public readonly IExternalApiService _externalApiService;
        /// <summary>
        /// 依赖注入
        /// </summary>
        public ExternalApiController(IExternalApiService externalApiService)
        {
            this._externalApiService = externalApiService;
        }
        /// <summary>
        /// 获取人员
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetUserInfos")]
        [AllowAnonymous]//跳过鉴权
        public async Task<ResponseAjaxResult<List<UserInfos>>> GetUserInfosAsync()
            => await _externalApiService.GetUserInfosAsync();
        /// <summary>
        /// 获取机构
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetInstutionInfos")]
        [AllowAnonymous]//跳过鉴权
        public async Task<ResponseAjaxResult<List<InstutionInfos>>> GetInstutionInfosAsync()
            => await _externalApiService.GetInstutionInfosAsync();
        /// <summary>
        /// 获取项目信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetProjectInfos")]
        [AllowAnonymous]//跳过鉴权
        public async Task<ResponseAjaxResult<List<ProjectInfos>>> GetProjectInfosAsync()
            => await _externalApiService.GetProjectInfosAsync();
        /// <summary>
        /// 获取项目干系人
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetProjectLeaderInfos")]
        [AllowAnonymous]//跳过鉴权
        public async Task<ResponseAjaxResult<List<ProjectLeaderInfos>>> GetProjectLeaderInfosAsync()
            => await _externalApiService.GetProjectLeaderInfosAsync();
        /// <summary>
        /// 获取项目信息状态
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetProjectStatusInfos")]
        [AllowAnonymous]//跳过鉴权
        public async Task<ResponseAjaxResult<List<ProjectStatusInfos>>> GetProjectStatusInfosAsync()
            => await _externalApiService.GetProjectStatusInfosAsync();
    }
}
