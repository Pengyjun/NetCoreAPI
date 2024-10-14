using GDCMasterDataReceiveApi.Application.Contracts.Dto.OtherModels;
using MiniExcelLibs.Attributes;

namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.RegionalCenter
{
    /// <summary>
    /// 中交区域中心 反显
    /// </summary>
    public class RegionalCenterSearchDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public string? Id { get; set; }
        /// <summary>
        /// 中交区域中心代码:中交区域中心编码
        /// </summary>
        public string? Code { get; set; }
        /// <summary>
        /// 中交区域中心描述:编码描述
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// 状态：数据是否有效的标识:有效：1无效：0
        /// </summary>
        public string? State { get; set; }
    }
    /// <summary>
    /// 中交区域中心 详细
    /// </summary>
    public class RegionalCenterDetailsDto
    {

        [ExcelIgnore]
        public string? Id { get; set; }
        /// <summary>
        /// 中交区域中心代码:中交区域中心编码
        /// </summary>
        [ExcelColumnName("中交区域中心代码")]
        public string? Code { get; set; }
        /// <summary>
        /// 中交区域中心描述:编码描述
        /// </summary>
        [ExcelColumnName("中交区域中心描述")]
        public string? Description { get; set; }
        /// <summary>
        /// 版本
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
        [ExcelIgnore]
        public DateTime? CreateTime { get; set; }
        [ExcelIgnore]
        public DateTime? UpdateTime { get; set; }
    }
    /// <summary>
    /// 中交区域中心 接收
    /// </summary>
    public class RegionalCenterItem
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
        /// 中交区域中心代码:中交区域中心编码
        /// </summary>
        public string? ZCRCCODE { get; set; }
        /// <summary>
        /// 中交区域中心描述:编码描述
        /// </summary>
        public string? ZCRCNAME { get; set; }
        /// <summary>
        /// 版本
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
        /// <summary>
        /// 多语言描述表类型
        /// </summary>
        public RegionalCenterModels? ZLANG_LIST { get; set; }
    }
}
