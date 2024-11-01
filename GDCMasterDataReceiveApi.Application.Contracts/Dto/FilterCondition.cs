namespace GDCMasterDataReceiveApi.Application.Contracts.Dto
{
    /// <summary>
    /// 筛选json
    /// </summary>
    public class FilterCondition : BaseRequestDto
    {
        /// <summary>
        /// 筛选条件json
        /// </summary>
         public string? FilterConditionJson { get; set; }
        /// <summary>
        /// 动态搜索条件
        /// </summary>
        public List<JsonToSqlRequestDto>?  JsonToSqlRequestDtos { get; set; }

    }
}
