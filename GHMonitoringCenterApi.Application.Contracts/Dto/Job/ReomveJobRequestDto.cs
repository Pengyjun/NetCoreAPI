using GHMonitoringCenterApi.Application.Contracts.Dto.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Job
{
    /// <summary>
    /// 移除任务请求
    /// </summary>
    public class ReomveJobRequestDto : IValidatableObject
    {
        /// <summary>
        /// 任务Id
        /// </summary>
        public Guid JobId { get; set; }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if ( JobId == Guid.Empty)
            {
                yield return new ValidationResult("任务Id不能为空", new string[] { nameof(JobId) });
            }
        }
    }
}
