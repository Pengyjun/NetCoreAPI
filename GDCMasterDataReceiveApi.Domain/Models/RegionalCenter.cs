using GDCMasterDataReceiveApi.Domain.OtherModels;
using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 中交区域中心
    /// </summary>
    [SugarTable("t_regionalcenter", IsDisabledDelete = true)]
    public class RegionalCenter : BaseEntity<long>
    {
        /// <summary>
        /// 发送记录ID 发送记录的ID，必须保证此ID在同一个发送批次中是唯一的。用于记录发送方对于此发送记录的唯一标识。
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string? ZZSERIAL { get; set; }
        /// <summary>
        /// 中交区域中心代码:中交区域中心编码
        /// </summary>
        [SugarColumn(Length = 6, ColumnName = "Code")]
        public string? ZCRCCODE { get; set; }
        /// <summary>
        /// 中交区域中心描述:编码描述
        /// </summary>
        [SugarColumn(Length = 100, ColumnName = "Description")]
        public string? ZCRCNAME { get; set; }
        /// <summary>
        /// 版本
        /// </summary>
        [SugarColumn(ColumnDataType = "int", Length = 10, ColumnName = "Version")]
        public string? ZVERSION { get; set; }
        /// <summary>
        /// 状态：数据是否有效的标识:有效：1无效：0
        /// </summary>
        [SugarColumn(Length = 1, ColumnName = "State")]
        public string? ZSTATE { get; set; }
        /// <summary>
        /// 是否删除 数据是否有效的标识:有效：1无效：0
        /// </summary>
        [SugarColumn(Length = 1, ColumnName = "DataIdentifier")]
        public string? ZDELETE { get; set; }
        /// <summary>
        /// 多语言描述表类型
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<ZMDGS_ZLANG4>? ZMDGTT_ZLANG { get; set; }
    }
}
