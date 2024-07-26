namespace GDCMasterDataReceiveApi.Application.Contracts.Dto
{

    /// <summary>
    ///下拉基本响应DTO
    /// </summary>
    public class BasePullDownResponseDto
    {
        /// <summary>
        /// Key
        /// </summary>
        public string? Key { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string? Name { get; set; }
    }
}
