namespace GDCMasterDataReceiveApi.Application.Contracts
{
    /// <summary>
    /// mdm通用响应基础数据
    /// </summary>
    public class ResponseResult
    {
        /// <summary>
        /// 响应信息
        /// </summary>
        public RETURN ES_RETURN { get; set; }
        /// <summary>
        /// 响应结果
        /// </summary>
        public List<RESULT> ET_RESULT { get; set; }
    }
    /// <summary>
    /// 响应信息
    /// </summary>
    public class RETURN
    {
        /// <summary>
        /// 接口请求唯一ID
        /// </summary>
        public string ZINSTID { get; set; }
        /// <summary>
        /// 响应时间  格式：(yyyyMMddHHmmss)
        /// </summary>
        public string ZZRESTIME { get; set; }
        /// <summary>
        /// 请求状态 状态：S（成功）/E（错误）如果整批数据处理成功，则返回成功；如果部分数据处理成功，则返回错误。
        /// </summary>
        public string ZZSTAT { get; set; }
        /// <summary>
        /// 请求响应消息  描述请求调用的情况，可以返回发送成功或错误数据的数量，也可以返回整批请求数据的错误描述
        /// </summary>
        public string? ZZMSG { get; set; }
        /// <summary>
        /// 备用字段1
        /// </summary>
        public string? ZZATTR1 { get; set; }
        /// <summary>
        /// 备用字段2
        /// </summary>
        public string? ZZATTR2 { get; set; }
        /// <summary>
        /// 备用字段3
        /// </summary>
        public string? ZZATTR3 { get; set; }
    }
    /// <summary>
    /// 响应结果
    /// </summary>
    public class RESULT
    {
        /// <summary>
        /// 发送记录ID
        /// </summary>
        public string ZZSERIAL { get; set; }
        /// <summary>
        /// 发送记录状态  返回状态：S（成功）/E（错误）
        /// </summary>
        public string ZZSTAT { get; set; }
        /// <summary>
        /// 详细记录的处理结果   如果数据处理产生错误，返回错误的描述；
        /// </summary>
        public string? ZZMSG { get; set; }
    }
}
