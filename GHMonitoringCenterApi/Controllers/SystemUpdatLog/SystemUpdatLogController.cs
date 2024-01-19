using GHMonitoringCenterApi.Application.Contracts.Dto;
using GHMonitoringCenterApi.Application.Contracts.Dto.SystemUpdatLog;
using GHMonitoringCenterApi.Application.Contracts.IService.SystemUpdatLog;
using GHMonitoringCenterApi.CustomAttribute;
using GHMonitoringCenterApi.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GHMonitoringCenterApi.Controllers.SystemUpdatLog
{
    /// <summary>
    /// 系统更新相关控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class SystemUpdatLogController: BaseController
    {
        #region 依赖注入
        public ISystemUpdatLogService systemUpdatLog { get; set; }
        public SystemUpdatLogController(ISystemUpdatLogService systemUpdatLog)
        {
            this.systemUpdatLog = systemUpdatLog;
        }
        #endregion
        /// <summary>
        /// 添加系统日志
        /// </summary>
        /// <returns></returns>
        [HttpPost("SaveSystemLog")]
        [UnitOfWork]
        public async Task<ResponseAjaxResult<bool>> SaveSystemLogAsync([FromBody] AddSystemLogRequsetDto addSystemLog)
        {
            return await systemUpdatLog.SaveSystemLogAsync(addSystemLog, CurrentUser.RoleInfos[0].IsAdmin, CurrentUser);
        }
        /// <summary>
        /// 查询系统日志
        /// </summary>
        /// <param name="addSystemLog"></param>
        /// <returns></returns>
        [HttpGet("SearchSystemLog")]
        public async Task<ResponseAjaxResult<List<SystemUpdatLogReponseDto>>> SearchSystemLogAsync([FromQuery] SearchSystemUpdatLogRequsetDto QuerySystemLog)
        {   
            return await systemUpdatLog.SearchSystemLogAsync(QuerySystemLog);
        }
        /// <summary>
        /// 系统日志已读标识
        /// </summary>
        [HttpGet("LogReadSign")]
        public async Task<ResponseAjaxResult<SearchLogReadSignReponseDto>> SearchLogReadSignAsync()
        {            
            return await systemUpdatLog.SearchLogReadSignAsync(CurrentUser.Id);
        }
        /// <summary>
        /// 添加已读标识
        /// </summary>
        /// <returns></returns>
        [HttpGet("AddLogReadSign")]
        public async Task<ResponseAjaxResult<bool>> AddLogReadSignAsync()
        {
            return await systemUpdatLog.AddLogReadSignAsync(CurrentUser.Id);
        }
        /// <summary>
        /// 修改系统日志
        /// </summary>
        /// <returns></returns>
        [HttpPost("UpdataSystemLog")]
        public async Task<ResponseAjaxResult<bool>> UpdataSystemLogAsync([FromBody] UpdataSystemUpdatLogRequsetDto updataSystemUpdatLogRequsetDto)
        {
            return await systemUpdatLog.UpdataSystemLogAsync(updataSystemUpdatLogRequsetDto,CurrentUser.RoleInfos[0].IsAdmin,CurrentUser);
        }
        /// <summary>
        /// 删除系统日志
        /// </summary>
        /// <param name="updataSystemUpdatLogRequsetDto"></param>
        /// <returns></returns>
        [HttpPost("DeleteSystemLog")]
        public async Task<ResponseAjaxResult<bool>> DeleteSystemLogAsync([FromBody] BasePrimaryRequestDto basePrimaryRequestDto)
        {
            return await systemUpdatLog.DeleteSystemLogAsync(basePrimaryRequestDto.Id, CurrentUser.RoleInfos[0].IsAdmin, CurrentUser);
        }
    }
}
