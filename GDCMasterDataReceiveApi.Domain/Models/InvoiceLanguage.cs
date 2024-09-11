using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 发票语种
    /// </summary>
    [SugarTable("t_invoicelanguage", IsDisabledDelete = true)]
    public class InvoiceLanguage : BaseEntity<long>
    {
        /// <summary>
        /// 发票代码 key
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? InvoiceCode { get; set; }
        /// <summary>
        /// 语种代码
        /// 10
        /// </summary>
        [SugarColumn(Length = 100, ColumnName = "Code")]
        public string? ZLANGCODE { get; set; }
        /// <summary>
        /// 编码描述
        /// 255
        /// </summary>
        [SugarColumn(Length = 1000, ColumnName = "Remark")]
        public string? ZCODE_DESC { get; set; }
    }
    /// <summary>
    /// 多语言描述表类型
    /// </summary>
    public class InvoiceLang
    {
        public List<InvoiceLanguageItem>? Item { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class InvoiceLanguageItem
    {
        /// <summary>
        /// 语种代码
        /// 10
        /// </summary>
        public string? ZLANGCODE { get; set; }
        /// <summary>
        /// 编码描述
        /// 255
        /// </summary>
        public string? ZCODE_DESC { get; set; }

        /// <summary>
        /// 发票编码
        /// </summary>
        public string? InvoiceCode { get; set; }
    }
}
