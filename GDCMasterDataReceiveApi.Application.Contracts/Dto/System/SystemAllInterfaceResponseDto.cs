using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.System
{
    /// <summary>
    /// 获取系统所有接口方法响应DTO
    /// </summary>
    public class SystemAllInterfaceResponseDto
    {
        /// <summary>
        /// 接口中文名称
        /// </summary>
        public string? Key { get; set; }
        /// <summary>
        /// 接口英文名称
        /// </summary>
        public string? Value { get; set; }
    }
}
