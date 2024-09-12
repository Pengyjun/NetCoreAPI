namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.OtherModels
{
    /// <summary>
    /// 物资设备其他需要处理的models
    /// </summary>
    public class DeviceModels
    { }
    /// <summary>
    /// 物资设备分类的属性列表
    /// </summary>
    public class ZMDGS_PROPERTY
    {
        public List<ClassDevice> Item { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ClassDevice
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id {  get; set; }
        /// <summary>
        /// 11
        /// 分类编码:分类的唯一性编码
        /// </summary>
        public string ZCLASS { get; set; }
        /// <summary>
        /// 属性编码:属性唯一代码
        /// 10
        /// </summary>
        public string ZATTRCODE { get; set; }
        /// <summary>
        /// 属性名称
        /// 100
        /// </summary>
        public string ZATTRNAME { get; set; }
        /// <summary>
        /// 属性计量单位
        /// 50
        /// </summary>
        public string ZATTRUNIT { get; set; }
        /// <summary>
        /// 备注
        /// 300
        /// </summary>
        public string? ZREMARK { get; set; }
    }
    /// <summary>
    /// 物资设备分类属性值列表
    /// </summary>
    public class ZMDGS_VALUE
    {
        public List<ClassDeviceValue>? Item { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ClassDeviceValue
    {
        /// <summary>
        /// 分类编码: 分类的唯一性编码
        /// 11
        /// </summary>
        public string ZCLASS { get; set; }
        /// <summary>
        /// 属性编码: 属性唯一代码
        /// 10 
        /// </summary>
        public string ZATTRCODE { get; set; }
        /// <summary>
        /// 属性值编码: 属性值唯一代码
        /// 10
        /// </summary>
        public string ZVALUECODE { get; set; }
        /// <summary>
        /// 属性值名称
        /// 100
        /// </summary>
        public string ZVALUENAME { get; set; }
    }
    /// <summary>
    /// 物资设备属性列表
    /// </summary>
    public class ZMDGTT_MATATTR_DATA_IF
    {
        /// <summary>
        /// 物资设备主数据编码
        /// 11
        /// </summary>
        public string ZMATERIAL { set; get; }
        /// <summary>
        /// 属性编码
        /// 10
        /// </summary>
        public string ZATTRCODE { set; get; }
        /// <summary>
        /// 属性名称
        /// 100
        /// </summary>
        public string ZATTRNAME { set; get; }
        /// <summary>
        /// 属性值编码
        /// 10
        /// </summary>
        public string ZVALUECODE { set; get; }
        /// <summary>
        /// 属性值名称
        /// 100
        /// </summary>
        public string ZVALUENAME { set; get; }
        /// <summary>
        /// 属性计量单位
        /// 50
        /// </summary>
        public string ZATTRUNIT { set; get; }
    }
}
