using GHMonitoringCenterApi.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report
{
    /// <summary>
    /// 获取船舶动态日报请求
    /// </summary>
    public class ShipDynamicDayReportRequestDto : IValidatableObject, IResetModelProperty
    {

        /// <summary>
        /// 船舶Id（PomId）
        /// </summary>
        public Guid ShipId { get; set; }

        /// <summary>
        /// 填报日期（例：20230401），不传默认返回的是昨天的数据
        /// </summary>
        public int? DateDay { get; set; }

        /// <summary>
        /// 如果填报的日期是没数据，是否取前一天的数据，默认是true
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
            if (ShipId == Guid.Empty)
            {
                yield return new ValidationResult("船舶Id不能为空", new string[] { nameof(ShipId) });
            }

        }

    }
}
