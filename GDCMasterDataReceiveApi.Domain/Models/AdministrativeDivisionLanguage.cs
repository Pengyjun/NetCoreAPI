using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDCMasterDataReceiveApi.Domain.Models
{

    /// <summary>
    /// 境内行政区划
    /// </summary>
    [SugarTable("t_administrativedivisionlanguage", IsDisabledDelete = true)]
    public class AdministrativeDivisionLanguage
    {
        [SugarColumn(Length = 32)]
        public long? Id { get; set; }
        /// <summary>
        /// 语种代码
        /// </summary>
        [SugarColumn(Length =32)]
        public string? ZLANGCODE { get; set; }
        /// <summary>
        /// 编码描述
        /// </summary>
        [SugarColumn(Length = 32)]
        public string? ZCODE { get; set; }
    }
}
