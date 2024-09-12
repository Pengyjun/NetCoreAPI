using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 国民经济行业分类语种
    /// </summary>
    [SugarTable("t_nationaleconomylanguage", IsDisabledDelete = true)]
    public class NationalEconomyLanguage : BaseEntity<long>
    {
        /// <summary>
        /// 国民经济行业分类代码
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? ZNEQCODE { get; set; }
        /// <summary>
        /// 语种代码
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? ZLANGCODE { get; set; }
        /// <summary>
        /// 编码描述
        /// </summary>
        [SugarColumn(Length = 200)]
        public string? ZCODE_DESC { get; set; }
    }
}
