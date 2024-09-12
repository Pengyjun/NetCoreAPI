namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.OtherModels
{
    /// <summary>
    /// 语言语种其他models
    /// </summary>
    public class LanguageModels
    { }
    /// <summary>
    /// 多语言描述表类型
    /// </summary>
    public class ZMDGTT_ZLANG2
    {
        public List<LanguageC>? Item { get; set; }
    }
    public class LanguageC
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
        /// 域值描述:域值多语言描述
        /// 255
        /// </summary>
        public string? ZVALUE_DESC { get; set; }
    }
}
