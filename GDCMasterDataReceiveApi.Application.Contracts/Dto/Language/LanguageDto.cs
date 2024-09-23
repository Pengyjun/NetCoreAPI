using GDCMasterDataReceiveApi.Application.Contracts.Dto.OtherModels;
using MiniExcelLibs.Attributes;

namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.Language
{
    /// <summary>
    /// 语言语种 反显
    /// </summary>
    public class LanguageSearchDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public string? Id { get; set; }
        /// <summary>
        /// GB/T 4880.2/B目录代码:GB/T 4880.2/B目录代码
        /// </summary>
        public string? DirCode { get; set; }
        /// <summary>
        /// GB/T 4880.2/T术语代码:GB/T 4880.2/T术语代码
        /// </summary>
        public string? TermCode { get; set; }
        /// <summary>
        /// 汉语名称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 英语名称
        /// </summary>
        public string? EnglishName { get; set; }
        /// <summary>
        /// 状态：数据是否有效的标识:有效：1无效：0
        /// </summary>
        public string? State { get; set; }
    }
    /// <summary>
    /// 语言语种详情
    /// </summary>
    public class LanguageDetailsDto
    {
        [ExcelIgnore]
        public DateTime? CreateTime { get; set; }
        [ExcelIgnore]
        public DateTime? UpdateTime { get; set; }
        [ExcelIgnore]
        public string? Id { get; set; }
        /// <summary>
        /// GB/T 4880.2/B目录代码:GB/T 4880.2/B目录代码
        /// </summary>
        [ExcelColumnName("GB/T 4880.2/B目录代码:GB/T 4880.2/B目录代码")]
        public string? DirCode { get; set; }
        /// <summary>
        /// GB/T 4880.2/T术语代码:GB/T 4880.2/T术语代码
        /// </summary>
        [ExcelColumnName("GB/T 4880.2/T术语代码:GB/T 4880.2/T术语代码")]
        public string? TermCode { get; set; }
        /// <summary>
        /// 汉语名称
        /// </summary>
        [ExcelColumnName("汉语名称")]
        public string? Name { get; set; }
        /// <summary>
        /// 英语名称
        /// </summary>
        [ExcelColumnName("英语名称")]
        public string? EnglishName { get; set; }
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
    /// 语言语种 接收
    /// </summary>
    public class LanguageItem
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
        /// GB/T 4880.2/B目录代码:GB/T 4880.2/B目录代码
        /// </summary>
        public string? ZLANG_BIB { get; set; }
        /// <summary>
        /// GB/T 4880.2/T术语代码:GB/T 4880.2/T术语代码
        /// </summary>
        public string? ZLANG_TER { get; set; }
        /// <summary>
        /// 汉语名称
        /// </summary>
        public string? ZLANG_ZH { get; set; }
        /// <summary>
        /// 英语名称
        /// </summary>
        public string? ZLANG_EN { get; set; }
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
        /// <summary>
        /// 多语言描述表类型
        /// </summary>
        public ZMDGTT_ZLANG2? ZLANG_LIST { get; set; }
    }
}
