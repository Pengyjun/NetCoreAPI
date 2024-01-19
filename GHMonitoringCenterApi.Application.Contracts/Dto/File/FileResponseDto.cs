using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.File
{
    /// <summary>
    /// 文件的返回信息
    /// </summary>
    public class FileResponseDto
    {

        /// <summary>
        /// 字节集合
        /// </summary>
        public byte[]? Buffer { get; set; }
    }
}
