using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDCMasterDataReceiveApi.Application.Contracts.Dto
{

    /// <summary>
    ///下拉基本响应DTO
    /// </summary>
    public class BasePullDownResponseDto
    {
        /// <summary>
        /// Key
        /// </summary>
        public string? Key { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string? Name { get; set; }
    }
}
