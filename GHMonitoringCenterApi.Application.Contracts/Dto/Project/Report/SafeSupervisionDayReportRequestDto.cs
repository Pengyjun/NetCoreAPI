using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project
{
    /// <summary>
    ///  获取安监日报请求
    /// </summary>
    public class SafeSupervisionDayReportRequestDto : IValidatableObject,IResetModelProperty
    {

        /// <summary>
        /// 项目Id
        /// </summary>
        public Guid ProjectId { get; set; }

        /// <summary>
        /// 填报日期(例：20230401 固定8位数)
        /// <para>DateDay==null Or 不传 默认取 今天日期</para>
        /// </summary>
        public int? DateDay { get; set; }

        /// <summary>
        /// 如果填报日期的数据不存在，是否取前一天数据
        /// </summary>
        public bool IsTakeBeforeDayData { get; set; } = true;


        /// <summary>
        /// 填报日期(时间格式)
        /// </summary>
        public DateTime? DateDayTime { get; set; }

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
            if ( ProjectId == Guid.Empty)
            {
                yield return new ValidationResult("项目Id不能为空", new string[] { nameof(ProjectId) });
            }
        }
    }
}
