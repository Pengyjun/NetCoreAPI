namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.LouDong
{
    /// <summary>
    /// 入参请求实体
    /// </summary>
    public class LouDongRequestDto : BaseRequestDto
    {
        /// <summary>
        /// 需要查询的条件数组
        /// </summary>
        public List<string>? FilterParams { get; set; }
    }
}
