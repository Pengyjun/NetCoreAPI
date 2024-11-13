using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 往来单位主数据
    /// </summary>
    [SugarTable("t_corresunit", IsDisabledDelete = true)]
    public class CorresUnit : BaseEntity<long>
    {
        /// <summary>
        /// 往来单位主数据编码:公司往来单位的唯一编码
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "DealUnitMDCode")]
        public string? ZBP { get; set; }
        /// <summary>
        /// 是否集团内单位:是否为中交集团内部单位0:否1:是
        /// </summary>
        [SugarColumn(Length = 2, ColumnName = "IsGroupUnit")]
        public string? ZINCLIENT { get; set; }
        /// <summary>
        /// 核算单位编码:财务核算组织的编码，内部核算单位必填
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "AccUnitCode")]
        public string? ZACORGNO { get; set; }
        /// <summary>
        /// 机构主数据编码:行政组织的编码，内部行政组织必填
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "OrgMDCode")]
        public string? ZORG { get; set; }
        /// <summary>
        /// 名称（中文）:往来单位中文名称，境内单位必填
        /// </summary>
        [SugarColumn(Length = 256, ColumnName = "Name")]
        public string? ZBPNAME_ZH { get; set; }
        /// <summary>
        /// 名称（英文）:往来单位英文名称，境外单位必填，
        /// </summary>
        [SugarColumn(Length = 256, ColumnName = "NameEnglish")]
        public string? ZBPNAME_EN { get; set; }
        /// <summary>
        /// 名称（当地语言）:往来单位当地官方语言名称，当地语言为英文时，该属性与名称（英文）填写内容相同，境外单位必填
        /// </summary>
        [SugarColumn(Length = 256, ColumnName = "NameInLLanguage")]
        public string? ZBPNAME_LOC { get; set; }
        /// <summary>
        /// 往来单位类别:依据组织机构的功能和性质进行的分类
        /// </summary>
        [SugarColumn(Length = 8, ColumnName = "CategoryUnit")]
        public string? ZBPTYPE { get; set; }
        /// <summary>
        /// 统一社会信用代码:境内单位-企业的唯一性校验标准
        /// </summary>
        [SugarColumn(Length = 32, ColumnName = "RegistrationNo")]
        public string? ZUSCC { get; set; }
        /// <summary>
        /// 组织机构代码:境内单位获取的组织机构代码证的编号
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "OrgCode")]
        public string? ZOIBC { get; set; }
        /// <summary>
        /// 工商注册号:境内单位-企业的工商注册登记号
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "BRegistrationNo")]
        public string? ZBRNO { get; set; }
        /// <summary>
        /// 纳税人识别号:境内单位-企业在税务登记证上的号码
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "TaxpayerIdentifyNo")]
        public string? ZTRNO { get; set; }
        /// <summary>
        /// 境外注册号:境外单位依法注册的组织编号
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "AbroadRegistrationNo")]
        public string? ZOSRNO { get; set; }
        /// <summary>
        /// 身份证号码:境内个人必填且为唯一性校验标准
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "IdNo")]
        public string? ZIDNO { get; set; }
        /// <summary>
        /// 境外社保号/ID:境外个人社保号或境外个人ID（ID针对不同国家可填写为身份证号或护照号），境外个人必填且为唯一性校验标准
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "AbroadSocialSecurityNo")]
        public string? ZSSNO { get; set; }
        /// <summary>
        /// 国家/地区:往来单位所在国家/地区
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "Country")]
        public string? ZZCOUNTRY { get; set; }
        /// <summary>
        /// 省:境内单位注册地所在省/直辖市/自治区/特别行政区，境内单位必填
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "Province")]
        public string? ZPROVINCE { get; set; }
        /// <summary>
        /// 市:境内单位注册地所在市/地区/自治州/盟，境内单位必填
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "City")]
        public string? ZCITY { get; set; }
        /// <summary>
        /// 县:境内单位注册地所在县（自治县、县级市、旗、自治旗、市辖区、林区、特区），境内单位必填
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "County")]
        public string? ZCOUNTY { get; set; }
        /// <summary>
        /// 企业性质:工商行政管理部门对企业登记注册的类型，境内单位-企业且为集团外单位必填
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "EnterpriseNature")]
        public string? ZETPSPROPERTY { get; set; }
        /// <summary>
        /// 上级法人单位:往来单位的上级法人单位
        /// </summary>
        [SugarColumn(Length = 128, ColumnName = "SupLegalEntity")]
        public string? ZCOMPYREL { get; set; }
        /// <summary>
        /// 往来单位性质:国资监管维度
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "NatureOfUnit")]
        public string? ZBPNATURE { get; set; }
        /// <summary>
        /// 往来单位状态:标记往来单位的启/停用状态01启用/02停用
        /// </summary>
        [SugarColumn(Length = 16, ColumnName = "StatusOfUnit")]
        public string? ZBPSTATE { get; set; }
        /// <summary>
        /// 来源系统:记录数据来源系统
        /// </summary>
        [SugarColumn(Length = 128, ColumnName = "SourceSystem")]
        public string? ZSYSTEM { get; set; }
        /// <summary>
        /// 往来单位类型:01客户02供应商03分包商
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "TypeOfUnit")]
        public string? ZBPKINDS { get; set; }
        /// <summary>
        /// 创建时间:格式：20230324121212
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal", Length = 18, ColumnName = "CreatTime")]
        public decimal ZCRAT { get; set; }
        /// <summary>
        /// 创建人:4A人员编码
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "CreateBy")]
        public string? ZCRBY { get; set; }
        /// <summary>
        /// 修改时间:格式：20230324121212
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal", Length = 18, ColumnName = "ChangeTime")]
        public decimal ZCHAT { get; set; }
        /// <summary>
        /// 修改人:4A人员编码
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "ModifiedBy")]
        public string? ZCHBY { get; set; }
        /// <summary>
        /// 所属二级单位:记录数据最后修改人的所属二级单位
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "UnitSec")]
        public string? Z2NDORG { get; set; }
        /// <summary>
        /// 是否属于本系统自己新增或修改 true 是 默认false
        /// </summary>
        public bool OwnerSystem { get; set; } = false;
    }

}
