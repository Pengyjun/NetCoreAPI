using GDCMasterDataReceiveApi.Application.Contracts.Dto.OtherModels;
using System.ComponentModel;

namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.DeviceDetailCode
{
    /// <summary>
    /// 物资设备明细编码 反显
    /// </summary>
    public class DeviceDetailCodeSearchDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public string? Id { get; set; }
        /// <summary>
        /// 物资设备主数据编码
        /// </summary>
        public string? MDCode { get; set; }
        /// <summary>
        /// 品名编码:物资设备的品名分类码
        /// </summary>
        public string? ProductNameCode { get; set; }
        /// <summary>
        /// 物资设备全称:物资设备规范的名称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 物资设备说明:物资设备的说明
        /// </summary>
        public string? Descption { get; set; }
    }
    /// <summary>
    /// 物资设备明细编码详细
    /// </summary>
    public class DeviceDetailCodeDetailsDto
    {
        public string? Id { get; set; }
        /// <summary>
        /// 物资设备主数据编码
        /// </summary>
        [Description("ZMATERIAL")]
        [DisplayName("物资设备主数据编码")]
        public string? MDCode { get; set; }
        /// <summary>
        /// 品名编码:物资设备的品名分类码
        /// </summary>
        [Description("ZCLASS")]
        [DisplayName("品名编码")]
        public string? ProductNameCode { get; set; }
        /// <summary>
        /// 物资设备全称:物资设备规范的名称
        /// </summary>
        [Description("ZMNAME")]
        [DisplayName("物资设备全称")]
        public string? Name { get; set; }
        /// <summary>
        /// 物资设备说明:物资设备的说明
        /// </summary>
        [Description("ZMNAMES")]
        [DisplayName("物资设备说明")]
        public string? Descption { get; set; }
        /// <summary>
        /// 物资设备主数据状态:物资设备主数据的使用状态
        /// </summary>
        [Description("ZSTATE")]
        [DisplayName("物资设备主数据状态")]
        public string? State { get; set; }
        /// <summary>
        /// 是否常用编码:0否，1是
        /// </summary>
        [Description("ZOFTENCODE")]
        [DisplayName("是否常用编码")]
        public string? IsCode { get; set; }
        /// <summary>
        /// 备注:备注说明
        /// </summary>
        [Description("ZREMARK")]
        [DisplayName("备注")]
        public string? Remark { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
    /// <summary>
    /// 物资设备明细编码 接收
    /// </summary>
    public class DeviceDetailCodeItem
    {
        /// <summary>
        /// 
        /// </summary>
        public long? Id { get; set; }
        /// <summary>
        /// 发送记录ID 发送记录的ID，必须保证此ID在同一个发送批次中是唯一的。用于记录发送方对于此发送记录的唯一标识。
        /// </summary>
        public string? ZZSERIAL { get; set; }
        /// <summary>
        /// 物资设备主数据编码
        /// </summary>
        public string? ZMATERIAL { get; set; }
        /// <summary>
        /// 品名编码:物资设备的品名分类码
        /// </summary>
        public string? ZCLASS { get; set; }
        /// <summary>
        /// 物资设备全称:物资设备规范的名称
        /// </summary>
        public string? ZMNAME { get; set; }
        /// <summary>
        /// 物资设备说明:物资设备的说明
        /// </summary>
        public string? ZMNAMES { get; set; }
        /// <summary>
        /// 物资设备主数据状态:物资设备主数据的使用状态
        /// </summary>
        public string? ZSTATE { get; set; }
        /// <summary>
        /// 是否常用编码:0否，1是
        /// </summary>
        public string? ZOFTENCODE { get; set; }
        /// <summary>
        /// 备注:备注说明
        /// </summary>
        public string? ZREMARK { get; set; }
        /// <summary>
        /// 物资设备明细属性列表
        /// </summary>
        public ZMATTTR_LIST? ZMATTTR_LIST { get; set; }
    }
}
