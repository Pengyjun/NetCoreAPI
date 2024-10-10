namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.DHData
{
    /// <summary>
    /// 
    /// </summary>
    public class KeyVerificationRequestDto
    {
        /// <summary>
        /// 页码参数
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 接口授权码
        /// </summary>
        public string? FCode { get; set; }
        /// <summary>
        /// 接口地址
        /// </summary>
        public string? InterfaceUrl { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? UpdateTime { get; set; }
    }
}
