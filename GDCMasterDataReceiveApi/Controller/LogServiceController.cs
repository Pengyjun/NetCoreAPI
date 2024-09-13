using GDCMasterDataReceiveApi.Application.Contracts.Dto.LogService;
using GDCMasterDataReceiveApi.Application.Contracts.IService.ILogService;
using GDCMasterDataReceiveApi.Domain.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GDCMasterDataReceiveApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogServiceController : ControllerBase
    {

        public ILogService logService { get; set; }

        public LogServiceController(ILogService logService)
        {
            this.logService = logService;
        }
        /// <summary>
        /// 审计日志查询
        /// </summary>
        /// <param name="auditLogRequestDto"></param>
        /// <returns></returns>
        [HttpGet("SearchAuditLog")]
        public async Task<ResponseAjaxResult<List<AuditLogResponseDto>>> SearchAuditLogAsync([FromQuery] AuditLogRequestDto auditLogRequestDto)
        { 
          return  await logService.SearchAuditLogAsync(auditLogRequestDto);
        }
        /// <summary>
        /// 接收日志查询
        /// </summary>
        /// <param name="auditLogRequestDto"></param>
        /// <returns></returns>
        [HttpGet("SearchReceiveLog")]
        public async Task<ResponseAjaxResult<List<ReceiveLogResponseDto>>> SearchReceiveLogAsync([FromQuery] ReceiveLogRequestDto receiveLogRequestDto)
        {
            return await logService.SearchReceiveLogAsync(receiveLogRequestDto);
        }
    }
}
