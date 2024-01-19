using GHMonitoringCenterApi.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report
{
    /// <summary>
    /// 查询一条分包船舶月报请求
    /// </summary>
    public class SubShipMonthReportRequestDto : IValidatableObject,IResetModelProperty
    {
        /// <summary>
        /// 项目id
        /// </summary>
        public Guid ProjectId { get; set; }

        /// <summary>
        /// 分包船舶Id
        /// </summary>
        public Guid ShipId { get; set; }

        /// <summary>
        /// 填报月份
        /// </summary>
        public int? DateMonth { get; set; }

        /// <summary>
        /// 填报日期(时间格式)
        /// </summary>
        public DateTime? DateMonthTime { get; set; }

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
            if (ShipId == Guid.Empty)
            {
                yield return new ValidationResult("船舶Id不能为空", new string[] { nameof(ShipId) });
            }
        }
    }
}
