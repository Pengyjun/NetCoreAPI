using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.LogService
{

    /// <summary>
    /// 审计日志请求DTO
    /// </summary>
    public class AuditLogRequestDto : BaseRequestDto
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public string? StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string? EndTime { get; set; }


        
    }
}
