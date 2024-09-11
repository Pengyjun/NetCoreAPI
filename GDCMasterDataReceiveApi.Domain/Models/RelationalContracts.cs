using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 委托关系
    /// </summary>
    [SugarTable("t_relationalcontracts", IsDisabledDelete = true)]
    public class RelationalContracts : BaseEntity<long>
    {
        /// <summary>
        /// 发送记录ID 发送记录的ID，必须保证此ID在同一个发送批次中是唯一的。用于记录发送方对于此发送记录的唯一标识。
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string? ZZSERIAL { get; set; }
        /// <summary>
        /// 机构主数据编码:2023年2月20日多机构新增
        /// </summary>
        [SugarColumn(Length = 9, ColumnName = "MDCode")]
        public string? MDM_CODE { get; set; }
        /// <summary>
        /// 委托单位明细行项目:2023年2月20日多机构新增主映射关系传值‘0’
        /// </summary>
        [SugarColumn(Length = 4, ColumnName = "DetailedLine")]
        public string? ZNUMC4 { get; set; }
        /// <summary>
        /// 视图标识:目前仅传输单值:zx-机构视图、zy-核算组织视图、ZG-管理组织视图、Z4-税务代管组织（行政）视图。
        /// </summary>
        [SugarColumn(Length = 2, ColumnName = "ViewIdentification")]
        public string? ZMVIEW_FLAG { get; set; }
        /// <summary>
        /// 委托单位编码:2023年2月20日多机构新增
        /// </summary>
        [SugarColumn(Length = 9, ColumnName = "Code")]
        public string? ZDELEGATE_ORG { get; set; }
        /// <summary>
        /// 委托状态:2023年2月20日多机构新增值域；1-启用0-停用
        /// </summary>
        [SugarColumn(Length = 1, ColumnName = "Status")]
        public string? ZDELEGATE_STATE { get; set; }
        /// <summary>
        /// 机构编码（无含义码）
        /// 1、如果视图标识=ZX，OID字段：取机构主数据编码MDM_CODE在Z1模型中的OID编码。
        /// 2、如果视图标识=ZY，OID字段：取委托单位编码ZDELEGATE_ORG在Z1模型中的OID编码。
        /// </summary>
        [SugarColumn(Length = 20, ColumnName = "OrgCode")]
        public string? OID { get; set; }
        /// <summary>
        /// 核算机构编码:
        /// 1、如果视图标识=ZX，ZACO字段：取委托单位编码ZDELEGATE_ORG在Z2模型中的ZACO编码；
        /// 2、如果视图标识=ZY，ZACO字段：取机构主数据编码MDM_CODE在Z2模型中的ZACO编码。
        /// </summary>
        [SugarColumn(Length = 18, ColumnName = "AccOrgCode")]
        public string? ZACO { get; set; }
        /// <summary>
        /// 组织树编码:2023年2月20日多机构新增值域：1-组织机构树，2-核算组织树，3-管理组织树
        /// </summary>
        [SugarColumn(Length = 10, ColumnName = "TreeCode")]
        public string? ZTREEID { get; set; }
        /// <summary>
        /// 组织树版本号:2023年2月20日多机构新增
        /// </summary>
        [SugarColumn(Length = 4, ColumnName = "Version")]
        public string? ZTREEVER { get; set; }
    }
}
