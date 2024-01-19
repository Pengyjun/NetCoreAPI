using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project
{
    /// <summary>
    /// 获取月报请求
    /// </summary>
    public class ProjectMonthReportRequestDto : IValidatableObject, IResetModelProperty
    {
        /// <summary>
        /// 项目Id
        /// </summary>
        public Guid ProjectId { get; set; }

        /// <summary>
        /// 填报月份(例：202304 固定6位数)
        /// </summary>
        public int? DateMonth { get; set; }

        /// <summary>
        /// 填报月份（时间格式）
        /// </summary>
        public DateTime? DateMonthTime { get; set; }

        /// <summary>
        /// 任务Id
        /// </summary>
        public Guid? JobId { get; set; }

        /// <summary>
        /// 重置属性
        /// </summary>
        public void ResetModelProperty()
        {
            if (DateMonth == null && DateMonthTime != null)
            {
                DateMonth = DateMonthTime.Value.ToDateMonth();
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ProjectId == Guid.Empty)
            {
                yield return new ValidationResult("项目Id不能为空", new string[] { nameof(ProjectId) });
            }
        }

    }
}
