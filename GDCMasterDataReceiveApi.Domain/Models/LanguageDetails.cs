using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 语言语种明细
    /// </summary>
    [SugarTable("t_languagedetails", IsDisabledDelete = true)]
    public class LanguageDetails : BaseEntity<long>
    {
        /// <summary>
        /// 关联的编码
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? Code { get; set; }
        /// <summary>
        /// 语种代码
        /// 10
        /// </summary>
        [SugarColumn(Length = 100, ColumnName = "LangCode")]
        public string? ZLANGCODE { get; set; }
        /// <summary>
        /// 编码描述
        /// 255
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "CodeDescption")]
        public string? ZCODE_DESC { get; set; }
        /// <summary>
        /// 域值描述:域值多语言描述
        /// 255
        /// </summary>
        [SugarColumn(Length = 500, ColumnName = "ValueDescption")]
        public string? ZVALUE_DESC { get; set; }
    }
}
