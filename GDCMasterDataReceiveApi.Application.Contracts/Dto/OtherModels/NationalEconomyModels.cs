namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.OtherModels
{
    /// <summary>
    /// 国民经济行业分类其他models
    /// </summary>
    public class NationalEconomyModels
    {
        public List<ZMDGTT_ZLANG>? Item { get; set; }
    }
    public class ZMDGTT_ZLANG
    {
        /// <summary>
        /// 国民经济行业分类代码
        /// </summary>
        public string? ZNEQCODE {  get; set; }
        /// <summary>
        /// 语种代码
        /// </summary>
        public string? ZLANGCODE {  get; set; }
        /// <summary>
        /// 编码描述
        /// </summary>
        public string? ZCODE_DESC {  get; set; }
    }
}
