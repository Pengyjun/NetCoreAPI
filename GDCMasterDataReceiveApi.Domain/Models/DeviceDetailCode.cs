using SqlSugar;
using System.ComponentModel.DataAnnotations.Schema;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 物资设备明细编码
    /// </summary>
    [SugarTable("t_devicedetailcode", IsDisabledDelete = true)]
    public class DeviceDetailCode : BaseEntity<long>
    {
        /// <summary>
        /// 发送记录ID 发送记录的ID，必须保证此ID在同一个发送批次中是唯一的。用于记录发送方对于此发送记录的唯一标识。
        /// </summary>
        [NotMapped]
        public string ZZSERIAL { get; set; }
        /// <summary>
        /// 物资设备主数据编码
        /// </summary>
        [SugarColumn(Length = 11, ColumnName = "MaterialAndEquipmentMDCode")]
        public string ZMATERIAL { get; set; }
        /// <summary>
        /// 品名编码:物资设备的品名分类码
        /// </summary>
        [SugarColumn(Length = 11, ColumnName = "ProductNameCode")]
        public string ZCLASS { get; set; }
        /// <summary>
        /// 物资设备全称:物资设备规范的名称
        /// </summary>
        [SugarColumn(Length = 100, ColumnName = "MaterialAndEquipmentName")]
        public string ZMNAME { get; set; }
        /// <summary>
        /// 物资设备说明:物资设备的说明
        /// </summary>
        [SugarColumn(Length = 100, ColumnName = "MaterialAndEquipmentRemark")]
        public string ZMNAMES { get; set; }
        /// <summary>
        /// 物资设备主数据状态:物资设备主数据的使用状态
        /// </summary>
        [SugarColumn(Length = 1, ColumnName = "State")]
        public string ZSTATE { get; set; }
        /// <summary>
        /// 是否常用编码:0否，1是
        /// </summary>
        [SugarColumn(Length = 1, ColumnName = "IsEncodingCommonlyUsed")]
        public string? ZOFTENCODE { get; set; }
        /// <summary>
        /// 备注:备注说明
        /// </summary>
        [SugarColumn(Length = 300, ColumnName = "Remark")]
        public string? ZREMARK { get; set; }
        /// <summary>
        /// 物资设备属性列表
        /// </summary>
        [NotMapped]
        public List<ZMDGTT_MATATTR_DATA_IF>? ZMATTTR_LIST { get; set; }
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
