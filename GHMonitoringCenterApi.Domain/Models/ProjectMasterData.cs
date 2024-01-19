using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 项目主数据
    /// </summary>
    [SugarTable("t_projectmasterdata", IsDisabledDelete = true)]
    public class ProjectMasterData : BaseEntity<Guid>
    {
        /// <summary>
        /// 项目主数据编码
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? ProjectMasterCode { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? ProjectName { get; set; }
        /// <summary>
        /// 项目简称
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? ShortName { get; set; }
        /// <summary>
        /// 项目外文名称
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? Foreign { get; set; }
        /// <summary>
        /// 上级项目主数据编码
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? SuperiorMasterCode { get; set; }
        /// <summary>
        /// 项目曾用名
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? BeforeName { get; set; }
        /// <summary>
        /// 项目类型
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? ProjectType { get; set; }
        /// <summary>
        /// 中交项目业务分类
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? BusinessClassification { get; set; }
        /// <summary>
        /// 项目所在国家/地区
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? ProjectCountry { get; set; }
        /// <summary>
        /// 项目所在地(城市)
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? ProjectCity { get; set; }
        /// <summary>
        /// 所属事业部
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? AffiliationDept { get; set; }
        /// <summary>
        /// 收入来源
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? RevenueSource { get; set; }
        /// <summary>
        /// 投资主体
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? InvestBody { get; set; }
        /// <summary>
        /// 项目批复/决议文号
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? ApprovalNo { get; set; }
        /// <summary>
        /// 决议时间
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? ResolutionDate { get; set; }
        /// <summary>
        /// 项目机构
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? ProjectOrgCode { get; set; }
        /// <summary>
        /// 计税方式
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? TaxMethod { get; set; }
        /// <summary>
        /// 项目年份
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? ProjectYear { get; set; }
        /// <summary>
        /// 项目计划开始日期
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? PlanStartDate { get; set; }
        /// <summary>
        /// 项目计划结束日期
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? PlanEndDate { get; set; }
        /// <summary>
        /// 项目组织形式
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? ProjectOrgForm { get; set; }
        /// <summary>
        /// 中标主体
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? WinningBidder { get; set; }
        /// <summary>
        /// 项目工程名称
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? ProjectConstructionName { get; set; }
        /// <summary>
        /// 责任主体
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? Responsible { get; set; }
        /// <summary>
        /// 土地成交确认书编号/收并购协议编号
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? LandNo { get; set; }
        /// <summary>
        /// 项目获取时间
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? ProjectAcquireDate { get; set; }
        /// <summary>
        /// 工商变更时间
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? CommerceChangeDate { get; set; }
        /// <summary>
        /// 操盘情况
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? TradingSituation { get; set; }
        /// <summary>
        /// 并表情况
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? MergeTableSituation { get; set; }
        /// <summary>
        /// 保险机构名称
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? InsureOrgName { get; set; }
        /// <summary>
        /// 保单号
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? PolicyNo { get; set; }
        /// <summary>
        /// 投保人
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? InsurePeople { get; set; }
        /// <summary>
        /// 保险起始日期
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? InsuranceStartDate { get; set; }
        /// <summary>
        /// 保险终止日期
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? InsuranceEndDate { get; set; }
        /// <summary>
        /// 基金主数据编码
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? FundMasterCode { get; set; }
        /// <summary>
        /// 基金名称
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? FundName { get; set; }
        /// <summary>
        /// 基金编号
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? FundNo { get; set; }
        /// <summary>
        /// 币种
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? Currency { get; set; }
        /// <summary>
        /// 基金组织形式
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? FundOrgForm { get; set; }
        /// <summary>
        /// 基金管理人类型
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? FundManageType { get; set; }
        /// <summary>
        /// 基金成立日期
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? FundStateDate { get; set; }
        /// <summary>
        /// 基金到期日期
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? FundEndDate { get; set; }
        /// <summary>
        /// 托管人名称
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? CustodianName { get; set; }
        /// <summary>
        /// 承租人名称
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? TenantName { get; set; }
        /// <summary>
        /// 承租人类型
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? TenantTyoe { get; set; }
        /// <summary>
        /// 租赁物名称
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? Leasesname { get; set; }
        /// <summary>
        /// 起租日期
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? LeaseStartDate { get; set; }
        /// <summary>
        /// 到期日期
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? LeaseEndDate { get; set; }
        /// <summary>
        /// 所属二级单位
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? SecondaryUnit { get; set; }
        /// <summary>
        /// 项目机构
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? Organization { get; set; }
        /// <summary>
        /// 状态,有效：1 无效：0
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? State { get; set; } = "1";
        /// <summary>
        /// 停用原因
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? StopReason { get; set; }
        /// <summary>
        /// 项目管理方式
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? ProjectManageStyle { get; set; }
        /// <summary>
        /// 参与二级单位，该项目参与的其他二级单位，支持多值
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? ParticipateUnit { get; set; }
        /// <summary>
        /// 项目GUID
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid Projectid { get; set; }
    }
}
