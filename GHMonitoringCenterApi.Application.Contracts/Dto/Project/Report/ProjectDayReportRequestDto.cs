using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using SqlSugar.Extensions;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project
{
    /// <summary>
    /// 获取日报请求
    /// <para>DayReportId与ProjectId+DateDay都可以查询指定日期的一条日报</para>
    /// <para>优先级DayReportId>ProjectId+DateDay</para>
    /// </summary>
    public class ProjectDayReportRequestDto : IValidatableObject, IResetModelProperty
    {
        /// <summary>
        /// 项目Id
        /// </summary>
        public Guid ProjectId { get; set; }

        /// <summary>
        /// 填报日期(例：20230401 固定8位数)
        /// <para>DateDay==null Or 不传 默认取 昨日日期</para>
        /// </summary>
        public int? DateDay { get; set; }

        /// <summary>
        ///如果没有项目日报，是否取前一天的日报数据
        /// </summary>
        public bool IsTakeBeforeDayData { get; set; } = true;

        /// <summary>
        /// 填报日期(时间格式)
        /// </summary>
        public DateTime? DateDayTime { get; set; }

        //public int Type { get; set; } = 1;

        /// <summary>
        /// 重置属性
        /// </summary>
        public void ResetModelProperty()
        {
            if (DateDay == null && DateDayTime != null)
            {
                DateDay = DateDayTime.Value.ToDateDay();
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
