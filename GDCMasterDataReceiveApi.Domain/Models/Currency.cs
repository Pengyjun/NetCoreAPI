﻿using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 币种
    /// </summary>
    [SugarTable("t_currency", IsDisabledDelete = true)]
    public class Currency : BaseEntity<long>
    {
        /// <summary>
        /// 发送记录ID 发送记录的ID，必须保证此ID在同一个发送批次中是唯一的。用于记录发送方对于此发送记录的唯一标识。
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string? ZZSERIAL { get; set; }
        /// <summary>
        /// 数字代码:货币数字代码
        /// </summary>
        [SugarColumn(Length = 32, ColumnName = "Code")]
        public string? ZCURRENCYCODE {  get; set; }
        /// <summary>
        /// 货币名称:货币的中文描述
        /// </summary>
        [SugarColumn(Length = 128, ColumnName = "Name")]
        public string? ZCURRENCYNAME {  get; set; }
        /// <summary>
        /// 字母代码:货币字母代码
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "LetterCode")]
        public string? ZCURRENCYALPHABET {  get; set; }
        /// <summary>
        /// 货币标准名称:货币标准名称
        /// </summary>
        [SugarColumn(Length = 128, ColumnName = "StandardName")]
        public string? STANDARDNAMEE {  get; set; }
        /// <summary>
        /// 备注:说明备注
        /// </summary>
        [SugarColumn(Length = 512, ColumnName = "Remark")]
        public string? ZREMARKS {  get; set; }
        /// <summary>
        /// 版本：数据的版本号。数据每次变更时，版本号自动加1。
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
        ///// <summary>
        ///// 多语言描述表类型
        ///// </summary>
        //[SugarColumn(IsIgnore = true)]
        //public InvoiceLang? ZLANG_LIST { get; set; }
    }
}
