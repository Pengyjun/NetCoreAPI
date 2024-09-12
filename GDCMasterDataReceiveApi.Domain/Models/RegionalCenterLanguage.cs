using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 中交区域中心语种
    /// </summary>
    [SugarTable("t_regionalcenterlanguage", IsDisabledDelete = true)]
    public class RegionalCenterLanguage : BaseEntity<long>
    {
        /// <summary>
        /// 中交区域中心代码
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? Code { get; set; }
        /// <summary>
        /// 语种代码
        /// 10
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? ZLANGCODE { get; set; }
        /// <summary>
        /// 编码描述
        /// 255
        /// </summary>
        [SugarColumn(Length = 500)]
        public string? ZCODE_DESC { get; set; }
    }
}
