using SqlSugar;
using System.ComponentModel.DataAnnotations.Schema;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 大洲
    /// </summary>
    [SugarTable("t_countrycontinent", IsDisabledDelete = true)]
    public class CountryContinent : BaseEntity<long>
    {
        /// <summary>
        /// 发送记录ID 发送记录的ID，必须保证此ID在同一个发送批次中是唯一的。用于记录发送方对于此发送记录的唯一标识。
        /// </summary>
        [NotMapped]
        public string ZZSERIAL { get; set; }
        /// <summary>
        /// 大洲代码：大洲代码
        /// </summary>
        [SugarColumn(Length = 12, ColumnName = "ContinentCode")]
        public string ZCONTINENTCODE { get; set; }
        /// <summary>
        /// 大洲名称：大洲名称
        /// </summary>
        [SugarColumn(Length = 12, ColumnName = "ContinentName")]
        public string ZCONTINENTNAME { get; set; }
        /// <summary>
        /// 区域代码：大洲所属区域代码
        /// </summary>
        [SugarColumn(Length = 6, ColumnName = "AreaCode")]
        public string ZAREACODE { get; set; }
        /// <summary>
        /// 区域描述：区域描述
        /// </summary>
        [SugarColumn(Length = 60, ColumnName = "RegionalDescriptors")]
        public string ZAREANAME { get; set; }
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
        public List<ZMDGS_ZLANG>? ZLANG_LIST { get; set; }
    }
    /// <summary>
    /// 多语言描述表类型
    /// </summary>
    public class ZMDGS_ZLANG
    {
        /// <summary>
        /// 语种代码
        /// </summary>
        public string? ZLANGCODE { get; set; }
        /// <summary>
        /// 大洲代码描述
        /// </summary>
        public string? ZCODE_DESC { get; set; }
        /// <summary>
        /// 区域代码描述
        /// </summary>
        public string? ZAREA_DESC { get; set; }
    }
}
