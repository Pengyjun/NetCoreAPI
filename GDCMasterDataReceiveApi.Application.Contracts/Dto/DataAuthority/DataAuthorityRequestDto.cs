namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.DataAuthority
{
    /// <summary>
    /// 可查看字段 列表请求dto  反显
    /// </summary>
    public class DataAuthoritySearchRequestDto : BaseRequestDto
    {

    }
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
    /// <summary>
    /// 可查看字段请求dto
    /// </summary>
    public class DataAuthorityRequestDto
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public long? UId { get; set; }
        /// <summary>
        /// 角色id
        /// </summary>
        public string RId { get; set; }
        /// <summary>
        /// 机构id
        /// </summary>
        public long InstutionId { get; set; }
        /// <summary>
        /// 部门id
        /// </summary>
        public long? DepId { get; set; }
        /// <summary>
        /// 项目id
        /// </summary>
        public long? PjectId { get; set; }
    }
    /// <summary>
    /// 新增/修改授权字段dto
    /// </summary>
    public class AddOrModifyDataAuthorityRequestDto
    {
        /// <summary>
        /// id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 拼接的字段串
        /// </summary>
        public string? Colums { get; set; }
        /// <summary>
        /// 当前操作人id
        /// </summary>
        public long UId { get; set; }
        /// <summary>
        /// 当前操作人角色id
        /// </summary>
        public string RId { get; set; }
        /// <summary>
        /// 当前操作人机构id
        /// </summary>
        public long InstutionId { get; set; }
        /// <summary>
        /// 当前操作人项目部id
        /// </summary>
        public long? DepId { get; set; }
        /// <summary>
        /// 当前操作人  自己属于的项目id
        /// </summary>
        public long? PjectId { get; set; }
    }
}
