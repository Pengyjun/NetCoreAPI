using GDCMasterDataReceiveApi.Application.Contracts.Dto.BankCard;
using System.ComponentModel;

namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.CorresUnit
{
    /// <summary>
    /// 往来单位主数据 反显
    /// </summary>
    public class CorresUnitSearchDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public string? Id { get; set; }
        /// <summary>
        /// 名称（中文）:往来单位中文名称，境内单位必填
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 名称（英文）:往来单位英文名称，境外单位必填，
        /// </summary>
        public string? NameEnglish { get; set; }
        /// <summary>
        /// 名称（当地语言）:往来单位当地官方语言名称，当地语言为英文时，该属性与名称（英文）填写内容相同，境外单位必填
        /// </summary>
        public string? NameInLLanguage { get; set; }
        /// <summary>
        /// 往来单位类别:依据组织机构的功能和性质进行的分类
        /// </summary>
        public string? CategoryUnit { get; set; }
        /// <summary>
        /// 国家/地区:往来单位所在国家/地区
        /// </summary>
        public string? Country { get; set; }
        /// <summary>
        /// 省:境内单位注册地所在省/直辖市/自治区/特别行政区，境内单位必填
        /// </summary>
        public string? Province { get; set; }
        /// <summary>
        /// 市:境内单位注册地所在市/地区/自治州/盟，境内单位必填
        /// </summary>
        public string? City { get; set; }
        /// <summary>
        /// 县:境内单位注册地所在县（自治县、县级市、旗、自治旗、市辖区、林区、特区），境内单位必填
        /// </summary>
        public string? County { get; set; }
        /// <summary>
        /// 往来单位性质:国资监管维度
        /// </summary>
        public string? NatureOfUnit { get; set; }
        /// <summary>
        /// 往来单位状态:标记往来单位的启/停用状态01启用/02停用
        /// </summary>
        public string? StatusOfUnit { get; set; }
        /// <summary>
        /// 往来单位类型:01客户02供应商03分包商
        /// </summary>
        public string? TypeOfUnit { get; set; }
    }
    /// <summary>
    /// 往来单位详情
    /// </summary>
    public class CorresUnitDetailsDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public string? Id { get; set; }
        /// <summary>
        /// 往来单位主数据编码:公司往来单位的唯一编码
        /// </summary>
        [DisplayName("往来单位主数据编码")]
        public string? DealUnitMDCode { get; set; }
        /// <summary>
        /// 是否集团内单位:是否为中交集团内部单位0:否1:是
        /// </summary>
        [DisplayName("是否集团内单位")]
        public string? IsGroupUnit { get; set; }
        /// <summary>
        /// 核算单位编码:财务核算组织的编码，内部核算单位必填
        /// </summary>
        [DisplayName("核算单位编码")]
        public string? AccUnitCode { get; set; }
        /// <summary>
        /// 机构主数据编码:行政组织的编码，内部行政组织必填
        /// </summary>
        [DisplayName("机构主数据编码")]
        public string? OrgMDCode { get; set; }
        /// <summary>
        /// 名称（中文）:往来单位中文名称，境内单位必填
        /// </summary>
        [DisplayName("名称（中文）")]
        public string? Name { get; set; }
        /// <summary>
        /// 名称（英文）:往来单位英文名称，境外单位必填，
        /// </summary>
        [DisplayName("名称（英文）")]
        public string? NameEnglish { get; set; }
        /// <summary>
        /// 名称（当地语言）:往来单位当地官方语言名称，当地语言为英文时，该属性与名称（英文）填写内容相同，境外单位必填
        /// </summary>
        [DisplayName("名称（当地语言）")]
        public string? NameInLLanguage { get; set; }
        /// <summary>
        /// 往来单位类别:依据组织机构的功能和性质进行的分类
        /// </summary>
        [DisplayName("往来单位类别")]
        public string? CategoryUnit { get; set; }
        /// <summary>
        /// 统一社会信用代码:境内单位-企业的唯一性校验标准
        /// </summary>
        [DisplayName("统一社会信用代码")]
        public string? RegistrationNo { get; set; }
        /// <summary>
        /// 组织机构代码:境内单位获取的组织机构代码证的编号
        /// </summary>
        [DisplayName("组织机构代码")]
        public string? OrgCode { get; set; }
        /// <summary>
        /// 工商注册号:境内单位-企业的工商注册登记号
        /// </summary>
        [DisplayName("工商注册号")]
        public string? BRegistrationNo { get; set; }
        /// <summary>
        /// 纳税人识别号:境内单位-企业在税务登记证上的号码
        /// </summary>
        [DisplayName("纳税人识别号")]
        public string? TaxpayerIdentifyNo { get; set; }
        /// <summary>
        /// 境外注册号:境外单位依法注册的组织编号
        /// </summary>
        [DisplayName("境外注册号")]
        public string? AbroadRegistrationNo { get; set; }
        /// <summary>
        /// 身份证号码:境内个人必填且为唯一性校验标准
        /// </summary>
        [DisplayName("身份证号码")]
        public string? IdNo { get; set; }
        /// <summary>
        /// 境外社保号/ID:境外个人社保号或境外个人ID（ID针对不同国家可填写为身份证号或护照号），境外个人必填且为唯一性校验标准
        /// </summary>
        [DisplayName("境外社保号/ID")]
        public string? AbroadSocialSecurityNo { get; set; }
        /// <summary>
        /// 银行账号列表
        /// </summary>
        [DisplayName("银行账号列表")]
        public List<BankCardDetailsDto>?BankList { get; set; }
        /// <summary>
        /// 国家/地区:往来单位所在国家/地区
        /// </summary>
        [DisplayName("国家/地区")]
        public string? Country { get; set; }
        /// <summary>
        /// 省:境内单位注册地所在省/直辖市/自治区/特别行政区，境内单位必填
        /// </summary>
        [DisplayName("省")]
        public string? Province { get; set; }
        /// <summary>
        /// 市:境内单位注册地所在市/地区/自治州/盟，境内单位必填
        /// </summary>
        [DisplayName("市")]
        public string? City { get; set; }
        /// <summary>
        /// 县:境内单位注册地所在县（自治县、县级市、旗、自治旗、市辖区、林区、特区），境内单位必填
        /// </summary>
        [DisplayName("县")]
        public string? County { get; set; }
        /// <summary>
        /// 企业性质:工商行政管理部门对企业登记注册的类型，境内单位-企业且为集团外单位必填
        /// </summary>
        [DisplayName("企业性质")]
        public string? EnterpriseNature { get; set; }
        /// <summary>
        /// 上级法人单位:往来单位的上级法人单位
        /// </summary>
        [DisplayName("上级法人单位")]
        public string? SupLegalEntity { get; set; }
        /// <summary>
        /// 往来单位性质:国资监管维度
        /// </summary>
        [DisplayName("往来单位性质")]
        public string? NatureOfUnit { get; set; }
        /// <summary>
        /// 来源系统:记录数据来源系统
        /// </summary>
        [DisplayName("来源系统")]
        public string? SourceSystem { get; set; }
        /// <summary>
        /// 往来单位类型:01客户02供应商03分包商
        /// </summary>
        [DisplayName("往来单位类型")]
        public string? TypeOfUnit { get; set; }
        /// <summary>
        /// 创建时间:格式：20230324121212
        /// </summary>
        [DisplayName("创建时间")]
        public decimal CreatTime { get; set; }
        /// <summary>
        /// 创建人:4A人员编码
        /// </summary>
        [DisplayName("创建人")]
        public string? CreateBy { get; set; }
        /// <summary>
        /// 修改时间:格式：20230324121212
        /// </summary>
        [DisplayName("修改时间")]
        public decimal ChangeTime { get; set; }
        /// <summary>
        /// 修改人:4A人员编码
        /// </summary>
        [DisplayName("修改人")]
        public string? ModifiedBy { get; set; }
        /// <summary>
        /// 所属二级单位:记录数据最后修改人的所属二级单位
        /// </summary>
        [DisplayName("所属二级单位")]
        public string? UnitSec { get; set; }
        /// <summary>
        /// 往来单位状态:标记往来单位的启/停用状态01启用/02停用
        /// </summary>
        [DisplayName("往来单位状态")]
        public string? StatusOfUnit { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
    /// <summary>
    /// 往来单位主数据 接收
    /// </summary>
    public class CorresUnitReceiveDto
    {
        /// <summary>
        /// 发送记录ID 发送记录的ID，必须保证此ID在同一个发送批次中是唯一的。用于记录发送方对于此发送记录的唯一标识。
        /// </summary>
        public  long? Id { get; set; }
        /// <summary>
        /// 往来单位主数据编码:公司往来单位的唯一编码
        /// </summary>
        public string? ZBP { get; set; }
        /// <summary>
        /// 是否集团内单位:是否为中交集团内部单位0:否1:是
        /// </summary>
        public string? ZINCLIENT { get; set; }
        /// <summary>
        /// 核算单位编码:财务核算组织的编码，内部核算单位必填
        /// </summary>
        public string? ZACORGNO { get; set; }
        /// <summary>
        /// 机构主数据编码:行政组织的编码，内部行政组织必填
        /// </summary>
        public string? ZORG { get; set; }
        /// <summary>
        /// 名称（中文）:往来单位中文名称，境内单位必填
        /// </summary>
        public string? ZBPNAME_ZH { get; set; }
        /// <summary>
        /// 名称（英文）:往来单位英文名称，境外单位必填，
        /// </summary>
        public string? ZBPNAME_EN { get; set; }
        /// <summary>
        /// 名称（当地语言）:往来单位当地官方语言名称，当地语言为英文时，该属性与名称（英文）填写内容相同，境外单位必填
        /// </summary>
        public string? ZBPNAME_LOC { get; set; }
        /// <summary>
        /// 往来单位类别:依据组织机构的功能和性质进行的分类
        /// </summary>
        public string? ZBPTYPE { get; set; }
        /// <summary>
        /// 统一社会信用代码:境内单位-企业的唯一性校验标准
        /// </summary>
        public string? ZUSCC { get; set; }
        /// <summary>
        /// 组织机构代码:境内单位获取的组织机构代码证的编号
        /// </summary>
        public string? ZOIBC { get; set; }
        /// <summary>
        /// 工商注册号:境内单位-企业的工商注册登记号
        /// </summary>
        public string? ZBRNO { get; set; }
        /// <summary>
        /// 纳税人识别号:境内单位-企业在税务登记证上的号码
        /// </summary>
        public string? ZTRNO { get; set; }
        /// <summary>
        /// 境外注册号:境外单位依法注册的组织编号
        /// </summary>
        public string? ZOSRNO { get; set; }
        /// <summary>
        /// 身份证号码:境内个人必填且为唯一性校验标准
        /// </summary>
        public string? ZIDNO { get; set; }
        /// <summary>
        /// 境外社保号/ID:境外个人社保号或境外个人ID（ID针对不同国家可填写为身份证号或护照号），境外个人必填且为唯一性校验标准
        /// </summary>
        public string? ZSSNO { get; set; }
        /// <summary>
        /// 国家/地区:往来单位所在国家/地区
        /// </summary>
        public string? ZZCOUNTRY { get; set; }
        /// <summary>
        /// 省:境内单位注册地所在省/直辖市/自治区/特别行政区，境内单位必填
        /// </summary>
        public string? ZPROVINCE { get; set; }
        /// <summary>
        /// 市:境内单位注册地所在市/地区/自治州/盟，境内单位必填
        /// </summary>
        public string? ZCITY { get; set; }
        /// <summary>
        /// 县:境内单位注册地所在县（自治县、县级市、旗、自治旗、市辖区、林区、特区），境内单位必填
        /// </summary>
        public string? ZCOUNTY { get; set; }
        /// <summary>
        /// 企业性质:工商行政管理部门对企业登记注册的类型，境内单位-企业且为集团外单位必填
        /// </summary>
        public string? ZETPSPROPERTY { get; set; }
        /// <summary>
        /// 上级法人单位:往来单位的上级法人单位
        /// </summary>
        public string? ZCOMPYREL { get; set; }
        /// <summary>
        /// 往来单位性质:国资监管维度
        /// </summary>
        public string? ZBPNATURE { get; set; }
        /// <summary>
        /// 往来单位状态:标记往来单位的启/停用状态01启用/02停用
        /// </summary>
        public string? ZBPSTATE { get; set; }
        /// <summary>
        /// 来源系统:记录数据来源系统
        /// </summary>
        public string? ZSYSTEM { get; set; }
        /// <summary>
        /// 往来单位类型:01客户02供应商03分包商
        /// </summary>
        public string? ZBPKINDS { get; set; }
        /// <summary>
        /// 创建时间:格式：20230324121212
        /// </summary>
        public string? ZCRAT { get; set; }
        /// <summary>
        /// 创建人:4A人员编码
        /// </summary>
        public string? ZCRBY { get; set; }
        /// <summary>
        /// 修改时间:格式：20230324121212
        /// </summary>
        public decimal ZCHAT { get; set; }
        /// <summary>
        /// 修改人:4A人员编码
        /// </summary>
        public string? ZCHBY { get; set; }
        /// <summary>
        /// 所属二级单位:记录数据最后修改人的所属二级单位
        /// </summary>
        public string? Z2NDORG { get; set; }
        /// <summary>
        /// 银行账号
        /// </summary>
        public BankItem? ZBANK {  get; set; }
    }

    public class BankItem
    {
        public List<ZBANKLIST> Item { get; set; }
    }
    public class ZBANKLIST
    {
        /// <summary>
        /// 发送记录ID 发送记录的ID，必须保证此ID在同一个发送批次中是唯一的。用于记录发送方对于此发送记录的唯一标识。
        /// </summary>
        public string? ZZSERIAL { get; set; }
        /// <summary>
        /// 往来单位主数据编码:公司往来单位的唯一编码
        /// </summary>
        public string? ZBP { get; set; }
        /// <summary>
        /// 银行账号主键：银行账号主键
        /// </summary>
        public string? ZBANK { get; set; }
        /// <summary>
        /// 账户名称：填报银行账户的全称
        /// </summary>
        public string? ZKOINH { get; set; }
        /// <summary>
        /// 银行账号/IBAN：填报银行账号/IBAN，该字段作为银行账户的唯一标识
        /// </summary>
        public string? ZBANKN { get; set; }
        /// <summary>
        /// 金融机构编码：填报具体开户网点，引用金融机构主数据
        /// </summary>
        public string? ZFINC { get; set; }
        /// <summary>
        /// 金融机构名称：填报具体开户网点，引用金融机构主数据
        /// </summary>
        public string? ZFINAME { get; set; }
        /// <summary>
        /// 账户状态：银行账户所处状态，包括正常、冻结、其他，默认正常。
        /// </summary>
        public string? ZBANKSTA { get; set; }
        /// <summary>
        /// 账户币种：按账户实际币种填报。默认人民币。可多值以英文逗号隔开。
        /// </summary>
        public string? ZCURR { get; set; }
        /// <summary>
        /// 国家/地区：金融机构对应的国家/地区
        /// </summary>
        public string? ZZCOUNTR2 { get; set; }
        /// <summary>
        /// 省：金融机构对应的省，条件必填，国家/地区为中国的必填
        /// </summary>
        public string? ZPROVINC2 { get; set; }
        /// <summary>
        /// 市：金融机构对应的市，条件必填，国家/地区为中国的必填
        /// </summary>
        public string? ZCITY2 { get; set; }
        /// <summary>
        /// 县：2023.2改为非必填
        /// </summary>
        public string? ZCOUNTY2 { get; set; }
    }
}
