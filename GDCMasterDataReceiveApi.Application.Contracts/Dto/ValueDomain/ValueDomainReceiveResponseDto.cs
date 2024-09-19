namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.ValueDomain
{
    /// <summary>
    /// 值域响应dto
    /// </summary>
    public class ValueDomainReceiveResponseDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 值域编码
        /// </summary>

        public string? ZDOM_CODE { get; set; }
        /// <summary>
        /// 值域编码描述
        /// </summary>

        public string? ZDOM_DESC { get; set; }
        /// <summary>
        /// 域值 
        /// </summary>

        public string? ZDOM_VALUE { get; set; }
        /// <summary>
        /// 域值描述 
        /// </summary>

        public string? ZDOM_NAME { get; set; }
        /// <summary>
        /// 上级域值编码 
        /// </summary>

        public string? ZDOM_SUP { get; set; }
        /// <summary>
        /// 备注
        /// </summary>

        public string? ZREMARKS { get; set; }
        /// <summary>
        /// 版本
        /// </summary>

        public string? ZVERSION { get; set; }

    }
}
