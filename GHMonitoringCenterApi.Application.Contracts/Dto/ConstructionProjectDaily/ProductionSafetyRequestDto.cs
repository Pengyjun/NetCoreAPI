using GHMonitoringCenterApi.Application.Contracts.Dto.Project;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.ConstructionProjectDaily
{
	/// <summary>
	/// 产值 安监查询dto
	/// </summary>
	public class ProductionSafetyRequestDto:ProjectSearchRequestDto, IValidatableObject
	{

		/// <summary>
		/// 排序  1 倒序  2 正序
		/// </summary>
		public string[] Sort { get; set; } = new string[] {};
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
