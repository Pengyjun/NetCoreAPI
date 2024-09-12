using SqlSugar;
using System.ComponentModel.DataAnnotations.Schema;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 币种
    /// </summary>
    [SugarTable("t_currencylanguage", IsDisabledDelete = true)]
    public class Currencylanguage 
    {
        /// <summary>
        /// 
        /// </summary>
        public long? Id { get; set; }
        /// <summary>
        /// 语种代码
        /// </summary>
        [SugarColumn(Length = 64)]
        public string? ZLANGCODE { get; set; }
        /// <summary>
        /// 编码描述
        /// </summary>
        [SugarColumn(Length = 128)]
        public string? ZCODE_DESC { get; set; }
      
    }
}
