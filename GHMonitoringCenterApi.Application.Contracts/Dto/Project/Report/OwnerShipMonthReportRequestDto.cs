using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report
{
    /// <summary>
    /// 搜索一条自有船舶月报
    /// </summary>
    public  class OwnerShipMonthReportRequestDto:IValidatableObject, IResetModelProperty
    {
        /// <summary>
        /// 项目Id
        /// </summary>
        public Guid ProjectId { get; set; }

        /// <summary>
        /// 船舶Id 
        /// </summary>
        public Guid ShipId { get; set; }

        /// <summary>
        /// 填报月份(例：202304 固定6位数)
        /// </summary>
        public int? DateMonth { get; set; }

        /// <summary>
        /// 填报月份(时间格式)
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

        /// <summary>
        /// 参数验证
        /// </summary>
        /// <returns></returns>
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
