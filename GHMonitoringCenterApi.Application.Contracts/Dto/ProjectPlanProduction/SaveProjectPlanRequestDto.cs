using GHMonitoringCenterApi.Application.Contracts.Dto;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.ProjectPlanProduction
{
    /// <summary>
    /// 项目产值计划请求DTO
    /// </summary>
    public class SaveProjectPlanRequestDto : IValidatableObject
    {
        /// <summary>
        /// 请求类型  true是新增  false是编辑
        /// </summary>
        public bool RequestType { get; set; } = true;
        /// <summary>
        /// 项目ID
        /// </summary>
        public Guid ProjectId { get; set; }
        /// <summary>
        /// 年度
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// 公司ID
        /// </summary>
        public Guid CompanyId { get; set; }
        public decimal? OnePlanProductionValue { get; set; }
        public decimal? TwoPlanProductionValue { get; set; }
        public decimal? ThreePlanProductionValue { get; set; }
        public decimal? FourPlanProductionValue { get; set; }
        public decimal? FivePlanProductionValue { get; set; }
        public decimal? SixPlanProductionValue { get; set; }
        public decimal? SevenPlanProductionValue { get; set; }
        public decimal? EightPlanProductionValue { get; set; }
        public decimal? NinePlanProductionValue { get; set; }
        public decimal? TenPlanProductionValue { get; set; }
        public decimal? ElevenPlanProductionValue { get; set; }
        public decimal? TwelvePlanProductionValue { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (OnePlanProductionValue == null || OnePlanProductionValue < 0)
            {
                yield return new ValidationResult("第一月计划产值不能为空或小于0", new string[] { nameof(OnePlanProductionValue) });
            }
            if (TwoPlanProductionValue == null || TwoPlanProductionValue < 0)
            {
                yield return new ValidationResult("第二月计划产值不能为空或小于0", new string[] { nameof(TwoPlanProductionValue) });
            }
            if (ThreePlanProductionValue == null || ThreePlanProductionValue < 0)
            {
                yield return new ValidationResult("第三月计划产值不能为空或小于0", new string[] { nameof(ThreePlanProductionValue) });
            }
            if (FourPlanProductionValue == null || FourPlanProductionValue < 0)
            {
                yield return new ValidationResult("第四月计划产值不能为空或小于0", new string[] { nameof(FourPlanProductionValue) });
            }
            if (FivePlanProductionValue == null || FivePlanProductionValue < 0)
            {
                yield return new ValidationResult("第五月计划产值不能为空或小于0", new string[] { nameof(FivePlanProductionValue) });
            }
            if (SixPlanProductionValue == null || SixPlanProductionValue < 0)
            {
                yield return new ValidationResult("第六月计划产值不能为空或小于0", new string[] { nameof(SixPlanProductionValue) });
            }
            if (SevenPlanProductionValue == null || SevenPlanProductionValue < 0)
            {
                yield return new ValidationResult("第七月计划产值不能为空或小于0", new string[] { nameof(SevenPlanProductionValue) });
            }
            if (EightPlanProductionValue == null || EightPlanProductionValue < 0)
            {
                yield return new ValidationResult("第八月计划产值不能为空或小于0", new string[] { nameof(EightPlanProductionValue) });
            }
            if (NinePlanProductionValue == null || NinePlanProductionValue < 0)
            {
                yield return new ValidationResult("第九月计划产值不能为空或小于0", new string[] { nameof(NinePlanProductionValue) });
            }
            if (TenPlanProductionValue == null || TenPlanProductionValue < 0)
            {
                yield return new ValidationResult("第十月计划产值不能为空或小于0", new string[] { nameof(TenPlanProductionValue) });
            }
            if (ElevenPlanProductionValue == null || ElevenPlanProductionValue < 0)
            {
                yield return new ValidationResult("第十一月计划产值不能为空或小于0", new string[] { nameof(ElevenPlanProductionValue) });
            }
            if (TwelvePlanProductionValue == null || TwelvePlanProductionValue < 0)
            {
                yield return new ValidationResult("第十二月计划产值不能为空或小于0", new string[] { nameof(TwelvePlanProductionValue) });
            }
        }
    }
}
