namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.LouDong
{
    /// <summary>
    /// 入参请求实体
    /// </summary>
    public class LouDongRequestDto : BaseRequestDto
    {
        /// <summary>
        /// 查询json
        /// </summary>
        public string Sql { get; set; }
        /// <summary>
        /// 参数
        /// </summary>
        public List<FilterParams>? FilterParams { get; set; }
    }
    /// <summary>
    /// 筛选对象
    /// </summary>
    public class FilterParams
    {
        /// <summary>
        /// 字段名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 查询的条件值
        /// </summary>
        public object Value { get; set; }
        /// <summary>
        /// 条件符号 > = like < 等
        /// </summary>
        public string Condition { get; set; }
        /// <summary>
        /// 逻辑操作  and  还是 or
        /// </summary>
        public string? LogicOperator { get; set; }
        /// <summary>
        /// 是否为开始括号 true开始
        /// </summary>
        public bool IsGroupStart { get; set; }
        /// <summary>
        /// 是否为结束括号 false
        /// </summary>
        public bool IsGroupEnd { get; set; }
    }
}
