using GDCMasterDataReceiveApi.Domain.OtherModels;
using SqlSugar;
using System.ComponentModel.DataAnnotations.Schema;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 语言语种
    /// </summary>
    [SugarTable("t_language", IsDisabledDelete = true)]
    public class Language : BaseEntity<long>
    {
        /// <summary>
        /// 发送记录ID 发送记录的ID，必须保证此ID在同一个发送批次中是唯一的。用于记录发送方对于此发送记录的唯一标识。
        /// </summary>
        [NotMapped]
        public string ZZSERIAL { get; set; }
        /// <summary>
        /// GB/T 4880.2/B目录代码:GB/T 4880.2/B目录代码
        /// </summary>
        [SugarColumn(Length = 12, ColumnName = "DirCode")]
        public string ZLANG_BIB { get; set; }
        /// <summary>
        /// GB/T 4880.2/T术语代码:GB/T 4880.2/T术语代码
        /// </summary>
        [SugarColumn(Length = 12, ColumnName = "TermCode")]
        public string ZLANG_TER { get; set; }
        /// <summary>
        /// 汉语名称
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "Name")]
        public string ZLANG_ZH { get; set; }
        /// <summary>
        /// 英语名称
        /// </summary>
        [SugarColumn(Length = 58, ColumnName = "EnglishName")]
        public string ZLANG_EN { get; set; }
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
        [SugarColumn(Length = 1, ColumnName = "DataIdentifier")]
        public string ZDELETE { get; set; }
        /// <summary>
        /// 多语言描述表类型
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<ZMDGTT_ZLANG2>? ZLANG_LIST { get; set; }
    }
}
