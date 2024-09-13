using GDCMasterDataReceiveApi.Domain.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.LogService
{
    /// <summary>
    /// 接收日志查询DTO
    /// </summary>
    public class ReceiveLogRequestDto:BaseRequestDto
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 数据接收类型
        /// </summary>
        public ReceiveDataType ReceiveDataType { get; set; }
    }
}
