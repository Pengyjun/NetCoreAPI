using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project
{

    /// <summary>
    /// 项目返回DTO
    /// </summary>
    public class ProjectResponseDto
    {
        /// <summary>
        /// 是否新增或修改  true:修改 ，false：新增
        /// </summary>
        public bool IsAddOrUpdate { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        public int Sequence { get; set; }
        /// <summary>
        /// 项目ID
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>

        public string? Name { get; set; }
        /// <summary>
        /// 项目简称
        /// </summary>
        public string? ShortName { get; set; }
        /// <summary>
        /// 项目编码
        /// </summary>
        public string? Code { get; set; }
        /// <summary>
        /// 项目类别
        /// </summary>
        public int Category { get; set; }
        /// <summary>
        /// 类型ID
        /// </summary>
        public Guid? TypeId { get; set; }
        /// <summary>
        /// 类型名称
        /// </summary>
        public string? TypeName { get; set; }
        /// <summary>
        /// 合同金额
        /// </summary>
        public decimal? Amount { get; set; }
        /// <summary>
        /// 货币Id
        /// </summary>
        public Guid? CurrencyId { get; set; }
        /// <summary>
        /// 货币名称
        /// </summary>
        public string? ZCURRENCYNAME { get; set; }
        /// <summary>
        /// 所属公司ID
        /// </summary>
        public Guid? CompanyId { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string? CompanyName { get; set; }
        /// <summary>
        /// 所属项目部
        /// </summary>
        public Guid? ProjectDept { get; set; }
        /// <summary>
        /// 项目状态ID
        /// </summary>
        public Guid? StatusId { get; set; }
        /// <summary>
        /// 项目状态
        /// </summary>
        public string? Status { get; set; }

        /// <summary>
        /// 传统、新兴工程类别标签（0：传统；1：新兴）
        /// </summary>
        public int Tag { get; set; }
        /// <summary>
        /// 现汇、投资工程类别标签（0：现汇；1：投资）
        /// </summary>
        public int Tag2 { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 项目工况级数
        /// </summary>
        public string? ConditiongradeName { get; set; }
        /// <summary>
        /// 工况规模
        /// </summary>
        public string? GradeName { get; set; }
        /// <summary>
        /// 施工地点
        /// </summary>
        public string? AreaName { get; set; }
        /// <summary>
        /// 施工区域
        /// </summary>
        public string? RegionName { get; set; }
        /// <summary>
        /// 有效合同额
        /// </summary>
        public decimal? Ecamount { get; set; }
        /// <summary>
        /// 合同约定的计量支付百分比  %
        /// </summary>
        public decimal? ContractmeapayProp { get; set; }
        /// <summary>
        /// 税率
        /// </summary>
        public decimal? Rate { get; set; }
        /// <summary>
        /// 汇率
        /// </summary>
        public decimal? ExchangeRate { get; set; }
        /// <summary>
        /// 项目施工资质
        /// </summary>
        public string? ProjectConstructionQualification { get; set; }
        /// <summary>
        /// 项目部地址
        /// </summary>
        public string? ProjectDeptAddress { get; set; }
        /// <summary>
        /// 合同工程量
        /// </summary>
        public decimal? Quantity { get; set; }
        /// <summary>
        /// 合同工程量说明
        /// </summary>
        public string? QuantityRemarks { get; set; }
        /// <summary>
        /// 竣工日期
        /// </summary>
        public string? Completiondate { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public decimal? Longitude { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public decimal? Latitude { get; set; }
        /// <summary>
        /// 是否完成编制
        /// </summary>
        public string? IsStrength { get; set; }
        /// <summary>
        /// 完工工程量
        /// </summary>
        public decimal? CompleteQuantity { get; set; }
        /// <summary>
        /// 完工产值
        /// </summary>
        public decimal? CompleteOutput { get; set; }
        /// <summary>
        /// 标后预算毛利率
        /// </summary>
        public decimal? BudgetInterestRate { get; set; }
        /// <summary>
        /// 计划完成编制时间
        /// </summary>
        public string? CompilationTime { get; set; }
        /// <summary>
        /// 未编制标后预算原因
        /// </summary>
        public string? BudgetaryReasons { get; set; }
        /// <summary>
        /// 行业分类标准
        /// </summary>
        public string? ClassifyStandard { get; set; }
        /// <summary>
        /// 是否具有特殊社会效应
        /// </summary>
        public string? SocietySpeceffect { get; set; }
        /// <summary>
        /// 疏浚吹填面积(万方)
        /// </summary>
        public decimal? ReclamationArea { get; set; }
        /// <summary>
        /// 管理员人数
        /// </summary>
        public int? Administrator { get; set; }
        /// <summary>
        /// 施工人数
        /// </summary>
        public int? Constructor { get; set; }
        /// <summary>
        /// 报表负责人
        /// </summary>
        public string? Reportformer { get; set; }
        /// <summary>
        /// 报表负责人联系方式
        /// </summary>
        public string? ReportformerTel { get; set; }
        /// <summary>
        /// 项目描述
        /// </summary>
        public string? Remarks { get; set; }
        /// <summary>
		/// 开工时间
		/// </summary>
        public DateTime? CommencementTime { get; set; }
		/// <summary>
		/// 完工日期
		/// </summary>
		public DateTime? CompletionTime { get; set; }
		/// <summary>
		/// 项目合同工期(开始)
		/// </summary>
		public DateTime? StartContractDuration { get; set; }
		/// <summary>
		/// 项目合同工期(结束)
		/// </summary>
		public DateTime? EndContractDuration { get; set; }
	}
}
