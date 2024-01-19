using GHMonitoringCenterApi.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project
{
    /// <summary>
    /// 获取船舶日报请求
    /// <para><code>DateDayTime</code>与<code>DateDay</code>二选一即可确定填报日期，DateDay优先级>DateDayTime</para>
    /// </summary>
    public class ShipDayReportRequestDto : IValidatableObject, IResetModelProperty
    {
        /// <summary>
        /// 项目Id
        /// </summary>
        public Guid? ProjectId { get; set; }

        /// <summary>
        /// 船舶Id 
        /// </summary>
        public Guid ShipId { get; set; }

        /// <summary>
        /// 填报日期(例：20230401 固定8位数)
        /// <para>DateDay==null Or 不传 默认取 昨天日期</para>
        /// </summary>
        public int? DateDay { get; set; }

        /// <summary>
        /// 如果船舶日报,是否取前一天的数据，默认是true
        /// </summary>
        public bool IsTakeBeforeDayData { get; set; } = true;

        /// <summary>
        /// 填报日期(时间格式)
        /// </summary>
        public DateTime? DateDayTime { get; set; }

        /// <summary>
        /// 船舶日报类型(1:项目-船舶日报,2:动态船舶日报)
        /// </summary>
        public ShipDayReportType ShipDayReportType { get; set; }

        /// <summary>
        /// 重置属性
        /// </summary>
        public void ResetModelProperty()
        {
            if (DateDay == null && DateDayTime != null)
            {
                DateDay = DateDayTime.Value.ToDateDay();
            }
            if (ShipDayReportType == ShipDayReportType.DynamicShip)
            {
                ProjectId = Guid.Empty;
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ShipId == Guid.Empty)
            {
                yield return new ValidationResult("船舶Id不能为空", new string[] { nameof(ShipId) });
            }
            if (ShipDayReportType == ShipDayReportType.ProjectShip)
            {
                if (ProjectId == null || ProjectId == Guid.Empty)
                {
                    yield return new ValidationResult("项目Id不能为空", new string[] { nameof(ProjectId) });
                }
            }
        }
    }
}
