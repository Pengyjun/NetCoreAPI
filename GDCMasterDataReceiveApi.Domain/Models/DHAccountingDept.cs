using SqlSugar;
using System.ComponentModel;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// DH核算部门
    /// </summary>
    [SugarTable("t_dh_accountingdept", IsDisabledDelete = true)]
    public class DHAccountingDept : BaseEntity<long>
    {
        /// <summary>
        /// 核算组织编号
        /// </summary>
        [DisplayName("核算组织编号")]
        public string? Zacorgno { get; set; }
        /// <summary>
        /// 核算部门编号
        /// </summary>
        [DisplayName("核算部门编号")]
        public string? Zdcode { get; set; }
        /// <summary>
        /// 核算组织ID
        /// </summary>
        [DisplayName("核算组织ID")]
        public string? Zacid { get; set; }
        /// <summary>
        /// 核算部门ID
        /// </summary>
        [DisplayName("核算部门ID")]
        public string? Zdid { get; set; }
        /// <summary>
        /// 核算部门中文简体名称
        /// </summary>
        [DisplayName("核算部门中文简体名称")]
        public string? ZdnameChs { get; set; }
        /// <summary>
        /// 核算部门中文繁体名称
        /// </summary>
        [DisplayName("核算部门中文繁体名称")]
        public string? ZdnameCht { get; set; }
        /// <summary>
        /// 核算部门英文名称
        /// </summary>
        [DisplayName("核算部门英文名称")]
        public string? ZdnameEn { get; set; }
        /// <summary>
        /// 上级核算部门ID
        /// </summary>
        [DisplayName("上级核算部门ID")]
        public string? Zdparentid { get; set; }
        /// <summary>
        /// 核算部门停用标志 1:停用，0:未停用
        /// </summary>
        [DisplayName("核算部门停用标志")]
        public string? Zdatstate { get; set; }
        /// <summary>
        /// 数据删除标识 1:删除，0：正常
        /// </summary>
        [DisplayName("数据删除标识")]
        public string? Zdelete { get; set; }
    }
}
