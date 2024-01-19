using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.ProjectYearPlan
{


    /// <summary>
    /// 项目年初计划请求DTO
    /// </summary>
    public class ProjectYearPlanRequestDto:BaseRequestDto
    {
        /// <summary>
        /// 项目状态
        /// </summary>
        public Guid?  StatusId { get; set; }

        /// <summary>
        /// 年份筛选 默认2024年
        /// </summary>
        public int? Year { get; set; } = 2024;
        /// <summary>
        /// 公司id
        /// </summary>
        public Guid? CompanyId { get; set; }
        /// <summary>
        ///项目部
        /// </summary>
        public Guid? ProjectDept { get; set; }
    }

    public class InsertProjectYearPlanRequestDto : IValidatableObject 
    {

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 项目ID
        /// </summary>
        public Guid ProjectId { get; set; }
        /// <summary>
        /// 合同额
        /// </summary>
        public decimal ContractAmount { get; set; }

        /// <summary>
        /// 一月份工程量
        /// </summary>
        public decimal? OneQuantity { get; set; }
        /// <summary>
        /// 一月份产值
        /// </summary>
        public decimal? OneProductionValue { get; set; }
        /// <summary>
        /// 二月份工程量
        /// </summary>
        public decimal? TwoQuantity { get; set; }
        /// <summary>
        /// 二月份产值
        /// </summary>
        public decimal? TwoProductionValue { get; set; }


        public decimal? ThreeQuantity { get; set; }
        public decimal? ThreeProductionValue { get; set; }
        public decimal? FourQuantity { get; set; }
        public decimal? FourProductionValue { get; set; }
        public decimal? FiveQuantity { get; set; }
        public decimal? FiveProductionValue { get; set; }
        public decimal? SixQuantity { get; set; }
        public decimal? SixProductionValue { get; set; }
        public decimal? SevenQuantity { get; set; }
        public decimal? SevenProductionValue { get; set; }
        public decimal? EightQuantity { get; set; }
        public decimal? EightProductionValue { get; set; }
        public decimal? NineQuantity { get; set; }
        public decimal? NineProductionValue { get; set; }
        public decimal? TenQuantity { get; set; }
        public decimal? TenProductionValue { get; set; }
        public decimal? ElevenQuantity { get; set; }
        public decimal? ElevenProductionValue { get; set; }
        public decimal? TwelveQuantity { get; set; }
        public decimal? TwelveProductionValue { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(ProjectName))
            {
                yield return new ValidationResult("项目名称不能为空！", new string[] { nameof(ProjectName) });
            }
            if (ContractAmount<=0)
            {
                yield return new ValidationResult("合同额必须大于0！", new string[] { nameof(ContractAmount) });
            }
        }
     }


    public class InsertProjectPlanProductionRequestDto : IValidatableObject
    {

        /// <summary>
        /// 项目ID
        /// </summary>
        public Guid ProjectId { get; set; }

        /// <summary>
        /// 一月份工程量
        /// </summary>
        public decimal? OneQuantity { get; set; }
        /// <summary>
        /// 一月份产值
        /// </summary>
        public decimal? OneProductionValue { get; set; }
        /// <summary>
        /// 二月份工程量
        /// </summary>
        public decimal? TwoQuantity { get; set; }
        /// <summary>
        /// 二月份产值
        /// </summary>
        public decimal? TwoProductionValue { get; set; }


        public decimal? ThreeQuantity { get; set; }
        public decimal? ThreeProductionValue { get; set; }
        public decimal? FourQuantity { get; set; }
        public decimal? FourProductionValue { get; set; }
        public decimal? FiveQuantity { get; set; }
        public decimal? FiveProductionValue { get; set; }
        public decimal? SixQuantity { get; set; }
        public decimal? SixProductionValue { get; set; }
        public decimal? SevenQuantity { get; set; }
        public decimal? SevenProductionValue { get; set; }
        public decimal? EightQuantity { get; set; }
        public decimal? EightProductionValue { get; set; }
        public decimal? NineQuantity { get; set; }
        public decimal? NineProductionValue { get; set; }
        public decimal? TenQuantity { get; set; }
        public decimal? TenProductionValue { get; set; }
        public decimal? ElevenQuantity { get; set; }
        public decimal? ElevenProductionValue { get; set; }
        public decimal? TwelveQuantity { get; set; }
        public decimal? TwelveProductionValue { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ProjectId!=Guid.Empty)
            {
                yield return new ValidationResult("项目ID不能为空！", new string[] { nameof(ProjectId) });
            }
        }
    }
}
