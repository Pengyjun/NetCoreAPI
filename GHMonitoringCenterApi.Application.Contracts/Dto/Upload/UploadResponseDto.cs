using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Upload
{
    /// <summary>
    /// 上传响应DTO
    /// </summary>
    public class UploadResponseDto
    {
        /// <summary>
        /// 文件ID
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 文件名称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 原始文件名称
        /// </summary>
        public string? OriginName { get; set; }
        /// <summary>
        /// 后缀名称
        /// </summary>
        public string? SuffixName { get; set; }
    }
}
