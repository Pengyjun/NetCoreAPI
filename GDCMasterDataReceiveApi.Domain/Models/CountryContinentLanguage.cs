using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDCMasterDataReceiveApi.Domain.Models
{

    /// <summary>
    /// 大洲多语言类型
    /// </summary>
    [SugarTable("t_countrycontinentlanguage", IsDisabledDelete = true)]
    public class CountryContinentLanguage
    {
     
        public long? Id { get; set; }
        /// <summary>
        /// 语种代码
        /// </summary>  
        [SugarColumn(Length = 128)]
        public string? ZLANGCODE { get; set; }
        /// <summary>
        /// 大洲代码描述
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? ZCODE_DESC { get; set; }
        /// <summary>
        /// 区域代码描述
        /// </summary>
        public string? ZAREA_DESC { get; set; }
    }
}
