using GDCMasterDataReceiveApi.Application.Contracts.Dto.LogService;
using GDCMasterDataReceiveApi.Domain.Shared;
using GDCMasterDataReceiveApi.Domain.Shared.Annotation;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDCMasterDataReceiveApi.Application.Contracts.IService.ILogService
{

    /// <summary>
    /// 日志服务接口
    /// </summary>
    [DependencyInjection]
    public interface ILogService
    {
        /// <summary>
        /// 审计日志查询
        /// </summary>
        /// <param name="auditLogRequestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<AuditLogResponseDto>>> SearchAuditLogAsync(AuditLogRequestDto auditLogRequestDto);



        /// <summary>
        /// 接收日志查询
        /// </summary>
        /// <param name="receiveLogRequestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<ReceiveLogResponseDto>>> SearchReceiveLogAsync(ReceiveLogRequestDto  receiveLogRequestDto);
    }
}
