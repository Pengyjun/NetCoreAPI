using SqlSugar;

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
        public string? Zacorgno { get; set; }
        /// <summary>
        /// 核算部门编号
        /// </summary>
        public string? Zdcode { get; set; }
        /// <summary>
        /// 核算组织ID
        /// </summary>
        public string? Zacid { get; set; }
        /// <summary>
        /// 核算部门ID
        /// </summary>
        public string? Zdid { get; set; }
        /// <summary>
        /// 核算部门中文简体名称
        /// </summary>
        public string? ZdnameChs { get; set; }
        /// <summary>
        /// 核算部门中文繁体名称
        /// </summary>
        public string? ZdnameCht { get; set; }
        /// <summary>
        /// 核算部门英文名称
        /// </summary>
        public string? ZdnameEn { get; set; }
        /// <summary>
        /// 上级核算部门ID
        /// </summary>
        public string? Zdparentid { get; set; }
        /// <summary>
        /// 核算部门停用标志 1:停用，0:未停用
        /// </summary>
        public string? Zdatstate { get; set; }
        /// <summary>
        /// 数据删除标识 1:删除，0：正常
        /// </summary>
        public string? Zdelete { get; set; }
    }
}
