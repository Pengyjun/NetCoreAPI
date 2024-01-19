using Newtonsoft.Json;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Timing
{

    /// <summary>
    /// 项目数据响应DTO
    /// </summary>
    public class PomProjectResponseDto
    {

        /// <summary>
        /// 项目ID
        /// </summary>
        [JsonProperty(propertyName: "Id")]
        public int? PomId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 简称
        /// </summary>
        public string? ShortName { get; set; }
        /// <summary>
        /// 得分
        /// </summary>
        public decimal? Score { get; set; }
        /// <summary>
        /// 项目主数据编码
        /// </summary>
        public string? MasterCode { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public string? Code { get; set; }
        /// <summary>
        /// 项目所属公司 id
        /// </summary>
        public Guid? Company { get; set; }
        /// <summary>
        /// 项目所属项目部 id
        /// </summary>
        public Guid? ProjectDept { get; set; }
        /// <summary>
        /// 项目规模 id
        /// </summary>
        public Guid? Scale { get; set; }
        /// <summary>
        /// 项目状态 id
        /// </summary>
        public Guid? StatusId { get; set; }
        /// <summary>
        /// 项目类型 id
        /// </summary>
        public Guid? Type { get; set; }
        /// <summary>
        /// 工况级数 id
        /// </summary>
        public string? ConditionGradeId { get; set; }
        /// <summary>
        /// 币种 id
        /// </summary>
        public Guid? Currency { get; set; }
        /// <summary>
        /// 施工地点 id
        /// </summary>
        public string? AreaId { get; set; }
        /// <summary>
        /// 施工区域 id
        /// </summary>
        public Guid? RegionId { get; set; }
        /// <summary>
        /// 项目施工资质Id
        /// </summary>
        public Guid? ProjectConstructionQualificationId { get; set; }
        /// <summary>
        /// 项目部地址
        /// </summary>
        public string? ProjectDeptAddress { get; set; }
        /// <summary>
        /// 项目合同金额
        /// </summary>
        public decimal? Amount { get; set; }
        /// <summary>
        /// 有效合同额
        /// </summary>
        public decimal? ECAmount { get; set; }
        /// <summary>
        /// 合同约定的计量支付比例(%)
        /// </summary>
        public decimal? ContractMeaPayProp { get; set; }
        /// <summary>
        /// 合同工程量
        /// </summary>
        public decimal? Quantity { get; set; }
        /// <summary>
        /// 对合同工程量说明
        /// </summary>
        public string? QuantityRemarks { get; set; }
        /// <summary>
        /// 竣工日期
        /// </summary>
        public string? CompletionDate { get; set; }
        /// <summary>
        /// 项目所在地点经度
        /// </summary>
        public decimal? Longitude { get; set; }
        /// <summary>
        /// 项目所在地点维度
        /// </summary>
        public decimal? Latitude { get; set; }
        /// <summary>
        /// 传统、新兴工程类别标签（0：传统；1：新兴）
        /// </summary>
        public int? Tag { get; set; }

        /// <summary>
        /// 现汇、投资工程类别标签（0：现汇；1：投资）
        /// </summary>
        public int? Tag2 { get; set; }

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
        //public string? CompilationTime { get; set; }
        /// <summary>
        /// 未编制标后预算原因
        /// </summary>
        public string? BudgetaryReasons { get; set; }
        /// <summary>
        /// 项目类别
        /// </summary>
        public int? Category { get; set; }
        /// <summary>
        /// 行业分类标准
        /// </summary>
        public string? ClassifyStandard { get; set; }
        /// <summary>
        /// 具有特殊社会效应
        /// </summary>
        public bool? SocietySpecEffect { get; set; }
        /// <summary>
        /// /工况级数名称
        /// </summary>
        public string? ConditionGradeName { get; set; }
        /// <summary>
        /// 管理员人数(人)
        /// </summary>
        public int? Administrator { get; set; }
        /// <summary>
        /// 施工人数(人)
        /// </summary>
        public int? Constructor { get; set; }
        /// <summary>
        /// 报表负责人
        /// </summary>
        public string? ReportFormer { get; set; }
        /// <summary>
        /// 报表负责人联系方式
        /// </summary>
        public string? ReportForMertel { get; set; }
        /// <summary>
        /// 项目描述
        /// </summary>
        public string? Remarks { get; set; }
        /// <summary>
        /// 主数据项目Id
        /// </summary>
        public Guid? MasterProjectId { get; set; }
        /// <summary>
        /// 是否是重点项目 0 否 1 是
        /// </summary>
        public int? IsMajor { get; set; }
    }
}
