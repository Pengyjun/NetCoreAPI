namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.GovernanceData
{
    /// <summary>
    /// 数据标准
    /// </summary>
    public class DataStardardDto
    {
        /// <summary>
        /// 类别编码
        /// </summary>
        public string? TypeCode { get; set; }
        /// <summary>
        /// 类别名称
        /// </summary>
        public string? TypeName { get; set; }
        /// <summary>
        /// 标准编码
        /// </summary>
        public string? StardardCode { get; set; }
        /// <summary>
        /// 标准名称
        /// </summary>
        public string? StardardName { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string? StatusName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string? CreateTime { get; set; }
    }
    /// <summary>
    /// 请求dto
    /// </summary>
    public class DataStardardRequestDto : BaseRequestDto
    {
        /// <summary>
        /// 编码
        /// </summary>
        public string? Code { get; set; }
    }
}
