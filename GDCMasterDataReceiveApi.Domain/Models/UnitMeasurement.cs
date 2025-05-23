﻿using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 常用计量单位
    /// </summary>
    [SugarTable("t_unitmeasurement", IsDisabledDelete = true)]
    public class UnitMeasurement : BaseEntity<long>
    {
        /// <summary>
        /// 发送记录ID 发送记录的ID，必须保证此ID在同一个发送批次中是唯一的。用于记录发送方对于此发送记录的唯一标识。
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string? ZZSERIAL { get; set; }
        /// <summary>
        /// 计量单位代码:业务主键
        /// </summary>
        [SugarColumn(Length = 20, ColumnName = "Code")]
        public string? ZUNITCODE { get; set; }
        /// <summary>
        /// 计量单位名称:计量单位的名称或说明，一般采用中文或常用符号。
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "Name")]
        public string? ZUNITNAME { get; set; }
        ///// <summary>
        ///// 计量单位名称（其它语言的集合）
        ///// </summary>
        //[SugarColumn(IsIgnore = true)]
        //public List<ZMDGTT_UNIT_LANG>? ZUNIT_LANG { get; set; }
        /// <summary>
        /// 版本：数据的版本号。数据每次变更时，版本号自动加1。
        /// </summary>
        [SugarColumn(ColumnDataType = "int", Length = 15, ColumnName = "Version")]
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
    }
}
