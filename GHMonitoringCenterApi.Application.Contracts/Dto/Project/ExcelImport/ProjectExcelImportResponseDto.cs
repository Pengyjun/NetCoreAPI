using GHMonitoringCenterApi.CustomAttribute;
using MiniExcelLibs.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using AutoMapper.Configuration.Annotations;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project.ExcelImport
{
    /// <summary>
    /// 项目列表导出类
    /// </summary>
    public class ProjectExcelImportResponseDto
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        [ExcelColumn(Name = "项目名", Width = 50)]
        public string? Name { get; set; }
        /// <summary>
        /// 项目类别
        /// </summary>
        [ExcelColumn(Ignore = true, Width = 30)]
        public int Category { get; set; }
        /// <summary>
        /// 项目类别
        /// </summary>
        [ExcelColumn(Name = "项目类别", Width = 30)]
        public string? CategoryName { get; set; }
        /// <summary>
        /// 类型名称
        /// </summary>
        [ExcelColumn(Name = "项目类型", Width = 30)]
        public string? TypeName { get; set; }
        /// <summary>
        /// 合同金额
        /// </summary>
        [ExcelColumn(Name = "合同总价", Width = 30)]
        public decimal? Amount { get; set; }

        /// <summary>
        /// 货币名称
        /// </summary>
        [ExcelColumn(Name = "合同币种", Width = 30)]
        
        public string? ZCURRENCYNAME { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        [ExcelColumn(Name = "所属公司", Width = 30)]
        public string? CompanyName { get; set; }
        /// <summary>
        /// 项目状态
        /// </summary>
        [ExcelColumn(Name = "项目状态", Width = 30)]
        public string? Status { get; set; }

        /// <summary>
        /// 项目工况级数
        /// </summary>
        [ExcelColumn(Name = "项目工况级数", Width = 30)]
        [IgnoreField]
        public string? ConditiongradeName { get; set; }
        /// <summary>
        /// 工况规模
        /// </summary>
        [ExcelColumn(Name = "工况规模", Width = 30)]
        [IgnoreField]
        public string? GradeName { get; set; }
        /// <summary>
        /// 施工地点
        /// </summary>
       
        [ExcelColumn(Name = "施工地点", Width = 30)]
        [IgnoreField]
        public string? AreaName { get; set; }
        /// <summary>
        /// 施工区域
        /// </summary>
        [ExcelColumn(Name = "施工区域", Width = 30)]
        [IgnoreField]
        public string? RegionName { get; set; }
        /// <summary>
        /// 有效合同额
        /// </summary>
        [ExcelColumn(Name = "有效合同额", Width = 30)]
        [IgnoreField]
        public decimal? Ecamount { get; set; }
        /// <summary>
        /// 合同约定的计量支付百分比  %
        /// </summary>
        [ExcelColumn(Name = "合同约定的计量支付百分比(%)", Width = 30)]
        [IgnoreField]
        public decimal? ContractmeapayProp { get; set; }
        /// <summary>
        /// 税率
        /// </summary>
        [ExcelColumn(Name = "税率", Width = 30)]
        [IgnoreField]
        public decimal? Rate { get; set; }
        /// <summary>
        /// 项目施工资质
        /// </summary>
        [ExcelColumn(Name = "项目施工资质", Width = 30)]
        [IgnoreField]
        public string? ProjectConstructionQualification { get; set; }
        /// <summary>
        /// 项目部地址
        /// </summary>
        [ExcelColumn(Name = "项目部地址", Width = 30)]
        [IgnoreField]
        public string? ProjectDeptAddress { get; set; }
        /// <summary>
        /// 合同工程量
        /// </summary>
        [ExcelColumn(Name = "合同工程量", Width = 30)]
        [IgnoreField]
        public decimal? Quantity { get; set; }
        /// <summary>
        /// 合同工程量说明
        /// </summary>
        [ExcelColumn(Name = "合同工程量说明", Width = 30)]
        [IgnoreField]
        public string? QuantityRemarks { get; set; }
        /// <summary>
        /// 竣工日期
        /// </summary>
        [ExcelColumn(Name = "竣工日期", Width = 30)]
        [IgnoreField]
        public string? Completiondate { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        [ExcelColumn(Name = "经度", Width = 30)]
        [IgnoreField]
        public decimal? Longitude { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        [ExcelColumn(Name = "纬度", Width = 30)]
        [IgnoreField]
        public decimal? Latitude { get; set; }
        /// <summary>
        /// 是否完成编制
        /// </summary>
        [ExcelColumn(Name = "是否完成编制", Width = 30)]
        [IgnoreField]
        public string? IsStrength { get; set; }
        /// <summary>
        /// 完工工程量
        /// </summary>
        [ExcelColumn(Name = "完工工程量", Width = 30)]
        [IgnoreField]
        public decimal? CompleteQuantity { get; set; }
        /// <summary>
        /// 完工产值
        /// </summary>
        [ExcelColumn(Name = "完工产值", Width = 30)]
        [IgnoreField]
        public decimal? CompleteOutput { get; set; }
        /// <summary>
        /// 标后预算毛利率
        /// </summary>
        [ExcelColumn(Name = "标后预算毛利率", Width = 30)]
        [IgnoreField]
        public decimal? BudgetInterestRate { get; set; }
        /// <summary>
        /// 计划完成编制时间
        /// </summary>
        [ExcelColumn(Name = "计划完成编制时间", Width = 30)]
        [IgnoreField]
        public string? CompilationTime { get; set; }
        /// <summary>
        /// 未编制标后预算原因
        /// </summary>
        [ExcelColumn(Name = "未编制标后预算原因", Width = 50)]
        [IgnoreField]
        public string? BudgetaryReasons { get; set; }
        /// <summary>
        /// 行业分类标准
        /// </summary>
        [ExcelColumn(Name = "行业分类标准", Width = 30)]
        [IgnoreField]
        public string? ClassifyStandard { get; set; }
        /// <summary>
        /// 是否具有特殊社会效应
        /// </summary>
        [ExcelColumn(Name = "是否具有特殊社会效应", Width = 30)]
        [IgnoreField]
        public string? SocietySpeceffect { get; set; }
        /// <summary>
        /// 疏浚吹填面积(万方)
        /// </summary>
        [ExcelColumn(Name = "疏浚吹填面积", Width = 30)]
        [IgnoreField]
        public decimal? ReclamationArea { get; set; }
        /// <summary>
        /// 管理员人数
        /// </summary>
        [ExcelColumn(Name = "管理员人数", Width = 30)]
        [IgnoreField]
        public int? Administrator { get; set; }
        /// <summary>
        /// 施工人数
        /// </summary>
        [ExcelColumn(Name = "施工人数", Width = 30)]
        [IgnoreField]
        public int? Constructor { get; set; }
        /// <summary>
        /// 项目描述
        /// </summary>
        [ExcelColumn(Name = "项目描述", Width = 30)]
        [IgnoreField]
        public string? Remarks { get; set; }
        /// <summary>
        /// 报表负责人
        /// </summary>
        [ExcelColumn(Name = "报表负责人", Width = 30)]
        [IgnoreField]
        public string? Reportformer { get; set; }
        /// <summary>
        /// 报表负责人联系方式
        /// </summary>
        [ExcelColumn(Name = "报表负责人联系方式", Width = 30)]
        [IgnoreField]
        public string? ReportformerTel { get; set; }
        /// <summary>
        /// 是否全量导出
        /// </summary>
        //[ExcelColumn(Name = "是否全量导出", Width = 30)]
        //[IgnoreField]
        //public bool? IsFullExport { get; set; }
    }
}
