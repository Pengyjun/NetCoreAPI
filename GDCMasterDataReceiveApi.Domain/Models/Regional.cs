using SqlSugar;
using System.ComponentModel.DataAnnotations.Schema;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 中交区域总部
    /// </summary>
    [SugarTable("t_regional", IsDisabledDelete = true)]
    public class Regional : BaseEntity<long>
    {
        /// <summary>
        /// 发送记录ID 发送记录的ID，必须保证此ID在同一个发送批次中是唯一的。用于记录发送方对于此发送记录的唯一标识。
        /// </summary>
        [NotMapped]
        public string ZZSERIAL { get; set; }
        /// <summary>
        /// 中交区域总部代码:中交区域中心编码
        /// </summary>
        [SugarColumn(Length = 2, ColumnName = "CodeOfCCCCRegionalHeadquarters")]
        public string ZCRHCODE { get; set; }
        /// <summary>
        /// 中交区域总部描述:编码描述
        /// </summary>
        [SugarColumn(Length = 100, ColumnName = "DescriptionOfCCCCRegionalHeadquarters")]
        public string ZCRHNAME { get; set; }
        /// <summary>
        /// 简称:编码描述简称
        /// </summary>
        [SugarColumn(Length = 100, ColumnName = "ShortName")]
        public string ZCRHABBR { get; set; }
        /// <summary>
        /// 区域范围:中交区域中心管辖范围
        /// </summary>
        [SugarColumn(Length = 100, ColumnName = "AreaRange")]
        public string ZCRHSCOPE { get; set; }
        /// <summary>
        /// 版本：数据的版本号。数据每次变更时，版本号自动加1。
        /// </summary>
        [SugarColumn(ColumnDataType = "int", Length = 10, ColumnName = "Version")]
        public string ZVERSION { get; set; }
        /// <summary>
        /// 状态：数据是否有效的标识:有效：1无效：0
        /// </summary>
        [SugarColumn(Length = 1, ColumnName = "State")]
        public string ZSTATE { get; set; }
        /// <summary>
        /// 是否删除 数据是否有效的标识:有效：1无效：0
        /// </summary>
        [SugarColumn(Length = 1, ColumnName = "IsDeleteValidIdentifier")]
        public string ZDELETE { get; set; }
        /// <summary>
        /// 多语言描述表类型
        /// </summary>
        [NotMapped]
        public List<ZMDGTT_ZLANG3>? ZLANG_LIST { get; set; }
    }
    /// <summary>
    /// 多语言描述表类型
    /// </summary>
    public class ZMDGTT_ZLANG3
    {
        /// <summary>
        /// 语种代码
        /// </summary>
        public string? ZLANGCODE { get; set; }
        /// <summary>
        /// 编码描述:ZCRHCODE中交区域总部代码多语言描述
        /// </summary>
        public string? ZCODE_DESC { get; set; }
        /// <summary>
        /// 简称:ZCRHABBR简称
        /// </summary>
        public string? ZSCRTEXT_S { get; set; }
    }
}
