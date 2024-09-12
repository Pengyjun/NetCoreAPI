namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.OtherModels
{
    /// <summary>
    /// 区域其他models
    /// </summary>
    public class RegionalCenterModels
    {
        public List<ZMDGS_ZLANG4>? Item { get; set; }
    }
    /// <summary>
    /// 多语言描述表类型
    /// </summary>
    public class ZMDGS_ZLANG4
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
    }
}
