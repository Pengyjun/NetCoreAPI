using GDCMasterDataReceiveApi.Domain.OtherModels;
using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 国家地区
    /// </summary>
    [SugarTable("t_countryregion", IsDisabledDelete = true)]
    public class CountryRegion : BaseEntity<long>
    {
        /// <summary>
        /// 发送记录ID 发送记录的ID，必须保证此ID在同一个发送批次中是唯一的。用于记录发送方对于此发送记录的唯一标识。
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string ZZSERIAL { get; set; }
        /// <summary>
        /// 国家地区代码
        /// </summary>
        [SugarColumn(Length = 3, ColumnName = "Country")]
        public string ZCOUNTRYCODE { get; set; }
        /// <summary>
        /// 中文名称
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "Name")]
        public string ZCOUNTRYNAME { get; set; }
        /// <summary>
        /// 英文名称
        /// </summary>
        [SugarColumn(Length = 100, ColumnName = "NameEnglish")]
        public string? ZCOUNTRYENAME { get; set; }
        /// <summary>
        /// 国标三字符代码
        /// </summary>
        [SugarColumn(Length = 3, ColumnName = "NationalCode")]
        public string? ZGBCHAR { get; set; }
        /// <summary>
        /// 国标数字代码
        /// </summary>
        [SugarColumn(Length = 3, ColumnName = "DigitCode")]
        public string? ZGBNUM { get; set; }
        /// <summary>
        /// 大洲代码
        /// </summary>
        [SugarColumn(Length = 3, ColumnName = "ContinentCode")]
        public string? ZCONTINENTCODE { get; set; }
        /// <summary>
        /// 中交区域中心代码
        /// </summary>
        [SugarColumn(Length = 6, ColumnName = "CCCCCenterCode")]
        public string? ZCRCCODE { get; set; }
        /// <summary>
        /// 版本号:数据的版本号。数据每次变更时，版本号自动加1。
        /// </summary>
        [SugarColumn(ColumnDataType = "int", Length = 10, ColumnName = "Version")]
        public int ZVERSION { get; set; }
        /// <summary>
        /// 状态:1是已启用，0是已停用
        /// </summary>
        [SugarColumn(Length = 1, ColumnName = "State")]
        public string ZSTATE { get; set; }
        /// <summary>
        /// 是否删除数据是否有效的标识:    有效：1无效：0
        /// </summary>
        [SugarColumn(Length = 1, ColumnName = "DataIdentifier")]
        public string ZDELETE { get; set; }
        /// <summary>
        /// 一带一路(国资委):0-否，1-是
        /// </summary>
        [SugarColumn(Length = 1, ColumnName = "RoadGuoZiW")]
        public string ZBRGZW { get; set; }
        /// <summary>
        /// 一带一路(海外):0-否，1-是
        /// </summary>
        [SugarColumn(Length = 1, ColumnName = "RoadHaiW")]
        public string ZBRHW { get; set; }
        /// <summary>
        /// 一带一路(共建):0-否，1-是
        /// </summary>
        [SugarColumn(Length = 1, ColumnName = "RoadGongJ")]
        public string ZBRGJ { get; set; }
        /// <summary>
        /// 区域代码
        /// </summary>
        [SugarColumn(Length = 6, ColumnName = "AreaCode")]
        public string ZAREACODE { get; set; }
        /// <summary>
        /// 多语言描述表类型
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<ZMDGTT_ZLANG>? ZLANG_LIST { get; set; }
    }
}
