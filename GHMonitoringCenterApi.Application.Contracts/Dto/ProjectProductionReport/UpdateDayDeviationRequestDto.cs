using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.ProjectProductionReport
{
    /// <summary>
    /// 日报偏差值变更
    /// </summary>
    public class UpdateDayDeviationRequestDto : IValidatableObject
    {
        /// <summary>
        /// 项目id
        /// </summary>
        public Guid ProjectId { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string? ProjectName { get; set; }
        /// <summary>
        /// 状态id
        /// </summary>
        public Guid StatusId { get; set; }
        /// <summary>
        /// 当日实际产值（元）
        /// </summary>
        public decimal DayActualProductionAmount { get; set; }
        /// <summary>
        /// 日报偏差值
        /// </summary>
        public decimal DayActualProductionAmountDeviation { get; set; }
        /// <summary>
        /// 开累产值
        /// </summary>
        public decimal TotalAmount { get; set; }
        /// <summary>
        /// 开累偏差值
        /// </summary>
        public decimal TotalAmountDeviation { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ProjectId == Guid.Empty)
            {
                yield return new ValidationResult("项目ID不能为空", new string[] { nameof(ProjectId) });
            }
            if (StatusId == Guid.Empty)
            {
                yield return new ValidationResult("项目状态ID不能为空", new string[] { nameof(StatusId) });
            }
        }
    }
}
