using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.OtherModels
{
    /// <summary>
    /// 币种语言
    /// </summary>
    public class CurrencyLanguageRequestDto
    {
        /// <summary>
        /// 币种语言项
        /// </summary>
        public List<CurrencyItem>? Item { get; set; }
    }

    public class CurrencyItem
    {
        /// <summary>
        /// 语种代码
        /// </summary>
        public string? ZLANGCODE { get; set; }
        /// <summary>
        /// 编码描述
        /// </summary>
        public string? ZCODE_DESC { get; set; }
    }
}
