using GHMonitoringCenterApi.Application.Contracts.Dto.GetProjectChanges;
using GHMonitoringCenterApi.Application.Contracts.IService.OperationLog;
using GHMonitoringCenterApi.Domain.Shared;
using GHMonitoringCenterApi.Domain.Shared.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using UtilsSharp;

namespace GHMonitoringCenterApi.Controllers
{
    /// <summary>
    /// 操作日志
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OperationLogController : BaseController
    {
        #region 依赖注入
        private ILogService logService { get; set; }
        private ILogger<OperationLogController> logger { get; set; }
        public OperationLogController(ILogService logService, ILogger<OperationLogController> logger)
        {
            this.logService = logService;
            this.logger = logger;
        }
        #endregion

        /// <summary>
        /// 登陆日志
        /// </summary>
        /// <param name="logRequestDto"></param>
        /// <returns></returns>
        [HttpGet("LoginLog")]
        public async Task<ResponseAjaxResult<bool>> LoginLogAsync()
        {
             ResponseAjaxResult<bool> responseAjaxResult=new ResponseAjaxResult<bool>();
            var logRequestDto = new LogInfo()
            {
                OperationType = 5,
                BusinessModule = "LoginLogAsync",
                OperationObject = "",
                BusinessRemark = $"{CurrentUser.Name}用户登录",
                //ClientIp = IpHelper.GetClientIp(),
                //Deviceinformation = HttpContentAccessFactory.Current.Request.Headers["User-Agent"].ToString(),
                OperationId = CurrentUser.Id,
                OperationName = CurrentUser.Name,
                logDiffDtos = new List<LogDiffDto>()
            };
            
            await logService.WriteLogAsync(logRequestDto);
            responseAjaxResult.Data = true;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }

        /// <summary>
        /// 登出日志
        /// </summary>
        /// <param name="logRequestDto"></param>
        /// <returns></returns>
        [HttpPost("LogoutLog")]
        public async Task<ResponseAjaxResult<bool>> LogoutAsync()
        {
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            var logRequestDto = new LogInfo()
            {
                OperationType = 6,
                BusinessModule = "LogoutAsync",
                OperationObject = "",
                BusinessRemark = $"{CurrentUser.Name}用户登出",
                //ClientIp = IpHelper.GetClientIp(),
                //Deviceinformation = HttpContentAccessFactory.Current.Request.Headers["User - Agent"].ToString(),
                OperationId = CurrentUser.Id,
                OperationName = CurrentUser.Name,
                logDiffDtos = new List<LogDiffDto>()
            };
            await logService.WriteLogAsync(logRequestDto);
            responseAjaxResult.Data = true;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }/// <summary>
         /// 日志信息
         /// </summary>
         /// <param name="getProjectChangesRequestDto"></param>
         /// <returns></returns>
        [HttpGet("GetProjectChanges")]
        public async Task<ResponseAjaxResult<List<SearchProjectChangesResponseDto>>> GetProjectChangesAsync([FromQuery] GetProjectChangesRequestDto getProjectChangesRequestDto)
        {
            return await logService.GetProjectChangesAsync(getProjectChangesRequestDto);
        }
    }
}
