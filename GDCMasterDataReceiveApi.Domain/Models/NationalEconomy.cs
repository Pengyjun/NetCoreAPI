using SqlSugar;
using System.ComponentModel.DataAnnotations.Schema;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 国民经济行业分类
    /// </summary>
    [SugarTable("t_nationaleconomy", IsDisabledDelete = true)]
    public class NationalEconomy : BaseEntity<long>
    {
        /// <summary>
        /// 发送记录ID 发送记录的ID，必须保证此ID在同一个发送批次中是唯一的。用于记录发送方对于此发送记录的唯一标识。
        /// </summary>
        [NotMapped]
        public string ZZSERIAL { get; set; }
        /// <summary>
        /// 国民经济行业分类代码
        /// </summary>
        [SugarColumn(Length = 4, ColumnName = "NationalEconomicIndustryClassificationCode")]
        public string ZNEQCODE { get; set; }
        /// <summary>
        /// 国民经济行业分类类别名称
        /// </summary>
        [SugarColumn(Length = 100, ColumnName = "NationalEconomicIndustryClassificationName")]
        public string ZNEQNAME { get; set; }
        /// <summary>
        /// 国民经济行业分类上级代码
        /// </summary>
        [SugarColumn(Length = 4, ColumnName = "SupNationalEconomicIndustryClassificationCode")]
        public string ZNEQCODEUP { get; set; }
        /// <summary>
        /// 国民经济行业分类说明
        /// </summary>
        [SugarColumn(Length = 500, ColumnName = "ClassificationOfNationalEconomicIndustries")]
        public string? ZNEQDESC { get; set; }
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
        public List<ZMDGS_ZLANG5>? ZLANG_LIST { get; set; }
    }
    /// <summary>
    /// 多语言描述表类型
    /// </summary>
    public class ZMDGS_ZLANG5
    {
        /// <summary>
        /// 语种代码
        /// 10
        /// </summary>
        public string? ZLANGCODE { get; set; }
        /// <summary>
        /// 编码描述
        /// 255
        /// </summary>
        public string? ZCODE_DESC { get; set; }
    }
}
