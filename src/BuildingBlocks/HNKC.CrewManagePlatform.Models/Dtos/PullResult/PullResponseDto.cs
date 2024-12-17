namespace HNKC.CrewManagePlatform.Models.Dtos.PullResult
{

    /// <summary>
    /// 
    /// </summary>
    public class PullResponseDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string? Code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? Message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? Count { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<DictionaryCtDto>? Data { get; set; }
    }
    /// <summary>
    /// 响应dto
    /// </summary>
    public class DictionaryCtDto
    {
        /// <summary>
        /// 域值描述
        /// </summary>
        public string? ZDOM_NAME { get; set; }
        /// <summary>
        /// 域值
        /// </summary>
        public string? ZDOM_VALUE { get; set; }
        /// <summary>
        /// 编码描述
        /// </summary>
        public string? ZDOM_DESC { get; set; }
        /// <summary>
        /// 值域编码
        /// </summary>
        public string? ZDOM_CODE { get; set; }
        /// <summary>
        /// 域值层级 
        /// </summary>
        public string? ZDOM_LEVEL { get; set; }
        /// <summary>
        /// 上级域值编码 
        /// </summary>
        public string? ZDOM_SUP { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string? ZREMARKS { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public string? ZCHTIME { get; set; }
        /// <summary>
        /// 版本
        /// </summary>
        public string? ZVERSION { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string? ZSTATE { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public string? ZDELETE { get; set; }
        /// <summary>
        /// 源id
        /// </summary>
        public string? Id { get; set; }
    }
}
