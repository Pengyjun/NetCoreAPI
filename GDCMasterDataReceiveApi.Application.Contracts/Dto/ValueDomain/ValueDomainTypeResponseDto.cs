namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.ValueDomain
{

    /// <summary>
    /// 值域分类
    /// </summary>
    public class ValueDomainTypeResponseDto
    {
        /// <summary>
        /// 值域编码
        /// </summary>
        public string? Code { get; set; }
        /// <summary>
        /// 值域描述
        /// </summary>
        public string? Desc { get; set; }
    }
    /// <summary>
    /// 数据标准请求dto
    /// </summary>
    public class VDomainRequestDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string? Id { get; set; }
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
    }
    /// <summary>
    /// 
    /// </summary>
    public class SaveVDomainDto
    {
        /// <summary>
        /// 1增 2 改 3 删
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 入参
        /// </summary>
        public VDomainRequestDto? Vd { get; set; }
    }
}
