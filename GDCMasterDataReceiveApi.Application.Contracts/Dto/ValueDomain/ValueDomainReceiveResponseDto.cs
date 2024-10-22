using System.ComponentModel;

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
        [DisplayName("值域编码")]

        public string? ZDOM_CODE { get; set; }
        /// <summary>
        /// 值域编码描述
        /// </summary>
        [DisplayName("值域编码描述")]

        public string? ZDOM_DESC { get; set; }
        /// <summary>
        /// 域值 
        /// </summary>
        [DisplayName("域值")]

        public string? ZDOM_VALUE { get; set; }
        /// <summary>
        /// 域值描述 
        /// </summary>
        [DisplayName("域值描述")]

        public string? ZDOM_NAME { get; set; }
        /// <summary>
        /// 上级域值编码 
        /// </summary>

        [DisplayName("上级域值编码")]
        public string? ZDOM_SUP { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [DisplayName("备注")]

        public string? ZREMARKS { get; set; }
        /// <summary>
        /// 版本
        /// </summary>
        [DisplayName("版本")]

        public string? ZVERSION { get; set; }

        public DateTime? CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }

    }
}
