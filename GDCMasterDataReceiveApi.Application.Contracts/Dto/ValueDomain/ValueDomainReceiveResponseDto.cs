using MiniExcelLibs.Attributes;

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
        [ExcelIgnore]
        public string Id { get; set; }
        /// <summary>
        /// 值域编码
        /// </summary>
        [ExcelColumnName("值域编码")]

        public string? ZDOM_CODE { get; set; }
        /// <summary>
        /// 值域编码描述
        /// </summary>
        [ExcelColumnName("值域编码描述")]

        public string? ZDOM_DESC { get; set; }
        /// <summary>
        /// 域值 
        /// </summary>
        [ExcelColumnName("域值")]

        public string? ZDOM_VALUE { get; set; }
        /// <summary>
        /// 域值描述 
        /// </summary>
        [ExcelColumnName("域值描述")]

        public string? ZDOM_NAME { get; set; }
        /// <summary>
        /// 上级域值编码 
        /// </summary>

        [ExcelIgnore]
        public string? ZDOM_SUP { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [ExcelColumnName("备注")]

        public string? ZREMARKS { get; set; }
        /// <summary>
        /// 版本
        /// </summary>
        [ExcelIgnore]

        public string? ZVERSION { get; set; }

        [ExcelIgnore]
        public DateTime? CreateTime { get; set; }
        [ExcelIgnore]
        public DateTime? UpdateTime { get; set; }

    }
}
