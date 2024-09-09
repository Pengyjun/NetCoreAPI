namespace GDCMasterDataReceiveApi.Domain.Shared
{
    /// <summary>
    /// 公用条件类
    /// </summary>
    public class Condition
    { }
    /// <summary>
    /// 条件数组
    /// </summary>
    public class FilterParams
    {
        /// <summary>
        /// 中文键
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }
    }
    /// <summary>
    /// 加密参数对象
    /// </summary>
    public class Encryption
    {
        /// <summary>
        /// 键
        /// </summary>
        public string? Key { set; get; }
        /// <summary>
        /// 值
        /// </summary>
        public string? Value { set; get; }
    }
}
