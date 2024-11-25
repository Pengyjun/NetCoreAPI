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
        /// 数据是否下钻
        /// </summary>
        public bool IsDrilldown { get; set; } = false;
        /// <summary>
        /// 下钻日期
        /// </summary>
        public DateTime? DrilldownDate { get; set; }
        /// <summary>
        /// 动态搜索条件
        /// </summary>
        public List<JsonToSqlRequestDto>? JsonToSqlRequestDtos { get; set; }

    }
}
