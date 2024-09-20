namespace GDCMasterDataReceiveApi.Application.Contracts.Dto
{
    /// <summary>
    /// 条件筛选相关
    /// </summary>
    public class FilterConditionDto
    {
        /// <summary>
        /// 列名  键
        /// </summary>
        public string? CoulmnKey { get; set; }
        /// <summary>
        /// 列名  值
        /// </summary>
        public string? CoulmnName { get; set; }
        /// <summary>
        /// 类型 ： 日期  单选按钮 复选...
        /// </summary>
        public string? Type { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public List<FilterChildData>? Options { get; set; }
    }
    /// <summary>
    /// 数据模型
    /// </summary>
    public class FilterChildData
    {
        /// <summary>
        /// 键
        /// </summary>
        public string? Key { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string? Val { get; set; }
        /// <summary>
        /// 值域编码  不传
        /// </summary>
        public string? Code { get; set; }
        /// <summary>
        /// 值域描述 不传
        /// </summary>
        public string? Desc { get; set; }
    }
}
