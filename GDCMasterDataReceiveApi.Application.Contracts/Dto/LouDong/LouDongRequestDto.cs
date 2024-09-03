namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.LouDong
{
    /// <summary>
    /// 入参请求实体
    /// </summary>
    public class LouDongRequestDto : BaseRequestDto
    {
        /// <summary>
        /// 列
        /// </summary>
        public List<string>? Columns { get; set; }
    }

}
