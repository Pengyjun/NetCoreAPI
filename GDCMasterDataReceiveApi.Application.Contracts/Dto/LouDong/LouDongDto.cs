namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.LouDong
{
    /// <summary>
    /// 楼栋反显Dto
    /// </summary>
    public class LouDongDto
    {
        /// <summary>
        /// 发送记录ID 发送记录的ID，必须保证此ID在同一个发送批次中是唯一的。用于记录发送方对于此发送记录的唯一标识。
        /// </summary>
        public string? ZZSERIAL { get; set; }
        /// <summary>
        /// 楼栋编码：楼栋主数据编码由1位字母和4位数字组成，采用标识符+流水号的编码规则,如：B0001
        /// </summary>
        public string? Code { get; set; }
        /// <summary>
        /// 楼栋名称：同一分期（子项目）下的楼栋唯一标识，同一个分期（子项目）下楼栋名称不允许重复
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 业态信息 ：该楼栋的业态，可多值，需选择末级；通用类字典102接口下发 多值用英文逗号隔开
        /// </summary>
        public string? BFormat { get; set; }
        /// <summary>
        /// 项目主数据编码 ：该楼栋所属的分期（子项目）编码，如无分期（子项目）可填主项目的编码
        /// </summary>
        public string? PjectMDCode { get; set; }
        /// <summary>
        /// 状态：(0:无效,1:有效)
        /// </summary>
        public string? State { get; set; }
        /// <summary>
        /// 来源系统
        /// </summary>
        public string? SourceSystem { get; set; }
    }
    /// <summary>
    /// 楼栋  接收
    /// </summary>

    public class LouDongReceiveDto
    {
        /// <summary>
        /// 发送记录ID 发送记录的ID，必须保证此ID在同一个发送批次中是唯一的。用于记录发送方对于此发送记录的唯一标识。
        /// </summary>
        public string ZZSERIAL { get; set; }
        /// <summary>
        /// 楼栋编码：楼栋主数据编码由1位字母和4位数字组成，采用标识符+流水号的编码规则,如：B0001
        /// </summary>
        public string ZBLDG_NAME { get; set; }
        /// <summary>
        /// 楼栋名称：同一分期（子项目）下的楼栋唯一标识，同一个分期（子项目）下楼栋名称不允许重复
        /// </summary>
        public string ZBLDG { get; set; }
        /// <summary>
        /// 业态信息 ：该楼栋的业态，可多值，需选择末级；通用类字典102接口下发 多值用英文逗号隔开
        /// </summary>
        public string ZFORMATINF { get; set; }
        /// <summary>
        /// 项目主数据编码 ：该楼栋所属的分期（子项目）编码，如无分期（子项目）可填主项目的编码
        /// </summary>
        public string ZPROJECT { get; set; }
        /// <summary>
        /// 状态：(0:无效,1:有效)
        /// </summary>
        public string ZSTATE { get; set; }
        /// <summary>
        /// 来源系统
        /// </summary>
        public string ZSYSTEM { get; set; }
    }
}
