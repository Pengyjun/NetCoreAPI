using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 国家语言
    /// </summary>
    [SugarTable("t_countrylanguage", IsDisabledDelete = true)]
    public class CountryLanguage
    {

        /// <summary>
        /// 语种代码
        /// </summary>  
        [SugarColumn(Length = 128)]
        public string? ZLANGCODE { get; set; }
        /// <summary>
        /// 编码描述
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? ZCODE_DESC { get; set; }
    }
}
