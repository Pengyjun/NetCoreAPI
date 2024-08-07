using SqlSugar;
using System.ComponentModel.DataAnnotations.Schema;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 项目类
    /// </summary>
    [SugarTable("t_Project", IsDisabledDelete = true)]
    public class Project : BaseEntity<long>
    {
        /// <summary>
        /// 发送记录ID 发送记录的ID，必须保证此ID在同一个发送批次中是唯一的。用于记录发送方对于此发送记录的唯一标识。
        /// </summary>
        [NotMapped]
        public string ZZSERIAL { get; set; }
        /// <summary>
        /// 项目主数据编码
        /// </summary>
        [SugarColumn(Length = 12, ColumnName = "ProjectMDCode")]
        public string ZPROJECT { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "ProjectName")]
        public string? ZPROJNAME { get; set; }
        /// <summary>
        /// 项目外文名称
        /// </summary>
        [SugarColumn(Length = 500, ColumnName = "ProjectForeignName")]
        public string? ZPROJENAME { get; set; }
        /// <summary>
        /// 项目类型
        /// </summary>
        [SugarColumn(Length = 6, ColumnName = "ProjectType")]
        public string? ZPROJTYPE { get; set; }
        /// <summary>
        /// 国家/地区
        /// </summary>
        [SugarColumn(Length = 3, ColumnName = "Country")]
        public string? ZZCOUNTRY { get; set; }
        /// <summary>
        /// 项目所在地
        /// </summary>
        [SugarColumn(Length = 6, ColumnName = "ProjectLocation")]
        public string? ZPROJLOC { get; set; }
        /// <summary>
        /// 中交项目业务分类
        /// </summary>
        [SugarColumn(Length = 5, ColumnName = "BusinessClassificationOfCCCCProjects")]
        public string? ZCPBC { get; set; }
        /// <summary>
        /// 投资主体
        /// </summary>
        [SugarColumn(Length = 9, ColumnName = "InvestmentSubject")]
        public string? ZINVERSTOR { get; set; }
        /// <summary>
        /// 项目批复/决议文号
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "ResolutionNumber")]
        public string? ZAPPROVAL { get; set; }
        /// <summary>
        /// 项目批复/决议时间
        /// </summary>
        [SugarColumn(Length = 8, ColumnName = "ResolutionTime")]
        public string? ZAPVLDATE { get; set; }
        /// <summary>
        /// 收入来源
        /// </summary>
        [SugarColumn(Length = 2, ColumnName = "SourceOfIncome")]
        public string? ZSI { get; set; }
        /// <summary>
        /// 项目机构
        /// </summary>
        [SugarColumn(Length = 20, ColumnName = "ProjectOrganization")]
        public string ZPRO_ORG { get; set; }
        /// <summary>
        /// 项目简称
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "ProjectAbbreviation")]
        public string? ZHEREINAFTER { get; set; }
        /// <summary>
        /// 上级项目主数据编码
        /// </summary>
        [SugarColumn(Length = 12, ColumnName = "SupProjectMDCode")]
        public string? ZPROJECTUP { get; set; }
        /// <summary>
        /// 项目年份
        /// </summary>
        [SugarColumn(Length = 4, ColumnName = "ProjectYear")]
        public string? ZPROJYEAR { get; set; }
        /// <summary>
        /// 项目计划开始日期
        /// </summary>
        [SugarColumn(Length = 8, ColumnName = "ProjectPlanStartDate")]
        public string? ZSTARTDATE { get; set; }
        /// <summary>
        /// 项目计划完成日期
        /// </summary>
        [SugarColumn(Length = 8, ColumnName = "ProjectPlanCompletionDate")]
        public string? ZFINDATE { get; set; }
        /// <summary>
        /// 项目工程名称
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "ProjectEngineeringName")]
        public string? ZENG { get; set; }
        /// <summary>
        /// 责任主体
        /// </summary>
        [SugarColumn(Length = 20, ColumnName = "ResponsibleParty")]
        public string? ZRESP { get; set; }
        /// <summary>
        /// 土地成交确认书编号/收并购协议编号
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "LandTransactionConfirmationNumber")]
        public string? ZLDLOC { get; set; }
        /// <summary>
        /// 项目获取时间
        /// </summary>
        [SugarColumn(Length = 8, ColumnName = "ProjectAcquisitionTime")]
        public string? ZLDLOCGT { get; set; }
        /// <summary>
        /// 工商变更时间
        /// </summary>
        [SugarColumn(Length = 8, ColumnName = "BusinessChangeTime")]
        public string? ZCBR { get; set; }
        /// <summary>
        /// 操盘情况
        /// </summary>
        [SugarColumn(Length = 2, ColumnName = "TradingSituation")]
        public string? ZTRADER { get; set; }
        /// <summary>
        /// 保险机构名称
        /// </summary>
        [SugarColumn(Length = 20, ColumnName = "NameOfInsuranceInstitution")]
        public string? ZINSURANCE { get; set; }
        /// <summary>
        /// 保单号 
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "PolicyNo")]
        public string? ZPOLICYNO { get; set; }
        /// <summary>
        /// 投保人 
        /// </summary>
        [SugarColumn(Length = 20, ColumnName = "Applicant")]
        public string? ZINSURED { get; set; }
        /// <summary>
        /// 保险起始日期
        /// </summary>
        [SugarColumn(Length = 8, ColumnName = "StartingDateOfInsurance")]
        public string? ZISTARTDATE { get; set; }
        /// <summary>
        /// 保险终止日期
        /// </summary>
        [SugarColumn(Length = 8, ColumnName = "TerminationDateOfInsurance")]
        public string? ZIFINDATE { get; set; }
        /// <summary>
        /// 基金主数据编码 
        /// </summary>
        [SugarColumn(Length = 12, ColumnName = "FundMDCode")]
        public string? ZFUND { get; set; }
        /// <summary>
        /// 基金名称
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "FundName")]
        public string? ZFUNDNAME { get; set; }
        /// <summary>
        /// 基金编号
        /// </summary>
        [SugarColumn(Length = 6, ColumnName = "FundNo")]
        public string? ZFUNDNO { get; set; }
        /// <summary>
        /// 币种
        /// </summary>
        [SugarColumn(Length = 3, ColumnName = "Currency")]
        public string? ZZCURRENCY { get; set; }
        /// <summary>
        /// 基金组织形式
        /// </summary>
        [SugarColumn(Length = 3, ColumnName = "FormOfFundOrganization")]
        public string? ZFUNDORGFORM { get; set; }
        /// <summary>
        /// 项目机构
        /// </summary>
        [SugarColumn(Length = 10, ColumnName = "ProjectOrganization")]
        public string? ZPRO_BP { get; set; }
        /// <summary>
        /// 基金管理人类型
        /// </summary>
        [SugarColumn(Length = 8, ColumnName = "TypeOfFundManager")]
        public string? ZFUNDMTYPE { get; set; }
        /// <summary>
        /// 基金成立日期
        /// </summary>
        [SugarColumn(Length = 8, ColumnName = "FundEstablishmentDate")]
        public string? ZFSTARTDATE { get; set; }
        /// <summary>
        /// 基金到期日期
        /// </summary>
        [SugarColumn(Length = 8, ColumnName = "FundExpirationDate")]
        public string? ZFFINDATE { get; set; }
        /// <summary>
        /// 托管人名称 
        /// </summary>
        [SugarColumn(Length = 20, ColumnName = "CustodianName")]
        public string? ZCUSTODIAN { get; set; }
        /// <summary>
        /// 承租人名称 
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "TenantName")]
        public string? ZLESSEE { get; set; }
        /// <summary>
        /// 承租人类型
        /// </summary>
        [SugarColumn(Length = 3, ColumnName = "TenantType")]
        public string? ZLESSEETYPE { get; set; }
        /// <summary>
        /// 租赁物名称 
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "NameOfLeasedProperty")]
        public string? ZLEASESNAME { get; set; }
        /// <summary>
        /// 起租日期
        /// </summary>
        [SugarColumn(Length = 8, ColumnName = "LeaseStartDate")]
        public string? ZLSTARTDATE { get; set; }
        /// <summary>
        /// 到期日期
        /// </summary>
        [SugarColumn(Length = 8, ColumnName = "DueDate")]
        public string ZLFINDATE { get; set; }
        /// <summary>
        /// 所属二级单位:二级单位组织机构编码OID
        /// </summary>
        [SugarColumn(Length = 20, ColumnName = "SecondUnit")]
        public string Z2NDORG { get; set; }
        /// <summary>
        /// 状态：数据是否有效的标识:有效：1无效：0
        /// </summary>
        [SugarColumn(Length = 1, ColumnName = "State")]
        public string ZSTATE { get; set; }
        /// <summary>
        /// 停用原因 1完工停用2错误停用
        /// </summary>
        [SugarColumn(Length = 1, ColumnName = "ReasonForDiscontinuation")]
        public string? ZSTOPREASON { get; set; }
        /// <summary>
        /// 计税方式
        /// </summary>
        [SugarColumn(Length = 1, ColumnName = "")]
        public string ZTAXMETHOD { get; set; }
        /// <summary>
        /// 项目组织形式
        /// </summary>
        [SugarColumn(Length = 4, ColumnName = "TaxationMethod")]
        public string? ZPOS { get; set; }
        /// <summary>
        /// 中标主体
        /// </summary>
        [SugarColumn(Length = 500, ColumnName = "WinningBidder")]
        public string? ZAWARDMAI { get; set; }
        /// <summary>
        /// 并表情况
        /// </summary>
        [SugarColumn(Length = 2, ColumnName = "ConsolidatedSituation")]
        public string? ZCS { get; set; }
        /// <summary>
        /// 所属事业部
        /// </summary>
        [SugarColumn(Length = 2, ColumnName = "BusinessUnit")]
        public string? ZBIZDEPT { get; set; }
        /// <summary>
        /// 项目管理方式:该项目适用的管理方式
        /// </summary>
        [SugarColumn(Length = 2, ColumnName = "ProjectManagementApproach")]
        public string ZMANAGE_MODE { get; set; }
        /// <summary>
        /// 参与二级单位：该项目参与的其他二级单位，支持多值
        /// </summary>
        [SugarColumn(Length = 500, ColumnName = "ParticipateInSecondUnits")]
        public string? ZCY2NDORG { get; set; }
        /// <summary>
        /// 是否联合体项目：是否联合体：1是，2否
        /// </summary>
        [SugarColumn(Length = 1, ColumnName = "IsConsortiumProject")]
        public string ZWINNINGC { get; set; }
        /// <summary>
        /// 中标交底项目编号：传入多值时用逗号给开
        /// </summary>
        [SugarColumn(ColumnDataType = "text", ColumnName = "BidDisclosureProjectNumber")]
        public string ZAWARDP { get; set; }
        /// <summary>
        /// 创建时间：格式：YYYYMMDDHHMMSS
        /// </summary>
        [SugarColumn(Length = 20, ColumnName = "CreateDate")]
        public string ZCREATE_AT { get; set; }
        /// <summary>
        /// 曾用名列表
        /// </summary>
        [NotMapped]
        public List<ZMDGS_OLDNAME>? ZOLDNAME_LIST { get; set; }
    }
    /// <summary>
    /// 曾用名列表
    /// </summary>
    public class ZMDGS_OLDNAME
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
}
