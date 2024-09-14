using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 多组织-税务代管组织(行政)
    /// </summary>
    [SugarTable("t_escroworganization", IsDisabledDelete = true)]
    public class EscrowOrganization : BaseEntity<long>
    {
        /// <summary>
        /// 发送记录ID 发送记录的ID，必须保证此ID在同一个发送批次中是唯一的。用于记录发送方对于此发送记录的唯一标识。
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string? ZZSERIAL { get; set; }
        /// <summary>
        /// 接口唯一ID  发送记录的ID，必须保证此ID在同一个发送批次中是唯一的。用于记录发送方对于此发送记录的唯一标识。
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string? ZINSTID { get; set; }
        /// <summary>
        /// 机构主数据编码:机构主数据的唯一标识
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "OrgMDCode")]
        public string? MDM_CODE { get; set; }
        /// <summary>
        /// 上级机构主数据编码 :20230627新增
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "SupOrgMDCode")]
        public string? ZORGUP { get; set; }
        /// <summary>
        /// HR机构主数据编码:机构与人力系统的机构唯一编号（OID，不可改变）
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "HROrgMDCode")]
        public string? OID { get; set; }
        /// <summary>
        /// HR上级机构主数据编码 : 上级机构编码，关联机构主数据编码ZZOID（OID）。
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "SupHROrgMDCode")]
        public string? POID { get; set; }
        /// <summary>
        /// 机构编码:机构编码
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "OrgCode")]
        public string? OCODE { get; set; }
        /// <summary>
        /// 所属二级单位编码 
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "UnitSec")]
        public string? GPOID { get; set; }
        /// <summary>
        /// 机构规则码
        /// </summary>
        [SugarColumn(Length = 500, ColumnName = "OrgGruleCode")]
        public string? ORULE { get; set; }
        /// <summary>
        /// 机构属性:机构属性，值域校验
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "OrgAttr")]
        public string? TYPE { get; set; }
        /// <summary>
        /// 机构子属性:机构子属性，值域校验
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "OrgChildAttr")]
        public string? TYPEEXT { get; set; }
        /// <summary>
        /// 节点排序号:序号
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "NodeSequence")]
        public string? SNO { get; set; }
        /// <summary>
        /// 名称（中文:Z0名称（中文）
        /// </summary>
        [SugarColumn(Length = 100, ColumnName = "Name")]
        public string? NAME { get; set; }
        /// <summary>
        /// 简称（中文:Z0简称（中文）
        /// </summary>
        [SugarColumn(Length = 100, ColumnName = "ShortNameChinese")]
        public string? SHORTNAME { get; set; }
        /// <summary>
        /// 机构状态:机构状态，值域校验
        /// </summary>
        [SugarColumn(Length = 10, ColumnName = "OrgStatus")]
        public string? STATUS { get; set; }
        /// <summary>
        /// 层级:机构树层级
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "TreeLevel")]
        public string? GRADE { get; set; }
        /// <summary>
        /// 备注:Z0备注
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "Remark")]
        public string? NOTE { get; set; }
        /// <summary>
        /// 机构所在地:机构所在地，值域校验
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "LocationOfOrg")]
        public string? ORGPROVINCE { get; set; }
        /// <summary>
        /// 国家名称:国家代码，值域校验
        /// </summary>
        [SugarColumn(Length = 20, ColumnName = "Country")]
        public string? CAREA { get; set; }
        /// <summary>
        /// 地域属性:地域属性，值域校验
        /// </summary>
        [SugarColumn(Length = 20, ColumnName = "RegionalAttr")]
        public string? TERRITORYPRO { get; set; }
        /// <summary>
        /// 注册号/统一社会信用:18位长度校验，类型为机构时必填。
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "RegistrationNo")]
        public string? REGISTERCODE { get; set; }
        /// <summary>
        /// 通讯地址:机构的通讯地址
        /// </summary>
        [SugarColumn(Length = 500, ColumnName = "TelAddress")]
        public string? ZADDRESS { get; set; }
        /// <summary>
        /// 持股情况:持股情况
        /// </summary>
        [SugarColumn(Length = 20, ColumnName = "Shareholding")]
        public string? SHAREHOLDINGS { get; set; }
        /// <summary>
        /// 是否独立核算:是否独立核算
        /// </summary>
        [SugarColumn(Length = 20, ColumnName = "IsIndependenceAcc")]
        public string? IS_INDEPENDENT { get; set; }
        /// <summary>
        /// 来源系统:默认为MDM
        /// </summary>
        [SugarColumn(Length = 20, ColumnName = "SourceSystem")]
        public string? ZSYSTEM { get; set; }
        /// <summary>
        /// 名称（英文）:Z0名称（英文）
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "NameEnglish")]
        public string? ENGLISHNAME { get; set; }
        /// <summary>
        /// 简称（英文）:Z0简称（英文）
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "ShortNameEnglish")]
        public string? ENGLISHSHORTNAME { get; set; }
        /// <summary>
        /// 名称（当地语言）:Z0名称（当地语言）
        /// </summary>
        [SugarColumn(Length = 500, ColumnName = "NameLLanguage")]
        public string? ZZTNAME_LOC { get; set; }
        /// <summary>
        /// 名称（当地语言简称）:Z0名称（当地语言简称）
        /// </summary>
        [SugarColumn(Length = 500, ColumnName = "ShortNameLLanguage")]
        public string? ZZTSHNAME_LOC { get; set; }
        /// <summary>
        /// 视图标识:多值按,隔开，值域:zx-机构视图、zy-核算组织视图、ZG-管理组织视图、Z4-税务代管组织（行政）视图。
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "ViewIdentification")]
        public string? VIEW_FLAG { get; set; }
    }
}
