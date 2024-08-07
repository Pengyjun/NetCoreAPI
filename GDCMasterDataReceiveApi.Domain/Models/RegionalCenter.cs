﻿using SqlSugar;
using System.ComponentModel.DataAnnotations.Schema;

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
        [NotMapped]
        public string ZZSERIAL { get; set; }
        /// <summary>
        /// 中交区域中心代码:中交区域中心编码
        /// </summary>
        [SugarColumn(Length = 6, ColumnName = "CCCCRegionalCenterCode")]
        public string ZCRCCODE { get; set; }
        /// <summary>
        /// 中交区域中心描述:编码描述
        /// </summary>
        [SugarColumn(Length = 100, ColumnName = "DescriptionOfCCCCRegionalCenter")]
        public string ZCRCNAME { get; set; }
        /// <summary>
        /// 版本
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
        public List<ZMDGS_ZLANG4>? ZMDGTT_ZLANG { get; set; }
    }
    /// <summary>
    /// 多语言描述表类型
    /// </summary>
    public class ZMDGS_ZLANG4
    {
        /// <summary>
        /// 语种代码
        /// 10
        /// </summary>
        public string ? ZLANGCODE {  get; set; }
        /// <summary>
        /// 编码描述
        /// 255
        /// </summary>
        public string ? ZCODE_DESC {  get; set; }
    }
}
