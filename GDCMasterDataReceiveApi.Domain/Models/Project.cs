using SqlSugar;
using System.ComponentModel.DataAnnotations.Schema;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 项目类
    /// </summary>
    [SugarTable("t_project", IsDisabledDelete = true)]
    public class Project : BaseEntity<long>
    {
        /// <summary>
        /// 发送记录ID 发送记录的ID，必须保证此ID在同一个发送批次中是唯一的。用于记录发送方对于此发送记录的唯一标识。
        /// </summary>
        //[NotMapped]
        //public string? ZZSERIAL { get; set; }
        /// <summary>
        /// 项目主数据编码
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "MDCode")]
        public string? ZPROJECT { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        [SugarColumn(Length = 256, ColumnName = "Name")]
        public string? ZPROJNAME { get; set; }
        /// <summary>
        /// 项目外文名称
        /// </summary>
        [SugarColumn(Length = 256, ColumnName = "ForeignName")]
        public string? ZPROJENAME { get; set; }
        /// <summary>
        /// 项目类型
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "Type")]
        public string? ZPROJTYPE { get; set; }
        /// <summary>
        /// 国家/地区
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "Country")]
        public string? ZZCOUNTRY { get; set; }
        /// <summary>
        /// 项目所在地
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "Location")]
        public string? ZPROJLOC { get; set; }
        /// <summary>
        /// 中交项目业务分类
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "BTypeOfCCCC")]
        public string? ZCPBC { get; set; }
        /// <summary>
        /// 投资主体
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "Invest")]
        public string? ZINVERSTOR { get; set; }
        /// <summary>
        /// 项目批复/决议文号
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "ResolutionNo")]
        public string? ZAPPROVAL { get; set; }
        /// <summary>
        /// 项目批复/决议时间
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "ResolutionTime")]
        public string? ZAPVLDATE { get; set; }
        /// <summary>
        /// 收入来源
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "SourceOfIncome")]
        public string? ZSI { get; set; }
        /// <summary>
        /// 项目机构
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "PjectOrg")]
        public string? ZPRO_ORG { get; set; }
        /// <summary>
        /// 项目简称
        /// </summary>
        [SugarColumn(Length = 256, ColumnName = "Abbreviation")]
        public string? ZHEREINAFTER { get; set; }
        /// <summary>
        /// 上级项目主数据编码
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "SupMDCode")]
        public string? ZPROJECTUP { get; set; }
        /// <summary>
        /// 项目年份
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "Year")]
        public string? ZPROJYEAR { get; set; }
        /// <summary>
        /// 项目计划开始日期
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "PlanStartDate")]
        public string? ZSTARTDATE { get; set; }
        /// <summary>
        /// 项目计划完成日期
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "PlanCompletionDate")]
        public string? ZFINDATE { get; set; }
        /// <summary>
        /// 项目工程名称
        /// </summary>
        [SugarColumn(Length = 256, ColumnName = "EngineeringName")]
        public string? ZENG { get; set; }
        /// <summary>
        /// 责任主体
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "ResponsibleParty")]
        public string? ZRESP { get; set; }
        /// <summary>
        /// 土地成交确认书编号/收并购协议编号
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "LandTransactionNo")]
        public string? ZLDLOC { get; set; }
        /// <summary>
        /// 项目获取时间
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "AcquisitionTime")]
        public string? ZLDLOCGT { get; set; }
        /// <summary>
        /// 工商变更时间
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "BChangeTime")]
        public string? ZCBR { get; set; }
        /// <summary>
        /// 操盘情况
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "TradingSituation")]
        public string? ZTRADER { get; set; }
        /// <summary>
        /// 保险机构名称
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "NameOfInsureOrg")]
        public string? ZINSURANCE { get; set; }
        /// <summary>
        /// 保单号 
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "PolicyNo")]
        public string? ZPOLICYNO { get; set; }
        /// <summary>
        /// 投保人 
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "Applicant")]
        public string? ZINSURED { get; set; }
        /// <summary>
        /// 保险起始日期
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "StartDateOfInsure")]
        public string? ZISTARTDATE { get; set; }
        /// <summary>
        /// 保险终止日期
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "EndDateOfInsure")]
        public string? ZIFINDATE { get; set; }
        /// <summary>
        /// 基金主数据编码 
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "FundMDCode")]
        public string? ZFUND { get; set; }
        /// <summary>
        /// 基金名称
        /// </summary>
        [SugarColumn(Length = 256, ColumnName = "FundName")]
        public string? ZFUNDNAME { get; set; }
        /// <summary>
        /// 基金编号
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "FundNo")]
        public string? ZFUNDNO { get; set; }
        /// <summary>
        /// 币种
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "Currency")]
        public string? ZZCURRENCY { get; set; }
        /// <summary>
        /// 基金组织形式
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "FundOrgForm")]
        public string? ZFUNDORGFORM { get; set; }
        /// <summary>
        /// 项目机构
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "PjectOrgBP")]
        public string? ZPRO_BP { get; set; }
        /// <summary>
        /// 基金管理人类型
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "FundManager")]
        public string? ZFUNDMTYPE { get; set; }
        /// <summary>
        /// 基金成立日期
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "FundEstablishmentDate")]
        public string? ZFSTARTDATE { get; set; }
        /// <summary>
        /// 基金到期日期
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "FundExpirationDate")]
        public string? ZFFINDATE { get; set; }
        /// <summary>
        /// 托管人名称 
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "CustodianName")]
        public string? ZCUSTODIAN { get; set; }
        /// <summary>
        /// 承租人名称 
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "TenantName")]
        public string? ZLESSEE { get; set; }
        /// <summary>
        /// 承租人类型
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "TenantType")]
        public string? ZLESSEETYPE { get; set; }
        /// <summary>
        /// 租赁物名称 
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "NameOfLeased")]
        public string? ZLEASESNAME { get; set; }
        /// <summary>
        /// 起租日期
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "LeaseStartDate")]
        public string? ZLSTARTDATE { get; set; }
        /// <summary>
        /// 到期日期
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "DueDate")]
        public string? ZLFINDATE { get; set; }
        /// <summary>
        /// 所属二级单位:二级单位组织机构编码OID
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "UnitSec")]
        public string? Z2NDORG { get; set; }
        /// <summary>
        /// 状态：数据是否有效的标识:有效：1无效：0
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "State")]
        public string? ZSTATE { get; set; }
        /// <summary>
        /// 停用原因 1完工停用2错误停用
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "ReasonForDeactivate")]
        public string? ZSTOPREASON { get; set; }
        /// <summary>
        /// 计税方式
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "TaxMethod")]
        public string? ZTAXMETHOD { get; set; }
        /// <summary>
        /// 项目组织形式
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "OrgMethod")]
        public string? ZPOS { get; set; }
        /// <summary>
        /// 中标主体
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "WinningBidder")]
        public string? ZAWARDMAI { get; set; }
        /// <summary>
        /// 并表情况
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "ConsolidatedTable")]
        public string? ZCS { get; set; }
        /// <summary>
        /// 所属事业部
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "BDep")]
        public string? ZBIZDEPT { get; set; }
        /// <summary>
        /// 项目管理方式:该项目适用的管理方式
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "Management")]
        public string? ZMANAGE_MODE { get; set; }
        /// <summary>
        /// 参与二级单位：该项目参与的其他二级单位，支持多值
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "ParticipateInUnitSecs")]
        public string? ZCY2NDORG { get; set; }
        /// <summary>
        /// 是否联合体项目：是否联合体：1是，2否
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "IsJoint")]
        public string? ZWINNINGC { get; set; }
        /// <summary>
        /// 中标交底项目编号：传入多值时用逗号给开
        /// </summary>
        [SugarColumn(ColumnDataType = "text", ColumnName = "BidDisclosureNo")]
        public string? ZAWARDP { get; set; }
        /// <summary>
        /// 创建时间：格式：YYYYMMDDHHMMSS
        /// </summary>
        [SugarColumn(Length = 64, ColumnName = "CreateDate")]
        public string? ZCREATE_AT { get; set; }
        /// <summary>
        /// 曾用名列表
        /// </summary>
        //[SugarColumn(IsIgnore = true)]
        // public ZOLDNAME_LISTItem? ZOLDNAME_LISTItem { get; set; }=new ZOLDNAME_LISTItem();
    }
    /// <summary>
    /// 曾用名列表
    /// </summary>
    public class ZOLDNAME_LISTItem
    {
        /// <summary>
        /// 
        /// </summary>
        public List<ProjectUsedNameItem>? ProjectUsedNameItems { get; set; } = new List<ProjectUsedNameItem>();
    }
    public class ProjectUsedNameItem
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
