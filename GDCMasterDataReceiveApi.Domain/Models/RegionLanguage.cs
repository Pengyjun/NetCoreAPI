using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 中交区域总部语种
    /// </summary>
    [SugarTable("t_regionlanguage", IsDisabledDelete = true)]
    public class RegionLanguage : BaseEntity<long>
    {
        /// <summary>
        /// 中交区域总部代码
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? Code { get; set; }
        /// <summary>
        /// 语种代码
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? ZLANGCODE { get; set; }
        /// <summary>
        /// 编码描述:ZCRHCODE中交区域总部代码多语言描述
        /// </summary>
        [SugarColumn(Length = 200)]
        public string? ZCODE_DESC { get; set; }
        /// <summary>
        /// 简称:ZCRHABBR简称
        /// </summary>
        [SugarColumn(Length = 200)]
        public string? ZSCRTEXT_S { get; set; }
    }
}
