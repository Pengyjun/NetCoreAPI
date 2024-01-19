using System.ComponentModel.DataAnnotations;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project
{
    /// <summary>
    /// 修改推送项目月报特殊字段
    /// </summary>
    public class ModifyPushMonthReportSpecialFieldsRequestDto : IValidatableObject
    {
        /// <summary>
        /// 月报主键id
        /// </summary>
        public Guid MonthReportId { get; set; }

        /// <summary>
        /// 进度偏差原因简述推送pom
        /// </summary>
        public string? ProgressDeviationDescriptionPushPom { get; set; }

        /// <summary>
        /// 主要形象进度描述推送pom
        /// </summary>
        public string? ProgressDescriptionPushPom { get; set; }

        /// <summary>
        /// 参数校验
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(MonthReportId.ToString()) || MonthReportId == Guid.Empty)
            {
                yield return new ValidationResult("项目月报Id不能为空", new string[] { nameof(MonthReportId) });
            }
        }
    }
}
