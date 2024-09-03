using GDCMasterDataReceiveApi.Domain.Models;

namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.DeviceClassCode
{
    /// <summary>
    /// 物资设备分类编码 反显
    /// </summary>
    public class DeviceClassCodeDto
    {
        /// <summary>
        /// 分类编码: 分类的唯一性编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 分类层级: 该分类名称对应的层级。分为1至4级。
        /// </summary>
        public string Level { get; set; }
        /// <summary>
        /// 分类名称: 分类的唯一性编码
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 分类别名:
        /// </summary>
        public string? AliasName { get; set; }
        /// <summary>
        /// 分类说明
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// 上级分类编码: 该分类的上级分类的编码，体现分类的层级结构。
        /// </summary>
        public string? SupCode { get; set; }
        /// <summary>
        /// 计量单位: 物资设备分类的计量单位代码。
        /// </summary>
        public string? UnitOfMeasurement { get; set; }
        /// <summary>
        /// 使用状态: 该分类是否为正常使用状态，当分类正常使用时为“1”，分类停止使用时为“0”。
        /// </summary>
        public string State { get; set; }
        /// <summary>
        /// 是否删除：该分类是否被标记删除，通过上游失效时间进行判断
        /// </summary>
        public string? DataIdentifier { get; set; }
        /// <summary>
        /// 排序规则: 系统排序规则用于前台数据排序使用
        /// </summary>
        public string? SortRule { get; set; }
    }
    /// <summary>
    /// 物资设备分类编码 接收
    /// </summary>
    public class DeviceClassCodeReceiveDto
    {
        /// <summary>
        /// 发送记录ID 发送记录的ID，必须保证此ID在同一个发送批次中是唯一的。用于记录发送方对于此发送记录的唯一标识。
        /// </summary>
        public string ZZSERIAL { get; set; }
        /// <summary>
        /// 分类编码: 分类的唯一性编码
        /// </summary>
        public string ZCLASS { get; set; }
        /// <summary>
        /// 分类层级: 该分类名称对应的层级。分为1至4级。
        /// </summary>
        public string ZCLEVEL { get; set; }
        /// <summary>
        /// 分类名称: 分类的唯一性编码
        /// </summary>
        public string ZCNAME { get; set; }
        /// <summary>
        /// 分类别名:
        /// </summary>
        public string? ZCALIAS { get; set; }
        /// <summary>
        /// 分类说明
        /// </summary>
        public string? ZCDESC { get; set; }
        /// <summary>
        /// 上级分类编码: 该分类的上级分类的编码，体现分类的层级结构。
        /// </summary>
        public string? ZCLASSUP { get; set; }
        /// <summary>
        /// 计量单位: 物资设备分类的计量单位代码。
        /// </summary>
        public string? ZMSEHI { get; set; }
        /// <summary>
        /// 使用状态: 该分类是否为正常使用状态，当分类正常使用时为“1”，分类停止使用时为“0”。
        /// </summary>
        public string ZUSSTATE { get; set; }
        /// <summary>
        /// 是否删除：该分类是否被标记删除，通过上游失效时间进行判断
        /// </summary>
        public string? ZDELETE { get; set; }
        /// <summary>
        /// 排序规则: 系统排序规则用于前台数据排序使用
        /// </summary>
        public string? ZSORT { get; set; }
        /// <summary>
        /// 物资设备分类的属性列表
        /// </summary>
        public List<ZMDGS_PROPERTY>? ZPROPERTY_LIST { get; set; }
        /// <summary>
        /// 物资设备分类属性值列表
        /// </summary>
        public List<ZMDGS_VALUE>? ZVALUE_LIST { get; set; }
    }
}
