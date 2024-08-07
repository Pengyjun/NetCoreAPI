using SqlSugar;
using System.ComponentModel.DataAnnotations.Schema;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 核算部门
    /// </summary>
    [SugarTable("t_accountingdepartment", IsDisabledDelete = true)]
    public class AccountingDepartment : BaseEntity<long>
    {
        /// <summary>
        /// 发送记录ID 发送记录的ID，必须保证此ID在同一个发送批次中是唯一的。用于记录发送方对于此发送记录的唯一标识。
        /// </summary>
        [NotMapped]
        public string ZZSERIAL { get; set; }
        /// <summary>
        /// 核算组织编号:9月18日新加
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "AccountingOrganizationNumber")]
        public string ZACORGNO { get; set; }
        /// <summary>
        /// 核算部门编号
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "AccountingDepartmentNumber")]
        public string ZDCODE { get; set; }
        /// <summary>
        /// 核算组织ID
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "AccountingOrganizationID")]
        public string ZACID { get; set; }
        /// <summary>
        /// 核算部门ID
        /// </summary>
        [SugarColumn(Length = 36, ColumnName = "AccountingDepartmentID")]
        public string ZDID { get; set; }
        /// <summary>
        /// 核算部门中文简体名称
        /// </summary>
        [SugarColumn(Length = 100, ColumnName = "AccountingDepartmentShortName")]
        public string ZDNAME_CHS { get; set; }
        /// <summary>
        /// 核算部门中文繁体名称
        /// </summary>
        [SugarColumn(Length = 100, ColumnName = "AccountingDepartmentTraditionalName")]
        public string? ZDNAME_CHT { get; set; }
        /// <summary>
        /// 核算部门英文名称
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "AccountingDepartmentEnglishName")]
        public string? ZDNAME_EN { get; set; }
        /// <summary>
        /// 上级核算部门ID
        /// </summary>
        [SugarColumn(Length = 36, ColumnName = "SupAccountingDepartmentID")]
        public string? ZDPARENTID { get; set; }
        /// <summary>
        /// 核算部门停用标志：1:停用0:未停用
        /// </summary>
        [SugarColumn(Length = 1, ColumnName = "State")]
        public string? ZDATSTATE { get; set; }
        /// <summary>
        /// 数据删除标识：1:删除0：正常
        /// </summary>
        [SugarColumn(Length = 1, ColumnName = "IsDeleteValidIdentifier")]
        public string? ZDELETE { get; set; }
    }
}
