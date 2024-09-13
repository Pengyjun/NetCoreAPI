using GDCMasterDataReceiveApi.Domain.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.LogService
{

    /// <summary>
    /// 接收日志返回Dto
    /// </summary>
    public class ReceiveLogResponseDto
    {
        /// <summary>
        /// 数据接收类型
        /// </summary>
        public ReceiveDataType? ReceiceDataType { get; set; }
        /// <summary>
        /// 请求参数
        /// </summary>
        public string? RequestParame { get; set; }
        /// <summary>
        /// 接收数量
        /// </summary>
        public int? ReceiveCount { get; set; }
        /// <summary>
        /// 成功数量
        /// </summary>
        public int? SuccessCount { get; set; }
        /// <summary>
        /// 失败原因
        /// </summary>
        public string? FailMessage { get; set; }
        /// <summary>
        /// 请求时间
        /// </summary>
        public DateTime? RequestTime { get; set; }
    }
}
