namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.DHData
{
    /// <summary>
    /// DH行政和核算机构映射关系
    /// </summary>
    public class DHAdministrativeDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 映射关系ID 主键
        /// </summary>
        public string? Fzid { get; set; }
        /// <summary>
        /// 行政机构主数据编码
        /// </summary>
        public string? Fzorgid { get; set; }
        /// <summary>
        /// 行政机构编码
        /// </summary>
        public string? Fzorgcode { get; set; }
        /// <summary>
        /// 核算组织编码
        /// </summary>
        public string? Fzaid { get; set; }
        /// <summary>
        /// 核算组织ID
        /// </summary>
        public string? Fzaorgno { get; set; }
        /// <summary>
        /// 版本
        /// </summary>
        public string? Fzversion { get; set; }
        /// <summary>
        /// 状态 0:停用;1:启用
        /// </summary>
        public string? Fzstate { get; set; }
        /// <summary>
        /// 是否删除 0:停用;1:启用
        /// </summary>
        public string? Fzdelete { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreatedAt { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
    }
}
