using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 多组织-核算机构
    /// </summary>
    [SugarTable("t_accountingorganization", IsDisabledDelete = true)]
    public class AccountingOrganization : BaseEntity<long>
    {
        /// <summary>
        /// 发送记录ID 发送记录的ID，必须保证此ID在同一个发送批次中是唯一的。用于记录发送方对于此发送记录的唯一标识。
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string? ZZSERIAL { get; set; }
        /// <summary>
        /// 机构主数据编码:机构主数据的唯一标识
        /// </summary>
        [SugarColumn(Length = 9, ColumnName = "OrgCode")]
        public string? MDM_CODE { get; set; }
        /// <summary>
        /// 机构名称（中文）:机构的规范全称
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "Name")]
        public string? ZZTNAME_ZH { get; set; }
        /// <summary>
        /// 机构简称（中文）:机构的规范简称
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "OrgShortName")]
        public string? ZZTSHNAME_CHS { get; set; }
        /// <summary>
        /// 机构名称（英文）
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "OrgELName")]
        public string? ZZTNAME_EN { get; set; }
        /// <summary>
        /// 机构简称（英文）
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "OrgELShortName")]
        public string? ZZTSHNAME_EN { get; set; }
        /// <summary>
        /// 机构名称（当地语言）
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "OrgLLanguage")]
        public string? ZZTNAME_LOC { get; set; }
        /// <summary>
        /// 机构简称（当地语言）
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "OrgShortNameLLanguage")]
        public string? ZZTSHNAME_LOC { get; set; }
        /// <summary>
        /// 机构状态:值域：1-运营、2-筹备、3-停用、4-撤销
        /// </summary>
        [SugarColumn(Length = 1, ColumnName = "State")]
        public string? ZOSTATE { get; set; }
        /// <summary>
        /// 上级机构主数据编码:组织机构的上级机构主数据编码，MDM CODE
        /// </summary>
        [SugarColumn(Length = 9, ColumnName = "SupOrgCode")]
        public string? ZORGUP { get; set; }
        /// <summary>
        /// 机构主数据编码（旧）
        /// </summary>
        [SugarColumn(Length = 20, ColumnName = "OldOrgCode")]
        public string? OID { get; set; }
        /// <summary>
        /// 上级机构主数据编码（旧）
        /// </summary>
        [SugarColumn(Length = 20, ColumnName = "OldSupOrgCode")]
        public string? POID { get; set; }
        /// <summary>
        /// 机构规则码:机构所有上级机构主数据编码串列
        /// </summary>
        [SugarColumn(Length = 500, ColumnName = "OrgGruleCode")]
        public string? ZORULE { get; set; }
        /// <summary>
        /// 所属二级单位编码
        /// </summary>
        [SugarColumn(Length = 9, ColumnName = "UnitSecCode")]
        public string? ZGPOID { get; set; }
        /// <summary>
        /// 层级:当前节点所处树形的层级
        /// </summary>
        [SugarColumn(Length = 2, ColumnName = "TreeLevel")]
        public string? ZO_LEVEL { get; set; }
        /// <summary>
        /// 节点排序号:当前节点下机构的排序号
        /// </summary>
        [SugarColumn(Length = 9, ColumnName = "LevelSequence")]
        public string? ZSNO { get; set; }
        /// <summary>
        /// 机构编码:机构27位有含义编码
        /// </summary>
        [SugarColumn(Length = 27, ColumnName = "MeanOrgCode")]
        public string? ZORGNO { get; set; }
        /// <summary>
        /// 机构属性:机构的管理属性 
        /// </summary>
        [SugarColumn(Length = 3, ColumnName = "OrgAttr")]
        public string? ZOATTR { get; set; }
        /// <summary>
        /// 机构子属性:机构的管理属性细类 
        /// </summary>
        [SugarColumn(Length = 3, ColumnName = "OrgChildAttr")]
        public string? ZOCATTR { get; set; }
        /// <summary>
        /// 国家名称:机构所处的国家/地区
        /// </summary>
        [SugarColumn(Length = 3, ColumnName = "CountryName")]
        public string? ZCYNAME { get; set; }
        /// <summary>
        /// 机构所在地:当国家名称为142中国时必填
        /// </summary>
        [SugarColumn(Length = 100, ColumnName = "OrgLocation")]
        public string? ZORGLOC { get; set; }
        /// <summary>
        /// 地域属性:机构所处的位置对应的中交的区域划分
        /// </summary>
        [SugarColumn(Length = 5, ColumnName = "RegionalAttr")]
        public string? ZREGIONAL { get; set; }
        /// <summary>
        /// 注册号/统一社会信用代码
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "RegistrationNo")]
        public string? ZCUSCC { get; set; }
        /// <summary>
        /// 通讯地址:机构的通讯地址
        /// </summary>
        [SugarColumn(Length = 500, ColumnName = "TelLocation")]
        public string? ZADDRESS { get; set; }
        /// <summary>
        /// 持股情况:指本机构各股东的持股情况
        /// </summary>
        [SugarColumn(Length = 2, ColumnName = "Shareholding")]
        public string? ZHOLDING { get; set; }
        /// <summary>
        /// 是否独立核算:是否单独建立核算账套对本机构发生的业务进行会计核算，填写“是”或“否”
        /// </summary>
        [SugarColumn(Length = 1, ColumnName = "IsIndependenceAcc")]
        public string? ZCHECKIND { get; set; }
        /// <summary>
        /// 组织树编码:机构树形编码，编码1，代表组织机构
        /// </summary>
        [SugarColumn(Length = 10, ColumnName = "OrgTreeCode")]
        public string? ZTREEID1 { get; set; }
        /// <summary>
        /// 组织树版本号
        /// </summary>
        [SugarColumn(Length = 4, ColumnName = "OrgTreeVersion")]
        public string? ZTREEVER { get; set; }
        /// <summary>
        /// 视图标识:多值按,隔开，值域:zx-机构视图、zy-核算组织视图、ZG-管理组织视图、Z4-税务代管组织（行政）视图。
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "ViewIdentifier")]
        public string? VIEW_FLAG { get; set; }
        /// <summary>
        /// 企业分类代码:企业分类,如果多值则按“,”（英文逗号）进行隔开
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "EnterpriseCode")]
        public string? ZENTC { get; set; }
    }
}
