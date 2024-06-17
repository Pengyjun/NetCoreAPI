using GHMonitoringCenterApi.Application.Contracts.Dto.Project;
using System.ComponentModel.DataAnnotations;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.ConstructionProjectDaily
{
    /// <summary>
    /// 船舶日报查询dto
    /// </summary>
    public class ShipDailyRequestDto:ProjectSearchRequestDto,IValidatableObject
	{
		/// <summary>
		/// 船舶Id
		/// </summary>
		public Guid? ShipPingId { get; set; }
		/// <summary>
		/// 开始日期
		/// </summary>
		public DateTime? StartTime { get; set; }

		/// <summary>
		/// 结束日期
		/// </summary>
		public DateTime? EndTime { get; set; }

		public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			//开始日期结束日期都不是空
			if (!((StartTime == DateTime.MinValue || string.IsNullOrWhiteSpace(StartTime.ToString())) && (EndTime == DateTime.MinValue || string.IsNullOrWhiteSpace(EndTime.ToString()))))
			{
				if (StartTime > EndTime)
				{
					yield return new ValidationResult("开始日期大于结束日期", new string[] { nameof(StartTime) });
				}
			}
		}
	}
}
