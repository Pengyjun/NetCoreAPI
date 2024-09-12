namespace GDCMasterDataReceiveApi.Domain.OtherModels
{
    /// <summary>
    /// 区域总部其他models
    /// </summary>
    public class RegionalModels
    {
    }
    /// <summary>
    /// 多语言描述表类型
    /// </summary>
    public class ZMDGTT_ZLANG3
    {
        /// <summary>
        /// 语种代码
        /// </summary>
        public string? ZLANGCODE { get; set; }
        /// <summary>
        /// 编码描述:ZCRHCODE中交区域总部代码多语言描述
        /// </summary>
        public string? ZCODE_DESC { get; set; }
        /// <summary>
        /// 简称:ZCRHABBR简称
        /// </summary>
        public string? ZSCRTEXT_S { get; set; }
    }
}
