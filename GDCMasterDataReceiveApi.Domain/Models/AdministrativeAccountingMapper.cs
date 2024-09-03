using SqlSugar;
using System.ComponentModel.DataAnnotations.Schema;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 行政机构和核算机构映射关系
    /// </summary>
    [SugarTable("t_administrativeaccountingmapper", IsDisabledDelete = true)]
    public class AdministrativeAccountingMapper : BaseEntity<long>
    {
        /// <summary>
        /// 发送记录ID 发送记录的ID，必须保证此ID在同一个发送批次中是唯一的。用于记录发送方对于此发送记录的唯一标识。
        /// </summary>
        [NotMapped]
        public string ZZSERIAL { get; set; }
        /// <summary>
        /// 主键ID，必须唯一
        /// </summary>
        [SugarColumn(Length = 100, ColumnName = "KeyId")]
        public string ZID { get; set; }
        /// <summary>
        /// 核算组织ID
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "AccOrgId")]
        public string ZAID { get; set; }
        /// <summary>
        /// 核算组织编码
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "AccOrgCode")]
        public string ZAORGNO { get; set; }
        /// <summary>
        /// 行政组织ID
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "AdministrativeOrgId")]
        public string ZORGID { get; set; }
        /// <summary>
        /// 行政组织编码
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "AdministrativeOrgCode")]
        public string ZORGCODE { get; set; }
        /// <summary>
        /// 是否删除: 数据是否有效的标识:   有效：1无效：0
        /// </summary>
        [SugarColumn(Length = 1, ColumnName = "DataIdentifier")]
        public string ZDELETE { get; set; }
    }
}
