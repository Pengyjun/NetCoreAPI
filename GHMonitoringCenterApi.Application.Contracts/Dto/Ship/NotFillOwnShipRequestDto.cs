
using GHMonitoringCenterApi.Application.Contracts.Dto.Project;
using System.ComponentModel.DataAnnotations;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Ship
{
    /// <summary>
    /// 未报自有船舶月报列表请求dto
    /// </summary>
    public class NotFillOwnShipRequestDto : ProjectSearchRequestDto, IValidatableObject
    {
        /// <summary>
        /// 传入开始日期
        /// </summary>
        public DateTime InStartDate { get; set; }
        /// <summary>
        /// 传入结束日期
        /// </summary>
        public DateTime InEndDate { get; set; }
        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //开始日期结束日期都不是空
            if (!(InStartDate == DateTime.MinValue || string.IsNullOrWhiteSpace(InStartDate.ToString())) && (InEndDate == DateTime.MinValue || string.IsNullOrWhiteSpace(InEndDate.ToString())))
            {
                if (InStartDate > InEndDate)
                {
                    yield return new ValidationResult("开始日期大于结束日期", new string[] { nameof(InStartDate) });
                }
            }
        }
    }
}
