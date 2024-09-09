namespace GDCMasterDataReceiveApi.Application.Contracts.Dto._4A.User
{
    /// <summary>
    /// 用户列表请求dto
    /// </summary>
    public class UserSearchRequestDto : BaseRequestDto
    {
        /// <summary>
        /// 拼接的 条件 AES加密文本
        /// </summary>
        //public string? EncryptedText {  get; set; }
        public List<string>? FilterConditions { get; set; }
    }
}
