using MiniExcelLibs.Attributes;

namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.Project
{


    #region 项目  反显   后台管理使用
    /// <summary>
    /// 项目  反显
    /// </summary>
    public class ProjectSearchDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 项目主数据编码
        /// </summary>
        public string MDCode { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 项目外文名称
        /// </summary>
        public string? ForeignName { get; set; }
        /// <summary>
        /// 项目类型
        /// </summary>
        public string? Type { get; set; }
        /// <summary>
        /// 国家/地区
        /// </summary>
        public string? Country { get; set; }
        /// <summary>
        /// 项目所在地
        /// </summary>
        public string? Location { get; set; }
        /// <summary>
        /// 项目批复/决议文号
        /// </summary>
        public string? ResolutionNo { get; set; }
        /// <summary>
        /// 项目批复/决议时间
        /// </summary>
        public string? ResolutionTime { get; set; }
        /// <summary>
        /// 项目机构
        /// </summary>
        public string PjectOrg { get; set; }
        /// <summary>
        /// 项目简称
        /// </summary>
        public string? Abbreviation { get; set; }
        /// <summary>
        /// 项目年份
        /// </summary>
        public string? Year { get; set; }
        /// <summary>
        /// 项目计划开始日期
        /// </summary>
        public string? PlanStartDate { get; set; }
        /// <summary>
        /// 项目计划完成日期
        /// </summary>
        public string? PlanCompletionDate { get; set; }
        /// <summary>
        /// 项目工程名称
        /// </summary>
        public string? EngineeringName { get; set; }
        /// <summary>
        /// 币种
        /// </summary>
        public string? Currency { get; set; }
        /// <summary>
        /// 项目机构
        /// </summary>
        public string? PjectOrgBP { get; set; }
        /// <summary>
        /// 所属二级单位:二级单位组织机构编码OID
        /// </summary>
        public string UnitSec { get; set; }
        /// <summary>
        /// 状态：数据是否有效的标识:有效：1无效：0
        /// </summary>
        public string State { get; set; }
    }
    /// <summary>
    /// 项目详情dto
    /// </summary>
    public class ProjectDetailsDto
    {
        [ExcelIgnore]
        public string Id { get; set;}
        /// <summary>
        /// 项目主数据编码
        /// </summary>
        [ExcelColumnName("项目主数据编码")]
        public string? MDCode { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        [ExcelColumnName("项目名称")]
        public string? Name { get; set; }
        /// <summary>
        /// 项目外文名称
        /// </summary>
        [ExcelColumnName("项目外文名称")]
        public string? ForeignName { get; set; }
        /// <summary>
        /// 项目类型
        /// </summary>
        [ExcelColumnName("项目类型")]
        public string? Type { get; set; }
        /// <summary>
        /// 国家/地区
        /// </summary>
        [ExcelColumnName("国家/地区")]
        public string? Country { get; set; }
        /// <summary>
        /// 项目所在地
        /// </summary>
        [ExcelColumnName("项目所在地")]
        public string? Location { get; set; }
        /// <summary>
        /// 中交项目业务分类
        /// </summary>
        [ExcelIgnore]
        public string? BTypeOfCCCC { get; set; }
        /// <summary>
        /// 投资主体
        /// </summary>
        [ExcelIgnore]
        public string? Invest { get; set; }
        /// <summary>
        /// 项目批复/决议文号
        /// </summary>
        [ExcelColumnName("项目批复/决议文号")]
        public string? ResolutionNo { get; set; }
        /// <summary>
        /// 项目批复/决议时间
        /// </summary>
        [ExcelColumnName("项目批复/决议时间")]
        public string? ResolutionTime { get; set; }
        /// <summary>
        /// 收入来源
        /// </summary>
        [ExcelIgnore]
        public string? SourceOfIncome { get; set; }
        /// <summary>
        /// 项目机构
        /// </summary>
        [ExcelIgnore]
        public string? PjectOrg { get; set; }
        /// <summary>
        /// 项目简称
        /// </summary>
        [ExcelColumnName("项目简称")]
        public string? Abbreviation { get; set; }
        /// <summary>
        /// 上级项目主数据编码
        /// </summary>
        [ExcelColumnName("上级项目主数据编码")]
        public string? SupMDCode { get; set; }
        /// <summary>
        /// 项目年份
        /// </summary>
        [ExcelIgnore]
        public string? Year { get; set; }
        /// <summary>
        /// 项目计划开始日期yyyymmdd
        /// </summary>
        [ExcelColumnName("项目计划开始日期")]
        public string? PlanStartDate { get; set; }
        /// <summary>
        /// 项目计划完成日期yyyymmdd
        /// </summary>
        [ExcelColumnName("项目计划完成日期")]
        public string? PlanCompletionDate { get; set; }
        /// <summary>
        /// 项目工程名称
        /// </summary>
        [ExcelColumnName("项目工程名称")]
        public string? EngineeringName { get; set; }
        /// <summary>
        /// 责任主体
        /// </summary>
        [ExcelIgnore]
        public string? ResponsibleParty { get; set; }
        /// <summary>
        /// 土地成交确认书编号/收并购协议编号
        /// </summary>
        [ExcelIgnore]
        public string? LandTransactionNo { get; set; }
        /// <summary>
        /// 项目获取时间
        /// </summary>
        [ExcelColumnName("项目获取时间")]
        public string? AcquisitionTime { get; set; }
        /// <summary>
        /// 工商变更时间yyyymmdd
        /// </summary>
        [ExcelIgnore]
        public string? BChangeTime { get; set; }
        /// <summary>
        /// 操盘情况
        /// </summary>
        [ExcelIgnore]
        public string? TradingSituation { get; set; }
        /// <summary>
        /// 保险机构名称
        /// </summary>
        [ExcelIgnore]
        public string? NameOfInsureOrg { get; set; }
        /// <summary>
        /// 保单号 
        /// </summary>
        [ExcelIgnore]
        public string? PolicyNo { get; set; }
        /// <summary>
        /// 投保人 
        /// </summary>
        [ExcelIgnore]
        public string? Applicant { get; set; }
        /// <summary>
        /// 保险起始日期yyyymmdd
        /// </summary>
        [ExcelIgnore]
        public string? StartDateOfInsure { get; set; }
        /// <summary>
        /// 保险终止日期yyyymmdd
        /// </summary>
        [ExcelIgnore]
        public string? EndDateOfInsure { get; set; }
        /// <summary>
        /// 基金主数据编码 
        /// </summary>
        [ExcelIgnore]
        public string? FundMDCode { get; set; }
        /// <summary>
        /// 基金名称
        /// </summary>
        [ExcelIgnore]
        public string? FundName { get; set; }
        /// <summary>
        /// 基金编号
        /// </summary>
        [ExcelIgnore]
        public string? FundNo { get; set; }
        /// <summary>
        /// 币种
        /// </summary>
        [ExcelIgnore]
        public string? Currency { get; set; }
        /// <summary>
        /// 基金组织形式
        /// </summary>
        [ExcelIgnore]
        public string? FundOrgForm { get; set; }
        /// <summary>
        /// 项目机构
        /// </summary>
        [ExcelIgnore]
        public string? PjectOrgBP { get; set; }
        /// <summary>
        /// 基金管理人类型
        /// </summary>
        [ExcelIgnore]
        public string? FundManager { get; set; }
        /// <summary>
        /// 基金成立日期yyyymmdd
        /// </summary>
        [ExcelIgnore]
        public string? FundEstablishmentDate { get; set; }
        /// <summary>
        /// 基金到期日期yyyymmdd
        /// </summary>
        [ExcelIgnore]
        public string? FundExpirationDate { get; set; }
        /// <summary>
        /// 托管人名称 
        /// </summary>
        [ExcelIgnore]
        public string? CustodianName { get; set; }
        /// <summary>
        /// 承租人名称 
        /// </summary>
        [ExcelIgnore]
        public string? TenantName { get; set; }
        /// <summary>
        /// 承租人类型
        /// </summary>
        [ExcelIgnore]
        public string? TenantType { get; set; }
        /// <summary>
        /// 租赁物名称 
        /// </summary>
        [ExcelIgnore]
        public string? NameOfLeased { get; set; }
        /// <summary>
        /// 起租日期yyyymmdd
        /// </summary>
        [ExcelIgnore]
        public string? LeaseStartDate { get; set; }
        /// <summary>
        /// 到期日期yyyymmdd
        /// </summary>
        [ExcelIgnore]
        public string? DueDate { get; set; }
        /// <summary>
        /// 所属二级单位:二级单位组织机构编码OID
        /// </summary>
        [ExcelIgnore]
        public string? UnitSec { get; set; }
        /// <summary>
        /// 状态：数据是否有效的标识:有效：1无效：0
        /// </summary>
        [ExcelIgnore]
        public string? State { get; set; }
        /// <summary>
        /// 停用原因 1完工停用2错误停用
        /// </summary>
        [ExcelIgnore]
        public string? ReasonForDeactivate { get; set; }
        /// <summary>
        /// 计税方式
        /// </summary>
        [ExcelIgnore]
        public string? TaxMethod { get; set; }
        /// <summary>
        /// 项目组织形式
        /// </summary>
        [ExcelIgnore]
        public string? OrgMethod { get; set; }
        /// <summary>
        /// 中标主体
        /// </summary>
        [ExcelIgnore]
        public string? WinningBidder { get; set; }
        /// <summary>
        /// 并表情况
        /// </summary>
        [ExcelIgnore]
        public string? ConsolidatedTable { get; set; }
        /// <summary>
        /// 所属事业部
        /// </summary>
        [ExcelIgnore]
        public string? BDep { get; set; }
        /// <summary>
        /// 项目管理方式:该项目适用的管理方式
        /// </summary>
        [ExcelIgnore]
        public string? Management { get; set; }
        /// <summary>
        /// 参与二级单位：该项目参与的其他二级单位，支持多值
        /// </summary>
        [ExcelIgnore]
        public string? ParticipateInUnitSecs { get; set; }
        /// <summary>
        /// 是否联合体项目：是否联合体：1是，2否
        /// </summary>
        [ExcelIgnore]
        public string? IsJoint { get; set; }
        /// <summary>
        /// 中标交底项目编号：传入多值时用逗号给开
        /// </summary>
        [ExcelIgnore]
        public string? BidDisclosureNo { get; set; }
        /// <summary>
        /// 创建时间：格式：YYYYMMDDHHMMSS
        /// </summary>
        [ExcelIgnore]
        public string? CreateDate { get; set; }
        [ExcelIgnore]
        public DateTime? CreateTime { get; set; }
        [ExcelIgnore]
        public DateTime? UpdateTime { get; set; }

    }

    #endregion


    #region 项目 接收   
    /// <summary>
    /// 项目 接收
    /// </summary>
    public class ProjectItem
    {
        /// <summary>
        /// 发送记录ID 发送记录的ID，必须保证此ID在同一个发送批次中是唯一的。用于记录发送方对于此发送记录的唯一标识。
        /// </summary>
        public long? Id { get; set; }
        /// <summary>
        /// 项目主数据编码
        /// </summary>
        public string? ZPROJECT { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string? ZPROJNAME { get; set; }
        /// <summary>
        /// 项目外文名称
        /// </summary>
        public string? ZPROJENAME { get; set; }
        /// <summary>
        /// 项目类型
        /// </summary>
        public string? ZPROJTYPE { get; set; }
        /// <summary>
        /// 国家/地区
        /// </summary>
        public string? ZZCOUNTRY { get; set; }
        /// <summary>
        /// 项目所在地
        /// </summary>
        public string? ZPROJLOC { get; set; }
        /// <summary>
        /// 中交项目业务分类
        /// </summary>
        public string? ZCPBC { get; set; }
        /// <summary>
        /// 投资主体
        /// </summary>
        public string? ZINVERSTOR { get; set; }
        /// <summary>
        /// 项目批复/决议文号
        /// </summary>
        public string? ZAPPROVAL { get; set; }
        /// <summary>
        /// 项目批复/决议时间
        /// </summary>
        public string? ZAPVLDATE { get; set; }
        /// <summary>
        /// 收入来源
        /// </summary>
        public string? ZSI { get; set; }
        /// <summary>
        /// 项目机构
        /// </summary>
        public string? ZPRO_ORG { get; set; }
        /// <summary>
        /// 项目简称
        /// </summary>
        public string? ZHEREINAFTER { get; set; }
        /// <summary>
        /// 上级项目主数据编码
        /// </summary>
        public string? ZPROJECTUP { get; set; }
        /// <summary>
        /// 项目年份
        /// </summary>
        public string? ZPROJYEAR { get; set; }
        /// <summary>
        /// 项目计划开始日期
        /// </summary>
        public string? ZSTARTDATE { get; set; }
        /// <summary>
        /// 项目计划完成日期
        /// </summary>
        public string? ZFINDATE { get; set; }
        /// <summary>
        /// 项目工程名称
        /// </summary>
        public string? ZENG { get; set; }
        /// <summary>
        /// 责任主体
        /// </summary>
        public string? ZRESP { get; set; }
        /// <summary>
        /// 土地成交确认书编号/收并购协议编号
        /// </summary>
        public string? ZLDLOC { get; set; }
        /// <summary>
        /// 项目获取时间
        /// </summary>
        public string? ZLDLOCGT { get; set; }
        /// <summary>
        /// 工商变更时间
        /// </summary>
        public string? ZCBR { get; set; }
        /// <summary>
        /// 操盘情况
        /// </summary>
        public string? ZTRADER { get; set; }
        /// <summary>
        /// 保险机构名称
        /// </summary>
        public string? ZINSURANCE { get; set; }
        /// <summary>
        /// 保单号 
        /// </summary>
        public string? ZPOLICYNO { get; set; }
        /// <summary>
        /// 投保人 
        /// </summary>
        public string? ZINSURED { get; set; }
        /// <summary>
        /// 保险起始日期
        /// </summary>
        public string? ZISTARTDATE { get; set; }
        /// <summary>
        /// 保险终止日期
        /// </summary>
        public string? ZIFINDATE { get; set; }
        /// <summary>
        /// 基金主数据编码 
        /// </summary>
        public string? ZFUND { get; set; }
        /// <summary>
        /// 基金名称
        /// </summary>
        public string? ZFUNDNAME { get; set; }
        /// <summary>
        /// 基金编号
        /// </summary>
        public string? ZFUNDNO { get; set; }
        /// <summary>
        /// 币种
        /// </summary>
        public string? ZZCURRENCY { get; set; }
        /// <summary>
        /// 基金组织形式
        /// </summary>
        public string? ZFUNDORGFORM { get; set; }
        /// <summary>
        /// 项目机构
        /// </summary>
        public string? ZPRO_BP { get; set; }
        /// <summary>
        /// 基金管理人类型
        /// </summary>
        public string? ZFUNDMTYPE { get; set; }
        /// <summary>
        /// 基金成立日期
        /// </summary>
        public string? ZFSTARTDATE { get; set; }
        /// <summary>
        /// 基金到期日期
        /// </summary>
        public string? ZFFINDATE { get; set; }
        /// <summary>
        /// 托管人名称 
        /// </summary>
        public string? ZCUSTODIAN { get; set; }
        /// <summary>
        /// 承租人名称 
        /// </summary>
        public string? ZLESSEE { get; set; }
        /// <summary>
        /// 承租人类型
        /// </summary>
        public string? ZLESSEETYPE { get; set; }
        /// <summary>
        /// 租赁物名称 
        /// </summary>
        public string? ZLEASESNAME { get; set; }
        /// <summary>
        /// 起租日期
        /// </summary>
        public string? ZLSTARTDATE { get; set; }
        /// <summary>
        /// 到期日期
        /// </summary>
        public string? ZLFINDATE { get; set; }
        /// <summary>
        /// 所属二级单位:二级单位组织机构编码OID
        /// </summary>
        public string? Z2NDORG { get; set; }
        /// <summary>
        /// 状态：数据是否有效的标识:有效：1无效：0
        /// </summary>
        public string? ZSTATE { get; set; }
        /// <summary>
        /// 停用原因 1完工停用2错误停用
        /// </summary>
        public string? ZSTOPREASON { get; set; }
        /// <summary>
        /// 计税方式
        /// </summary>
        public string? ZTAXMETHOD { get; set; }
        /// <summary>
        /// 项目组织形式
        /// </summary>
        public string? ZPOS { get; set; }
        /// <summary>
        /// 中标主体
        /// </summary>
        public string? ZAWARDMAI { get; set; }
        /// <summary>
        /// 并表情况
        /// </summary>
        public string? ZCS { get; set; }
        /// <summary>
        /// 所属事业部
        /// </summary>
        public string? ZBIZDEPT { get; set; }
        /// <summary>
        /// 项目管理方式:该项目适用的管理方式
        /// </summary>
        public string? ZMANAGE_MODE { get; set; }
        /// <summary>
        /// 参与二级单位：该项目参与的其他二级单位，支持多值
        /// </summary>
        public string? ZCY2NDORG { get; set; }
        /// <summary>
        /// 是否联合体项目：是否联合体：1是，2否
        /// </summary>
        public string? ZWINNINGC { get; set; }
        /// <summary>
        /// 中标交底项目编号：传入多值时用逗号给开
        /// </summary>
        public string? ZAWARDP { get; set; }
        /// <summary>
        /// 创建时间：格式：YYYYMMDDHHMMSS
        /// </summary>
        public string? ZCREATE_AT { get; set; }
        /// <summary>
        /// 曾用名列表
        /// </summary>
        public ZOLDNAME_LIST? ZOLDNAME_LIST { get; set; } = new ZOLDNAME_LIST();
    }

    /// <summary>
    /// 曾用名列表
    /// </summary>
    public class ZOLDNAME_LIST
    {
       
        /// <summary>
        /// 
        /// </summary>
        public List<ProjectUsedNameDto> item { get; set; } = new List<ProjectUsedNameDto>();
    }
    /// <summary>
    /// 
    /// </summary>
    public class ProjectUsedNameDto
    {
        /// <summary>
        /// 项目主数据编码
        /// 12
        /// </summary>
        public string? ZPROJECT { get; set; }
        /// <summary>
        /// 曾用名
        /// 500
        /// </summary>
        public string? ZOLDNAME { get; set; }
    }
    #endregion
}
