using GDCMasterDataReceiveApi.Domain.OtherModels;
using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 物资设备分类编码
    /// </summary>
    [SugarTable("t_deviceclasscode", IsDisabledDelete = true)]
    public class DeviceClassCode : BaseEntity<long>
    {
        /// <summary>
        /// 发送记录ID 发送记录的ID，必须保证此ID在同一个发送批次中是唯一的。用于记录发送方对于此发送记录的唯一标识。
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string ZZSERIAL { get; set; }
        /// <summary>
        /// 分类编码: 分类的唯一性编码
        /// </summary>
        [SugarColumn(Length = 11, ColumnName = "Code")]
        public string ZCLASS { get; set; }
        /// <summary>
        /// 分类层级: 该分类名称对应的层级。分为1至4级。
        /// </summary>
        [SugarColumn(Length = 1, ColumnName = "Level")]
        public string ZCLEVEL { get; set; }
        /// <summary>
        /// 分类名称: 分类的唯一性编码
        /// </summary>
        [SugarColumn(Length = 100, ColumnName = "Name")]
        public string ZCNAME { get; set; }
        /// <summary>
        /// 分类别名:
        /// </summary>
        [SugarColumn(Length = 100, ColumnName = "AliasName")]
        public string? ZCALIAS { get; set; }
        /// <summary>
        /// 分类说明
        /// </summary>
        [SugarColumn(Length = 1000, ColumnName = "Description")]
        public string? ZCDESC { get; set; }
        /// <summary>
        /// 上级分类编码: 该分类的上级分类的编码，体现分类的层级结构。
        /// </summary>
        [SugarColumn(Length = 11, ColumnName = "SupCode")]
        public string? ZCLASSUP { get; set; }
        /// <summary>
        /// 计量单位: 物资设备分类的计量单位代码。
        /// </summary>
        [SugarColumn(Length = 3, ColumnName = "UnitOfMeasurement")]
        public string? ZMSEHI { get; set; }
        /// <summary>
        /// 使用状态: 该分类是否为正常使用状态，当分类正常使用时为“1”，分类停止使用时为“0”。
        /// </summary>
        [SugarColumn(Length = 1, ColumnName = "State")]
        public string ZUSSTATE { get; set; }
        /// <summary>
        /// 是否删除：该分类是否被标记删除，通过上游失效时间进行判断
        /// </summary>
        [SugarColumn(Length = 1, ColumnName = "DataIdentifier")]
        public string? ZDELETE { get; set; }
        /// <summary>
        /// 排序规则: 系统排序规则用于前台数据排序使用
        /// </summary>
        [SugarColumn(Length = 20, ColumnName = "SortRule")]
        public string? ZSORT { get; set; }
        /// <summary>
        /// 物资设备分类的属性列表
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public ZMDGS_PROPERTY? ZPROPERTY_LIST { get; set; }
        /// <summary>
        /// 物资设备分类属性值列表
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public ZMDGS_VALUE? ZVALUE_LIST { get; set; }
    }
}
