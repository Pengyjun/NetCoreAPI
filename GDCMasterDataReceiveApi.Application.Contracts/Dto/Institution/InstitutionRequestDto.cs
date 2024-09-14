namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.Institution
{
    /// <summary>
    /// 
    /// </summary>
    public class InstitutionRequestDto : BaseRequestDto
    {
        /// <summary>
        /// 筛选条件json
        /// </summary>
        public string? FilterConditionJson { get; set; }
    }
    /// <summary>
    /// 用户条件反序列化实例
    /// </summary>
    public class DeserializeObjectInstitution
    {
        /// <summary>
        /// ids（首页下钻）
        /// </summary>
        public List<string>? Ids { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string? Name { get; set; }
    }
}
