using GDCMasterDataReceiveApi.Application.Contracts.Dto.OtherModels;
using MiniExcelLibs.Attributes;

namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.UnitMeasurement
{
    /// <summary>
    /// 常用计量单位 反显
    /// </summary>
    public class UnitMeasurementSearchDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public string? Id { get; set; }
        /// <summary>
        /// 计量单位代码:业务主键
        /// </summary>
        public string? Code { get; set; }
        /// <summary>
        /// 计量单位名称:计量单位的名称或说明，一般采用中文或常用符号。
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 状态：数据是否有效的标识:有效：1无效：0
        /// </summary>
        public string? State { get; set; }
        /// <summary>
        /// 是否删除 数据是否有效的标识:有效：1无效：0
        /// </summary>
        public string? DataIdentifier { get; set; }
    }
    /// <summary>
    /// 计量单位详情
    /// </summary>
    public class UnitMeasurementDetailsDto
    {
        [ExcelIgnore]
        public DateTime? CreateTime { get; set; }
        [ExcelIgnore]
        public DateTime? UpdateTime { get; set; }
        [ExcelIgnore]
        public string? Id { get; set; }
        /// <summary>
        /// 计量单位代码:业务主键
        /// </summary>
        [ExcelColumnName("计量单位代码")]
        public string? Code { get; set; }
        /// <summary>
        /// 计量单位名称:计量单位的名称或说明，一般采用中文或常用符号。
        /// </summary>
        [ExcelColumnName("计量单位名称")]
        public string? Name { get; set; }
        /// <summary>
        /// 版本：数据的版本号。数据每次变更时，版本号自动加1。
        /// </summary>
        [ExcelIgnore]
        public string? Version { get; set; }
        /// <summary>
        /// 状态：数据是否有效的标识:有效：1无效：0
        /// </summary>
        [ExcelIgnore]
        public string? State { get; set; }
        /// <summary>
        /// 是否删除 数据是否有效的标识:有效：1无效：0
        /// </summary>
        [ExcelIgnore]
        public string? DataIdentifier { get; set; }
    }
    /// <summary>
    /// 常用计量单位 接收
    /// </summary>
    public class UnitMeasurementItem 
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
        /// 计量单位代码:业务主键
        /// </summary>
        public string? ZUNITCODE { get; set; }
        /// <summary>
        /// 计量单位名称:计量单位的名称或说明，一般采用中文或常用符号。
        /// </summary>
        public string? ZUNITNAME { get; set; }
        /// <summary>
        /// 计量单位名称（其它语言的集合）
        /// </summary>
        public UnitMeasurementModels? ZUNIT_LANG { get; set; }
        /// <summary>
        /// 版本：数据的版本号。数据每次变更时，版本号自动加1。
        /// </summary>
        public string? ZVERSION { get; set; }
        /// <summary>
        /// 状态：数据是否有效的标识:有效：1无效：0
        /// </summary>
        public string? ZSTATE { get; set; }
        /// <summary>
        /// 是否删除 数据是否有效的标识:有效：1无效：0
        /// </summary>
        public string? ZDELETE { get; set; }
    }
}
