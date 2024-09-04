namespace GDCMasterDataReceiveApi.Domain.Shared
{
    /// <summary>
    /// mdm通用请求基础数据
    /// </summary>
    public class RequestResult<T>
    {
        /// <summary>
        /// 目标系统代码列表
        /// </summary>
        public List<string> IT_DEST_SYS_LIST { get; set; }
        /// <summary>
        /// 请求头
        /// </summary>
        public HeaderRequest IS_REQ_HEAD_ASYNC { get; set; }
        /// <summary>
        /// 数据接收
        /// </summary>
        public List<T>? IT_DATA { get; set; }
    }
    /// <summary>
    /// 请求头
    /// </summary>
    public class HeaderRequest
    {
        /// <summary>
        /// 接口请求唯一ID
        /// </summary>
        public string ZINSTID { get; set; }
        /// <summary>
        /// 调用接口的系统时间 格式：(yyyyMMddHHmmss)
        /// </summary>
        public string ZZREQTIME { get; set; }
        /// <summary>
        /// 来源系统代码
        /// </summary>
        public string ZZSRC_SYS { get; set; }
        /// <summary>
        /// 交易时间  用于OSB监控，值与ZZREQTIME相同，但是格式不同。格式：(yyyy-MM-ddTHH:mm:ss) 2014-06-23T13:57:30
        /// </summary>
        public string ZZATTR1 { get; set; }
        /// <summary>
        /// OSB异步分发的目标系统  异步分发数据时，OSB返回异步分发的结果，在此参数内填写OSB分发的目标系统。
        /// </summary>
        public string? ZZATTR2 { get; set; }
        /// <summary>
        /// 备用字段3
        /// </summary>
        public string? ZZATTR3 { get; set; }
        /// <summary>
        /// 主数据项
        /// </summary>
        public string? ZZOBJECT { get; set; }
    }
}