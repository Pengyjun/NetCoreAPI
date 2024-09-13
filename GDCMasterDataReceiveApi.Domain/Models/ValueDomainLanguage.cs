using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 值域多语言表
    /// </summary>

    [SugarTable("t_valuedomainlanguage", IsDisabledDelete = true)]
    public class ValueDomainLanguage
    {

        public long? Id { get; set; }
        /// <summary>
        /// 语种代码
        /// </summary>
        public string? ZLANGCODE { get; set; }
        /// <summary>
        /// 值域编码描述
        /// </summary>
        public string? ZCODE_DESC { get; set; }
        /// <summary>
        /// 域值描述
        /// </summary>
        public string? ZVALUE_DESC { get; set; }
    }
}
