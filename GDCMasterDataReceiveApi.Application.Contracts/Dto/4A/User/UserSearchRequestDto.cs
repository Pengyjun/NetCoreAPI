namespace GDCMasterDataReceiveApi.Application.Contracts.Dto._4A.User
{
    /// <summary>
    /// 用户列表请求dto
    /// </summary>
    public class UserSearchRequestDto : BaseRequestDto
    {
        /// <summary>
        /// 筛选条件json
        /// </summary>
        public string? FilterConditionJson { get; set; }
    }
    /// <summary>
    /// 用户条件反序列化实例
    /// </summary>
    public class DeserializeObjectUser
    {
        /// <summary>
        /// 用户ids（首页下钻）
        /// </summary>
        public List<string>? Ids { get; set; }
    }
}
