using MiniExcelLibs.Attributes;

namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.EscrowOrganization
{
    /// <summary>
    /// 多组织-税务代管组织(行政) 反显
    /// </summary>
    public class EscrowOrganizationSearchDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public string? Id { get; set; }
        /// <summary>
        /// 节点排序号:序号
        /// </summary>
        public string? NodeSequence { get; set; }
        /// <summary>
        /// 名称（中文:Z0名称（中文）
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 简称（中文:Z0简称（中文）
        /// </summary>
        public string? ShortNameChinese { get; set; }
        /// <summary>
        /// 机构状态:机构状态，值域校验
        /// </summary>
        public string? OrgStatus { get; set; }
        /// <summary>
        /// 备注:Z0备注
        /// </summary>
        public string? Remark { get; set; }
        /// <summary>
        /// 机构所在地:机构所在地，值域校验
        /// </summary>
        public string? LocationOfOrg { get; set; }
        /// <summary>
        /// 国家名称:国家代码，值域校验
        /// </summary>
        public string? Country { get; set; }
        /// <summary>
        /// 地域属性:地域属性，值域校验
        /// </summary>
        public string? RegionalAttr { get; set; }
        /// <summary>
        /// 通讯地址:机构的通讯地址
        /// </summary>
        public string? TelAddress { get; set; }
        /// <summary>
        /// 持股情况:持股情况
        /// </summary>
        public string? Shareholding { get; set; }
        /// <summary>
        /// 是否独立核算:是否独立核算
        /// </summary>
        public string? IsIndependenceAcc { get; set; }
        /// <summary>
        /// 名称（英文）:Z0名称（英文）
        /// </summary>
        public string? NameEnglish { get; set; }
        /// <summary>
        /// 名称（当地语言）:Z0名称（当地语言）
        /// </summary>
        public string? NameLLanguage { get; set; }
    }
    /// <summary>
    /// 多组织-税务代管组织(行政)详情
    /// </summary>
    public class EscrowOrganizationDetailsDto
    {
        [ExcelIgnore]
        public DateTime? CreateTime { get; set; }
        [ExcelIgnore]
        public DateTime? UpdateTime { get; set; }
        [ExcelIgnore]
        public string? Id { get; set; }
        /// <summary>
        /// 机构主数据编码:机构主数据的唯一标识
        /// </summary>
        [ExcelIgnore]
        public string? OrgMDCode { get; set; }
        /// <summary>
        /// 上级机构主数据编码 :20230627新增
        /// </summary>
        [ExcelIgnore]
        public string? SupOrgMDCode { get; set; }
        /// <summary>
        /// HR机构主数据编码:机构与人力系统的机构唯一编号（OID，不可改变）
        /// </summary>
        [ExcelColumnName("HR机构主数据编码")]
        public string? HROrgMDCode { get; set; }
        /// <summary>
        /// HR上级机构主数据编码 : 上级机构编码，关联机构主数据编码ZZOID（OID）。
        /// </summary>
        [ExcelIgnore]
        public string? SupHROrgMDCode { get; set; }
        /// <summary>
        /// 机构编码:机构编码
        /// </summary>
        [ExcelIgnore]
        public string? OrgCode { get; set; }
        /// <summary>
        /// 所属二级单位编码 
        /// </summary>
        [ExcelIgnore]
        public string? UnitSec { get; set; }
        /// <summary>
        /// 机构规则码
        /// </summary>
        [ExcelColumnName("机构规则码")]
        public string? OrgGruleCode { get; set; }
        /// <summary>
        /// 机构属性:机构属性，值域校验
        /// </summary>
        [ExcelColumnName("机构属性")]
        public string? OrgAttr { get; set; }
        /// <summary>
        /// 机构子属性:机构子属性，值域校验
        /// </summary>
        [ExcelIgnore]
        public string? OrgChildAttr { get; set; }
        /// <summary>
        /// 节点排序号:序号
        /// </summary>
        [ExcelIgnore]
        public string? NodeSequence { get; set; }
        /// <summary>
        /// 名称（中文:Z0名称（中文）
        /// </summary>
        [ExcelColumnName("名称")]
        public string? Name { get; set; }
        /// <summary>
        /// 简称（中文:Z0简称（中文）
        /// </summary>
        [ExcelColumnName("简称")]
        public string? ShortNameChinese { get; set; }
        /// <summary>
        /// 机构状态:机构状态，值域校验
        /// </summary>
        [ExcelIgnore]
        public string? OrgStatus { get; set; }
        /// <summary>
        /// 层级:机构树层级
        /// </summary>
        [ExcelIgnore]
        public string? TreeLevel { get; set; }
        /// <summary>
        /// 备注:Z0备注
        /// </summary>
        [ExcelColumnName("备注")]
        public string? Remark { get; set; }
        /// <summary>
        /// 机构所在地:机构所在地，值域校验
        /// </summary>
        [ExcelIgnore]
        public string? LocationOfOrg { get; set; }
        /// <summary>
        /// 国家名称:国家代码，值域校验
        /// </summary>
        [ExcelColumnName("国家名称")]
        public string? Country { get; set; }
        /// <summary>
        /// 地域属性:地域属性，值域校验
        /// </summary>
        [ExcelColumnName("地域属性")]
        public string? RegionalAttr { get; set; }
        /// <summary>
        /// 注册号/统一社会信用:18位长度校验，类型为机构时必填。
        /// </summary>
        [ExcelColumnName("注册号/统一社会信用")]
        public string? RegistrationNo { get; set; }
        /// <summary>
        /// 通讯地址:机构的通讯地址
        /// </summary>
        [ExcelColumnName("通讯地址")]
        public string? TelAddress { get; set; }
        /// <summary>
        /// 持股情况:持股情况
        /// </summary>
        [ExcelIgnore]
        public string? Shareholding { get; set; }
        /// <summary>
        /// 是否独立核算:是否独立核算
        /// </summary>
        [ExcelIgnore]
        public string? IsIndependenceAcc { get; set; }
        /// <summary>
        /// 名称（英文）:Z0名称（英文）
        /// </summary>
        [ExcelColumnName("名称（英文）")]
        public string? NameEnglish { get; set; }
        /// <summary>
        /// 简称（英文）:Z0简称（英文）
        /// </summary>
        [ExcelColumnName("简称（英文）")]
        public string? ShortNameEnglish { get; set; }
        /// <summary>
        /// 名称（当地语言）:Z0名称（当地语言）
        /// </summary>
        [ExcelColumnName("名称（当地语言）")]
        public string? NameLLanguage { get; set; }
        /// <summary>
        /// 名称（当地语言简称）:Z0名称（当地语言简称）
        /// </summary>
        [ExcelColumnName("名称（当地语言简称）")]
        public string? ShortNameLLanguage { get; set; }
        /// <summary>
        /// 视图标识:多值按,隔开，值域:zx-机构视图、zy-核算组织视图、ZG-管理组织视图、Z4-税务代管组织（行政）视图。
        /// </summary>
        [ExcelIgnore]
        public string? ViewIdentification { get; set; }
    }
    /// <summary>
    /// 多组织-税务代管组织(行政)  接收
    /// </summary>
    public class EscrowOrganizationItem
    {

        /// <summary>
        /// 
        /// </summary>
        public long? Id {  get; set; }
        /// <summary>
        /// 发送记录ID 发送记录的ID，必须保证此ID在同一个发送批次中是唯一的。用于记录发送方对于此发送记录的唯一标识。
        /// </summary>
        public string? ZZSERIAL { get; set; }
        /// <summary>
        /// 接口唯一ID  发送记录的ID，必须保证此ID在同一个发送批次中是唯一的。用于记录发送方对于此发送记录的唯一标识。
        /// </summary>
        public string? ZINSTID { get; set; }
        /// <summary>
        /// 机构主数据编码:机构主数据的唯一标识
        /// </summary>
        public string? MDM_CODE { get; set; }
        /// <summary>
        /// 上级机构主数据编码 :20230627新增
        /// </summary>
        public string? ZORGUP { get; set; }
        /// <summary>
        /// HR机构主数据编码:机构与人力系统的机构唯一编号（OID，不可改变）
        /// </summary>
        public string? OID { get; set; }
        /// <summary>
        /// HR上级机构主数据编码 : 上级机构编码，关联机构主数据编码ZZOID（OID）。
        /// </summary>
        public string? POID { get; set; }
        /// <summary>
        /// 机构编码:机构编码
        /// </summary>
        public string? OCODE { get; set; }
        /// <summary>
        /// 所属二级单位编码 
        /// </summary>
        public string? GPOID { get; set; }
        /// <summary>
        /// 机构规则码
        /// </summary>
        public string? ORULE { get; set; }
        /// <summary>
        /// 机构属性:机构属性，值域校验
        /// </summary>
        public string? TYPE { get; set; }
        /// <summary>
        /// 机构子属性:机构子属性，值域校验
        /// </summary>
        public string? TYPEEXT { get; set; }
        /// <summary>
        /// 节点排序号:序号
        /// </summary>
        public string? SNO { get; set; }
        /// <summary>
        /// 名称（中文:Z0名称（中文）
        /// </summary>
        public string? NAME { get; set; }
        /// <summary>
        /// 简称（中文:Z0简称（中文）
        /// </summary>
        public string? SHORTNAME { get; set; }
        /// <summary>
        /// 机构状态:机构状态，值域校验
        /// </summary>
        public string? STATUS { get; set; }
        /// <summary>
        /// 层级:机构树层级
        /// </summary>
        public string? GRADE { get; set; }
        /// <summary>
        /// 备注:Z0备注
        /// </summary>
        public string? NOTE { get; set; }
        /// <summary>
        /// 机构所在地:机构所在地，值域校验
        /// </summary>
        public string? ORGPROVINCE { get; set; }
        /// <summary>
        /// 国家名称:国家代码，值域校验
        /// </summary>
        public string? CAREA { get; set; }
        /// <summary>
        /// 地域属性:地域属性，值域校验
        /// </summary>
        public string? TERRITORYPRO { get; set; }
        /// <summary>
        /// 注册号/统一社会信用:18位长度校验，类型为机构时必填。
        /// </summary>
        public string? REGISTERCODE { get; set; }
        /// <summary>
        /// 通讯地址:机构的通讯地址
        /// </summary>
        public string? ZADDRESS { get; set; }
        /// <summary>
        /// 持股情况:持股情况
        /// </summary>
        public string? SHAREHOLDINGS { get; set; }
        /// <summary>
        /// 是否独立核算:是否独立核算
        /// </summary>
        public string? IS_INDEPENDENT { get; set; }
        /// <summary>
        /// 来源系统:默认为MDM
        /// </summary>
        public string? ZSYSTEM { get; set; }
        /// <summary>
        /// 名称（英文）:Z0名称（英文）
        /// </summary>
        public string? ENGLISHNAME { get; set; }
        /// <summary>
        /// 简称（英文）:Z0简称（英文）
        /// </summary>
        public string? ENGLISHSHORTNAME { get; set; }
        /// <summary>
        /// 名称（当地语言）:Z0名称（当地语言）
        /// </summary>
        public string? ZZTNAME_LOC { get; set; }
        /// <summary>
        /// 名称（当地语言简称）:Z0名称（当地语言简称）
        /// </summary>
        public string? ZZTSHNAME_LOC { get; set; }
        /// <summary>
        /// 视图标识:多值按,隔开，值域:zx-机构视图、zy-核算组织视图、ZG-管理组织视图、Z4-税务代管组织（行政）视图。
        /// </summary>
        public string? VIEW_FLAG { get; set; }
    }
}
