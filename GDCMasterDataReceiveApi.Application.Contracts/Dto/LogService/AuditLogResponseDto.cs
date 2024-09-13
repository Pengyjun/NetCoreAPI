using GDCMasterDataReceiveApi.Domain.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.LogService
{

    /// <summary>
    /// 审计日志请求信息
    /// </summary>
    public class AuditLogResponseDto
    {
      
        /// <summary>
        /// 请求地址
        /// </summary>
        public string? Url { get; set; }
        /// <summary>
        /// 请求参数
        /// </summary>
        public string? RequestParame { get; set; }
        /// <summary>
        /// 异常信息
        /// </summary>
        public string? Exceptions { get; set; }
        /// <summary>
        /// 响应状态
        /// </summary>
        public int? ResponseStatus { get; set; }
        /// <summary>
        /// Ip
        /// </summary>
        public string? Ip { get; set; }
        /// <summary>
        /// 请求时间
        /// </summary>
        public string? RequestTime { get; set; }
    }
}
