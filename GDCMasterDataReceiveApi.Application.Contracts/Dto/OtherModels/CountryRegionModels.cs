namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.OtherModels
{
    /// <summary>
    /// 国家地区其他models
    /// </summary>
    public class CountryRegionModels
    {
        public List<ZLANG_LIST> Item { get; set; }
    }

    /// <summary>
    /// 多语言描述表类型
    /// </summary>
    public class ZLANG_LIST
    {
        /// <summary>
        /// 语种代码
        /// </summary>
        public string? ZLANGCODE { get; set; }
        /// <summary>
        /// 编码描述
        /// </summary>
        public string? ZCODE_DESC { get; set; }

    }
}
