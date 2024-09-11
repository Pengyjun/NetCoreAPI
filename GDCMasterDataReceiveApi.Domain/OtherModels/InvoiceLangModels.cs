namespace GDCMasterDataReceiveApi.Domain.OtherModels
{
    /// <summary>
    /// 发票语言语种其他models
    /// </summary>
    public class InvoiceLangModels
    {
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
