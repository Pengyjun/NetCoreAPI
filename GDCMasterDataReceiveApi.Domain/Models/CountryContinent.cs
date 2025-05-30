﻿using SqlSugar;

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
        [SugarColumn(IsIgnore = true)]
        public string? ZZSERIAL { get; set; }
        /// <summary>
        /// 大洲代码：大洲代码
        /// </summary>
        [SugarColumn(Length = 128, ColumnName = "ContinentCode")]
        public string? ZCONTINENTCODE { get; set; }
        /// <summary>
        /// 大洲名称：大洲名称
        /// </summary>
        [SugarColumn(Length = 128, ColumnName = "Name")]
        public string? ZCONTINENTNAME { get; set; }
        /// <summary>
        /// 区域代码：大洲所属区域代码
        /// </summary>
        [SugarColumn(Length = 128, ColumnName = "AreaCode")]
        public string? ZAREACODE { get; set; }
        /// <summary>
        /// 区域描述：区域描述
        /// </summary>
        [SugarColumn(Length = 128, ColumnName = "RegionalDescr")]
        public string? ZAREANAME { get; set; }
        /// <summary>
        /// 版本：数据的版本号。数据每次变更时，版本号自动加1。
        /// </summary>
        [SugarColumn(ColumnDataType = "int", Length = 10, ColumnName = "Version")]
        public string? ZVERSION { get; set; }
        /// <summary>
        /// 状态：数据是否有效的标识:有效：1无效：0
        /// </summary>
        [SugarColumn(Length = 4, ColumnName = "State")]
        public string? ZSTATE { get; set; }
        /// <summary>
        /// 是否删除 数据是否有效的标识:有效：1无效：0
        /// </summary>
        [SugarColumn(Length = 4, ColumnName = "DataIdentifier")]
        public string? ZDELETE { get; set; }
        /// <summary>
        /// 多语言描述表类型
        /// </summary>
        //[SugarColumn(IsIgnore = true)]
        //public List<ZMDGS_ZLANG>? ZLANG_LIST { get; set; }
    }
    
}
