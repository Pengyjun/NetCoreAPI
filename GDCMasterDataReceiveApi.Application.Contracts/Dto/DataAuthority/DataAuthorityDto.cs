namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.DataAuthority
{
    /// <summary>
    /// 可查看字段dto
    /// </summary>
    public class DataAuthorityDto
    {
        /// <summary>
        /// id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 授权的字段数组
        /// </summary>
        public List<string>? AuthorityColumns { get; set; }
    }
}
