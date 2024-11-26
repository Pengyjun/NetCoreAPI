using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.ValueDomain
{

    /// <summary>
    /// 值域分类
    /// </summary>
    public class ValueDomainTypeResponseDto
    {
        /// <summary>
        /// 值域编码
        /// </summary>
        public string? Code { get; set; }
        /// <summary>
        /// 值域描述
        /// </summary>
        public string? Desc { get; set; }
    }
}
