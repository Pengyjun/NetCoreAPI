using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.ProjectYearPlan
{
    public class ProjectPlanWbsRequestDto:IValidatableObject
    {
        /// <summary>
        /// 项目ID
        /// </summary>
        public Guid ProjectId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if( ProjectId==Guid.Empty)
            yield return new ValidationResult("项目ID不能为空", new string[] { nameof(ProjectId) });
        }
    }
}
