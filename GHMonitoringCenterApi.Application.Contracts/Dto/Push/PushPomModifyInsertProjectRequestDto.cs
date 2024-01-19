using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Push
{
    /// <summary>
    /// pom信息推送dto
    /// </summary>
    public class PushPomModifyInsertProjectRequestDto
    {
        /// <summary>
        /// 推送json
        /// </summary>
        public string ModifyInsRequestJson { get; set; }
    }

    /// <summary>
    /// 基础信息
    /// </summary>
    public class ModifyInsertProjectRequestDto
    {
        /// <summary>
        /// 项目Id
        /// </summary>
        public string ProjectId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 简称
        /// </summary>
        public string ShortName { get; set; }
        /// <summary>
        /// 主数据编码
        /// </summary>
        public string MasterCode { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 项目所属公司 id
        /// </summary>
        public string CompanyId { get; set; }
        /// <summary>
        /// 项目所属项目部 id
        /// </summary>
        public string ProjectDept { get; set; }
		/// <summary>
		/// 项目所属项目部名称
		/// </summary>
		public string ProjectDeptName { get; set; }
        /// <summary>
        /// 三级公司id
        /// </summary>
        public string TCompanyId { get; set; }
        /// <summary>
        /// 三级公司名称
        /// </summary>
		public string TCompanyName { get; set; }
		/// <summary>
		/// 项目状态 id
		/// </summary>
		public string StatusId { get; set; }
        public string StatusName { get; set; }
        /// <summary>
        /// 项目类型 id
        /// </summary>
        public string TypeId { get; set; }
        /// <summary>
        /// 工况级数 id
        /// </summary>
        public string ConditionGradeId { get; set; }
        public string ConditionGradeName { get; set; }
        /// <summary>
        /// 合同约定的计量支付比例
        /// </summary>
        public decimal? ContractMeaPayProp { get; set; }
        /// <summary>
        /// 项目规模Id
        /// </summary>
        public string Scale { get; set; }
        /// <summary>
        /// 类型名称
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// 项目规模名称
        /// </summary>
        public string ScaleName { get; set; }
        /// <summary>
        /// 币种 id
        /// </summary>
        public string CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        /// <summary>
        /// 施工地点 id
        /// </summary>
        public string AreaId { get; set; }
        /// <summary>
        /// 施工区域 id
        /// </summary>
        public string RegionId { get; set; }
        public string RegionName { get; set; }
        /// <summary>
        /// 项目合同金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 有效合同额（除税）
        /// </summary>
        public decimal? ECAmount { get; set; }
        /// <summary>
        /// 项目施工资质Id
        /// </summary>
        public string ProjectConstructionQualificationId { get; set; }
        /// <summary>
        /// 项目施工资质名称
        /// </summary>
        public string ProjectConstructionQualificationName { get; set; }
        /// <summary>
        /// 项目部地址
        /// </summary>
        public string ProjectDeptAddress { get; set; }
        /// <summary>
        /// 合同工程量
        /// </summary>
        public decimal? Quantity { get; set; }
        /// <summary>
        /// 项目得分
        /// </summary>
        public decimal Score { get; set; }
        /// <summary>
        /// 对合同工程量说明
        /// </summary>
        public string QuantityRemarks { get; set; }
        /// <summary>
        /// 竣工日期
        /// </summary>
        public DateTime CompletionDate { get; set; }
        /// <summary>
        /// 项目所在地点经度
        /// </summary>
        public string Longitude { get; set; }
        /// <summary>
        /// 项目所在地点维度
        /// </summary>
        public string Latitude { get; set; }
        /// <summary>
        /// 传统、新兴工程类别标签（0：传统；1：新兴）
        /// </summary>
        public int Tag { get; set; }
        /// <summary>
        /// 现汇、投资工程类别标签（0：现汇；1：投资）
        /// </summary>
        public int Tag2 { get; set; }
        /// <summary>
        /// 项目类别 0 境内  1 境外
        /// </summary>
        public int Category { get; set; }
        /// <summary>
        /// 完工工程量
        /// </summary>
        public decimal CompleteQuantity { get; set; }
        /// <summary>
        /// 完工产值
        /// </summary>
        public decimal CompleteOutput { get; set; }
        /// <summary>
        /// 标后预算毛利率
        /// </summary>
        public decimal? BudgetInterestRate { get; set; }
        /// <summary>
        /// 计划完成编制时间
        /// </summary>
        public string CompilationTime { get; set; }
        /// <summary>
        /// 未编制标后预算原因
        /// </summary>
        public string BudgetaryReasons { get; set; }
        /// <summary>
        /// 行业分类标准
        /// </summary>
        public string ClassifyStandard { get; set; }
        /// <summary>
        /// 具有特殊社会效应
        /// </summary>
        public bool SocietySpeceffect { get; set; }
        /// <summary>
        /// 管理员人数(人)
        /// </summary>
        public int Administrator { get; set; }
        /// <summary>
        /// 施工人数(人)
        /// </summary>
        public int Constructor { get; set; }
        /// <summary>
        /// 报表负责人
        /// </summary>
        public string ReportFormer { get; set; }
        /// <summary>
        /// 报表负责人联系方式
        /// </summary>
        public string ReportFormertel { get; set; }
        /// <summary>
        /// 项目描述
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 主数据项目Id
        /// </summary>
        public string MasterProjectId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CreateBy { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Updateby { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime UpdatebyTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DeleteId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime DeleteTime { get; set; }
        /// <summary>
        /// 项目干系人列表
        /// </summary>
        public List<ProjectStakeholders> ProjectStakeholders { get; set; }
        /// <summary>
        /// 项目干系单位
        /// </summary>
        public List<ProjectStakeholderUnit> ProjectStakeholderUnit { get; set; }
    }
    /// <summary>
    /// 项目干系人
    /// </summary>
    public class ProjectStakeholders
    {
        /// <summary>
        /// 干系人类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 主数据项目id  
        /// </summary>
        public string ProjectId { get; set; }
        /// <summary>
        /// 人员id
        /// </summary>
        public string AssistantManagerId { get; set; }
        /// <summary>
        /// 离任 在任
        /// </summary>
        public string IsPresent { get; set; }
        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime? BeginDate { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
    }
    /// <summary>
    /// 项目干系单位
    /// </summary>
    public class ProjectStakeholderUnit
    {
        /// <summary>
        /// 项目id
        /// </summary>
        public string ProjectId { get; set; }
        /// <summary>
        /// 机构id
        /// </summary>
        public string OrganizationId { get; set; }
        /// <summary>
        /// 单位类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
    }
}
