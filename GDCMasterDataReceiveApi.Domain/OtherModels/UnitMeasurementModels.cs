namespace GDCMasterDataReceiveApi.Domain.OtherModels
{
    /// <summary>
    /// 计量单位其他models
    /// </summary>
    public class UnitMeasurementModels
    {
    }
    /// <summary>
    /// 计量单位名称（其它语言的集合）
    /// </summary>
    public class ZMDGTT_UNIT_LANG
    {
        /// <summary>
        /// 计量单位代码
        /// </summary>
        public string ZUNITCODE { get; set; }
        /// <summary>
        /// 语言代码：EN、ES	
        /// </summary>
        public string ZSPRAS { get; set; }
        /// <summary>
        /// 语言描述:
        /// 语言描述是对语言代码的说明：
        /// 英语: EN
        /// 西班牙语：ES
        /// </summary>
        public string ZSPTXT { get; set; }
        /// <summary>
        /// 单位描述:计量单位的名称或说明。
        /// </summary>
        public string? ZUNITDESCR { get; set; }
    }
}
