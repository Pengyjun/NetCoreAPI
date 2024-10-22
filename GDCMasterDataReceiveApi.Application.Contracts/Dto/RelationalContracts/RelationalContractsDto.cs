using MiniExcelLibs.Attributes;

namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.RelationalContracts
{
    /// <summary>
    /// 委托关系 反显
    /// </summary>
    public class RelationalContractsSearchDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public string? Id { get; set; }
        /// <summary>
        /// 机构主数据编码:2023年2月20日多机构新增
        /// </summary>
        public string? MDCode { get; set; }
        /// <summary>
        /// 委托单位明细行项目:2023年2月20日多机构新增主映射关系传值‘0’
        /// </summary>
        public string? DetailedLine { get; set; }
        /// <summary>
        /// 委托单位编码:2023年2月20日多机构新增
        /// </summary>
        public string? Code { get; set; }
        /// <summary>
        /// 委托状态:2023年2月20日多机构新增值域；1-启用0-停用
        /// </summary>
        public string? Status { get; set; }
    }
    /// <summary>
    /// 委托关系详细
    /// </summary>
    public class RelationalContractsDetailsDto
    {
        [ExcelIgnore]
        public string? Id { get; set; }
        /// <summary>
        /// 机构主数据编码:2023年2月20日多机构新增
        /// </summary>
        [ExcelIgnore]
        public string? MDCode { get; set; }
        /// <summary>
        /// 委托单位明细行项目:2023年2月20日多机构新增主映射关系传值‘0’
        /// </summary>
        [DisplayName("委托单位明细行项目")]
        public string? DetailedLine { get; set; }
        /// <summary>
        /// 视图标识:目前仅传输单值:zx-机构视图、zy-核算组织视图、ZG-管理组织视图、Z4-税务代管组织（行政）视图。
        /// </summary>
        [ExcelIgnore]
        public string? ViewIdentification { get; set; }
        /// <summary>
        /// 委托单位编码:2023年2月20日多机构新增
        /// </summary>
        [DisplayName("委托单位编码")]
        public string? Code { get; set; }
        /// <summary>
        /// 委托状态:2023年2月20日多机构新增值域；1-启用0-停用
        /// </summary>
        [ExcelIgnore]
        public string? Status { get; set; }
        /// <summary>
        /// 机构编码（无含义码）
        /// 1、如果视图标识=ZX，OID字段：取机构主数据编码MDM_CODE在Z1模型中的OID编码。
        /// 2、如果视图标识=ZY，OID字段：取委托单位编码ZDELEGATE_ORG在Z1模型中的OID编码。
        /// </summary>
        [DisplayName("机构编码（无含义码）")]
        public string? OrgCode { get; set; }
        /// <summary>
        /// 核算机构编码:
        /// 1、如果视图标识=ZX，ZACO字段：取委托单位编码ZDELEGATE_ORG在Z2模型中的ZACO编码；
        /// 2、如果视图标识=ZY，ZACO字段：取机构主数据编码MDM_CODE在Z2模型中的ZACO编码。
        /// </summary>
        [DisplayName("核算机构编码")]
        public string? AccOrgCode { get; set; }
        /// <summary>
        /// 组织树编码:2023年2月20日多机构新增值域：1-组织机构树，2-核算组织树，3-管理组织树
        /// </summary>
        [ExcelIgnore]
        public string? TreeCode { get; set; }
        /// <summary>
        /// 组织树版本号:2023年2月20日多机构新增
        /// </summary>
        [ExcelIgnore]
        public string? Version { get; set; }
        [ExcelIgnore]
        public DateTime? CreateTime { get; set; }
        [ExcelIgnore]
        public DateTime? UpdateTime { get; set; }
    }
    /// <summary>
    /// 委托关系 接收
    /// </summary>
    public class RelationalContractsItem
    {
        /// <summary>
        /// 
        /// </summary>
        public long? Id { get; set; }
        /// <summary>
        /// 发送记录ID 发送记录的ID，必须保证此ID在同一个发送批次中是唯一的。用于记录发送方对于此发送记录的唯一标识。
        /// </summary>
        public string? ZZSERIAL { get; set; }
        /// <summary>
        /// 机构主数据编码:2023年2月20日多机构新增
        /// </summary>
        public string? MDM_CODE { get; set; }
        /// <summary>
        /// 委托单位明细行项目:2023年2月20日多机构新增主映射关系传值‘0’
        /// </summary>
        public string? ZNUMC4 { get; set; }
        /// <summary>
        /// 视图标识:目前仅传输单值:zx-机构视图、zy-核算组织视图、ZG-管理组织视图、Z4-税务代管组织（行政）视图。
        /// </summary>
        public string? ZMVIEW_FLAG { get; set; }
        /// <summary>
        /// 委托单位编码:2023年2月20日多机构新增
        /// </summary>
        public string? ZDELEGATE_ORG { get; set; }
        /// <summary>
        /// 委托状态:2023年2月20日多机构新增值域；1-启用0-停用
        /// </summary>
        public string? ZDELEGATE_STATE { get; set; }
        /// <summary>
        /// 机构编码（无含义码）
        /// 1、如果视图标识=ZX，OID字段：取机构主数据编码MDM_CODE在Z1模型中的OID编码。
        /// 2、如果视图标识=ZY，OID字段：取委托单位编码ZDELEGATE_ORG在Z1模型中的OID编码。
        /// </summary>
        public string? OID { get; set; }
        /// <summary>
        /// 核算机构编码:
        /// 1、如果视图标识=ZX，ZACO字段：取委托单位编码ZDELEGATE_ORG在Z2模型中的ZACO编码；
        /// 2、如果视图标识=ZY，ZACO字段：取机构主数据编码MDM_CODE在Z2模型中的ZACO编码。
        /// </summary>
        public string? ZACO { get; set; }
        /// <summary>
        /// 组织树编码:2023年2月20日多机构新增值域：1-组织机构树，2-核算组织树，3-管理组织树
        /// </summary>
        public string? ZTREEID { get; set; }
        /// <summary>
        /// 组织树版本号:2023年2月20日多机构新增
        /// </summary>
        public string? ZTREEVER { get; set; }
    }
}
