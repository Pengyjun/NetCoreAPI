using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Job
{

    /// <summary>
    /// 任务审批记录请求
    /// </summary>
    public class JobRecordsRequestDto : IValidatableObject
    {
        /// <summary>
        /// 任务Id
        /// </summary>
        public Guid JobId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (JobId == Guid.Empty)
            {
                yield return new ValidationResult("任务Id不能为空", new string[] { nameof(JobId) });
            }
        }
    }
}
